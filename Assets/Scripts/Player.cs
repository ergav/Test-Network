using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    
    [SerializeField] private GameObject objectToSpawn;
    
    [SerializeField] private Material team1Material, team2Material;
    
    private NetworkVariable<Vector2> moveInput = new NetworkVariable<Vector2>();

    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float jumpForce = 5;

    private Rigidbody rb;

    private GameManager gameManager;

    private bool isGrounded;

    private float distToGround;

    private NetworkVariable<byte> teamIndex = new NetworkVariable<byte>();
    
    void Start()
    {
        if (inputReader != null && IsLocalPlayer)
        {
            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += JumpRPC;
        }

        distToGround = GetComponent<Collider>().bounds.extents.y;
        
        rb = GetComponent<Rigidbody>();
    }

    [Rpc(SendTo.Everyone)]
    public void SetTeamRPC(byte newTeamIndex)
    {
        if (newTeamIndex > 1)
        {
            return;
        }
    
        teamIndex.Value = newTeamIndex;
        
        if (newTeamIndex == 0)
        {
            GetComponent<Renderer>().material = team1Material;
        }
        else
        {
            GetComponent<Renderer>().material = team2Material;
        }
    }
    
    public override void OnNetworkSpawn()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        AddToListRPC();
    }

    public override void OnNetworkDespawn()
    {
        RemoveFromListRPC();
    }

    [Rpc(SendTo.Server)]
    private void AddToListRPC()
    {
        gameManager.AddPlayer(this);
    }
    
    [Rpc(SendTo.Server)]
    private void RemoveFromListRPC()
    {
        gameManager.RemovePlayer(this);
    }

    private void OnMove(Vector2 input)
    {
        MoveRPC(input);
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            rb.velocity = new Vector3(moveInput.Value.x * movementSpeed, rb.velocity.y, moveInput.Value.y * movementSpeed);
        }
    }
    
    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector2 data)
    {
        moveInput.Value = data;
    }

    [Rpc(SendTo.Server)]
    private void JumpRPC()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}