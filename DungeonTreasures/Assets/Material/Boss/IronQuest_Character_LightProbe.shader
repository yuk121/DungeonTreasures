// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:33363,y:33044,varname:node_2865,prsc:2|normal-5321-RGB,emission-2502-OUT,custl-7931-OUT;n:type:ShaderForge.SFN_LightVector,id:9332,x:31099,y:33109,varname:node_9332,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4815,x:31103,y:33381,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:4323,x:31469,y:33230,varname:node_4323,prsc:2,dt:1|A-9332-OUT,B-4815-OUT;n:type:ShaderForge.SFN_Multiply,id:6316,x:31888,y:33335,varname:node_6316,prsc:2|A-1952-OUT,B-7961-OUT;n:type:ShaderForge.SFN_Add,id:3903,x:32075,y:33335,cmnt:HalfLambert,varname:node_3903,prsc:2|A-6316-OUT,B-7961-OUT;n:type:ShaderForge.SFN_Dot,id:2606,x:31888,y:33666,varname:node_2606,prsc:2,dt:1|A-4815-OUT,B-5659-OUT;n:type:ShaderForge.SFN_HalfVector,id:5659,x:31673,y:33727,varname:node_5659,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:809,x:32795,y:33091,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:node_809,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5321,x:33389,y:32851,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_5321,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_LightColor,id:413,x:31469,y:33451,varname:node_413,prsc:2;n:type:ShaderForge.SFN_Power,id:3919,x:32076,y:33736,varname:node_3919,prsc:2|VAL-2606-OUT,EXP-7095-OUT;n:type:ShaderForge.SFN_Add,id:7931,x:33117,y:33530,varname:node_7931,prsc:2|A-8114-OUT,B-1364-OUT;n:type:ShaderForge.SFN_Multiply,id:8114,x:32792,y:33291,cmnt:Base,varname:node_8114,prsc:2|A-809-RGB,B-6529-OUT,C-3903-OUT;n:type:ShaderForge.SFN_Slider,id:971,x:31194,y:33963,ptovrint:False,ptlb:Glossiness,ptin:_Glossiness,varname:node_971,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:7,max:32;n:type:ShaderForge.SFN_Multiply,id:1364,x:32792,y:33718,cmnt:specular,varname:node_1364,prsc:2|A-413-RGB,B-3919-OUT,C-2298-OUT;n:type:ShaderForge.SFN_Slider,id:8541,x:31516,y:34038,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:_Glossiness_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2,max:4;n:type:ShaderForge.SFN_Exp,id:7095,x:31888,y:33821,varname:node_7095,prsc:2,et:1|IN-799-OUT;n:type:ShaderForge.SFN_Tex2d,id:7549,x:30847,y:33680,ptovrint:False,ptlb:Specular Map,ptin:_SpecularMap,varname:node_7549,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2298,x:31888,y:34077,varname:node_2298,prsc:2|A-8541-OUT,B-7549-RGB;n:type:ShaderForge.SFN_Multiply,id:799,x:31673,y:33876,varname:node_799,prsc:2|A-7549-A,B-971-OUT;n:type:ShaderForge.SFN_Fresnel,id:5363,x:31847,y:32648,varname:node_5363,prsc:2|EXP-8220-OUT;n:type:ShaderForge.SFN_Slider,id:8220,x:31459,y:32616,ptovrint:False,ptlb:Rim Power,ptin:_RimPower,varname:node_5255,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:2,max:6;n:type:ShaderForge.SFN_Blend,id:5665,x:31847,y:32812,cmnt:Fresnel  Specular ,varname:node_5665,prsc:2,blmd:14,clmp:False|SRC-8175-RGB,DST-3666-OUT;n:type:ShaderForge.SFN_Color,id:8175,x:31603,y:32732,ptovrint:False,ptlb:RimStrength,ptin:_RimStrength,varname:_Black_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8235295,c2:0.8235295,c3:0.8235295,c4:1;n:type:ShaderForge.SFN_Desaturate,id:3666,x:31616,y:32898,varname:node_3666,prsc:2|COL-7549-RGB;n:type:ShaderForge.SFN_Multiply,id:6623,x:32081,y:32747,varname:node_6623,prsc:2|A-5363-OUT,B-5665-OUT;n:type:ShaderForge.SFN_Multiply,id:2502,x:32795,y:32930,cmnt:Fresnel Final,varname:node_2502,prsc:2|A-6203-OUT,B-413-RGB;n:type:ShaderForge.SFN_Multiply,id:6203,x:32306,y:32834,cmnt:Fresnel With LightProbeColor,varname:node_6203,prsc:2|A-6623-OUT,B-6529-OUT;n:type:ShaderForge.SFN_Code,id:9962,x:31749,y:33089,varname:node_9962,prsc:2,code:cgBlAHQAdQByAG4AIABTAGgAYQBkAGUAUwBIADkAKABmAGwAbwBhAHQANAAoAE4ALAAxACkAKQA7AA==,output:2,fname:LightProbeCode,width:247,height:114,input:2,input_1_label:N|A-4815-OUT;n:type:ShaderForge.SFN_Slider,id:3234,x:31749,y:32989,ptovrint:False,ptlb:LightProbePower,ptin:_LightProbePower,varname:node_3234,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2,max:8;n:type:ShaderForge.SFN_Multiply,id:6529,x:32081,y:33007,cmnt:LightProbe,varname:node_6529,prsc:2|A-3234-OUT,B-9962-OUT;n:type:ShaderForge.SFN_Multiply,id:1952,x:31693,y:33335,varname:node_1952,prsc:2|A-4323-OUT,B-413-RGB;n:type:ShaderForge.SFN_Vector1,id:7961,x:32075,y:33480,varname:node_7961,prsc:2,v1:0.5;proporder:809-5321-7549-8541-971-8220-8175-3234;pass:END;sub:END;*/

