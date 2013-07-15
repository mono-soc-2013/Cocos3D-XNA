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
    public static class LCC3StencilOperationExtension
    {
        internal static StencilOperation XnaStencilOperation(this LCC3StencilOperation stencilOp)
        {
            StencilOperation xnaStencilOperation = StencilOperation.Keep;

            switch (stencilOp)
            {
                case LCC3StencilOperation.Zero:
                    xnaStencilOperation = StencilOperation.Zero;
                    break;
                case LCC3StencilOperation.Replace:
                    xnaStencilOperation = StencilOperation.Replace;
                    break;
                case LCC3StencilOperation.Increment:
                    xnaStencilOperation = StencilOperation.Increment;
                    break;
                case LCC3StencilOperation.Decrement:
                    xnaStencilOperation = StencilOperation.Decrement;
                    break;
                case LCC3StencilOperation.IncrementSaturation:
                    xnaStencilOperation = StencilOperation.IncrementSaturation;
                    break;
                case LCC3StencilOperation.DecrementSaturation:
                    xnaStencilOperation = StencilOperation.DecrementSaturation;
                    break;
                case LCC3StencilOperation.Invert:
                    xnaStencilOperation = StencilOperation.Invert;
                    break;
            }

            return xnaStencilOperation;
        }
    }
}

