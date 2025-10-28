using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
  private Queue<IPickupable> pendingPickups = new();

  private void OnEnable()
  {
    EventBus.Subscribe<PickupCollectedEvent>(OnPickupCollected);
  }

  private void OnDisable()
  {
    EventBus.Unsubscribe<PickupCollectedEvent>(OnPickupCollected);
  }

  private void OnPickupCollected(PickupCollectedEvent e)
  {
    pendingPickups.Enqueue(e.Pickup);
  }

  public IEnumerator ProcessAllPickups()
  {
    Debug.Log($"Processing {pendingPickups.Count} pickups...");

    while (pendingPickups.Count > 0)
    {
      var pickup = pendingPickups.Dequeue();

      // Apply the pickup effect
      if (pickup is PickupBall pickupBall)
      {
        EventBus.Publish(new PickupBallEvent(1));
        pickupBall.HandleOnDeath();
      }
      yield return null;
    }
  }

  public int PendingPickupCount => pendingPickups.Count;
}