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
    public class LCC3VertexAttr
    {
        // ivars

        LCC3ElementType _elementType;
        uint _elementSize;
        uint _vertexStride;
        object[] _vertices;
        bool _shouldNormalize;
        bool _isKnown;
        bool _isEnabled;
        bool _isEnabledKnown;
        bool _wasBound;


        #region Properties

        internal LCC3ElementType ElementType
        {
            get { return _elementType; }
            set { _elementType = value; }
        }

        internal uint ElementSize
        {
            get { return _elementSize; }
            set { _elementSize = value; }
        }

        internal uint VertexStride
        {
            get { return _vertexStride; }
            set { _vertexStride = value; }
        }

        internal object[] Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }

        internal bool ShouldNormalize
        {
            get { return _shouldNormalize; }
            set { _shouldNormalize = value; }
        }

        internal bool IsKnown
        {
            get { return _isKnown; }
            set { _isKnown = value; }
        }

        internal bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        internal bool IsEnabledKnown
        {
            get { return _isEnabledKnown; }
            set { _isEnabledKnown = value; }
        }

        internal bool WasBound
        {
            get { return _wasBound; }
            set { _wasBound = value; }
        }

        #endregion Properties
    }
}

