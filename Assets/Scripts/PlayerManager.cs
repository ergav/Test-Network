using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private Transform player1Spawn, player2Spawn;
    [SerializeField] private List<Player> players = new List<Player>();

    private void SetSpawn(Player player)
    {
        if (players[0] == player)
        {
            player.transform.position = player1Spawn.position;
            player.SetTeamRPC(0);
        }
        else
        {
            player.transform.position = player2Spawn.position; 
            player.SetTeamRPC(1);
        }
    }
    
    public void AddPlayer(Player player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
            SetSpawn(player);
        }
    }

    public void RemovePlayer(Player player)
    {
        players.Remove(player);
    }
}