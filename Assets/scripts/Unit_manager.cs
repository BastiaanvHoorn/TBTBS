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
            unit.can_move = true;
            unit.parrent_tile = tile;
            unit.move(tile, this, true);
            Debug.Log("Spawning " + unit.to_string());
            return unit;
        }
        public bool is_tile_free(Tile tile)
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
        public bool is_attackable(Tile tile)
        {
            foreach (Unit unit in units)
            {
                if (unit.parrent_tile.position_axial == tile.position_axial)
                {
                    return true;
                }
            }
            return false;
        }

        public Unit get_unit_by_tile(Tile tile)
        {
            return units.Find(unit => unit.parrent_tile.position_axial == tile.position_axial);
        }
        public void kill(Unit unit)
        {
            units.Remove(unit);
            Debug.Log(unit.to_string() + " has been killed");
            GameObject.Destroy(unit.obj);
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
