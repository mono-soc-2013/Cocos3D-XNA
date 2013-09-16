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

// Texture defs
#define MAX_TEXTURES 2
#define TU_MODE_REPLACE 0
#define TU_MODE_COMBINE 1
#define TU_MODE_ADD 2
#define TU_MODE_DECAL 3
#define TU_MODE_BLEND 4

#ifdef SM4

// Macros for targetting shader model 4.0 (DX11)

#define TECHNIQUE(name, vsname, psname ) \
	technique name { pass Pass0 { VertexShader = compile vs_4_0_level_9_1 vsname (); PixelShader = compile ps_4_0_level_9_1 psname(); } }
	
#define TECHNIQUE_2PS(name, vsname, psname1, psname2 ) \
	technique name { pass Pass0 { VertexShader = compile vs_4_0_level_9_1 vsname (); PixelShader = compile ps_4_0_level_9_1 psname1(); } \ 
	pass Pass1 { AlphaBlendEnable = True; SrcBlend = DstColor; DestBlend = Zero; SrcBlendAlpha = Zero; DestBlendAlpha = SrcAlpha; PixelShader = compile ps_4_0_level_9_1 psname2(); } }

#define DECLARE_TEXTURE(Name, index) \
    Texture2D<float4> Name : register(t##index); \
    sampler Name##Sampler : register(s##index)

#define SAMPLE_TEXTURE(Name, texCoord)  Name.Sample(Name##Sampler, texCoord)

#else

// Macros for targetting shader model 2.0 (DX9)

#define TECHNIQUE(name, vsname, psname ) \
	technique name { pass Pass0 { VertexShader = compile vs_2_0 vsname (); PixelShader = compile ps_2_0 psname(); } }

#define TECHNIQUE_2PS(name, vsname, psname1, psname2 ) \
	technique name { pass Pass0 { AlphaBlendEnable = False; SrcBlend = One; DestBlend = Zero; VertexShader = compile vs_2_0 vsname (); PixelShader = compile ps_2_0 psname1(); } \
	pass Pass1 { AlphaBlendEnable = True; SrcBlend = Zero; DestBlend = SrcColor; SrcBlendAlpha = Zero; DestBlendAlpha = SrcAlpha; PixelShader = compile ps_2_0 psname2();} } 
	
#define DECLARE_TEXTURE(Name, index) \
    sampler2D Name : register(s##index);

#define SAMPLE_TEXTURE(Name, texCoord)  tex2D(Name, texCoord)


#endif
