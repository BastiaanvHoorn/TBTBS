using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.client
{
    class Server : MonoBehaviour
    {
        #region
        enum Messagetype
        {
            error,
            warning,
            information,
        }
        struct client
        {
            public string ip;
        }

        public int players;
        public int port;
        public int disconnect_timeout;
        public UnityEngine.UI.Text connected_clients;
        public UnityEngine.UI.Text log;
        public UnityEngine.UI.InputField server_name_input;
        public UnityEngine.UI.Text server_name;
        public UnityEngine.UI.Text start_button_text;
        private string name;
        private List<client> clients = new List<client>();
        #endregion


        private void start_server()
        {
            if (server_name.text != "")
            {
                name = server_name.text;
                Network.InitializeServer(players, port, !Network.HavePublicAddress());
                start_button_text.text = "Stop Server";
                print_log("starting server with the name: " + name);
                MasterServer.RegisterHost("BassieTBTBS", name, "Still needs a good name");
                server_name_input.interactable = false;
            }
            else
            {
                print_log("Please input a server name", Messagetype.error);
            }
        }

        private void print_log(String message, Messagetype message_type = Messagetype.information)
        {
            switch (message_type)
            {
                case Messagetype.information:
                    log.text += "Info: " + message + "\n";
                    break;
                case Messagetype.error:
                    log.text += "<color=red>Error: " + message + "</color>\n";
                    break;
                case Messagetype.warning:
                    log.text += "<color=#FFFF5C>Warning: " + message + "</color>\n";
                    break;
            }
        }
        private void stop_server()
        {
            Network.Disconnect(disconnect_timeout);
            start_button_text.text = "Start Server";
            print_log("Stopped the server");
            server_name_input.interactable = true;
        }

        //Gui calls
        #region MyRegion
        public void start_stop_server()
        {
            if (!Network.isServer)
            {
                start_server();
            }
            else
            {
                stop_server();
            }
        } 
        #endregion
        //UnityEngine calls
        #region
        void OnGUI()
        {
            connected_clients.text = "";
            foreach (client client in clients)
            {
                connected_clients.text += client.ip + "\n";
            }
            
        }
        void OnServerInitialized()
        {
            print_log("Server initialized and ready");
        }

        void OnPlayerConnected(NetworkPlayer player)
        {
            print_log("Player connected from " + player.ipAddress + ":" + player.port);
            client client = new client();
            client.ip = player.ipAddress;
            clients.Add(client);
        }

        void OnPlayerDisconnected(NetworkPlayer player)
        {
            print_log("Player disconnected from " + player.ipAddress + ":" + player.port);
            int index = clients.FindIndex(_client => _client.ip == player.ipAddress);
            clients.RemoveAt(index);
        }
        #endregion
        
    }
}
