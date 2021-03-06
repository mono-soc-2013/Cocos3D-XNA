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
    public class LCC3Scene : LCC3Node
    {
        // ivars

        LCC3Light[] _lights;
        CCColor4F _ambientLight;

        #region Properties

        public LCC3Light[] Lights
        {
            get { return _lights; }
            set { _lights = value; }
        }

        public CCColor4F AmbientLight
        {
            get { return _ambientLight; }
            set { _ambientLight = value; }
        }

        public override LCC3Scene Scene
        {
            get { return this; }
        }
        
        #endregion Properties

        public LCC3Scene()
        {
        }
    }
}

