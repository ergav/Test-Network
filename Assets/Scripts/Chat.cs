using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Chat : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public override void OnNetworkSpawn()
    {
        SubmitMessageRPC("Hello there");
    }
    
    [Rpc(SendTo.Server)]
    public void SubmitMessageRPC(FixedString128Bytes message)
    {
        UpdateMessageRPC(message);
    }
    
    [Rpc(SendTo.Everyone)]
    public void UpdateMessageRPC(FixedString128Bytes message)
    {
        text.text = message.ToString();
        Debug.Log(message.ToString());
    }
}