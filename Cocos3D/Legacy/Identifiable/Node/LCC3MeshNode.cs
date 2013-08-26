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
    public class LCC3MeshNode : LCC3Node
    {
        // Instance fields

        private LCC3Mesh _mesh;
        private LCC3Material _material;

        #region Properties

        public LCC3Mesh Mesh
        {
            get { return _mesh; }
            set { _mesh = value; }
        }

        public LCC3Material Material
        {
            get { return _material; }
            set { _material = value; }
        }

        public LCC3NormalScaling EffectiveNormalScalingMethod
        {
            get { return LCC3NormalScaling.CC3NormalScalingNone; }
        }

        public override bool ShouldUseLighting
        {
            get { return (_material != null) ? _material.ShouldUseLighting : false; }
        }

        #endregion Properties


        public LCC3MeshNode()
        {
        }
    }
}

