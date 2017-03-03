// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32867,y:32807,varname:node_3138,prsc:2|emission-9424-RGB,clip-8115-OUT;n:type:ShaderForge.SFN_Tex2d,id:9798,x:31396,y:32436,ptovrint:False,ptlb:tex1,ptin:_tex1,varname:node_9798,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e0c8b92b12f3c9449afe1f54007f083d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:9424,x:31874,y:33146,varname:node_9424,prsc:2;n:type:ShaderForge.SFN_ToggleProperty,id:209,x:31731,y:32741,ptovrint:False,ptlb:use R,ptin:_useR,varname:node_209,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_ToggleProperty,id:6831,x:31734,y:32811,ptovrint:False,ptlb:use G,ptin:_useG,varname:node_6831,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_ToggleProperty,id:5545,x:31731,y:32899,ptovrint:False,ptlb:use B,ptin:_useB,varname:node_5545,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_ToggleProperty,id:7624,x:31731,y:32978,ptovrint:False,ptlb:use A,ptin:_useA,varname:node_7624,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_Multiply,id:6334,x:31952,y:32635,varname:node_6334,prsc:2|A-9798-R,B-209-OUT;n:type:ShaderForge.SFN_Multiply,id:1424,x:31952,y:32760,varname:node_1424,prsc:2|A-9798-G,B-6831-OUT;n:type:ShaderForge.SFN_Multiply,id:1881,x:31952,y:32876,varname:node_1881,prsc:2|A-9798-B,B-5545-OUT;n:type:ShaderForge.SFN_Multiply,id:9149,x:31952,y:32997,varname:node_9149,prsc:2|A-9798-A,B-7624-OUT;n:type:ShaderForge.SFN_Add,id:6732,x:32158,y:32704,varname:node_6732,prsc:2|A-6334-OUT,B-1424-OUT;n:type:ShaderForge.SFN_Add,id:8488,x:32137,y:32929,varname:node_8488,prsc:2|A-1881-OUT,B-9149-OUT;n:type:ShaderForge.SFN_Add,id:8115,x:32348,y:32803,varname:node_8115,prsc:2|A-6732-OUT,B-8488-OUT;proporder:9798-209-6831-5545-7624;pass:END;sub:END;*/

Shader "Shader Forge/Particles" {
    Properties {
        _tex1 ("tex1", 2D) = "white" {}
        [MaterialToggle] _useR ("use R", Float ) = 1
        [MaterialToggle] _useG ("use G", Float ) = 0
        [MaterialToggle] _useB ("use B", Float ) = 0
        [MaterialToggle] _useA ("use A", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _tex1; uniform float4 _tex1_ST;
            uniform fixed _useR;
            uniform fixed _useG;
            uniform fixed _useB;
            uniform fixed _useA;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _tex1_var = tex2D(_tex1,TRANSFORM_TEX(i.uv0, _tex1));
                clip((((_tex1_var.r*_useR)+(_tex1_var.g*_useG))+((_tex1_var.b*_useB)+(_tex1_var.a*_useA))) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = i.vertexColor.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _tex1; uniform float4 _tex1_ST;
            uniform fixed _useR;
            uniform fixed _useG;
            uniform fixed _useB;
            uniform fixed _useA;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _tex1_var = tex2D(_tex1,TRANSFORM_TEX(i.uv0, _tex1));
                clip((((_tex1_var.r*_useR)+(_tex1_var.g*_useG))+((_tex1_var.b*_useB)+(_tex1_var.a*_useA))) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
