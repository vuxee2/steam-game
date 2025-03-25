using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Animator anim;
    public CinemachineCamera vcam;

    private CharacterController characterController;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float crouchSpeed = 1f;
    public float jumpPower = 10f;
    public float gravity = 30f;
    public float sensitivity = 2f;
    public float lookXLimit = 75f;

    [Header("Height Settings")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;

    private float speed;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private bool isRunning;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        speed = walkSpeed;
    }

    void Update()
    {
        HandleMovement();
        HandleCrouch();
        ApplyGravity();
        HandleJump();
        UpdateAnimation();
        HandleCameraRotation();
    }

    private void HandleMovement()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        isRunning = Input.GetKey(KeyCode.LeftShift) && characterController.height == defaultHeight;
        speed = Mathf.Lerp(speed, isRunning ? runSpeed : walkSpeed, Time.deltaTime * 10f);

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = (forward * moveZ + right * moveX) * speed;
        moveDirection.x = move.x;
        moveDirection.z = move.z;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight;
            speed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            speed = isRunning ? runSpeed : walkSpeed;
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    private void HandleJump()
    {
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
    }

    private void UpdateAnimation()
    {
        bool isMoving = moveDirection.x != 0 || moveDirection.z != 0;
        anim.SetBool("isWalking", isMoving);
        float walkAnimSpeed = isRunning ? 1.2f : Mathf.Lerp(anim.GetFloat("walkSpeed"), 0.7f * speed / 2f, 5f * Time.deltaTime);
        anim.SetFloat("walkSpeed", walkAnimSpeed);
    }

    private void HandleCameraRotation()
    {
        if (!canMove) return;

        // Rotate camera up and down
        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        vcam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Rotate player around Y axis
        rotationY += Input.GetAxis("Mouse X") * sensitivity;
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
