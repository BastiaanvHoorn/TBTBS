using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.tile
{
    class Test :Tile
    {
        public Test(Vector3 _position, int _index) 
            : base(_position, _index)
        {
        }
        public Test(int x, int y, int z, int _index)
            : base(x, y, z, _index)
        {

        }

        protected override void on_click()
        {
            Debug.Log("clicked on Test");
        }
    }
}
