using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIUnit : MonoBehaviour {
    
    public ClassType classType;
    public Animator animator;

    public NavMeshAgent agent;
    [HideInInspector] public float maxRange;
    [HideInInspector] public float idealRange;
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public bool isAttacking = false;

    public float attackAngleOffset = 30f;
    [SerializeField] private RagdollOnOff ragdollOnOff;
    public Transform ragdollHips;

    private Transform childTransform;
    private float ragdollTimer;

    private GameObject instantiatedFlames;
    private Collider instantiatedFlamesCollider;
    private bool ignitedFlames = false;
    private bool dissapearing = false;
    private bool stoppingUnit = false;
    private ReturnToPool returnToPool;


    private void Start() {
        childTransform = transform.GetChild(0);

        Vector2 ranges = ClassTypeManager.Instance.GetClassTypeRanges(classType);
        maxRange = ranges.x;
        idealRange = ranges.y;

        if (GetComponent<ReturnToPool>() != null) {
            returnToPool = GetComponent<ReturnToPool>();
        }
        StartAIUnit();
    }

    public void StartAIUnit() {
        isAlive = true;
        stoppingUnit = false;
        agent.enabled = true;
        
        ragdollTimer = 0;
        ragdollOnOff.RagdollModeOff();

        ignitedFlames = false;
        dissapearing = false;

        AIManager.Instance.units.Add(this);
        StartCoroutine(CheckIfMoving());
    }

    public IEnumerator StopAIUnit() {
        float stopTime = 0f;
        while (stopTime < 2f) {
            yield return null;
            stopTime += Time.deltaTime;
        }
        ragdollOnOff.StopAllSounds();
        ragdollHips.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        AIManager.Instance.units.Remove(this);
        StopCoroutine(CheckIfMoving());
        
        returnToPool.Release();             // release gameobject to object pool
    }

    private void Update() {
        if (isAlive) {
            if (!PlayerHealthManager.Instance.isAlive) {
                SwitchStateIdle();
            }

            // Rotate towards player when stopped
            if (agent.isStopped && classType != ClassType.Healer) {
                Vector3 targetDirection = AIManager.Instance.target.position - transform.position;
                Quaternion spreadAngle = Quaternion.AngleAxis(attackAngleOffset, new Vector3(0, 1, 0));     // Offset look direction
                targetDirection = spreadAngle * targetDirection;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 4f * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else {
                // Fix child transform
                childTransform.localRotation = Quaternion.identity;
                childTransform.localPosition = Vector3.Lerp(childTransform.localPosition, Vector3.zero, 2f * Time.deltaTime);
            }

            if (!agent.isStopped && classType == ClassType.Healer) {
                Vector3 targetDirection = AIManager.Instance.target.position - transform.position;
                Quaternion spreadAngle = Quaternion.AngleAxis(attackAngleOffset, new Vector3(0, 1, 0));     // Offset look direction
                targetDirection = spreadAngle * targetDirection;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 4f * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
        else {
            if (agent.enabled) {
                agent.enabled = false;
                AIManager.Instance.killCount++;
            }
            ragdollTimer += Time.deltaTime;
            if (ragdollTimer > AIManager.Instance.igniteTime && !ignitedFlames) {
                ignitedFlames = true;
                instantiatedFlames = Instantiate(AIManager.Instance.fireVFX, ragdollHips.position, Quaternion.identity);
                instantiatedFlamesCollider = instantiatedFlames.GetComponent<SphereCollider>();
            }
            if (ragdollTimer > AIManager.Instance.igniteTime + AIManager.Instance.flameDuration && !dissapearing) {
                dissapearing = true;
                ragdollOnOff.DissapearThroughGround();
                Destroy(instantiatedFlames, 2f);
                
                if (!stoppingUnit) {
                    stoppingUnit = true;
                    StartCoroutine(StopAIUnit());
                }
            }
            if (instantiatedFlames != null && instantiatedFlamesCollider.enabled) {
                instantiatedFlames.transform.position = ragdollHips.position;
            }
        }
        //if (ragdollHips.position.y < AIManager.Instance.target.position.y - 5f) {
        //    StopAIUnit();
        //}
    }

    private IEnumerator CheckIfMoving() {
        while (isAlive) {
            if (!agent.isStopped && Vector3.Distance(AIManager.Instance.target.position, transform.position) <= idealRange + 4f) {
                SwitchStateAttacking();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void MoveTo(Vector3 position) {
        agent.SetDestination(position);
    }

    public void SwitchStateMoving() {
        isAttacking = false;
        agent.isStopped = false;
        animator.SetBool("attackMelee1", false);
        animator.SetBool("attackMelee2", false);
        animator.SetBool("attackCast", false);
        animator.SetBool("attackBow", false);
        animator.SetBool("attackHeal", false);

        animator.SetBool("isMoving", true);
    }

    public void SwitchStateAttacking() {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("isMoving", false);
        switch (classType) {
            case ClassType.Tank: animator.SetBool("attackMelee1", true); break;
            case ClassType.Healer: animator.SetBool("attackHeal", true); break;
            case ClassType.Mage: animator.SetBool("attackCast", true); break;
            case ClassType.Warlock: animator.SetBool("attackBow", true); break;
            case ClassType.Rogue: animator.SetBool("attackMelee2", true); break;
        }
    }

    public void SwitchStateIdle() {
        isAttacking = false;
        agent.isStopped = true;
        animator.SetBool("attackMelee1", false);
        animator.SetBool("attackMelee2", false);
        animator.SetBool("attackCast", false);
        animator.SetBool("attackBow", false);
        animator.SetBool("attackHeal", false);

        animator.SetBool("isMoving", false);
    }
}