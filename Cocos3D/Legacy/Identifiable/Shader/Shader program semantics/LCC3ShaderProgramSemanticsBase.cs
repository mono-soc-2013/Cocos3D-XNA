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
    public class LCC3ShaderProgramSemanticsBase : ILCC3ShaderSemanticDelegate
    {
        #region Allocation and initialization

        public LCC3ShaderProgramSemanticsBase()
        {
        }

        #endregion Allocation and initialization


        #region Querying semantic attributes

        public string NameOfSemantic(LCC3Semantic semantic)
        {
            return null;
        }

        public LCC3ShaderVariableScope VariableScopeForSemantic(LCC3Semantic semantic)
        {
            switch (semantic)
            {
                case LCC3Semantic.SemanticDrawCountCurrentFrame:
                case LCC3Semantic.SemanticRandomNumber:
                case LCC3Semantic.SemanticBoneMatrixCount:
                case LCC3Semantic.SemanticBoneMatricesGlobal:
                case LCC3Semantic.SemanticBoneMatricesInvTranGlobal:
                case LCC3Semantic.SemanticBoneMatricesEyeSpace:
                case LCC3Semantic.SemanticBoneMatricesInvTranEyeSpace:
                case LCC3Semantic.SemanticBoneMatricesModelSpace:
                case LCC3Semantic.SemanticBoneMatricesInvTranModelSpace:
                    return LCC3ShaderVariableScope.ScopeDraw;

                case LCC3Semantic.SemanticViewMatrix:
                case LCC3Semantic.SemanticViewMatrixInv:
                case LCC3Semantic.SemanticViewMatrixInvTran:
                case LCC3Semantic.SemanticProjMatrix:
                case LCC3Semantic.SemanticProjMatrixInv:
                case LCC3Semantic.SemanticProjMatrixInvTran:
                case LCC3Semantic.SemanticViewProjMatrix:
                case LCC3Semantic.SemanticViewProjMatrixInv:
                case LCC3Semantic.SemanticViewProjMatrixInvTran:

                case LCC3Semantic.SemanticCameraLocationGlobal:
                case LCC3Semantic.SemanticCameraFrustum:
                case LCC3Semantic.SemanticViewport:

                case LCC3Semantic.SemanticIsUsingLighting:
                case LCC3Semantic.SemanticSceneLightColorAmbient:

                case LCC3Semantic.SemanticLightIsEnabled:
                case LCC3Semantic.SemanticLightPositionGlobal:
                case LCC3Semantic.SemanticLightPositionEyeSpace:
                case LCC3Semantic.SemanticLightInvertedPositionGlobal:
                case LCC3Semantic.SemanticLightInvertedPositionEyeSpace:
                case LCC3Semantic.SemanticLightColorAmbient:
                case LCC3Semantic.SemanticLightColorDiffuse:
                case LCC3Semantic.SemanticLightColorSpecular:
                case LCC3Semantic.SemanticLightAttenuation:
                case LCC3Semantic.SemanticLightSpotDirectionGlobal:
                case LCC3Semantic.SemanticLightSpotDirectionEyeSpace:
                case LCC3Semantic.SemanticLightSpotExponent:
                case LCC3Semantic.SemanticLightSpotCutoffAngle:
                case LCC3Semantic.SemanticLightSpotCutoffAngleCosine:

                case LCC3Semantic.SemanticFogIsEnabled:
                case LCC3Semantic.SemanticFogColor:
                case LCC3Semantic.SemanticFogAttenuationMode:
                case LCC3Semantic.SemanticFogDensity:
                case LCC3Semantic.SemanticFogStartDistance:
                case LCC3Semantic.SemanticFogEndDistance:

                case LCC3Semantic.SemanticFrameTime:
                case LCC3Semantic.SemanticApplicationTime:
                case LCC3Semantic.SemanticApplicationTimeSine:
                case LCC3Semantic.SemanticApplicationTimeCosine:
                case LCC3Semantic.SemanticApplicationTimeTangent:
                   
                    return LCC3ShaderVariableScope.ScopeScene;

                default:
                    return LCC3ShaderVariableScope.ScopeNode;
            }

            return LCC3ShaderVariableScope.ScopeNode;
        }

        #endregion Querying semantic attributes


        #region Semantic delegate methods

        public bool ConfigureVariable(LCC3ShaderVariable variable)
        {
            return false;
        }

        public bool PopulateUniformWithVisitor(LCC3ShaderUniform uniform, LCC3NodeDrawingVisitor visitor)
        {
            LCC3Semantic semantic = uniform.Semantic;
            LCC3Material material;

            switch (semantic)
            {

            #region Setting material semantics

                case LCC3Semantic.SemanticColor:
                    uniform.SetValue(visitor.CurrentColor.ToVector4());
                    return true;
                case LCC3Semantic.SemanticMaterialColorAmbient:
                    uniform.SetValue(visitor.CurrentMaterial.EffectiveAmbientColor.ToVector4());
                    return true;
                case LCC3Semantic.SemanticMaterialColorDiffuse:
                    uniform.SetValue(visitor.CurrentMaterial.EffectiveDiffuseColor.ToVector4());
                    return true;
                case LCC3Semantic.SemanticMaterialColorSpecular:
                    uniform.SetValue(visitor.CurrentMaterial.EffectiveSpecularColor.ToVector4());
                    return true;
                case LCC3Semantic.SemanticMaterialColorEmission:
                    uniform.SetValue(visitor.CurrentMaterial.EffectiveEmissionColor.ToVector4());
                    return true;
                case LCC3Semantic.SemanticMaterialOpacity:
                    uniform.SetValue(visitor.CurrentMaterial.EffectiveDiffuseColor.A);
                    return true;
                case LCC3Semantic.SemanticMaterialShininess:
                    uniform.SetValue(visitor.CurrentMaterial.Shininess);
                    return true;
                case LCC3Semantic.SemanticMaterialReflectivity:
                    uniform.SetValue(visitor.CurrentMaterial.Reflectivity);
                    return true;
                case LCC3Semantic.SemanticMinimumDrawnAlpha:
                    material = visitor.CurrentMaterial;
                    uniform.SetValue(material.ShouldDrawLowAlpha ? 0.0f : material.AlphaTestReference);
                    return true;

            #endregion Setting material semantics
            
            }

            return false;
        }

        #endregion Semantic delegate methods
    }
}

