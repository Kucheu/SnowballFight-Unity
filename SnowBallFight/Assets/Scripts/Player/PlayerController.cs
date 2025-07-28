using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Collections;

public class PlayerController : NetworkBehaviour
{
    public event Action PlayerTeamChanged;

    [SerializeField]
    private GameObject cameraholder;
    [SerializeField]
    private GameObject snowBallholder;

    [SerializeField] private float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] private NetworkObject snowballPrefab;

    [SerializeField] AudioSource audioSource;


    float verticalLookRoattion;
    public bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody playerRigidbody;
    Animator animator;

    private NetworkVariable<FixedString64Bytes> ownerID = new NetworkVariable<FixedString64Bytes>();
    private NetworkVariable<TeamType> teamType = new NetworkVariable<TeamType>();
    private NetworkVariable<bool> isKilled = new NetworkVariable<bool>();

    public string OwnerID => ownerID.Value.ToString();
    public bool IsKilled => isKilled.Value;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        teamType.OnValueChanged += OnTeamValueChanged;
        PlayerTeamChanged?.Invoke(); //for set after spawn
        if (IsOwner)
        {
            CameraController.Instance.FollowObject(cameraholder.transform);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        teamType.OnValueChanged -= OnTeamValueChanged;
    }

    private void Update()
    {
        if (!IsOwner) return;

        if(true)//Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
            Move();
            Jump();
            Shoot();
        }
        else
        {
            //moveAmount = Vector3.zero;
        }

        if(transform.position.y < -10)
        {
            GetHitRpc();
        }
    }

    public void SetTeam(TeamType teamType)
    {
        this.teamType.Value = teamType;
    }

    public void SetOwnerID(string ownerID)
    {
        this.ownerID.Value = ownerID;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        if(true)//Cursor.lockState == CursorLockMode.Locked)
        {
            playerRigidbody.MovePosition(playerRigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        }
        animator.SetFloat("MoveX", moveAmount.x/sprintSpeed);
        animator.SetFloat("MoveZ", moveAmount.z/sprintSpeed);

    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Shoot");
        }
    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            playerRigidbody.AddForce(transform.up * jumpForce);
        }
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRoattion += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRoattion = Mathf.Clamp(verticalLookRoattion, -89f, 89f);

        cameraholder.transform.localEulerAngles = Vector3.left * verticalLookRoattion; 
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    [Rpc(SendTo.Authority)]
    public void GetHitRpc()
    {
        Debug.LogError("Killed");
        isKilled.Value = true;
        //animator.SetTrigger("DEAD");
    }

    public TeamType GetTeam()
    {
        return teamType.Value;
    }

    public void SendSnowball()
    {
        if(IsOwner)
        {
            SendSnowballRPC(snowBallholder.transform.position, cameraholder.transform.rotation);
        }
    }

    [Rpc(SendTo.Server)]
    private void SendSnowballRPC(Vector3 position, Quaternion rotation)
    {
        var snowballObject = Instantiate(snowballPrefab, position, rotation);
        snowballObject.Spawn(destroyWithScene: true);
        var snowBall = snowballObject.GetComponent<SnowBall>();
        snowBall.SetTeam(teamType.Value);
        snowBall.SetForce();
        snowBall.SetOwner(OwnerID);
    }

    public void OnTeamValueChanged(TeamType previous, TeamType current)
    {
        PlayerTeamChanged?.Invoke();
    }
}
