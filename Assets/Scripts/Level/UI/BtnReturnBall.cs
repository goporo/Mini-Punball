using UnityEngine;

public class BtnReturnBall : CanvasGroupUIBase
{
  protected override void Awake()
  {
    base.Awake();
    Hide();
  }

  private void OnEnable()
  {
    EventBus.Subscribe<AllBallReturnedEvent>(HandleAllBallReturned);
    EventBus.Subscribe<BallStuckEvent>(HandleBallStuck);
  }

  private void OnDisable()
  {
    EventBus.Unsubscribe<AllBallReturnedEvent>(HandleAllBallReturned);
    EventBus.Unsubscribe<BallStuckEvent>(HandleBallStuck);
  }

  private void HandleBallStuck(BallStuckEvent e)
  {
    Show();
  }

  private void HandleAllBallReturned(AllBallReturnedEvent e)
  {
    Hide();
  }
}
