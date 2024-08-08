using System;
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
    [SerializeField] private float jumpForce = 5;

    private Rigidbody rb;
    
    void Start()
    {
        if (inputReader != null && IsLocalPlayer)
        {
            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += JumpRPC;
        }

        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(Vector2 input)
    {
        MoveRPC(input);
    }
    
    void Update()
    {
        // if (IsServer)
        // {
        //     transform.position += new Vector3(moveInput.Value.x * movementSpeed * Time.deltaTime, 0, moveInput.Value.y * movementSpeed * Time.deltaTime);
        // }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            rb.velocity = new Vector3(moveInput.Value.x * movementSpeed, rb.velocity.y, moveInput.Value.y * movementSpeed);
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

    [Rpc(SendTo.Server)]
    private void JumpRPC()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}