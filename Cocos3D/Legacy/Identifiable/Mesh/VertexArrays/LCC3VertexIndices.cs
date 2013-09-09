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
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3D
{
    public class LCC3VertexIndices : LCC3DrawableVertexArray
    {
        IndexBuffer _xnaIndexBuffer;
        uint _lastDrawnStartingIndex;
        uint _lastDrawnIndicesCount;

        #region Properties

        // Static properties

        public new static LCC3Semantic DefaultSemantic 
        { 
            get { return LCC3Semantic.SemanticNone; }
        }

        // Instance properties

        public override string NameSuffix
        {
            get { return "Indices"; }
        }

        public override LCC3BufferTarget BufferTarget
        {
            get { return LCC3BufferTarget.ElementArrayBuffer; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3VertexIndices(int tag, string name) : base(tag, name)
        {
            this.ElementType = LCC3ElementType.UnsignedInt;
            this.ElementSize = 1;
        }

        public void PopulateFromRunLengthArray(ushort[] runLenArray)
        {
            uint elemNum, rlaIdx, runNum, rlaLen;
            rlaLen = (uint)runLenArray.Count();
            runNum = 0;
            elemNum = 0;
            rlaIdx = 0;

            // First determine how much space needs to be allocated

            while(rlaIdx < rlaLen)
            {
                ushort runLength = runLenArray[rlaIdx];
                elemNum += runLength;
                rlaIdx += (uint)runLength + 1;
                runNum++;
            }

            this.AllocatedVertexCapacity = elemNum;
            this.AllocateStripLengths(runNum);

            // Now load vertex data

            runNum = 0;
            elemNum = 0;
            rlaIdx = 0;

            while(rlaIdx < rlaLen) 
            {
                ushort runLength = runLenArray[rlaIdx++];
                _stripLengths[runNum++] = runLength;
                for (int i = 0; i < runLength; i++) 
                {
                    this.SetIndex(runLenArray[rlaIdx++], elemNum++);
                }
            }
        }

        #endregion Allocation and initialization


        #region Configuring Indices

        public void SetIndex(uint vtxIndex, uint index)
        {
            _vertices[(int)index] = vtxIndex;
        }

        public uint IndexAt(uint index)
        {
            return (uint)_vertices[(int)index];
        }

        #endregion Configuring Indices


        #region Binding artifacts

        protected override void BindContentToAttributeAtIndexWithVisitor(object[] vertexData, LCC3VertexAttrIndex vaIdx, LCC3NodeDrawingVisitor visitor)
        {
            // Vertex indices are not part of vertex content
        }

        #endregion Binding artifacts


        #region Drawing

        public override void DrawFromIndexWithVisitor(uint vertexIndex, uint vertexCount, LCC3NodeDrawingVisitor visitor)
        {
            base.DrawFromIndexWithVisitor(vertexIndex, vertexCount, visitor);

            switch (this.ElementType)
            {
                case LCC3ElementType.UnsignedInt:
                    this.PrimitiveDrawFromIndexWithVisitor<uint>(vertexIndex, vertexCount, visitor);
                    break;

            }
        }

        private void PrimitiveDrawFromIndexWithVisitor<T>(uint vertexIndex, uint vertexCount, LCC3NodeDrawingVisitor visitor) where T : struct, IComparable
        {
            this.LoadXnaIndexBuffer<T>(vertexIndex, vertexCount);
            visitor.ProgramPipeline.DrawIndices<T>(vertexCount, vertexIndex, this.ElementType, this.DrawingMode, 0);
        }

        private void LoadXnaIndexBuffer<T>(uint vertexIndex, uint vertexCount) where T : struct, IComparable
        {
            LCC3ProgPipeline progPipeline = LCC3ProgPipeline.SharedPipeline();

            if (_xnaIndexBuffer == null || _lastDrawnIndicesCount != vertexCount || _lastDrawnStartingIndex != vertexIndex)
            {
                T[] indices = Array.ConvertAll(_vertices, item => (T)item);
                ArraySegment<T> arraySegment = new ArraySegment<T>(indices, (int)vertexIndex, (int)vertexCount);
                T[] subIndices = arraySegment.Array;

                Type xnaType = this.ElementType.CSharpType();

                _xnaIndexBuffer = new IndexBuffer(progPipeline.XnaGraphicsDevice, xnaType, subIndices.Count(), BufferUsage.WriteOnly);

                _xnaIndexBuffer.SetData(subIndices);

                _lastDrawnIndicesCount = vertexCount;
                _lastDrawnStartingIndex = vertexIndex;
            }

            progPipeline.XnaIndexBuffer = _xnaIndexBuffer;
        }

        #endregion Drawing
    }
}

