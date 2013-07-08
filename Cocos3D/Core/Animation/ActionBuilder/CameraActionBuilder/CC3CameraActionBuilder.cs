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
    public abstract class CC3CameraActionBuilder : CC3TranslationActionBuilder
    {
        protected CC3Vector _cameraTargetTranslationChange;
        protected CC3Vector4 _cameraAxisAndRotationInDegreesChangeRelativeToCameraTarget;


        #region Constructors

        public CC3CameraActionBuilder() : base()
        {

        }

        #endregion Constructors


        #region Resetting builder

        public override void Reset()
        {
            base.Reset();

            _cameraTargetTranslationChange = CC3Vector.CC3VectorZero;
            _cameraAxisAndRotationInDegreesChangeRelativeToCameraTarget = new CC3Vector4(CC3Vector.CC3VectorUp, 0.0f);
        }

        #endregion Resetting builder


        #region Camera rotation methods

        public CC3CameraActionBuilder RotateCameraAroundAxisRelativeToTargetByDegrees(CC3Vector rotationAxis, 
                                                                                      float rotationInDegrees)
        {
            _cameraAxisAndRotationInDegreesChangeRelativeToCameraTarget = new CC3Vector4(rotationAxis, rotationInDegrees);

            return this;
        }

        #endregion Camera rotation methods


        #region Camera panning methods

        public CC3CameraActionBuilder PanCameraByTranslationOffset(CC3Vector panningTranslationOffset)
        {
            _translationChange = panningTranslationOffset;
            _cameraTargetTranslationChange = panningTranslationOffset;
            return this;
        }

        public CC3CameraActionBuilder PanCameraLeftByAmount(float offsetAmount)
        {
            return this.PanCameraByTranslationOffset(new CC3Vector(-offsetAmount, 0.0f, 0.0f));
        }

        public CC3CameraActionBuilder PanCameraRightByAmount(float offsetAmount)
        {
            return this.PanCameraByTranslationOffset(new CC3Vector(offsetAmount, 0.0f, 0.0f));
        }

        public CC3CameraActionBuilder PanCameraUpByAmount(float offsetAmount)
        {
            return this.PanCameraByTranslationOffset(new CC3Vector(0.0f, offsetAmount, 0.0f));
        }

        public CC3CameraActionBuilder PanCameraDownByAmount(float offsetAmount)
        {
            return this.PanCameraByTranslationOffset(new CC3Vector(0.0f, -offsetAmount, 0.0f));
        }

        public CC3CameraActionBuilder PanCameraForwardByAmount(float offsetAmount)
        {
            return this.PanCameraByTranslationOffset(new CC3Vector(0.0f, 0.0f, -offsetAmount));
        }

        public CC3CameraActionBuilder PanCameraBackwardByAmount(float offsetAmount)
        {
            return this.PanCameraByTranslationOffset(new CC3Vector(0.0f, 0.0f, offsetAmount));
        }

        #endregion Camera panning methods
    }
}

