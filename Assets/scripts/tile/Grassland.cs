using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.tile
{
    class Grassland : Tile
    {
        public override Vector2 tex_location { get { return new Vector2(0, 256); } }
        public override int tex_prio { get { return 1; } }
        public override float move_cost { get { return 1f; } }


        protected override void on_click()
        {
        }
    }
}
