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
using Microsoft.Xna.Framework;

namespace Cocos3D
{
    public static class LCC3ColorUtil
    {
        public static CCColor4F CCC4FBlendAlpha(CCColor4F rgba)
        {
            CCColor4F result;
            result.R = MathHelper.Clamp(rgba.R * rgba.A, 0.0f, 1.0f);
            result.G = MathHelper.Clamp(rgba.G * rgba.A, 0.0f, 1.0f);
            result.B = MathHelper.Clamp(rgba.B * rgba.A, 0.0f, 1.0f);
            result.A = rgba.A;

            return result;
        }

        public static CCColor3B CCC3BFromCCC4F(CCColor4F floatColor)
        {
            CCColor3B color;
            color.R = CCColorByteFromFloat(floatColor.R);
            color.G = CCColorByteFromFloat(floatColor.G);
            color.B = CCColorByteFromFloat(floatColor.B);

            return color;
        }

        public static byte CCColorByteFromFloat(float colorValue) 
        {
            return (byte)(MathHelper.Clamp(colorValue, 0.0f, 1.0f) * 255.0f);
        }
    }
}

