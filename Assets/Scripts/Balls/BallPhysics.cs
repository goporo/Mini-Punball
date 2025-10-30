using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class BallPhysics : MonoBehaviour
{
    [Header("Movement Settings")]
    public float constantSpeed = 15f;
    private float lifeTime = 30f;

    [Header("Bounce Settings")]
    public LayerMask bounceLayer = -1;
    [Tooltip("Minimum distance for a valid collision to prevent self-collision")]
    [HideInInspector] public float minCollisionDistance = 0.01f;
    public Vector3 boxSize = Vector3.one * 0.1f;

    public Vector3 BoxSize { get { return boxSize; } }
    private float topLineZ = 5f;
    private float startLineZ = -4f;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool isMoving = false;
    private bool hasPassedStartLine = false;
    private BallSO ballSO;
    private PlayerRunStats playerRunStats;
    private BallBase ballBase;


    public void Init(PlayerRunStats playerRunStats, BallSO ballSO, Vector3 initialDirection)
    {
        this.playerRunStats = playerRunStats;
        this.ballSO = ballSO;
        SetDirection(initialDirection);
    }
    public void ResetState()
    {
        isMoving = false;
        hasPassedStartLine = false;
        moveDirection = Vector3.zero;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        ballBase = GetComponent<BallBase>();

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
    }

    void FixedUpdate()
    {
        if (!hasPassedStartLine && transform.position.z > startLineZ)
        {
            hasPassedStartLine = true;
        }

        if (hasPassedStartLine)
        {
            if (transform.position.z > topLineZ || transform.position.z < startLineZ)
                HandleBallReturned(ballBase);
        }

        if (isMoving)
        {
            MoveBall();
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

                int finalDamage = ballSO.BaseDamage * playerRunStats.CurrentAttack;
                var context = new DamageContext
                {
                    amount = finalDamage,
                    statusEffect = ballSO.statusEffect
                };

                target?.TakeDamage(context);
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
