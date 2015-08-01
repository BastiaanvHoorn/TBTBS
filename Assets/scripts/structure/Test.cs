using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.structure
{
    class Test : Structure
    {
        public override int attack_range
        {
            get
            {
                return -1;
            }
        }

        public override int damage
        {
            get
            {
                return -1;
            }
        }

        public override int max_health
        {
            get
            {
                return 20;
            }
        }

        public override string model_name
        {
            get
            {
                return "test_structure";
            }
        }

        public override string name
        {
            get
            {
                return "Test";
            }
        }
    }
}
