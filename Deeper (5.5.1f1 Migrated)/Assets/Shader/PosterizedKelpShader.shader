// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|emission-5200-OUT,custl-6704-OUT,voffset-7020-OUT;n:type:ShaderForge.SFN_Lerp,id:5200,x:32899,y:32709,varname:node_5200,prsc:2|A-9891-RGB,B-4066-RGB,T-5634-OUT;n:type:ShaderForge.SFN_Color,id:9891,x:32439,y:32540,ptovrint:False,ptlb:EmissionColor,ptin:_EmissionColor,varname:node_1345,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_FragmentPosition,id:8350,x:32113,y:32244,varname:node_8350,prsc:2;n:type:ShaderForge.SFN_Divide,id:5650,x:32332,y:32339,varname:node_5650,prsc:2|A-8350-Z,B-7966-OUT;n:type:ShaderForge.SFN_Color,id:4066,x:32435,y:32727,ptovrint:False,ptlb:FogColor,ptin:_FogColor,varname:node_8894,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Lerp,id:6704,x:32831,y:32897,varname:node_6704,prsc:2|A-7420-OUT,B-4066-RGB,T-5634-OUT;n:type:ShaderForge.SFN_Multiply,id:7420,x:32440,y:32895,varname:node_7420,prsc:2|A-7835-OUT,B-8682-RGB,C-2171-OUT;n:type:ShaderForge.SFN_Add,id:2171,x:32197,y:32822,varname:node_2171,prsc:2|A-3038-OUT,B-3884-RGB,C-5128-OUT;n:type:ShaderForge.SFN_LightColor,id:8682,x:32197,y:32689,varname:node_8682,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:7835,x:32197,y:32560,varname:node_7835,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3038,x:31949,y:32560,cmnt:Diffuse Light,varname:node_3038,prsc:2|A-7504-RGB,B-3905-OUT;n:type:ShaderForge.SFN_AmbientLight,id:3884,x:31949,y:32680,varname:node_3884,prsc:2;n:type:ShaderForge.SFN_Color,id:7504,x:31744,y:32578,ptovrint:False,ptlb:DiffuseColor,ptin:DiffuseColor,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Posterize,id:3905,x:31744,y:32744,varname:node_3905,prsc:2|IN-9147-OUT,STPS-1256-OUT;n:type:ShaderForge.SFN_Posterize,id:5128,x:31744,y:32875,varname:node_5128,prsc:2|IN-927-OUT,STPS-1256-OUT;n:type:ShaderForge.SFN_Power,id:927,x:31509,y:32924,cmnt:Specular Light,varname:node_927,prsc:2|VAL-9723-OUT,EXP-4932-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1256,x:31509,y:32823,ptovrint:False,ptlb:Bands,ptin:_Bands,varname:_Bands,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Dot,id:9147,x:31307,y:32651,varname:node_9147,prsc:2,dt:1|A-6380-OUT,B-826-OUT;n:type:ShaderForge.SFN_Dot,id:9723,x:31307,y:32824,varname:node_9723,prsc:2,dt:1|A-826-OUT,B-2658-OUT;n:type:ShaderForge.SFN_Exp,id:4932,x:31269,y:33041,varname:node_4932,prsc:2,et:1|IN-6626-OUT;n:type:ShaderForge.SFN_Add,id:6626,x:31098,y:33041,varname:node_6626,prsc:2|A-8633-OUT,B-2142-OUT;n:type:ShaderForge.SFN_Multiply,id:8633,x:30930,y:32979,varname:node_8633,prsc:2|A-9484-OUT,B-3810-OUT;n:type:ShaderForge.SFN_Vector1,id:2142,x:30930,y:33129,varname:node_2142,prsc:2,v1:1;n:type:ShaderForge.SFN_HalfVector,id:2658,x:31098,y:32884,varname:node_2658,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:826,x:31098,y:32745,prsc:2,pt:True;n:type:ShaderForge.SFN_LightVector,id:6380,x:31098,y:32624,varname:node_6380,prsc:2;n:type:ShaderForge.SFN_Slider,id:9484,x:30580,y:32953,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Gloss,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4682218,max:1;n:type:ShaderForge.SFN_Vector1,id:3810,x:30737,y:33023,varname:node_3810,prsc:2,v1:10;n:type:ShaderForge.SFN_Append,id:7020,x:33167,y:33241,varname:node_7020,prsc:2|A-6990-OUT,B-4304-OUT,C-9971-OUT;n:type:ShaderForge.SFN_Vector1,id:6990,x:33173,y:33396,varname:node_6990,prsc:2,v1:0;n:type:ShaderForge.SFN_Divide,id:4304,x:32852,y:33072,varname:node_4304,prsc:2|A-8083-OUT,B-2414-Y;n:type:ShaderForge.SFN_Multiply,id:8083,x:32681,y:33071,varname:node_8083,prsc:2|A-3935-OUT,B-8620-OUT;n:type:ShaderForge.SFN_Vector1,id:8620,x:32676,y:33210,cmnt:Vert Amp,varname:node_8620,prsc:2,v1:0.4;n:type:ShaderForge.SFN_ObjectScale,id:2414,x:32777,y:33357,varname:node_2414,prsc:2,rcp:False;n:type:ShaderForge.SFN_Divide,id:9971,x:32853,y:33556,varname:node_9971,prsc:2|A-8123-OUT,B-2414-X;n:type:ShaderForge.SFN_Multiply,id:8123,x:32681,y:33556,varname:node_8123,prsc:2|A-6635-OUT,B-3218-OUT;n:type:ShaderForge.SFN_Vector1,id:3218,x:32680,y:33692,cmnt:Horz Amp,varname:node_3218,prsc:2,v1:0.4;n:type:ShaderForge.SFN_Sin,id:6635,x:32506,y:33556,varname:node_6635,prsc:2|IN-5002-OUT;n:type:ShaderForge.SFN_Add,id:5002,x:32342,y:33556,varname:node_5002,prsc:2|A-5528-OUT,B-2042-OUT;n:type:ShaderForge.SFN_Sin,id:3935,x:32505,y:33072,varname:node_3935,prsc:2|IN-1562-OUT;n:type:ShaderForge.SFN_Add,id:1562,x:32339,y:33072,varname:node_1562,prsc:2|A-6054-OUT,B-8352-OUT;n:type:ShaderForge.SFN_Vector1,id:7988,x:32163,y:33024,cmnt:Vert Pd,varname:node_7988,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:6054,x:32163,y:33089,varname:node_6054,prsc:2|A-7988-OUT,B-4953-OUT;n:type:ShaderForge.SFN_Multiply,id:8352,x:32163,y:33214,varname:node_8352,prsc:2|A-7898-T,B-3230-OUT;n:type:ShaderForge.SFN_Vector1,id:3230,x:32163,y:33353,cmnt:Vert Freq,varname:node_3230,prsc:2,v1:-1;n:type:ShaderForge.SFN_Vector1,id:8000,x:32169,y:33509,cmnt:Horz Pd,varname:node_8000,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:5528,x:32168,y:33571,varname:node_5528,prsc:2|A-8000-OUT,B-5551-X;n:type:ShaderForge.SFN_Multiply,id:2042,x:32167,y:33696,varname:node_2042,prsc:2|A-7898-T,B-2833-OUT;n:type:ShaderForge.SFN_Vector1,id:2833,x:32168,y:33838,cmnt:Horz Freq,varname:node_2833,prsc:2,v1:-3;n:type:ShaderForge.SFN_Add,id:4953,x:31957,y:33098,varname:node_4953,prsc:2|A-5551-Y,B-5551-Z;n:type:ShaderForge.SFN_Time,id:7898,x:31847,y:33353,varname:node_7898,prsc:2;n:type:ShaderForge.SFN_Add,id:7892,x:31727,y:33671,varname:node_7892,prsc:2|A-5551-X,B-5551-Z;n:type:ShaderForge.SFN_FragmentPosition,id:5551,x:31452,y:33363,varname:node_5551,prsc:2;n:type:ShaderForge.SFN_Slider,id:7966,x:31991,y:32425,ptovrint:False,ptlb:FogDistSlider,ptin:_FogDistSlider,varname:node_7966,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:100,max:1000;n:type:ShaderForge.SFN_Clamp01,id:5634,x:32498,y:32339,varname:node_5634,prsc:2|IN-5650-OUT;proporder:9891-4066-7504-1256-9484-7966;pass:END;sub:END;*/

