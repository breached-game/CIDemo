using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LauncherUI : MonoBehaviour
{
    private NetworkManager manager;
    public GameObject status;
    public GameObject launcherUI;
    public GameObject roomManager;

    // Start is called before the first frame update
    void Start()
    {
        manager = roomManager.GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            launcherUI.SetActive(true);
        }
        else
        {
            StatusLabels();
        }

        /* I dont know what this does
        // client ready
        if (NetworkClient.isConnected && !NetworkClient.ready)
        {
            if (GUILayout.Button("Client Ready"))
            {
                NetworkClient.Ready();
                if (NetworkClient.localPlayer == null)
                {
                    NetworkClient.AddPlayer();
                }
            }
        }
        */
    }

    public void StartButtonHostClient()
    {
        if (!NetworkClient.active)
        {
            manager.StartHost();
        }
        else print("Trying to connect when already connected");
    }

    public void StartButtonClient()
    {
        if (!NetworkClient.active)
        {
            manager.StartClient();
        }
        else print("Trying to connect when already connected");
    }
    public void StartButtonServer()
    {
        if (!NetworkClient.active)
        {
            manager.StartServer();
        }
        else print("Trying to connect when already connected");
    }


    void StatusLabels()
    {
        // host mode
        // display separately because this always confused people:
        //   Server: ...
        //   Client: ...
        if (NetworkServer.active && NetworkClient.active)
        {
            status.GetComponent<Text>().text = "Host: running via " + Transport.activeTransport;
        }
        // server only
        else if (NetworkServer.active)
        {
            status.GetComponent<Text>().text = "Server: running via " + Transport.activeTransport;
        }
        // client only
        else if (NetworkClient.isConnected)
        {
            status.GetComponent<Text>().text = "Client: running via " + Transport.activeTransport + " via " + Transport.activeTransport;
        }
    }

    public void StopButtons()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
                manager.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
                manager.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
                manager.StopServer();
        }
    }
}
