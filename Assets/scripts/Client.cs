using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    class Client : MonoBehaviour
    {
        public string ip;
        public int port;
        public void connect()
        {
            Network.Connect(ip, port);
        }

        void OnConnectedToServer()
        {
            Debug.Log("Connected to server!");
        }
    }
}
