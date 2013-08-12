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
    public class LCC3VertexTangents : LCC3VertexArray
    {
        #region Properties

        public override string NameSuffix
        {
            get { return "Tangents"; }
        }

        public override LCC3Semantic DefaultSemantic
        {
            get { return LCC3Semantic.SemanticVertexTangent; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3VertexTangents(int tag, string name) : base(tag, name)
        {
        }

        #endregion Allocation and initialization


        #region Setting/getting tangents

        public LCC3Vector TangentAtIndex(uint index)
        {
            return (LCC3Vector)_vertices[(int)index];
        }

        public void SetTangentAtIndex(LCC3Vector aTangent, uint index)
        {
            _vertices[(int)index] = aTangent;
        }

        #endregion Setting/getting tangents
    }
}

