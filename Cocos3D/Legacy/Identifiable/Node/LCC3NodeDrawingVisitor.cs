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
        // ivars

        LCC3ProgPipeline _progPipeline;
        LCC3ShaderProgram _currentShaderProgram;
        uint _currentTextureUnitIndex;
        CCColor4F _currentColor;
        uint _textureUnitCount;

        LCC3Matrix4x4 _viewMatrix;
        LCC3Matrix4x4 _projMatrix;
        LCC3Matrix4x4 _modelMatrix;
        LCC3Matrix4x4 _modelViewMatrix;
        LCC3Matrix4x4 _viewProjMatrix;
        LCC3Matrix4x4 _modelViewProjMatrix;

        bool _isVPMtxDirty;
        bool _isMVMtxDirty;
        bool _isMVPMtxDirty;

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

        public LCC3Matrix4x4 ViewMatrix
        {
            get { return _viewMatrix; }
            set
            { 
                _viewMatrix = value;
                _isVPMtxDirty = true;
                _isMVMtxDirty = true;
                _isMVPMtxDirty = true;
            }
        }

        public LCC3Matrix4x4 ModelMatrix
        {
            get { return _modelMatrix; }
            set
            { 
                _modelMatrix = value;
                _isMVMtxDirty = true;
                _isMVPMtxDirty = true;
            }
        }

        public LCC3Matrix4x4 ProjMatrix
        {
            get { return _projMatrix; }
            set
            { 
                _projMatrix = value;
                _isVPMtxDirty = true;
                _isMVPMtxDirty = true;
            }
        }

        
        public LCC3Matrix4x4 ModelViewMatrix
        {
            get 
            { 
                if (_isMVMtxDirty == true)
                {
                    _modelViewMatrix = _modelMatrix * _viewMatrix;
                    _isMVMtxDirty = false;
                }
                return _modelViewMatrix; 
            }
        }

        public LCC3Matrix4x4 ViewProjMatrix
        {
            get 
            { 
                if (_isVPMtxDirty == true)
                {
                    _viewProjMatrix = _viewMatrix * _projMatrix;
                    _isVPMtxDirty = false;
                }
                return _viewProjMatrix; 
            }
        }

        public LCC3Matrix4x4 ModelViewProjMatrix
        {
            get 
            { 
                if (_isMVPMtxDirty == true)
                {
                    _modelViewProjMatrix = _modelMatrix * _viewMatrix * _projMatrix;
                    _isMVPMtxDirty = false;
                }
                return _modelViewProjMatrix; 
            }
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

