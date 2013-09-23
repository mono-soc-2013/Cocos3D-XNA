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
using System.Collections.Generic;

namespace Cocos3D
{
    public class LCC3ShaderProgramContext
    {
        // ivars

        LCC3ShaderProgram _program;
        List<LCC3ShaderUniform> _uniforms;
        Dictionary<string, LCC3ShaderUniform> _uniformsByName;

        #region Properties

        public LCC3ShaderProgram Program
        {
            get { return _program; }
            set { _program = value; this.RemoveAllOverrides(); }
        }

        #endregion Properties


        #region Constructors

        public LCC3ShaderProgramContext(LCC3ShaderProgram program)
        {
            _uniforms = new List<LCC3ShaderUniform>();
            _uniformsByName = new Dictionary<string, LCC3ShaderUniform>();
            this.Program = program;
        }

        #endregion Constructors


        #region Variables

        public LCC3ShaderUniform UniformOverrideNamed(string name)
        {
            LCC3ShaderUniform uniform = _uniformsByName[name];

            if (uniform == null)
            {
                uniform = this.AddUniformOverrideForUniform(_program.UniformNamed(name));
            }

            return uniform;
        }

        public LCC3ShaderUniform UniformOverrideForSemantic(LCC3Semantic semantic)
        {
            return this.UniformOverrideForSemantic(semantic, 0);
        }

        public LCC3ShaderUniform UniformOverrideForSemantic(LCC3Semantic semantic, int semanticIndex)
        {
            foreach (LCC3ShaderUniform uniform in _uniforms)
            {
                if (uniform.Semantic == semantic && uniform.SemanticIndex == semanticIndex)
                {
                    return uniform;
                }
            }

            return this.AddUniformOverrideForUniform(_program.UniformForSemantic(semantic, semanticIndex));
        }

        public LCC3ShaderUniform UniformOverrideAtLocation(LCC3VertexAttrIndex location)
        {
            foreach (LCC3ShaderUniform uniform in _uniforms)
            {
                if (uniform.Location == location)
                {
                    return uniform;
                }
            }

            return this.AddUniformOverrideForUniform(_program.UniformAtLocation(location));
        }

        public LCC3ShaderUniform AddUniformOverrideForUniform(LCC3ShaderUniform uniform)
        {
            if (uniform != null)
            {
                LCC3ShaderUniform newUniform = uniform.CloneVariable<LCC3ShaderUniformOverride>();
                _uniformsByName[newUniform.Name] = newUniform;
                _uniforms.Add(newUniform);

                return newUniform;
            }

            return null;
        }

        public void RemoveUniformOverride(LCC3ShaderUniform uniform)
        {
            _uniforms.Remove(uniform);
            _uniformsByName.Remove(uniform.Name);

            if (_uniforms.Count == 0)
            {
                this.RemoveAllOverrides();
            }
        }

        public void RemoveAllOverrides()
        {
            _uniforms.Clear();
            _uniformsByName.Clear();
        }

        #endregion Variables


        #region Drawing

        public bool PopulateUniformWithVisitor(LCC3ShaderUniform uniform, LCC3NodeDrawingVisitor visitor)
        {
            if (uniform.Program != _program)
            {
                return false;
            }

            foreach(LCC3ShaderUniform currentUniform in _uniforms)
            {
                if (currentUniform.Location == uniform.Location)
                {
                    uniform.SetValueFromUniform(currentUniform);
                    return true;
                }
            }

            return false;
        }

        #endregion Drawing
    
    }

}

