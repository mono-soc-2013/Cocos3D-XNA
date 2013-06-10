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
using NUnit.Framework;
using Cocos3D;

namespace Cocos3DTests
{
    [TestFixture()]
	public class CC3VectorUnitTests
    {
        // Private static fields

        private static readonly CC3Vector _vec = new CC3Vector(1.0f, 2.0f, 3.0f);
        private static readonly CC3Vector _vecXChange = new CC3Vector(_vec, -1.0f, 0.0f, 0.0f);
        private static readonly CC3Vector _vecYChange = new CC3Vector(_vec, 0.0f, -1.0f, 0.0f);
        private static readonly CC3Vector _vecZChange = new CC3Vector(_vec, 0.0f, 0.0f, -1.0f);

        #region Vector operator tests

        [Test()]
        public void Is_Equality_Operator_Robust()
        {
            Assert.IsFalse(_vec == _vecXChange);
            Assert.IsFalse(_vec == _vecYChange);
            Assert.IsFalse(_vec == _vecZChange);
        }

        [Test()]
		public void Is_Equality_Operator_Symmetric()
        {
            Assert.IsTrue((_vec == _vecXChange) == (_vecXChange == _vec));
        }

        [Test()]
        public void Is_Equality_Operator_Transitive()
        {
            bool condition1 = (_vec == _vecXChange) && (_vec == _vecYChange);
            bool condition2 = _vecXChange == _vecYChange;

            Assert.IsTrue(condition1 == condition2);
        }

        [Test()]
        public void Is_Multiply_Operator_Commutative()
        {
            Assert.AreEqual(_vec * 2.0f, 2.0f * _vec);
        }

        #endregion Vector operator tests


        #region Equality handling tests

        [Test()]
        public void Does_Equal_Handle_Non_CC3Vector_Object()
        {
            Object obj = new object();

            Assert.AreNotEqual(obj, _vec);
        }

        [Test()]
        public void Does_Equal_Handle_CC3Vector_Object()
        {
            Assert.AreEqual(_vec, _vec);
        }

        #endregion Equality handling tests
       

        #region Vector calculation static method tests

        [Test()]
        public void Is_CC3VectorMinimize_Robust()
        {
            CC3Vector vecA = new CC3Vector(-2.0f, 0.0f, 2.0f);
            CC3Vector vecB = new CC3Vector(2.0f, -2.0f, -2.0f);
            CC3Vector expectedMinVec = new CC3Vector(-2.0f);

            Assert.AreEqual(CC3Vector.CC3VectorMinimize(vecA, vecB), expectedMinVec);
        }

        [Test()]
        public void Is_CC3VectorMaximize_Robust()
        {
            CC3Vector vecA = new CC3Vector(-2.0f, 2.0f, 2.0f);
            CC3Vector vecB = new CC3Vector(2.0f, -2.0f, -2.0f);
            CC3Vector expectedMaxVec = new CC3Vector(2.0f);

            Assert.AreEqual(CC3Vector.CC3VectorMaximize(vecA, vecB), expectedMaxVec);
        }

        #endregion Vector calculation static method tests


        #region Vector calculation instance method tests
       
        [Test()]
        public void Does_NormalizedVector_Leave_Original_Vec_Unchanged()
        {
            CC3Vector normalizedVec = _vec.NormalizedVector();

            Assert.AreNotEqual(_vec, normalizedVec);
        }    

        #endregion Vector calculation instance method tests

    }
}

