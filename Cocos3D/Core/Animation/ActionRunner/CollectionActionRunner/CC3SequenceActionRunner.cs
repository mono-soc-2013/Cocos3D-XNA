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

namespace Cocos3D
{
    public class CC3SequenceActionRunner : CC3CollectionActionRunner
    {
        // Instance fields

        private float _startingTimeFractionOfNextActionToBeAdded;
        private List<float> _listOfActionStartingTimeFractions;
        private List<float> _listOfActionDurationFractions;

        #region Constructors

        public CC3SequenceActionRunner(float actionDuration)
            : base(actionDuration)
        {
            _listOfActionStartingTimeFractions = new List<float>();
            _listOfActionDurationFractions = new List<float>();
            _startingTimeFractionOfNextActionToBeAdded = 0.0f;
        }

        #endregion Constructors


        #region Adding action methods

        protected override void AddActionRunner(CC3ActionRunner actionRunner)
        {
            float actionDurationFraction = actionRunner.ActionDuration / this.ActionDuration;;
            _listOfActionStartingTimeFractions.Add(_startingTimeFractionOfNextActionToBeAdded);
            _listOfActionDurationFractions.Add(actionDurationFraction);

            _startingTimeFractionOfNextActionToBeAdded += actionDurationFraction;
       
            base.AddActionRunner(actionRunner);
        }

        #endregion Adding action methods


        #region Running action methods

        protected internal override void UpdateAction(float timeElapsedFraction, float timeIncrementFraction)
        {
            float remainingTimeIncrementFraction = timeIncrementFraction;

            for (int i=0; i< _listOfActionStartingTimeFractions.Count; i++)
            {
                if (remainingTimeIncrementFraction <= 0.0f)
                    break;

                float actionStartingTimeFraction = _listOfActionStartingTimeFractions[i];
                float actionDurationFraction = _listOfActionDurationFractions[i];

                if (actionStartingTimeFraction + actionDurationFraction >= timeElapsedFraction)
                {
                    float amountOfTimeIncrementFractionToConsume 
                        = Math.Min(actionStartingTimeFraction - timeElapsedFraction + actionDurationFraction, 
                                   remainingTimeIncrementFraction);

                    _listOfActionRunners[i].UpdateAction(timeElapsedFraction - actionStartingTimeFraction, 
                                                         (1/actionDurationFraction) * amountOfTimeIncrementFractionToConsume);

                    remainingTimeIncrementFraction -= amountOfTimeIncrementFractionToConsume;
                }
            }
        }

        #endregion Running action methods
    }
}

