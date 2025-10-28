using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  private bool shotAllBall = false;
  public bool ShotAllBall => shotAllBall;
  private bool canShoot = false;

  void OnEnable()
  {
    EventBus.Subscribe<AllBallReturnedEvent>(OnAllBallsReturned);
  }

  void OnDisable()
  {
    EventBus.Unsubscribe<AllBallReturnedEvent>(OnAllBallsReturned);
  }
  public void EnableShooting(bool enable)
  {
    shotAllBall = !enable;
    canShoot = enable;
    EventBus.Publish(new PlayerCanShootEvent(enable));

  }

  private void OnAllBallsReturned(AllBallReturnedEvent evt)
  {
    if (!canShoot) return;

    shotAllBall = true;
  }




}
