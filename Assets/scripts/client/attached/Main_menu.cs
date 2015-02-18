using UnityEngine;
using System.Collections;

namespace Assets.scripts.client.attached
{
    public class Main_menu : MonoBehaviour
    {
        public int disconnect_timeout;
        HostData[] hostdata;
        public UnityEngine.UI.Text servers;
        public void start_game()
        {
            Application.LoadLevel("world");
        }

        public void connect()
        {
            Network.Connect(hostdata[0]);
        }

        public void disconnect()
        {
            Network.Disconnect(disconnect_timeout);
        }
        public void refresh()
        {
            Debug.Log("requested hostlist");
            MasterServer.RequestHostList("BassieTBTBS");
        }

        void Awake()
        {
            refresh();

        }

        void Start()
        {
            hostdata = MasterServer.PollHostList();
        }
        void Update()
        {
            if (hostdata.Length == 0)
            {
                Debug.Log("searching for a server");
                hostdata = MasterServer.PollHostList();
            }
            foreach(HostData element in hostdata)
            {
                servers.text=(element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit);
            }
        }

    }
}
