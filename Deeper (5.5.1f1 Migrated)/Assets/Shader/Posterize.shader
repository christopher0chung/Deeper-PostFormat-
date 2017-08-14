// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.03921569,fgcg:0.09019608,fgcb:0.07843138,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|emission-5544-OUT,custl-3227-OUT,olwid-5385-OUT,olcol-5118-RGB;n:type:ShaderForge.SFN_Slider,id:6812,x:31255,y:33408,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Gloss,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4682218,max:1;n:type:ShaderForge.SFN_Vector1,id:410,x:31412,y:33478,varname:node_410,prsc:2,v1:10;n:type:ShaderForge.SFN_Multiply,id:5980,x:31451,y:33607,varname:node_5980,prsc:2|A-6812-OUT,B-410-OUT;n:type:ShaderForge.SFN_Vector1,id:2005,x:31451,y:33757,varname:node_2005,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:8057,x:31619,y:33669,varname:node_8057,prsc:2|A-5980-OUT,B-2005-OUT;n:type:ShaderForge.SFN_Exp,id:98,x:31781,y:33669,varname:node_98,prsc:2,et:1|IN-8057-OUT;n:type:ShaderForge.SFN_HalfVector,id:2244,x:31619,y:33512,varname:node_2244,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:6544,x:31619,y:33373,prsc:2,pt:False;n:type:ShaderForge.SFN_LightVector,id:8614,x:31619,y:33252,varname:node_8614,prsc:2;n:type:ShaderForge.SFN_Dot,id:1697,x:31819,y:33279,varname:node_1697,prsc:2,dt:1|A-8614-OUT,B-6544-OUT;n:type:ShaderForge.SFN_Dot,id:1159,x:31819,y:33452,varname:node_1159,prsc:2,dt:1|A-6544-OUT,B-2244-OUT;n:type:ShaderForge.SFN_Power,id:226,x:32021,y:33552,cmnt:Specular Light,varname:node_226,prsc:2|VAL-1159-OUT,EXP-98-OUT;n:type:ShaderForge.SFN_Posterize,id:968,x:32256,y:33503,varname:node_968,prsc:2|IN-226-OUT,STPS-5730-OUT;n:type:ShaderForge.SFN_Posterize,id:4412,x:32256,y:33372,varname:node_4412,prsc:2|IN-1697-OUT,STPS-5730-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5730,x:32021,y:33451,ptovrint:False,ptlb:Bands,ptin:_Bands,varname:_Bands,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Color,id:2937,x:32256,y:33206,ptovrint:False,ptlb:DiffuseColor,ptin:DiffuseColor,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:4140,x:32461,y:33188,cmnt:Diffuse Light,varname:node_4140,prsc:2|A-2937-RGB,B-4412-OUT;n:type:ShaderForge.SFN_AmbientLight,id:91,x:32461,y:33308,varname:node_91,prsc:2;n:type:ShaderForge.SFN_Add,id:9775,x:32760,y:33316,varname:node_9775,prsc:2|A-4140-OUT,B-91-RGB,C-968-OUT;n:type:ShaderForge.SFN_LightColor,id:1090,x:32760,y:33183,varname:node_1090,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:2642,x:32760,y:33054,varname:node_2642,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4696,x:32944,y:33183,varname:node_4696,prsc:2|A-2642-OUT,B-1090-RGB,C-9775-OUT;n:type:ShaderForge.SFN_Color,id:1345,x:32559,y:32640,ptovrint:False,ptlb:EmissionColor,ptin:_EmissionColor,varname:node_1345,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:8894,x:32602,y:32936,ptovrint:False,ptlb:FogColor,ptin:_FogColor,varname:node_8894,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Lerp,id:3030,x:32861,y:32816,varname:node_3030,prsc:2|A-1345-RGB,B-8894-RGB,T-8222-OUT;n:type:ShaderForge.SFN_Lerp,id:3227,x:33018,y:33006,varname:node_3227,prsc:2|A-4696-OUT,B-8894-RGB,T-8222-OUT;n:type:ShaderForge.SFN_Divide,id:8222,x:32401,y:32813,varname:node_8222,prsc:2|A-1410-Z,B-5749-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:1410,x:32073,y:32718,varname:node_1410,prsc:2;n:type:ShaderForge.SFN_Vector1,id:5749,x:32097,y:32947,varname:node_5749,prsc:2,v1:100;n:type:ShaderForge.SFN_Multiply,id:5544,x:33042,y:32794,varname:node_5544,prsc:2|A-6819-OUT,B-3030-OUT;n:type:ShaderForge.SFN_Slider,id:6819,x:32809,y:32625,ptovrint:False,ptlb:Emission Multiplier,ptin:_EmissionMultiplier,varname:node_6819,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:5;n:type:ShaderForge.SFN_Slider,id:5385,x:32682,y:33487,ptovrint:False,ptlb:OutlineWidth,ptin:_OutlineWidth,varname:node_5385,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:5;n:type:ShaderForge.SFN_Color,id:5118,x:33100,y:33340,ptovrint:False,ptlb:OutlineColor,ptin:_OutlineColor,varname:node_5118,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;proporder:6812-5730-2937-1345-8894-6819-5385-5118;pass:END;sub:END;*/

Shader "Shader Forge/Posterize" {
    Properties {
        _Gloss ("Gloss", Range(0, 1)) = 0.4682218
        _Bands ("Bands", Float ) = 2
        DiffuseColor ("DiffuseColor", Color) = (0.5,0.5,0.5,1)
        _EmissionColor ("EmissionColor", Color) = (0.5,0.5,0.5,1)
        _FogColor ("FogColor", Color) = (0,0,0,1)
        _EmissionMultiplier ("Emission Multiplier", Range(0, 5)) = 1
        _OutlineWidth ("OutlineWidth", Range(0, 5)) = 0
        _OutlineColor ("OutlineColor", Color) = (1,1,1,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 100
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            uniform float _OutlineWidth;
            uniform float4 _OutlineColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_OutlineWidth,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                return fixed4(_OutlineColor.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            uniform float _Gloss;
            uniform float _Bands;
            uniform float4 DiffuseColor;
            uniform float4 _EmissionColor;
            uniform float4 _FogColor;
            uniform float _EmissionMultiplier;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float node_8222 = (i.posWorld.b/100.0);
                float3 emissive = (_EmissionMultiplier*lerp(_EmissionColor.rgb,_FogColor.rgb,node_8222));
                float3 finalColor = emissive + lerp((attenuation*_LightColor0.rgb*((DiffuseColor.rgb*floor(max(0,dot(lightDirection,i.normalDir)) * _Bands) / (_Bands - 1))+UNITY_LIGHTMODEL_AMBIENT.rgb+floor(pow(max(0,dot(i.normalDir,halfDirection)),exp2(((_Gloss*10.0)+1.0))) * _Bands) / (_Bands - 1))),_FogColor.rgb,node_8222);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            uniform float _Gloss;
            uniform float _Bands;
            uniform float4 DiffuseColor;
            uniform float4 _EmissionColor;
            uniform float4 _FogColor;
            uniform float _EmissionMultiplier;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_8222 = (i.posWorld.b/100.0);
                float3 finalColor = lerp((attenuation*_LightColor0.rgb*((DiffuseColor.rgb*floor(max(0,dot(lightDirection,i.normalDir)) * _Bands) / (_Bands - 1))+UNITY_LIGHTMODEL_AMBIENT.rgb+floor(pow(max(0,dot(i.normalDir,halfDirection)),exp2(((_Gloss*10.0)+1.0))) * _Bands) / (_Bands - 1))),_FogColor.rgb,node_8222);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
