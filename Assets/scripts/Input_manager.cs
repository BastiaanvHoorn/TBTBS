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
        public Text movespeed;
        public Text strength;
        public Text type;
        public Input_manager(GameObject _focus, Animator _focus_an)
        {
            focus = _focus;
            focus_an = _focus_an;
            movespeed = GameObject.Find("stat 1").GetComponent<Text>();
            strength = GameObject.Find("stat 2").GetComponent<Text>();
            type = GameObject.Find("stat 3").GetComponent<Text>();

        }

        public void process_input(ref Unit_manager unit_manager, ref Tile_manager tile_manager, Player player)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Object.Destroy(current_range);
                for (int i = 0; i < tile_manager.count; i++)
                {
                    if (tile_manager[i].is_pixel_of_tile(Input.mousePosition, Camera.main))
                    {
                        Tile tile = tile_manager[i];
                        Vector3 tile_pos = tile.position;
                        move_focus(tile_pos);
                        Debug.Log(tile.position_cube);
                        Debug.Log(tile.position_axial);
                        if (Input.GetMouseButtonDown(0))
                        {
                            for (int j = 0; j < unit_manager.count; j++)
                            {
                                if (unit_manager[j].obj.transform.position == tile_pos && unit_manager[j].player == player)
                                {
                                    //When a unit is found at the clicked tile, switch the selected unit and exit all loops
                                    selected_unit = j;
                                    select_unit(unit_manager[j], ref unit_manager, ref tile_manager);
                                    goto Done;
                                }
                            }

                            //If no unit is found at the clicked tile, move the selected unit to this tile
                            if (selected_unit != -1)
                            {
                                unit_manager[selected_unit].move_goal = tile_manager[i];
                            }
                            selected_unit = -1;
                            goto Done;
                        }
                        else
                        {
                            Object.Destroy(current_range);
                            move_focus(tile_pos);
                            selected_unit = -1;
                        }
                    }
                }
                Done:;
            }
        }

        private void select_unit(Unit unit, ref Unit_manager unit_manager, ref Tile_manager tile_manager)
        {
            type.text = unit.name;
            movespeed.text = "Moverange: " + unit.move_range.ToString();
            strength.text = "Strength: " + unit.current_health.ToString() + "/" + unit.max_health.ToString();
            current_range = unit_manager[selected_unit].show_range(ref tile_manager);
            move_focus(unit.parrent_tile.position);
        }
        private void move_focus(Vector3 pos)
        {
            focus.transform.position = pos + new Vector3(0, .005f, 0);
            focus_an.Play("focus_fade", -1, .7f);
        }
    }
}
