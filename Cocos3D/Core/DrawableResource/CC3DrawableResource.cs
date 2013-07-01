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
    public abstract class CC3DrawableResource
    {
        // Instance fields

        protected CC3GraphicsContext _graphicsContext;


        #region Properties

        public CC3GraphicsContext GraphicsContext
        {
            get { return _graphicsContext; }
        }

        #endregion Properties


        #region Constructors

        public CC3DrawableResource()
        {
            // Use default graphics context
        }

        public CC3DrawableResource(CC3GraphicsContext graphicsContext)
        {
            _graphicsContext = graphicsContext;
        }

        #endregion Constructors

    }
}
