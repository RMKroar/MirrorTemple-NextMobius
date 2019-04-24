
// (c) Copyright Cleverous 2016. All rights reserved.

using UnityEngine;
using UnityEngine.Networking;

namespace Isobox
{
    public class scr_networkHUD : MonoBehaviour
    {
        public string IpAddress;
        public string Port;
        public float GuiOffset;
        private bool _started;

        public void Start()
        {
            _started = false;
        }
        public void OnGUI()
        {
            GUILayout.Space(GuiOffset);
            if (!_started)
            {
                if (GUILayout.Button("Host"))
                {
                    _started = true;
                    NetworkManager.singleton.networkPort = int.Parse(Port);
                    NetworkManager.singleton.StartHost();
                }

                GUILayout.Space(0);
                IpAddress = GUILayout.TextField(IpAddress, GUILayout.Width(200));
                Port = GUILayout.TextField(Port, 7);
                if (GUILayout.Button("Connect"))
                {
                    _started = true;
                    NetworkManager.singleton.networkAddress = IpAddress;
                    NetworkManager.singleton.networkPort = int.Parse(Port);
                    NetworkManager.singleton.StartClient();
                }
            }
            else
            {
                if (GUILayout.Button("Disconnect"))
                {
                    _started = false;
                    NetworkManager.singleton.StopHost();
                }
            }
        }
    }
}
