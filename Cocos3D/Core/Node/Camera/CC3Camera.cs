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

        private ICC3CameraObserver _cameraObserver;

        private CC3Matrix _viewMatrix;
        private CC3Vector _cameraTarget;
        private CC3Vector _cameraUpDirection;
        private CC3Quaternion _cameraRotationChangeRelativeToTargetNeededToUpdate;

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

        internal ICC3CameraObserver CameraObserver
        {
            get { return _cameraObserver; }
            set { _cameraObserver = value; }
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


        #region Constructors

        // Users should only create camera via corresponding builder class
        internal CC3Camera(CC3Vector cameraPosition, CC3Vector cameraTarget) : base(cameraPosition)
        {
            _cameraTarget = cameraTarget;
            _cameraRotationChangeRelativeToTargetNeededToUpdate = CC3Quaternion.CC3QuaternionIdentity;

            float distBetweenCameraAndTarget = (cameraPosition - cameraTarget).Length();
            float angleBetweenCameraAndTarget = (float)Math.Asin((cameraTarget.Y - cameraPosition.Y) / distBetweenCameraAndTarget);

            _cameraUpDirection 
                    = CC3Vector.CC3VectorUp.RotatedVector(CC3Quaternion.CreateFromXAxisRotation(angleBetweenCameraAndTarget));

            this.UpdateViewMatrix();
        }

        #endregion Constructors


        #region Updating world, view and projection matrices

        // Update world matrix methods
        
        protected override void ShouldUpdateWorldMatrix()
        {
            // Instead of updating world, we update the view
            this.ShouldUpdateViewMatrix();
        }

        // Update view matrix methods

        internal void IncrementallyUpdateViewTransform(CC3Vector cameraTranslationChange, 
                                                       CC3Vector cameraTargetTranslationChange,
                                                       CC3Quaternion cameraRotationChangeRelativeToTarget)
        {
            this.WorldTranslationChangeNeededToUpdate = cameraTranslationChange;
            _cameraRotationChangeRelativeToTargetNeededToUpdate = cameraRotationChangeRelativeToTarget;
            _cameraTarget += cameraTargetTranslationChange;
            _cameraUpDirection = _cameraUpDirection.RotatedVector(cameraRotationChangeRelativeToTarget);

            this.ShouldUpdateViewMatrix();

            foreach (ICC3NodeTransformObserver transformObserver in _listOfNodeTransformObservers)
            {
                transformObserver.ObservedNodeWorldTransformDidChange(this, 
                                                                      cameraTranslationChange, 
                                                                      CC3Vector.CC3VectorUnitCube, 
                                                                      _cameraRotationChangeRelativeToTargetNeededToUpdate,
                                                                      _cameraTarget - this.WorldPosition - cameraTranslationChange);
            }
        }

        private void ShouldUpdateViewMatrix()
        {
            this.UpdateViewMatrix();

            _cameraObserver.CameraViewMatrixDidChange(this);
        }

        private void UpdateViewMatrix()
        {
            _viewMatrix 
                = CC3Matrix.CreateCameraViewMatrix(this.WorldPosition + this.WorldTranslationChangeNeededToUpdate, 
                                                   _cameraTarget, 
                                                   _cameraRotationChangeRelativeToTargetNeededToUpdate,
                                                   _cameraUpDirection);
            _worldMatrix = _viewMatrix.Inverse();

            this.FinishedUpdatingViewMatrix();
        }

        private void FinishedUpdatingViewMatrix()
        {
            _cameraRotationChangeRelativeToTargetNeededToUpdate = CC3Quaternion.CC3QuaternionIdentity;

            base.FinishedUpdatingWorldMatrix();
        }

        // Update projectiom matrix methods

        protected void ShouldUpdateProjectionMatrix()
        {
            this.UpdateProjectionMatrix();

            _cameraObserver.CameraProjectionMatrixDidChange(this);
        }

        protected abstract void UpdateProjectionMatrix();

        #endregion Updating view and projection matrices
    }
}

