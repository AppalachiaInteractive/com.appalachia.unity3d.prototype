sampler2D _MainTex;
sampler2D _AlphaTex;
float _AlphaSplitEnabled;
fixed4 _Color = fixed4(1, 1, 1, 1);
uniform float PointSize = .04;
uniform float HeightOpacityMultiplier = .3;
uniform float Average;
fixed4 AverageColor = fixed4(1, 1, 1, 1);
uniform float AverageThickness = .02;
uniform float EdgeFadeWidth = .03;
uniform float GraphValues[512];
uniform float GraphValues_Length;

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ INTERPOLATE_ON
#include "UnityCG.cginc"

struct appdata_t
{
	float4 vertex    : POSITION;
	float4 color     : COLOR;
	float2 texcoord  : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
	float4 vertex    : SV_POSITION;
	fixed4 color	 : COLOR;
	float2 texcoord  : TEXCOORD0;
	UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert(appdata_t IN)
{
	v2f OUT;

	UNITY_SETUP_INSTANCE_ID(IN);
	UNITY_INITIALIZE_OUTPUT(v2f, OUT);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

	OUT.vertex = UnityObjectToClipPos(IN.vertex);
	OUT.texcoord = IN.texcoord;
	OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
	OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

	return OUT;
}

fixed4 SampleSpriteTexture(float2 uv)
{
	fixed4 color = tex2D(_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
	if (_AlphaSplitEnabled)
		color.a = tex2D(_AlphaTex, uv).r;
#endif

	return color;
}

/*
float GetVerticalValue(float x_coord)
{
	const float raw_index = x_coord * GraphValues_Length;
	const int current_index = floor(raw_index);
				
	const float vertical_value = GraphValues[current_index];

	return vertical_value;
}*/

float GetVerticalValue(float x_coord)
{
	const float raw_index = x_coord * GraphValues_Length;
	const int current_index = floor(raw_index);
	const float current_vertical_value = GraphValues[current_index];
	
	// ReSharper disable once CppLocalVariableMayBeConst because when INTERPOLATE_ON we modify it.
	float vertical_value = current_vertical_value;
	
	#ifdef INTERPOLATE_ON
	
	int next_index = ceil(raw_index);
				
	if (next_index >= GraphValues_Length)
	{
		next_index = current_index;
	}
	
	const float next_vertical_value = GraphValues[next_index];	

	const float next_index_strength = raw_index - current_index;
	const float current_index_strength = 1.0 - next_index_strength;
				
	vertical_value = (current_vertical_value*current_index_strength) + (next_vertical_value*next_index_strength);

	#endif
			
	return vertical_value;
}

fixed4 DrawHorizontalLine(fixed4 color, const float y_coord, const float height, const float thickness, const fixed4 lineColor)
{	
	if (y_coord < height && y_coord > height - thickness)
	{
		if (color.a > 0)
		{
			color = (color+lineColor)*.5;
		}
		else
		{
			color = lineColor;						
		}					
	}

	return color;
}

fixed4 FadeEdges(fixed4 color, const float x_coord)
{	
	if (x_coord < EdgeFadeWidth)
	{
		color.a *= 1 - (EdgeFadeWidth - x_coord) / EdgeFadeWidth;
	}
	else if (x_coord > 1 -EdgeFadeWidth)
	{
		color.a *= (1 - x_coord) / EdgeFadeWidth;
	}

	return color;
}


fixed4 NoFillAboveLine(fixed4 color, const float vertical_value, const float y_coord)
{	
	if (y_coord > vertical_value)
	{
		color.a = 0;
	}
	
	return color;
}
		
fixed4 NoFillBeneathLine(fixed4 color, const float vertical_value, const float y_coord)
{	
	if (vertical_value - y_coord > PointSize)
	{
		color.a = 0;
	}

	return color;
}
		
fixed4 FillBeneathLine(fixed4 color, const float vertical_value, const float y_coord)
{	
	if (vertical_value - y_coord > PointSize)
	{
		color.a *= y_coord * HeightOpacityMultiplier / vertical_value;
	}

	return color;
}