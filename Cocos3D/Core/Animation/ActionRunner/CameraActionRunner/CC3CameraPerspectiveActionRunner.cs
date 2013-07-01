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
    public class CC3CameraPerspectiveActionRunner : CC3CameraActionRunner
    {
        // Instance fields

        private CC3CameraPerspectiveAction _perspectiveCameraAction;
        private CC3CameraPerspective _targetPerspectiveCamera;

       
        public CC3CameraPerspectiveActionRunner(CC3CameraPerspectiveAction perspectiveCameraAction, 
                                                CC3CameraPerspective targetPerspectiveCamera,
                                                float actionDuration) : base(actionDuration)
        {
            _perspectiveCameraAction = perspectiveCameraAction;
            _targetPerspectiveCamera = targetPerspectiveCamera;
        }

        protected override void UpdateAction(float timeElapsedFraction, float timeIncrementFraction)
        {
            CC3Vector incrementalCameraTranslationChange = 
                _perspectiveCameraAction.IncrementalTranslationChange(timeElapsedFraction, timeIncrementFraction);

            CC3Quaternion incrementalCameraRotationChangeRelativeToTarget =
                _perspectiveCameraAction.IncrementalCameraRotationChangeRelativeToCameraTarget(timeElapsedFraction, timeIncrementFraction);

            CC3Vector incrementalCameraTargetTranslationChange = 
                _perspectiveCameraAction.IncrementalCameraTargetTranslationChange(timeElapsedFraction, timeIncrementFraction);

            _targetPerspectiveCamera.IncrementallyUpdateViewTransform(incrementalCameraTranslationChange, 
                                                                      incrementalCameraTargetTranslationChange,
                                                                      incrementalCameraRotationChangeRelativeToTarget);
        }
    }
}

