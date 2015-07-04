using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.server
{
    class Player
    {
        public NetworkPlayer network_player { get; set; }
        public string name { get; set; }
        public Player(NetworkPlayer player)
        {
            network_player = player;
        }

    }
}
