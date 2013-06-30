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
    public class CC3TransformActionRunner : CC3ActionRunner
    {
        // Instance fields

        private CC3TransformAction _transformAction;
        private CC3Node _targetNode;


        #region Constructors

        public CC3TransformActionRunner(CC3TransformAction transformAction, CC3Node targetNode, float actionDuration)
            : base(actionDuration)
        {
            _transformAction = transformAction;
            _targetNode = targetNode;
        }

        #endregion Constructors


        #region Running action methods

        protected override void UpdateAction(float timeElapsedFraction, float timeIncrementFraction)
        {
            CC3Vector incrementalTranslationChange = 
                _transformAction.IncrementalTranslationChange(timeElapsedFraction, timeIncrementFraction);
            CC3Vector incrementalScaleChange = 
                _transformAction.IncrementalScaleChange(timeElapsedFraction, timeIncrementFraction);
            CC3Quaternion incrementalRotationChange = 
                _transformAction.IncrementalRotationChangeRelativeToAnchor(timeElapsedFraction, timeIncrementFraction);
            CC3Vector rotationAnchorPoint = _transformAction.RotationAnchorPointRelativeToPosition;

            _targetNode.IncrementallyUpdateWorldTransform(incrementalTranslationChange, 
                                                          incrementalScaleChange, 
                                                          incrementalRotationChange,
                                                          rotationAnchorPoint);
        }

        #endregion Running action methods
       
    }
}

