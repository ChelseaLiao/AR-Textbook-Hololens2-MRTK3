Shader "Custom/RoundRect/Title"
{
    Properties
    {
         _Color("Color", Color) = (1,1,1,1)
         _MainTex("Texture", 2D) = "white" {}
         _Radius("Radius", Range(0, 0.5)) = 0.025
         _scale("Scale", vector) = (1,1,0,1)

        [MaterialToggle] _TR("TopRightCorner", Float) = 1
        [MaterialToggle] _BR("BottomRightCorner", Float) = 1
        [MaterialToggle] _BL("BottomLeftCorner", Float) = 1
        [MaterialToggle] _TL("TopLeftCorner", FLoat) = 1
        [Header(DrawCircle)]
        [MaterialToggle] _DrawCircle("DrawCircle", Float) = 1

    }
        SubShader
        {
            Tags {
                   "RenderType" = "Transparent"
                   "Queue" = "Transparent"
                   "IgnoreProjector" = "True"
                   "PreviewType" = "Plane"
                   "CanUseSpriteAtlas" = "True"
                  }
            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha
                ZWrite Off
                ZTest LEqual
                Cull Off
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #pragma multi_compile __ UNITY_UI_ALPHACLIP
                #include "UnityCG.cginc"
                #include "UnityUI.cginc"




                struct appdata
                {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;

                    float4 vertex : SV_POSITION;
                    float3 worldPos : TEXCOORD1;
                    fixed4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                fixed4 _TextureSampleAdd;
                float _Radius;
                float _TR, _BR, _BL, _TL, _DrawCircle;
                float4 _scale;
                float4 _Color;

                //====================Circle Drawing========================
                float DrawCircle(float x, float y, float r, float sfx1,float sfy1,v2f i)
                {
                    float x_r = x - i.uv.x;
                    float y_r = y - i.uv.y;
                    float dis = sqrt((x_r) * (x_r) / sfx1 + (y_r) * (y_r) / sfy1);
                    if (dis <= r)
                    {
                        if (r - dis < 0.001)
                        {
                            return (r - dis) / 0.001;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 0;
                    }


                }


                v2f vert(appdata v)
                {
                    v2f o;
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                    o.color = v.color * _Color;

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {


                    // UV sampling data
                    fixed4 col = (tex2D(_MainTex, i.uv) + _TextureSampleAdd) * i.color;

                // Used to save the pixel distance from the center of the circle
                half dist = 0;
                //  Default top-right center coordinates
                half2 TRC = half2(1, 1);
                // Default coordinates of the center of the circle in the lower right corner
                half2 BRC = half2(1, 0);
                // Default coordinates of the center of the circle in the lower left corner
                half2 BLC = half2(0, 0);
                // Default coordinates of the center of the circle in the upper left corner
                half2 TLC = half2(0, 1);
                // Correct the X coordinate position of the center of the circle
                half sfx0 = 1;
                // Correct the distance between the UV point and the center of the circle
                half sfx1 = 1;
                // Correct the position of the Y-axis coordinate of the center of the circle
                half sfy0 = 1;
                // Correct the distance between the UV point and the center of the circle
                half sfy1 = 1;


                // Calculate center of circle, UV correction factor when width > height of rectangle
                if (_scale.x > _scale.y)
                {
                    _scale.x = _scale.x / _scale.y;
                    _scale.y = 1;
                    // Calculate the scaling factor for the X coordinate of the center of the circle in UV coordinate space.
                    sfx0 = _scale.y / _scale.x;
                    sfx1 = (_scale.y / pow(_scale.x,2));
                }
                else
                {
                    _scale.y = _scale.y / _scale.x;
                    _scale.x = 1;
                    // Calculate the scaling factor for the center Y coordinate in UV coordinate space.
                    sfy0 = _scale.x / _scale.y;
                    sfy1 = (_scale.x / pow(_scale.y,2));
                }

                half2 center = half2(TRC.x - _Radius * sfx0,TRC.y - _Radius * sfy0);
                //==============upper right================
                if (_TR == 1)
                {

                    if (center.x < i.uv.x && center.y < i.uv.y)
                    {
                        dist = sqrt(pow(center.x - i.uv.x, 2) / sfx1 + pow(center.y - i.uv.y , 2) / sfy1);
                        //col.rgb *= float3(1,0,0);                        

                    }

                    if (_DrawCircle == 1)
                    {
                       float c1 = DrawCircle(center.x,center.y,_Radius,sfx1,sfy1,i);
                       if (c1 == 1)
                       {
                           col.rgb *= float3(1,0,0);
                       }

                    }

                    if (dist > _Radius)
                    {
                        col.a = 0;
                        //discard;
                    }
                }
                //==============lower right================
                half2 center02 = half2(BRC.x - _Radius * sfx0, BRC.y + _Radius * sfy0);

                if (_BR == 1)
                {

                    if (center02.x < i.uv.x && center02.y > i.uv.y)
                    {
                        dist = sqrt(pow(center02.x - i.uv.x, 2) / sfx1 + pow(center02.y - i.uv.y , 2) / sfy1);


                    }
                    if (_DrawCircle == 1)
                    {
                        float c2 = DrawCircle(center02.x,center02.y,_Radius,sfx1,sfy1,i);
                        if (c2 == 1)
                        {
                            col.rgb *= float3(0,1,0);
                        }

                    }


                    if (dist > _Radius)
                    {
                        col.a = 0;
                    }
                }
                //===============lower left================
                half2 center03 = half2(saturate(BLC.x + _Radius * sfx0), saturate(BLC.y + _Radius * sfy0));

                if (_BL == 1)
                {

                    if (center03.x > i.uv.x && center03.y > i.uv.y)
                    {
                        dist = sqrt(pow(center03.x - i.uv.x, 2) / sfx1 + pow(center03.y - i.uv.y ,2) / sfy1);

                    }
                    if (_DrawCircle == 1)
                    {
                        float c3 = DrawCircle(center03.x,center03.y,_Radius,sfx1,sfy1,i);
                        if (c3 == 1)
                        {
                            col.rgb *= float3(0,0,1);
                        }

                    }

                    if (dist > _Radius)
                    {
                        col.a = 0;
                    }
                }
                //==============upper left=================
                half2 center04 = half2(TLC.x + _Radius * sfx0, TLC.y - _Radius * sfy0);
                if (_TL == 1)
                {

                    if (center04.x > i.uv.x && center04.y < i.uv.y)
                    {
                        dist = sqrt(pow(center04.x - i.uv.x, 2) / sfx1 + pow(center04.y - i.uv.y , 2) / sfy1);
                    }
                    if (_DrawCircle == 1)
                    {
                        float c4 = DrawCircle(center04.x,center04.y,_Radius,sfx1,sfy1,i);
                        if (c4 == 1)
                        {
                            col.rgb *= float3(1,1,0);
                        }

                    }


                    if (dist > _Radius)
                    {
                        col.a = 0;
                    }
                }
                //================core judgement============================

                return col;
            }
            ENDCG
        }
        }
}