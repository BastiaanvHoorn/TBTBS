﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Unit add<Unit_type>(Tile tile) where Unit_type :Unit, new()
        {
            Unit_type unit = new Unit_type();
            units.Add(unit);
            
            unit.move(tile, this);
            return unit;
        }

        public bool is_tile_empty(Tile tile)
        {
            for(int i = 0; i < count; i++)
            {
                if(units[i].parrent_tile == tile)
                {
                    return false;
                }
            }
            return true;
        }
    }
}