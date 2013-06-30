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
    public class AppDelegate : CCApplication
    {
        private Game _game;
        private GraphicsDeviceManager _graphicsDeviceManager;

        public AppDelegate(Game game, GraphicsDeviceManager graphicsDeviceManager)
            : base(game, graphicsDeviceManager)
        {
            s_pSharedApplication = this;
            _graphicsDeviceManager = graphicsDeviceManager;

            _game = game;
        }

        public override bool ApplicationDidFinishLaunching()
        {
            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.PreferMultiSampling = true;

            // Ask to adapter to give us dimensions of screen
            _graphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // Initialize director
            CCDirector director = CCDirector.SharedDirector;
            director.SetOpenGlView();

            // Turn on display FPS
            director.DisplayStats = true;

            // Set FPS. the default value is 1.0/60 if you don't call this
            director.AnimationInterval = 1.0 / 60;

            CCDrawManager.InitializeDisplay(_game, _graphicsDeviceManager, DisplayOrientation.Default);

            CCDrawManager.SetDesignResolutionSize(_graphicsDeviceManager.PreferredBackBufferWidth, 
                                                  _graphicsDeviceManager.PreferredBackBufferHeight, 
                                                  CCResolutionPolicy.ShowAll);

            // Create the scene.
            CC3Scene scene = new ShowcaseGameScene(new CC3GraphicsContext(_graphicsDeviceManager));

            // This is an extension method of CCDirector
            director.RunWithCC3Scene(scene);

            return true;
        }
    }
}

