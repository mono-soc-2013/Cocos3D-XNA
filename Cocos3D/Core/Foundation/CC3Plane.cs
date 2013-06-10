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
    public struct CC3Plane: IEquatable<CC3Plane>
    {
        // Private static fields

        private static readonly CC3Plane _CC3PlaneZero = new CC3Plane(0.0f);

        // Private instance fields

        private Plane _xnaPlane;
        private CC3Vector _unitLengthNormalVec;

        #region Properties

        // Static properties

        public CC3Plane CC3PlaneZero
        {
            get { return _CC3PlaneZero; }
        }

        // Instance properties

        public CC3Vector UnitLengthNormal
        {
            get { return _unitLengthNormalVec; }
        }

        public float D
        {
            get { return _xnaPlane.D; }
        }


        #endregion Properties


        #region Operators

        public static bool operator ==(CC3Plane p1, CC3Plane p2)
        {
            return p1._xnaPlane == p2._xnaPlane;
        }

        public static bool operator !=(CC3Plane p1, CC3Plane p2)
        {
            return p1._xnaPlane != p2._xnaPlane;
        }

        public static CC3Plane operator -(CC3Plane plane)
        {
            CC3Vector newNormalVec = -(plane._unitLengthNormalVec);

            return new CC3Plane(newNormalVec, plane.D);
        }

        #endregion Operators


        #region Static methods


        #endregion Static methods


        #region Constructors

        public CC3Plane(float a, float b, float c, float d)
        {
            _xnaPlane = new Plane(a, b, c, d);

            // Doing this to standardize the plane fields
            // This ensures correctness when testing equality
            _xnaPlane.Normalize();

            _unitLengthNormalVec = new CC3Vector(_xnaPlane.Normal);
        }

        public CC3Plane(float value): this(value, value, value, value)
        {

        }

        public CC3Plane(CC3Vector normalVector, float d)
        : this(normalVector.X, normalVector.Y, normalVector.Z, d)
        {

        }

        internal CC3Plane(Plane xnaPlane)
        : this(new CC3Vector(xnaPlane.Normal), xnaPlane.D)
        {

        }

        // Warning: make sure any new constructors normalize underlying XNA Plane object
        // See constructor: public CC3Plane(float a, float b, float c, float d)

        #endregion Constructors


        #region Instance methods

        // Equality handling

        public override bool Equals(object obj)
        {
            // Using overloaded == operator
            return (obj is CC3Plane) ? this == (CC3Plane)obj : false;
        }

        public bool Equals(CC3Plane other)
        {
            // Using overloaded == operator
            return this == other;
        }

        public override int GetHashCode()
        {
            return _xnaPlane.GetHashCode();
        }

        // Plane calculation instance methods

        public float PerpendicularDistanceFromPlane(CC3Vector point)
        {
            return CC3Vector.CC3VectorDot(point, _unitLengthNormalVec) + _xnaPlane.D;
        }

        public bool IsPointInFrontOfPlane(CC3Vector point) 
        {
            return (this.PerpendicularDistanceFromPlane(point) > 0.0f);
        }
           
        #endregion Instance methods
    }

    
}

