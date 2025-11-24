using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(BallBase))]
public class BallPhysics : MonoBehaviour
{
    [Header("Movement Settings")]
    private float constantSpeed = 10f;
    private float lifeTime = 30f;

    [Header("Bounce Settings")]
    public LayerMask bounceLayer = -1;
    [Tooltip("Minimum distance for a valid collision to prevent self-collision")]
    [HideInInspector] public float minCollisionDistance = 0.01f;

    public Vector3 BoxSize
    {
        get
        {
            if (boxCollider == null) boxCollider = GetComponent<BoxCollider>();
            return boxCollider.size;
        }
    }
    [SerializeField] private GameObject ballVisual;

    private IBallPhysicsBehavior behavior;

    private float topLineZ = 5f;
    private float startLineZ = -5f;

    private Vector3 moveDirection;
    private bool isMoving = false;
    private bool isReturnable = false;
    private PlayerRunStats playerRunStats;
    private BallBase ballBase;
    private BoxCollider boxCollider;


    public void Init(PlayerRunStats playerRunStats, Vector3 initialDirection, Quaternion initialRotation)
    {
        this.playerRunStats = playerRunStats;
        SetDirectionAndRotation(initialDirection, initialRotation);
        behavior = ballBase.Stats.PhysicsBehavior.CreateBehaviorInstance();
        behavior.Init(this, ballBase);
    }
    public void ResetState()
    {
        isMoving = false;
        isReturnable = false;
        moveDirection = Vector3.zero;
    }

    void Awake()
    {
        ballBase = GetComponent<BallBase>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Start()
    {
        StartCoroutine(DelayAutoReturn(lifeTime, ballBase));
    }

    public void SetDirectionAndRotation(Vector3 direction, Quaternion rotation)
    {
        moveDirection = direction.normalized;
        moveDirection.y = 0;
        ballVisual.transform.rotation = rotation;
        // Only rotate the visual, not the main GameObject
        isMoving = true;
        isReturnable = true;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            MoveBall();
        }

        if (isReturnable)
        {
            if (transform.position.z > topLineZ || transform.position.z < startLineZ)
            {
                HandleBallReturned(ballBase);
            }
        }
    }

    public void StandardMove(float deltaTime)
    {
        float moveDistance = constantSpeed * deltaTime;

        transform.position += moveDirection * moveDistance;
    }
    void MoveBall()
    {
        if (behavior == null) return;

        float moveDistance = constantSpeed * Time.fixedDeltaTime;

        // Check for collision ahead
        if (Physics.BoxCast(transform.position, BoxSize * 1.0f, moveDirection,
            out RaycastHit hit, transform.rotation, moveDistance, bounceLayer))
        {
            // Move to just before the hit point
            float safeDistance = Mathf.Max(0, hit.distance - minCollisionDistance);
            transform.position += moveDirection * safeDistance;

            if (hit.collider.CompareTag("Enemy"))
            {
                behavior.OnHitEnemy(hit);
                // After hitting enemy, continue moving the remaining distance
                float remainingDistance = moveDistance - safeDistance;
                if (remainingDistance > 0.001f)
                {
                    transform.position += moveDirection * remainingDistance;
                }
            }
            else
            {
                // Hit a wall or other object, reflect
                behavior.OnHitWall(hit);
            }
        }
        else
        {
            // No collision, just move
            behavior.Move(Time.fixedDeltaTime);
        }
    }

    public void ProcessDamage(RaycastHit hit)
    {
        var hitbox = hit.collider.gameObject.GetComponent<Hitbox>();
        if (hitbox)
        {
            // Dynamically look up attack using ballBase reference
            float ballATK = playerRunStats.Balls.GetBallAttack(ballBase);

            var ctx = DamageContext.CreateBallDamage(
                hitbox.Enemy,
                ballATK,
                hitbox.Type,
                ballBase.Stats.BallType,
                ballBase.Stats.DamageModifiers,
                ballBase.Stats.OnHitEffect,
                ballBase.Stats.StatusEffect.Type,
                ballBase.Stats.DamageType
            );

            hitbox.OnHit(ctx);
        }
    }

    public void Reflect(RaycastHit hit)
    {
        Vector3 reflection = Vector3.Reflect(moveDirection, hit.normal);
        reflection.y = 0;

        if (reflection.sqrMagnitude > 0.01f)
        {
            moveDirection = reflection.normalized;
            ballVisual.transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }
    }

    public void Penetrate(RaycastHit hit)
    {
        // No change to moveDirection, just continue moving forward if hit enemy, but reflect if hit wall
        if (!hit.collider.CompareTag("Enemy"))
        {
            Reflect(hit);
        }
    }

    public Vector3 CalculateReflection(Vector3 normal)
    {
        Vector3 reflection = Vector3.Reflect(moveDirection, normal);
        reflection.y = 0;
        return reflection.normalized;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Pickup"))
        {
            other.GetComponent<IPickupable>()?.OnPickup();
        }
    }

    IEnumerator DelayAutoReturn(float delay, BallBase ball)
    {
        yield return new WaitForSeconds(delay);
        HandleBallReturned(ball);
    }

    public void HandleBallReturned(BallBase ball)
    {
        EventBus.Publish(new BallReturnedEvent(ball));
    }

    public void ForceReturn()
    {
        if (isReturnable)
        {
            var player = LevelContext.Instance.Player;
            Vector3 returnDirection = (player.transform.position - transform.position).normalized;
            SetDirectionAndRotation(returnDirection, Quaternion.LookRotation(returnDirection, Vector3.up));

            HandleBallReturned(ballBase);
        }

    }

}
