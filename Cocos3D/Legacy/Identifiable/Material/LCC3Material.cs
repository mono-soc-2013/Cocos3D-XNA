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
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Cocos2D;

namespace Cocos3D
{
    public class LCC3Material : LCC3Identifiable, ICCBlendProtocol, ICCRGBAProtocol
    {
        // Static fields

        private static int _lastAssignedMaterialTag = 0;
        private static int _currentMaterialTag = 0;
        private static readonly CCColor4F _CC3DefaultMaterialColorAmbient 
            = new CCColor4F(0.2f, 0.2f, 0.2f, 1.0f);
        private static readonly CCColor4F _CC3DefaultMaterialColorDiffuse 
            = new CCColor4F(0.8f, 0.8f, 0.8f, 1.0f);
        private static readonly CCColor4F _CC3DefaultMaterialColorSpecular 
            = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
        private static readonly CCColor4F _CC3DefaultMaterialColorEmission 
            = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
        private const float _CC3DefaultMaterialShininess = 0.0f;
        private const float _CC3MaximumMaterialShininess = 128.0f;
        private const float _CC3DefaultMaterialReflectivity = 128.0f;
  
        // Instance fields

        private LCC3Texture _texture;
        private List<LCC3Texture> _textureOverlays;
        private LCC3ShaderProgramContext _shaderContext;
        private CCColor4F _ambientColor;
        private CCColor4F _diffuseColor;
        private CCColor4F _specularColor;
        private CCColor4F _emissionColor;
        private float _shininess;
        private float _reflectivity;
        private LCC3AlphaTestFuncMode _alphaTestFunc;
        private LCC3BlendType _srcBlendType;
        private LCC3BlendType _dstBlendType;
        private bool _shouldUseLighting;

        #region Properties

        // Static properties

        public static CCColor4F CC3DefaultMaterialColorAmbient
        {
            get { return _CC3DefaultMaterialColorAmbient; }
        }

        public static CCColor4F CC3DefaultMaterialColorDiffuse
        {
            get { return _CC3DefaultMaterialColorDiffuse; }
        }

        public static CCColor4F CC3DefaultMaterialColorSpecular
        {
            get { return _CC3DefaultMaterialColorSpecular; }
        }

        public static CCColor4F CC3DefaultMaterialColorEmission
        {
            get { return _CC3DefaultMaterialColorEmission; }
        }

        public static float CC3DefaultMaterialShininess
        {
            get { return _CC3DefaultMaterialShininess; }
        }

        public static float CC3DefaultMaterialReflectivity
        {
            get { return _CC3DefaultMaterialReflectivity; }
        }

        // Instance properties

        public CCColor4F AmbientColor
        {
            get { return _ambientColor; }
        }

        public CCColor4F DiffuseColor
        {
            get { return _diffuseColor; }
        }

        public CCColor4F SpecularColor
        {
            get { return _specularColor; }
        }

        public CCColor4F EmissionColor
        {
            get { return _specularColor; }
        }

        public LCC3Texture Texture
        {
            get { return _texture; }
            set { _texture = value; this.TexturesHaveChanged(); }
        }

        public float Shininess
        {
            get { return _shininess; }
            set { _shininess = MathHelper.Clamp(value, 0.0f, LCC3Material.CC3DefaultMaterialShininess); }
        }

        public float Reflectivity
        {
            get { return _reflectivity; }
            set { _reflectivity = MathHelper.Clamp(value, 0.0f, 1.0f); }
        }

        public LCC3BlendType SourceBlend
        {
            get { return _srcBlendType; }
            set { _srcBlendType = value; }
        }

        public LCC3BlendType DestinationBlend
        {
            get { return _dstBlendType; }
            set { _dstBlendType = value; }
        }

        public bool IsOpaque
        {
            get { return (_srcBlendType == LCC3BlendType.One) && (_dstBlendType == LCC3BlendType.One); }
            set
            {
                if (value == true)
                {
                    _srcBlendType = LCC3BlendType.One;
                    _dstBlendType = LCC3BlendType.Zero;
                }
                else
                {
                    if ((_srcBlendType == LCC3BlendType.One) && this.HasPremultipliedAlpha == false)
                    {
                        _srcBlendType = LCC3BlendType.SourceAlpha;
                    }

                    if (_dstBlendType == LCC3BlendType.Zero)
                    {
                        _dstBlendType = LCC3BlendType.InverseSourceAlpha;
                    }
                }
            }
        }

