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
	public struct LCC3Quaternion : IEquatable<LCC3Quaternion>
	{
		// Private static fields

		private static readonly LCC3Quaternion _CC3QuatZero = new LCC3Quaternion(0.0f);
		private static readonly LCC3Quaternion _CC3QuatIdty = new LCC3Quaternion(new LCC3Vector(0.0f), 1.0f);
		private static readonly LCC3Quaternion _CC3QuatNull = new LCC3Quaternion(float.PositiveInfinity);       

		// Private instance fields

		private Quaternion _xnaQuat;


		#region Properties

		// Static properties

		public static LCC3Quaternion CC3QuaternionZero
		{
			get { return _CC3QuatZero; }
		}

		public static LCC3Quaternion CC3QuaternionIdentity
		{
			get { return _CC3QuatIdty; }
		}

		public static LCC3Quaternion CC3QuaternionNull
		{
			get { return _CC3QuatNull; }
		}

		// Instance properties

		public float X
		{
			get { return _xnaQuat.X; }
		}

		public float Y
		{
			get { return _xnaQuat.Y; }
		}

		public float Z
		{
			get { return _xnaQuat.Z; }
		}

		public float W
		{
			get { return _xnaQuat.W; }
		}

		internal Quaternion XnaQuaternion
		{
			get { return _xnaQuat; }
		}

		#endregion Properties


		#region Operators

		public static bool operator ==(LCC3Quaternion value1, LCC3Quaternion value2)
		{
			return value1._xnaQuat == value2._xnaQuat;
		}

		public static bool operator !=(LCC3Quaternion value1, LCC3Quaternion value2)
		{
			return value1._xnaQuat != value2._xnaQuat;
		}

		public static LCC3Quaternion operator +(LCC3Quaternion value1, LCC3Quaternion value2)
		{
			Quaternion newXnaQuat = value1._xnaQuat + value2._xnaQuat;

			return new LCC3Quaternion(newXnaQuat);
		}

		public static LCC3Quaternion operator -(LCC3Quaternion value)
		{
			Quaternion newXnaQuat = -(value._xnaQuat);

			return new LCC3Quaternion(newXnaQuat);
		}

		public static LCC3Quaternion operator *(LCC3Quaternion value1, LCC3Quaternion value2)
		{
			Quaternion newXnaQuat = value1._xnaQuat * value2._xnaQuat;

			return new LCC3Quaternion(newXnaQuat);
		}

		public static LCC3Quaternion operator *(LCC3Quaternion value, float scaleFactor)
		{
			Quaternion newXnaQuat = value._xnaQuat * scaleFactor;

			return new LCC3Quaternion(newXnaQuat);
		}

		public static LCC3Quaternion operator *(float scaleFactor, LCC3Quaternion value)
		{
			return value * scaleFactor;
		}

		#endregion Operators


		#region Static methods

		public static LCC3Quaternion CreateFromAxisAngle(LCC3Vector axis, float angle)
		{
			Quaternion newXnaQuat 
			= Quaternion.CreateFromAxisAngle(axis.XnaVector, angle);

			return new LCC3Quaternion(newXnaQuat);
		}

		public static LCC3Quaternion CreateFromAxisAngle(LCC3Vector4 axisAndAngle)
		{
			return LCC3Quaternion.CreateFromAxisAngle(axisAndAngle.TruncateToCC3Vector(), axisAndAngle.W);
		}

		public static LCC3Quaternion CreateFromXAxisRotation(float angle)
		{
			return LCC3Quaternion.CreateFromAxisAngle(new LCC3Vector(1.0f, 0.0f, 0.0f), angle);
		}

		public static LCC3Quaternion CreateFromZAxisRotation(float angle)
		{
			return LCC3Quaternion.CreateFromAxisAngle(new LCC3Vector(0.0f, 0.0f, 1.0f), angle);
		}

		public static float CC3QuaternionDot(LCC3Quaternion q1, LCC3Quaternion q2)
		{
			return Quaternion.Dot(q1._xnaQuat, q2._xnaQuat);
		}

		public static LCC3Quaternion CC3QuaternionSlerp(LCC3Quaternion q1, LCC3Quaternion q2, float amount)
		{
			Quaternion xnaSlerpQuat = Quaternion.Slerp(q1._xnaQuat, q2._xnaQuat, amount);

			return new LCC3Quaternion(xnaSlerpQuat);
		}

		#endregion Static methods


		#region Constructors

		public LCC3Quaternion(float x, float y, float z, float w)
		{
			_xnaQuat = new Quaternion(x, y, z, w);
		}

		public LCC3Quaternion(LCC3Vector vec3, float scalar)
			: this(vec3.X, vec3.Y, vec3.Z, scalar)
		{

		}

		public LCC3Quaternion(float value) : this(value, value, value, value)
		{

		}

		internal LCC3Quaternion(Quaternion xnaQuaternion)
		{
			// Structs copy by value so we get new copy
			_xnaQuat = xnaQuaternion;
		}

		#endregion Constructors


		#region Instance methods

		// Equality handling

		public override bool Equals(object obj)
		{
			// Using overloaded == operator
			return (obj is LCC3Quaternion) ? this == (LCC3Quaternion)obj : false;
		}

		public bool Equals(LCC3Quaternion other)
		{
			// Using overloaded == operator
			return this == other;
		}

		public override int GetHashCode()
		{
			return _xnaQuat.GetHashCode();
		}

		// Calculation instance methods

		public float Length()
		{
			return _xnaQuat.Length();
		}

		public float LengthSquared()
		{
			return _xnaQuat.LengthSquared();
		}

		public LCC3Quaternion NormalizedQuaternion()
		{
			// Structs copy by value so we get new copy
			Quaternion normalizedXnaQuat = _xnaQuat;
			normalizedXnaQuat.Normalize();

			return new LCC3Quaternion(normalizedXnaQuat);
		}

		public LCC3Quaternion Inverse()
		{
			Quaternion inverseXnaQuat = Quaternion.Inverse(_xnaQuat);

			return new LCC3Quaternion(inverseXnaQuat);
		}

		#endregion Instance methods
	}



}

