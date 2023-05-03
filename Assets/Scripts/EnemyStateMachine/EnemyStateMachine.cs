using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyStateMachine : MonoBehaviour
{
    // déclare les variables de référence
    private Animator animator;
    private CharacterController characterController;
    
    // variables pour context steering
    [SerializeField] private List<SteeringBehaviour> steeringBehaviours;
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private AIData aiData;
    [SerializeField] private ContextSolver movementDirectionSolver;
    [SerializeField] private SeekBehaviour seekBehaviour;
    private Vector3 appliedMovement;
    [SerializeField] private float runMultiplier = 2.0f;
    [SerializeField] private float rotationFactorPerFrame = 5.0f;
    [SerializeField] private float detectionDelay = 0.05f;

    // variables qui mettent en mémoire setter/getter paramètre IDs
    private int isWalkingHash;
    private int isRunningHash;
    private int isDeadHash;
    private int isAttackingHash;
    private int attackIndexHash;
    private int velocityHash;
    
    // variables pour patrol
    [SerializeField] private float patrolPointsSpawnRadius = 10.0f;
    private List<GameObject> patrolPoints;
    private int patrolPointsLenght;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private LayerMask terrainLayerMask;
    private int currentPoint;
    private float timer;
    [SerializeField] private float waitTime = 2.0f;
    private bool isWaiting;

    // variables pour attaquer
    [SerializeField] private float attackDelay = 1.0f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float maxSpeed = 1.0f;
    
    // variables pour les states
    private EnemyBaseState currentState;
    private EnemyStateFactory states;
    
    // Getter/ Setter
    public EnemyBaseState CurrentState { get => currentState; set => currentState = value; }
    public Animator Animator => animator;
    public CharacterController CharacterController { get => characterController; set => characterController = value; }
    public List<SteeringBehaviour> SteeringBehaviours { get => steeringBehaviours; set => steeringBehaviours = value; }
    public List<Detector> Detectors { get => detectors; set => detectors = value; }
    public AIData AIData { get => aiData; set => aiData = value; }
    public ContextSolver MovementDirectionSolver { get => movementDirectionSolver; set => movementDirectionSolver = value; }
    public SeekBehaviour SeekBehaviour { get => seekBehaviour; set => seekBehaviour = value; }
    public List<GameObject> PatrolPoints { get => patrolPoints; set => patrolPoints = value; }
    public float AppliedMovementX { get => appliedMovement.x; set => appliedMovement.x = value; }
    public float AppliedMovementY { get => appliedMovement.y; set => appliedMovement.y = value; }
    public float AppliedMovementZ { get => appliedMovement.z; set => appliedMovement.z = value; }
    public float RunMultiplier { get => runMultiplier; }
    public float AttackDistance { get => attackDistance; }
    public float AttackDelay { get => attackDelay; }
    public float MaxSpeed { get => maxSpeed; }
    public float WaitTime { get => waitTime; }
    public float Timer { get => timer; set => timer = value; }
    public int CurrentPoint { get => currentPoint; set => currentPoint = value; }
    public int PatrolPointsLenght { get => patrolPointsLenght; }
    public int IsWalkingHash { get => isWalkingHash; }
    public int IsRunningHash { get => isRunningHash; }
    public int IsDeadHash { get => isDeadHash; }
    public int IsAttackingHash { get => isAttackingHash; }
    public int AttackIndexHash { get => attackIndexHash; }
    public bool IsWaiting { get => isWaiting; set => isWaiting = value; }

    private void Awake()
    {
        // set initialement les variables
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        // set les paramètres hash reference
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isDeadHash = Animator.StringToHash("isDead");
        isAttackingHash = Animator.StringToHash("isAttacking");
        attackIndexHash = Animator.StringToHash("attackIndex");
        velocityHash = Animator.StringToHash("velocity");
    }

    private void Start()
    {
        InitializePatrolPoints();

        // setup state
        states = new EnemyStateFactory(this);
        currentState = states.Patrol();
        currentState.EnterState();
        
        InvokeRepeating(nameof(PerformDetection), 0, detectionDelay);
    }

    private void InitializePatrolPoints()
    {
        patrolPointsLenght = new Random().Next(2, 10);
        patrolPoints = new List<GameObject>();
        var empty = new GameObject();
        var origin = characterController.transform.position;
        var incorrectChart = new bool[patrolPointsLenght];

        for (var i = 0; i < patrolPointsLenght; i++)
        {
            var isIncorrect = false;
            var position = origin + UnityEngine.Random.insideUnitSphere * patrolPointsSpawnRadius;
            position.y = 0.1f;
            
            
            patrolPoints.Add(Instantiate(empty, position, Quaternion.identity));
            
            var hit = Physics.OverlapSphere(patrolPoints[i].transform.position, 1.0f, obstacleLayerMask);
            
            if (hit.Length > 0) 
            { 
                isIncorrect = true;
            }

            if (!Physics.Raycast(patrolPoints[i].transform.position, Vector3.down, Mathf.Infinity, terrainLayerMask))
            {
                isIncorrect = true;
            }

            // Met en mémoire les points qui ne sont pas bons
            incorrectChart[i] = isIncorrect;
        }
        
        // Enlève les points qui ne sont pas bons
        for (var i = 0; i < incorrectChart.Length; i++)
        {
            var j = 0;
            if (incorrectChart[i])
            {
                patrolPoints.Remove(patrolPoints[j]);
                --patrolPointsLenght;
            }
            else
            {
                ++j;
            }
        }
        
        Destroy(empty);
    }

    private void HandleAnimation()
    {
        var direction = Mathf.Sqrt(Mathf.Pow(appliedMovement.x,2) + Mathf.Pow(appliedMovement.z,2));
        
        Animator.SetFloat(velocityHash, direction);
    }

    private void PerformDetection()
    {
        foreach (var detector in detectors)
        {
            detector.Detect(aiData);
        }
    }
    
    private void HandleRotation()
    {
        var direction = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
        Vector3 positionToLookAt;
        // change la position où l'agent devrait pointer
        positionToLookAt.x = direction.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = direction.z;
        // la rotation présentement de l'agent
        var currentRotation = transform.rotation;

        if (AppliedMovementX != 0 && AppliedMovementY != 0)
        {
            var targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void Update()
    {
        HandleRotation();
        currentState.UpdateStates();
        HandleAnimation();
        characterController.Move(appliedMovement * Time.deltaTime);
    }

    public void SetTimer()
    {
        timer += Time.deltaTime;
    }

    public int GetRandomIndex()
    {
        return new Random().Next(0, 4);
    }
    
    public void ResetIsAttacking()
    {
        animator.SetBool(isAttackingHash, false);
    }
}
