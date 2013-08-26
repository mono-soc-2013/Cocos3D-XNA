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
    public class LCC3VertexMatrixIndices : LCC3VertexArray
    {
        #region Properties

        public override string NameSuffix
        {
            get { return "MatrixIndices"; }
        }

        public override LCC3Semantic DefaultSemantic
        {
            get { return LCC3Semantic.SemanticVertexMatrixIndices; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3VertexMatrixIndices(int tag, string name) : base(tag, name)
        {
            this.ElementType = LCC3ElementType.UnsignedByte;
            this.ElementSize = 0;
        }

        #endregion Allocation and initialization


        #region Setting/getting matrix indices

        public uint[] MatrixIndicesAtIndex(uint index)
        {
            return (uint[])_vertices[(int)index];
        }

        public uint MatrixIndexForVertexUnitAtIndex(uint vertexUnit, uint index)
        {
            return this.MatrixIndicesAtIndex(index)[(int)vertexUnit];
        }

        public void SetMatrixIndicesAtIndex(uint[] matrixIndices, uint index)
        {
            _vertices[(int)index] = matrixIndices;
        }

        #endregion Setting/getting weights
    }
}

