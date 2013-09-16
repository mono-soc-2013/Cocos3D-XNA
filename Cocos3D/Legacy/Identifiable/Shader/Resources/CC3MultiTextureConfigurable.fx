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

#include "CC3ShaderMacros.fxh"
#include "CC3ShaderStructures.fxh"

// Uniforms

// Environment uniforms
uniform float4x4 u_cc3WorldMatrix;
uniform float4x4 u_cc3ViewMatrix;
uniform float4x4 u_cc3ProjMatrix;
uniform float3  u_cc3EyePosition;

// Material uniforms
uniform float4 u_cc3MaterialAmbientColor;
uniform float4 u_cc3MaterialDiffuseColor;	
uniform float4 u_cc3MaterialSpecularColor;
uniform float4 u_cc3MaterialEmissiveColor;
uniform float  u_cc3MaterialShininess;

// Lighting uniforms
uniform float4 u_cc3LightSceneAmbientLightColor;
uniform float3 u_cc3DirLightDirection;
uniform float4 u_cc3DirLightDiffuseColor;
uniform float4 u_cc3DirLightSpecularColor;

// Texture uniforms
uniform int u_cc3TextureCount;
uniform int u_cc3TextureUnitMode0;
uniform int u_cc3TextureUnitMode1;
uniform float4 u_cc3TextureUnitColor0;
uniform float4 u_cc3TextureUnitColor1;

DECLARE_TEXTURE(u_cc3Texture0, 0);
DECLARE_TEXTURE(u_cc3Texture1, 1);


//--------------------------------------------------------------------------------------
// PER PIXEL PHONG
//--------------------------------------------------------------------------------------
float4 CalcPhongLighting(Material material, DirectionalLight light, float3 surfaceNormal, float3 eyeVector, float4 sceneAmbientColor)
{
    float3 L = - light.Direction;
	float3 N = surfaceNormal;
	float3 R = reflect( light.Direction, N);
	float3 V = eyeVector;
	
	float4 Ia = material.AmbientColor * sceneAmbientColor;
    float4 Id = (material.DiffuseColor * saturate( dot(N,L) ) * light.DiffuseColor) + material.EmissiveColor;
    float4 Is = material.SpecularColor * light.SpecularColor * pow(saturate(dot(R,V)), material.Shininess);
 
    return Ia + (Id + Is);
}

//--------------------------------------------------------------------------------------
// MULTI TEXTURING
//--------------------------------------------------------------------------------------
float4 ApplyTexture(float4 color, float4 texColor, int texUnitMode, float4 texUnitColor)
{	
	float4 newColor = color;
	
	if(texUnitMode == TU_MODE_REPLACE)
	{
		newColor = texColor;
	}
	
	else if(texUnitMode == TU_MODE_ADD)
	{
		newColor.rgb += texColor.rgb;
	}
	
	else if(texUnitMode == TU_MODE_DECAL)
	{
		newColor.rgb = (texColor.rgb * texColor.a) + (newColor.rgb * (1.0 - texColor.a));
	}
	
	else if(texUnitMode == TU_MODE_BLEND)
	{
		newColor.rgb =  (newColor.rgb * (1.0 - (texColor.rgb * texUnitColor.a))) + (texUnitColor.rgb * texColor.rgb * texUnitColor.a);
	}
	
	return newColor;
}

float4 ApplyTextures(float4 color, float2 texCoord)
{
	int numOfTex = min(u_cc3TextureCount, MAX_TEXTURES);
	
	float4 newColor = ApplyTexture(color, SAMPLE_TEXTURE(u_cc3Texture0, texCoord), u_cc3TextureUnitMode0, u_cc3TextureUnitColor0);
	
	if(numOfTex > 1)
	{
		newColor = ApplyTexture(newColor, SAMPLE_TEXTURE(u_cc3Texture1, texCoord), u_cc3TextureUnitMode1, u_cc3TextureUnitColor1);
	}
	
	return newColor;
}

//--------------------------------------------------------------------------------------
// PER PIXEL LIGHTING
//--------------------------------------------------------------------------------------

// Vertex shader
PsInputPerPixelPhong VS_PIXEL_LIGHTING_PHONG( VSInputNmTxVc input )
{
    PsInputPerPixelPhong output;
 
    output.WorldPosition = mul( input.Position, u_cc3WorldMatrix );
    output.Position = mul( output.WorldPosition, u_cc3ViewMatrix );
    output.Position = mul( output.Position, u_cc3ProjMatrix );
 
    //set texture coords
    output.TexCoord = input.TexCoord;
 
    //set required lighting vectors for interpolation
    output.Normal = normalize( mul(float4(input.Normal,0), u_cc3WorldMatrix)).xyz;
	
	return output;
}

// Pixel shader - first pass (multi-texturing)
float4 PS_PIXEL_MULTITEX( PsInputPerPixelPhong input ) : SV_Target0
{
	float4 color = 0;
        
    return  ApplyTextures(color, input.TexCoord);
}

// Pixel shader - second pass (lighting)
float4 PS_PIXEL_LIGHTING_PHONG( PsInputPerPixelPhong input ) : SV_Target0
{
    //calculate lighting vectors - renormalize vectors
    input.Normal = normalize( input.Normal );
	
    float3 eyeVector = normalize(u_cc3EyePosition - input.WorldPosition.xyz);

	Material material;
	material.AmbientColor = u_cc3MaterialAmbientColor;
	material.DiffuseColor = u_cc3MaterialDiffuseColor;
	material.EmissiveColor = u_cc3MaterialEmissiveColor;
	material.SpecularColor = u_cc3MaterialSpecularColor;
	material.Shininess = u_cc3MaterialShininess;
 
	DirectionalLight light;
	light.Direction = u_cc3DirLightDirection;
	light.DiffuseColor = u_cc3DirLightDiffuseColor;
	light.SpecularColor = u_cc3DirLightSpecularColor;
 
    //calculate lighting
    return CalcPhongLighting( material, light, input.Normal, eyeVector, u_cc3LightSceneAmbientLightColor);
}

// Two pass technique - second pass color is multiplied by first
TECHNIQUE_2PS(MultiTexPerPixelShader, VS_PIXEL_LIGHTING_PHONG, PS_PIXEL_MULTITEX, PS_PIXEL_LIGHTING_PHONG);