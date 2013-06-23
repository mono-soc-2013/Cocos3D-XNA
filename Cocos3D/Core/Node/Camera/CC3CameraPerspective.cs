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
    public class CC3CameraPerspective : CC3Camera
    {
        // Static fields

        private const float _defaultFieldOfView = 45.0f;
        private const float _defaultAspectRatio = 1.0f;

        // Instance fields

        private float _fieldOfView;
        private float _aspectRatio;


        #region Properties

        // Static properties

        public static float DefaultFieldOfView
        {
            get { return _defaultFieldOfView; }
        }

        public static float DefaultAspectRatio
        {
            get { return _defaultAspectRatio; }
        }

        // Instance properties

        public float FieldOfView
        {
            get { return _fieldOfView; }
            set 
            {   _fieldOfView = value; 
                this.ShouldUpdateProjectionMatrix(); 
            }
        }

        #endregion Properties


        #region Static camera projection calculation methods

        private static CC3Matrix CameraPerspectiveProjectionMatrix(float cameraFieldOfView, float cameraAspectRatio, 
                                                                   float cameraNearClippingDistance, float cameraFarClippingDistance)
        {
            Matrix xnaProjMatrix 
                = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(cameraFieldOfView), cameraAspectRatio, 
                                                      cameraNearClippingDistance, cameraFarClippingDistance);

            return new CC3Matrix(xnaProjMatrix);
        }

        #endregion Static camera projection calculation methods


        #region Constructors

        // Users should only create camera via corresponding builder class
        internal CC3CameraPerspective(CC3Vector cameraPosition, CC3Vector cameraTarget, 
                                      float fieldOfView, float aspectRatio, 
                                      float nearClippingDistance, float farClippingDistance)
            : base(cameraPosition, cameraTarget)
        {
            _fieldOfView = fieldOfView;
            _aspectRatio = aspectRatio;
            _nearClippingDistance = nearClippingDistance;
            _farClippingDistance = farClippingDistance;

            this.UpdateProjectionMatrix();
        }

        #endregion Constructors


        #region Updating projection matrix

        // Subclasses should not call this directly
        // Instead call ShouldUpdateProjectionMatrix() inherited from base
        protected override void UpdateProjectionMatrix()
        {
            _projectionMatrix 
                = CC3CameraPerspective.CameraPerspectiveProjectionMatrix(_fieldOfView, _aspectRatio, 
                                                                         _nearClippingDistance, _farClippingDistance);
        }

        #endregion Updating projection matrix
    }
}

