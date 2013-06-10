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
	public struct CC3Vector : IEquatable<CC3Vector>
	{
		// Private static fields

		private static readonly CC3Vector _CC3VectorZero = new CC3Vector(0f);
        private static readonly CC3Vector _CC3VectorUnitCube = new CC3Vector(1f);
        private static readonly CC3Vector _CC3VectorNull = new CC3Vector(float.PositiveInfinity);

		// Private instance fields

		private Vector3 _xnaVec3;

        #region Properties
		
        // Static properties

        public static CC3Vector CC3VectorZero
        {
            get { return _CC3VectorZero; }
        }

        public static CC3Vector CC3VectorUnitCube
        {
            get { return _CC3VectorUnitCube; }
        }

        public static CC3Vector CC3VectorNull
        {
            get { return _CC3VectorNull; }
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

        public static bool operator ==(CC3Vector value1, CC3Vector value2)
        {
            return value1._xnaVec3 == value2._xnaVec3;
        }

        public static bool operator !=(CC3Vector value1, CC3Vector value2)
        {
            return value1._xnaVec3 != value2._xnaVec3;
        }

        public static CC3Vector operator +(CC3Vector value1, CC3Vector value2)
        {
            Vector3 newXnaVec3 = value1._xnaVec3 + value2._xnaVec3;

            return new CC3Vector(newXnaVec3);
        }

        public static CC3Vector operator -(CC3Vector value)
        {
            Vector3 newXnaVec3 = -(value._xnaVec3);

            return new CC3Vector(newXnaVec3);
        }

        public static CC3Vector operator -(CC3Vector value1, CC3Vector value2)
        {
            Vector3 newXnaVec3 = value1._xnaVec3 - value2._xnaVec3;

            return new CC3Vector(newXnaVec3);
        }

        public static CC3Vector operator *(CC3Vector value1, CC3Vector value2)
        {
            Vector3 newXnaVec3 = value1._xnaVec3 * value2._xnaVec3;

            return new CC3Vector(newXnaVec3);
        }

        public static CC3Vector operator *(CC3Vector value, float scaleFactor)
        {
            Vector3 newXnaVec3 = value._xnaVec3 * scaleFactor;

            return new CC3Vector(newXnaVec3);
        }

        public static CC3Vector operator *(float scaleFactor, CC3Vector value)
        {
            return value * scaleFactor;
        }

        public static CC3Vector operator /(CC3Vector value1, CC3Vector value2)
        {
            Vector3 newXnaVec3 = value1._xnaVec3 / value2._xnaVec3;

            return new CC3Vector(newXnaVec3);
        }

        public static CC3Vector operator /(CC3Vector value, float divider)
        {
            Vector3 newXnaVec3 = value._xnaVec3 / divider;

            return new CC3Vector(newXnaVec3);
        }

        #endregion Operators


        #region Vector calculation static methods

        public static CC3Vector CC3VectorMinimize(CC3Vector v1, CC3Vector v2)
        {
            Vector3 minVector = Vector3.Min(v1._xnaVec3, v2._xnaVec3);
            return new CC3Vector(minVector);
        }

        public static CC3Vector CC3VectorMaximize(CC3Vector v1, CC3Vector v2)
        {
            Vector3 maxVector = Vector3.Max(v1._xnaVec3, v2._xnaVec3);
            return new CC3Vector(maxVector);
        }

        public static CC3Vector CC3VectorLerp(CC3Vector v1, CC3Vector v2, float amount)
        {
            Vector3 lerpVector = Vector3.Lerp(v1._xnaVec3, v2._xnaVec3, amount);
            return new CC3Vector(lerpVector);
        }

        public static CC3Vector CC3VectorAverage(CC3Vector v1, CC3Vector v2)
        {
            return CC3Vector.CC3VectorLerp(v1, v2, 0.5f);
        }

        public static float CC3VectorDot(CC3Vector v1, CC3Vector v2)
        {
            return Vector3.Dot(v1._xnaVec3, v2._xnaVec3);
        }

        public static float CC3Distance(CC3Vector v1, CC3Vector v2)
        {
            return Vector3.Distance(v1._xnaVec3, v2._xnaVec3);
        }

        public static float CC3DistanceSquared(CC3Vector v1, CC3Vector v2)
        {
            return Vector3.DistanceSquared(v1._xnaVec3, v2._xnaVec3);
        }

        public static CC3Vector CC3CrossProduct(CC3Vector v1, CC3Vector v2)
        {
            return new CC3Vector(Vector3.Cross(v1._xnaVec3, v2._xnaVec3));
        }

        #endregion Vector calculation static methods


        #region Constructors

		public CC3Vector(float x, float y, float z)
		{
			_xnaVec3 = new Vector3(x, y, z);
		}

        public CC3Vector(float value) : this(value, value, value)
        {

        }

        public CC3Vector(CC3Vector vec3, float xOffset, float yOffset, float zOffset)
        : this(vec3.X + xOffset, vec3.Y + yOffset, vec3.Z + zOffset)
        {

        }

        public CC3Vector(CC3Vector vec3, float offset): this(vec3, offset, offset, offset)
        {

        }

		internal CC3Vector(Vector3 xnaVec3)
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
            return (obj is CC3Vector) ? this == (CC3Vector)obj : false;
		}

		public bool Equals(CC3Vector other)
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

        public CC3Vector NormalizedVector()
        {
            // Structs copy by value so we get new copy
            Vector3 normalizedXnaVec = _xnaVec3;
            normalizedXnaVec.Normalize();

            return new CC3Vector(normalizedXnaVec);
        }

        #endregion Instance methods
	}
}

