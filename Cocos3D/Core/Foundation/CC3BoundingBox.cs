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
    public struct CC3BoundingBox : IEquatable<CC3BoundingBox>
    {
        // Private static fields

        private static CC3BoundingBox _CC3BoundingBoxZero 
            = new CC3BoundingBox(CC3Vector.CC3VectorZero, CC3Vector.CC3VectorZero);
        private static CC3BoundingBox _CC3BoundingBoxNull
            = new CC3BoundingBox(CC3Vector.CC3VectorNull, CC3Vector.CC3VectorNull);

        // Private instance fields

        private BoundingBox _xnaBoundingBox;
        private CC3Vector _min;
        private CC3Vector _max;


        #region Properties

        // Static properties

        public static CC3BoundingBox CC3BoundingBoxZero
        {
            get { return _CC3BoundingBoxZero; }
        }

        public static CC3BoundingBox CC3BoundingBoxNull
        {
            get { return _CC3BoundingBoxNull; }
        }

        // Public instance properties

        public CC3Vector Minimum
        {
            get { return _min; }
        }

        public CC3Vector Maximum
        {
            get { return _max; }
        }

        public CC3Vector Center
        {
            get { return CC3Vector.CC3VectorAverage(_min, _max); }
        }

        // Internal instance properties

        internal BoundingBox XnaBoundingBox
        {
            get { return _xnaBoundingBox; }
        }

        #endregion Properties


        #region Operators

        public static bool operator ==(CC3BoundingBox value1, CC3BoundingBox value2)
        {
            return value1._xnaBoundingBox == value2._xnaBoundingBox;
        }

        public static bool operator !=(CC3BoundingBox value1, CC3BoundingBox value2)
        {
            return value1._xnaBoundingBox != value2._xnaBoundingBox;
        }

        public static CC3BoundingBox operator *(CC3BoundingBox value, float scaleFactor)
        {
            return new CC3BoundingBox(value.Minimum * scaleFactor, value.Maximum * scaleFactor);
        }

        public static CC3BoundingBox operator *(float scaleFactor, CC3BoundingBox value)
        {
            return value * scaleFactor;
        }

        public static CC3BoundingBox operator *(CC3BoundingBox value, CC3Vector scaleVector)
        {
            return new CC3BoundingBox(value.Minimum * scaleVector, value.Maximum * scaleVector);
        }
   
        public static CC3BoundingBox operator *(CC3Vector scaleVector, CC3BoundingBox value)
        {
            return value * scaleVector;
        }

        #endregion Operators


        #region Static methods

        public static CC3BoundingBox Union(CC3BoundingBox box1, CC3BoundingBox box2)
        {
            BoundingBox newXnaBox 
                = BoundingBox.CreateMerged(box1._xnaBoundingBox, box2._xnaBoundingBox);

            return new CC3BoundingBox(newXnaBox);
        }

        #endregion Static methods


        #region Constructors

        public CC3BoundingBox(CC3Vector min, CC3Vector max)
        {
            // Structures copy by value
            _min = min;
            _max = max;

            _xnaBoundingBox = new BoundingBox(_min.XnaVector, _max.XnaVector);
        }

        public CC3BoundingBox(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        : this(new CC3Vector(minX, minY, minZ), new CC3Vector(maxX, maxY, maxZ))
        {

        }

        public CC3BoundingBox(CC3BoundingBox box, CC3Vector paddingVec)
        : this(box._min + paddingVec, box._max + paddingVec)
        {

        }

        public CC3BoundingBox(CC3BoundingBox box, float uniformPadding)
        : this(box._min + new CC3Vector(uniformPadding), box._max + new CC3Vector(uniformPadding))
        {

        }

        internal CC3BoundingBox(BoundingBox xnaBoundingBox)
        {
            // Structures copy by value
            _xnaBoundingBox = xnaBoundingBox;

            _min = new CC3Vector(_xnaBoundingBox.Min);
            _max = new CC3Vector(_xnaBoundingBox.Max);
        }

        #endregion Constructors


        #region Instance methods

        // Equality handling

        public override bool Equals(object obj)
        {
            // Using overloaded == operator
            return (obj is CC3BoundingBox) ? this == (CC3BoundingBox)obj : false;
        }

        public bool Equals(CC3BoundingBox other)
        {
            // Using overloaded == operator
            return this == other;
        }

        public override int GetHashCode()
        {
            return _xnaBoundingBox.GetHashCode();
        }

        // Other instance methods

        public bool Contains(CC3Vector vec)
        {
            return _xnaBoundingBox.Contains(vec.XnaVector) != ContainmentType.Disjoint;
        }

        #endregion Instance methods
    }
}

