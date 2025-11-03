using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathEffect/Explode")]
public class ExplodeDeath : DeathEffect
{
  [SerializeField] private int radius = 1;
  [SerializeField] private float damageByHealthRatio = 0.5f;
  public override void OnDeath(Enemy enemy, BoardState board)
  {
    Debug.Log($"{enemy.name} exploded!");
    var targets = board.GetSurroundingObjects(enemy.CurrentCell, radius);
    foreach (var obj in targets)
    {
      if (obj == null) continue;
      if (obj.TryGetComponent<IDamageable>(out var target))
      {
        var ctx = new DamageContext
        {
          amount = Mathf.RoundToInt(enemy.Stats.Health * damageByHealthRatio),
        };
        target.TakeDamage(ctx);
      }
    }


  }

}