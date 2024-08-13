using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class NetworkManagerUI : MonoBehaviour
{
    [FormerlySerializedAs("_networkManager")] [SerializeField] private NetworkManager networkManager;
    
    private void OnGUI()
    {
        if (GUILayout.Button("Host"))
        {
            networkManager.StartHost();
        }
        if (GUILayout.Button("Join"))
        {
            networkManager.StartClient();
        }        
        if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }
    }
}