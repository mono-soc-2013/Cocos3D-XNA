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
using Microsoft.Xna.Framework;

namespace Cocos3D
{
    public struct CC3Matrix
    {
        // Static fields

        private static readonly CC3Matrix _identity = new CC3Matrix(Matrix.Identity);

        // Instance fields

        private Matrix _xnaMatrix;


        #region Properties

        // Static properties

        public static CC3Matrix Identity
        {
            get { return _identity; }
        }

        // Instance properties

        public /* internal */ Matrix XnaMatrix
        {
            get { return _xnaMatrix; }
        }

        #endregion Properties

        #region Constructors

        internal CC3Matrix(Matrix xnaMatrix)
        {
            // Structs copy by value
            _xnaMatrix = xnaMatrix;
        }

        #endregion Constructors

        #region Calculation methods

        public CC3Matrix Inverse()
        {
            return new CC3Matrix(Matrix.Invert(_xnaMatrix));
        }

        #endregion Calculation methods
    }
}