        public bool ShouldDrawLowAlpha
        {
            get 
            { 
                switch (_alphaTestFunc)
                {
                    case LCC3AlphaTestFuncMode.Always:
                    case LCC3AlphaTestFuncMode.Less:
                    case LCC3AlphaTestFuncMode.LessOrEqual:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool ShouldApplyOpacityToColor
        {
            get { return (_srcBlendType == LCC3BlendType.One) && this.HasPremultipliedAlpha; }
        }

        public CCColor4F EffectiveAmbientColor
        {
            get { return this.ShouldApplyOpacityToColor ? LCC3ColorUtil.CCC4FBlendAlpha(_ambientColor) : _ambientColor; }
        }

        public CCColor4F EffectiveDiffuseColor
        {
            get { return this.ShouldApplyOpacityToColor ? LCC3ColorUtil.CCC4FBlendAlpha(_diffuseColor) : _diffuseColor; }
        }

        public CCColor4F EffectiveSpecularColor
        {
            get { return this.ShouldApplyOpacityToColor ? LCC3ColorUtil.CCC4FBlendAlpha(_specularColor) : _specularColor; }
        }

        public CCColor4F EffectiveEmissionColor
        {
            get { return this.ShouldApplyOpacityToColor ? LCC3ColorUtil.CCC4FBlendAlpha(_emissionColor) : _emissionColor; }
        }

        public LCC3ShaderProgram ShaderProgram
        {
            get { return _shaderContext.Program; }
            set
            {
                if (value == this.ShaderProgram)
                {
                    return;
                }

                if (value == null)
                {
                    _shaderContext = null;
                    return;
                }

                if (_shaderContext != null)
                {
                    _shaderContext.Program = value;
                    return;
                }

                _shaderContext = new LCC3ShaderProgramContext(value);
            }
        }

        public LCC3ShaderProgramContext ShaderContext
        {
            get { return _shaderContext; }
        }

        protected List<LCC3Texture> TextureOverlays
        {
            get { return _textureOverlays; }
        }

        // Texture properties

        public bool HasPremultipliedAlpha
        {
            get 
            { 
                if (_texture != null && _texture.HasPremultipliedAlpha == true)
                {
                    return true;
                }

                foreach (LCC3Texture texture in _textureOverlays)
                {
                    if (texture.HasPremultipliedAlpha == true)
                        return true;
                }

                return false;
            }
        }

        public bool HasBumpMap
        {
            get 
            { 
                if (_texture != null && _texture.IsBumpMap == true)
                {
                    return true;
                }

                foreach (LCC3Texture texture in _textureOverlays)
                {
                    if (texture.IsBumpMap == true)
                        return true;
                }

                return false;
            }
        }

        public LCC3Vector LightDirection
        {
            get 
            { 
                if (_texture != null && _texture.IsBumpMap == true)
                {
                    return _texture.LightDirection;
                }

                foreach (LCC3Texture texture in _textureOverlays)
                {
                    if (texture.IsBumpMap == true)
                        return _texture.LightDirection;
                }

                return LCC3Vector.CC3VectorZero;
            }

            set
            {
                _texture.LightDirection = value;

                foreach (LCC3Texture texture in _textureOverlays)
                {
                    texture.LightDirection = value;
                }
            }
        }

        #endregion Properties


        #region CCRGBAProtocol & CCBlendProtocol support

        public CCColor3B Color
        {
            get { return LCC3ColorUtil.CCC3BFromCCC4F(_diffuseColor); }
            set 
            {
                float rf = LCC3ColorUtil.CCColorByteFromFloat(value.R);
                float gf = LCC3ColorUtil.CCColorByteFromFloat(value.G);
                float bf = LCC3ColorUtil.CCColorByteFromFloat(value.B);

                _ambientColor = new CCColor4F(rf, gf, bf, _ambientColor.A);
                _diffuseColor = new CCColor4F(rf, gf, bf, _diffuseColor.A);
            }
        }

        public CCColor3B DisplayedColor
        {
            get { return this.Color; }
        }

        public byte Opacity
        {
            get { return LCC3ColorUtil.CCColorByteFromFloat(_diffuseColor.A); }
            set 
            {
                float af = LCC3ColorUtil.CCColorByteFromFloat(value);
                _ambientColor.A = af;
                _diffuseColor.A = af;
                _specularColor.A = af;
                _emissionColor.A = af;

                if (value < Byte.MaxValue) 
                {
                    this.IsOpaque = false;
                }
            }
        }

        public byte DisplayedOpacity
        {
            get { return this.Opacity; }
        }

        public bool IsOpacityModifyRGB 
        {
            get { return false; }
            set {}
        }

        public bool CascadeColorEnabled
        {
            get { return false; }
            set {}
        }

        public bool CascadeOpacityEnabled 
        {
            get { return false; }
            set {}
        }

        public void UpdateDisplayedColor(CCColor3B color)
        {

        }

        public void UpdateDisplayedOpacity(byte opacity)
        {

        }

        public CCBlendFunc BlendFunc
        {
            get { return new CCBlendFunc(); }
            set { }
        }
        
        #endregion CCRGBAProtocol & CCBlendProtocol support


        #region Allocation and initialization

        public LCC3Material(int tag, string name) : base(tag, name)
        {
            _textureOverlays = new List<LCC3Texture>();
            _ambientColor = LCC3Material.CC3DefaultMaterialColorAmbient;
            _diffuseColor = LCC3Material.CC3DefaultMaterialColorDiffuse;
            _specularColor = LCC3Material.CC3DefaultMaterialColorSpecular;
            _emissionColor = LCC3Material.CC3DefaultMaterialColorEmission;
            _shininess = LCC3Material.CC3DefaultMaterialShininess;
            _reflectivity = LCC3Material.CC3DefaultMaterialReflectivity;
            _srcBlendType = LCC3BlendType.One;
            _dstBlendType = LCC3BlendType.Zero;
            _alphaTestFunc = LCC3AlphaTestFuncMode.Always;
            _shouldUseLighting = true;

        }

        public void PopulateFrom(LCC3Material anotherMaterial)
        {
            base.PopulateFrom(anotherMaterial);

            _ambientColor = anotherMaterial.AmbientColor;
            _diffuseColor = anotherMaterial.DiffuseColor;
            _specularColor = anotherMaterial.SpecularColor;
            _emissionColor = anotherMaterial.EmissionColor;
            _shininess = anotherMaterial.Shininess;
            _reflectivity = anotherMaterial.Reflectivity;
            _srcBlendType = anotherMaterial._srcBlendType;
            _dstBlendType = anotherMaterial._dstBlendType;
            _alphaTestFunc = anotherMaterial._alphaTestFunc;
            _shouldUseLighting = anotherMaterial._shouldUseLighting;

            _shaderContext = anotherMaterial.ShaderContext;
            _texture = anotherMaterial.Texture;

            _textureOverlays.Clear();

            _textureOverlays.AddRange(anotherMaterial.TextureOverlays);

        }

        #endregion Allocation and initialization


        #region Tag allocation

        public new static void ResetTagAllocation()
        {
            _lastAssignedMaterialTag = 0;
        }

        protected override int NextTag()
        {
            return ++_lastAssignedMaterialTag;
        }

        #endregion Tag allocation


        #region Textures

        public uint TextureCount()
        {
            return (uint)_textureOverlays.Count + ((_texture != null) ? 1U: 0U);
        }

        public void AddTexture(LCC3Texture texture)
        {
            if (_texture == null)
            {
                _texture = texture;
            }
            else
            {
                uint maxTexUnits = LCC3ProgPipeline.MaxNumberOfTextureUnits;

                Debug.Assert((this.TextureCount() < maxTexUnits),
                             String.Format("Attempt to add texture ignored because platform supports only {0} texture units.", maxTexUnits));

                _textureOverlays.Add(_texture);
            }

            this.TexturesHaveChanged();
        }

        public void RemoveTexture(LCC3Texture texture)
        {
            if (_texture == texture)
            {
                _texture = null;
            }
            else
            {
                _textureOverlays.Remove(texture);
                this.TexturesHaveChanged();
            }
        }

        public void RemoveAllTextures()
        {
            this.RemoveTexture(_texture);

            _textureOverlays.Clear();
            this.TexturesHaveChanged();
        }

        public LCC3Texture TextureForTextureUnit(uint texUnit)
        {
            return (texUnit == 0) ? _texture : _textureOverlays[(int)texUnit - 1];
        }

        public LCC3Texture GetTextureNamed(string texName)
        {

            if (_texture != null)
            {
                if (texName ==  _texture.Name)
                {
                    return _texture;
                }
            }

            return _textureOverlays.FirstOrDefault(texture => texture.Name == texName);
        }

        public void TexturesHaveChanged()
        {

            this.IsOpaque = this.IsOpaque;
        }

        #endregion Textures


        #region Drawing

        public void DrawWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            if (this.SwitchingMaterial())
            {
                this.ApplyAlphaTestWithVisitor(visitor);
                this.ApplyBlendWithVisitor(visitor);
                this.ApplyColorsWithVisitor(visitor);
                this.DrawTexturesWithVisitor(visitor);
            }
        }

        public void ApplyAlphaTestWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            // Using progammable pipeline so don't do anything here
        }

        public void ApplyBlendWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            LCC3ProgPipeline progPipeline = visitor.ProgramPipeline;
            bool shouldBlend = !this.IsOpaque;

            progPipeline.EnableBlend(shouldBlend);
            if(shouldBlend == true)
            {
                visitor.ProgramPipeline.SetBlendFuncSrcAndDst(_srcBlendType, _dstBlendType);
            }
        }

        public void ApplyColorsWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            visitor.CurrentColor = this.EffectiveDiffuseColor;
        }

        public void DrawTexturesWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            visitor.CurrentTextureUnitIndex = 0U;

            _texture.DrawWithVisitor(visitor);

            foreach (LCC3Texture texture in _textureOverlays)
            {
                texture.DrawWithVisitor(visitor);
            }

            visitor.DisableUnusedTextureUnits();
        }

        public void UnbindWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            LCC3Material.ResetSwitching();
        }

        #endregion Drawing


        #region Material context switching

        public static void ResetSwitching()
        {
            LCC3Material._currentMaterialTag = 0;
        }

        public bool SwitchingMaterial()
        {
            bool shouldSwitch = LCC3Material._currentMaterialTag != this.Tag;
            LCC3Material._currentMaterialTag = this.Tag;

            return shouldSwitch;
        }

        #endregion Material context switching
    }
}

