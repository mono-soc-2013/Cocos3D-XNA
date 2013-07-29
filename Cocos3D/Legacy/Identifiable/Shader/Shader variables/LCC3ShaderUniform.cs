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
   
    public class LCC3ShaderUniform : LCC3ShaderVariable
    {
        // Instance fields

        private object _varValue;


        #region Properties

        public object VarValue
        {
            get { return _varValue; }
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
            _semanticVertex = LCC3SemanticVertex.SemanticNone;
            _semanticVertexIndex = 0;
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

        public virtual bool UpdateValue()
        {
            _program.UpdateUniformValue(this);

            return true;
        }

        #endregion Accessing uniform values

    }
}

