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

namespace Cocos3D
{
    public struct LCC3Face
    {
        // static vars

        static readonly LCC3Face _CC3FaceZero = new LCC3Face(LCC3Vector.CC3VectorZero);

        // ivars

        LCC3Vector _vertex1;
        LCC3Vector _vertex2;
        LCC3Vector _vertex3;


        #region Properties

        // static properties

        public static LCC3Face CC3FaceZero
        {
            get { return _CC3FaceZero; }
        }

        // instance properties

        public LCC3Vector Center
        {
            get
            {
                return (_vertex1 + _vertex2 + _vertex3) / 3;
            }
        }

        public LCC3Vector FaceNormal
        {
            get
            {
                return LCC3Vector.CC3CrossProduct(_vertex2 - _vertex1, _vertex3 - _vertex1);
            }
        }

        public LCC3Plane FacePlane
        {
            get
            {
                return new LCC3Plane(_vertex1, _vertex2, _vertex3);
            }
        }

        #endregion Properties


        #region Constructors

        public LCC3Face(LCC3Vector v1, LCC3Vector v2, LCC3Vector v3)
        {
            _vertex1 = v1;
            _vertex2 = v2;
            _vertex3 = v3;
        }

        private LCC3Face(LCC3Vector v): this(v,v,v)
        {
        }

        #endregion Constructors
    }
}

