using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    class Server : MonoBehaviour
    {
        public int players;
        public int port; 
        public void start_server()
        {
            Network.InitializeServer(players, port, !Network.HavePublicAddress());
        }

        void OnServerInitialized()
        {
            Debug.Log("Server initialized and ready");
        }

        void OnPlayerConnected(NetworkPlayer player)
        {
            Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
        }
    }
}
