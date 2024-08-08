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

    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject chatBox;

    private void Start()
    {
        if (inputReader != null)
        {
            inputReader.ChatEvent += ChatRPC;
        }
    }

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

    public void ReadStringInput(string s)
    {
        SubmitMessageRPC(s);
    }
    
    [Rpc(SendTo.Me)]
    private void ChatRPC()
    {
        chatBox.SetActive(true);
    }
}