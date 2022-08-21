using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIUnit : MonoBehaviour {
    
    public ClassType classType;
    public Animator animator;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public float maxRange;
    [HideInInspector] public float idealRange;
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public bool isAttacking = false;

    [SerializeField] private float attackAngleOffset = 30f;
    [SerializeField] private RagdollOnOff ragdollOnOff;
    [SerializeField] private Transform ragdollHips;

    private Transform childTransform;
    private float ragdollTimer;

    private GameObject instantiatedFlames;
    private Collider instantiatedFlamesCollider;
    private bool ignitedFlames = false;
    private bool dissapearing = false;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        AIManager.Instance.units.Add(this);

        childTransform = transform.GetChild(0);

        Vector2 ranges = ClassTypeManager.Instance.GetClassTypeRanges(classType);
        maxRange = ranges.x;
        idealRange = ranges.y;

        StartCoroutine(CheckIfMoving());
    }

    private void Update() {
        if (isAlive) {
            if (!PlayerHealthManager.Instance.isAlive) {
                SwitchStateIdle();
            }

            // Rotate towards player when stopped
            if (agent.isStopped) {
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
        }
        else {
            if (agent.enabled) {
                agent.enabled = false;
            }
            ragdollTimer += Time.deltaTime;
            if (ragdollTimer > AIManager.Instance.igniteTime && !ignitedFlames) {
                ignitedFlames = true;
                instantiatedFlames = Instantiate(AIManager.Instance.fireVFX, ragdollHips.position, Quaternion.identity);
                instantiatedFlamesCollider = instantiatedFlames.GetComponent<SphereCollider>();
            }
            if (ragdollTimer > AIManager.Instance.igniteTime + AIManager.Instance.flameDuration && !dissapearing) {
                dissapearing = true;
                StartCoroutine(ragdollOnOff.DissapearThroughGround());
                Destroy(instantiatedFlames, 2f);
            }
            if (instantiatedFlames != null && instantiatedFlamesCollider.enabled) {
                instantiatedFlames.transform.position = ragdollHips.position;
            }
        }
    }

    private IEnumerator CheckIfMoving() {
        while (isAlive) {
            if (!agent.isStopped) {
                if (agent.remainingDistance < 0.5f) {
                    SwitchStateAttacking();
                }
                else if (Vector3.Distance(AIManager.Instance.target.position, transform.position) < idealRange) {
                    SwitchStateAttacking();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void MoveTo(Vector3 position) {
        agent.SetDestination(position);
    }

    public void SwitchStateMoving() {
        isAttacking = false;
        agent.isStopped = false;
        animator.SetBool("attackMelee", false);
        animator.SetBool("attackCast", false);
        animator.SetBool("attackBow", false);
        animator.SetBool("isMoving", true);
    }

    public void SwitchStateAttacking() {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("isMoving", false);
        switch (classType) {
            case ClassType.Tank: animator.SetBool("attackMelee", true); break;
            case ClassType.Healer: break;
            case ClassType.Mage: animator.SetBool("attackCast", true); break;
            case ClassType.Warlock: animator.SetBool("attackBow", true); break;
            case ClassType.Rogue: animator.SetBool("attackMelee", true); break;
        }
    }

    public void SwitchStateIdle() {
        isAttacking = false;
        agent.isStopped = true;
        animator.SetBool("attackMelee", false);
        animator.SetBool("attackCast", false);
        animator.SetBool("attackBow", false);
        animator.SetBool("isMoving", false);
    }
}