Shader "Shader Forge/PosterizedKelpShader" {
    Properties {
        _EmissionColor ("EmissionColor", Color) = (0.5,0.5,0.5,1)
        _FogColor ("FogColor", Color) = (0,0,0,1)
        DiffuseColor ("DiffuseColor", Color) = (0.5,0.5,0.5,1)
        _Bands ("Bands", Float ) = 2
        _Gloss ("Gloss", Range(0, 1)) = 0.4682218
        _FogDistSlider ("FogDistSlider", Range(0, 1000)) = 100
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
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
            uniform float4 _TimeEditor;
            uniform float4 _EmissionColor;
            uniform float4 _FogColor;
            uniform float4 DiffuseColor;
            uniform float _Bands;
            uniform float _Gloss;
            uniform float _FogDistSlider;
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
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                float4 node_7898 = _Time + _TimeEditor;
                v.vertex.xyz += float3(0.0,((sin(((0.5*(mul(unity_ObjectToWorld, v.vertex).g+mul(unity_ObjectToWorld, v.vertex).b))+(node_7898.g*(-1.0))))*0.4)/objScale.g),((sin(((2.0*mul(unity_ObjectToWorld, v.vertex).r)+(node_7898.g*(-3.0))))*0.4)/objScale.r));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float node_5634 = saturate((i.posWorld.b/_FogDistSlider));
                float3 emissive = lerp(_EmissionColor.rgb,_FogColor.rgb,node_5634);
                float3 finalColor = emissive + lerp((attenuation*_LightColor0.rgb*((DiffuseColor.rgb*floor(max(0,dot(lightDirection,normalDirection)) * _Bands) / (_Bands - 1))+UNITY_LIGHTMODEL_AMBIENT.rgb+floor(pow(max(0,dot(normalDirection,halfDirection)),exp2(((_Gloss*10.0)+1.0))) * _Bands) / (_Bands - 1))),_FogColor.rgb,node_5634);
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
            uniform float4 _TimeEditor;
            uniform float4 _EmissionColor;
            uniform float4 _FogColor;
            uniform float4 DiffuseColor;
            uniform float _Bands;
            uniform float _Gloss;
            uniform float _FogDistSlider;
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
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                float4 node_7898 = _Time + _TimeEditor;
                v.vertex.xyz += float3(0.0,((sin(((0.5*(mul(unity_ObjectToWorld, v.vertex).g+mul(unity_ObjectToWorld, v.vertex).b))+(node_7898.g*(-1.0))))*0.4)/objScale.g),((sin(((2.0*mul(unity_ObjectToWorld, v.vertex).r)+(node_7898.g*(-3.0))))*0.4)/objScale.r));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_5634 = saturate((i.posWorld.b/_FogDistSlider));
                float3 finalColor = lerp((attenuation*_LightColor0.rgb*((DiffuseColor.rgb*floor(max(0,dot(lightDirection,normalDirection)) * _Bands) / (_Bands - 1))+UNITY_LIGHTMODEL_AMBIENT.rgb+floor(pow(max(0,dot(normalDirection,halfDirection)),exp2(((_Gloss*10.0)+1.0))) * _Bands) / (_Bands - 1))),_FogColor.rgb,node_5634);
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
            Cull Back
            
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
            uniform float4 _TimeEditor;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                float4 node_7898 = _Time + _TimeEditor;
                v.vertex.xyz += float3(0.0,((sin(((0.5*(mul(unity_ObjectToWorld, v.vertex).g+mul(unity_ObjectToWorld, v.vertex).b))+(node_7898.g*(-1.0))))*0.4)/objScale.g),((sin(((2.0*mul(unity_ObjectToWorld, v.vertex).r)+(node_7898.g*(-3.0))))*0.4)/objScale.r));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
