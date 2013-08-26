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
    public class LCC3Light : LCC3Node
    {
        // Instance fields

        private bool _isVisible;
        private CCColor4F _ambientColor;
        private CCColor4F _diffuseColor;
        private CCColor4F _specularColor;

        #region Properties

        // Static properties

        public static uint DefaultMaxNumOfLights
        {
            get { return 3; }
        }

        // Instance properties

        public bool Visible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        public bool IsDirectionalOnly
        {
            get { return true; }
        }

        public CCColor4F AmbientColor
        {
            get { return _ambientColor; }
            set { _ambientColor = value; }
        }

        public CCColor4F DiffuseColor
        {
            get { return _diffuseColor; }
            set { _diffuseColor = value; }
        }

        public CCColor4F SpecularColor
        {
            get { return _specularColor; }
            set { _specularColor = value; }
        }

        public override LCC3Vector4 GlobalHomogeneousPosition
        {
            get 
            { 
                float w = this.IsDirectionalOnly ? 0.0f : 1.0f;
                return new LCC3Vector4(this.GlobalLocation, w);
            }
        }

        #endregion Properties


        public LCC3Light()
        {
            _ambientColor = LCC3ColorUtil.CCC4FBlackTransparent;
            _diffuseColor = LCC3ColorUtil.CCC4FBlackTransparent;
            _specularColor = LCC3ColorUtil.CCC4FBlackTransparent;
        }
    }
}

