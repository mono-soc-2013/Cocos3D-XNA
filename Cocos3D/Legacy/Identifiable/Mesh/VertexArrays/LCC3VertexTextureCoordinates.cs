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
    public class LCC3VertexTextureCoordinates : LCC3VertexArray
    {
        // Static vars

        private static bool _defaultExpectsVerticallyFlippedTextures;

        // ivars

        CCSize _mapSize;
        CCRect _textureRectangle;
        bool _expectsVerticallyFlippedTextures;


        #region Properties

        public static bool DefaultExpectsVerticallyFlippedTextures
        {
            get { return _defaultExpectsVerticallyFlippedTextures; }
            set { _defaultExpectsVerticallyFlippedTextures = value; }
        }

        public override string NameSuffix
        {
            get { return "TexCoords"; }
        }

        public override LCC3Semantic DefaultSemantic
        {
            get { return LCC3Semantic.SemanticVertexTexture; }
        }

        public CCRect TextureRectangle
        {
            get { return _textureRectangle; }
            set
            {
                CCRect oldRect = _textureRectangle;
                _textureRectangle = value;
                this.AlignWithTextureRectangle(value, oldRect);
            }
        }

        protected CCSize MapSize
        {
            get { return _mapSize; }
        }

        public bool ExpectsVerticallyFlippedTextures
        {
            get { return _expectsVerticallyFlippedTextures; }
            set { _expectsVerticallyFlippedTextures = value; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3VertexTextureCoordinates(int tag, string name) : base(tag, name)
        {
        }

        public void PopulateFrom(LCC3VertexTextureCoordinates another)
        {
            base.PopulateFrom(another);

            _mapSize = another.MapSize;
            _textureRectangle = another.TextureRectangle;
            _expectsVerticallyFlippedTextures = another.ExpectsVerticallyFlippedTextures;
        }

        #endregion Allocation and initialization
       

        #region Configuring tex coords

        public CCTex2F TexCoord2FAt(uint index)
        {
            return (CCTex2F)_vertices[(int)index];
        }

        public void SetTexCoord2FAtIndex(CCTex2F tex2F, uint index)
        {
            _vertices[(int)index] = tex2F;
        }

        #endregion Configuring tex coords


        #region Configuring texture rectangle

        public void AlignWithTextureRectangle(CCRect newRect, CCRect oldRect)
        {
            float mw = _mapSize.Width;
            float mh = _mapSize.Height;

            float ox = oldRect.Origin.X;
            float oy = oldRect.Origin.Y;
            float ow = oldRect.Size.Width;
            float oh = oldRect.Size.Height;

            float nx = newRect.Origin.X;
            float ny = newRect.Origin.Y;
            float nw = newRect.Size.Width;
            float nh = newRect.Size.Height;

            for (uint i = 0; i < this.VertexCount; i++) 
            {
                CCTex2F ptc = this.TexCoord2FAt(i);

                float origU = ((ptc.U / mw) - ox) / ow;         
                ptc.U = (nx + (origU * nw)) * mw;            

                if (_expectsVerticallyFlippedTextures) 
                {
                    float origV = (1.0f - (ptc.V / mh) - oy) / oh;   
                    ptc.V = (1.0f - (ny + (origV * nh))) * mh;        
                } 
                else 
                {
                    float origV = ((ptc.V / mh) - oy) / oh;         
                    ptc.V = (ny + (origV * nh)) * mh;               
                }

                this.SetTexCoord2FAtIndex(ptc, i);
            }
        }

        #endregion Configuring texture recatangle
    }
}

