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
    public class LCC3NodeVisitor
    {
        // ivars

        LCC3Node _startingNode;
        LCC3Node _currentNode;

        #region Properties

        public LCC3Node StartingNode
        {
            get { return _startingNode; }
            set { _startingNode = value; }
        }

        public LCC3Node CurrentNode
        {
            get { return _currentNode; }
            set { _currentNode = value; }
        }

        public LCC3MeshNode CurrentMeshNode
        {
            get { return _currentNode as LCC3MeshNode; }
        }

        public LCC3Material CurrentMaterial
        {
            get { return this.CurrentMeshNode.Material; }
        }

        public LCC3Mesh CurrentMesh
        {
            get { return this.CurrentMeshNode.Mesh; }
        }

        public LCC3Scene Scene
        {
            get { return _startingNode.Scene; }
        }

        #endregion Properties


        #region Constructors

        public LCC3NodeVisitor()
        {
        }

        #endregion Constructors


        #region Accessing node contents

        public LCC3Light LightAtIndex(uint index)
        {
            LCC3Light[] lights = this.Scene.Lights;
            if (index < (uint)lights.Length)
            {
                return lights[(int)index];
            }

            return null;
        }

        #endregion Accessing node contents
    }
}

