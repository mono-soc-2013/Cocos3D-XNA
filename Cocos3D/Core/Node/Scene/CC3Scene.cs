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
using Cocos2D;

namespace Cocos3D
{
    public class CC3Scene : CC3DrawableNode, ICC3CameraObserver
    {
        // Instance fields

        private ProxyCCScene _proxy2dCCScene;

        private CC3Camera _activeCamera;


        #region Properties

        // Instance properties

        public CC3Camera ActiveCamera
        {
            get { return _activeCamera; }
            set 
            { 
                if (_activeCamera != null)
                {
                    // No longer interested in changes to previous active camera
                    _activeCamera.CameraObserver = null;
                }

                _activeCamera = value;

                if (_activeCamera != null)
                {
                    _activeCamera.CameraObserver = this;
                    this.CameraViewMatrixDidChange(_activeCamera);
                    this.CameraProjectionMatrixDidChange(_activeCamera);
                }
            }
        }

        internal CCScene Proxy2dCCScene
        {
            get { return _proxy2dCCScene; }
        }

        #endregion Properties


        #region Constructors

        public CC3Scene(CC3GraphicsContext graphicsContext) : base(graphicsContext)
        {
            _proxy2dCCScene = new ProxyCCScene(this);
        }

        #endregion Constructors


        #region Camera listener interface methods

        public void CameraViewMatrixDidChange(CC3Camera camera)
        {
            if (_activeCamera == camera)
            {
                _graphicsContext.ViewMatrix = _activeCamera.ViewMatrix;
            }
        }

        public void CameraProjectionMatrixDidChange(CC3Camera camera)
        {
            if (_activeCamera == camera)
            {
                _graphicsContext.ProjectionMatrix = _activeCamera.ProjectionMatrix;
            }
        }

        #endregion Camera listener interface methods


        #region Drawing

        private void PrepareToDrawScence()
        {
            this.Draw();
        }

        public override void Draw()
        {

        }

        public virtual void UpdateScene(float dt)
        {

        }

        #endregion Drawing


        #region Private proxy CCScene

        private class ProxyCCScene : CCInputScene
        {
            private CC3Scene _parentCC3Scene;


            internal ProxyCCScene(CC3Scene parentCC3Scene) : base()
            {
                _parentCC3Scene = parentCC3Scene;

            }

            public override void Visit()
            {
                _parentCC3Scene.PrepareToDrawScence();
            }

            public override void Update(float dt)
            {
                _parentCC3Scene.UpdateScene(dt);
            }
        }

        #endregion Private proxy CCScene
    }
}

