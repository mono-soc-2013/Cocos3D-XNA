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
using Cocos3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3DShowcase
{
    public partial class AppDelegate : CCApplication
    {
        private ShowcaseGameScene _scene;

        public AppDelegate(Game game, GraphicsDeviceManager graphicsDeviceManager)
            : base(game, graphicsDeviceManager)
        {
            s_pSharedApplication = this;

            // Calling full screen doesn't do anything on Mac
            graphicsDeviceManager.IsFullScreen = true;
            graphicsDeviceManager.PreferMultiSampling = false;

            // Instead ask to adapter to give us dimensions of screen
            graphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;


            CCDrawManager.InitializeDisplay(game, graphicsDeviceManager, DisplayOrientation.Unknown);

            // Create the scene.
            _scene = new ShowcaseGameScene(new CC3GraphicsContext(graphicsDeviceManager));
        }

        public override bool ApplicationDidFinishLaunching()
        {
            // Initialize director
            CCDirector director = CCDirector.SharedDirector;
            director.SetOpenGlView();

            // Turn on display FPS
            director.DisplayStats = true;

            // Set FPS. the default value is 1.0/60 if you don't call this
            director.AnimationInterval = 1.0 / 60;



            // This is an extension method of CCDirector
            director.RunWithCC3Scene(_scene);

            return true;
        }
    }
}

