using System;

namespace Assets.Scripts.unit
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
        public override string model_name 
        {
            get
            {
                return "test_unit.obj";
            }
        }

        public override int move_range
        {
            get
            {
                return 2;
            }
        }

        public override int max_health
        {
            get
            {
                return 10;
            }
        }

        public override int attack_range
        {
            get
            {
                //mellee
                return 1;
            }
        }

        public override int damage
        {
            get
            {
                return (int)Math.Ceiling(current_health / 4.0);
            }
        }

        public Test() {}
    }
}
