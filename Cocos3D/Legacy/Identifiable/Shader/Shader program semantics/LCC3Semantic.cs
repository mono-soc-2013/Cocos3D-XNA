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
    public enum LCC3Semantic
    {
        SemanticNone = 0,                       

        #region Vertex Content

        SemanticVertexLocation,                 
        SemanticVertexNormal,                  
        SemanticVertexTangent,                 
        SemanticVertexBitangent,                
        SemanticVertexColor,                    
        SemanticVertexWeights,                  
        SemanticVertexMatrixIndices,            
        SemanticVertexPointSize,                
        SemanticVertexTexture,

        SemanticHasVertexNormal,                
        SemanticShouldNormalizeVertexNormal,    
        SemanticShouldRescaleVertexNormal,      
        SemanticHasVertexTangent,               
        SemanticHasVertexBitangent,             
        SemanticHasVertexColor,                 
        SemanticHasVertexWeight,                
        SemanticHasVertexMatrixIndex,          
        SemanticHasVertexTextureCoordinate,     
        SemanticHasVertexPointSize,            
        SemanticIsDrawingPoints,

        #endregion Vertex Content


        #region Environment Matrices 

        SemanticModelLocalMatrix,               
        SemanticModelLocalMatrixInv,            
        SemanticModelLocalMatrixInvTran,       

        SemanticModelMatrix,                    
        SemanticModelMatrixInv,                 
        SemanticModelMatrixInvTran,             

        SemanticViewMatrix,                     
        SemanticViewMatrixInv,                  
        SemanticViewMatrixInvTran,             

        SemanticModelViewMatrix,                
        SemanticModelViewMatrixInv,             
        SemanticModelViewMatrixInvTran,         

        SemanticProjMatrix,                     
        SemanticProjMatrixInv,                  
        SemanticProjMatrixInvTran,              

        SemanticViewProjMatrix,                 
        SemanticViewProjMatrixInv,              
        SemanticViewProjMatrixInvTran,         

        SemanticModelViewProjMatrix,           
        SemanticModelViewProjMatrixInv,         
        SemanticModelViewProjMatrixInvTran,

        #endregion Environment Matrices 


        #region Other environment vars

        SemanticEyePosition,

        #endregion Other environment vars


        #region Skinning Matrices

        SemanticBonesPerVertex,                 
        SemanticBoneMatrixCount,                
        SemanticBoneMatricesGlobal,             
        SemanticBoneMatricesInvTranGlobal,      
        SemanticBoneMatricesEyeSpace,           
        SemanticBoneMatricesInvTranEyeSpace,    
        SemanticBoneMatricesModelSpace,       
        SemanticBoneMatricesInvTranModelSpace, 

        #endregion Skinning Matrices


        #region Camera

        SemanticCameraLocationGlobal,           
        SemanticCameraLocationModelSpace,       
        SemanticCameraFrustum,                  
        SemanticViewport,  

        #endregion Camera


        #region Material

        SemanticColor,                          
        SemanticMaterialColorAmbient,           
        SemanticMaterialColorDiffuse,           
        SemanticMaterialColorSpecular,          
        SemanticMaterialColorEmission,         
        SemanticMaterialOpacity,                
        SemanticMaterialShininess,              
        SemanticMaterialReflectivity,           
        SemanticMinimumDrawnAlpha, 

        #endregion Material


        #region Lighting

        SemanticIsUsingLighting,                
        SemanticSceneLightColorAmbient,         

        SemanticLightIsEnabled,
        SemanticLightDirection,
        SemanticLightPositionGlobal,           
        SemanticLightPositionEyeSpace,          
        SemanticLightPositionModelSpace,        
        SemanticLightInvertedPositionGlobal,    
        SemanticLightInvertedPositionEyeSpace,  
        SemanticLightInvertedPositionModelSpace,
        SemanticLightColorAmbient,              
        SemanticLightColorDiffuse,              
        SemanticLightColorSpecular,             
        SemanticLightAttenuation,               
        SemanticLightSpotDirectionGlobal,       
        SemanticLightSpotDirectionEyeSpace,     
        SemanticLightSpotDirectionModelSpace,   
        SemanticLightSpotExponent,              
        SemanticLightSpotCutoffAngle,           
        SemanticLightSpotCutoffAngleCosine,     

        SemanticFogIsEnabled,                   
        SemanticFogColor,                      
        SemanticFogAttenuationMode,             
        SemanticFogDensity,                     
        SemanticFogStartDistance,              
        SemanticFogEndDistance,                                               

        #endregion Lighting


        #region Textures

        SemanticTextureCount, 
        SemanticTexUnitMode,
        SemanticTexUnitConstantColor,
        SemanticTextureSampler,                 
        SemanticTexture2DCount,                 
        SemanticTexture2DSampler,       
        SemanticTextureCubeCount,             
        SemanticTextureCubeSampler,             

        #endregion Textures


        #region Model

        SemanticCenterOfGeometry,               
        SemanticBoundingBoxMin,                 
        SemanticBoundingBoxMax,                 
        SemanticBoundingBoxSize,                
        SemanticBoundingRadius,                 
        SemanticAnimationFraction,

        #endregion Model


        #region Particles

        SemanticPointSize,                      
        SemanticPointSizeAttenuation,           
        SemanticPointSizeMinimum,               
        SemanticPointSizeMaximum,             
        SemanticPointSpritesIsEnabled,   

        #endregion Particles


        #region Time

        SemanticFrameTime,                     
        SemanticApplicationTime,              
        SemanticApplicationTimeSine,        
        SemanticApplicationTimeCosine,
        SemanticApplicationTimeTangent,

        #endregion Time


        #region Misc environment

        SemanticDrawCountCurrentFrame,        
        SemanticRandomNumber,               
        SemanticAppBase,                        
        SemanticMax

        #endregion Misc environment
    }
}

