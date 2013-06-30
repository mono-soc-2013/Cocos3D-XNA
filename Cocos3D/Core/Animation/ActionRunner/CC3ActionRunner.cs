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
using Cocos2D;

namespace Cocos3D
{
    public abstract class CC3ActionRunner
    {
        // Instance fields

        private ProxyCCActionInterval _proxy2dCCActionInterval;
        private ProxyCCTargetNode _proxy2dCCNodeActionTarget;

        private readonly float _actionDuration;
        private float _actionTimeElapsed;

        #region Properties

        public float ActionDuration
        {
            get { return _actionDuration; }
        }

        #endregion Properties

        #region Constructors

        protected CC3ActionRunner(float actionDuration, ProxyCCTargetNode proxy2dCCNodeActionTarget)
        {
            _proxy2dCCActionInterval = new ProxyCCActionInterval(this, actionDuration);
            _proxy2dCCNodeActionTarget = proxy2dCCNodeActionTarget;

            _actionDuration = actionDuration;
        }

        internal CC3ActionRunner(float actionDuration) : this(actionDuration, new ProxyCCTargetNode())
        {

        }

        #endregion Constructors


        #region Running action methods

        public void RunAction()
        {
            this.StopAction();

            this.ResumeAction();

            _proxy2dCCNodeActionTarget.RunAction(_proxy2dCCActionInterval);
        }

        public void StopAction()
        {
            this.PauseAction();

            _proxy2dCCNodeActionTarget.StopAllActions();
            _actionTimeElapsed = 0.0f;
        }

        public void PauseAction()
        {
            _proxy2dCCNodeActionTarget.ActionIsRunning = false;
        }

        public void ResumeAction()
        {
            _proxy2dCCNodeActionTarget.ActionIsRunning = true;
        }

        protected abstract void UpdateAction(float timeElapsedFraction, float timeIncrementFraction);

        private void ShouldUpdateAction(float timeIncrement)
        {
            float newTimeElapsed = _actionTimeElapsed + timeIncrement;

            if (newTimeElapsed < _actionDuration)
            {
                this.UpdateAction(_actionTimeElapsed/_actionDuration, timeIncrement/_actionDuration);

                _actionTimeElapsed = newTimeElapsed;
            }
        }

        #endregion Running action methods


        #region Private proxy CCActionInterval

        private class ProxyCCActionInterval : CCActionInterval
        {
            private CC3ActionRunner _parentCC3ActionRunner;

            internal ProxyCCActionInterval(CC3ActionRunner parentCC3ActionRunner, float actionDuration) 
                : base(actionDuration)
            {
                _parentCC3ActionRunner = parentCC3ActionRunner;
            }

            public override void Step(float timeIncrement)
            {
                base.Step(timeIncrement);

                _parentCC3ActionRunner.ShouldUpdateAction(timeIncrement);
            }
        }

        #endregion Private proxy CCActionInterval


        #region Private proxy CCNode

        protected class ProxyCCTargetNode : CCNode
        {
            internal bool ActionIsRunning
            {
                get { return this.m_bRunning; }
                set { this.m_bRunning = value; }
            }

            internal ProxyCCTargetNode() : base()
            {

            }

        }

        #endregion Private proxy CCNode
    }
}

