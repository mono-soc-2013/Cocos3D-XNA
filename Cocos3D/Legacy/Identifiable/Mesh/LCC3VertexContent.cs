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
    [Flags]
    public enum LCC3VertexContent
    {
        VertexContentNone = 0,
        VertexContentLocation = 1 << 0,
        VertexContentNormal = 1 << 1,
        VertexContentTangent = 1 << 2,
        VertexContentBitangent = 1 << 3,
        VertexContentColor = 1 << 4,
        VertexContentTextureCoordinates = 1 << 5,
        VertexContentPointSize = 1 << 6,
        VertexContentWeights = 1 << 7,
        VertexContentMatrixIndices = 1 << 8
    }
}

