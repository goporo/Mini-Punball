using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(BallBase))]
public class BallPhysics : MonoBehaviour
{
    [Header("Movement Settings")]
    public float constantSpeed = 15f;
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
    private float topLineZ = 5f;
    private float startLineZ = -5f;

    private Vector3 moveDirection;
    private bool isMoving = false;
    private bool isReturnable = false;
    private BallSO ballSO;
    private PlayerRunStats playerRunStats;
    private BallBase ballBase;
    private BoxCollider boxCollider;


    public void Init(PlayerRunStats playerRunStats, BallSO ballSO, Vector3 initialDirection)
    {
        this.playerRunStats = playerRunStats;
        this.ballSO = ballSO;
        SetDirection(initialDirection);
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

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
        moveDirection.y = 0;
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
    void MoveBall()
    {
        float moveDistance = constantSpeed * Time.fixedDeltaTime;

        // Classic brick breaker approach: BoxCast ahead, move, then bounce if hit
        if (Physics.BoxCast(transform.position, BoxSize * 1.0f, moveDirection, out RaycastHit hit, transform.rotation, moveDistance, bounceLayer))
        {
            // Move to just before the hit point
            float safeDistance = Mathf.Max(0, hit.distance - minCollisionDistance);
            transform.position += moveDirection * safeDistance;

            if (hit.collider.CompareTag("Enemy"))
            {
                var target = hit.collider.gameObject.GetComponent<HealthComponent>();

                var ctx = new ResolveHitContext
                {
                    Enemy = target?.GetComponent<Enemy>(),
                    Ball = ballBase
                };
                CombatResolver.Instance.ResolveHit(ctx);

            }

            Vector3 reflection = Vector3.Reflect(moveDirection, hit.normal);
            reflection.y = 0;

            if (reflection.sqrMagnitude > 0.01f)
            {
                moveDirection = reflection.normalized;
            }
        }
        else
        {
            transform.position += moveDirection * moveDistance;
        }
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

    void HandleBallReturned(BallBase ball)
    {
        EventBus.Publish(new BallReturnedEvent(ball));
    }

}
