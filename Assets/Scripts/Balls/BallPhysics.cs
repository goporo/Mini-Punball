using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class BallPhysics : MonoBehaviour
{
    [Header("Movement Settings")]
    public float constantSpeed = 15f;
    public float lifeTime = 15f;

    [Header("Bounce Settings")]
    public LayerMask bounceLayer = -1;

    public Vector3 BoxSize { get { return boxSize; } }
    private float topLineZ = 5f;
    private float bottomLineZ = -3.7f;
    private Vector3 boxSize = Vector3.one * 0.1f;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool isMoving = false;
    private bool hasPassedStartLine = false;
    private BallSO ballSO;
    private PlayerRunStats playerRunStats;


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
    }

    void Start()
    {
        StartCoroutine(DelayAutoDestroy(lifeTime, GetComponent<BallBase>()));
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
        moveDirection.y = 0;
        isMoving = true;
    }

    void FixedUpdate()
    {
        if (!hasPassedStartLine && transform.position.z > bottomLineZ)
        {
            hasPassedStartLine = true;
        }

        if (hasPassedStartLine)
        {
            if (transform.position.z > topLineZ || transform.position.z < bottomLineZ)
                HandleBallReturned(GetComponent<BallBase>());
        }

        if (isMoving)
        {
            MoveBall();
        }
    }

    void MoveBall()
    {
        float moveDistance = constantSpeed * Time.fixedDeltaTime;
        Vector3 targetPosition = transform.position + moveDirection * moveDistance;


        if (Physics.BoxCast(transform.position, BoxSize * 1.0f, moveDirection, out RaycastHit hit, transform.rotation, moveDistance, bounceLayer))
        {
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

            Vector3 hitPoint = transform.position + moveDirection * hit.distance;
            transform.position = hitPoint;

            Vector3 reflection = Vector3.Reflect(moveDirection, hit.normal);
            reflection.y = 0;
            moveDirection = reflection.normalized;

            float remainingDistance = moveDistance - hit.distance;
            if (remainingDistance > 0)
            {
                Vector3 bounceTarget = transform.position + moveDirection * remainingDistance;
                transform.position = bounceTarget;
            }
        }
        else
        {
            transform.position = targetPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Pickup"))
        {
            other.GetComponent<IPickupable>()?.OnPickup();
        }
    }

    IEnumerator DelayAutoDestroy(float delay, BallBase ball)
    {
        yield return new WaitForSeconds(delay);
        HandleBallReturned(ball);
    }

    void HandleBallReturned(BallBase ball)
    {
        EventBus.Publish(new BallReturnedEvent(ball));
    }

}
