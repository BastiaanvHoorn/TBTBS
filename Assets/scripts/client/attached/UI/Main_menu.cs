using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
namespace Assets.scripts.client.attached.UI
{
    public class Main_menu : MonoBehaviour
    {
        //Variable declaration
        #region
        public int disconnect_timeout;
        HostData[] hostdata;
        public Image servers;
        public Image server_list_item;
        private string client_name;
        private int server_count = 0;
        private List<Image> server_buttons = new List<Image>();
        private bool refreshing;
        private NetworkPlayer server;
        #endregion
        //RPCs
        #region
        #endregion
        //UI calls
        #region
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
            clear_server_list();
            refreshing = true;
            MasterServer.ClearHostList();
            MasterServer.RequestHostList("BassieTBTBS");

        }
        #endregion
        //Unity calls
        #region
        void Awake()
        {
            clear_server_list();
            refresh();

        }
        void OnConnectedToServer()
        {
            GetComponent<NetworkView>().RPC("set_name", RPCMode.Server);
        }
        void Start()
        {
            hostdata = MasterServer.PollHostList();

        }
        void Update()
        {
            if (refreshing)
            {
                hostdata = MasterServer.PollHostList();
                if (hostdata.Length > 0)
                {

                    clear_server_list();
                    foreach (HostData element in hostdata)
                    {
                        add_server_to_list(element);
                        Debug.Log("added a server");
                    }
                    refreshing = false;
                }
            }
        }
        #endregion
        //Private methods
        #region
        private void add_server_to_list(HostData element)
        {
            Image server_item = Instantiate(server_list_item) as Image;
            server_item.transform.parent = servers.transform;
            server_item.rectTransform.anchoredPosition = new Vector2(0, 0);
            server_item.rectTransform.sizeDelta = new Vector2(-6, 30);
            server_item.rectTransform.localPosition = new Vector3(0, 170 - (server_count * 30), 0);
            Text[] text = server_item.GetComponentsInChildren<Text>();
            text[0].text = element.gameName;
            text[1].text = (element.connectedPlayers - 1) + "/" + (element.playerLimit - 1);
            server_item.GetComponent<Button>().onClick.AddListener(() => Network.Connect(element));
            server_buttons.Add(server_item);
            server_count++;
        }

        private void clear_server_list()
        {
            foreach (Image button in server_buttons)
            {
                foreach (Transform child in button.transform)
                {
                    Object.Destroy(child.gameObject);
                }
                Object.Destroy(button.gameObject);
            }
            server_buttons = new List<Image>();
            server_count = 0;
        }
        #endregion

    }
}
