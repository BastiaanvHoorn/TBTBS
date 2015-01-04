using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.tile
{
    class Grassland : Tile
    {
        public override Vector2 tex_location
        {
            get
            {
                return new Vector2(0, 256);
            }
        }

        protected override void on_click()
        {
        }
    }
}
