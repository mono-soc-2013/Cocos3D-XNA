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
    public class CC3TranslationAction
    {
        // Instance fields

        private CC3Vector _translationChange;

        #region Properties

        public CC3Vector TranslationChange
        {
            get { return _translationChange; }
        }

        #endregion Properties


        #region Constructors

        public CC3TranslationAction(CC3Vector translationChange)
        {
            _translationChange = translationChange;
        }

        #endregion Constructors


        #region Getting transform changes for a subinterval of time

        internal CC3Vector IncrementalTranslationChange(float timeElapsedFraction, float timeIncrementFraction)
        {
            return _translationChange * timeIncrementFraction;
        }

        #endregion Getting transform changes for a subinterval of time
    }
}

