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
using MonoMac.AppKit;
using MonoMac.Foundation;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3DShowcase
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new MacAppDelegate();
            NSApplication.Main(args);
        }
    }

    class MacAppDelegate : NSApplicationDelegate
    {
        private ShowcaseGame _game;

        public override void FinishedLaunching(MonoMac.Foundation.NSObject notification)
        {
            // Don't use 'using' to dispose of this
            // On Mac, the game is run on a background thread
            _game = new ShowcaseGame();
            _game.Run();
        }


        public override void WillTerminate(NSNotification notification)
        {
            _game.Dispose();
            _game = null;
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }
    }
}

