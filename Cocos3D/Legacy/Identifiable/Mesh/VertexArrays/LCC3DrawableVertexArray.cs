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
using System.Diagnostics;

namespace Cocos3D
{
    public class LCC3DrawableVertexArray : LCC3VertexArray
    {
        // Instance fields

        private LCC3DrawMode _drawingMode;
        protected uint[] _stripLengths;


        #region Properties

        public LCC3DrawMode DrawingMode
        {
            get { return _drawingMode; }
        }

        public uint StripCount
        {
            get { return _stripLengths != null ? (uint)(_stripLengths.Length) : 0; }
        }

        public uint[] StripLengths
        {
            get { return _stripLengths; }
        }

        public uint FaceCount
        {
            get 
            { 
                if (this.StripCount > 0) 
                {
                    uint faceCount = 0;
                    foreach (uint stripLen in _stripLengths)
                    {
                        faceCount += this.FaceCountFromVertexIndexCount(stripLen);
                    }
                    return faceCount;
                } 
                else 
                {
                    return this.FaceCountFromVertexIndexCount(this.VertexCount);
                }
    
            }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3DrawableVertexArray(int tag, string name) : base(tag, name)
        {
            _drawingMode = LCC3DrawMode.TriangleList;
        }

        public void PopulateFrom(LCC3DrawableVertexArray anotherArray)
        {
            base.PopulateFrom(anotherArray);

            _drawingMode = anotherArray.DrawingMode;

            this.AllocateStripLengths(anotherArray.StripCount);

            anotherArray.StripLengths.CopyTo(_stripLengths, 0);
        }

        protected void AllocateStripLengths(uint sCount)
        {
            Array.Resize(ref _stripLengths, (int)sCount);
        }

        protected void DeallocateStripLengths()
        {
            _stripLengths = null;
        }
       
        #endregion Allocation and initialization


        #region Drawing

        public void DrawWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            if (this.StripCount > 0)
            {
                uint startOfStrip = 0;
                foreach (uint stripLen in _stripLengths)
                {
                    this.DrawFromIndexWithVisitor(startOfStrip, stripLen, visitor);
                    startOfStrip += stripLen;
                }
            }
            else
            {
                this.DrawFromIndexWithVisitor(0, this.VertexCount, visitor);
            }
        }

        protected virtual void DrawFromIndexWithVisitor(uint vertexIndex, uint vertexCount, LCC3NodeDrawingVisitor visitor)
        {
            // Subclasses to implement
        }

        public static uint FaceCountFromVertexIndexCount(uint vertexCount, LCC3DrawMode drawingMode)
        {
            switch (drawingMode)
            {
                case LCC3DrawMode.TriangleList:
                    return vertexCount / 3;
                    case LCC3DrawMode.TriangleStrip:
                    return vertexCount - 2;
                    case LCC3DrawMode.LineList:
                    return vertexCount / 2;
                    case LCC3DrawMode.LineStrip:
                    return vertexCount - 1;
                    default:
                    Debug.Assert(false, String.Format("Encountered unknown drawing mode {0}", drawingMode));

                    return 0;
            }
        }

        protected uint FaceCountFromVertexIndexCount(uint vertexCount)
        {
            return LCC3DrawableVertexArray.FaceCountFromVertexIndexCount(vertexCount, _drawingMode);
        }

        protected uint VertexIndexCountFromFaceCount(uint faceCount)
        {
            switch (_drawingMode)
            {
                case LCC3DrawMode.TriangleList:
                    return faceCount * 3;
                    case LCC3DrawMode.TriangleStrip:
                    return faceCount + 2;
                    case LCC3DrawMode.LineList:
                    return faceCount * 2;
                    case LCC3DrawMode.LineStrip:
                    return faceCount + 1;
                    default:
                    Debug.Assert(false, String.Format("Encountered unknown drawing mode {0}", this.DrawingMode));

                    return 0;
            }
        }

        protected LCC3FaceIndices FaceIndicesAtIndex(uint faceIndex)
        {
            return new LCC3FaceIndices(0, 0, 0);
        }

        protected LCC3FaceIndices FaceIndicesAtIndex(uint faceIndex, uint stripLen)
        {
            return new LCC3FaceIndices(0, 0, 0);
        }

        #endregion Drawing
    }
}

