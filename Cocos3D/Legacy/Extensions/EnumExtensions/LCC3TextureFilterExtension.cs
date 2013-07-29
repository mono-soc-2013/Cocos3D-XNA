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
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3D
{
    public static class LCC3TextureFilterExtension
    {
        internal static TextureFilter XnaTextureFilter(this LCC3TextureFilter textureFilter)
        {
            TextureFilter xnaTextureFilter = TextureFilter.Linear;

            switch (textureFilter)
            {
                case LCC3TextureFilter.Anisotropic:
                    xnaTextureFilter = TextureFilter.Anisotropic;
                    break;
                case LCC3TextureFilter.Linear:
                    xnaTextureFilter = TextureFilter.Linear;
                    break;
                case LCC3TextureFilter.LinearMipPoint:
                    xnaTextureFilter = TextureFilter.LinearMipPoint;
                    break;
            }

            return xnaTextureFilter;
        }
    }
}

