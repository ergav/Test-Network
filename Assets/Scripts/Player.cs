using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    
    [SerializeField] private GameObject objectToSpawn;
    
    private NetworkVariable<Vector2> moveInput = new NetworkVariable<Vector2>();

    [SerializeField] private float movementSpeed = 5;
    void Start()
    {
        if (inputReader != null && IsLocalPlayer)
        {
            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += SpawnRPC;
        }
    }

    private void OnMove(Vector2 input)
    {
        MoveRPC(input);
    }
    
    void Update()
    {
        if (IsServer)
        {
            transform.position += new Vector3(moveInput.Value.x * movementSpeed * Time.deltaTime, 0, moveInput.Value.y * movementSpeed * Time.deltaTime);
        }
    }
    
    [Rpc(SendTo.Server)]
    private void SpawnRPC()
    {
        NetworkObject ob = Instantiate(objectToSpawn).GetComponent<NetworkObject>();
        ob.Spawn();
    }
    
    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector2 data)
    {
        moveInput.Value = data;
    }
}