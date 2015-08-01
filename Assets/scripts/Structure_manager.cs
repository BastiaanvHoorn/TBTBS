using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class Structure_manager
    {
        List<Structure> structures;
        public Structure_manager()
        {
            structures = new List<Structure>();
        }
        public Structure this[int index]
        {
            get { return structures[index]; }
        }
        public Structure add<Structure_type>(Tile tile, Player player) where Structure_type : Structure, new()
        {
            Structure_type structure = new Structure_type();
            structures.Add(structure);
            structure.player = player;
            structure.occupiying_tile = tile;
            structure.build();
            structure.obj.transform.position = tile.position;
            UnityEngine.Debug.Log("Spawning " + structure.to_string());
            return structure;
        }
    }
}
