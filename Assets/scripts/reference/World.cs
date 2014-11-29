using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.scripts.reference;

namespace Assets.scripts.reference
{
    class World
    {
        public static float vertical_offset = Math.sqrt_3 + .5f; //2.23205080757
        public static float vertical_space = vertical_offset * 2; //4.46410161514
        public static float horizontal_space = 3 + Math.cos_30; //3.86602540378

        public static Vector2 vertex0 = new Vector2(-1, -reference.Math.sqrt_3);
        public static Vector2 vertex1 = new Vector2(1, -reference.Math.sqrt_3);
        public static Vector2 vertex2 = new Vector2(2, 0);
        public static Vector2 vertex3 = new Vector2(1, reference.Math.sqrt_3);
        public static Vector2 vertex4 = new Vector2(-1, reference.Math.sqrt_3);
        public static Vector2 vertex5 = new Vector2(-2, 0);


        public static Vector2[] hex_uv = new Vector2[6]{
            new Vector2(64, 17),
            new Vector2(192, 17),
            new Vector2(256, 128),
            new Vector2(192, 238),
            new Vector2(64, 238),
            new Vector2(0, 128),
        };
        public static Vector2[] tri_uv = new Vector2[3]{
            new Vector2(256,0),
            new Vector2(366.85f,64),
            new Vector2(256,128),
        };
        public static Vector2[] rect_uv = new Vector2[4]{
            new Vector2(512,128),
            new Vector2(256,128),
            new Vector2(256,256),
            new Vector2(512,256)
        };

        public static decimal tex_scale = 0.00048828125m;
        public static int tex_square_amount = 8;
    }
}
