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

namespace Cocos3D
{
    public class LCC3FaceArray : LCC3Identifiable
    {
        // Instance vars

        LCC3Mesh _mesh;
        LCC3FaceIndices[] _indices;
        LCC3Vector[] _centers;
        LCC3Vector[] _normals;
        LCC3Plane[] _planes;
        LCC3FaceNeighbours[] _neighbours;

        bool _indicesAreDirty;
        bool _centersAreDirty;
        bool _normalsAreDirty;
        bool _planesAreDirty;
        bool _neighboursAreDirty;
        bool _shouldCacheFaces;


        #region Properties

        // Instance properties

        public LCC3Mesh Mesh
        {
            get { return _mesh; }
            set { _mesh = value; }
        }

        public uint FaceCount
        {
            get { return _mesh != null ? _mesh.FaceCount : 0; }
        }

        public LCC3FaceIndices[] Indices
        {
            get
            {
                if (_indicesAreDirty == true || _indices == null)
                {
                    this.PopulateIndices();
                }

                return _indices;
            }

            set
            {
                this.DeallocateIndices();
                _indices = value;
            }
        }

        public LCC3Vector[] Centers
        {
            get
            {
                if (_centersAreDirty == true || _centers == null)
                {
                    this.PopulateCenters();
                }

                return _centers;
            }

            set
            {
                this.DeallocateCenters();
                _centers = value;
            }
        }

        public LCC3Vector[] Normals
        {
            get
            {
                if (_normalsAreDirty == true || _normals == null)
                {
                    this.PopulateNormals();
                }

                return _normals;
            }

            set
            {
                this.DeallocateNormals();
                _normals = value;
            }
        }

        public LCC3Plane[] Planes
        {
            get
            {
                if (_planesAreDirty == true || _planes == null)
                {
                    this.PopulatePlanes();
                }

                return _planes;
            }

            set
            {
                this.DeallocatePlanes();
                _planes = value;
            }
        }

        public LCC3FaceNeighbours[] Neighbours
        {
            get
            {
                if (_neighboursAreDirty == true || _neighbours == null)
                {
                    this.PopulateNeighbours();
                }

                return _neighbours;
            }

            set
            {
                this.DeallocateNeighbours();
                _neighbours = value;
            }
        }

