using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class cameraControl : MonoBehaviour
{
    public Camera camera;

    [Header("Sensivity settings")]
    [Range(0.01f, 100f)] public float camSensivity = 15f;
    [Range(0f, 200f)] public float camBarrelSensivity = 2f;
    public float xRotationLimit = 85f;



    [Header("Movement settings")]
    public float flySpeed = 10f;
    [Min(0f)]public float sprintMultiplier = 2f;
    [Min(0f)]public float acceleration = 10f;



    [Header("Toggles")]
    public bool showCursor = false;
    public bool invertY = false;
    public bool barrelZ = false;
    public bool freeCam = true;


    //Private variables
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float zRotation = 0f;
    private Vector3 currentVelocity = new();
    private const float microMultiplier = 0.02f;
    private const float normalMultiplier = 10f;
    private const float updateCheckTimer = 1f;




    //Initialization
    void Start()
    {
        camSensivity = PlayerPrefs.GetFloat("Sensivity", 15f);
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 currentAngles = transform.eulerAngles;
        xRotation = currentAngles.x;
        yRotation = currentAngles.y;
        zRotation = 0f;
    }

    void Update()
    {
        rotateCamera();
        movementCamera();
    }

    private void HandleSensivityUpdate(PlayerPrefSettings setting, float value)
    {
        if (setting.ToString() == "Sensivity") camSensivity = value;
    }

    void OnEnable()
    {
        SettingsEvents.OnSettingChanged += HandleSensivityUpdate; 
    }

    void OnDisable()
    {
        SettingsEvents.OnSettingChanged -= HandleSensivityUpdate;
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
            zRotation = 0f;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }
    }





    private void movementCamera()
    {
        if (!freeCam || Keyboard.current == null) return;

        float forwardInput = 0f;
        float strafeInput = 0f;
        float verticalInput = 0f;

        if (Keyboard.current.wKey.isPressed) forwardInput += 1f;
        if (Keyboard.current.sKey.isPressed) forwardInput -= 1f;
        if (Keyboard.current.aKey.isPressed) strafeInput -= 1f;
        if (Keyboard.current.dKey.isPressed) strafeInput += 1f;
        if (Keyboard.current.spaceKey.isPressed || Keyboard.current.eKey.isPressed) verticalInput += 1f;
        if (Keyboard.current.ctrlKey.isPressed || Keyboard.current.cKey.isPressed || Keyboard.current.qKey.isPressed) verticalInput -= 1f;

        Vector3 camForward = transform.forward;
        Vector3 camStrafe = transform.right;
        Vector3 worldVert = Vector3.up;

        Vector3 horizDirection = (camForward * forwardInput) + (camStrafe * strafeInput);
        if (horizDirection.sqrMagnitude > 1f) horizDirection.Normalize();

        Vector3 vertDirection = worldVert * verticalInput;

        Vector3 finalDirection = vertDirection + horizDirection;

        float currentFlySpeed = flySpeed;
        if (Keyboard.current.shiftKey.isPressed)
        {
            currentFlySpeed *= sprintMultiplier;
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 70f, 25f * Time.deltaTime);
        }
        else camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 60f, 25f * Time.deltaTime);

        Vector3 targetVelocity = finalDirection * currentFlySpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        transform.position += currentVelocity * Time.deltaTime;
    }
}