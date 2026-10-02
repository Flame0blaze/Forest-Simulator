using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class cameraControl : MonoBehaviour
{

    [Header("Sensivity settings")]
    [Range(1, 100)] public float camSensivity = 15;
    [Range(0, 100)] public float camBarrelSensivity = 2;
    [Range(0, 90)] public float xRotationLimit = 85;



    [Header("Movement settings")]
    public float flySpeed = 10;
    [Min(0)]public int sprintMultiplier = 2;
    [Min(0)]public float acceleration = 10;



    [Header("Toggles")]
    public bool showCursor = false;
    public bool invertY = false;
    public bool barrelZ = false;
    public bool freeCam = true;


    //Private variables
    private float xRotation = 0;
    private float yRotation = 0;
    private float zRotation = 0;
    private Vector3 currentVelocity = new();
    private const float microMultiplier = 0.02f;
    private const float normalMultiplier = 10;




    //Initialization
    void Start()
    {
        camSensivity = PlayerPrefs.GetFloat("Sensivity", 15);
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 currentAngles = transform.eulerAngles;
        xRotation = currentAngles.x;
        yRotation = currentAngles.y;
        zRotation = 0;
    }
    void Update()
    {
        rotateCamera();
        movementCamera();
    }




    //Functions
    private void rotateCamera()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        if (Keyboard.current.altKey.wasPressedThisFrame)
        {
            showCursor = !showCursor;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) //Check if cursor is above a ui element
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        
        

        if (!showCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            float mouseX = mouseDelta.x * microMultiplier; // Using a microMultiplier because we work with mouse movement in pixels. Pixels are independent of the framerate, 
            float mouseY = mouseDelta.y * microMultiplier; // so you will need another tiny multiplier for that

            yRotation += mouseX * camSensivity;
            xRotation += mouseY * (invertY ? 1 : -1) * camSensivity;
            float limit = xRotationLimit;
            xRotation = Mathf.Clamp(xRotation, -limit, limit);

            if (barrelZ)
            {
                if (Keyboard.current.zKey.isPressed) zRotation -= camBarrelSensivity * normalMultiplier * Time.deltaTime;
                if (Keyboard.current.xKey.isPressed) zRotation += camBarrelSensivity * normalMultiplier * Time.deltaTime;
            }

            transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }
        else if (showCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            zRotation = 0;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }
    }





    private void movementCamera()
    {
        if (!freeCam || Keyboard.current == null) return;

        float forwardInput = 0;
        float strafeInput = 0;
        float verticalInput = 0;

        if (Keyboard.current.wKey.isPressed) forwardInput += 1;
        if (Keyboard.current.sKey.isPressed) forwardInput -= 1;
        if (Keyboard.current.aKey.isPressed) strafeInput -= 1;
        if (Keyboard.current.dKey.isPressed) strafeInput += 1;
        if (Keyboard.current.spaceKey.isPressed || Keyboard.current.eKey.isPressed) verticalInput += 1;
        if (Keyboard.current.ctrlKey.isPressed || Keyboard.current.cKey.isPressed || Keyboard.current.qKey.isPressed) verticalInput -= 1;

        Vector3 camForward = transform.forward;
        Vector3 camStrafe = transform.right;
        Vector3 camVert = transform.up;

        Vector3 direction = (camForward * forwardInput) + (camStrafe * strafeInput) + (camVert * verticalInput);

        if (direction.sqrMagnitude > 1) direction.Normalize();

        float currentFlySpeed = flySpeed;
        if (Keyboard.current.shiftKey.isPressed) currentFlySpeed *= sprintMultiplier;

        Vector3 targetVelocity = direction * currentFlySpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        transform.position += currentVelocity * Time.deltaTime;
    }
}