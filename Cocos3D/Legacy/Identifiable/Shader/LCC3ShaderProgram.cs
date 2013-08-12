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
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos3D
{
    public class LCC3ShaderProgram : LCC3Identifiable
    {
        // Static fields

        private static int _lastAssignedProgramTag = 0;
        private static Dictionary<string, LCC3ShaderProgram> _programsByName 
            = new Dictionary<string, LCC3ShaderProgram>();

        // Instance fields

        private Effect _xnaShaderEffect;

        private ILCC3ShaderSemanticDelegate _semanticDelegate;

        private List<LCC3ShaderUniform> _uniformsSceneScope;
        private List<LCC3ShaderUniform> _uniformsNodeScope;
        private List<LCC3ShaderUniform> _uniformsDrawScope;
        private List<LCC3ShaderAttribute> _attributes;

        private string _shaderPreamble;

        private bool _isSceneScopeDirty;

        #region Properties
       
        public string PlatformPreamble
        {
            get { return LCC3ProgPipeline.SharedPipeline().DefaultShaderPreamble(); }
        }

        public Effect XnaShaderEffect
        {
            get { return _xnaShaderEffect; }
        }

        #endregion Properties


        #region Allocation and initialization

        public LCC3ShaderProgram(int tag, string name) : base(tag, name)
        {
            _uniformsSceneScope = new List<LCC3ShaderUniform>();
            _uniformsNodeScope = new List<LCC3ShaderUniform>();
            _uniformsDrawScope = new List<LCC3ShaderUniform>();
            _attributes = new List<LCC3ShaderAttribute>();

            _shaderPreamble = this.PlatformPreamble;

            _isSceneScopeDirty = true;
        }

        public LCC3ShaderProgram(int tag, string name, ILCC3ShaderSemanticDelegate semanticDelegate, string shaderFilename)
            : this(tag, name)
        {
            _semanticDelegate = semanticDelegate;
            this.CompileAndLinkShaderFile(shaderFilename);
        }

        #endregion Allocation and initialization


        #region Tag allocation

        public new static void ResetTagAllocation()
        {
            _lastAssignedProgramTag = 0;
        }

        protected override int NextTag()
        {
            return ++_lastAssignedProgramTag;
        }

        #endregion Tag allocation


        #region Variables

        public string UniformNameAtIndex(int indexIn)
        {
            EffectParameter _xnaEffectParam = _xnaShaderEffect.Parameters[indexIn];
            return _xnaEffectParam.Name;
        }

        public LCC3ShaderUniform UniformNamed(string varName)
        {
            foreach (LCC3ShaderUniform uniform in _uniformsSceneScope)
            {
                if (uniform.Name == varName)
                {
                    return uniform;
                }
            }

            foreach (LCC3ShaderUniform uniform in _uniformsNodeScope)
            {
                if (uniform.Name == varName)
                {
                    return uniform;
                }
            }

            foreach (LCC3ShaderUniform uniform in _uniformsDrawScope)
            {
                if (uniform.Name == varName)
                {
                    return uniform;
                }
            }

            return null;
        }

        public LCC3ShaderUniform UniformAtLocation(LCC3VertexAttrIndex uniformLocation)
        {
            foreach (LCC3ShaderUniform uniform in _uniformsSceneScope)
            {
                if (uniform.Location == uniformLocation)
                {
                    return uniform;
                }
            }

            foreach (LCC3ShaderUniform uniform in _uniformsNodeScope)
            {
                if (uniform.Location == uniformLocation)
                {
                    return uniform;
                }
            }

            foreach (LCC3ShaderUniform uniform in _uniformsDrawScope)
            {
                if (uniform.Location == uniformLocation)
                {
                    return uniform;
                }
            }
           
            return null;
        }

        public LCC3ShaderUniform UniformForSemantic(LCC3Semantic semantic)
        {
            return this.UniformForSemantic(semantic, 0);
        }

        public LCC3ShaderUniform UniformForSemantic(LCC3Semantic semantic, int semanticIndex)
        {
            foreach (LCC3ShaderUniform uniform in _uniformsSceneScope)
            {
                if (uniform.Semantic == semantic && uniform.SemanticIndex == semanticIndex)
                {
                    return uniform;
                }
            }

            foreach (LCC3ShaderUniform uniform in _uniformsNodeScope)
            {
                if (uniform.Semantic == semantic && uniform.SemanticIndex == semanticIndex)
                {
                    return uniform;
                }
            }

            foreach (LCC3ShaderUniform uniform in _uniformsDrawScope)
            {
                if (uniform.Semantic == semantic && uniform.SemanticIndex == semanticIndex)
                {
                    return uniform;
                }
            }

            return null;
        }

        internal void UpdateUniformValue(LCC3ShaderUniform uniform)
        {
            EffectParameter _xnaEffectParam = _xnaShaderEffect.Parameters[uniform.Name];

            if (_xnaEffectParam != null)
            {
                if (uniform.Type == LCC3ShaderVariableType.Matrix)
                {
                    _xnaEffectParam.SetValue(uniform.XnaMatrix);
                }
                else if (uniform.Type == LCC3ShaderVariableType.Texture2D)
                {
                    _xnaEffectParam.SetValue(uniform.XnaTexture2D);
                }
                else if (uniform.Type == LCC3ShaderVariableType.Vector4)
                {
                    _xnaEffectParam.SetValue(uniform.XnaVector4);
                }
                else if (uniform.Type == LCC3ShaderVariableType.Vector3)
                {
                    _xnaEffectParam.SetValue(uniform.XnaVector3);
                }
                else if (uniform.Type == LCC3ShaderVariableType.Float)
                {
                    _xnaEffectParam.SetValue(uniform.FloatValue);
                }
            }
        }

        public LCC3ShaderAttribute AttributeNamed(string varName)
        {
            foreach (LCC3ShaderAttribute attribute in _attributes)
            {
                if (attribute.Name == varName)
                {
                    return attribute;
                }
            }

            return null;
        }

        public LCC3ShaderAttribute AttributeAtLocation(LCC3VertexAttrIndex attributeLocation)
        {
            foreach (LCC3ShaderAttribute attribute in _attributes)
            {
                if (attribute.Location == attributeLocation)
                {
                    return attribute;
                }
            }

            return null;
        }

        public LCC3ShaderAttribute AttributeForSemantic(LCC3Semantic semantic)
        {
            return this.AttributeForSemantic(semantic, 0);
        }

        public LCC3ShaderAttribute AttributeForSemantic(LCC3Semantic semantic, uint semanticIndex)
        {
            foreach (LCC3ShaderAttribute attribute in _attributes)
            {
                if (attribute.Semantic == semantic && attribute.SemanticIndex == semanticIndex)
                {
                    return attribute;
                }
            }

            return null;
        }

        public void MarkSceneScopeDirty()
        {
            _isSceneScopeDirty = true;
        }

        public void WillBeginDrawingScene()
        {
            this.MarkSceneScopeDirty();
        }

        #endregion Variables


        #region Compiling and linking

        internal static byte[] LoadEffectResource(string name)
        {
            var stream = File.OpenRead(name); //assembly.GetManifestResourceStream(name);
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public void CompileAndLinkShaderFile(string shaderFilename)
        {
            //_xnaShaderEffect = LCC3ProgPipeline.SharedPipeline().XnaGame.Content.Load<Effect>(shaderFilename);
            _xnaShaderEffect = new Effect(LCC3ProgPipeline.SharedPipeline().XnaGraphicsDevice, 
                                          LCC3ShaderProgram.LoadEffectResource(shaderFilename));

            this.ConfigureUniforms();
        }

        public void ConfigureUniforms()
        {
            _uniformsSceneScope.Clear();
            _uniformsNodeScope.Clear();
            _uniformsDrawScope.Clear();

            for (int i = 0; i < _xnaShaderEffect.Parameters.Count; i++)
            {
                LCC3ShaderUniform uniform = LCC3ShaderUniform.VariableInProgram(this, i);
                _semanticDelegate.ConfigureVariable(uniform);
                this.AddUniform(uniform);
            }
        }

        public void AddUniform(LCC3ShaderUniform uniform)
        {
            switch (uniform.Scope)
            {
                case LCC3ShaderVariableScope.ScopeScene:
                    _uniformsSceneScope.Add(uniform);
                    break;
                case LCC3ShaderVariableScope.ScopeDraw:
                    _uniformsDrawScope.Add(uniform);
                    break;
                default:
                    _uniformsNodeScope.Add(uniform);
                    break;
            }
        }

        public void ConfigureAttributes()
        {
            _attributes.Clear();

            LCC3ProgPipeline pipeline = LCC3ProgPipeline.SharedPipeline();

            foreach(LCC3VertexAttrIndex vertexAttrIndex in pipeline.EnabledVertexAttributeIndices())
            {
                LCC3ShaderAttribute attr = LCC3ShaderAttribute.VariableInProgram(this, vertexAttrIndex);
                _semanticDelegate.ConfigureVariable(attr);
                _attributes.Add(attr);
            }

        }

        #endregion Compiling and linking


        #region Binding

        public void BindWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            visitor.CurrentShaderProgram = this;

            LCC3ProgPipeline.SharedPipeline().CurrentlyActiveShader = this;
        }

        public void PopulateVertexAttributesWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            LCC3ProgPipeline pipeline = LCC3ProgPipeline.SharedPipeline();

            foreach (LCC3ShaderAttribute attribute in _attributes)
            {
                pipeline.BindVertexAttributeWithVisitor(attribute, visitor);
            }
        }

        public void PopulateSceneScopeUniformsWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            if (_isSceneScopeDirty == true)
            {
                this.PopulateUniformsWithVisitor(_uniformsSceneScope, visitor);

                _isSceneScopeDirty = false;
            }
        }

        public void PopulateNodeScopeUniformsWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            this.PopulateSceneScopeUniformsWithVisitor(visitor);

            this.PopulateUniformsWithVisitor(_uniformsNodeScope, visitor);
        }

        public void PopulateDrawScopeUniformsWithVisitor(LCC3NodeDrawingVisitor visitor)
        {
            this.PopulateUniformsWithVisitor(_uniformsDrawScope, visitor);
        }

        public void PopulateUniformsWithVisitor(List<LCC3ShaderUniform> uniforms, LCC3NodeDrawingVisitor visitor)
        {
            LCC3ShaderProgramContext progCtx = visitor.CurrentMaterial.ShaderContext;

            foreach (LCC3ShaderUniform uniform in uniforms)
            {
                if (progCtx.PopulateUniformWithVisitor(uniform, visitor) 
                    || _semanticDelegate.PopulateUniformWithVisitor(uniform, visitor))
                {
                    uniform.UpdateShaderValue();
                } 
            }
        }

        #endregion Binding


        #region Program cache

        public static void AddProgram(LCC3ShaderProgram program)
        {
            if (program == null)
                return;

            Debug.Assert(LCC3ShaderProgram.GetProgramNamed(program.Name) !=null,
                         String.Format(@"Already contains a program named {0} 
                            Remove it first before adding another", program.Name));

            LCC3ShaderProgram._programsByName.Add(program.Name, program);
        }

        public static LCC3ShaderProgram GetProgramNamed(string name)
        {
            return LCC3ShaderProgram._programsByName[name];
        }

        public static void RemoveProgram(LCC3ShaderProgram program)
        {
            LCC3ShaderProgram.RemoveProgramNamed(program.Name);
        }

        public static void RemoveProgramNamed(string name)
        {
            LCC3ShaderProgram._programsByName.Remove(name);
        }

        #endregion Program cache
    }
}

