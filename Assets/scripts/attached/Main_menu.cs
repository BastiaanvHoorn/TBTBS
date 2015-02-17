using UnityEngine;
using System.Collections;

namespace Assets.scripts.attached
{
    public class Main_menu : MonoBehaviour
    {

        public void start_game()
        {
            Application.LoadLevel("world");
        }

    }
}
