Shader "Hidden/GlitchShaderArray2"
{
Properties
{
	_MainTex ("Texture", 2D) = "white" {}
	_DisplacementTex ("Displacement", 2D) = "bump" {}
	_Intensity ("Intensity", Range(0.1,1.0)) = 1
	_ScreenPoint ("ScreenPoint", Vector) = (0,0,0,0)
	_PointScale ("PointScale",Range(0,100)) = 0
	_Randomness ("Randomness", Range(0,100)) = 1
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
		float4 _ScreenPoint;
		float _PointScale;
		float _Randomness;
		
		float filterRadius;
		float flip_up, flip_down;
		float displace;
		float scale;

		uniform int _PositionsLength = 0;
		uniform float3 _Positions [99];
		uniform float2 _Scales [99];
		uniform float4 scaleRandomizer;
			
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




		float rand(float3 co){
   		  	return frac(sin( dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
 		}


		half4 frag (v2f i) : COLOR
		{
			//i.uv.xy += (normal.xy - 0.5) * displace * _Intensity;
			
			half4 color = tex2D(_MainTex, i.uv.xy);
			half4 redcolor = tex2D(_MainTex, i.uv.xy + 0.01 * filterRadius * _Intensity);
			half4 greencolor = tex2D(_MainTex, i.uv.xy + 0.01 * filterRadius * _Intensity);

			i.screenPos.xy /= i.screenPos.w; // CONVERT TO SCREEN COORDINATES. NOW I NEED TO CONVERT TO PIXELS
			
			
			
			float2 worldpos = (i.screenPos) * _ScreenParams.xy;

			for(int j = 0; j<_PositionsLength;j++){

//				_Positions[j].y *= (rand(float3(int(i.pos.y-0.001f),int(0),int(i.pos.y+0.001f)))) * _Randomness;
//				_Positions[j].x *= (rand(float3(int(i.pos.x-0.001f),int(0),int(i.pos.x+0.001f)))) * _Randomness;

				if(worldpos.y > _Positions[j].y - (_Scales[j].x+scaleRandomizer.x) && worldpos.y < _Positions[j].y + (_Scales[j].x+scaleRandomizer.y) && 
				   worldpos.x < _Positions[j].x + (_Scales[j].x+scaleRandomizer.z) && worldpos.x > _Positions[j].x - (_Scales[j].x+scaleRandomizer.w)){

			    //if(distance(_Positions[j].xy,worldpos) < _Randomness){
		 	//		Circle area instead -- more uniform.

					half4 normal = tex2D (_DisplacementTex, i.uv.xy * _Scales[j].x);

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
			}

			return color;				
					}




		ENDCG
	}
}
}



