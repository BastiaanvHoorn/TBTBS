using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.unit
{
    class Test:Unit
    {
        public Test() {}
        public override string model_name 
        {
            get
            {
                return "test_unit.blend";
            }
        }

        public override void move(Tile target)
        {
            this.parrent_tile = target;
            this.obj.transform.position = target.position;
        }
    }
}
