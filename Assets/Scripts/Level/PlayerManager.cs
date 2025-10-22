using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  public GameObject playerPrefab;
  [SerializeField] Transform spawnPoint;

  public static event Action OnPlayerShootCompleted;

  public static void RaisePlayerShootCompleted()
  {
    OnPlayerShootCompleted?.Invoke();
  }

  // Called by LevelController to set up the player for a level
  public void InitPlayer()
  {
    Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity, transform);
  }

  public GameObject GetCurrentPlayer()
  {
    return playerPrefab;
  }


}