        public bool ShouldCacheFaces
        {
            get { return _shouldCacheFaces; }
            set { _shouldCacheFaces = value; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3FaceArray(int tag, string name) : base(tag, name)
        {
            _indicesAreDirty = true;
            _centersAreDirty = true;
            _normalsAreDirty = true;
            _planesAreDirty = true;
            _neighboursAreDirty = true;
        }

        public void PopulateFrom(LCC3FaceArray another)
        {
            _mesh = another.Mesh;
            _shouldCacheFaces = another._shouldCacheFaces;
            _indices = another.Indices;
            _centers = another.Centers;
            _normals = another.Normals;
            _neighbours = another.Neighbours;

            _indicesAreDirty = another._indicesAreDirty;
            _centersAreDirty = another._centersAreDirty;
            _normalsAreDirty = another._normalsAreDirty;
            _neighboursAreDirty = another._neighboursAreDirty;
        }

        #endregion Allocation and initialization


        #region Faces

        public LCC3Face FaceAtIndex(uint faceIndex)
        {
            return (_mesh !=null) ? _mesh.FaceAtIndex(faceIndex) : LCC3Face.CC3FaceZero;
        }

        #endregion Faces


        #region Indices

        public void PopulateIndices()
        {
            if ( _indices == null)
            {
                this.AllocateIndices();
            }

            uint faceCount = this.FaceCount;

            for (uint faceIdx = 0; faceIdx < faceCount; faceIdx++) 
            {
                _indices[faceIdx] = this.UncachedIndicesAtFaceIndex(faceIdx);
            }

            _indicesAreDirty = false;
        }

        public LCC3FaceIndices IndicesAtFaceIndex(uint faceIndex)
        {
            if (_shouldCacheFaces)
            {
                return _indices[(int)faceIndex];
            }

            return this.UncachedIndicesAtFaceIndex(faceIndex);
        }

        public LCC3FaceIndices UncachedIndicesAtFaceIndex(uint faceIndex)
        {
            return _mesh.UncachedFaceIndicesAtFaceIndex(faceIndex);
        }

        private LCC3FaceIndices[] AllocateIndices()
        {
            this.DeallocateIndices();
            uint faceCount = this.FaceCount;

            if (faceCount > 0) 
            {
                _indices = new LCC3FaceIndices[faceCount];
            }

            return _indices;
        }

        private void DeallocateIndices()
        {
            _indices = null;
        }

        public void MarkIndicesDirty()
        {
            _indicesAreDirty = true;
        }

        #endregion Indices


        #region Centers

        public void PopulateCenters()
        {
            if ( _centers == null)
            {
                this.AllocateCenters();
            }

            uint faceCount = this.FaceCount;

            for (uint faceIdx = 0; faceIdx < faceCount; faceIdx++) 
            {
                _centers[faceIdx] = this.FaceAtIndex(faceIdx).Center;
            }

            _centersAreDirty = false;
        }

        public LCC3Vector CenterAtFaceIndex(uint faceIndex)
        {
            if (_shouldCacheFaces)
            {
                return _centers[(int)faceIndex];
            }

            return this.FaceAtIndex(faceIndex).Center;
        }

        private LCC3Vector[] AllocateCenters()
        {
            this.DeallocateCenters();
            uint faceCount = this.FaceCount;

            if (faceCount > 0) 
            {
                _centers = new LCC3Vector[faceCount];
            }

            return _centers;
        }

        private void DeallocateCenters()
        {
            _centers = null;
        }

        public void MarkCentersDirty()
        {
            _centersAreDirty = true;
        }

        #endregion Centers


        #region Normals

        public void PopulateNormals()
        {
            if ( _normals == null)
            {
                this.AllocateNormals();
            }

            uint faceCount = this.FaceCount;

            for (uint faceIdx = 0; faceIdx < faceCount; faceIdx++) 
            {
                _normals[faceIdx] = this.FaceAtIndex(faceIdx).FaceNormal;
            }

            _normalsAreDirty = false;
        }

        public LCC3Vector NormalAtFaceIndex(uint faceIndex)
        {
            if (_shouldCacheFaces)
            {
                return _normals[(int)faceIndex];
            }

            return this.FaceAtIndex(faceIndex).FaceNormal;
        }

        private LCC3Vector[] AllocateNormals()
        {
            this.DeallocateNormals();
            uint faceCount = this.FaceCount;

            if (faceCount > 0) 
            {
                _normals = new LCC3Vector[faceCount];
            }

            return _normals;
        }

        private void DeallocateNormals()
        {
            _normals = null;
        }

        public void MarkNormalsDirty()
        {
            _normalsAreDirty = true;
        }

        #endregion Normals


        #region Planes

        public void PopulatePlanes()
        {
            if ( _planes  == null)
            {
                this.AllocatePlanes();
            }

            uint faceCount = this.FaceCount;

            for (uint faceIdx = 0; faceIdx < faceCount; faceIdx++) 
            {
                _planes[faceIdx] = this.FaceAtIndex(faceIdx).FacePlane;
            }

            _planesAreDirty = false;
        }

        public LCC3Plane PlaneAtFaceIndex(uint faceIndex)
        {
            if (_shouldCacheFaces)
            {
                return _planes[(int)faceIndex];
            }

            return this.FaceAtIndex(faceIndex).FacePlane;
        }

        private LCC3Plane[] AllocatePlanes()
        {
            this.DeallocatePlanes();
            uint faceCount = this.FaceCount;

            if (faceCount > 0) 
            {
                _planes = new LCC3Plane[faceCount];
            }

            return _planes;
        }

        private void DeallocatePlanes()
        {
            _planes = null;
        }

        public void MarkPlanesDirty()
        {
            _planesAreDirty = true;
        }

        #endregion Planes


        #region Neighbours

        public void PopulateNeighbours()
        {
            throw new NotImplementedException();
        }

        public LCC3FaceNeighbours NeighboursAtFaceIndex(uint faceIndex)
        {
            return _neighbours[(int)faceIndex];
        }

        private LCC3FaceNeighbours[] AllocateNeighbours()
        {
            this.DeallocateNeighbours();
            uint faceCount = this.FaceCount;

            if (faceCount > 0) 
            {
                _neighbours = new LCC3FaceNeighbours[faceCount];
            }

            return _neighbours;
        }

        private void DeallocateNeighbours()
        {
            _neighbours = null;
        }

        public void MarkNeighboursDirty()
        {
            _neighboursAreDirty = true;
        }

        #endregion Neighbours
    }
}

