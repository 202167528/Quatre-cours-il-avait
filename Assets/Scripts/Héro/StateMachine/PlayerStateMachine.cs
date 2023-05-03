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
    private WeaponDataSO weaponData;
    private PotionDataSO potionData;
    private ItemManager itemManager;
    [SerializeField] private LayerMask itemLayerMask;
    
    // variables qui mettent en mémoire setter/getter paramètre IDs
    private int isWalkingHash;
    private int isJumpingHash;
    private int velocityXHash;
    private int velocityZHash;
    private int useHash;
    private int attackIndexHash;
    
    // variables qui mettent en mémoire les valeurs de Character Input
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 appliedMovement;
    private Vector3 rotationTarget;
    private bool isMovementPressed;
    private bool isRunPressed;
    [SerializeField] private float rotationFactorPerFrame = 5.0f;
    [SerializeField] private float rotationRunFactorPerFrame = 10.0f;
    [SerializeField] private float runMultiplier = 5.0f;
    [SerializeField] private float moveMultiplier = 2.5f;
    
    // variables pour la gravité
    private float gravity = Physics.gravity.y;
    private float initialGravity;
    
    // variables pour attaquer
    private bool isUsePressed;
    private bool isAttacking;
    [SerializeField] private float centerPointRadius = 1.0f;
    
    // variables pour interact
    private bool isInteractPressed;
    [SerializeField] private float itemDetectionRadius = 2.0f;
    private Collider[] colliders;
    
    // variables pour aim
    private bool isAimPressed;
    private GameObject target;
    [SerializeField] private float targetDetectionRadius = 5.0f;
    [SerializeField] private LayerMask targetLayerMask;

    // variables pour le saut
    private bool isJumpPressed;
    private float initialJumpVelocity;
    [SerializeField] private float maxJumpHeight = 0.5f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float fallMultiplier = 2.0f;
    private bool requireNewJumpPress;
    
    // variables pour le lancer
    private bool isThrowPressed;
    [SerializeField] private float maxThrowDistance = 5.0f;
    [SerializeField] private float maxThrowUpwardDistance = 2.0f;

    // variables pour les states
    private PlayerBaseState currentState;
    private PlayerStateFactory states;
    
    // Getter/ Setter
    public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }
    public Animator Animator => animator;
    public WeaponDataSO WeaponData { get => weaponData; set => weaponData = value; }
    public PotionDataSO PotionData { get => potionData; set => potionData = value; }
    public ItemManager ItemManager => itemManager;
    public GameObject Target { get => target; set => target = value; }
    public LayerMask ItemLayerMask => itemLayerMask;
    public LayerMask TargetLayerMask => targetLayerMask;
    public CharacterController CharacterController { get => characterController; set => characterController = value; }
    public int IsJumpingHash => isJumpingHash;
    public int IsWalkingHash => isWalkingHash;
    public int UseHash => useHash;
    public int AttackIndexHash => attackIndexHash;
    public bool RequireNewJumpPress { get => requireNewJumpPress; set => requireNewJumpPress = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsJumpPressed => isJumpPressed;
    public bool IsThrowPressed => isThrowPressed;
    public bool IsUsePressed => isUsePressed;
    public bool IsMovementPressed => isMovementPressed;
    public bool IsRunPressed => isRunPressed;
    public bool IsInteractPressed => isInteractPressed;
    public bool IsAimPressed => isAimPressed;
    public float AppliedMovementY { get => appliedMovement.y; set => appliedMovement.y = value; }
    public float InitialJumpVelocity { get => initialJumpVelocity; set => initialJumpVelocity = value; }
    public float CurrentMovementY { get => currentMovement.y; set => currentMovement.y = value; }
    public float Gravity { get => gravity; set => gravity = value; }
    public float FallMultiplier { get => fallMultiplier; set => fallMultiplier = value; }
    public float AppliedMovementX { get => appliedMovement.x; set => appliedMovement.x = value; }
    public float AppliedMovementZ { get => appliedMovement.z; set => appliedMovement.z = value; }
    public float RunMultiplier { get => runMultiplier; }
    public float MoveMultiplier { get => moveMultiplier; }
    public float TargetDetectionRadius { get => targetDetectionRadius; }
    public float ItemDetectionRadius { get => itemDetectionRadius; }
    public float CenterPointRadius { get => centerPointRadius; }
    public float MaxThrowDistance { get => maxThrowDistance; }
    public float MaxThrowUpwardDistance { get => maxThrowUpwardDistance; }
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
        itemManager = GetComponent<ItemManager>();
        
        // setup state
        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

        // set les paramètres hash reference
        isWalkingHash = Animator.StringToHash("isWalking");
        isJumpingHash = Animator.StringToHash("isJumping");
        velocityXHash = Animator.StringToHash("velocityX");
        velocityZHash = Animator.StringToHash("velocityZ");
        attackIndexHash = Animator.StringToHash("attackIndex");
        useHash = Animator.StringToHash("use");
        

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
        characterInput.PlayerInput.Aim.started += OnAim;
        characterInput.PlayerInput.Aim.canceled += OnAim;
        characterInput.PlayerInput.Aim.performed += OnAim;
        characterInput.PlayerInput.Use.started += OnUse;
        characterInput.PlayerInput.Use.canceled += OnUse;
        characterInput.PlayerInput.Throw.started += OnThrow;
        characterInput.PlayerInput.Throw.canceled += OnThrow;
        characterInput.PlayerInput.Throw.performed += OnThrow;

        SetupJumpVariables();
    }
    
    private void HandleRotationWithAim()
    {
        var lookPos = target != null ? target.transform.position - transform.position : transform.forward;
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
        positionToLookAt.x = currentMovementInput.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = currentMovementInput.y;
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
        var direction = appliedMovement.x * transform.forward + appliedMovement.z * transform.right;
        
        Animator.SetFloat(velocityXHash, direction.x);
        Animator.SetFloat(velocityZHash, direction.y);
    }

    private void Update()
    {
        if (isAimPressed)
        {
            HandleRotationWithAim();
        }
        else
        {
            HandleRotation();
        }
        
        HandleRotationAnimation();
        
        currentState.UpdateStates();
        characterController.Move(appliedMovement * Time.deltaTime);
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        isThrowPressed = context.ReadValueAsButton();
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        isUsePressed = context.ReadValueAsButton();
    }
    
    private void OnAim(InputAction.CallbackContext context)
    {
        if (characterController.isGrounded)
            isAimPressed = context.ReadValueAsButton();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (characterController.isGrounded)
            isInteractPressed = context.ReadValueAsButton();
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
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    
    // Active l'animation d'attaque
    public void EnableAttacking()
    {
        isAttacking = true;
    }

    // Désactive l'animation d'attaque
    public void DisableAttacking()
    {
        isAttacking = false;
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
