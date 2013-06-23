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
    public abstract class CC3Camera : CC3Node
    {
        // Static fields

        private const float _defaultNearClippingDistance = 1.0f;
        private const float _defaultFarClippingDistance = 1000.0f;

        // Instance fields

        private ICC3CameraListener _cameraListenter;

        private CC3Matrix _viewMatrix;
        private CC3Vector _cameraTarget;

        protected CC3Matrix _projectionMatrix;
        protected float _nearClippingDistance;
        protected float _farClippingDistance;

        #region Properties

        // Static properties

        public static float DefaultNearClippingDistance
        {
            get { return _defaultNearClippingDistance; }
        }

        public static float DefaultFarClippingDistance
        {
            get { return _defaultFarClippingDistance; }
        }

        // Instance properties

        public float NearClippingDistance
        {
            get { return _nearClippingDistance; }
            set 
            {   _nearClippingDistance = value;
                this.ShouldUpdateProjectionMatrix(); 
            }
        }

        public float FarClippingDistance
        {
            get { return _farClippingDistance; }
            set 
            {   _farClippingDistance = value;
                this.ShouldUpdateProjectionMatrix(); 
            }
        }

        internal ICC3CameraListener CameraListener
        {
            get { return _cameraListenter; }
            set { _cameraListenter = value; }
        }

        internal CC3Matrix ViewMatrix
        {
            get { return _viewMatrix; }
        }

        internal CC3Matrix ProjectionMatrix
        {
            get { return _projectionMatrix; }
        }

        #endregion Properties


        #region Static camera view calculation methods

        private static CC3Matrix CameraViewMatrix(CC3Vector cameraPosition, CC3Vector cameraTarget)
        {
            Matrix xnaViewMatrix = Matrix.CreateLookAt(cameraPosition.XnaVector, 
                                                       cameraTarget.XnaVector, 
                                                       CC3Vector.CC3VectorUp.XnaVector);
            
            return new CC3Matrix(xnaViewMatrix);
        }

        #endregion Static camera view calculation methods


        #region Constructors

        // Users should only create camera via corresponding builder class
        internal CC3Camera(CC3Vector cameraPosition, CC3Vector cameraTarget) : base()
        {
            _worldPosition = cameraPosition;
            _cameraTarget = cameraTarget;

            this.UpdateViewMatrix();
        }

        #endregion Constructors


        #region Updating view and projection matrices

        protected void ShouldUpdateProjectionMatrix()
        {
            this.UpdateProjectionMatrix();

            _cameraListenter.CameraProjectionMatrixDidChange(this);
        }

        protected abstract void UpdateProjectionMatrix();


        private void ShouldUpdateViewMatrix()
        {
            this.UpdateViewMatrix();

            _cameraListenter.CameraViewMatrixDidChange(this);
        }

        private void UpdateViewMatrix()
        {
            _viewMatrix = CC3Camera.CameraViewMatrix(_worldPosition, _cameraTarget);
        }

        #endregion Updating view and projection matrices
    }
}

