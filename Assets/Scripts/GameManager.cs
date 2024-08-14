using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private Transform player1Spawn, player2Spawn;
    [SerializeField] private List<Player> players = new List<Player>();

    private int player1Score, player2Score;

    [SerializeField] private TextMeshProUGUI player1ScoreText, player2ScoreText;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPoint;
    private NetworkObject spawnedBall;

    [SerializeField] private CameraMovement mainCamera;
    
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
            
            if (players.Count == 2)
            {
                SpawnBallRPC();
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    private void SpawnBallRPC()
    {
        spawnedBall = Instantiate(ballPrefab, ballSpawnPoint.position, ballSpawnPoint.rotation).GetComponent<NetworkObject>();
        spawnedBall.Spawn();
        mainCamera.objectToFollow = spawnedBall.transform;
    }
    
    public void RemovePlayer(Player player)
    {
        players.Remove(player);
    }

    [Rpc(SendTo.Everyone)]
    public void ScoreTeam1RPC()
    {
        player1Score++;
        player1ScoreText.text = player1Score.ToString();
    }
    
    [Rpc(SendTo.Everyone)]
    public void ScoreTeam2RPC()
    {
        player2Score++;
        player2ScoreText.text = player2Score.ToString();
    }

    public void ResetPosition()
    {
        Rigidbody rb = spawnedBall.GetComponent<Rigidbody>();
        spawnedBall.transform.position = ballSpawnPoint.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        for (int i = 0; i < players.Count; i++)
        {
            SetSpawn(players[i]);
        }
    }
}