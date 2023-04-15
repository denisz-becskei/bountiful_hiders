using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ObjectMovement : MonoBehaviour
{
    private float moveSpeed = 4f; // The speed the player will move at
    private float jumpForce = 3f; // The force applied to the player when jumping
    private Transform thirdPersonCamera; // The third-person camera transform
    private TransformToObject tto;
    private BountyProgressHandler bph;

    private Rigidbody rb; // The player's Rigidbody component
    private Vector3 moveDirection; // The direction the player should move in
    public bool isGrounded; // Whether or not the player is on the ground
    private bool isRootLocked = false;

    public GameObject rootedIndicator;
    public Sprite rootedUnlocked;
    public Sprite rootedLocked;

    public bool jump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tto = GameObject.FindObjectOfType<TransformToObject>();
        bph = GameObject.FindObjectOfType<BountyProgressHandler>();
        thirdPersonCamera = Camera.main.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRootLocked = !isRootLocked;
            if (isRootLocked)
            {
                SendNotification.Notify("You put yourself into a Root Lock!");
                rootedIndicator.GetComponent<Image>().sprite = rootedLocked;
                if (tto.isBountySet)
                {
                    bph.StartTimer();
                }
            }
            else
            {
                rootedIndicator.GetComponent<Image>().sprite = rootedUnlocked;
                if (bph.timerRunning)
                {
                    bph.StopTimer();
                }
            }
        }

        if (transform.position.y < -1)
        {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
            tto.Retain();
        }
        // Jump if the player is on the ground and the jump button is pressed
        jump = Input.GetKeyDown(KeyCode.Space);
        if (isGrounded && jump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private float groundedTimeThreshold = 0.2f;
    private float groundedTimer;

    void FixedUpdate()
    {
        if (!isRootLocked)
        {
            // Get the input from the player
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            bool slow = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float speedMultiplier = slow ? 0.3f : 1.0f;

            // Calculate the direction to move in based on the camera's forward direction
            Vector3 cameraForward = Vector3.Scale(thirdPersonCamera.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = thirdPersonCamera.right;
            moveDirection = (vertical * cameraForward + horizontal * cameraRight).normalized;

            GetComponentInChildren<Transform>().rotation = new Quaternion(0, Camera.main.transform.rotation.y, 0, Camera.main.transform.rotation.w);

            // Perform a ground check using a spherecast
            bool groundedThisFrame = Physics.CheckSphere(transform.position, 0.2f);

            // Update the grounded timer
            if (groundedThisFrame)
            {
                groundedTimer += Time.fixedDeltaTime;
                if (groundedTimer >= groundedTimeThreshold)
                {
                    isGrounded = true;
                }
            }
            else
            {
                groundedTimer = 0f;
                isGrounded = false;
            }

            // Apply movement to the player's Rigidbody
            rb.MovePosition(rb.position + moveSpeed * speedMultiplier * Time.fixedDeltaTime * moveDirection);
        }
    }
}
