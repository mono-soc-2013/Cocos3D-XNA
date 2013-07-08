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
    public class CC3CameraPerspectiveAction : CC3CameraAction
    {
        // Instance fields

        private float _cameraFieldOfViewInRadiansChange;


        #region Properties

        // Instance properties

        public float CameraFieldOfViewInRadiansChange 
        {
            get { return _cameraFieldOfViewInRadiansChange; }
        }

        #endregion Properties


        #region Constructors

        public CC3CameraPerspectiveAction(CC3Vector cameraTranslationChange, 
                                          CC3Vector cameraTargetTranslationChange,
                                          CC3Vector4 cameraAxisAndRotationInDegreesChangeRelativeToCameraTarget,
                                          float cameraFieldOfViewInRadiansChange) 
            : base(cameraTranslationChange, 
                   cameraTargetTranslationChange,
                   cameraAxisAndRotationInDegreesChangeRelativeToCameraTarget)
        {
            _cameraFieldOfViewInRadiansChange = cameraFieldOfViewInRadiansChange;
        }

        #endregion Constructors


        #region Getting camera view transform changes for a subinterval of time

        internal float IncrementalCameraFieldOfViewInRadiansChange(float timeElapsedFraction, float timeIncrementFraction)
        {
            return _cameraFieldOfViewInRadiansChange * timeIncrementFraction;
        }

        #endregion Getting camera view transform changes for a subinterval of time
    }
}

