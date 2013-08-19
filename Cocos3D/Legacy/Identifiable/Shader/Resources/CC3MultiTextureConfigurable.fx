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
#include "Structures.fxh"

#define MAX_LIGHTS 3

// Uniforms

uniform float4x4 u_cc3MatrixModelView;
uniform float4x4 u_cc3MatrixProj;
uniform float3x3 u_cc3MatrixModelViewInvTran;


uniform float4 u_cc3MaterialAmbientColor;
uniform float4 u_cc3MaterialDiffuseColor;
uniform float4 u_cc3MaterialSpecularColor;
uniform float4 u_cc3MaterialEmissionColor;

uniform float u_cc3MaterialShininess;

uniform bool u_cc3LightIsUsingLighting;
uniform float4 u_cc3LightSceneAmbientLightColor;

uniform bool u_cc3LightIsLightEnabled[MAX_LIGHTS];
uniform float4 u_cc3LightPositionEyeSpace[MAX_LIGHTS];
uniform float4 u_cc3LightAmbientColor[MAX_LIGHTS];
uniform float4 u_cc3LightDiffuseColor[MAX_LIGHTS];
uniform float4 u_cc3LightSpecularColor[MAX_LIGHTS];


// Constants

static const float3 kHalfPlaneOffset = float3(0.0, 0.0, 1.0);

DECLARE_TEXTURE(Texture, 0);


void VertexToEyeSpace(VSInputNmTx vin)
{
	//VtxPosEye = mul(vin.Position, u_cc3MatrixModelView);
	//VtxNormEye = mul(vin.Normal, u_cc3MatrixModelViewInvTran);
}


float4 IlluminateWith(int ltIdx, float3 VtxNormEye) 
{
	float3 ltDir;
	float intensity = 1.0;


	// Directional light. Vector is expected to be normalized!
	ltDir = float3(u_cc3LightPositionEyeSpace[ltIdx].x, u_cc3LightPositionEyeSpace[ltIdx].y, u_cc3LightPositionEyeSpace[ltIdx].z);
	
	
	// If no light intensity, short-circuit and return no color
	if (intensity <= 0.0) return float4(0.0, 0.0, 0.0, 0.0);

	// Employ lighting equation to calculate vertex color
	float4 vtxColor = (u_cc3LightAmbientColor[ltIdx] * u_cc3MaterialAmbientColor);
	vtxColor += mul(u_cc3LightDiffuseColor[ltIdx] * u_cc3MaterialDiffuseColor, max(0.0, dot(VtxNormEye, ltDir)));

	
	// Project normal onto half-plane vector to determine specular component
	float specProj =  dot(VtxNormEye, normalize(ltDir + kHalfPlaneOffset));
	
	
	if (specProj > 0.0) 
	{
		float specPow = specProj * u_cc3MaterialShininess;
		
		vtxColor +=
			u_cc3MaterialSpecularColor *
			u_cc3LightSpecularColor[ltIdx];
		
		vtxColor *= specPow;
	}
	
	// Return the attenuated vertex color
	return vtxColor;
}


float4 Illuminate(float3 VtxNormEye)
{
	float4 vtxColor = u_cc3MaterialEmissionColor + (u_cc3MaterialAmbientColor * u_cc3LightSceneAmbientLightColor);
		
	for (int ltIdx = 0; ltIdx < MAX_LIGHTS; ltIdx++)
	{
		if (u_cc3LightIsLightEnabled[ltIdx] == true)
		{
			vtxColor += IlluminateWith(ltIdx, VtxNormEye);
		}
	}
	

	vtxColor.a = u_cc3MaterialDiffuseColor.a;


	return vtxColor;
}


// Vertex shader

VSOutputTx VSBasicTx(VSInputNmTx vin)
{
	float4 VtxPosEye;
	float3 VtxNormEye;
    
	VtxPosEye = mul(vin.Position, u_cc3MatrixModelView);
	VtxNormEye = mul(vin.Normal, u_cc3MatrixModelViewInvTran);

    VSOutputTx vout;
    
    vout.PositionPS = mul(VtxPosEye, u_cc3MatrixProj);
	vout.Diffuse = Illuminate(VtxNormEye);
	vout.Specular = float4(0.0, 0.0, 0.0, 0.0);
    vout.TexCoord = vin.TexCoord;

    return vout;
}

// Fragment shader

float4 PSBasicTx(VSOutputTx pin) : SV_Target0
{
    return SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
}

TECHNIQUE(BasicEffect_Texture, VSBasicTx, PSBasicTx);

