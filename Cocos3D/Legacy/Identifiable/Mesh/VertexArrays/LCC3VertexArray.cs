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
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3D
{
    public class LCC3VertexArray : LCC3Identifiable
    {
        // Static fields

        private static int _lastAssignedVertexArrayTag = 0;

        // Instance fields

        private uint _bufferID;
        protected object[] _vertices;
        private uint _vertexCount;
        private uint _elementOffset;
        private int _elementSize;
        private uint _vertexStride;
        protected LCC3ElementType _elementType;
        private uint _allocatedVertexCapacity;
        private LCC3Semantic _semantic;

        private bool _shouldNormalizeContent;
        private bool _shouldAllowVertexBuffering;
        private bool _shouldReleaseRedundantContent;
        private bool _isUsingGraphicsBuffer;


        #region Properties

        // Instance properties

        public override string NameSuffix
        {
            get { return String.Empty; }
        }

        public uint BufferID
        {
            get { return _bufferID; }
        }

        public virtual LCC3BufferTarget BufferTarget
        {
            get { return LCC3BufferTarget.ArrayBuffer; }
        }

        public object[] Vertices
        {
            get { return _vertices; }
            set
            {
                uint currVtxCount = _vertexCount;
                this.AllocatedVertexCapacity = 0;
                _vertices = value;

                if (_vertices != null)
                {
                    _vertexCount = currVtxCount;
                }

                this.VerticesWereChanged();
            }

        }

        public virtual uint VertexCount
        {
            get { return _vertexCount; }
            set { _vertexCount = value; }
        }

        public int ElementSize
        {
            get { return _elementSize; }
            set
            {
                int currSize = _elementSize;
                _elementSize = value;

                if (this.AllocateVertexCapacity(_allocatedVertexCapacity) == false)
                {
                    _elementSize = currSize;
                }
            }
        }

        public uint ElementOffset
        {
            get { return _elementOffset; }
            set { _elementOffset = value; }
        }

        public virtual LCC3ElementType ElementType
        {
            get { return 0; }
            set
            {
                LCC3ElementType currType = _elementType;
                _elementType = value;

                if (this.AllocateVertexCapacity(_allocatedVertexCapacity) == false)
                {
                    _elementType = currType;
                }
            }
        }

        public uint ElementLength
        {
            get { return (uint)(_elementType.Size() * _elementSize); }
        }

        public uint VertexStride
        {
            get { return (_vertexStride != 0) ? _vertexStride : this.ElementLength; }
        }

        public uint AvailableVertexCount
        {
            get { return (_allocatedVertexCapacity > 0) ? _allocatedVertexCapacity : _vertexCount; }
        }

        public uint AllocatedVertexCapacity
        {
            get { return _allocatedVertexCapacity; }
            set { this.AllocateVertexCapacity(value); }
        }

        public LCC3Semantic Semantic
        {
            get { return _semantic; }
            set { _semantic = value; }
        }

        public virtual LCC3Semantic DefaultSemantic
        {
            get { return LCC3Semantic.SemanticNone; }
        }

        public bool ShouldNormalizeContent
        {
            get { return _shouldNormalizeContent; }
            set { _shouldNormalizeContent = value; }
        }

        public bool ShouldAllowVertexBuffering
        {
            get { return _shouldAllowVertexBuffering; }
            set { _shouldAllowVertexBuffering = value; }
        }

        public bool ShouldReleaseRedundantContent
        {
            get { return _shouldReleaseRedundantContent; }
        }

        public bool IsUsingGraphicsBuffer
        {
            get { return _isUsingGraphicsBuffer; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3VertexArray(int tag, string name) : base(tag, name)
        {
            _elementType = LCC3ElementType.Float;
            _elementSize = 3;
            _shouldAllowVertexBuffering = true;
            _shouldReleaseRedundantContent = true;
            _semantic = this.DefaultSemantic;
        }

        public void PopulateFrom(LCC3VertexArray anotherArray)
        {
            base.PopulateFrom(anotherArray);

            _semantic = anotherArray.Semantic;
            _elementType = anotherArray.ElementType;
            _elementSize = anotherArray.ElementSize;
            _vertexStride = anotherArray.VertexStride;
            _elementOffset = anotherArray.ElementOffset;
            _shouldNormalizeContent = anotherArray.ShouldNormalizeContent;
            _shouldAllowVertexBuffering = anotherArray.ShouldAllowVertexBuffering;
            _shouldReleaseRedundantContent = anotherArray.ShouldReleaseRedundantContent;

            this.DeleteGraphicsBuffer();

            if (anotherArray.AllocatedVertexCapacity > 0)
            {
                this.AllocatedVertexCapacity = anotherArray.AllocatedVertexCapacity;
                _vertices = (object[])anotherArray.Vertices.Clone();
            }
            else
            {
                _vertices = anotherArray.Vertices;
            }

            _vertexCount = anotherArray.VertexCount;
        }

        public bool AllocateVertexCapacity(uint vtxCount)
        {
            if (_allocatedVertexCapacity == vtxCount)
            {
                return true;
            }

            if (_allocatedVertexCapacity == 0) 
            {
                _vertices = null;
            }

            if (vtxCount > 0)
            {
                Array.Resize<object>(ref _vertices, (int)vtxCount);
            }
            else
            {
                _vertices = null;
            }

            _allocatedVertexCapacity = vtxCount;
            _vertexCount = vtxCount;

            this.VerticesWereChanged();

            return true;
        }

        public virtual void VerticesWereChanged()
        {
            // Subclasses to override
        }

        #endregion Allocation and initialization


        #region Tag allocation

        public new static void ResetTagAllocation()
        {
            _lastAssignedVertexArrayTag = 0;
        }

        protected override int NextTag()
        {
            return ++_lastAssignedVertexArrayTag;
        }

        #endregion Tag allocation


        #region Binding artifacts

        public void CreateGraphicsBuffer()
        {
            if (_shouldAllowVertexBuffering == true && _isUsingGraphicsBuffer == false)
            {
                LCC3ProgPipeline pipeline = LCC3ProgPipeline.SharedPipeline();
                LCC3BufferTarget target = this.BufferTarget;

                _bufferID = pipeline.GenerateBuffer();
                pipeline.BindBufferToTarget(_bufferID, target);
                pipeline.LoadBufferTarget(_bufferID, target, _vertices);
                _isUsingGraphicsBuffer = true;
            }
        }

        public void UpdateGraphicsBuffer(uint offsetIndex, uint vtxCount)
        {
            if(_isUsingGraphicsBuffer == true)
            {
                LCC3ProgPipeline pipeline = LCC3ProgPipeline.SharedPipeline();
                LCC3BufferTarget target = this.BufferTarget;
                pipeline.UpdateBufferTarget(_bufferID, target, _vertices, offsetIndex);  
            }
        }

        public void UpdateGraphicsBuffer()
        {
            this.UpdateGraphicsBuffer(0, _vertexCount);
        }

        public void DeleteGraphicsBuffer()
        {
            if(_isUsingGraphicsBuffer == true)
            {
                LCC3ProgPipeline.SharedPipeline().DeleteBuffer(_bufferID);
                _isUsingGraphicsBuffer = false;
            }
        }


        public virtual void ReleaseRedundantContent()
        {
            if (this.IsUsingGraphicsBuffer == true && _shouldReleaseRedundantContent == true)
            {
                uint currVtxCount = _vertexCount;
                this.AllocatedVertexCapacity = 0;
                _vertexCount = currVtxCount;
            }
        }

        public void BindContentToAttributeAtIndexWithVisitor(LCC3VertexAttrIndex vaIdx, LCC3NodeDrawingVisitor visitor)
        {
            if (_vertices != null && _vertexCount > 0)
            {
                this.BindContentToAttributeAtIndexWithVisitor(_vertices, vaIdx, visitor);
            }
        }

        protected virtual void BindContentToAttributeAtIndexWithVisitor(object[] vertexData, LCC3VertexAttrIndex vaIdx, LCC3NodeDrawingVisitor visitor)
        {
            visitor.ProgramPipeline.BindVertexContentToAttributeAtIndex(vertexData, _elementType, _shouldNormalizeContent, vaIdx);
        }

        #endregion Binding artifacts

    }
}

