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
    public class LCC3VertexColors : LCC3VertexArray
    {
        private readonly static CCColor3B _CCColor3BBlack = new CCColor3B(0,0,0);

        #region Properties

        // Static properties

        public new static LCC3Semantic DefaultSemantic 
        { 
            get { return LCC3Semantic.SemanticVertexColor; }
        }

        // Instance properties

        public override string NameSuffix
        {
            get { return "Colors"; }
        }

        public override LCC3ElementType ElementType
        {
            set
            {
                _elementType = value;
                this.ShouldNormalizeContent = (_elementType != LCC3ElementType.Float);
            }
        }

        // CCRGBAProtocol properties

        public CCColor3B Color
        {
            get
            {
                if (this.VertexCount == 0)
                {
                    return _CCColor3BBlack;
                }

                return this.Color4BAtIndex(0).CCColor3B();
            }

            set
            {
                for(uint i = 0; i < this.VertexCount; i++)
                {
                    CCColor4B vtxColor = this.Color4BAtIndex(i);
                    this.SetColor4BAtIndex(new CCColor4B(value.R, value.G, value.B, vtxColor.A), i);
                }

                this.UpdateGraphicsBuffer();
            }
        }

        public byte Opacity
        {
            get { return (this.VertexCount > 0) ? this.Color4BAtIndex(0).A : (byte)0; }
            set
            {
                for(uint i = 0; i < this.VertexCount; i++)
                {
                    CCColor4B vtxColor = this.Color4BAtIndex(i);
                    this.SetColor4BAtIndex(new CCColor4B(vtxColor.R, vtxColor.G, vtxColor.B, value), i);
                }

                this.UpdateGraphicsBuffer();
            }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3VertexColors(int tag, string name) : base(tag, name)
        {
            this.ElementType = LCC3ElementType.UnsignedByte;
        }

        #endregion Allocation and initialization


        #region Configuring colors

        public CCColor4F Color4FAtIndex(uint index)
        {
            object vertexData = _vertices[(int)index];

            switch (_elementType)
            {
                case LCC3ElementType.Fixed:
                case LCC3ElementType.UnsignedByte:
                    return LCC3ColorUtil.CCC4FFromCCC4B((CCColor4B)vertexData);
                default:
                    return (CCColor4F)vertexData;
            }
        }

        public void SetColor4FAtIndex(CCColor4F color, uint index)
        {
            switch (_elementType)
            {
                case LCC3ElementType.Fixed:
                case LCC3ElementType.UnsignedByte:
                    _vertices[(int)index] = LCC3ColorUtil.CCC4BFromCCC4F(color);
                    break;
                default:
                    _vertices[(int)index] = color;
                    break;
            }
        }

        public CCColor4B Color4BAtIndex(uint index)
        {
            object vertexData = _vertices[(int)index];

            switch (_elementType)
            {
                case LCC3ElementType.Float:
                    return LCC3ColorUtil.CCC4BFromCCC4F((CCColor4F)vertexData);
                default:
                    return (CCColor4B)vertexData;
            }
        }

        public void SetColor4BAtIndex(CCColor4B color, uint index)
        {
            switch (_elementType)
            {
                case LCC3ElementType.Float:
                    _vertices[(int)index] = LCC3ColorUtil.CCC4FFromCCC4B(color);
                    break;
                default:
                    _vertices[(int)index] = color;
                    break;
            }
        }

        #endregion Configuring colors

    }
}

