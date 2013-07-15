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
    public static class LCC3DepthStencilFuncModeExtension
    {
        internal static CompareFunction XnaCompareFunc(this LCC3DepthStencilFuncMode depthStencilFuncMode)
        {
            CompareFunction xnaCompareFunc = CompareFunction.Less;

            switch (depthStencilFuncMode)
            {
                case LCC3DepthStencilFuncMode.Never:
                    xnaCompareFunc = CompareFunction.Never;
                    break;
                case LCC3DepthStencilFuncMode.Less:
                    xnaCompareFunc = CompareFunction.Less;
                    break;
                case LCC3DepthStencilFuncMode.Equal:
                    xnaCompareFunc = CompareFunction.Equal;
                    break;
                case LCC3DepthStencilFuncMode.LessOrEqual:
                    xnaCompareFunc = CompareFunction.LessEqual;
                    break;
                case LCC3DepthStencilFuncMode.Greater:
                    xnaCompareFunc = CompareFunction.Greater;
                    break;
                case LCC3DepthStencilFuncMode.NotEqual:
                    xnaCompareFunc = CompareFunction.NotEqual;
                    break;
                case LCC3DepthStencilFuncMode.GreaterOrEqual:
                    xnaCompareFunc = CompareFunction.GreaterEqual;
                    break;
                case LCC3DepthStencilFuncMode.Always:
                    xnaCompareFunc = CompareFunction.Always;
                    break;
            }

            return xnaCompareFunc;
        }
    }
}
