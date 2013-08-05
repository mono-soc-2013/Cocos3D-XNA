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
using Cocos2D;

namespace Cocos3D
{
    public class LCC3NodeDrawingVisitor : LCC3NodeVisitor
    {
        // Instance fields

        private LCC3ProgPipeline _progPipeline;
        private LCC3ShaderProgram _currentShaderProgram;
        private uint _currentTextureUnitIndex;
        private CCColor4F _currentColor;
        private uint _textureUnitCount;

        #region Properties

        public LCC3ProgPipeline ProgramPipeline
        {
            get { return _progPipeline; }
            set { _progPipeline = value; }
        }

        public LCC3ShaderProgram CurrentShaderProgram
        {
            get { return _currentShaderProgram; }
            set { _currentShaderProgram = value; }
        }

        public uint CurrentTextureUnitIndex
        {
            get { return _currentTextureUnitIndex; }
            set { _currentTextureUnitIndex = value; }
        }

        public CCColor4F CurrentColor
        {
            get { return _currentColor; }
            set { _currentColor = value; }
        }

        #endregion Properties

        public LCC3NodeDrawingVisitor()
        {
        }

        public void DisableUnusedTextureUnits()
        {
            _textureUnitCount = _currentTextureUnitIndex;
        }
    }
}

