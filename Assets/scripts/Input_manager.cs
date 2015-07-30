using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    //THIS CLASS DOES NOT HANDLE THE CAMERA CONTROLS
    //THE CAMERA CONTROLS ARE HANDELED IN "attached/Camera_controller.cs"
    class Input_manager
    {
        public GameObject focus;
        public Animator focus_an;
        private int selected_unit; //Index of the unit currently selected, if none: -1
        private GameObject current_range;
        private GameObject old_range;
        public Text movespeed;
        public Text strength;
        public Text type;
        public Input_manager(GameObject _focus, Animator _focus_an)
        {
            selected_unit = -1;
            focus = _focus;
            focus_an = _focus_an;
            movespeed = GameObject.Find("stat 1").GetComponent<Text>();
            strength = GameObject.Find("stat 2").GetComponent<Text>();
            type = GameObject.Find("stat 3").GetComponent<Text>();

        }

        public void process_input(ref Unit_manager unit_manager, ref Tile_manager tile_manager, Player player)
        {
            if(current_range != null)
            {
                Object.Destroy(old_range);
            }
            if (Input.GetMouseButtonDown(0))
            {
                foreach (Tile tile in tile_manager.tiles)
                {
                    Object.Destroy(current_range);
                    if (tile.is_pixel_of_tile(Input.mousePosition, Camera.main))
                    {
                        Vector3 tile_pos = tile.position;
                        move_focus(tile_pos);
                        //Debug.Log(tile.position_cube);
                        //Debug.Log(tile.position_axial);
                        //Debug.Log(tile.position);
                        for (int j = 0; j < unit_manager.count; j++)
                        {
                            if (unit_manager[j].obj.transform.position == tile_pos && unit_manager[j].player == player)
                            {
                                //When a unit is found at the clicked tile, switch the selected unit and exit all loops
                                selected_unit = j;
                                select_unit(unit_manager[j], ref unit_manager, ref tile_manager);
                                if(unit_manager[j].path != null)
                                {
                                    old_range = unit_manager[j].display_range(ref tile_manager);
                                }
                                goto Done;
                            }
                        }


                        if (selected_unit != -1)
                        {
                            Path path = new Path(unit_manager[selected_unit].occupiying_tile, tile, tile_manager, unit_manager[selected_unit]);
                            unit_manager[selected_unit].path = path;
                        }
                        selected_unit = -1;
                        break;
                    }
                }
            Done:;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Tile tile = get_tile_at_mouse(tile_manager);
                if (tile != null)
                {
                    if (selected_unit != -1)
                    {
                        Object.Destroy(current_range);
                        selected_unit = -1;
                    }
                    move_focus(tile.position);

                }
            }
            else
            {
                if (selected_unit != -1)
                {
                    foreach (Tile tile in tile_manager.tiles)
                    {
                        if (tile.is_pixel_of_tile(Input.mousePosition, Camera.main))
                        {
                            Object.Destroy(current_range);
                            Path path = new Path(unit_manager[selected_unit].occupiying_tile, tile, tile_manager, unit_manager[selected_unit]);
                            if(path.tiles.Count != 0)
                            {
                                current_range = unit_manager[selected_unit].display_range(ref tile_manager, path);

                            }
                            move_focus(tile.position);
                        }
                    }
                }
            }
        }

        private void select_unit(Unit unit, ref Unit_manager unit_manager, ref Tile_manager tile_manager)
        {
            type.text = unit.name;
            movespeed.text = "Moverange: " + unit.move_range.ToString();
            strength.text = "Strength: " + unit.current_health.ToString() + "/" + unit.max_health.ToString();
            move_focus(unit.next_tile.position);
        }
        private void move_focus(Vector3 pos)
        {
            if (focus.transform.position - new Vector3(0, .005f, 0) != pos)
            {
                focus.transform.position = pos + new Vector3(0, .005f, 0);
                focus_an.Play("focus_fade", -1, .7f);
            }
        }
        private Tile get_tile_at_mouse(Tile_manager tile_manager)
        {
            foreach (Tile tile in tile_manager.tiles)
            {
                if (tile.is_pixel_of_tile(Input.mousePosition, Camera.main))
                {
                    return tile;
                }
            }
            return null;
        }
    }
}
