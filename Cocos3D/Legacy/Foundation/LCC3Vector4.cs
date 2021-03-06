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
    public struct LCC3Vector4 : IEquatable<LCC3Vector4>
    {
        // static vars

        static readonly LCC3Vector4 _CC3Vector4Zero = new LCC3Vector4(0f);
        static readonly LCC3Vector4 _CC3Vector4One = new LCC3Vector4(1f);
        static readonly LCC3Vector4 _CC3Vector4Null = new LCC3Vector4(float.PositiveInfinity);

        // ivars

        Vector4 _xnaVec4;


        #region Properties

        // Static properties

        public static LCC3Vector4 CC3Vector4Zero
        {
            get { return _CC3Vector4Zero; }
        }

        public static LCC3Vector4 CC3Vector4One
        {
            get { return _CC3Vector4One; }
        }

        public static LCC3Vector4 CC3Vector4Null
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

        public static bool operator ==(LCC3Vector4 value1, LCC3Vector4 value2)
        {
            return value1._xnaVec4 == value2._xnaVec4;
        }

        public static bool operator !=(LCC3Vector4 value1, LCC3Vector4 value2)
        {
            return value1._xnaVec4 != value2._xnaVec4;
        }
        
        public static LCC3Vector4 operator +(LCC3Vector4 value1, LCC3Vector4 value2)
        {
            Vector4 newXnaVec4 = value1._xnaVec4 + value2._xnaVec4;

            return new LCC3Vector4(newXnaVec4);
        }

        public static LCC3Vector4 operator -(LCC3Vector4 value)
        {
            Vector4 newXnaVec4 = -(value._xnaVec4);

            return new LCC3Vector4(newXnaVec4);
        }

        public static LCC3Vector4 operator *(LCC3Vector4 value1, LCC3Vector4 value2)
        {
            Vector4 newXnaVec4 = value1._xnaVec4 * value2._xnaVec4;

            return new LCC3Vector4(newXnaVec4);
        }

        public static LCC3Vector4 operator *(LCC3Vector4 value, float scaleFactor)
        {
            Vector4 newXnaVec4 = value._xnaVec4 * scaleFactor;

            return new LCC3Vector4(newXnaVec4);
        }

        public static LCC3Vector4 operator *(float scaleFactor, LCC3Vector4 value)
        {
            return value * scaleFactor;
        }

        #endregion Operators


        #region Constructors

        public LCC3Vector4(float x, float y, float z, float w)
        {
            _xnaVec4 = new Vector4(x, y, z, w);
        }

        public LCC3Vector4(LCC3Vector vec3, float w)
        : this(vec3.X, vec3.Y, vec3.Z, w)
        {

        }

        public LCC3Vector4(float value): this(value, value, value, value)
        {

        }

        public LCC3Vector4(CC3Vector4 vec4, float xOffset, float yOffset, float zOffset, float wOffset)
        : this(vec4.X + xOffset, vec4.Y + yOffset, vec4.Z + zOffset, vec4.W + wOffset)
        {

        }

        public LCC3Vector4(CC3Vector4 vec4, float offset): this(vec4, offset, offset, offset, offset)
        {

        }

        internal LCC3Vector4(Vector4 xnaVec4)
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
            return (obj is LCC3Vector4) ? this == (LCC3Vector4)obj : false;
        }

        public bool Equals(LCC3Vector4 other)
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

        public LCC3Vector4 NormalizedVector()
        {
            // Structs copy by value so we get new copy
            Vector4 normalizedXnaVec4 = _xnaVec4;
            normalizedXnaVec4.Normalize();

            return new LCC3Vector4(normalizedXnaVec4);
        }

        public LCC3Vector TruncateToCC3Vector()
        {
            return new LCC3Vector(_xnaVec4.X,_xnaVec4.Y,_xnaVec4.Z);
        }

        public LCC3Vector4 HomogeneousNegate()
        {
            return new LCC3Vector4(-_xnaVec4.X,-_xnaVec4.Y,-_xnaVec4.Z, _xnaVec4.W);
        }

        #endregion Instance methods
    }
}

