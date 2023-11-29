using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using System.Collections;

public class Monster : MonoBehaviour
{
    
    //3 states: Patrolling, Moving, Attacking
    //Moving overrides patrolling and attacking.
    
    private Animator myAnimator;
    private NavMeshAgent ai;

    private const int IdleAnims = 2;

    [SerializeField] private float maxHealth, boredTimer = 1;
    private float health;
    private Coroutine stateRoutine, isRotating;
    private EAiState aiState = EAiState.Idle;
    private WaitForSeconds idleTimer;
    
    private enum EAiState
    {
        Idle, //Idles when task complete
        Wander, //Wanders when not used for x seconds
        CommandMove, //Happens when told to move
        CommandAttack //Happens when attack called, can only be cancelled by move
    }
    
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        ai = GetComponent<NavMeshAgent>();
        health = maxHealth;
        idleTimer = new WaitForSeconds(boredTimer);
        EnterIdle();
    }

    private IEnumerator Idle()
    {
        yield return idleTimer; //Wander when complete
        if(aiState == EAiState.Idle)
        {
            bool findLoc;
            RaycastHit hit;
            do
            {
                Vector3 randomSphere = Random.insideUnitCircle * 10;
                randomSphere.z = randomSphere.y;
                randomSphere.y = 1000;
                findLoc = Physics.Raycast(randomSphere, -Vector3.up, out hit, 1000, StaticUtilities. GroundLayerID);
            }
            while(!findLoc);
            MoveToTarget(hit.point);
            aiState = EAiState.Wander;
        }
    }

    private void EnterIdle()
    {
        aiState = EAiState.Idle;
        StartCoroutine(Idle());
    }

    private void Update()
    {
        Vector3 velocity = transform.InverseTransformVector(ai.velocity);
        
        myAnimator.SetFloat(StaticUtilities.XSpeedAnimId, velocity.x);
        myAnimator.SetFloat(StaticUtilities.YSpeedAnimId, velocity.z);
    }

    public void MoveToTarget(Vector3 hitInfoPoint)
    {
        aiState = EAiState.CommandMove;
        ai.SetDestination(hitInfoPoint);
    }

    public void ChangeIdleState()
    {
        int rngIndex = Random.Range(0, 2);
        myAnimator.SetFloat(StaticUtilities.IdleAnimId, rngIndex);
    }

    public void TryAttack(RaycastHit hitObject, Vector3 normalizedHitPoint)
    {
        if(isRotating != null)
        {
            StopCoroutine(isRotating);
        }
        isRotating = StartCoroutine(RotateToTarget(normalizedHitPoint));
    }

    private IEnumerator RotateToTarget(Vector3 normalizedHitPoint)
    {
        float angle;
        do
        {
            angle = Vector3.Dot(transform.right, normalizedHitPoint);
            myAnimator.SetFloat(StaticUtilities.TurnAnimID, angle);
            yield return null;
        } while (Mathf.Abs(angle) >= 0.01f);
        isRotating = null;
    }

    private void Attack()
    {
        myAnimator.SetTrigger(StaticUtilities.AttackAnimID);
    }
}
