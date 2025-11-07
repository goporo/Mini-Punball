using UnityEngine;
using System.Collections.Generic;

public class UIPool : MonoBehaviour
{
  [SerializeField] private GameObject damagePopupPrefab;
  private Queue<GameObject> pool = new Queue<GameObject>();

  public GameObject GetDamagePopup()
  {
    GameObject popup;
    if (pool.Count > 0)
    {
      popup = pool.Dequeue();
      popup.SetActive(true);
    }
    else
    {
      popup = Instantiate(damagePopupPrefab);
    }
    return popup;
  }

  public void ReturnDamagePopup(GameObject popup)
  {
    popup.SetActive(false);
    pool.Enqueue(popup);
  }
}