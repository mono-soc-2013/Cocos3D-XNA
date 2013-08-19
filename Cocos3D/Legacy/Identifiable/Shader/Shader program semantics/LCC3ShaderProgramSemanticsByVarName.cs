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
    public class LCC3ShaderProgramSemanticsByVarName : LCC3ShaderProgramSemanticsBase
    {
        // Instance fields

        private Dictionary<string, LCC3ShaderVariableConfiguration> _varConfigsByName;


        #region Allocation and initialization

        public LCC3ShaderProgramSemanticsByVarName()
        {
            _varConfigsByName = new Dictionary<string, LCC3ShaderVariableConfiguration>();
        }

        #endregion Allocation and initialization


        #region Variable configuration

        public override bool ConfigureVariable(LCC3ShaderVariable variable)
        {
            LCC3ShaderVariableConfiguration varConfig = _varConfigsByName[variable.Name];

            if (varConfig != null) 
            {
                variable.Semantic = varConfig.Semantic;
                variable.SemanticIndex = varConfig.SemanticIndex;
                variable.Type = varConfig.Type;
                variable.Scope = this.VariableScopeForSemantic(varConfig.Semantic);
                return true;
            }

            return false;
        }

        public void AddVariableConfiguration(LCC3ShaderVariableConfiguration varConfig)
        {
            _varConfigsByName[varConfig.Name] = varConfig;
        }

        private void MapVarNameToSemantic(string name, LCC3Semantic semantic, uint semanticIndex, LCC3ElementType elementType)
        {
            LCC3ShaderVariableConfiguration varConfig = new LCC3ShaderVariableConfiguration();
            varConfig.Name = name;
            varConfig.Semantic = semantic;
            varConfig.SemanticIndex = semanticIndex;
            varConfig.Type = elementType;

            this.AddVariableConfiguration(varConfig);
        }

        private void MapVarNameToSemantic(string name, LCC3Semantic semantic, LCC3ElementType elementType)
        {
            this.MapVarNameToSemantic(name, semantic, 0, elementType);
        }

        #endregion Variable configuration


        #region Default mappings

        public void PopulateWithDefaultVariableNameMappings()
        {
            // Vertex state
            this.MapVarNameToSemantic("u_cc3VertexHasNormal", LCC3Semantic.SemanticHasVertexNormal, LCC3ElementType.Boolean);    
            this.MapVarNameToSemantic("u_cc3VertexHasTangent", LCC3Semantic.SemanticHasVertexTangent, LCC3ElementType.Boolean);    
            this.MapVarNameToSemantic("u_cc3VertexHasBitangent", LCC3Semantic.SemanticHasVertexBitangent, LCC3ElementType.Boolean);    
            this.MapVarNameToSemantic("u_cc3VertexHasColor", LCC3Semantic.SemanticHasVertexColor, LCC3ElementType.Boolean);    
            this.MapVarNameToSemantic("u_cc3VertexHasWeights", LCC3Semantic.SemanticHasVertexWeight, LCC3ElementType.Boolean);    
            this.MapVarNameToSemantic("u_cc3VertexHasMatrixIndices", LCC3Semantic.SemanticHasVertexMatrixIndex, LCC3ElementType.Boolean);    
            this.MapVarNameToSemantic("u_cc3VertexHasTexCoord", LCC3Semantic.SemanticHasVertexTextureCoordinate, LCC3ElementType.Boolean);    
            this.MapVarNameToSemantic("u_cc3VertexHasPointSize", LCC3Semantic.SemanticHasVertexPointSize, LCC3ElementType.Boolean);    
            this.MapVarNameToSemantic("u_cc3VertexShouldNormalizeNormal", LCC3Semantic.SemanticShouldNormalizeVertexNormal, LCC3ElementType.Boolean);    
            this.MapVarNameToSemantic("u_cc3VertexShouldRescaleNormal", LCC3Semantic.SemanticShouldRescaleVertexNormal, LCC3ElementType.Boolean); 

            // Materials
            this.MapVarNameToSemantic("u_cc3Color", LCC3Semantic.SemanticColor, LCC3ElementType.Vector4);                                 
            this.MapVarNameToSemantic("u_cc3MaterialAmbientColor", LCC3Semantic.SemanticMaterialColorAmbient, LCC3ElementType.Vector4);   
            this.MapVarNameToSemantic("u_cc3MaterialDiffuseColor", LCC3Semantic.SemanticMaterialColorDiffuse, LCC3ElementType.Vector4);   
            this.MapVarNameToSemantic("u_cc3MaterialSpecularColor", LCC3Semantic.SemanticMaterialColorSpecular, LCC3ElementType.Vector4); 
            this.MapVarNameToSemantic("u_cc3MaterialEmissionColor", LCC3Semantic.SemanticMaterialColorEmission, LCC3ElementType.Vector4);
            this.MapVarNameToSemantic("u_cc3MaterialOpacity", LCC3Semantic.SemanticMaterialOpacity, LCC3ElementType.Float);             
            this.MapVarNameToSemantic("u_cc3MaterialShininess", LCC3Semantic.SemanticMaterialShininess, LCC3ElementType.Float);         
            this.MapVarNameToSemantic("u_cc3MaterialReflectivity", LCC3Semantic.SemanticMaterialReflectivity, LCC3ElementType.Float);   
            this.MapVarNameToSemantic("u_cc3MaterialMinimumDrawnAlpha", LCC3Semantic.SemanticMinimumDrawnAlpha, LCC3ElementType.Float);

            // Matrices
            this.MapVarNameToSemantic("u_cc3MatrixModelView", LCC3Semantic.SemanticModelViewMatrix, LCC3ElementType.Float4x4); 
            this.MapVarNameToSemantic("u_cc3MatrixProj", LCC3Semantic.SemanticModelViewProjMatrix, LCC3ElementType.Float4x4);
            this.MapVarNameToSemantic("u_cc3MatrixModelViewInvTran", LCC3Semantic.SemanticModelViewMatrixInvTran, LCC3ElementType.Float3x3);

            // Lighting
            this.MapVarNameToSemantic("u_cc3LightIsUsingLighting", LCC3Semantic.SemanticIsUsingLighting, LCC3ElementType.Boolean);  
            this.MapVarNameToSemantic("u_cc3LightSceneAmbientLightColor", LCC3Semantic.SemanticSceneLightColorAmbient, LCC3ElementType.Vector4); 

            for(uint semanticIndex=0; semanticIndex < LCC3Light.DefaultMaxNumOfLights; semanticIndex++)
            {
                this.MapVarNameToSemantic("u_cc3LightIsLightEnabled", LCC3Semantic.SemanticLightIsEnabled, semanticIndex, LCC3ElementType.BooleanArray);
                this.MapVarNameToSemantic("u_cc3LightPositionEyeSpace", LCC3Semantic.SemanticLightPositionEyeSpace, semanticIndex, LCC3ElementType.Vector4Array);
                this.MapVarNameToSemantic("u_cc3LightDiffuseColor", LCC3Semantic.SemanticLightColorDiffuse, semanticIndex, LCC3ElementType.Vector4Array);
                this.MapVarNameToSemantic("u_cc3LightAmbientColor", LCC3Semantic.SemanticLightColorAmbient, semanticIndex, LCC3ElementType.Vector4Array);
                this.MapVarNameToSemantic("u_cc3LightSpecularColor", LCC3Semantic.SemanticLightColorSpecular, semanticIndex, LCC3ElementType.Vector4Array);
            }
        }

        #endregion Default mappings
    }
}

