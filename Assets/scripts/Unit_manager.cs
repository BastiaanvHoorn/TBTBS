using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    public class Unit_manager
    {
        public List<Unit> units { get; private set; }
        public int count
        {
            get { return units.Count; }
        }

        public Unit_manager()
        {
            units = new List<Unit>();
        }
        public Unit this[int index]
        {
            get { return units[index]; }
        }

        public Unit add<Unit_type>(Tile tile, Player player) where Unit_type :Unit, new()
        {
            Unit_type unit = new Unit_type();
            units.Add(unit);
            unit.player = player;
            unit.spawn();
            unit.move(tile, this, true);
            Debug.Log("Spawning " + unit.to_string());
            return unit;
        }
        public bool is_tile_empty(Tile tile)
        {
            for (int i = 0; i < count; i++)
            {
                if (units[i].parrent_tile == tile)
                {
                    return false;
                }
            }
            return true;
        }
        
        public void move_units()
        {
            foreach(Unit unit in units)
            {
                unit.move_towards();
            }
        }
    }
}
