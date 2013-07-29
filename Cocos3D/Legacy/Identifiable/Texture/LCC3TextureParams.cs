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

namespace Cocos3D
{
    public struct LCC3TextureParams
    {
        // Instance fields

        private LCC3TextureFilter _minifyingFilter;
        private LCC3TextureFilter _magnifyingFilter;
        private LCC3TextureWrapMode _horizontalWrapMode;
        private LCC3TextureWrapMode _verticalWrapMode;


        #region Properties

        public LCC3TextureFilter MinifyingFilter
        {
            get { return _minifyingFilter; }
        }

        public LCC3TextureFilter MagnifyingFilter
        {
            get { return _magnifyingFilter; }
        }

        public LCC3TextureWrapMode HorizontalWrapMode
        {
            get { return _horizontalWrapMode; }
        }

        public LCC3TextureWrapMode VerticalWrapMode
        {
            get { return _verticalWrapMode; }
        }

        #endregion Properties


        #region Constructors

        public LCC3TextureParams(LCC3TextureFilter minifyingFilter, 
                                 LCC3TextureFilter magnifyingFilter,
                                 LCC3TextureWrapMode horizontalWrapMode,
                                 LCC3TextureWrapMode verticalWrapMode)
        {
            _minifyingFilter = minifyingFilter;
            _magnifyingFilter = magnifyingFilter;
            _horizontalWrapMode = horizontalWrapMode;
            _verticalWrapMode = verticalWrapMode;
        }

        #endregion Constructors
    }
}

