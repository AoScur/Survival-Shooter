using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zom : LivingEntity
{
    public LayerMask whatIsTarget;

    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    private AudioSource zomAudioPlayer;
    public AudioClip deathClip;
    public AudioClip hurtClip;

    public ParticleSystem hurtEffect;
    private Animator zomAnimator;
    // private Renderer zomRenderer;

    public float damage = 20f;
    public float timeBetAttack = 0.5f;
    private float lastAttackTime;

    private bool hasTarget
    {
        get
        {
            return targetEntity != null && !targetEntity.dead;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        zomAudioPlayer = GetComponent<AudioSource>();
        zomAnimator = GetComponent<Animator>();
        //zomRenderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        zomAnimator.SetBool("HasTarget", hasTarget);
    }

    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;

                var colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                foreach (var collider in colliders)
                {
                    var entity = collider.GetComponent<LivingEntity>();
                    if (entity != null)
                    {
                        targetEntity = entity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead)
        {
            return;
        }

        hurtEffect.transform.position = hitPoint;
        hurtEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hurtEffect.Play();

        zomAudioPlayer.PlayOneShot(hurtClip);
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        Debug.Log("die function");
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        zomAnimator.SetTrigger("Die");
        zomAudioPlayer.PlayOneShot(deathClip);

        var colliders = GetComponents<Collider>();

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (dead || Time.time - lastAttackTime < timeBetAttack)
        {
            return;
        }

        var attackTarget = other.GetComponent<LivingEntity>();

        if (attackTarget != null && attackTarget == targetEntity)
        {
            lastAttackTime = Time.time;
            var hitPoint = other.ClosestPoint(transform.position);
            var hitNormal = (transform.position - other.transform.position).normalized;

            attackTarget.OnDamage(damage, hitPoint, hitNormal);
        }
    }

    private IEnumerator StartSinking()
    {
        Debug.Log("StartSinking");
        while (transform.position.y > -3)
        {
            transform.Translate(Vector3.down * Time.deltaTime);
            yield return null;
        }
    }
}
