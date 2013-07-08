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
    public class CC3TransformAction : CC3TranslationAction
    {
        // Instance fields

        private CC3Vector _scaleChange;
        private CC3AnimatableRotation _rotationChangeRelativeToAnchor;
        private CC3Vector _rotationAnchorPointRelativeToPosition;

        #region Properties

        // Instance properties

        public CC3Vector ScaleChange
        {
            get { return _scaleChange; }
        }

        public CC3Vector RotationAnchorPointRelativeToPosition
        {
            get { return _rotationAnchorPointRelativeToPosition; }
        }

        #endregion Properties


        #region Constructors

        public CC3TransformAction(CC3Vector translationChange, 
                                  CC3Vector scaleChange, 
                                  CC3Vector4 axisAndRotationInDegreesChangeRelativeToAnchor,
                                  CC3Vector rotationAnchorPointRelativeToPosition)
            : base (translationChange)
        {
            _scaleChange = scaleChange;
            _rotationChangeRelativeToAnchor = new CC3AnimatableRotation(axisAndRotationInDegreesChangeRelativeToAnchor);
            _rotationAnchorPointRelativeToPosition = rotationAnchorPointRelativeToPosition;
        }

        #endregion Constructors


        #region Getting transform changes for a subinterval of time

        internal CC3Vector IncrementalScaleChange(float timeElapsedFraction, float timeIncrementFraction)
        {
            return _scaleChange * timeIncrementFraction;
        }

        internal CC3Quaternion IncrementalRotationChangeRelativeToAnchor(float timeElapsedFraction, float timeIncrementFraction)
        {
            return _rotationChangeRelativeToAnchor.IncrementalRotationChange(timeElapsedFraction, timeIncrementFraction);
        }

        #endregion Getting transform changes for a subinterval of time
    }
}

