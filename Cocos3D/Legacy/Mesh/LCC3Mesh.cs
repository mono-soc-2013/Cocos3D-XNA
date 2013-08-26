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
    public class LCC3Mesh
    {
        // Instance fields

        LCC3VertexLocations _vertexLocations;
        LCC3VertexNormals _vertexNormals;
        LCC3VertexTangents _vertexTangents;
        LCC3VertexIndices _vertexIndices;
        LCC3VertexColors _vertexColors;
        LCC3VertexTextureCoordinates _vertexTexCoords;

        #region Properties

        // Instance properties

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
        }

        public LCC3VertexTextureCoordinates VertexTextureCoords
        {
            get { return _vertexTexCoords; }
            set { _vertexTexCoords = value; }
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
            get { return false; }
        }

        public bool HasVertexColors
        {
            get { return (_vertexColors != null); }
        }

        public bool HasVertexWeights
        {
            get { return false; }
        }

        public bool HasVertexMatrixIndices
        {
            get { return false; }
        }

        public bool HasVertexTextureCoordinates
        {
            get { return (_vertexTexCoords != null); }
        }

        public bool HasVertexPointSizes
        {
            get { return false; }
        }
                
        #endregion Properties


        public LCC3Mesh()
        {
        }

        public LCC3VertexArray VertexArrayForSemanticAtIndex(LCC3Semantic vertexSemantic, uint semanticIndex)
        {
            switch (vertexSemantic) 
            {
                case LCC3Semantic.SemanticVertexLocation: 
                    return this.VertexLocations;
                case LCC3Semantic.SemanticVertexNormal:
                    return this.VertexNormals;
                case LCC3Semantic.SemanticColor:
                    return this.VertexColors;
                case LCC3Semantic.SemanticVertexTexture:
                    return this.VertexTextureCoords;
                default:
                    return null;
            }
        }
    
    }
}

