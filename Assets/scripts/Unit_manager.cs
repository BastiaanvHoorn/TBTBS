using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
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
            unit.next_tile = tile;
            unit.move_goal = tile;
            unit.start_move();
            Debug.Log("Spawning " + unit.to_string());
            return unit;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns>Returns null if no unit is found</returns>
        public Unit get_unit_by_next_tile(Tile tile)
        {
            return units.Find(unit => unit.next_tile.position_axial == tile.position_axial);
        }
        public Unit get_unit_by_occupying_tile(Tile tile)
        {
            Unit unit = units.Find(_unit => _unit.occupiying_tile.position_axial == tile.position_axial);
            return unit;
        }
        public void kill(Unit unit)
        {
            units.Remove(unit);
            Debug.Log(unit.to_string() + " has been killed");
            GameObject.Destroy(unit.obj);
        }

    }
}
