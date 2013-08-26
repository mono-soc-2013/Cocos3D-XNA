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

DECLARE_TEXTURE(Texture, 0);

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
		color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
	}
    else
	{
		color = pin.Diffuse;
	}
	
	
    AddSpecular(color, pin.Specular.rgb);
    
    return color;
}

TECHNIQUE(BasicEffect_Texture, VSBasicVertexLightingTxVc, PSBasicVertexLightingTxNoFog);