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
    public class CC3Layer : CCLayerColor
    {
        // Private instance fields

        private CC3Scene _scene;

        #region Properties

        public CC3Scene Scene
        {
            get { return _scene; }
            set { this.CloseScene(); _scene = value; } 
        }

        #endregion Properties

        #region Constructors

        public CC3Layer() : base()
        {

        }

        #endregion Constructors


        #region Updating layer
       
        // Overriden update methods from Cocos2D

        public override void OnEnter() 
        {
            base.OnEnter();
        }

        public override void OnExit()
        {


            base.OnExit();
        }

        public override void Update(float dt)
        {

        }

        // Protected update layer methods

        protected virtual void OnOpenCC3Layer()
        {

        }

        protected virtual void OnCloseCC3Layer()
        {

        }

        // Private update layer methods

        private void OpenScene()
        {
            // Set viewport
        }

        private void CloseScene()
        {

        }
       
        #endregion Updating layer


        #region Drawing

        // Overriden drawing methods from Cocos2D

        public override void Draw()
        {

        }

        #endregion Drawing
    }
}

