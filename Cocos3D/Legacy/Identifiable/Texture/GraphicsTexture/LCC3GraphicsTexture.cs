//
// Copyright 2013 Rami Tabbara
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
//
// Please see README.md to locate the external API documentation.
//
using System;
using System.Collections.Generic;
using Cocos2D;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3D
{
    public class LCC3GraphicsTexture : LCC3Identifiable
    {
        // Static vars

        static int _lastAssignedGraphicsTextureTag = 0;
        static LCC3TextureParams _defaultTextureParameters = new LCC3TextureParams(
            LCC3TextureFilter.LinearMipPoint, LCC3TextureFilter.Linear, 
            LCC3TextureWrapMode.Wrap, LCC3TextureWrapMode.Wrap);
        static Dictionary<string, LCC3GraphicsTexture> _texturesByName = new Dictionary<string, LCC3GraphicsTexture>();

        // ivars

        protected Texture _xnaTexture;

        LCC3IntSize _size;
        bool _hasPremultipliedAlpha;
        bool _isFlippedVertically;
        bool _hasMipmap;
        CCSize _coverage;

        bool _texParametersAreDirty;
        LCC3TextureFilter _minifyingFunction;
        LCC3TextureFilter _magnifyingFunction;
        LCC3TextureWrapMode _horizontalWrapMode;
        LCC3TextureWrapMode _verticalWrapMode;
       

        #region Properties

        public bool HasPremultipliedAlpha
        {
            get { return _hasPremultipliedAlpha; }
            set { _hasPremultipliedAlpha = value; }
        }

        public bool IsFlippedVertically
        {
            get { return _isFlippedVertically; }
            set { _isFlippedVertically = value; }
        }

        public bool HasMipmap
        {
            get { return _hasMipmap; }
            set { _hasMipmap = value; }
        }

        public virtual bool IsTexture2D
        {
            get { return false; }
        }

        public virtual bool IsTextureCube
        {
            get { return false; }
        }

        public Texture2D XnaTexture2D
        {
            get { return (this.IsTexture2D == true) ? (Texture2D)_xnaTexture : null; }
        }

        public CCSize Coverage
        {
            get { return _coverage; }
            set { _coverage = value; }
        }

        public bool IsPOTWidth
        {
            get { return (_size.Width == CCUtils.CCNextPOT(_size.Width)); }
        }

        public bool IsPOTHeight
        {
            get { return (_size.Height == CCUtils.CCNextPOT(_size.Height)); }
        }

        public bool IsPOT
        {
            get { return this.IsPOTWidth && this.IsPOTHeight; }
        }

        public virtual LCC3GraphicsTextureTarget TextureTarget
        {
            get { return LCC3GraphicsTextureTarget.NoTarget; }
        }

        // Texture parameters

        public virtual LCC3TextureParams DefaultTextureParameters
        {
            get { return _defaultTextureParameters; }
            set { _defaultTextureParameters = value; }
        }

        public LCC3TextureFilter MinifyingFunction
        {
            get { return _minifyingFunction; }
            set 
            { 
                _minifyingFunction = value; 
                this.MarkTextureParametersDirty(); 
            }
        }

        public LCC3TextureFilter MagnifyingFunction
        {
            get { return _magnifyingFunction; }
            set { _magnifyingFunction = value; this.MarkTextureParametersDirty(); }
        }

        public LCC3TextureWrapMode HorizontalWrappingFunction
        {
            get { return this.IsPOTWidth == true ? _horizontalWrapMode : LCC3TextureWrapMode.Clamp; }
            set 
            { 
                _horizontalWrapMode = value; 
                this.MarkTextureParametersDirty(); 
            }
        }

        public LCC3TextureWrapMode VerticalWrappingFunction
        {
            get { return this.IsPOTHeight == true ? _verticalWrapMode : LCC3TextureWrapMode.Clamp; }
            set 
            { 
                _verticalWrapMode = value; 
                this.MarkTextureParametersDirty(); 
            }
        }

        public LCC3TextureParams TextureParameters
        {
            get { return new LCC3TextureParams(_minifyingFunction, _magnifyingFunction, _horizontalWrapMode, _verticalWrapMode); }
            set 
            {
                _minifyingFunction = value.MinifyingFilter;
                _magnifyingFunction = value.MagnifyingFilter;
                _horizontalWrapMode = value.HorizontalWrapMode;
                _verticalWrapMode = value.VerticalWrapMode;
                this.MarkTextureParametersDirty();
            }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3GraphicsTexture(string fileName) : base()
        {
            // Subclasses should load appropiate texture type
            _xnaTexture = null;
        }

        public LCC3GraphicsTexture(int tag, string name) : base(tag, name)
        {
            _size = new LCC3IntSize(0, 0);
            _coverage = CCSize.Zero;
            _hasMipmap = false;
            _isFlippedVertically = true;
            _hasPremultipliedAlpha = false;
            this.TextureParameters = this.DefaultTextureParameters;
        }   

        public virtual void PopulateFrom(LCC3GraphicsTexture anotherTexture)
        {
            // Subclasses to implement
        }

        private void MarkTextureParametersDirty()
        {
            _texParametersAreDirty = true;
        }

        #endregion Allocation and initialization


        #region Tag allocation

        public new static void ResetTagAllocation()
        {
            _lastAssignedGraphicsTextureTag = 0;
        }

        protected override int NextTag()
        {
            return ++_lastAssignedGraphicsTextureTag;
        }

        #endregion Tag allocation


        #region Texture cache

        public static void AddGraphicsTexture(LCC3GraphicsTexture texture)
        {
            if (texture != null)
            {
                _texturesByName[texture.Name] = texture;
            }
        }

        public static LCC3GraphicsTexture GetGraphicsTextureNamed(string textureName)
        {
            return _texturesByName[textureName];
        }

        public void RemoveGraphicsTexture(LCC3GraphicsTexture texture)
        {
            this.RemoveGraphicsTextureNamed(texture.Name);
        }

        public void RemoveGraphicsTextureNamed(string textureName)
        {
            _texturesByName.Remove(textureName);
        }

        #endregion Texture cache


        #region Drawing

        protected virtual void BindTextureContentToTarget(LCC3GraphicsTextureTarget target)
        {
            // Subclasses to implement
        }

        public void BindWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            LCC3ProgPipeline progPipeline = visitor.ProgramPipeline;
            uint texUnitIndex = visitor.CurrentTextureUnitIndex;
            LCC3GraphicsTextureTarget texTarget = this.TextureTarget;

            progPipeline.BindTextureToTargetAtIndex(this, texTarget, texUnitIndex);
        }

        public void BindTextureParametersWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            LCC3ProgPipeline progPipeline = visitor.ProgramPipeline;
            uint tuIndex = visitor.CurrentTextureUnitIndex;

            progPipeline.SetTextureMinifyFuncAtIndex(this.MinifyingFunction, tuIndex);
            progPipeline.SetTextureMagnifyFuncAtIndex(this.MagnifyingFunction, tuIndex);
            progPipeline.SetTextureHorizWrapFuncAtIndex(this.HorizontalWrappingFunction, tuIndex);
            progPipeline.SetTextureVertWrapFuncAtIndex(this.VerticalWrappingFunction, tuIndex);

            _texParametersAreDirty = false;
        }

        #endregion Drawing
    }
}

