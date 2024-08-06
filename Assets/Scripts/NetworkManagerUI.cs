using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private NetworkManager _networkManager;
    
    private void OnGUI()
    {
        if (GUILayout.Button("Host"))
        {
            _networkManager.StartHost();
        }
        if (GUILayout.Button("Join"))
        {
            _networkManager.StartClient();
        }        
        if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }
    }
}