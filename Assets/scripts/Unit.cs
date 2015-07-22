using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.tile;

namespace Assets.Scripts
{
    public abstract class Unit
    {
        #region abstract unit stats
        public abstract string model_name { get; }
        public abstract string name { get; }
        public abstract int move_range { get; }
        public abstract int max_health { get; }
        public abstract int attack_range { get; }
        public abstract int damage { get; }
        #endregion

        public Tile parrent_tile { get; set; }
        public Path path { get; set; }
        public GameObject obj { get; set; }
        public int current_health { get; private set; }
        public Player player { get; set; }
        public Tile move_goal { get; set; }

        public Unit()
        {
            path = new Path();
            current_health = max_health;
        }

        //Create the actual gameobject in the scene
        #region spawning methods

        public void spawn()
        {
            obj = new GameObject("soldiers");

            add_unit(new Vector3(-1, 0, -1)).transform.parent = obj.transform;
            add_unit(new Vector3(.1f, 0, -.7f)).transform.parent = obj.transform;
            add_unit(new Vector3(.7f, 0, -1.5f)).transform.parent = obj.transform;
            add_unit(new Vector3(.2f, 0, .1f)).transform.parent = obj.transform;
            add_unit(new Vector3(1.4f, 0, -.1f)).transform.parent = obj.transform;
            add_unit(new Vector3(.8f, 0, .9f)).transform.parent = obj.transform;
            add_unit(new Vector3(-.4f, 0, 1.1f)).transform.parent = obj.transform;
            add_unit(new Vector3(-.9f, 0, .4f)).transform.parent = obj.transform;
        }

        private GameObject add_unit(Vector3 position)
        {
            GameObject unit = new GameObject("soldier");
            unit.AddComponent<MeshFilter>().mesh = Resources.Load<Mesh>("meshes/test_unit");
            unit.transform.localScale = new Vector3(.2f, .2f, .2f);
            //unit.AddComponent<MeshFilter>().mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/meshes/test_unit.3ds");
            Material material = unit.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
            if (this.player == Player.Blue)
            {
                material.color = reference.Player_color.blue;
            }
            else
            {
                material.color = reference.Player_color.red;
            }
            unit.transform.position = position;
            return unit;
        }
        public virtual string to_string()
        {
            return this.GetType().ToString() + " on " + this.obj.transform.position.ToString();
        }
        #endregion
        /// <summary>
        /// Sets the parrent_tile to the move_goal
        /// </summary>
        /// <param name="target">The tile that the unit will try to move to</param>
        /// <param name="spawn">If this move command spawns the unit. This will bypass the check if the target is actually in range</param>
        /// <returns>Returns true if the move succeeded, returns false if failed</returns>
        public virtual bool move(Tile_manager tile_manager = null)
        {
            if (tile_manager == null)
            {
                this.obj.transform.position = move_goal.position;
            }
            else
            {
                if (move_goal != null)
                {

                    path = new Path(parrent_tile.position_cube, move_goal.position_cube, tile_manager);
                }
            }
            return true;
            //if ((Tile_manager.is_in_range(this.parrent_tile, move_goal, move_range) && can_move || spawn) && unit_manager.is_tile_free(move_goal))
            //{
            //    if (!spawn)
            //    {
            //        can_move = false;
            //    }
            //    else
            //    {
            //        this.obj.transform.position = move_goal.position;
            //    }
            //    this.parrent_tile = move_goal;
            //    return true;
            //}
            //if (unit_manager.is_attackable(move_goal) && Tile_manager.is_in_range(move_goal, parrent_tile, attack_range) && can_attack)
            //{
            //    Unit move_goal_unit = unit_manager.get_unit_by_tile(move_goal);
            //    if (move_goal_unit.player != this.player)
            //    {

            //        if (move_goal_unit.attack(this))
            //        {
            //            unit_manager.kill(move_goal_unit);
            //        }
            //        can_attack = false;
            //        can_move = false;
            //    }
            //}
            //return false;
        }
        /// <summary>
        /// This function is called by the attacker
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>returns if the attacked unit survived</returns>
        public bool attack(Unit attacker)
        {
            int damage = attacker.damage;
            current_health -= damage;
            display_damage(damage);
            Debug.Log(attacker.to_string() + " attacked " + this.to_string() + " with " + damage + " damage; " + current_health + " health remaining.");
            if (current_health < 1)
            {
                return true;
            }

            return false;
        }
        private void display_damage(int damage)
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();

            GameObject damage_splat = new GameObject("damage splat");
            damage_splat.AddComponent<RectTransform>();
            damage_splat.AddComponent<CanvasRenderer>();
            Image image = damage_splat.AddComponent<Image>();

            image.sprite = Resources.Load<Sprite>("sprites/damage splats/" + damage);

            damage_splat.transform.SetParent(canvas.transform);
            RectTransform transform = damage_splat.GetComponent<RectTransform>();
            transform.localScale = new Vector3(.5f, .5f, .5f);
            Camera camera = Camera.main;
            Vector3 position = obj.transform.position;
            transform.position = camera.WorldToScreenPoint(position);
            GameObject.Destroy(damage_splat, 2);
        }

        /// <summary>
        /// Used to animate movement
        /// </summary>
        public void move_towards(Tile_manager tile_manager)
        {
            Vector3 parrent_pos = parrent_tile.position;
            Vector3 this_pos = obj.transform.position;
            if (path.tiles.Count > 0 && parrent_pos == this_pos)
            {
                parrent_tile = path.tiles[0];
                path.tiles.RemoveAt(0);

            }
            //Animation
            if (this_pos == parrent_pos)
            {
                return;
            }
            if (this_pos.y == parrent_pos.y || Util.v3_to_v2(this_pos, "y") == Util.v3_to_v2(parrent_pos, "y"))
            {
                this_pos = Vector3.MoveTowards(this_pos, parrent_pos, .1f);
            }
            else if (this_pos.y >= parrent_pos.y)
            {
                this_pos = Vector3.MoveTowards(this_pos, new Vector3(parrent_pos.x, this_pos.y, parrent_pos.z), .1f);
            }
            else
            {
                this_pos = Vector3.MoveTowards(this_pos, new Vector3(this_pos.x, parrent_pos.y, this_pos.z), .1f);
            }
            this.obj.transform.position = this_pos;
        }
        public GameObject show_range(ref Tile_manager world)
        {
            Tile_manager range = new Tile_manager();
            List<Tile> tiles = world.get_tiles_in_range(parrent_tile, move_range);

            range.add(tiles);
            range.add<Grassland>(Util.v2_to_v3(parrent_tile.position_offset, "y", parrent_tile.height));

            Vector3[] vertices = range.get_vertices();
            List<int> tri = range.get_tri(vertices);
            Vector2[] uv = range.get_uv();

            GameObject obj = new GameObject();
            Mesh mesh = obj.AddComponent<MeshFilter>().mesh;
            Renderer renderer = obj.AddComponent<MeshRenderer>();

            mesh.vertices = vertices;
            mesh.triangles = tri.ToArray();
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.Optimize();
            renderer.material = new Material(Shader.Find("Transparent/Diffuse"));
            renderer.material.SetColor("_Color", new Color(.12f, .85f, .12f, .5f));
            obj.transform.position += new Vector3(0, .01f, 0);

            return obj;
        }
    }
}
