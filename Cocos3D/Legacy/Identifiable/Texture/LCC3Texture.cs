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
using Cocos2D;

namespace Cocos3D
{
    public class LCC3Texture : LCC3Identifiable
    {
        // Static fields

        private static int _lastAssignedTextureTag = 0;

        // Instance fields

        LCC3GraphicsTexture _graphicsTexture;
        LCC3TextureUnit _textureUnit;

        #region Properties

        public LCC3GraphicsTexture GraphicsTexture
        {
            get { return _graphicsTexture; }
            set 
            { 
                _graphicsTexture = value; 
                if (this.Name == null)
                {
                    this.Name = _graphicsTexture.Name;
                }
            }
        }

        public LCC3TextureUnit TextureUnit
        {
            get { return _textureUnit; }
            set { _textureUnit = value; }
        }

        public bool HasPremultipliedAlpha
        {
            get { return (_graphicsTexture != null) && (_graphicsTexture.HasPremultipliedAlpha); }
        }

        public bool IsFlippedVertically
        {
            get { return (_graphicsTexture != null) && (_graphicsTexture.IsFlippedVertically); }
        }

        public bool IsTexture2D
        {
            get { return (_graphicsTexture != null) && (_graphicsTexture.IsTexture2D); }
        }

        public bool IsTextureCube
        {
            get { return (_graphicsTexture != null) && (_graphicsTexture.IsTextureCube); }
        }

        public CCSize Coverage
        {
            get { return (_graphicsTexture != null ? _graphicsTexture.Coverage : CCSize.Zero); }
        }
            
        public LCC3Vector LightDirection
        {
            get { return (_textureUnit != null ? _textureUnit.LightDirection : LCC3Vector.CC3VectorZero); }
            set { _textureUnit.LightDirection = value; }
        }

        public bool IsBumpMap
        {
            get { return (_textureUnit != null) && _textureUnit.IsBumpMap; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3Texture(string name, string fileName) : base(name)
        {
            this.LoadTextureFile(fileName);
        }

        public LCC3Texture(int tag, string name) : base(tag, name)
        {
        }

        public void PopulateFrom(LCC3Texture texture)
        {
            base.PopulateFrom(texture);

            _graphicsTexture = texture.GraphicsTexture;
            _textureUnit = texture.TextureUnit;
        }

        #endregion Allocation and initialization


        #region Tag allocation

        public new static void ResetTagAllocation()
        {
            _lastAssignedTextureTag = 0;
        }

        protected override int NextTag()
        {
            return ++_lastAssignedTextureTag;
        }

        #endregion Tag allocation


        #region Texture file loading

        private bool LoadTextureFile(string fileName)
        {
            _graphicsTexture = new LCC3GraphicsTexture(fileName);

            return (_graphicsTexture != null);
        }

        #endregion Texture file loading


        #region Drawing

        public void DrawWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            if (_graphicsTexture != null)
            {
                this.BindGraphicsTextureWithVisitor(visitor);
                visitor.CurrentTextureUnitIndex += 1;
            }
        }

        public void BindGraphicsTextureWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            _graphicsTexture.BindWithVisitor(visitor);
            this.BindTextureEnvironmentWithVisitor(visitor);
        }

        public void BindTextureEnvironmentWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            if (_textureUnit != null)
            {
                _textureUnit.BindWithVisitor(visitor);
            }
            else
            {
                LCC3TextureUnit.BindDefaultWithVisitor(visitor);
            }
        }

        #endregion Drawing

    }
}

