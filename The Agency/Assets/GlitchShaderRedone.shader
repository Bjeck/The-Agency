Shader "Hidden/GlitchShaderRedone"
{
Properties
{
	_MainTex ("Texture", 2D) = "white" {}
	_DisplacementTex ("Displacement", 2D) = "bump" {}
	_Intensity ("Intensity", Range(0.1,1.0)) = 1
	_ScreenHeight ("ScreenHeightLow", Range(0,2000)) = 0
	_ScreenWidth ("ScreenHeightHigh", Range(0,2000)) = 0
	_ScreenWidth ("ScreenWidthLeft", Range(0,2000)) = 0
	_ScreenWidth ("ScreenWidthRight", Range(0,2000)) = 0
}

SubShader
{
	// No culling or depth
	Cull Off ZWrite Off ZTest Always
	Fog { Mode off }

	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest
		
		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform sampler2D _DisplacementTex;
		float _Intensity;
		float _ScreenHeightLow;
		float _ScreenHeightHigh;
		float _ScreenWidthLeft;
		float _ScreenWidthRight;
		
		float filterRadius;
		float flip_up, flip_down;
		float displace;
		float scale;

		uniform int _PoisitionsLength;
		uniform float3 _Positions [100];
		uniform float2 _Properties [100];

	//	struct appdata
	//	{
	//		float4 vertex : POSITION;
	//		float2 uv : TEXCOORD0;
	//	};

	//	struct v2f
	//	{
	//		float2 uv : TEXCOORD0;
	//		float4 vertex : SV_POSITION;
	//	};
	
		
		struct v2f
		{
			float4 pos : POSITION;
			float2 uv : TEXCOORD0;
			float4 screenPos : TEXCOORD1;
		};
		

		v2f vert (appdata_img v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.texcoord.xy;
			o.screenPos = ComputeScreenPos(o.pos);
			//o.screenPos = UNITY_MATRIX_MVP[3];
			
			return o;
		}

		half4 frag (v2f i) : COLOR
		{
			
			
			
				
			//i.uv.xy += (normal.xy - 0.5) * displace * _Intensity;
			
			half4 color = tex2D(_MainTex, i.uv.xy);
			half4 redcolor = tex2D(_MainTex, i.uv.xy + 0.01 * filterRadius * _Intensity);
			half4 greencolor = tex2D(_MainTex, i.uv.xy + 0.01 * filterRadius * _Intensity);

			i.screenPos.xy /= i.screenPos.w; // CONVERT TO SCREEN COORDINATES. NOW I NEED TO CONVERT TO PIXELS
			
			
			
			//float2 worldpos = 0.5*(i.screenPos.xy+1.0) * _ScreenParams.xy;
			float2 worldpos = (i.screenPos) * _ScreenParams.xy;
			
			
			if(worldpos.y > _ScreenHeightLow && worldpos.y < _ScreenHeightHigh && worldpos.x < _ScreenWidthRight && worldpos.x > _ScreenWidthLeft){
				half4 normal = tex2D (_DisplacementTex, i.uv.xy * scale);
				
				if(i.uv.y < flip_up)
					i.uv.y = 1 - (i.uv.y + flip_up);
				
				if(i.uv.y > flip_down)
					i.uv.y = 1 - (i.uv.y - flip_down);
					
				i.uv.xy += (normal.xy - 0.5) * displace * _Intensity;
				
				
			
				if(_Intensity > 1){		
					half4 redcolor = tex2D(_MainTex,  i.uv.xy + 0.01 * filterRadius * _Intensity);	
					half4 greencolor = tex2D(_MainTex,  i.uv.xy + 0.01 * filterRadius * _Intensity);
				
										
					if(filterRadius > 0){
						color.r = (redcolor.r * 1.2);
						color.b = greencolor.b * 1.2;
					}else{
						color.g = redcolor.b * 1.2;
						color.r = greencolor.g * 1.2;
					}
				}
				else{
					color = tex2D(_MainTex, i.uv.xy);
				}
				

			}
			else{
				color = tex2D(_MainTex, i.uv.xy);
			}
			
		
			//if(filterRadius > 0){
			//	color = colorTest;
			//	color = colorTest2;
			//}
			//else{
				
			
			//}
		
			return color;
		}
		ENDCG
	}
}
}
