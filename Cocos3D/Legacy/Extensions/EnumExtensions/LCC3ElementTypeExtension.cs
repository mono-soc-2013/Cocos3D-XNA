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
    public static class LCC3ElementTypeExtension
    {
        internal static uint Size(this LCC3ElementType elementType)
        {
            uint size = 0;

            switch (elementType)
            {
                case LCC3ElementType.Float:
                    size = sizeof(float);
                    break;

            }

            return size;
        }

        internal static Type CSharpType(this LCC3ElementType elementType)
        {
            Type csharpType = null;

            switch (elementType)
            {
                case LCC3ElementType.Float:
                    csharpType = typeof(float);
                    break;
                case LCC3ElementType.UnsignedByte:
                    csharpType = typeof(byte);
                    break;
                case LCC3ElementType.UnsignedShort:
                    csharpType = typeof(ushort);
                    break;
                case LCC3ElementType.UnsignedInt:
                    csharpType = typeof(uint);
                    break;
                case LCC3ElementType.Int:
                    csharpType = typeof(int);
                    break;
                case LCC3ElementType.Boolean:
                    csharpType = typeof(bool);
                    break;
            }

            return csharpType;
        }
    }
}

