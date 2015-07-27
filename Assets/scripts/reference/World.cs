using UnityEngine;

namespace Assets.Scripts.reference
{
    public static class World
    {
        public const float vertical_offset = Math.sqrt_3 + .5f; //2.23205080757
        public const float vertical_space = vertical_offset * 2; //4.46410161514
        public const float horizontal_space = 3 + Math.cos_30; //3.86602540378
        public const float half_sqrt_3 = Math.sqrt_3 / 2;

        public readonly static Vector2 vertex0 = new Vector2(-1, -Math.sqrt_3);
        public readonly static Vector2 vertex1 = new Vector2(1, -Math.sqrt_3);
        public readonly static Vector2 vertex2 = new Vector2(2, 0);
        public readonly static Vector2 vertex3 = new Vector2(1, Math.sqrt_3);
        public readonly static Vector2 vertex4 = new Vector2(-1, Math.sqrt_3);
        public readonly static Vector2 vertex5 = new Vector2(-2, 0);

        public static Vector2 get_hex_uv(int i)
        {
            return hex_uv[i] * (float)tex_scale;
        }
        private static Vector2[] hex_uv = new Vector2[6]
        {
            new Vector2(66, 19),  //bot-left
            new Vector2(190, 19), //bot-right
            new Vector2(254, 128),//mid-right
            new Vector2(190, 236),//top-right
            new Vector2(66, 236), //top-left
            new Vector2(2, 128),  //mid-left
        };

        public static Vector2 get_tri_uv(int i)
        {
            return tri_uv[i] * (float)tex_scale;
        }
        private static Vector2[] tri_uv = new Vector2[3]
        {
            new Vector2(258,2),     //bot-left
            new Vector2(364.85f,64),//right
            new Vector2(258,126),   //top-left
        };

        public static Vector2 get_rect_uv(int i)
        {
            return rect_uv[i] * (float)tex_scale;
        }
        private static Vector2[] rect_uv = new Vector2[4]
        {
            new Vector2(510,130),//bot-right
            new Vector2(258,130),//bot-left
            new Vector2(258,254),//top-left
            new Vector2(510,254) //top-right
        };

        public const decimal tex_scale = 0.00048828125m;
        public const int tex_square_amount = 8;
    }
}
