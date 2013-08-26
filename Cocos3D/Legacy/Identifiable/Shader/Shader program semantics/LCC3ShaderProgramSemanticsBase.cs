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
using Cocos2D;

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

        }

        #endregion Querying semantic attributes


        #region Semantic delegate methods

        public virtual bool ConfigureVariable(LCC3ShaderVariable variable)
        {
            return false;
        }

        public bool PopulateUniformWithVisitor(LCC3ShaderUniform uniform, LCC3NodeDrawingVisitor visitor)
        {
            LCC3Semantic semantic = uniform.Semantic;
            LCC3Material material;
            bool isInverted;

            switch (semantic)
            {
                #region Attribute Qualifiers

                case LCC3Semantic.SemanticVertexNormal:
                    uniform.SetValue(visitor.CurrentMesh.HasVertexNormals);
                    return true;
                case LCC3Semantic.SemanticShouldNormalizeVertexNormal:
                    uniform.SetValue(visitor.CurrentMeshNode.EffectiveNormalScalingMethod == LCC3NormalScaling.CC3NormalScalingNormalize);
                    return true;
                case LCC3Semantic.SemanticShouldRescaleVertexNormal:
                    uniform.SetValue(visitor.CurrentMeshNode.EffectiveNormalScalingMethod == LCC3NormalScaling.CC3NormalScalingRescale);
                    return true;
                case LCC3Semantic.SemanticHasVertexTangent:
                    uniform.SetValue(visitor.CurrentMesh.HasVertexTangents);
                    return true;
                case LCC3Semantic.SemanticHasVertexBitangent:
                    uniform.SetValue(visitor.CurrentMesh.HasVertexBitangents);
                    return true;
                case LCC3Semantic.SemanticHasVertexColor:
                    uniform.SetValue(visitor.CurrentMesh.HasVertexColors);
                    return true;
                case LCC3Semantic.SemanticHasVertexWeight:
                    uniform.SetValue(visitor.CurrentMesh.HasVertexWeights);
                    return true;
                case LCC3Semantic.SemanticHasVertexMatrixIndex:
                    uniform.SetValue(visitor.CurrentMesh.HasVertexMatrixIndices);
                    return true;
                case LCC3Semantic.SemanticHasVertexTextureCoordinate:
                    uniform.SetValue(visitor.CurrentMesh.HasVertexTextureCoordinates);
                    return true;
                case LCC3Semantic.SemanticHasVertexPointSize:
                    uniform.SetValue(visitor.CurrentMesh.HasVertexPointSizes);
                    return true;
                case LCC3Semantic.SemanticIsDrawingPoints:
                    uniform.SetValue(false); // XNA doesn't support drawing mode by points
                    return true;

                #endregion Attribute Qualifiers
                

                #region Environment semantics
                case LCC3Semantic.SemanticEyePosition:
                    uniform.SetValue(visitor.ViewMatrix.Inverse().TranslationOfTransformMatrix());
                    return true;
                case LCC3Semantic.SemanticModelMatrix:
                    uniform.SetValue(visitor.ModelMatrix);
                    return true;
                case LCC3Semantic.SemanticModelViewMatrix:
                    uniform.SetValue(visitor.ModelViewMatrix);
                    return true;
                case LCC3Semantic.SemanticModelViewProjMatrix:
                    uniform.SetValue(visitor.ModelViewProjMatrix);
                    return true;
                case LCC3Semantic.SemanticProjMatrix:
                    uniform.SetValue(visitor.ProjMatrix);
                    return true;
                case LCC3Semantic.SemanticModelViewMatrixInvTran:
                    uniform.SetValue(visitor.ModelViewMatrix.InverseAdjointTranspose());
                    return true;
                case LCC3Semantic.SemanticModelMatrixInvTran:
                    uniform.SetValue(visitor.ModelMatrix.Transpose().Inverse());
                    return true;


                #endregion Environment semantics


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
                    uniform.SetValue(visitor.CurrentMaterial.EffectiveSpecularColor.ToVector4().TruncateToCC3Vector());
                    return true;
                case LCC3Semantic.SemanticMaterialColorEmission:
                    uniform.SetValue(visitor.CurrentMaterial.EffectiveEmissionColor.ToVector4().TruncateToCC3Vector());
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


                #region Light semantics

                case LCC3Semantic.SemanticIsUsingLighting:
                    uniform.SetValue(visitor.CurrentNode.ShouldUseLighting);
                    return true;
                case LCC3Semantic.SemanticSceneLightColorAmbient:
                    uniform.SetValue(visitor.Scene.AmbientLight.ToVector4());
                    return true;
                case LCC3Semantic.SemanticLightIsEnabled:
                    for (uint i = 0; i < uniform.Size; i++) 
                    {
                        LCC3Light light = visitor.LightAtIndex(uniform.SemanticIndex + i);
                        uniform.SetValueAtIndex(light.Visible, i);
                    }
                    return true;
                case LCC3Semantic.SemanticLightInvertedPositionEyeSpace:
                    isInverted = true;
                    goto case LCC3Semantic.SemanticLightPositionEyeSpace;
                case LCC3Semantic.SemanticLightPositionEyeSpace:
                    for (uint i = 0; i < uniform.Size; i++) 
                    {
                        LCC3Light light = visitor.LightAtIndex(uniform.SemanticIndex + i);
                        LCC3Vector4 ltPos = light.GlobalHomogeneousPosition;
                        if (isInverted == true) 
                        {
                            ltPos = ltPos.HomogeneousNegate();
                        }

                        // Transform global position/direction to eye space and normalize if direction
                        ltPos = visitor.ViewMatrix.TransformCC3Vector4(ltPos);
                        if (light.IsDirectionalOnly == true)
                        {
                            ltPos = ltPos.NormalizedVector();
                        }

                        uniform.SetValueAtIndex(ltPos, i);
                    }
                    return true;
                case LCC3Semantic.SemanticLightDirection:
                    for (uint i = 0; i < uniform.Size; i++) 
                    {
                        LCC3Light light = visitor.LightAtIndex(uniform.SemanticIndex + i);
                        uniform.SetValueAtIndex(light.Location, i);
                    }
                    return true;

                case LCC3Semantic.SemanticLightColorDiffuse:
                    for (uint i = 0; i < uniform.Size; i++) 
                    {
                        LCC3Light light = visitor.LightAtIndex(uniform.SemanticIndex + i);
                        CCColor4F ltColor = light.Visible ? light.DiffuseColor : LCC3ColorUtil.CCC4FBlackTransparent;
                        uniform.SetValueAtIndex(ltColor.ToVector4().TruncateToCC3Vector(), i);
                    }
                    return true;
                case LCC3Semantic.SemanticLightColorAmbient:
                    for (uint i = 0; i < uniform.Size; i++) 
                    {
                        LCC3Light light = visitor.LightAtIndex(uniform.SemanticIndex + i);
                        CCColor4F ltColor = light.Visible ? light.AmbientColor : LCC3ColorUtil.CCC4FBlackTransparent;
                        uniform.SetValueAtIndex(ltColor.ToVector4(), i);
                    }
                    return true;
                case LCC3Semantic.SemanticLightColorSpecular:
                    for (uint i = 0; i < uniform.Size; i++) 
                    {
                        LCC3Light light = visitor.LightAtIndex(uniform.SemanticIndex + i);
                        CCColor4F ltColor = light.Visible ? light.SpecularColor : LCC3ColorUtil.CCC4FBlackTransparent;
                        uniform.SetValueAtIndex(ltColor.ToVector4().TruncateToCC3Vector(), i);
                    }
                    return true;

                #endregion Light semantics
            
            }

            return false;
        }

        #endregion Semantic delegate methods
    }
}

