using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    // déclare les variables de référence
    private CharacterInput characterInput;
    private CharacterController characterController;
    private Animator animator;
    
    // variables qui mettent en mémoire setter/getter paramètre IDs
    private int isWalkingHash;
    private int isRunningHash;
    private int isJumpingHash;
    private int isFallingHash;
    private int isInteractingHash;
    private int angleHash;
    
    // variables qui mettent en mémoire les valeurs de Character Input
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private Vector3 appliedMovement;
    private Vector2 mouseLook;
    private Vector3 rotationTarget;
    private bool isMovementPressed;
    private bool isRunPressed;
    [SerializeField] private float rotationFactorPerFrame = 5.0f;
    [SerializeField] private float rotationRunFactorPerFrame = 15.0f;
    [SerializeField] private float runMultiplier = 2.0f;
    
    // variables pour la gravité
    private float gravity = Physics.gravity.y;
    private float initialGravity;
    
    // variables pour interact
    private bool isInteractPressed;
    private bool isInteracting;
    
    // variables pour le saut
    private bool isJumpPressed;
    private float initialJumpVelocity;
    [SerializeField] private float maxJumpHeight = 1.0f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float fallMultiplier = 2.0f;
    private bool requireNewJumpPress;
    private bool isJumping;
    private bool isJumpingAnimating;

    // variables pour les states
    private PlayerBaseState currentState;
    private PlayerStateFactory states;
    
    // Getter/ Setter
    public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }
    public Animator Animator => animator;
    public CharacterController CharacterController { get => characterController; set => characterController = value; }
    public int IsJumpingHash => isJumpingHash;
    public int IsWalkingHash => isWalkingHash;
    public int IsRunningHash => isRunningHash;
    public int IsFallingHash => isFallingHash;
    public int IsInteractingHash => isInteractingHash;
    public bool RequireNewJumpPress { get => requireNewJumpPress; set => requireNewJumpPress = value; }
    public bool IsJumpingAnimating { set => isJumpingAnimating = value; }
    public bool IsJumping { set => isJumping = value; }
    public bool IsInteracting { get => isInteracting; set => isInteracting = value; }
    public bool IsJumpPressed => isJumpPressed;
    public bool IsMovementPressed => isMovementPressed;
    public bool IsRunPressed => isRunPressed;
    public bool IsInteractPressed => isInteractPressed;
    //public float CurrentMovement { get => currentMovement.y; set => currentMovement.y = value; }
    public float AppliedMovementY { get => appliedMovement.y; set => appliedMovement.y = value; }
    public float InitialJumpVelocity { get => initialJumpVelocity; set => initialJumpVelocity = value; }
    public float CurrentMovementY { get => currentMovement.y; set => currentMovement.y = value; }
    public float Gravity { get => gravity; set => gravity = value; }
    public float FallMultiplier { get => fallMultiplier; set => fallMultiplier = value; }
    public float AppliedMovementX { get => appliedMovement.x; set => appliedMovement.x = value; }
    public float AppliedMovementZ { get => appliedMovement.z; set => appliedMovement.z = value; }
    public float RunMultiplier { get => runMultiplier; }
    public Vector2 CurrentMovementInput { get => currentMovementInput; }

    private void SetupJumpVariables()
    {
        var timeToApex = maxJumpTime / 2;
        initialGravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
    
    void Awake()
    {
        // set initialement les variables
        characterInput = new CharacterInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        // setup state
        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

        // set les paramètres hash reference
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isFallingHash = Animator.StringToHash("isFalling");
        isInteractingHash = Animator.StringToHash("isInteracting");
        angleHash = Animator.StringToHash("Angle");
        

        // set les callbacks du characterInput
        characterInput.PlayerInput.Movement.started += OnMovementInput;
        characterInput.PlayerInput.Movement.canceled += OnMovementInput;
        characterInput.PlayerInput.Movement.performed += OnMovementInput;
        characterInput.PlayerInput.Run.started += OnRun;
        characterInput.PlayerInput.Run.canceled += OnRun;
        characterInput.PlayerInput.Jump.started += OnJump;
        characterInput.PlayerInput.Jump.canceled += OnJump;
        characterInput.PlayerInput.Interact.started += OnInteract;
        characterInput.PlayerInput.Interact.canceled += OnInteract;
        characterInput.PlayerInput.MouseLook.started += OnLook;
        characterInput.PlayerInput.MouseLook.canceled += OnLook;
        characterInput.PlayerInput.MouseLook.performed += OnLook;
        
        SetupJumpVariables();
    }
    
    private void HandleRotationWithAim()
    {
        var ray = Camera.main.ScreenPointToRay(mouseLook);

        if (Physics.Raycast(ray, out var raycastHit))
            rotationTarget = raycastHit.point;
        
        var lookPos = rotationTarget - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);

        var aimDirection = new Vector3(rotationTarget.x, 0, rotationTarget.z);

        if (aimDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationFactorPerFrame * Time.deltaTime);
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        // change la position où le joueur devrait pointer
        positionToLookAt.x = appliedMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = appliedMovement.z;
        // la rotation présentement du joueur
        var currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            var targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationRunFactorPerFrame * Time.deltaTime);
        }
    }

    private void HandleRotationAnimation()
    {
        var movementInput = currentMovementInput.magnitude == 0 ? transform.forward : new Vector3(currentMovementInput.x, 0, currentMovementInput.y);
        var directionForward = Vector3.Dot(transform.forward, movementInput);
        //Debug.DrawRay(transform.position, transform.forward, Color.red);
        //Debug.DrawRay(transform.position, movementInput, Color.blue);

        var angleSign = Vector3.Cross(movementInput, transform.forward).y < 0 ? 1 : -1;
        var angle = angleSign * 360 * Mathf.Acos(directionForward / (transform.forward.magnitude * movementInput.magnitude)) / (2 * Mathf.PI);
        angle = Mathf.Clamp(angle, -180.0f, 180.0f);
        
        animator.SetFloat(angleHash, angle);
    }

    private void Update()
    {
        HandleRotationWithAim();
        //HandleRotation();
        HandleRotationAnimation();
        currentState.UpdateStates();
        characterController.Move(appliedMovement * Time.deltaTime);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        isInteractPressed = context.ReadValueAsButton();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }
    
    private void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }
    
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        if (characterController.isGrounded){}
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    
    private void OnEnable()
    {
        // active le player Input action map
        characterInput.PlayerInput.Enable();
    }

    private void OnDisable()
    {
        // désactive le player Input action map
        characterInput.PlayerInput.Disable();
    }
}
