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
    public struct LCC3Plane: IEquatable<LCC3Plane>
    {
        // static vars

        static readonly LCC3Plane _CC3PlaneZero = new LCC3Plane(0.0f);

        // ivars

        Plane _xnaPlane;
        LCC3Vector _unitLengthNormalVec;

        #region Properties

        // Static properties

        public LCC3Plane CC3PlaneZero
        {
            get { return _CC3PlaneZero; }
        }

        // Instance properties

        public LCC3Vector UnitLengthNormal
        {
            get { return _unitLengthNormalVec; }
        }

        public float D
        {
            get { return _xnaPlane.D; }
        }


        #endregion Properties


        #region Operators

        public static bool operator ==(LCC3Plane p1, LCC3Plane p2)
        {
            return p1._xnaPlane == p2._xnaPlane;
        }

        public static bool operator !=(LCC3Plane p1, LCC3Plane p2)
        {
            return p1._xnaPlane != p2._xnaPlane;
        }

        public static LCC3Plane operator -(LCC3Plane plane)
        {
            LCC3Vector newNormalVec = -(plane._unitLengthNormalVec);

            return new LCC3Plane(newNormalVec, plane.D);
        }

        #endregion Operators


        #region Constructors

        public LCC3Plane(float a, float b, float c, float d)
        {
            _xnaPlane = new Plane(a, b, c, d);

            // Doing this to standardize the plane fields
            // This ensures correctness when testing equality
            _xnaPlane.Normalize();

            _unitLengthNormalVec = new LCC3Vector(_xnaPlane.Normal);
        }

        public LCC3Plane(float value): this(value, value, value, value)
        {

        }

        public LCC3Plane(LCC3Vector normalVector, float d)
        : this(normalVector.X, normalVector.Y, normalVector.Z, d)
        {

        }

        public LCC3Plane(LCC3Vector normalVector, LCC3Vector location)
        : this(normalVector, - LCC3Vector.CC3VectorDot(location, normalVector))
        {
        }

        public LCC3Plane(LCC3Vector v1, LCC3Vector v2, LCC3Vector v3)
        : this(LCC3Vector.CC3CrossProduct(v2-v1, v3-v1).NormalizedVector(), v1) 
        {

        }

        internal LCC3Plane(Plane xnaPlane)
        : this(new LCC3Vector(xnaPlane.Normal), xnaPlane.D)
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
            return (obj is LCC3Plane) ? this == (LCC3Plane)obj : false;
        }

        public bool Equals(LCC3Plane other)
        {
            // Using overloaded == operator
            return this == other;
        }

        public override int GetHashCode()
        {
            return _xnaPlane.GetHashCode();
        }

        // Plane calculation instance methods

        public float PerpendicularDistanceFromPlane(LCC3Vector point)
        {
            return LCC3Vector.CC3VectorDot(point, _unitLengthNormalVec) + _xnaPlane.D;
        }

        public bool IsPointInFrontOfPlane(LCC3Vector point) 
        {
            return (this.PerpendicularDistanceFromPlane(point) > 0.0f);
        }

        #endregion Instance methods
    }

}


