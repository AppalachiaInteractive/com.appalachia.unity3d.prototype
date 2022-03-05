Shader "appalachia/v01/graphs/line"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[MaterialToggle] Interpolate("Interpolate", Float) = 0
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
				
			#include "graphs.cginc"
			
			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 color = IN.color;

				const fixed x_coord = IN.texcoord.x;
				const fixed y_coord = IN.texcoord.y;

				const float vertical_value = GetVerticalValue(x_coord);
		
				color = NoFillBeneathLine(color, vertical_value, y_coord);
				
				color = NoFillAboveLine(color, vertical_value, y_coord);
				
				color = DrawHorizontalLine(color, y_coord, Average, AverageThickness, AverageColor);
				
				color = FadeEdges(color, x_coord);
				
				fixed4 output = SampleSpriteTexture(IN.texcoord) * color;
				
				output.rgb *= output.a;
				
				return output;
			}

			ENDCG
		}
	}
}