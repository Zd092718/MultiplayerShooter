using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform viewPoint;
    public float mouseSensitivity = 1f;
    private float verticalRotationStore;
    private Vector2 mouseInput;


    public bool invertLook;

    public float moveSpeed = 5f, runSpeed = 8f;
    private float activeMoveSpeed;
    private Vector3 moveDirection, movement;

    public CharacterController characterController;

    private Camera cam;

    public float jumpForce = 12f, gravityMod = 2.5f;

    public Transform groundCheckPoint;
    private bool isGrounded;
    public LayerMask groundLayerMask;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

    void Update()
    {
        RotateView();

        MovePlayer();


        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // Provides mouse cursor access when pressing escap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        } else if(Cursor.lockState == CursorLockMode.None)
        {
            // re-enables character control if "escaped" out
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

    }

    void LateUpdate()
    {
        cam.transform.position = viewPoint.position;
        cam.transform.rotation = viewPoint.rotation;
    }

    private void MovePlayer()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runSpeed;
        } else
        {
            activeMoveSpeed = moveSpeed;
        }

        float yVel = movement.y;
        movement = ((transform.forward * moveDirection.z) + (transform.right * moveDirection.x)).normalized * activeMoveSpeed;
        movement.y = yVel;

        if (characterController.isGrounded)
        {
            movement.y = 0f;
        }

        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down,.25f, groundLayerMask);


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            movement.y = jumpForce;
        }

        

        movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;

        characterController.Move(movement * Time.deltaTime);
    }

    private void RotateView()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        verticalRotationStore += mouseInput.y;
        verticalRotationStore = Mathf.Clamp(verticalRotationStore, -60f, 60f);

        if (invertLook)
        {
            viewPoint.rotation = Quaternion.Euler(verticalRotationStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);

        }
        else
        {
            viewPoint.rotation = Quaternion.Euler(-verticalRotationStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
        }
    }

    private void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("We hit: " + hit.collider.gameObject.name);
        }
    }
}
