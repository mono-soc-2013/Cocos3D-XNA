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
    public struct CC3Vector4 : IEquatable<CC3Vector4>
    {
        // Private static fields

        private static readonly CC3Vector4 _CC3Vector4Zero = new CC3Vector4(0f);
        private static readonly CC3Vector4 _CC3Vector4Null = new CC3Vector4(float.PositiveInfinity);

        // Private instance fields

        private Vector4 _xnaVec4;


        #region Properties

        // Static properties

        public static CC3Vector4 CC3Vector4Zero
        {
            get { return _CC3Vector4Zero; }
        }

        public static CC3Vector4 CC3Vector4Null
        {
            get { return _CC3Vector4Null; }
        }

        // Public instance properties

        public float X
        {
            get { return _xnaVec4.X; }
        }

        public float Y
        {
            get { return _xnaVec4.Y; }
        }

        public float Z
        {
            get { return _xnaVec4.Z; }
        }

        public float W
        {
            get { return _xnaVec4.W; }
        }

        // Internal instance properties

        internal Vector4 XnaVector4
        {
            get { return _xnaVec4; }
        }

        #endregion Properties


        #region Operators

        public static bool operator ==(CC3Vector4 value1, CC3Vector4 value2)
        {
            return value1._xnaVec4 == value2._xnaVec4;
        }

        public static bool operator !=(CC3Vector4 value1, CC3Vector4 value2)
        {
            return value1._xnaVec4 != value2._xnaVec4;
        }
        
        public static CC3Vector4 operator +(CC3Vector4 value1, CC3Vector4 value2)
        {
            Vector4 newXnaVec4 = value1._xnaVec4 + value2._xnaVec4;

            return new CC3Vector4(newXnaVec4);
        }

        public static CC3Vector4 operator -(CC3Vector4 value)
        {
            Vector4 newXnaVec4 = -(value._xnaVec4);

            return new CC3Vector4(newXnaVec4);
        }

        public static CC3Vector4 operator *(CC3Vector4 value1, CC3Vector4 value2)
        {
            Vector4 newXnaVec4 = value1._xnaVec4 * value2._xnaVec4;

            return new CC3Vector4(newXnaVec4);
        }

        public static CC3Vector4 operator *(CC3Vector4 value, float scaleFactor)
        {
            Vector4 newXnaVec4 = value._xnaVec4 * scaleFactor;

            return new CC3Vector4(newXnaVec4);
        }

        public static CC3Vector4 operator *(float scaleFactor, CC3Vector4 value)
        {
            return value * scaleFactor;
        }

        #endregion Operators


        #region Constructors

        public CC3Vector4(float x, float y, float z, float w)
        {
            _xnaVec4 = new Vector4(x, y, z, w);
        }

        public CC3Vector4(CC3Vector vec3, float w)
        : this(vec3.X, vec3.Y, vec3.Z, w)
        {

        }

        public CC3Vector4(float value): this(value, value, value, value)
        {

        }

        public CC3Vector4(CC3Vector4 vec4, float xOffset, float yOffset, float zOffset, float wOffset)
        : this(vec4.X + xOffset, vec4.Y + yOffset, vec4.Z + zOffset, vec4.W + wOffset)
        {

        }

        public CC3Vector4(CC3Vector4 vec4, float offset): this(vec4, offset, offset, offset, offset)
        {

        }

        internal CC3Vector4(Vector4 xnaVec4)
        {
            // Structs copy by value so we get new copy
            _xnaVec4 = xnaVec4;
        }

        #endregion Constructors


        #region Instance methods

        // Equality handling

        public override bool Equals(object obj)
        {
            // Using overloaded == operator
            return (obj is CC3Vector4) ? this == (CC3Vector4)obj : false;
        }

        public bool Equals(CC3Vector4 other)
        {
            // Using overloaded == operator
            return this == other;
        }

        public override int GetHashCode()
        {
            return _xnaVec4.GetHashCode();
        }

        // Vector calculation instance methods

        public float Length()
        {
            return _xnaVec4.Length();
        }

        public float LengthSquared()
        {
            return _xnaVec4.LengthSquared();
        }

        public CC3Vector4 NormalizedVector()
        {
            // Structs copy by value so we get new copy
            Vector4 normalizedXnaVec4 = _xnaVec4;
            normalizedXnaVec4.Normalize();

            return new CC3Vector4(normalizedXnaVec4);
        }

        public CC3Vector TruncateToCC3Vector()
        {
            return new CC3Vector(_xnaVec4.X,_xnaVec4.Y,_xnaVec4.Z);
        }

        #endregion Instance methods
    }
}

