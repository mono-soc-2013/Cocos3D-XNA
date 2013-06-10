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
    public struct CC3Ray : IEquatable<CC3Ray>
    {
        // Private instance fields

        private Ray _xnaRay;
        private CC3Vector _startLoc;
        private CC3Vector _direction;

        #region Properties

        public CC3Vector StartLocation
        {
            get { return _startLoc; }
        }

        public CC3Vector Direction 
        {
            get { return _direction; }
        }

        // Internal instance properties

        internal Ray XnaRay
        {
            get { return _xnaRay; }
        }

        #endregion Properties


        #region  Operators

        public static bool operator ==(CC3Ray value1, CC3Ray value2)
        {
            return value1._xnaRay == value2._xnaRay;
        }

        public static bool operator !=(CC3Ray value1, CC3Ray value2)
        {
            return value1._xnaRay != value2._xnaRay;
        }

        #endregion Operators


        #region Constructors

        public CC3Ray(CC3Vector startLocation, CC3Vector direction)
        {
            // Structures copy by value
            _startLoc = startLocation;
            _direction = direction;
        
            _xnaRay = new Ray(_startLoc.XnaVector, _direction.XnaVector);
        }

        public CC3Ray(float sX, float sY, float sZ, float dX, float dY, float dZ)
        : this(new CC3Vector(sX, sY, sZ), new CC3Vector(dX, dY, dZ))
        {

        }

        internal CC3Ray(Ray xnaRay)
        {
            // Structures copy by value
            _xnaRay = xnaRay;

            _startLoc = new CC3Vector(xnaRay.Position);
            _direction = new CC3Vector(xnaRay.Direction);
        }

        #endregion Constructors


        #region Instance methods

        // Equality handling

        public override bool Equals(object obj)
        {
            // Using overloaded == operator
            return (obj is CC3Ray) ? this == (CC3Ray)obj : false;
        }

        public bool Equals(CC3Ray other)
        {
            // Using overloaded == operator
            return this == other;
        }

        public override int GetHashCode()
        {
            return _xnaRay.GetHashCode();
        }

        #endregion Instance methods
    }
}

