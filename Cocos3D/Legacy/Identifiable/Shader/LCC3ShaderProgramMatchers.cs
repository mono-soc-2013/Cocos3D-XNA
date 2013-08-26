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
    public class LCC3ShaderProgramMatchers
    {
        // Instance fields

        ILCC3ShaderSemanticDelegate _semanticDelegate;

        #region Properties

        public ILCC3ShaderSemanticDelegate SemanticDelegate
        {
            get { return _semanticDelegate; }
        }

        #endregion Properties


        #region Constructors

        public LCC3ShaderProgramMatchers()
        {
            LCC3ShaderProgramSemanticsByVarName sd = new LCC3ShaderProgramSemanticsByVarName();
            sd.PopulateWithDefaultVariableNameMappings();
            _semanticDelegate = sd;
        }

        #endregion Constructors


        #region Program options

        private LCC3ShaderProgram ProgramFromShaderFile(string shaderFileResource)
        {
            return new LCC3ShaderProgram(LCC3ShaderProgram.NextTag(), shaderFileResource, _semanticDelegate, shaderFileResource, true);
        }

        public LCC3ShaderProgram SingleTextureProgram(bool shouldAlphaTest=false)
        {
            // TBA
            return null;
        }

        public LCC3ShaderProgram ConfigurableProgram(bool shouldAlphaTest=false)
        {
            string shaderFileResource = null;

#if DIRECTX
            shaderFileResource = "Cocos3D.Legacy.Identifiable.Shader.Resources.CC3MultiTextureConfigurable.dx11.mgfxo";
#elif PSM 
            throw new NotImplementedException("PSM shader not implemented");
#else
            shaderFileResource = "Cocos3D.Legacy.Identifiable.Shader.Resources.CC3MultiTextureConfigurable.ogl.mgfxo";
#endif

            return ProgramFromShaderFile(shaderFileResource);
        }

        #endregion Program options
    }
}

