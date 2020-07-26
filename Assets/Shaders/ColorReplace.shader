Shader "Unlit/ColorReplace"
{
    Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RedReplacement ("Red replacment", Color) = (1, 0, 0, 0)
		_GreenReplacement ("Green replacment", Color) = (0, 1, 0, 0)
    }

    SubShader
    {
        Tags {
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

        LOD 100

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _RedReplacement;
			float4 _GreenReplacement;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

				half a = col.a;
				float red = step(.5, col.r);
				float green = step(.5, col.g);
				col = _RedReplacement * (col.r - green * red * .125) + _GreenReplacement * col.g;
				col.rgb *= a;
				col.a = a;
                return col;
            }
            ENDCG
        }
    }
}
