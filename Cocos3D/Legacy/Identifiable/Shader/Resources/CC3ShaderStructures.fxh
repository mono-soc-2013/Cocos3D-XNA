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

struct Material
{
    float4 AmbientColor, DiffuseColor, SpecularColor, EmissiveColor;
	float Shininess;
};

struct DirectionalLight
{
	float4 DiffuseColor;
	float4 SpecularColor;
	float3 Direction;
};

struct VSInputNmTxVc
{
    float4 Position : SV_Position;
    float3 Normal   : NORMAL;
    float2 TexCoord : TEXCOORD0;
    float4 Color    : COLOR;
};

struct PsInputPerPixelPhong
{
	float4 Position : SV_Position;   
	float4 WorldPosition : TEXCOORD2;
	float2 TexCoord : TEXCOORD;	
	float3 Normal : TEXCOORD1;		
};