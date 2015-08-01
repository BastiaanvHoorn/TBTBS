using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts
{
    abstract class Structure
    {
        public abstract string model_name { get; }
        /// <summary>
        /// The name that will be displayed on the screen when the structure is selected
        /// </summary>
        public abstract string name { get; }
        public abstract int max_health { get; }
        //attack_range and damage only where applicable
        public abstract int attack_range { get; }
        public abstract int damage { get; }
        /// <summary>
        /// The tile that the structure is occupying, rounded to the nearest tile while moving. Used for combat.
        /// </summary>
        public Tile occupiying_tile { get; set; }

        public int current_health { get; private set; }
        public GameObject obj { get; set; }
        public Player player { get; set; }
        public Structure()
        {
            current_health = max_health;
        }

        public void build()
        {
            obj = new GameObject("structure");
            obj.AddComponent<MeshFilter>().mesh = Resources.Load<Mesh>("meshes/" + model_name);
            //structure.transform.localScale = new Vector3(.2f, .2f, .2f);

            Material material = obj.AddComponent<MeshRenderer>().material;
            if (this.player == Player.Blue)
            {
                material.color = reference.Player_color.blue;
            }
            else
            {
                material.color = reference.Player_color.red;
            }
            obj.transform.position = occupiying_tile.position;
        }

        public virtual string to_string()
        {
            string s = string.Format("{0} {1} on {2}", this.player.ToString(), this.GetType().ToString(), this.obj.transform.position.ToString());
            return s;
        }
    }
}
