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
using Microsoft.Xna.Framework;

namespace Cocos3D
{
    public class CC3CameraOrthographic : CC3Camera
    {
        // Instance fields

        private float _viewWidth;
        private float _viewHeight;

        #region Properties

        // Instance properties

        public float ViewWidth
        {
            get { return _viewWidth; }
            set
            {
                _viewWidth = value;
                this.ShouldUpdateProjectionMatrix();
            }
        }

        public float ViewHeight
        {
            get { return _viewHeight; }
            set
            {
                _viewHeight = value;
                this.ShouldUpdateProjectionMatrix();
            }
        }

        #endregion Properties


        #region Static camera projection calculation methods

        private static CC3Matrix CameraOrthographicProjectionMatrix(float cameraViewWidth, float cameraViewHeight, 
                                                                    float cameraNearClippingDistance, float cameraFarClippingDistance)
        {
            Matrix xnaProjMatrix 
                = Matrix.CreateOrthographic(cameraViewWidth, cameraViewHeight, 
                                            cameraNearClippingDistance, cameraFarClippingDistance);

            return new CC3Matrix(xnaProjMatrix);
        }

        #endregion Static camera projection calculation methods


        #region Constructors

        // Users should only create camera via corresponding builder class
        internal CC3CameraOrthographic(CC3Vector cameraPosition, CC3Vector cameraTarget, 
                                      float viewWidth, float viewHeight, 
                                      float nearClippingDistance, float farClippingDistance)
            : base(cameraPosition, cameraTarget)
        {
            _viewWidth = viewWidth;
            _viewHeight = viewHeight;
            _nearClippingDistance = nearClippingDistance;
            _farClippingDistance = farClippingDistance;

            this.UpdateProjectionMatrix();
        }

        #endregion Constructors


        #region Updating projection matrix

        // Subclasses should not call this directly
        // Instead call ShouldUpdateProjectionMatrix()
        protected override void UpdateProjectionMatrix()
        {
            _projectionMatrix 
                = CC3CameraOrthographic.CameraOrthographicProjectionMatrix(_viewWidth, _viewHeight, 
                                                                         _nearClippingDistance, _farClippingDistance); 
        }

        #endregion Updating projection matrix
    }
}

