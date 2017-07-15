
// only when the combine tex's alpha value greater or equal to the value of cutoff can the pixel to be showed.

Shader "Javen/Mask" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Mask ("Culling Mask", 2D) = "white" {}
		_Cutoff ("Alpha Cutoff", Range(0,1)) = 0.1
	}
	
	SubShader
	{
		Tags { "Queue"="Transparent" }
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaTest GEqual [_Cutoff]
		
		Pass
		{
			SetTexture[_Mask] {combine texture}
			SetTexture[_MainTex] {combine texture, previous}
		}

	} 
}
