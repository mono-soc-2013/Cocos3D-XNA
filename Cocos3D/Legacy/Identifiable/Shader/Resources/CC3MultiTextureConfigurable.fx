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

#include "Macros.fxh"

#define SetCommonVSOutputParams \
    vout.PositionPS = cout.Pos_ps; \
    vout.Diffuse = cout.Diffuse; \
    vout.Specular = float4(cout.Specular, cout.FogFactor);


#define SetCommonVSOutputParamsNoFog \
    vout.PositionPS = cout.Pos_ps; \

// Uniforms

uniform float4x4 u_cc3MatrixModel; 
uniform float3  u_cc3EyePosition;
uniform float3x3 u_cc3WorldInverseTranspose;
uniform float4x4 u_cc3WorldViewProj;

uniform float4 u_cc3MaterialDiffuseColor;	
uniform float3 u_cc3MaterialSpecularColor;
uniform float3 u_cc3EmissiveColor;
uniform float  u_cc3MaterialShininess;

uniform float3 u_cc3DirLightDirection;
uniform float3 u_cc3DirLightDiffuseColor;
uniform float3 u_cc3DirLightSpecularColor;

uniform bool u_cc3VertexHasTexCoord;

#define MAX_TEXTURES 2
#define TU_MODE_REPLACE 0
#define TU_MODE_COMBINE 1
#define TU_MODE_ADD 2
#define TU_MODE_DECAL 3
#define TU_MODE_BLEND 4

uniform int u_cc3TextureCount;
uniform int u_cc3TextureUnitMode0;
uniform int u_cc3TextureUnitMode1;
uniform float4 u_cc3TextureUnitColor0;
uniform float4 u_cc3TextureUnitColor1;

DECLARE_TEXTURE(u_cc3Texture0, 0);
DECLARE_TEXTURE(u_cc3Texture1, 1);

// Structures

struct CommonVSOutput
{
    float4 Pos_ps;
    float4 Diffuse;
    float3 Specular;
    float  FogFactor;
};

struct VSInputNmTxVc
{
    float4 Position : SV_Position;
    float3 Normal   : NORMAL;
    float2 TexCoord : TEXCOORD0;
    float4 Color    : COLOR;
};


struct VSOutputTx
{
    float4 PositionPS : SV_Position;
    float4 Diffuse    : COLOR0;
    float4 Specular   : COLOR1;
    float2 TexCoord   : TEXCOORD0;
};

struct ColorPair
{
    float3 Diffuse;
    float3 Specular;
};

// Functions

void AddSpecular(inout float4 color, float3 specular)
{
    color.rgb += specular * color.a;
}

ColorPair ComputeLights(float3 eyeVector, float3 worldNormal, uniform int numLights)
{
    float3x3 lightDirections = 0;
    float3x3 lightDiffuse = 0;
    float3x3 lightSpecular = 0;
    float3x3 halfVectors = 0;
    
    [unroll]
    for (int i = 0; i < numLights; i++)
    {
        lightDirections[i] = float3x3(u_cc3DirLightDirection,     0.0, 0.0, 0.0 ,     0.0, 0.0, 0.0)    [i];
        lightDiffuse[i]    = float3x3(u_cc3DirLightDiffuseColor,  1.0, 1.0, 1.0 ,  1.0, 1.0, 1.0) [i];
        lightSpecular[i]   = float3x3(u_cc3DirLightSpecularColor, 1.0, 1.0, 1.0 , 1.0, 1.0, 1.0)[i];
        
        halfVectors[i] = normalize(eyeVector - lightDirections[i]);
    }

    float3 dotL = mul(-lightDirections, worldNormal);
    float3 dotH = abs(mul(halfVectors, worldNormal));
    
    float3 zeroL = step(0, dotL);

    float3 diffuse  = zeroL * dotL;
    float3 specular = pow(max(dotH, 0) * zeroL, u_cc3MaterialShininess);

    ColorPair result;
    
    result.Diffuse  = mul(diffuse,  lightDiffuse)  * u_cc3MaterialDiffuseColor.rgb + u_cc3EmissiveColor;
    result.Specular = max(mul(specular, lightSpecular),0) * u_cc3MaterialSpecularColor;

    return result;
}


CommonVSOutput ComputeCommonVSOutputWithLighting(float4 position, float3 normal, uniform int numLights)
{
    CommonVSOutput vout;
    
    float4 pos_ws = mul(position, u_cc3MatrixModel);
    float3 eyeVector = normalize(u_cc3EyePosition - pos_ws.xyz);
    float3 worldNormal = normalize(mul(normal, u_cc3WorldInverseTranspose));

    ColorPair lightResult = ComputeLights(eyeVector, worldNormal, numLights);
    
    vout.Pos_ps = mul(position, u_cc3WorldViewProj);
    vout.Diffuse = float4(lightResult.Diffuse, u_cc3MaterialDiffuseColor.a);
    vout.Specular = lightResult.Specular;
    vout.FogFactor = 0.0;
    
    return vout;
}

// Vertex shader

VSOutputTx VSBasicVertexLightingTxVc(VSInputNmTxVc vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
	
	if(u_cc3VertexHasTexCoord == false)
	{
		vout.Diffuse *= vin.Color;
	}
    
    return vout;
}

// Pixel shader

float4 PSBasicVertexLightingTxNoFog(VSOutputTx pin) : SV_Target0
{
	float4 color;
   
	if(u_cc3VertexHasTexCoord == true)
	{
		color = SAMPLE_TEXTURE(u_cc3Texture0, pin.TexCoord) * pin.Diffuse;
	}
    else
	{
		color = pin.Diffuse;
	}
	
	
    AddSpecular(color, pin.Specular.rgb);
    
    return color;
}

// Pixel shader multiple textures

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
		//newColor.a *= texColor.a;
	}
	
	return newColor;
}

float4 ApplyTextures(float4 color, float2 texCoord)
{
	int numOfTex = min(u_cc3TextureCount, MAX_TEXTURES);
	float4 newColor = color;
	
	newColor = ApplyTexture(newColor, SAMPLE_TEXTURE(u_cc3Texture0, texCoord), u_cc3TextureUnitMode0, u_cc3TextureUnitColor0);
	
	if(numOfTex > 1)
	{
		newColor = ApplyTexture(newColor, SAMPLE_TEXTURE(u_cc3Texture1, texCoord), u_cc3TextureUnitMode1, u_cc3TextureUnitColor1);
	}
	
	return newColor;
}

float4 PSBasicVertexLightingMultipleTxNoFog(VSOutputTx pin) : SV_Target0
{
	float4 color = 0;
   
	color = ApplyTextures(color, pin.TexCoord) * pin.Diffuse;
	
	
    AddSpecular(color, pin.Specular.rgb);
    
    return color;
}

TECHNIQUE(BasicEffect_Texture, VSBasicVertexLightingTxVc, PSBasicVertexLightingTxNoFog);
TECHNIQUE(MultiEffect_Texture, VSBasicVertexLightingTxVc, PSBasicVertexLightingMultipleTxNoFog);