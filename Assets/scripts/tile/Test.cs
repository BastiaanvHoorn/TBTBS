﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.tile
{
    class Test : Tile
    {
        public override Vector2 tex_location { get { return new Vector2(); } }
        public override int tex_prio { get { return -99; } }

        protected override void on_click()
        {
        }
    }
}
