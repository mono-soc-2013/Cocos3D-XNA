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
    public abstract class CC3CollectionActionRunner : CC3ActionRunner
    {
        // Instance fields

        protected List<CC3ActionRunner> _listOfActionRunners;

        #region Constructors

        public CC3CollectionActionRunner(float actionDuration)
            : base(actionDuration)
        {
            _listOfActionRunners = new List<CC3ActionRunner>();
        }

        #endregion Constructors

        #region Adding action methods

        protected virtual void AddActionRunner(CC3ActionRunner actionRunner)
        {
            _listOfActionRunners.Add(actionRunner);
        }

        public void AddActionWithTarget(CC3CameraPerspectiveAction action, CC3CameraPerspective target, float actionDuration)
        {
            CC3CameraPerspectiveActionRunner actionRunner = new CC3CameraPerspectiveActionRunner(action, target, actionDuration);
            this.AddActionRunner(actionRunner);
        }

        #endregion Adding action methods

    }
}

