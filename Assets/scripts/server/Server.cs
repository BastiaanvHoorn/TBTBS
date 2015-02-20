using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
        public int disconnect_timeout;
        public Text connected_clients;
        public Text log;
        public InputField server_name_input;
        public InputField port_input;
        public Button start_button;
        private string server_name;
        private List<client> clients = new List<client>();
        private bool started = false;
        #endregion


        private void start_server()
        {
           
            string server_name_text = server_name_input.text;
            int port = (port_input.text != "") ? Convert.ToInt32(port_input.text) : 0;
            if (server_name_text != "" && port > 10000)
            {
                name = server_name_text;
                Network.InitializeServer(players, port, !Network.HavePublicAddress());
                start_button.GetComponentInChildren<Text>().text = "Stop Server";
                print_log("starting server with the name: " + name + " on port: " + port);
                MasterServer.RegisterHost("BassieTBTBS", name, "Still needs a good name");
                toggle_started();
            }
            else
            {
                print_log("Please input a server name and a port that is bigger then 10000", Messagetype.error);
            }
        }
        private void stop_server()
        {
            Network.Disconnect(disconnect_timeout);
            start_button.GetComponentInChildren<Text>().text = "Start Server";
            print_log("Stopped the server");
            server_name_input.interactable = true;
            toggle_started();
        }
        private void toggle_started()
        {
            if (!started)
            {
                server_name_input.interactable = false;
                port_input.interactable = false;
                started = true;
            }
            else
            {
                server_name_input.interactable = true;
                port_input.interactable = true;
                started = false;
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