Shader "IronQuest/Character_LightProbe" {
    Properties {
        _Albedo ("Albedo", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _SpecularMap ("Specular Map", 2D) = "white" {}
        _Specular ("Specular", Range(0, 4)) = 2
        _Glossiness ("Glossiness", Range(1, 32)) = 7
        _RimPower ("Rim Power", Range(1, 6)) = 2
        _RimStrength ("RimStrength", Color) = (0.8235295,0.8235295,0.8235295,1)
        _LightProbePower ("LightProbePower", Range(0, 8)) = 2
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _Glossiness;
            uniform float _Specular;
            uniform sampler2D _SpecularMap; uniform float4 _SpecularMap_ST;
            uniform float _RimPower;
            uniform float4 _RimStrength;
            float3 LightProbeCode( float3 N ){
            return ShadeSH9(float4(N,1));
            }
            
            uniform float _LightProbePower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
////// Emissive:
                float4 _SpecularMap_var = tex2D(_SpecularMap,TRANSFORM_TEX(i.uv0, _SpecularMap));
                float3 node_6529 = (_LightProbePower*LightProbeCode( normalDirection )); // LightProbe
                float3 emissive = (((pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPower)*( _RimStrength.rgb > 0.5 ? (dot(_SpecularMap_var.rgb,float3(0.3,0.59,0.11)) + 2.0*_RimStrength.rgb -1.0) : (dot(_SpecularMap_var.rgb,float3(0.3,0.59,0.11)) + 2.0*(_RimStrength.rgb-0.5))))*node_6529)*_LightColor0.rgb);
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float node_7961 = 0.5;
                float3 finalColor = emissive + ((_Albedo_var.rgb*node_6529*(((max(0,dot(lightDirection,normalDirection))*_LightColor0.rgb)*node_7961)+node_7961))+(_LightColor0.rgb*pow(max(0,dot(normalDirection,halfDirection)),exp2((_SpecularMap_var.a*_Glossiness)))*(_Specular*_SpecularMap_var.rgb)));
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _Glossiness;
            uniform float _Specular;
            uniform sampler2D _SpecularMap; uniform float4 _SpecularMap_ST;
            uniform float _RimPower;
            uniform float4 _RimStrength;
            float3 LightProbeCode( float3 N ){
            return ShadeSH9(float4(N,1));
            }
            
            uniform float _LightProbePower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 node_6529 = (_LightProbePower*LightProbeCode( normalDirection )); // LightProbe
                float node_7961 = 0.5;
                float4 _SpecularMap_var = tex2D(_SpecularMap,TRANSFORM_TEX(i.uv0, _SpecularMap));
                float3 finalColor = ((_Albedo_var.rgb*node_6529*(((max(0,dot(lightDirection,normalDirection))*_LightColor0.rgb)*node_7961)+node_7961))+(_LightColor0.rgb*pow(max(0,dot(normalDirection,halfDirection)),exp2((_SpecularMap_var.a*_Glossiness)))*(_Specular*_SpecularMap_var.rgb)));
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            uniform sampler2D _SpecularMap; uniform float4 _SpecularMap_ST;
            uniform float _RimPower;
            uniform float4 _RimStrength;
            float3 LightProbeCode( float3 N ){
            return ShadeSH9(float4(N,1));
            }
            
            uniform float _LightProbePower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightColor = _LightColor0.rgb;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 _SpecularMap_var = tex2D(_SpecularMap,TRANSFORM_TEX(i.uv0, _SpecularMap));
                float3 node_6529 = (_LightProbePower*LightProbeCode( normalDirection )); // LightProbe
                o.Emission = (((pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPower)*( _RimStrength.rgb > 0.5 ? (dot(_SpecularMap_var.rgb,float3(0.3,0.59,0.11)) + 2.0*_RimStrength.rgb -1.0) : (dot(_SpecularMap_var.rgb,float3(0.3,0.59,0.11)) + 2.0*(_RimStrength.rgb-0.5))))*node_6529)*_LightColor0.rgb);
                
                float3 diffColor = float3(0,0,0);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
