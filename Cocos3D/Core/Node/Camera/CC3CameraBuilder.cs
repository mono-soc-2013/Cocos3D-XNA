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
    public abstract class CC3CameraBuilder
    {
        // Instance fields

        protected CC3Vector _cameraPostion;
        protected CC3Vector _cameraTarget;
        protected float _cameraNearClippingDistance;
        protected float _cameraFarClippingDistance;

        #region Constructors

        public CC3CameraBuilder()
        {
            _cameraPostion = CC3Vector.CC3VectorZero;
            _cameraTarget = CC3Vector.CC3VectorZero;
            _cameraNearClippingDistance = CC3Camera.DefaultNearClippingDistance;
            _cameraFarClippingDistance = CC3Camera.DefaultFarClippingDistance;
        }

        #endregion Constructors


        #region Building camera methods

        public CC3Camera Build()
        {
            // Return type and implementation set by subclasses
            return null;
        }

        #endregion Building camera methods


        #region Setting up camera view matrix

        // Position of camera methods

        public CC3CameraBuilder PositionAtPoint(CC3Vector cameraPosition)
        {
            _cameraPostion = cameraPosition;
            return this;
        }
        
        public CC3CameraBuilder PositionAtNode(CC3Node node, CC3Vector offset)
        {
            _cameraPostion = node.WorldPosition + offset;
            return this;
        }

        public CC3CameraBuilder PositionAtNode(CC3Node node)
        {
            return this.PositionAtNode(node, CC3Vector.CC3VectorZero);
        }


        // Looking methods

        public CC3CameraBuilder LookingAtPoint(CC3Vector cameraTarget)
        {
            _cameraTarget = cameraTarget;
            return this;
        }

        public CC3CameraBuilder LookingInDirection(CC3Vector worldDirection)
        {
            // This assumes camera position has been set prior
            _cameraTarget = _cameraPostion + worldDirection.NormalizedVector();
            return this;
        }

        public CC3CameraBuilder LookingAtNode(CC3Node node, CC3Vector offset)
        {
            _cameraTarget = node.WorldPosition + offset;
            return this;
        }

        public CC3CameraBuilder LookingAtNode(CC3Node node)
        {
            return this.LookingAtNode(node, CC3Vector.CC3VectorZero);
        }

        #endregion Setting up camera view matrix


        #region Setting up camera projection matrix

        public CC3CameraBuilder WithNearAndFarClippingDistances(float nearClippingDistance, float farClippingDistance)
        {
            _cameraNearClippingDistance = nearClippingDistance;
            _cameraFarClippingDistance = farClippingDistance;

            return this;
        }

        #endregion Setting up camera projection matrix

    }
}

