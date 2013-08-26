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
    public class LCC3ShaderVariableConfiguration
    {
        // Instance variables

        private string _name;
        private LCC3Semantic _semantic;
        private uint _semanticIndex;
        private LCC3ElementType _type;
        private uint _size;


        #region Properties

        internal string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        internal LCC3Semantic Semantic
        {
            get { return _semantic; }
            set { _semantic = value; }
        }

        internal uint SemanticIndex
        {
            get { return _semanticIndex; }
            set { _semanticIndex = value; }
        }

        internal LCC3ElementType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        internal uint Size
        {
            get { return _size; }
            set { _size = value; }
        }

        #endregion Properties


        #region Constructors

        public LCC3ShaderVariableConfiguration()
        {
            _semantic = LCC3Semantic.SemanticNone;
            _type = LCC3ElementType.None;
        }

        #endregion Constructors
    }
}

