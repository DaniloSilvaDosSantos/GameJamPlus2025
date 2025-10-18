using System.Collections;
using UnityEngine.InputSystem;
using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Declaração de objetos")]
    [SerializeField] private Transform Camera;
    [SerializeField] private Rigidbody RB;
    [SerializeField] private LayerMask Mobs;
    public InputActionAsset InputActions;

    //"Declaração de Inputs"
    private InputAction moveDir;
    private InputAction lookDir;
    private InputAction jumpButton;
    private InputAction attButton;

    private Vector2 LookAngle;
    private Vector2 MoveAngle;

    [Header("Sensibilidade do Olhar")]
    [SerializeField] private float lookSensX = 50f;
    [SerializeField] private float lookSensY = 50f;

    [Header("Variáveis de movimento")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float movementDamping = 2f;

    [Header("Flash da camera")]
    [SerializeField] private Light Flash;
    [SerializeField] private Transform SpotFlash;
    [SerializeField] private float flashPeak = 40f;
    [SerializeField] private float flashDuration = 0.2f;

    [Header("Audios")]
    [SerializeField] private AudioSource Audio;
    [SerializeField] private AudioClip FlashSound;
    [SerializeField] private AudioClip StepSound;


    // Helpers do movimento
    private Vector3 pVelocity = new Vector3(0,0,0);

    public float fireDelay = 3f;

    private float fireCoolDown = 0f;


    void Awake()
    {
        MouseRemoval();
    }

    void Update()
    {
        GatherInputVariables();

        HandleLooking();

        if(attButton.WasPressedThisFrame() && fireCoolDown <= 0f) FireHandler();
    }

    void FixedUpdate()
    {
        GatherFixedInputVariables();

        HandleCooldowns();

        HandleMovement();
    }

    private void OnEnable()
    {
        InputActionMap playerMap = InputActions.FindActionMap("Player");

        moveDir = playerMap.FindAction("Move");
        lookDir = playerMap.FindAction("Look");
        jumpButton = playerMap.FindAction("Jump");
        attButton = playerMap.FindAction("Attack");

        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    void MouseRemoval()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void GatherInputVariables()
    {
        LookAngle = lookDir.ReadValue<Vector2>();
    }

    private void GatherFixedInputVariables()
    {
        MoveAngle = moveDir.ReadValue<Vector2>();
    }

    void HandleLooking()
    {
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y - (-LookAngle.x * (lookSensX/400f)), transform.localEulerAngles.z);

        float headAngle = Camera.localEulerAngles.x -LookAngle.y * (lookSensY/400f);
        
        headAngle = (headAngle > 180) ? headAngle - 360 : headAngle;

        headAngle = Mathf.Clamp(headAngle, -80f, 80f);

        Camera.localRotation = Quaternion.Euler(headAngle, Camera.localEulerAngles.y, Camera.localEulerAngles.z);
    }

    void HandleCooldowns()
    {
        if(fireCoolDown > 0f)
        {
            fireCoolDown -= Time.deltaTime;
        }
        else fireCoolDown = 0f;

        if(Flash.intensity > 0f)
        {
            Flash.intensity -= (Time.deltaTime/flashDuration)*flashPeak;
        }
        else Flash.intensity = 0f;
    }

    void HandleMovement()
    {
        Vector3 localMovementTarget = (MoveAngle.y * transform.forward + MoveAngle.x * transform.right) * movementSpeed;

        RB.linearVelocity = Vector3.SmoothDamp(RB.linearVelocity, localMovementTarget, ref pVelocity, movementDamping);

        if (RB.linearVelocity.magnitude > 0.5f && !Audio.isPlaying) Audio.PlayOneShot(StepSound, 0.7F);
    }

    void FireHandler()
    {
        fireCoolDown = fireDelay;

        Audio.PlayOneShot(FlashSound, 0.7F);

        Collider[] hitColliders = Physics.OverlapSphere(SpotFlash.position, 2.5f, Mobs);
        foreach (var collider in hitColliders)
        {
            Destroy(collider.gameObject);
        }

        Flash.intensity = flashPeak;
    }
}
