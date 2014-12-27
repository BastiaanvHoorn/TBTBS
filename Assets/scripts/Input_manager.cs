using UnityEngine;
namespace Assets.scripts
{
    class Input_manager
    {
        public GameObject focus;
        public Animator focus_an;
        public int selected_unit; //Index of the unit currently selected, if none: -1
        public Input_manager(GameObject _focus, Animator _focus_an)
        {
            focus = _focus;
            focus_an = _focus_an;
        }

        public void process_input(ref Unit_manager unit_manager, ref Tile_manager tile_manager)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
            {
                for (int i = 0; i < tile_manager.count; i++)
                {
                    if (tile_manager[i].check_click(Input.mousePosition, Camera.main))
                    {
                        if (Input.GetMouseButton(0))
                        {
                            Vector3 tile_pos = tile_manager[i].position;
                            move_focus(tile_pos);
                            for (int j = 0; j < unit_manager.count; j++)
                            {
                                if (unit_manager[j].obj.transform.position == tile_pos)
                                {
                                    selected_unit = j;
                                }
                            }

                        }
                        else
                        {
                            //If the other mouse-button is pressed, move the selected unit to the clicked tile
                            if (tile_manager[i].check_click(Input.mousePosition, Camera.main) && selected_unit != -1)
                            {
                                Vector3 tile_pos = tile_manager[i].position;
                                move_focus(tile_pos);

                                unit_manager[selected_unit].move(tile_manager[i], unit_manager);


                            }
                        }
                    }
                }
            }
        }
        private void move_focus(Vector3 pos)
        {
            focus.transform.position = pos + new Vector3(0, .005f, 0);
            focus_an.Play("focus_fade", -1, .7f);
        }
    }
}
