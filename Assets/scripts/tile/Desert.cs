using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.tile
{
    class Desert : Tile
    {
        public override Vector2 tex_location { get { return new Vector2(0, 768); } }
        public override int tex_prio { get { return 2; } }
        protected override void on_click()
        {
        }
    }
}
