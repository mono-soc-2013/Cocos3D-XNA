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

        LCC3VertexNormals _vertexNormals;
        LCC3VertexIndices _vertexIndices;

        #region Properties

        // Instance properties

        public LCC3VertexIndices VertexIndices
        {
            get { return _vertexIndices; }
        }

        public bool HasVertexNormals
        {
            get { return (_vertexNormals != null); }
        }

        public bool HasVertexTangents
        {
            get { return false; }
        }

        public bool HasVertexBitangents
        {
            get { return false; }
        }

        public bool HasVertexColors
        {
            get { return false; }
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
            get { return false; }
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
            return null;
        }

        //-(CC3VertexArray*) vertexArrayForSemantic: (GLenum) semantic at: (GLuint) semanticIndex
    }
}

