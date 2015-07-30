using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.tile;

namespace Assets.Scripts
{
    public abstract class Unit
    {
        #region variable declaration
        #region abstract unit stats
        public abstract string model_name { get; }
        public abstract string name { get; }
        public abstract int move_range { get; }
        public abstract int max_health { get; }
        public abstract int attack_range { get; }
        public abstract int damage { get; }
        #endregion
        #region tiles to keep track of
        /// <summary>
        /// The tile that the unit is moving to.
        /// </summary>
        public Tile next_tile { get; set; }
        /// <summary>
        /// The tile that the unit is moving from.
        /// </summary>
        public Tile last_tile { get; set; }
        /// <summary>
        /// The tile that the unit is occupying, rounded to the nearest tile while moving. Used for combat.
        /// </summary>
        public Tile occupiying_tile { get; set; }
        #endregion
        public bool is_moving { get; set; }
        private float move_range_left { get; set; }
        public Path path { get; set; }
        public GameObject obj { get; set; }
        public int current_health { get; private set; }
        public Player player { get; set; }
        public Unit()
        {
            path = new Path();
            current_health = max_health;
        }
        #endregion
        #region spawning methods
        //Create the actual gameobject in the scene
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

            Material material = unit.AddComponent<MeshRenderer>().material;
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
            string s = string.Format("{0} {1} on {2}", this.player.ToString(), this.GetType().ToString(), this.obj.transform.position.ToString());
            return s;
        }
        #endregion
        #region movement methods
        /// <summary>
        /// Sets the next_tile to the move_goal
        /// </summary>
        /// <param name="target">The tile that the unit will try to move to</param>
        /// <param name="spawn">If this move command spawns the unit. This will bypass the check if the target is actually in range</param>
        /// <returns>Returns true if the move succeeded, returns false if failed</returns>
        public virtual void start_move(Tile_manager tile_manager)
        {
            move_range_left = move_range;
            is_moving = true;
            occupiying_tile = next_tile;
            last_tile = next_tile;
        }
        /// <summary>
        /// Used to animate movement
        /// </summary>
        public void move_update(Tile_manager tile_manager, Unit_manager unit_manager)
        {
            if (is_moving)
            {

                Vector3 next_pos = next_tile.position;
                Vector3 this_pos = obj.transform.position;
                //If we are over half of the movement to the next tile, we are no longer occupying the tile we came from but instead the tile we are moving towards
                if (Vector3.Distance(this_pos, next_pos) < Vector3.Distance(this_pos, occupiying_tile.position))
                {
                    //Check if we can attack the next tile
                    Unit target = next_tile.is_attackable(unit_manager, this);
                    if (target != null)
                    {
                        //Attack the target
                        if (!target.attack(this, unit_manager))
                        {
                            //If the target didn't die, go to the last visited tile and stop moving
                            next_tile = last_tile;

                            path.tiles.Clear();
                        }
                        else
                        {
                            unit_manager.kill(target);
                        }
                    }
                    occupiying_tile = next_tile;

                }
                //If we are at our last selected destination, try to select a new one.
                if (next_pos == this_pos)
                {
                    //If we have a path to follow, go look at some movement stuff
                    if (path.tiles.Count > 0)
                    {
                        move_range_left -= path.next.move_cost;
                        if (move_range_left > 0)
                        {
                            last_tile = next_tile;
                            next_tile = path.next;
                            //Remove the destination (at which we arrived) from the list
                            path.tiles.RemoveAt(0);
                        }
                        else
                        {
                            is_moving = false;
                        }

                    }
                    else
                    {
                        //If the path is empty, stop with moving
                        is_moving = false;
                    }
                }

                #region animation
                //If the y position of our position and the next tile are equal, or if everythig except the y coordinates are equal, move towards the tile in a straight line
                //float ms_factor = 0;
                Vector3 _move_pos;
                Vector3 occ_pos = last_tile.position;
                float hsq3 = reference.World.half_sqrt_3;
                if (this_pos.y == next_pos.y || Vector2.Distance(Util.v3_to_v2(this_pos, "y"), Util.v3_to_v2(next_pos, "y")) < 2)
                {
                    _move_pos = Vector3.MoveTowards(this_pos, next_pos, .1f / occupiying_tile.move_cost);
                }
                //If we are higher then our goal, move horizontally to the edge of this tile
                else if (this_pos.y >= next_pos.y)
                {
                    if (occ_pos.x < next_pos.x)
                    {
                        _move_pos = Vector3.MoveTowards(this_pos, new Vector3(next_pos.x - hsq3, this_pos.y, next_pos.z - .5f), .1f / occupiying_tile.move_cost);
                    }
                    else if (occ_pos.x > next_pos.x)
                    {
                        _move_pos = Vector3.MoveTowards(this_pos, new Vector3(next_pos.x - hsq3, this_pos.y, next_pos.z + .5f), .1f / occupiying_tile.move_cost);
                    }
                    else
                    {
                        _move_pos = Vector3.MoveTowards(this_pos, new Vector3(next_pos.x, this_pos.y, next_pos.z + .5f), .1f / occupiying_tile.move_cost);
                    }
                }
                else
                {
                    if (occ_pos.x == next_pos.x)
                    {
                        _move_pos = Vector3.MoveTowards(this_pos, new Vector3(this_pos.x, next_pos.y, occ_pos.z + .5f), .1f / occupiying_tile.move_cost);
                    }
                    else
                    {
                        _move_pos = Vector3.MoveTowards(this_pos, new Vector3(
                            occ_pos.x + ((occ_pos.x < next_pos.x) ? hsq3 : -hsq3),
                            next_pos.y,
                            occ_pos.z + ((occ_pos.z < next_pos.z) ? .5f : -.5f)),
                            .1f / occupiying_tile.move_cost);
                    }
                }
                this_pos = _move_pos;
                //Debug.Log(reference.World.diagonal_space);
                this.obj.transform.position = this_pos;
                #endregion
            }
        }
        #endregion
        /// <summary>
        /// Attack this unit
        /// </summary>
        /// <param name="attacker">The unit that is attacking</param>
        /// <param name="first">If this was a counter attack</param>
        /// <returns>returns if the attacked unit survived</returns>
        public virtual bool attack(Unit attacker, Unit_manager unit_manager, bool counter = false)
        {
            int damage = attacker.damage;
            current_health -= damage;
            display_damage(damage);

            Debug.Log(attacker.to_string() + " attacked " + this.to_string() + " with " + damage + " damage; " + current_health + " health remaining.");
            if (current_health < 1)
            {
                return true;
            }

            //If you were attacked first, do a counter attack
            if (!counter)
            {
                //Since we were attacked, cancel all movement for the rest of this turn.
                path.tiles.Clear();
                next_tile = occupiying_tile;
                //Do a counter-attack
                if (attacker.attack(this, unit_manager, true))
                {
                    unit_manager.kill(attacker);
                }
            }
            return false;
        }
        #region display methods
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
        public GameObject display_range(ref Tile_manager world, Path _path = null)
        {
            //If no path is specified, use the path that is already set.
            //Also don't display the range we can't move to
            bool display_actual_path = false;
            if (_path == null)
            {
                _path = path;
                display_actual_path = true;
            }

            GameObject _object = new GameObject("range");
            _object.transform.position = obj.transform.position;
            Tile last_tile = occupiying_tile;
            float display_range_left = move_range;
            foreach (Tile tile in _path.tiles)
            {
                display_range_left -= tile.move_cost;
                if (display_actual_path)
                {
                    if (display_range_left <= 0)
                    {
                        break;
                    }
                }
                GameObject plane = create_pathing_plane(tile, last_tile, (display_range_left > 0 ? Color.green : Color.red));
                plane.transform.SetParent(_object.transform);
                GameObject last_plane = create_pathing_plane(last_tile, tile, (display_range_left > 0 ? Color.green : Color.red));
                last_plane.transform.SetParent(_object.transform);
                last_tile = tile;
            }
            return _object;
        }
        private GameObject create_pathing_plane(Tile tile, Tile last_tile, Color color)
        {
            GameObject plane_parent = new GameObject("parent");
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.SetParent(plane_parent.transform);
            plane_parent.transform.position += new Vector3(tile.position.x, tile.position.y, tile.position.z);
            plane.transform.localScale = new Vector3(0.1f, 0.01f, 0.2f * (reference.Math.sqrt_3 / 2));
            plane.transform.position += new Vector3(0, 0, (reference.Math.sqrt_3 / 2));
            if (tile.position_axial + new Vector2(1, 0) == last_tile.position_axial)
            {
                plane_parent.transform.Rotate(0, 60, 0);
            }
            else if (tile.position_axial + new Vector2(1, -1) == last_tile.position_axial)
            {
                plane_parent.transform.Rotate(0, 120, 0);
            }
            else if (tile.position_axial + new Vector2(0, -1) == last_tile.position_axial)
            {
                plane_parent.transform.Rotate(0, 180, 0);
            }
            else if (tile.position_axial + new Vector2(-1, 0) == last_tile.position_axial)
            {
                plane_parent.transform.Rotate(0, 240, 0);
            }
            else if (tile.position_axial + new Vector2(-1, 1) == last_tile.position_axial)
            {
                plane_parent.transform.Rotate(0, 300, 0);
            }
            float angle = Vector2.Angle(Util.v3_to_v2(tile.position, "y"), Util.v3_to_v2(last_tile.position, "y"));
            plane.GetComponent<MeshRenderer>().material.color = color;
            plane_parent.transform.position += new Vector3(0, 0.01f);
            return plane_parent;
        }
        #endregion
    }
}
