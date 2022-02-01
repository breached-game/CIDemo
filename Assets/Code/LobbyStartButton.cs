using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStartButton : MonoBehaviour
{
    public void OnLobbyStartButtonPress()
    {
        print("Lobby button has been pressed");
        //PhotonNetwork.LoadLevel("Submarine");
    }
}
