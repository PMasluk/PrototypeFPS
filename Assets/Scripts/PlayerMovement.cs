using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [Header("Look"), SerializeField]
    private Transform playerCamera = null;
    [SerializeField]
    private float mouseSensitivity = 3.5f;
    [SerializeField]
    private bool lockCursor = true;
    [SerializeField]
    [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [Header("Walk"), SerializeField]
    private CharacterController controller;
    [SerializeField]
    private float walkSpeed = 6.0f;
    [SerializeField]
    private float gravity = -13.0f;
    [SerializeField]
    [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;

    [Header("Jump"), SerializeField]
    private AnimationCurve jumpFallOff;
    [SerializeField]
    private float jumpHigh;

    [Header("Sprint"), SerializeField]
    private float sprintSpeed;

    private float cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    private float currentSpeed;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;
    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private bool isJumping;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        Jump();
        Sprint();
    }

    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    private void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        float timeInAir = 0.0f;
        controller.slopeLimit = 90.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            controller.Move(Vector3.up * jumpForce * jumpHigh * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;

        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

        isJumping = false;
        controller.slopeLimit = 45.0f;
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

}