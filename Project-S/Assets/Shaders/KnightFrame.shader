Shader "Custom/KnightFrame" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,0.5)
        _FrameColor("Frame Color", Color) = (1,1,1,0.5)
        _MainTex("Texture", 2D) = "white" { }
        _Width("Tex Width", Float) = 200.0
        _Height("Tex Height", Float) = 200.0
        _Thick("Line Thickness", Int) = 2
    }


        SubShader{
        Tags{
        "Queue" = "Transparent"
        "IgnoreProjector" = "True"
        "RenderType" = "Transparent"
        "PreviewType" = "Plane"
        "CanUseSpriteAtlas" = "False"
    }
        Cull Off 
        Pass{


        Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma target 3.0

        fixed4 _Color;
        fixed4 _FrameColor;
    sampler2D _MainTex;
    float _Width;
    float _Height;
    int _Thick;

    struct vin {
        float4 vertex: POSITION;
        float4 texcoord : TEXCOORD0;
    };
    struct v2f {
        float2 uv : TEXCOORD0;
        float4 pos : SV_POSITION;
    };

    v2f vert(vin v)
    {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord;
        return o;
    }

    fixed4 frag(v2f i) : SV_Target
    {
        float4 texcol = tex2D(_MainTex, i.uv);

        // このピクセルの周囲の透明度の最大値を調べる
        float alphaMax = 0.0f;
        for (int x = -_Thick; x <= _Thick; ++x)
            for (int y = -_Thick; y <= _Thick; ++y)
            {
                float alpha = tex2D(_MainTex, i.uv + float2(x / _Width, y / _Height)).a;
                if (alpha > alphaMax)
                    alphaMax = alpha;
            }

        // このピクセルが透明なら、周囲の透明度の最大値で塗る。
        if (texcol.a < 0.5)
            return float4(_FrameColor.x, _FrameColor.y, _FrameColor.z, alphaMax);
        else
            return texcol * _Color;
    }
        ENDCG
    }
    }
}