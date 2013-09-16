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
    public struct LCC3BoundingBox : IEquatable<LCC3BoundingBox>
    {
        // Static vars

        static LCC3BoundingBox _CC3BoundingBoxZero 
        = new LCC3BoundingBox(LCC3Vector.CC3VectorZero, LCC3Vector.CC3VectorZero);
        static LCC3BoundingBox _CC3BoundingBoxNull
        = new LCC3BoundingBox(LCC3Vector.CC3VectorNull, LCC3Vector.CC3VectorNull);

        // ivars

        BoundingBox _xnaBoundingBox;
        LCC3Vector _min;
        LCC3Vector _max;

        #region Properties

        // Static properties

        public static LCC3BoundingBox CC3BoundingBoxZero
        {
            get { return _CC3BoundingBoxZero; }
        }

        public static LCC3BoundingBox CC3BoundingBoxNull
        {
            get { return _CC3BoundingBoxNull; }
        }

        // Public instance properties

        public LCC3Vector Minimum
        {
            get { return _min; }
        }

        public LCC3Vector Maximum
        {
            get { return _max; }
        }

        public LCC3Vector Center
        {
            get { return LCC3Vector.CC3VectorAverage(_min, _max); }
        }

        // Internal instance properties

        internal BoundingBox XnaBoundingBox
        {
            get { return _xnaBoundingBox; }
        }

        #endregion Properties


        #region Operators

        public static bool operator ==(LCC3BoundingBox value1, LCC3BoundingBox value2)
        {
            return value1._xnaBoundingBox == value2._xnaBoundingBox;
        }

        public static bool operator !=(LCC3BoundingBox value1, LCC3BoundingBox value2)
        {
            return value1._xnaBoundingBox != value2._xnaBoundingBox;
        }

        public static LCC3BoundingBox operator *(LCC3BoundingBox value, float scaleFactor)
        {
            return new LCC3BoundingBox(value.Minimum * scaleFactor, value.Maximum * scaleFactor);
        }

        public static LCC3BoundingBox operator *(float scaleFactor, LCC3BoundingBox value)
        {
            return value * scaleFactor;
        }

        public static LCC3BoundingBox operator *(LCC3BoundingBox value, LCC3Vector scaleVector)
        {
            return new LCC3BoundingBox(value.Minimum * scaleVector, value.Maximum * scaleVector);
        }

        public static LCC3BoundingBox operator *(LCC3Vector scaleVector, LCC3BoundingBox value)
        {
            return value * scaleVector;
        }

        #endregion Operators


        #region Static methods

        public static LCC3BoundingBox Union(LCC3BoundingBox box1, LCC3BoundingBox box2)
        {
            BoundingBox newXnaBox 
                = BoundingBox.CreateMerged(box1._xnaBoundingBox, box2._xnaBoundingBox);

            return new LCC3BoundingBox(newXnaBox);
        }

        #endregion Static methods


        #region Constructors

        public LCC3BoundingBox(LCC3Vector min, LCC3Vector max)
        {
            // Structures copy by value
            _min = min;
            _max = max;

            _xnaBoundingBox = new BoundingBox(_min.XnaVector, _max.XnaVector);
        }

        public LCC3BoundingBox(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        : this(new LCC3Vector(minX, minY, minZ), new LCC3Vector(maxX, maxY, maxZ))
        {

        }

        public LCC3BoundingBox(LCC3BoundingBox box, LCC3Vector paddingVec)
        : this(box._min + paddingVec, box._max + paddingVec)
        {

        }

        public LCC3BoundingBox(LCC3BoundingBox box, float uniformPadding)
        : this(box._min + new LCC3Vector(uniformPadding), box._max + new LCC3Vector(uniformPadding))
        {

        }

        internal LCC3BoundingBox(BoundingBox xnaBoundingBox)
        {
            // Structures copy by value
            _xnaBoundingBox = xnaBoundingBox;

            _min = new LCC3Vector(_xnaBoundingBox.Min);
            _max = new LCC3Vector(_xnaBoundingBox.Max);
        }

        #endregion Constructors


        #region Instance methods

        // Equality handling

        public override bool Equals(object obj)
        {
            // Using overloaded == operator
            return (obj is LCC3BoundingBox) ? this == (LCC3BoundingBox)obj : false;
        }

        public bool Equals(LCC3BoundingBox other)
        {
            // Using overloaded == operator
            return this == other;
        }

        public override int GetHashCode()
        {
            return _xnaBoundingBox.GetHashCode();
        }

        // Other instance methods

        public bool Contains(LCC3Vector vec)
        {
            return _xnaBoundingBox.Contains(vec.XnaVector) != ContainmentType.Disjoint;
        }

        #endregion Instance methods
    }
}



