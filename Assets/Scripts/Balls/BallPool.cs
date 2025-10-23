using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
  [SerializeField] private BallBase ballPrefab;
  [SerializeField] private int initialSize = 10;

  private readonly Queue<BallBase> pool = new();

  private void Awake()
  {
    // Pre-instantiate a few balls
    for (int i = 0; i < initialSize; i++)
      AddToPool(CreateNewBall());
  }

  private BallBase CreateNewBall()
  {
    BallBase ball = Instantiate(ballPrefab, transform);
    ball.gameObject.SetActive(false);
    return ball;
  }

  private void AddToPool(BallBase ball)
  {
    ball.gameObject.SetActive(false);
    pool.Enqueue(ball);
  }

  public BallBase Get()
  {
    if (pool.Count == 0)
      AddToPool(CreateNewBall());

    BallBase ball = pool.Dequeue();
    ball.gameObject.SetActive(true);
    return ball;
  }

  public void Return(BallBase ball)
  {
    AddToPool(ball);
  }
}
