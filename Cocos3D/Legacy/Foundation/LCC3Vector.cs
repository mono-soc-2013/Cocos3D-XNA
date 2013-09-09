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
    public struct LCC3Vector : IEquatable<LCC3Vector>
    {
        // Private static fields

        private static readonly LCC3Vector _CC3VectorZero = new LCC3Vector(0f);
        private static readonly LCC3Vector _CC3VectorUnitCube = new LCC3Vector(1f);
        private static readonly LCC3Vector _CC3VectorNull = new LCC3Vector(float.PositiveInfinity);
        private static readonly LCC3Vector _CC3VectorUp = new LCC3Vector(Vector3.Up);
        private static readonly LCC3Vector _CC3VectorUnitXPositive = new LCC3Vector(1.0f, 0.0f, 0.0f);
        private static readonly LCC3Vector _CC3VectorUnitYPositive = new LCC3Vector(0.0f, 1.0f, 0.0f);
        private static readonly LCC3Vector _CC3VectorUnitZPositive = new LCC3Vector(0.0f, 0.0f, 1.0f);
        private static readonly LCC3Vector _CC3VectorUnitXNegative = new LCC3Vector(-1.0f, 0.0f, 0.0f);
        private static readonly LCC3Vector _CC3VectorUnitYNegative = new LCC3Vector(0.0f, -1.0f, 0.0f);
        private static readonly LCC3Vector _CC3VectorUnitZNegative = new LCC3Vector(0.0f, 0.0f, -1.0f);

        // Private instance fields

        private Vector3 _xnaVec3;

        #region Properties
        
        // Static properties

        public static LCC3Vector CC3VectorZero
        {
            get { return _CC3VectorZero; }
        }

        public static LCC3Vector CC3VectorUnitCube
        {
            get { return _CC3VectorUnitCube; }
        }

        public static LCC3Vector CC3VectorNull
        {
            get { return _CC3VectorNull; }
        }

        public static LCC3Vector CC3VectorUp
        {
            get { return _CC3VectorUp; }
        }

        public static LCC3Vector CC3UnitXPositive
        {
            get { return _CC3VectorUnitXPositive; }
        }

        public static LCC3Vector CC3UnitYPositive
        {
            get { return _CC3VectorUnitYPositive; }
        }

        public static LCC3Vector CC3UnitZPositive
        {
            get { return _CC3VectorUnitZPositive; }
        }

        public static LCC3Vector CC3UnitXNegative
        {
            get { return _CC3VectorUnitXNegative; }
        }

        public static LCC3Vector CC3UnitYNegative
        {
            get { return _CC3VectorUnitYNegative; }
        }

        public static LCC3Vector CC3UnitZNegative
        {
            get { return _CC3VectorUnitZNegative; }
        }

        // Public instance properties

        public float X
        {
            get { return _xnaVec3.X; }
        }

        public float Y
        {
            get { return _xnaVec3.Y; }
        }

        public float Z
        {
            get { return _xnaVec3.Z; }
        }

        // Internal instance properties

        internal Vector3 XnaVector
        {
            get { return _xnaVec3; }
        }

        #endregion Properties


        #region Operators

        public static bool operator ==(LCC3Vector value1, LCC3Vector value2)
        {
            return value1._xnaVec3 == value2._xnaVec3;
        }

        public static bool operator !=(LCC3Vector value1, LCC3Vector value2)
        {
            return value1._xnaVec3 != value2._xnaVec3;
        }

        public static LCC3Vector operator +(LCC3Vector value1, LCC3Vector value2)
        {
            Vector3 newXnaVec3 = value1._xnaVec3 + value2._xnaVec3;

            return new LCC3Vector(newXnaVec3);
        }

        public static LCC3Vector operator -(LCC3Vector value)
        {
            Vector3 newXnaVec3 = -(value._xnaVec3);

            return new LCC3Vector(newXnaVec3);
        }

        public static LCC3Vector operator -(LCC3Vector value1, LCC3Vector value2)
        {
            Vector3 newXnaVec3 = value1._xnaVec3 - value2._xnaVec3;

            return new LCC3Vector(newXnaVec3);
        }

        public static LCC3Vector operator *(LCC3Vector value1, LCC3Vector value2)
        {
            Vector3 newXnaVec3 = value1._xnaVec3 * value2._xnaVec3;

            return new LCC3Vector(newXnaVec3);
        }

        public static LCC3Vector operator *(LCC3Vector value, float scaleFactor)
        {
            Vector3 newXnaVec3 = value._xnaVec3 * scaleFactor;

            return new LCC3Vector(newXnaVec3);
        }

        public static LCC3Vector operator *(float scaleFactor, LCC3Vector value)
        {
            return value * scaleFactor;
        }

        public static LCC3Vector operator /(LCC3Vector value1, LCC3Vector value2)
        {
            Vector3 newXnaVec3 = value1._xnaVec3 / value2._xnaVec3;

            return new LCC3Vector(newXnaVec3);
        }

        public static LCC3Vector operator /(LCC3Vector value, float divider)
        {
            Vector3 newXnaVec3 = value._xnaVec3 / divider;

            return new LCC3Vector(newXnaVec3);
        }

        #endregion Operators


        #region Vector calculation static methods

        public static LCC3Vector CC3VectorMinimize(LCC3Vector v1, LCC3Vector v2)
        {
            Vector3 minVector = Vector3.Min(v1._xnaVec3, v2._xnaVec3);
            return new LCC3Vector(minVector);
        }

        public static LCC3Vector CC3VectorMaximize(LCC3Vector v1, LCC3Vector v2)
        {
            Vector3 maxVector = Vector3.Max(v1._xnaVec3, v2._xnaVec3);
            return new LCC3Vector(maxVector);
        }

        public static LCC3Vector CC3VectorLerp(LCC3Vector v1, LCC3Vector v2, float amount)
        {
            Vector3 lerpVector = Vector3.Lerp(v1._xnaVec3, v2._xnaVec3, amount);
            return new LCC3Vector(lerpVector);
        }

        public static LCC3Vector CC3VectorAverage(LCC3Vector v1, LCC3Vector v2)
        {
            return LCC3Vector.CC3VectorLerp(v1, v2, 0.5f);
        }

        public static float CC3VectorDot(LCC3Vector v1, LCC3Vector v2)
        {
            return Vector3.Dot(v1._xnaVec3, v2._xnaVec3);
        }

        public static float CC3Distance(LCC3Vector v1, LCC3Vector v2)
        {
            return Vector3.Distance(v1._xnaVec3, v2._xnaVec3);
        }

        public static float CC3DistanceSquared(LCC3Vector v1, LCC3Vector v2)
        {
            return Vector3.DistanceSquared(v1._xnaVec3, v2._xnaVec3);
        }

        public static LCC3Vector CC3CrossProduct(LCC3Vector v1, LCC3Vector v2)
        {
            return new LCC3Vector(Vector3.Cross(v1._xnaVec3, v2._xnaVec3));
        }

        public static float CC3VectorAngle(LCC3Vector v1, LCC3Vector v2)
        {
            return (float)Math.Acos(LCC3Vector.CC3VectorDot(v1, v2) / (v1.Length() * v2.Length()));
        }

        #endregion Vector calculation static methods


        #region Constructors

        public LCC3Vector(float x, float y, float z)
        {
            _xnaVec3 = new Vector3(x, y, z);
        }

        public LCC3Vector(float value) : this(value, value, value)
        {

        }

        public LCC3Vector(LCC3Vector vec3, float xOffset, float yOffset, float zOffset)
        : this(vec3.X + xOffset, vec3.Y + yOffset, vec3.Z + zOffset)
        {

        }

        public LCC3Vector(LCC3Vector vec3, float offset): this(vec3, offset, offset, offset)
        {

        }

        public LCC3Vector(Vector3 xnaVec3)
        {
            // Structs copy by value so we get new copy
            _xnaVec3 = xnaVec3;
        }

        #endregion Constructors


        #region Instance methods

        // Equality handling

        public override bool Equals(object obj)
        {
            // Using overloaded == operator
            return (obj is LCC3Vector) ? this == (LCC3Vector)obj : false;
        }

        public bool Equals(LCC3Vector other)
        {
            // Using overloaded == operator
            return this == other;
        }

        public override int GetHashCode()
        {
            return _xnaVec3.GetHashCode();
        }

        // Vector calculation instance methods

        public float Length()
        {
            return _xnaVec3.Length();
        }

        public float LengthSquared()
        {
            return _xnaVec3.LengthSquared();
        }

        public LCC3Vector NormalizedVector()
        {
            // Structs copy by value so we get new copy
            Vector3 normalizedXnaVec = _xnaVec3;
            normalizedXnaVec.Normalize();

            return new LCC3Vector(normalizedXnaVec);
        }

        public LCC3Vector RotatedVector(CC3Quaternion quaternion)
        {
            Vector3 xnaRotatedVec = Vector3.Transform(_xnaVec3, quaternion.XnaQuaternion);
            return new LCC3Vector(xnaRotatedVec);
        }

        #endregion Instance methods
    }
}

