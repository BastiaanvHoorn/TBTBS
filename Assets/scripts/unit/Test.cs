using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.unit
{
    class Test:Unit
    {
        public override string name
        {
            get
            {
                return "test";
            }
        }
        public Test() {}
        public override string model_name 
        {
            get
            {
                return "test_unit.obj";
            }
        }
    }
}
