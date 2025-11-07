using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
  private Queue<IPickupable> pendingPickups = new();
  private bool isWaitingForSkillSelection = false;

  private void OnEnable()
  {
    EventBus.Subscribe<PickupCollectedEvent>(OnPickupCollected);
    EventBus.Subscribe<SkillSelectedEvent>(OnSkillSelected);
    EventBus.Subscribe<PickupBoxEvent>(OnPickupBoxEvent);
  }

  private void OnDisable()
  {
    EventBus.Unsubscribe<PickupCollectedEvent>(OnPickupCollected);
    EventBus.Unsubscribe<SkillSelectedEvent>(OnSkillSelected);
    EventBus.Unsubscribe<PickupBoxEvent>(OnPickupBoxEvent);
  }

  private void OnPickupCollected(PickupCollectedEvent e)
  {
    pendingPickups.Enqueue(e.Pickup);
  }

  private void OnSkillSelected(SkillSelectedEvent e)
  {
    isWaitingForSkillSelection = false;
  }

  private void OnPickupBoxEvent(PickupBoxEvent e)
  {
    isWaitingForSkillSelection = true;
  }

  public IEnumerator ProcessAllPickups()
  {
    // Phase 1: Process all PickupBox pickups first, one by one
    Queue<IPickupable> others = new();
    while (pendingPickups.Count > 0)
    {
      var pickup = pendingPickups.Dequeue();
      if (pickup is PickupBox)
      {
        EventBus.Publish(new PickupBoxEvent());
        yield return new WaitUntil(() => !isWaitingForSkillSelection);
        yield return new WaitForSeconds(0.5f); // Small delay after skill selection
      }
      else
      {
        others.Enqueue(pickup);
      }
      yield return null;
    }

    // Phase 2: Process all other pickups at the same time
    while (others.Count > 0)
    {
      var pickup = others.Dequeue();
      if (pickup is PickupBall)
      {
        EventBus.Publish(new PickupBallEvent());
      }
    }
  }

  private void PullAllPickups()
  {
    while (pendingPickups.Count > 0)
    {
      var pickup = pendingPickups.Dequeue();
      pickup.OnPickup();
    }
  }

  public int PendingPickupCount => pendingPickups.Count;
}