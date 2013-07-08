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
    public class CC3TranslationActionBuilder : CC3ActionBuilder
    {
        // Instance fields

        protected CC3Vector _translationChange;


        #region Constructors

        public CC3TranslationActionBuilder() : base()
        {
        }

        #endregion Constructors


        #region Resetting builder

        public override void Reset()
        {
            _translationChange = CC3Vector.CC3VectorZero;
        }

        #endregion Resetting builder


        #region Building translation action methods

        public CC3TranslationAction Build()
        {
            return new CC3TranslationAction(_translationChange);
        }

        #endregion Building translation action methods


        #region Moving methods

        public CC3ActionBuilder MoveByTranslationOffset(CC3Vector translationOffset)
        {
            _translationChange = translationOffset;
            return this;
        }

        #endregion Moving methods
    }
}

