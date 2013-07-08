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
    public class CC3CameraPerspectiveActionBuilder : CC3CameraActionBuilder
    {
        // Instance fields

        float _cameraFieldOfViewInRadiansChange;


        #region Constructors

        public CC3CameraPerspectiveActionBuilder() : base()
        {

        }

        #endregion Constructors


        #region Resetting builder

        public override void Reset()
        {
            base.Reset();

            _cameraFieldOfViewInRadiansChange = 0.0f;
        }

        #endregion Resetting builder


        #region Building camera action methods

        public new CC3CameraPerspectiveAction Build()
        {
            return new CC3CameraPerspectiveAction(_translationChange, 
                                                  _cameraTargetTranslationChange, 
                                                  _cameraAxisAndRotationInDegreesChangeRelativeToCameraTarget, 
                                                  _cameraFieldOfViewInRadiansChange);
        }

        #endregion Building camera action methods


        #region Zooming camera methods

        private CC3CameraPerspectiveActionBuilder SetCameraFieldOfViewChangeInRadians(float fieldOfViewChangeInRadians)
        {
            _cameraFieldOfViewInRadiansChange = fieldOfViewChangeInRadians;
            return this;
        }

        public CC3CameraPerspectiveActionBuilder SetCameraFieldOfViewChangeInDegrees(float fieldOfViewChangeInDegrees)
        {
            return this.SetCameraFieldOfViewChangeInRadians(MathHelper.ToRadians(fieldOfViewChangeInDegrees));
        }

        public CC3CameraPerspectiveActionBuilder ZoomInByFactorRelativeToSourceCamera(float zoomInFactor, CC3CameraPerspective camera)
        {
            if (zoomInFactor > 1.0f)
            {
                float cameraFieldOfViewInRadians = camera.FieldOfView;
                this.SetCameraFieldOfViewChangeInRadians(-(zoomInFactor - 1.0f) * cameraFieldOfViewInRadians);
            }

            return this;
        }

        public CC3CameraPerspectiveActionBuilder ZoomOutByFactorRelativeToSourceCamera(float zoomOutFactor, CC3CameraPerspective camera)
        {
            if (zoomOutFactor > 1.0f)
            {
                float cameraFieldOfViewInRadians = camera.FieldOfView;
                this.SetCameraFieldOfViewChangeInRadians((zoomOutFactor - 1.0f) * cameraFieldOfViewInRadians);
            }

            return this;
        }

        #endregion Zooming camera methods
    }
}

