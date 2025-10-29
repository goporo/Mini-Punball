using System;
using UnityEngine;
using System.Collections;

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

  private void EnableShooting(bool enable)
  {
    shotAllBall = !enable;
    canShoot = enable;
  }

  public IEnumerator StartShooting()
  {
    EnableShooting(true);
    EventBus.Publish(new PlayerCanShootEvent(true));
    yield return new WaitUntil(() => ShotAllBall);
    EnableShooting(false);
  }

  private void OnAllBallsReturned(AllBallReturnedEvent evt)
  {
    if (!canShoot) return;

    shotAllBall = true;
  }




}
