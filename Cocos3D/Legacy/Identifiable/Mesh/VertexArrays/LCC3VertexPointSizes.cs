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

namespace Cocos3D
{
    public class LCC3VertexPointSizes : LCC3VertexArray
    {
        #region Properties

        // Static properties

        public new static LCC3Semantic DefaultSemantic 
        { 
            get { return LCC3Semantic.SemanticVertexPointSize; }
        }

        // Instance properties

        public override string NameSuffix
        {
            get { return "PointSizes"; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3VertexPointSizes(int tag, string name) : base(tag, name)
        {
            this.ElementType = LCC3ElementType.Float;
            this.ElementSize = 1;
        }

        #endregion Allocation and initialization


        #region Configuring point size

        public float PointSizeAt(uint index)
        {
            return (float)_vertices[(int)index];
        }

        public void SetPointSizeAtIndex(float size, uint index)
        {
            _vertices[(int)index] = size;
        }

        #endregion Configuring point size
    }
}

