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
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3D
{
    public class LCC3Mesh : LCC3Identifiable
    {
        // Static fields

        static int _lastAssignedMeshTag = -1;
        static int _currentMeshTag = -1;

        // Instance fields

        float _capacityExpansionFactor;
        LCC3FaceArray _faceArray;
        LCC3VertexLocations _vertexLocations;
        LCC3VertexNormals _vertexNormals;
        LCC3VertexTangents _vertexTangents;
        LCC3VertexTangents _vertexBitangents;
        LCC3VertexIndices _vertexIndices;
        LCC3VertexColors _vertexColors;
        LCC3VertexTextureCoordinates _vertexTexCoords;
        LCC3VertexMatrixIndices _vertexMatrixIndices;
        LCC3VertexWeights _vertexWeights;
        LCC3VertexPointSizes _vertexPointSizes;

        VertexBuffer _xnaVertexBuffer;
        IndexBuffer _xnaIndexBuffer;

        #region Properties

        // Instance properties

        public float CapacityExpansionFactor
        {
            get { return _capacityExpansionFactor; }
            set { _capacityExpansionFactor = value; }
        }

        public LCC3VertexLocations VertexLocations
        {
            get { return _vertexLocations; }
            set { _vertexLocations = value; }
        }

        public LCC3VertexNormals VertexNormals
        {
            get { return _vertexNormals; }
            set { _vertexNormals = value; }
        }

        public LCC3VertexColors VertexColors
        {
            get { return _vertexColors; }
            set { _vertexColors = value; }
        }

        public LCC3VertexIndices VertexIndices
        {
            get { return _vertexIndices; }
            set { _vertexIndices = value; }
        }

        public LCC3VertexTangents VertexTangents
        {
            get { return _vertexTangents; }
            set { _vertexTangents = value; }
        }

        public LCC3VertexTangents VertexBitangents
        {
            get { return _vertexBitangents; }
            set { _vertexBitangents = value; }
        }

        public LCC3VertexTextureCoordinates VertexTextureCoords
        {
            get { return _vertexTexCoords; }
            set { _vertexTexCoords = value; }
        }

        public LCC3VertexMatrixIndices VertexMatrixIndices
        {
            get { return _vertexMatrixIndices; }
            set { _vertexMatrixIndices = value; }
        }

        public LCC3VertexWeights VertexWeights
        {
            get { return _vertexWeights; }
            set { _vertexWeights = value; }
        }

        public LCC3VertexPointSizes VertexPointSizes
        {
            get { return _vertexPointSizes; }
            set { _vertexPointSizes = value; }
        }

        public bool HasVertexLocations
        {
            get { return (_vertexLocations != null); }
        }

        public bool HasVertexNormals
        {
            get { return (_vertexNormals != null); }
        }

        public bool HasVertexTangents
        {
            get { return (_vertexTangents != null); }
        }

        public bool HasVertexBitangents
        {
            get { return (_vertexBitangents != null); }
        }

        public bool HasVertexColors
        {
            get { return (_vertexColors != null); }
        }

        public bool HasVertexWeights
        {
            get { return (_vertexWeights != null); }
        }

        public bool HasVertexMatrixIndices
        {
            get { return (_vertexMatrixIndices !=null); }
        }

        public bool HasVertexTextureCoordinates
        {
            get { return (_vertexTexCoords != null); }
        }

        public bool HasVertexPointSizes
        {
            get { return (_vertexPointSizes != null); }
        }

        // Vertex managment properties

        public LCC3VertexContent VertexContentTypes
        {
            get
            {
                LCC3VertexContent vtxContent = LCC3VertexContent.VertexContentNone;
                if (this.HasVertexLocations) vtxContent |= LCC3VertexContent.VertexContentLocation;
                if (this.HasVertexNormals) vtxContent |= LCC3VertexContent.VertexContentNormal;
                if (this.HasVertexTangents) vtxContent |= LCC3VertexContent.VertexContentTangent;
                if (this.HasVertexBitangents) vtxContent |= LCC3VertexContent.VertexContentBitangent;
                if (this.HasVertexColors) vtxContent |= LCC3VertexContent.VertexContentColor;
                if (this.HasVertexTextureCoordinates) vtxContent |= LCC3VertexContent.VertexContentTextureCoordinates;
                if (this.HasVertexWeights) vtxContent |= LCC3VertexContent.VertexContentWeights;
                if (this.HasVertexMatrixIndices) vtxContent |= LCC3VertexContent.VertexContentMatrixIndices;
                if (this.HasVertexPointSizes) vtxContent |= LCC3VertexContent.VertexContentPointSize;
                return vtxContent;
            }
            set { this.CreateVertexContent(value); }
        }

        public uint AllocatedVertexCapacity
        {
            get { return (_vertexLocations !=null) ? _vertexLocations.AllocatedVertexCapacity : 0; }
            set 
            { 
                if(_vertexLocations == null)
                {
                    this.VertexLocations = new LCC3VertexLocations(0,"locations");
                }

                _vertexLocations.AllocatedVertexCapacity = value;

                if(_vertexNormals != null)
                    _vertexNormals.AllocatedVertexCapacity = value;

                if(_vertexTangents != null)
                    _vertexTangents.AllocatedVertexCapacity = value;

                if(_vertexBitangents != null)
                    _vertexBitangents.AllocatedVertexCapacity = value;

                if(_vertexColors != null)
                    _vertexColors.AllocatedVertexCapacity = value;

                if(_vertexMatrixIndices != null)
                    _vertexMatrixIndices.AllocatedVertexCapacity = value;

                if(_vertexWeights != null)
                    _vertexWeights.AllocatedVertexCapacity = value;

                if(_vertexPointSizes != null)
                    _vertexPointSizes.AllocatedVertexCapacity = value;

                if(_vertexTexCoords != null)
                    _vertexTexCoords.AllocatedVertexCapacity = value;
            }
        }

        public uint AllocatedVertexIndexCapacity
        {
            get { return _vertexIndices != null ? _vertexIndices.AllocatedVertexCapacity : 0; }
            set
            {
                if ( _vertexIndices == null && value > 0 ) 
                { 
                    this.VertexIndices = new LCC3VertexIndices(0, "indices"); 
                }

                _vertexIndices.AllocatedVertexCapacity = value;
            }
        }

        // Accessing vertex content

        public uint VertexCount
        {
            get { return (_vertexLocations != null) ? _vertexLocations.VertexCount : 0; }
            set
            {
                _vertexLocations.VertexCount = value;
                _vertexNormals.VertexCount = value;
                _vertexTangents.VertexCount = value;
                _vertexBitangents.VertexCount = value;
                _vertexColors.VertexCount = value;
                _vertexMatrixIndices.VertexCount = value;
                _vertexWeights.VertexCount = value;
                _vertexPointSizes.VertexCount = value;
                _vertexTexCoords.VertexCount = value;
            }
        }

        public uint VertexIndexCount
        {
            get { return (_vertexIndices != null) ? _vertexIndices.VertexCount : 0; }
            set { _vertexIndices.VertexCount = value; }
        }

        // Mesh geometry

        public LCC3Vector CenterOfGeometry
        {
            get { return (_vertexLocations != null) ? _vertexLocations.CenterOfGeometry : LCC3Vector.CC3VectorZero; } 
        }

        public LCC3BoundingBox BoundingBox
        {
            get { return (_vertexLocations != null) ? _vertexLocations.BoundingBox : LCC3BoundingBox.CC3BoundingBoxNull; }
        }

        public float Radius
        {
            get { return (_vertexLocations != null) ? _vertexLocations.Radius : 0.0f; }
        }

        // CCRGBAProtocol support

        public CCColor3B Color
        {
            get { return (_vertexColors != null) ? _vertexColors.Color : CCTypes.CCBlack; }
            set { _vertexColors.Color = value; }
        }

        public byte Opacity
        {
            get { return (_vertexColors != null) ? _vertexColors.Opacity : (byte)0; }
            set { _vertexColors.Opacity = value; }
        }

        // Textures

        public bool ExpectsVerticallyFlippedTextures
        {
            get { return (_vertexTexCoords != null) ? _vertexTexCoords.ExpectsVerticallyFlippedTextures : false; }
            set { _vertexTexCoords.ExpectsVerticallyFlippedTextures = value; }
        }

        public CCRect TextureRectangle
        {
            get { return (_vertexTexCoords != null) ? _vertexTexCoords.TextureRectangle : CCRect.Zero; }
            set { _vertexTexCoords.TextureRectangle = value; }
        }

        // Drawing

        public LCC3DrawMode DrawingMode
        {
            get 
            { 
                if (_vertexIndices != null)
                {
                    return _vertexIndices.DrawingMode; 
                }

                if (_vertexLocations != null)
                {
                    return _vertexLocations.DrawingMode;
                }

                return LCC3DrawMode.TriangleStrip;
            }

            set
            {
                if (_vertexIndices != null)
                {
                    _vertexIndices.DrawingMode = value;
                }

                if (_vertexLocations != null)
                {
                    _vertexLocations.DrawingMode = value;
                }
            }
        }

        public bool SwitchingMesh
        {
            get
            {
                bool shouldSwitch = _currentMeshTag != this.Tag;
                _currentMeshTag = this.Tag; 
                return shouldSwitch;
            }
        }

        // Faces

        public LCC3FaceArray Faces
        {
            get 
            { 
                if (_faceArray == null)
                {
                    _faceArray = new LCC3FaceArray(0, String.Format("{0]-Faces", this.Name));
                    _faceArray.Mesh = this;
                }

                return _faceArray;
            }

            set { _faceArray = value; _faceArray.Mesh = this; }
        }

        public bool ShouldCacheFaces
        {
            get { return (_faceArray != null )? _faceArray.ShouldCacheFaces : false; }
            set { _faceArray.ShouldCacheFaces = value; }
        }

        public uint FaceCount
        {
            get 
            { 
                if (_vertexIndices != null) return _vertexIndices.FaceCount;
                if (_vertexLocations != null) return _vertexLocations.FaceCount;
                return 0; 
            }
        }

        // XNA properties

        internal VertexBuffer XnaVertexBuffer
        {
            get { return _xnaVertexBuffer; }
            set { _xnaVertexBuffer = value; }
        }

        internal IndexBuffer XnaIndexBuffer
        {
            get { return _xnaIndexBuffer; }
            set { _xnaIndexBuffer = value; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3Mesh(int tag, string name) : base(tag, name)
        {
            _capacityExpansionFactor = 1.25f;
        }

        public void PopulateFrom(LCC3Mesh anotherMesh)
        {
            base.PopulateFrom(anotherMesh);

            this.Faces = anotherMesh.Faces;
            this.VertexLocations = anotherMesh.VertexLocations;                    
            this.VertexNormals = anotherMesh.VertexNormals;                         
            this.VertexTangents = anotherMesh.VertexTangents;                       
            this.VertexBitangents = anotherMesh.VertexBitangents;                   
            this.VertexColors = anotherMesh.VertexColors;                           
            this.VertexMatrixIndices = anotherMesh.VertexMatrixIndices;             
            this.VertexWeights = anotherMesh.VertexWeights;                         
            this.VertexPointSizes = anotherMesh.VertexPointSizes;                   
            this.VertexTextureCoords = anotherMesh.VertexTextureCoords;
        }

        #endregion Allocation and initialization


        #region Tag allocation

        public new static void ResetTagAllocation()
        {
            _lastAssignedMeshTag = 0;
        }

        public static new int NextTag()
        {
            return ++_lastAssignedMeshTag;
        }

        #endregion Tag allocation


        #region Vertex Arrays

        public LCC3VertexArray VertexArrayForSemanticAtIndex(LCC3Semantic vertexSemantic, uint semanticIndex)
        {
            switch (vertexSemantic) 
            {
                case LCC3Semantic.SemanticVertexLocation: 
                    return this.VertexLocations;
                case LCC3Semantic.SemanticVertexNormal:
                    return this.VertexNormals;
                case LCC3Semantic.SemanticVertexTangent:
                    return this.VertexTangents;
                case LCC3Semantic.SemanticVertexBitangent:
                    return this.VertexBitangents;
                case LCC3Semantic.SemanticColor:
                    return this.VertexColors;
                case LCC3Semantic.SemanticVertexWeights:
                    return this.VertexWeights;
                case LCC3Semantic.SemanticVertexMatrixIndices:
                    return this.VertexMatrixIndices;
                case LCC3Semantic.SemanticPointSize:
                    return this.VertexPointSizes;
                case LCC3Semantic.SemanticVertexTexture:
                    return this.VertexTextureCoords;
                default:
                    return null;
            }
        }

        #endregion Vertex Arrays


        #region Vertex management

        private void CreateVertexContent(LCC3VertexContent vtxContentTypes)
        {
            if (_vertexLocations == null) 
            {
                this.VertexLocations = new LCC3VertexLocations(0, "locations");
            }

            if ((vtxContentTypes & LCC3VertexContent.VertexContentNormal) > 0) 
            {
                if (_vertexNormals == null)
                {
                    this.VertexNormals = new LCC3VertexNormals(0, "normals");
                }
            } 
            else 
            {
                this.VertexNormals = null;
            }

            if ((vtxContentTypes & LCC3VertexContent.VertexContentTangent) > 0) 
            {
                if (_vertexTangents == null) 
                {
                    this.VertexTangents = new LCC3VertexTangents(0, "tangents");
                }
            } 
            else 
            {
                this.VertexTangents = null;
            }

            if ((vtxContentTypes & LCC3VertexContent.VertexContentBitangent) > 0) 
            {
                if (_vertexBitangents == null) 
                {
                    this.VertexBitangents = new LCC3VertexTangents(0, "bitangents");
                }
            } 
            else 
            {
                this.VertexBitangents = null;
            }

            if ((vtxContentTypes & LCC3VertexContent.VertexContentColor) > 0) 
            {
                if (_vertexColors == null) 
                {
                    LCC3VertexColors vCols = new LCC3VertexColors(0, "colors");
                    vCols.ElementType = LCC3ElementType.UnsignedByte;
                    this.VertexColors = vCols;
                }
            } 
            else 
            {
                this.VertexColors = null;
            }

            if ((vtxContentTypes & LCC3VertexContent.VertexContentTextureCoordinates) > 0) 
            {
                if (_vertexTexCoords == null)
                {
                    this.VertexTextureCoords = new LCC3VertexTextureCoordinates(0, "texcoord");
                }
            } 
            else 
            {
                this.VertexTextureCoords = null;
            }

            if ((vtxContentTypes & LCC3VertexContent.VertexContentWeights) > 0) 
            {
                if (_vertexWeights == null) 
                {
                    this.VertexWeights = new LCC3VertexWeights(0, "weights");
                }
            } 
            else 
            {
                this.VertexWeights = null;
            }

            if ((vtxContentTypes & LCC3VertexContent.VertexContentMatrixIndices) > 0) 
            {
                if (_vertexMatrixIndices == null) 
                {
                    this.VertexMatrixIndices = new LCC3VertexMatrixIndices(0, "matrixIndices");
                }
            } 
            else 
            {
                this.VertexMatrixIndices = null;
            }

            if ((vtxContentTypes & LCC3VertexContent.VertexContentPointSize) > 0) 
            {
                if (_vertexPointSizes == null) 
                {
                    this.VertexPointSizes = new LCC3VertexPointSizes(0, "pointSizes");
                }
            } 
            else 
            {
                this.VertexPointSizes = null;
            }
        }

        public bool EnsureVertexCapacity(uint vtxCount)
        {
            uint currVtxCap = this.AllocatedVertexCapacity;
            if (currVtxCap > 0 && currVtxCap < vtxCount) 
            {
                this.AllocatedVertexCapacity = (uint)(vtxCount * this.CapacityExpansionFactor);
                return (this.AllocatedVertexCapacity > currVtxCap);
            }
            return false;
        }

        #endregion Vertex management


        #region Accessing vertex content

        public LCC3Vector VertexLocationAtIndex(uint index)
        {
            return (_vertexLocations != null) ? _vertexLocations.LocationAt(index) : LCC3Vector.CC3VectorZero;
        }

        public void SetVertexLocationAtIndex(LCC3Vector location, uint index)
        {
            _vertexLocations.SetLocation(location, index);
        }

        public LCC3Vector VertexNormalAtIndex(uint index)
        {
            return (_vertexNormals != null) ? _vertexNormals.NormalAtIndex(index) : LCC3Vector.CC3UnitZPositive;
        }

        public void SetNormalAtIndex(LCC3Vector normal, uint index)
        {
            _vertexNormals.SetNormalAtIndex(normal, index);
        }

        public LCC3Vector VertexTangentAtIndex(uint index)
        {
            return (_vertexTangents != null) ? _vertexTangents.TangentAtIndex(index) : LCC3Vector.CC3UnitXPositive;
        }

        public void SetTangentAtIndex(LCC3Vector tangent, uint index)
        {
            _vertexTangents.SetTangentAtIndex(tangent, index);
        }

        public LCC3Vector VertexBitangentAtIndex(uint index)
        {
            return (_vertexBitangents != null) ? _vertexBitangents.TangentAtIndex(index) : LCC3Vector.CC3UnitYPositive;
        }

        public void SetBitangentAtIndex(LCC3Vector bitangent, uint index)
        {
            _vertexBitangents.SetTangentAtIndex(bitangent, index);
        }

        public CCColor4F VertexColor4FAtIndex(uint index)
        {
            return (_vertexColors != null) ? _vertexColors.Color4FAtIndex(index) : LCC3ColorUtil.CCC4FBlackTransparent;
        }

        public void SetVertexColor4FAtIndex(CCColor4F color, uint index)
        {
            _vertexColors.SetColor4FAtIndex(color, index);
        }

        public float VertexWeightForVertexUnitAtIndex(uint vertexUnit, uint index)
        {
            return (_vertexWeights != null) ? _vertexWeights.WeightForVertexUnitAtIndex(vertexUnit, index) : 0.0f;
        }

        public float[] VertexWeightsAtIndex(uint index)
        {
            return (_vertexWeights != null) ? _vertexWeights.WeightsAtIndex(index) : null;
        }

        public void SetVertexWeightsAtIndex(float[] weights, uint index)
        {
            _vertexWeights.SetWeightsAtIndex(weights, index);
        }

        public uint[] VertexMatrixIndicesAtIndex(uint index)
        {
            return (_vertexMatrixIndices != null) ? _vertexMatrixIndices.MatrixIndicesAtIndex(index) : null;
        }

        public void SetVertexMatrixIndicesAtIndex(uint[] matrixIndices, uint index)
        {
            _vertexMatrixIndices.SetMatrixIndicesAtIndex(matrixIndices, index);
        }

        public float VertexPointSizeAtIndex(uint index)
        {
            return (_vertexPointSizes != null) ? _vertexPointSizes.PointSizeAt(index) : 0.0f;
        }

        public void SetVertexPointSizeAtIndex(float size, uint index)
        {
            _vertexPointSizes.SetPointSizeAtIndex(size, index);
        }

        public CCTex2F VertexTexCoord2FAtIndex(uint index)
        {
            return (_vertexTexCoords != null) ? _vertexTexCoords.TexCoord2FAt(index) : new CCTex2F(0.0f, 0.0f);
        }

        public void SetVertexTexCoord2FAtIndex(CCTex2F texCoord, uint index)
        {
            _vertexTexCoords.SetTexCoord2FAtIndex(texCoord, index);
        }

        public uint VertexIndexAtIndex(uint index)
        {
            return  (_vertexIndices != null) ? _vertexIndices.IndexAt(index) : 0;
        }

        public void SetVertexIndexAtIndex(uint vertexIndex, uint index)
        {
            _vertexIndices.SetIndex(vertexIndex, index);
        }

        #endregion Accessing vertex content


        #region Buffering content

        public void CreateGraphicsBuffers()
        {
            _vertexLocations.CreateGraphicsBuffer();
            _vertexNormals.CreateGraphicsBuffer();
            _vertexTangents.CreateGraphicsBuffer();
            _vertexBitangents.CreateGraphicsBuffer();
            _vertexColors.CreateGraphicsBuffer();
            _vertexMatrixIndices.CreateGraphicsBuffer();
            _vertexWeights.CreateGraphicsBuffer();
            _vertexPointSizes.CreateGraphicsBuffer();
            _vertexTexCoords.CreateGraphicsBuffer();
            _vertexIndices.CreateGraphicsBuffer();
        }

        public void DeleteGraphicsBuffers()
        {
            _vertexLocations.DeleteGraphicsBuffer();
            _vertexNormals.DeleteGraphicsBuffer();
            _vertexTangents.DeleteGraphicsBuffer();
            _vertexBitangents.DeleteGraphicsBuffer();
            _vertexColors.DeleteGraphicsBuffer();
            _vertexMatrixIndices.DeleteGraphicsBuffer();
            _vertexWeights.DeleteGraphicsBuffer();
            _vertexPointSizes.DeleteGraphicsBuffer();
            _vertexTexCoords.DeleteGraphicsBuffer();
            _vertexIndices.DeleteGraphicsBuffer();
        }

        public void ReleaseRedundantContent()
        {
            _vertexLocations.ReleaseRedundantContent();
            _vertexNormals.ReleaseRedundantContent();
            _vertexTangents.ReleaseRedundantContent();
            _vertexBitangents.ReleaseRedundantContent();
            _vertexColors.ReleaseRedundantContent();
            _vertexMatrixIndices.ReleaseRedundantContent();
            _vertexWeights.ReleaseRedundantContent();
            _vertexPointSizes.ReleaseRedundantContent();
            _vertexTexCoords.ReleaseRedundantContent();
            _vertexIndices.ReleaseRedundantContent();
        }

        public void DoNotBufferVertexContent()
        {
            this.DoNotBufferVertexLocations();
            this.DoNotBufferVertexNormals();
            this.DoNotBufferVertexTangents();
            this.DoNotBufferVertexBitangents();
            this.DoNotBufferVertexColors();
            this.DoNotBufferVertexMatrixIndices();
            this.DoNotBufferVertexWeights();
            this.DoNotBufferVertexPointSizes();
            this.DoNotBufferVertexTextureCoordinates();
        }

        public void DoNotBufferVertexLocations()
        {
            _vertexLocations.ShouldAllowVertexBuffering = false;
        }

        public void DoNotBufferVertexNormals()
        {
            _vertexNormals.ShouldAllowVertexBuffering = false;
        }

        public void DoNotBufferVertexTangents()
        {
            _vertexTangents.ShouldAllowVertexBuffering = false;
        }

        public void DoNotBufferVertexBitangents()
        {
            _vertexBitangents.ShouldAllowVertexBuffering = false;
        }

        public void DoNotBufferVertexColors()
        {
            _vertexColors.ShouldAllowVertexBuffering = false;
        }

        public void DoNotBufferVertexMatrixIndices()
        {
            _vertexMatrixIndices.ShouldAllowVertexBuffering = false;
        }

        public void DoNotBufferVertexWeights()
        {
            _vertexWeights.ShouldAllowVertexBuffering = false;
        }

        public void DoNotBufferVertexPointSizes()
        {
            _vertexPointSizes.ShouldAllowVertexBuffering = false;
        }

        public void DoNotBufferVertexTextureCoordinates()
        {
            _vertexTexCoords.ShouldAllowVertexBuffering = false;
        }

        public void DoNotBufferVertexIndices()
        {
            _vertexIndices.ShouldAllowVertexBuffering = false;
        }

        #endregion Buffering content


        #region Updating

        public void UpdateGraphicsBuffers(uint offsetIndex, uint vtxCount)
        {
            _vertexLocations.UpdateGraphicsBuffer(offsetIndex, vtxCount);
            _vertexNormals.UpdateGraphicsBuffer(offsetIndex, vtxCount);
            _vertexTangents.UpdateGraphicsBuffer(offsetIndex, vtxCount);
            _vertexBitangents.UpdateGraphicsBuffer(offsetIndex, vtxCount);
            _vertexColors.UpdateGraphicsBuffer(offsetIndex, vtxCount);
            _vertexMatrixIndices.UpdateGraphicsBuffer(offsetIndex, vtxCount);
            _vertexWeights.UpdateGraphicsBuffer(offsetIndex, vtxCount);
            _vertexPointSizes.UpdateGraphicsBuffer(offsetIndex, vtxCount);
            _vertexTexCoords.UpdateGraphicsBuffer(offsetIndex, vtxCount);
            _vertexIndices.UpdateGraphicsBuffer(offsetIndex, vtxCount);
        }

        public void UpdateGraphicsBuffers()
        {
            this.UpdateGraphicsBuffers(0, this.VertexCount);
        }

        public void UpdateVertexLocationsGraphicsBuffer()
        {
            _vertexLocations.UpdateGraphicsBuffer();
        }

        public void UpdateVertexNormalsGraphicsBuffer()
        {
            _vertexNormals.UpdateGraphicsBuffer();
        }

        public void UpdateVertexTangentsGraphicsBuffer()
        {
            _vertexTangents.UpdateGraphicsBuffer();
        }

        public void UpdateVertexBitangentsGraphicsBuffer()
        {
            _vertexBitangents.UpdateGraphicsBuffer();
        }

        public void UpdateVertexColorsGraphicsBuffer()
        {
            _vertexColors.UpdateGraphicsBuffer();
        }

        public void UpdateVertexWeightsGraphicsBuffer()
        {
            _vertexWeights.UpdateGraphicsBuffer();
        }

        public void UpdateVertexMatrixIndicesGraphicsBuffer()
        {
            _vertexMatrixIndices.UpdateGraphicsBuffer();
        }

        public void UpdateVertexTextureCoordinatesGraphicsBuffer()
        {
            _vertexTexCoords.UpdateGraphicsBuffer();
        }

        public void UpdateVertexIndicesGraphicsBuffer()
        {
            _vertexIndices.UpdateGraphicsBuffer();
        }

        #endregion Updating


        #region Mesh geomety

        public void MoveMeshOriginTo(LCC3Vector location)
        {
            _vertexLocations.MoveMeshOriginTo(location);
        }

        public void MoveMeshOriginToCenterOfGeometry()
        {
            _vertexLocations.MoveMeshOriginToCenterOfGeometry();
        }

        #endregion Mesh geometry


        #region Drawing

        public static void ResetSwitching()
        {
            _currentMeshTag = 0;
        }

        public void DrawWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            this.BindWithVisitor(visitor);
            this.DrawVerticesWithVisitor(visitor);
        }

        public void DrawWithVisitor(uint vertexIndex, uint count, LCC3NodeDrawingVisitor visitor)
        {
            this.BindWithVisitor(visitor);
            this.DrawVerticesWithVisitor(vertexIndex, count, visitor);
        }

        public void BindWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            if (this.SwitchingMesh == true)
            {
                visitor.ProgramPipeline.BindMeshWithVisitor(this, visitor);
            }
        }

        public void DrawVerticesWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            visitor.CurrentShaderProgram.PopulateDrawScopeUniformsWithVisitor(visitor);

            if (_vertexIndices != null) 
            {
                _vertexIndices.DrawWithVisitor(visitor);
            } 
            else 
            {
                _vertexLocations.DrawWithVisitor(visitor);
            }
        }

        public void DrawVerticesWithVisitor(uint vertexIndex, uint count, LCC3NodeDrawingVisitor visitor)
        {
            visitor.CurrentShaderProgram.PopulateDrawScopeUniformsWithVisitor(visitor);

            if (_vertexIndices != null) 
            {
                _vertexIndices.DrawFromIndexWithVisitor(vertexIndex, count, visitor);
            } 
            else 
            {
                _vertexLocations.DrawFromIndexWithVisitor(vertexIndex, count, visitor);
            }
        }

        #endregion Drawing


        #region Faces

        public LCC3Face FaceFromIndices(LCC3FaceIndices faceIndices)
        {
            return (_vertexLocations != null) ? _vertexLocations.FaceFromIndices(faceIndices) : LCC3Face.CC3FaceZero;
        }

        public LCC3FaceIndices UncachedFaceIndicesAtFaceIndex(uint faceIndex)
        {
            if (_vertexIndices != null)
            {
                return _vertexIndices.FaceIndicesAtIndex(faceIndex);
            }

            if (_vertexLocations != null)
            {
                return _vertexLocations.FaceIndicesAtIndex(faceIndex);
            }

            return LCC3FaceIndices.CC3FaceIndicesZero;
        }

        public LCC3Face FaceAtIndex(uint faceIndex)
        {
            return this.FaceFromIndices(this.FaceIndicesAtIndex(faceIndex));
        }

        public LCC3FaceIndices FaceIndicesAtIndex(uint faceIndex)
        {
            return this.Faces.IndicesAtFaceIndex(faceIndex); 
        }

        public LCC3Vector FaceCenterAtIndex(uint faceIndex)
        {
            return this.Faces.CenterAtFaceIndex(faceIndex);
        }

        public LCC3Vector FaceNormalAtIndex(uint faceIndex)
        {
            return this.Faces.NormalAtFaceIndex(faceIndex);
        }

        public LCC3Plane FacePlaneAtIndex(uint faceIndex)
        {
            return this.Faces.PlaneAtFaceIndex(faceIndex);
        }

        public LCC3FaceNeighbours FaceNeighbours(uint faceIndex)
        {
            return this.Faces.NeighboursAtFaceIndex(faceIndex);
        }

        #endregion Faces

    }
}

