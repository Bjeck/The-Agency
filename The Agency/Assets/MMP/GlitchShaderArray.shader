Shader "Hidden/GlitchShaderArray"
{
Properties
{
	_MainTex ("Texture", 2D) = "white" {}
	_DisplacementTex ("Displacement", 2D) = "bump" {}
	_Intensity ("Intensity", Range(0.1,1.0)) = 1
}

// This shader is the Glitch Shader, which takes the values from GlitchEffectArray and glitches the screen at the correct positions with given values.
//It is a vertex/fragment screen shader which is an overlay to a camera.

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

		uniform sampler2D _MainTex;			//need variables and textures are set up.
		uniform sampler2D _DisplacementTex;
		float _Intensity;
		
		float filterRadius;
		float flip_up, flip_down;
		float displace;

		uniform int _PositionsLength = 0;
		uniform float3 _Positions [99];		//these are the arrays of the positions. GLSL (mac) has a limit of 99 indeces per array. However, most of the time, not more than 3-6 are needed
		uniform float2 _Scales [99];
		uniform float4 scaleRandomizer;
			
		struct v2f
		{
			float4 pos : POSITION;
			float2 uv : TEXCOORD0;
			float4 screenPos : TEXCOORD1;
		};
		

		v2f vert (appdata_img v)						//The vertex shaders' only job is to calculate the screen position based on the current position of this vertex, as that is necessary for later comparison.
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.texcoord.xy;
			o.screenPos = ComputeScreenPos(o.pos);

			return o;
		}


		half4 frag (v2f i) : COLOR
		{
			
			half4 color = tex2D(_MainTex, i.uv.xy);
			half4 redcolor = tex2D(_MainTex, i.uv.xy + 0.01 * filterRadius * _Intensity);
			half4 greencolor = tex2D(_MainTex, i.uv.xy + 0.01 * filterRadius * _Intensity);		//Colors are set up for the glitch effect.

			i.screenPos.xy /= i.screenPos.w; // The screen position is convered to screen coordinates with this method. 
			float2 worldpos = (i.screenPos) * _ScreenParams.xy;		//Then they are converted to pixels, as this is a fragment shader we need to find out the world position of what this pixel is showing.

			for(int j = 0; j<_PositionsLength;j++){		//As this effect needs to happen for each position in the array, this is done in a for loop.

				if(worldpos.y > _Positions[j].y - (_Scales[j].x+scaleRandomizer.x) && worldpos.y < _Positions[j].y + (_Scales[j].x+scaleRandomizer.y) &&  
				   worldpos.x < _Positions[j].x + (_Scales[j].x+scaleRandomizer.z) && worldpos.x > _Positions[j].x - (_Scales[j].x+scaleRandomizer.w)){ //The primary if statement. Checks if this pixel is within the borders of the current Glitch position, and if so, it begins the effect. Else it just returns the correct color of the pixel

					half4 normal = tex2D (_DisplacementTex, i.uv.xy * _Scales[j].x);		//The first part of the glitch effect is the displacement, and this is calculated through the displacement texture.

					if(i.uv.y < flip_up)			//if the glitch effect determines this pixel should flip up or down (determined in GlitchEffectArray) it will do so here.
						i.uv.y = 1 - (i.uv.y + flip_up);
					
					if(i.uv.y > flip_down)
						i.uv.y = 1 - (i.uv.y - flip_down);
						
					i.uv.xy += (normal.xy - 0.5) * displace * _Intensity;


					//Finally, the color is altered depending on the filterRadius variable, also controlled from GlitchEffectArray
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

			return color;		
					
		}


		ENDCG
	}
}
}



