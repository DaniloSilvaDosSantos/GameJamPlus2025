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
    [SerializeField] private LayerMask Default;
    public InputActionAsset InputActions;

    //"Declaração de Inputs"
    private InputAction moveDir;
    private InputAction lookDir;
    private InputAction jumpButton;
    private InputAction attButton;
    private InputAction aimButton;

    private Vector2 LookAngle;
    private Vector2 MoveAngle;

    [Header("Sensibilidade do Olhar")]
    [SerializeField] private float lookSensX = 50f;
    [SerializeField] private float lookSensY = 50f;

    [Header("Feld of View")]
    [SerializeField] private float fovMin = 50f;
    [SerializeField] private float fovMax = 70f;

    [Header("Cam Shake")]
    private Vector3 startCamPos;
    private float shakeAmount = 0f;
    [SerializeField] private float shakeReduct = 100f;
    [SerializeField] private float shakeMax = 100f;


    [Header("Variáveis de movimento")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float movementDamping = 2f;

    [Header("Flash da camera")]
    [SerializeField] private Light Flash;
    [SerializeField] private Transform SpotFlash;
    [SerializeField] private Transform PhysCam;
    [SerializeField] private Camera GraphCam;
    [SerializeField] private float aimState = 0.0f;
    [SerializeField] private float aimDuration = 0.4f;
    [SerializeField] private float flashPeak = 40f;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private Vector3 idleCam = new Vector3(0.029f, -0.26f, 0.241f);
    [SerializeField] private Vector3 activeCam = new Vector3(0.029f, 0.26f, 0.241f);

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

        startCamPos = Camera.localPosition;
    }

    void Update()
    {
        GatherInputVariables();

        HandleLooking();

        CamShake();

        HandleAim();

        if(attButton.WasPressedThisFrame() && aimState > 0.9f && fireCoolDown <= 0f) FireHandler();
    }

    void FixedUpdate()
    {
        GatherFixedInputVariables();

        HandleCooldowns();

        CheckGrounded();

        HandleMovement();
    }

    private void OnEnable()
    {
        InputActionMap playerMap = InputActions.FindActionMap("Player");

        moveDir = playerMap.FindAction("Move");
        lookDir = playerMap.FindAction("Look");
        jumpButton = playerMap.FindAction("Jump");
        attButton = playerMap.FindAction("Attack");
        aimButton = playerMap.FindAction("Aim");

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

    void CheckGrounded()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - transform.up*.3f, 0.3f, Default);
        foreach (var collider in hitColliders)
        {
            if (collider != transform.GetComponent<Collider>())
            {
                RB.linearVelocity += transform.up * -1f * Time.deltaTime;
                return;
            }
        }

        MoveAngle = new Vector2 (0f,0f);
        RB.linearVelocity += transform.up * -10f * Time.deltaTime;
    }


    void HandleLooking()
    {
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y - (-LookAngle.x * (lookSensX/400f)), transform.localEulerAngles.z);

        float headAngle = Camera.localEulerAngles.x -LookAngle.y * (lookSensY/400f);
        
        headAngle = (headAngle > 180) ? headAngle - 360 : headAngle;

        headAngle = Mathf.Clamp(headAngle, -80f, 80f);

        Camera.localRotation = Quaternion.Euler(headAngle, Camera.localEulerAngles.y, Camera.localEulerAngles.z);
    }

    private void CamShake()
    {
        if (shakeAmount > 0f)
        {
            shakeAmount = Mathf.Clamp((shakeAmount - shakeReduct * Time.deltaTime), 0f, shakeMax);
        }

        Camera.localPosition = Vector3.Slerp(startCamPos, Camera.localPosition, 0.9f);

        Camera.localPosition += new Vector3(Random.Range(-0.5f, 0.5f) * (shakeAmount)/300, Random.Range(-0.5f, 0.5f) * (shakeAmount)/300, 0);
    }

    void HandleAim()
    {
        if (aimButton.IsPressed())
        {
            aimState = Mathf.Min(aimState + Time.deltaTime/aimDuration, 1f);
        }
        else aimState = Mathf.Max(aimState - Time.deltaTime/aimDuration, 0f);

        GraphCam.fieldOfView = Mathf.Lerp(fovMax, fovMin, aimState);
        PhysCam.localPosition = Vector3.Lerp(idleCam, activeCam, aimState);
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
        Vector3 localMovementTarget = (MoveAngle.y * transform.forward + MoveAngle.x * transform.right) * movementSpeed / (1f*(aimState) + 1f);

        RB.linearVelocity = Vector3.SmoothDamp(RB.linearVelocity, localMovementTarget, ref pVelocity, movementDamping);

        if (RB.linearVelocity.magnitude > 1f) 
        {
            shakeAmount = RB.linearVelocity.magnitude * 2;
            if (!Audio.isPlaying)
            {
                Audio.pitch = Random.Range(0.8f, 1.5f);
                Audio.PlayOneShot(StepSound, 0.7F);
            }
        }
    }

    void FireHandler()
    {
        fireCoolDown = fireDelay;

        Audio.PlayOneShot(FlashSound, 0.7F);

        Collider[] hitColliders = Physics.OverlapSphere(SpotFlash.position, 6f, Mobs);
        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.GetComponent<RunningDeer>() != null)
            {
                RunningDeer RD = collider.gameObject.GetComponent<RunningDeer>();
                if (RD.startle)
                {
                    RD.willDeerSappear = true;
                    RD.running = true;
                }
            }
        }

        Flash.intensity = flashPeak;
    }
}
