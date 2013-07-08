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
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Cocos3D
{
    internal class CC3AnimatableRotation
    {
        // Static fields

        /* 
        Quaternions have no concept of angle winding so if you want to animate a
        rotation by 360.0 you need to break it up into smaller angles < 180.0
        */
        private const float _maxQuaternionRotationInDegrees = 179.0f;

        // Instance fields

        private CC3Vector _rotationAxis;
        private float _rotationInDegrees;
        private List<RotationTimingInfo> _listOfRotationTimingInfo;

        #region Private classes

        private class RotationTimingInfo
        {
            private CC3Quaternion _quaternion;
            private float _startingTimeFraction;
            private float _durationTimeFraction;
            private float _rotationAmountInDegrees;

            internal CC3Quaternion Quaternion
            {
                get { return _quaternion; }
            }

            internal float StartingTimeFraction
            {
                get { return _startingTimeFraction; }
                set { _startingTimeFraction = value; }
            }

            internal float DurationTimeFraction
            {
                get { return _durationTimeFraction; }
                set { _durationTimeFraction = value; }
            }

            internal float RotationAmountInDegrees
            {
                get { return _rotationAmountInDegrees; }
                set { _rotationAmountInDegrees = value; }
            }

            internal RotationTimingInfo(CC3Quaternion quaternion) 
            : this(quaternion, 0.0f, 0.0f, 0.0f)
            {

            }

            internal RotationTimingInfo(CC3Quaternion quaternion, 
                                        float startingTimeFraction, 
                                        float durationTimeFraction,
                                        float rotationAmountInDegrees) 
            {
                _quaternion = quaternion;
                _startingTimeFraction = startingTimeFraction;
                _durationTimeFraction = durationTimeFraction;
                _rotationAmountInDegrees = rotationAmountInDegrees;
            }
        }

        #endregion Private classes

        #region Constructors

        internal CC3AnimatableRotation(CC3Vector rotationAxis, float rotationInDegrees)
        {
            _rotationAxis = rotationAxis.NormalizedVector();
            _rotationInDegrees = rotationInDegrees;
            _listOfRotationTimingInfo = new List<RotationTimingInfo>();

            this.SetupQuaternionComponents();
            this.SetupQuaternionTiming();
        }

        internal CC3AnimatableRotation(CC3Vector4 axisAndRotationInDegrees) 
            : this(axisAndRotationInDegrees.TruncateToCC3Vector(), axisAndRotationInDegrees.W)
        {

        }

        private void SetupQuaternionComponents()
        {
            float absRotation = Math.Abs(_rotationInDegrees);
            float rotationDirection = _rotationInDegrees >= 0.0f ? 1.0f : - 1.0f;
            float maxQuaternionRotationInRadians 
                = CC3AnimatableRotation._maxQuaternionRotationInDegrees * rotationDirection;

            CC3Quaternion maxQuaternionComponent
                = CC3Quaternion.CreateFromAxisAngle(_rotationAxis, 
                                                    MathHelper.ToRadians(maxQuaternionRotationInRadians));

            while (absRotation > CC3AnimatableRotation._maxQuaternionRotationInDegrees)
            {
                _listOfRotationTimingInfo.Add(new RotationTimingInfo(maxQuaternionComponent));
                absRotation -= CC3AnimatableRotation._maxQuaternionRotationInDegrees;
            }

            CC3Quaternion remainderQuaternionComponent 
                = CC3Quaternion.CreateFromAxisAngle(_rotationAxis, 
                                                    MathHelper.ToRadians(absRotation * rotationDirection));

            _listOfRotationTimingInfo.Add(new RotationTimingInfo(remainderQuaternionComponent));
        }

        // For now, we're assuming linear time interpolation so that rotation occurs at uniform rate
        private void SetupQuaternionTiming()
        {
            float absRotation = Math.Abs(_rotationInDegrees);
            float timeFractionDoingMaxQuaternionRotations = 0.0f;
            float maxQuaternionRotationTimeIncrementFraction = 0.0f;
            float numerOfMaxQuaternionRotationComponents = _listOfRotationTimingInfo.Count - 1;

            // Determining the time increments (as a fraction between 0.0f and 1.0f) that
            // sould be applied to each of the maximal quaternion rotations (if any)
            if(absRotation > 0.0f && numerOfMaxQuaternionRotationComponents > 0)
            {
                timeFractionDoingMaxQuaternionRotations 
                    = (numerOfMaxQuaternionRotationComponents * CC3AnimatableRotation._maxQuaternionRotationInDegrees) / absRotation;

                maxQuaternionRotationTimeIncrementFraction 
                    = timeFractionDoingMaxQuaternionRotations / numerOfMaxQuaternionRotationComponents;
            }

            // Setting the start time (as a fraction between 0.0f and 1.0f) when
            // each of the maximal quaternion rotations begin (if any)
            float startTimeOfRotationFraction = 0.0f;

            foreach (RotationTimingInfo rotationTimingInfo in _listOfRotationTimingInfo)
            {
                rotationTimingInfo.StartingTimeFraction = startTimeOfRotationFraction;
                rotationTimingInfo.DurationTimeFraction = maxQuaternionRotationTimeIncrementFraction;
                rotationTimingInfo.RotationAmountInDegrees = CC3AnimatableRotation._maxQuaternionRotationInDegrees;

                startTimeOfRotationFraction += maxQuaternionRotationTimeIncrementFraction;
            }

            // Finally, set start time of remainder rotation
            RotationTimingInfo remainderRotationTimingInfo = _listOfRotationTimingInfo[_listOfRotationTimingInfo.Count - 1];
            remainderRotationTimingInfo.StartingTimeFraction = timeFractionDoingMaxQuaternionRotations;
            remainderRotationTimingInfo.DurationTimeFraction = 1.0f - remainderRotationTimingInfo.StartingTimeFraction;
            remainderRotationTimingInfo.RotationAmountInDegrees = (1.0f - timeFractionDoingMaxQuaternionRotations) * _rotationInDegrees;
        }

        #endregion Constructors


        internal CC3Quaternion IncrementalRotationChange(float timeElapsedFraction, float timeIncrementFraction)
        {
            float remainingTimeIncrementFraction = timeIncrementFraction;
            CC3Quaternion combinedRotation = CC3Quaternion.CC3QuaternionIdentity;

            foreach (RotationTimingInfo rotationTimingInfo in _listOfRotationTimingInfo)
            {
                if (remainingTimeIncrementFraction <= 0.0f)
                    break;

                if (rotationTimingInfo.StartingTimeFraction + rotationTimingInfo.DurationTimeFraction >= timeElapsedFraction)
                {
                    float amountOfTimeIncrementFractionToConsume 
                        = Math.Min(rotationTimingInfo.StartingTimeFraction  - timeElapsedFraction + rotationTimingInfo.DurationTimeFraction, 
                                   remainingTimeIncrementFraction);

                    float fractionOfRotationToPerform 
                        = Math.Min(1.0f, amountOfTimeIncrementFractionToConsume / rotationTimingInfo.DurationTimeFraction);

                    combinedRotation *= CC3Quaternion.CC3QuaternionSlerp(CC3Quaternion.CC3QuaternionIdentity,
                                                                         rotationTimingInfo.Quaternion,
                                                                         fractionOfRotationToPerform); 

                    remainingTimeIncrementFraction -= amountOfTimeIncrementFractionToConsume;
                }

            }

            return combinedRotation;
        }
    }
}

