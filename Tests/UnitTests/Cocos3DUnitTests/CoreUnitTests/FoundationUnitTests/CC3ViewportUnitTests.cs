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
using Cocos2D;

namespace Cocos3DTests
{
    [TestFixture()]
	public class CC3ViewportUnitTests
    {
        // Private static fields

        private static readonly CC3Viewport _vp = new CC3Viewport(1, 2, 3, 4);
        private static readonly CC3Viewport _vpXChange = new CC3Viewport(_vp, -1, 0, 0, 0);
        private static readonly CC3Viewport _vpYChange = new CC3Viewport(_vp, 0, -1, 0, 0);
        private static readonly CC3Viewport _vpWidthChange = new CC3Viewport(_vp, 0, 0, -1, 0);
        private static readonly CC3Viewport _vpHeightChange = new CC3Viewport(_vp, 0, 0, 0, -1);

        #region Viewport operator tests

        [Test()]
        public void Is_Equality_Operator_Robust()
        {
            Assert.IsFalse(_vp == _vpXChange);
            Assert.IsFalse(_vp == _vpYChange);
            Assert.IsFalse(_vp == _vpWidthChange);
            Assert.IsFalse(_vp == _vpHeightChange);
        }

        [Test()]
        public void Is_Equality_Operator_Symmetric()
        {
            Assert.IsTrue((_vp == _vpXChange) == (_vpXChange == _vp));
        }

        [Test()]
        public void Is_Equality_Operator_Transitive()
        {
            bool condition1 = (_vp == _vpXChange) && (_vp == _vpYChange);
            bool condition2 = _vpXChange == _vpYChange;

            Assert.IsTrue(condition1 == condition2);
        }

        #endregion Viewport operator tests


        #region Viewport calculation instance method tests

        [Test()]
        public void Does_ContainsPoint_Handles_Degenerate_Case()
        {
            CC3Viewport degenerateViewport = new CC3Viewport(1, 2, 0, 0);

            Assert.IsTrue(degenerateViewport.ContainsPoint(new CCPoint(1,2)));
            Assert.IsFalse(degenerateViewport.ContainsPoint(new CCPoint(1,3)));
        }

        [Test()]
        public void Is_ContainsPoint_Robust()
        {
            CCRect vpRect = new CCRect(5.0f, -5.0f, 15.0f, 20.0f);
            CCSize vpSize = vpRect.Size;
            CC3Viewport vp = new CC3Viewport(vpRect);

            // General sanity test
            Assert.IsTrue(vp.ContainsPoint(vpRect.Origin + new CCPoint(vpSize.Width / 2.0f, vpSize.Height / 2.0f)));

            // Checking corners of viewport
            Assert.IsTrue(vp.ContainsPoint(vpRect.Origin));
            Assert.IsTrue(vp.ContainsPoint(vpRect.Origin + new CCPoint(vpSize.Width, 0.0f)));
            Assert.IsTrue(vp.ContainsPoint(vpRect.Origin + new CCPoint(0.0f, vpSize.Height)));
            Assert.IsTrue(vp.ContainsPoint(vpRect.Origin + new CCPoint(vpSize.Width, vpSize.Height)));

            // Test sensitivity to decimal changes in coordinates
            // Floats are cast as ints in viewport fields
            Assert.IsFalse(vp.ContainsPoint(vpRect.Origin + new CCPoint(-0.1f, 0.0f)));
            Assert.IsTrue(vp.ContainsPoint(vpRect.Origin + new CCPoint(0.1f, 0.0f)));
        }

        #endregion Viewport calculation instance method tests
    }
}

