using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class BallBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    public float constantSpeed = 15f;
    public float fixedY = 1f;
    public float lifeTime = 15f;

    [Header("Bounce Settings")]
    public LayerMask bounceLayer = -1;

    public Vector3 BoxSize { get { return boxSize; } }
    public static event Action<Vector3> OnBallReturned;
    private float startLineZ = -1.2f;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private Vector3 boxSize = Vector3.one * 0.1f;
    private bool isMoving = false;
    private bool hasPassedStartLine = false;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Start()
    {
        StartCoroutine(DelayAutoDestroy(lifeTime, Vector3.zero));
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

        if (hasPassedStartLine && transform.position.z < startLineZ)
        {
            HandleBallReturned(transform.position);
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

        targetPosition.y = fixedY;

        if (Physics.BoxCast(transform.position, BoxSize * 0.5f, moveDirection, out RaycastHit hit, transform.rotation, moveDistance, bounceLayer))
        {
            Vector3 hitPoint = transform.position + moveDirection * hit.distance;
            hitPoint.y = fixedY;
            transform.position = hitPoint;

            Vector3 reflection = Vector3.Reflect(moveDirection, hit.normal);
            reflection.y = 0;
            moveDirection = reflection.normalized;

            float remainingDistance = moveDistance - hit.distance;
            if (remainingDistance > 0)
            {
                Vector3 bounceTarget = transform.position + moveDirection * remainingDistance;
                bounceTarget.y = fixedY;
                transform.position = bounceTarget;
            }
        }
        else
        {
            transform.position = targetPosition;
        }
    }

    IEnumerator DelayAutoDestroy(float delay, Vector3 ballPos)
    {
        yield return new WaitForSeconds(delay);
        HandleBallReturned(ballPos);
    }

    void HandleBallReturned(Vector3 ballPos)
    {
        OnBallReturned?.Invoke(ballPos);
        GameContext.Instance.CanShoot = true;
        Destroy(gameObject, 0.05f);
    }



    // void OnCollisionEnter(Collision col)
    // {
    //     if (col.gameObject.CompareTag("Monster"))
    //     {
    //         Monster m = col.gameObject.GetComponent<Monster>();
    //         if (m) m.TakeDamage(1);
    //     }
    // }
}
