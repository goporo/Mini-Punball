using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
  [SerializeField] LevelMultiplierSO levelMultiplierSO;
  [SerializeField] List<Collectible> availableCollectibles = new();
  private Queue<Collectible> pendingCollects = new();
  private Queue<CollectibleType> pendingCollectTypes = new();

  private bool isWaitingForSkillSelection = false;

  private void OnEnable()
  {
    EventBus.Subscribe<OnCollectibleSpawnEvent>(HandleCollectibleSpawn);
    EventBus.Subscribe<SkillSelectedEvent>(OnSkillSelected);
    EventBus.Subscribe<PickupBoxEvent>(OnPickupBoxEvent);
    EventBus.Subscribe<EnemyDeathEvent>(HandleEnemyDeath);
  }

  private void OnDisable()
  {
    EventBus.Unsubscribe<OnCollectibleSpawnEvent>(HandleCollectibleSpawn);
    EventBus.Unsubscribe<SkillSelectedEvent>(OnSkillSelected);
    EventBus.Unsubscribe<PickupBoxEvent>(OnPickupBoxEvent);
    EventBus.Unsubscribe<EnemyDeathEvent>(HandleEnemyDeath);
  }

  private Collectible GetRandomCollectible()
  {
    if (availableCollectibles.Count == 0) return null;
    int index = Random.Range(0, availableCollectibles.Count);
    return availableCollectibles[index];
  }

  private void HandleEnemyDeath(EnemyDeathEvent e)
  {
    float dropChance = 0.5f;
    float roll = Random.Range(0f, 1f);
    if (roll <= dropChance)
    {
      Collectible collectible = GetRandomCollectible();
      if (collectible != null)
      {
        Vector3 spawnPosition = e.Context.Enemy.transform.position;
        Instantiate(collectible, spawnPosition, Quaternion.identity, transform);
      }
    }
  }

  private void HandleCollectibleSpawn(OnCollectibleSpawnEvent e)
  {
    pendingCollects.Enqueue(e.collectible);
  }

  private void OnSkillSelected(SkillSelectedEvent e)
  {
    isWaitingForSkillSelection = false;
  }

  private void OnPickupBoxEvent(PickupBoxEvent e)
  {
    isWaitingForSkillSelection = true;
  }

  public IEnumerator ForceClearAllCollects()
  {
    foreach (var item in pendingCollects)
    {
      item.ForceDestroy();
    }
    pendingCollects.Clear();
    pendingCollectTypes.Clear();
    yield return null;
  }

  public IEnumerator ProcessAllCollects()
  {
    yield return PullAllCollects();
    pendingCollects.Clear();

    Queue<CollectibleType> boxes = new();

    while (pendingCollectTypes.Count > 0)
    {
      var type = pendingCollectTypes.Dequeue();
      if (type == CollectibleType.Box)
      {
        boxes.Enqueue(type);
      }
      else
      {
        switch (type)
        {
          case CollectibleType.Ball:
            EventBus.Publish(new PickupBallEvent());
            break;
          case CollectibleType.Health:
            int healAmount = LevelContext.Instance.Player.GetModifiedHealAmount(
              levelMultiplierSO.GetWaveHealAmount(LevelContext.Instance.LevelController.CurrentLevel)
            );
            EventBus.Publish(new PickupHealthEvent(healAmount));
            break;
          case CollectibleType.Coin:
            // Handle coin pickup if needed
            break;
        }
      }
      yield return null;
    }

    while (boxes.Count > 0)
    {
      boxes.Dequeue();
      EventBus.Publish(new PickupBoxEvent());
      yield return new WaitUntil(() => !isWaitingForSkillSelection);
      yield return new WaitForSeconds(0.5f);
    }
  }
  private IEnumerator PullAllCollects()
  {
    int count = pendingCollects.Count;
    if (count == 0) yield break;

    int finished = 0;
    foreach (var collectible in pendingCollects)
    {
      StartCoroutine(AnimateAndCount(collectible, () => finished++));
    }

    yield return new WaitUntil(() => finished == count);
  }

  private IEnumerator AnimateAndCount(Collectible collectible, System.Action onFinish)
  {
    yield return collectible.AnimateToPlayer();
    pendingCollectTypes.Enqueue(collectible.Type);
    onFinish?.Invoke();
  }

}