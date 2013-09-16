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
    public class LCC3VertexLocations : LCC3DrawableVertexArray
    {
        // ivars

        uint _firstVertex;
        LCC3BoundingBox _boundingBox;
        LCC3Vector _centerOfGeometry;
        float _radius;
        bool _boundaryIsDirty;
        bool _radiusIsDirty;

        #region Properties

        // Static properties

        public new static LCC3Semantic DefaultSemantic 
        { 
            get { return LCC3Semantic.SemanticVertexLocation; }
        }

        // Instance properties

        public override string NameSuffix
        {
            get { return "Locations"; }
        }

        public override uint VertexCount
        {
            set { base.VertexCount = value; this.MarkBoundaryDirty(); }
        }

        protected bool BoundaryIsDirty
        {
            get { return _boundaryIsDirty; }
        }

        protected bool RadiusIsDirty
        {
            get { return _radiusIsDirty; }
        }

        public LCC3BoundingBox BoundingBox
        {
            get { this.BuildBoundingBoxIfNecessary(); return _boundingBox; }
        }

        public LCC3Vector CenterOfGeometry
        {
            get { this.BuildBoundingBoxIfNecessary(); return _centerOfGeometry; }
        }

        public float Radius
        {
            get { this.CalcRadiusIfNecessary(); return _radius; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3VertexLocations(int tag, string name) : base(tag, name)
        {
            _firstVertex = 0;
            _centerOfGeometry = LCC3Vector.CC3VectorZero;
            _boundingBox = LCC3BoundingBox.CC3BoundingBoxZero;
            _radius = 0.0f;

            this.MarkBoundaryDirty();
        }

        public void PopulateFrom(LCC3VertexLocations another)
        {
            base.PopulateFrom(another);

            _firstVertex = another._firstVertex;
            _boundingBox = another.BoundingBox;
            _centerOfGeometry = another.CenterOfGeometry;
            _radius = another.Radius;
            _boundaryIsDirty = another.BoundaryIsDirty;
            _radiusIsDirty = another.RadiusIsDirty;
        }

        #endregion Allocation and initialization


        #region Configuring locations

        public void MarkBoundaryDirty()
        {
            _boundaryIsDirty = true;
            _radiusIsDirty = true;
        }

        public override void VerticesWereChanged()
        {
            if (_vertices != null && this.VertexCount > 0)
            {
                this.MarkBoundaryDirty();
            }
        }

        public LCC3Vector LocationAt(uint index)
        {
            LCC3Vector location = (LCC3Vector)_vertices[(int)index];

            switch(this.ElementSize)
            {
                case 2:
                    location = new LCC3Vector(location.X, location.Y, 0.0f);
                    break;
            }

            return location;
        }

        public void SetLocation(LCC3Vector location, uint index)
        {
            switch(this.ElementSize)
            {
                case 2:
                    _vertices[index] = new CCPoint(location.X, location.Y);
                    break;
                case 4:
                    _vertices[index] = new LCC3Vector4(location, 1.0f);
                    break;
                default:
                    _vertices[index] = location;
                    break;
            }

            this.MarkBoundaryDirty();
        }

        public LCC3Vector4 HomogeneousLocationAt(uint index)
        {
            LCC3Vector4 location = (LCC3Vector4)_vertices[(int)index];
            float newZ = location.Z;
            float newW = location.W;

            switch(this.ElementSize)
            {
                case 2:
                    newZ = 0.0f;
                    newW = 1.0f;
                    break;
                case 3:
                    newW = 1.0f;
                    break;
                default:
                    break;
            }

            location = new LCC3Vector4(location.X, location.Y, newZ, newW);

            return location;
        }

        public void SetHomogeneousLocation(LCC3Vector4 location, uint index)
        {
            switch(this.ElementSize)
            {
                case 2:
                    _vertices[index] = new CCPoint(location.X, location.Y);
                    break;
                case 3:
                    _vertices[index] = location.TruncateToCC3Vector();
                    break;
                default:
                    _vertices[index] = location;
                    break;
            }
        }

        private void BuildBoundingBoxIfNecessary()
        {
            if (_boundaryIsDirty)
            {
                this.BuildBoundingBox();
            }
        }

        private void BuildBoundingBox()
        {
            LCC3Vector vl, vlMin, vlMax;
            vl = (this.VertexCount > 0) ? this.LocationAt(0) : LCC3Vector.CC3VectorZero;
            vlMin = vl;
            vlMax = vl;

            for (uint i = 1; i < this.VertexCount; i++) 
            {
                vl = this.LocationAt(i);
                vlMin = LCC3Vector.CC3VectorMinimize(vlMin, vl);
                vlMax = LCC3Vector.CC3VectorMaximize(vlMax, vl);
            }

            _boundingBox = new LCC3BoundingBox(vlMin, vlMax);
            _centerOfGeometry = _boundingBox.Center;
            _boundaryIsDirty = false;
        }

        private void CalcRadiusIfNecessary()
        {
            if (_radiusIsDirty)
            {
                this.CalcRadius();
            }
        }

        private void CalcRadius()
        {
            LCC3Vector cog = this.CenterOfGeometry;

            if (_vertices != null && this.VertexCount > 0) 
            {
                float radiusSq = 0.0f;

                for (uint i=0; i < this.VertexCount; i++) 
                {
                    LCC3Vector vl = this.LocationAt(i);
                    float distSq = LCC3Vector.CC3DistanceSquared(vl, cog);
                    radiusSq = Math.Max(radiusSq, distSq);
                }

                _radius = (float)Math.Sqrt(radiusSq);      
                _radiusIsDirty = false;
            }
        }

        public void MoveMeshOriginTo(LCC3Vector location)
        {
            for (uint i = 0; i < this.VertexCount; i++) 
            {
                LCC3Vector locOld = this.LocationAt(i);
                LCC3Vector locNew = locOld - location;
                this.SetLocation(locNew, i);
            }

            this.MarkBoundaryDirty();
            this.UpdateGraphicsBuffer();
        }

        public void MoveMeshOriginToCenterOfGeometry()
        {
            this.MoveMeshOriginTo(this.CenterOfGeometry);
        }

        #endregion Configuring locations


        #region Drawing

        public override void ReleaseRedundantContent()
        {
            this.BuildBoundingBoxIfNecessary();
            this.CalcRadiusIfNecessary();
            base.ReleaseRedundantContent();
        }

        public override void DrawFromIndexWithVisitor(uint vertexIndex, uint vertexCount, LCC3NodeDrawingVisitor visitor)
        {
            base.DrawFromIndexWithVisitor(vertexIndex, vertexCount, visitor);
            visitor.ProgramPipeline.DrawVertices(this.DrawingMode, (int)(_firstVertex + vertexIndex), (int)vertexCount);
        }

        #endregion Drawing


        #region Faces

        public LCC3Face FaceFromIndices(LCC3FaceIndices faceIndices)
        {
            return new LCC3Face(this.LocationAt(faceIndices.Index1), 
                                this.LocationAt(faceIndices.Index2), 
                                this.LocationAt(faceIndices.Index3));
        }

        #endregion Faces
    }
}

