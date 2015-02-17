using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.client
{
    class Server : MonoBehaviour
    {
        enum Messagetype
        {
            error,
            warning,
            information,
        }
        public int players;
        public int port;
        public UnityEngine.UI.Text connected_clients;
        public UnityEngine.UI.Text log;
        public UnityEngine.UI.Button start_button;
        public void start_stop_server()
        {
            if(!Network.isServer)
            {
                Network.InitializeServer(players, port, !Network.HavePublicAddress());
                start_button.interactable = false;
            }
        }

        void OnServerInitialized()
        {
            print_log("Server initialized and ready");
        }

        void OnPlayerConnected(NetworkPlayer player)
        {
            print_log("Player connected from " + player.ipAddress + ":" + player.port);
        }

        private void print_log(String message, Messagetype message_type = Messagetype.information)
        {
            switch (message_type)
            {
                case Messagetype.information:
                    log.text += "info:" + message + "\n";
                    break;
                case Messagetype.error:
                    log.text += "<color=red>Info: " + message + "</color>\n";
                    break;
                case Messagetype.warning:
                    log.text += "<color=#FFFF5C>Info: " + message + "</color>\n";
                    break;
            }
        }
    }
}
