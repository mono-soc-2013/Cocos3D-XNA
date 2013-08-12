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
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3D
{
   
    public class LCC3ShaderUniform : LCC3ShaderVariable
    {
        // Instance fields

        private object _varValue;


        #region Properties

        public object VarValue
        {
            get { return _varValue; }
        }

        internal float FloatValue
        {
            get 
            { 
                float floatValue = 0.0f;

                if (_type == LCC3ShaderVariableType.Float)
                {
                    floatValue = (float)_varValue;
                }

                return floatValue; 
            }
        }

        internal Matrix XnaMatrix
        {
            get 
            { 
                Matrix xnaMatrix = Matrix.Identity;

                if (_type == LCC3ShaderVariableType.Matrix)
                {
                    LCC3Matrix4x4 matrix = _varValue as LCC3Matrix4x4;
                    xnaMatrix = matrix.XnaMatrix;
                }

                return xnaMatrix; 
            }
        }

        internal Texture2D XnaTexture2D
        {
            get 
            { 
                Texture2D xnaTex2D = null;

                if (_type == LCC3ShaderVariableType.Texture2D)
                {
                    LCC3GraphicsTexture2D texture = _varValue as LCC3GraphicsTexture2D;
                    xnaTex2D = texture.XnaTexture2D;
                }

                return xnaTex2D; 
            }
        }

        internal Vector3 XnaVector3
        {
            get 
            { 
                Vector3 xnaVec3 = Vector3.Zero;

                if (_type == LCC3ShaderVariableType.Vector3)
                {
                    LCC3Vector vec3 = (LCC3Vector)_varValue;
                    xnaVec3 = vec3.XnaVector;
                }

                return xnaVec3; 
            }
        }

        internal Vector4 XnaVector4
        {
            get 
            { 
                Vector4 xnaVec4 = Vector4.Zero;

                if (_type == LCC3ShaderVariableType.Vector4)
                {
                    LCC3Vector4 vec4 = (LCC3Vector4)_varValue;
                    xnaVec4 = vec4.XnaVector4;
                }

                return xnaVec4; 
            }
        }

        #endregion Properties


        #region Allocation and initialization

        public static LCC3ShaderUniform VariableInProgram(LCC3ShaderProgram program, int index)
        {
            return new LCC3ShaderUniform(program, index);
        }

        public LCC3ShaderUniform() : base()
        {
        }

        public LCC3ShaderUniform(LCC3ShaderProgram program, int index) : base(program, index)
        {

        }

        public void PopulateFrom(LCC3ShaderUniform uniform)
        {
            base.PopulateFrom(uniform);
        }

        public override void PopulateFromProgram()
        {
            _semantic = LCC3Semantic.SemanticNone;
            _semanticIndex = 0;
            _name = _program.UniformNameAtIndex(_index);
        }

        #endregion Allocation and initialization


        #region Accessing uniform values

        public void SetValue(object value)
        {
            _varValue = value;
        }

        public void SetValueFromUniform(LCC3ShaderUniform uniform)
        {
            _varValue = uniform.VarValue;
        }

        public virtual bool UpdateShaderValue()
        {
            _program.UpdateUniformValue(this);

            return true;
        }

        #endregion Accessing uniform values

    }
}

