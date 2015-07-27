using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.tile
{
    class Stone : Tile
    {
        public override Vector2 tex_location { get { return new Vector2(0, 512); } }
        public override int tex_prio { get { return 3; } }
        public override float move_cost { get { return 1.2f; } }
        protected override void on_click()
        {
        }
    }
}
