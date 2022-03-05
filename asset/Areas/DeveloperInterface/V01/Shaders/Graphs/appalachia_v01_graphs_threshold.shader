Shader "appalachia/v01/graphs/threshold"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[MaterialToggle] Interpolate("Interpolate", Float) = 0

		_GoodColor("Good Color", Color) = (1,1,1,1)
		_CautionColor("Caution Color", Color) = (1,1,1,1)
		_CriticalColor("Critical Color", Color) = (1,1,1,1)

		_GoodThreshold("Good Threshold", Float) = 0.5
		_CautionThreshold("Caution Threshold", Float) = 0.25
		_ThresholdThickness("Threshold Thickness", Float) = .02
	}

	SubShader
	{			
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			Name "Default"
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
				
				
			#include "graphs.cginc"
			
			fixed4 _GoodColor;
			fixed4 _CautionColor;
			fixed4 _CriticalColor;

			fixed  _GoodThreshold;
			fixed  _CautionThreshold;
			fixed _ThresholdThickness;

			fixed4 GetThresholdColor(fixed4 color, float vertical_value)
			{
				if (vertical_value > _GoodThreshold)
				{
					color *= _GoodColor;
				}
				else if (vertical_value > _CautionThreshold)
				{
					color *= _CautionColor;
				}
				else
				{
					color *= _CriticalColor;
				}
				
				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 color = IN.color;

				fixed x_coord = IN.texcoord.x;
				fixed y_coord = IN.texcoord.y;

				const float vertical_value = GetVerticalValue(x_coord);
				
				color = GetThresholdColor(color, vertical_value);				
		
				color = FillBeneathLine(color, vertical_value, y_coord);
				
				color = NoFillAboveLine(color, vertical_value, y_coord);
												
				color = DrawHorizontalLine(color, y_coord, Average, AverageThickness, AverageColor);
								
				color = DrawHorizontalLine(color, y_coord, _CautionThreshold, _ThresholdThickness, _CautionColor);
								
				color = DrawHorizontalLine(color, y_coord, _GoodThreshold, _ThresholdThickness, _GoodColor);
				
				color = FadeEdges(color, x_coord);
				
				fixed4 output = SampleSpriteTexture(IN.texcoord) * color;
				
				output.rgb *= output.a;

				return output;
			}

			ENDCG
		}
	}
}