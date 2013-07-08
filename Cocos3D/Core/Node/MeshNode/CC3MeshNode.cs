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
    public class CC3MeshNode : CC3DrawableNode
    {
        // Instance fields

        private CC3Mesh _mesh;
        private CC3Material _material;

        #region Properties

        public CC3Mesh Mesh
        {
            get { return _mesh; }
        }

        public CC3Material Material
        {
            get { return _material; }
        }

        #endregion Properties


        #region Constructors

        public CC3MeshNode(CC3GraphicsContext graphicsContext, CC3Mesh mesh, CC3Material material) 
            : base(graphicsContext)
        {
            _mesh = mesh;
            _material = material;
        }

        #endregion Constructors


        #region Drawing methods

        public override void Draw()
        {
            _graphicsContext.BindMesh(_mesh);
            _graphicsContext.BindMaterial(_material);
        }

        #endregion Drawing methods
    }
}

