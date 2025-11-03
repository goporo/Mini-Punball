using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/ESkillRanged")]
public class ESkillRanged : EnemySkillBehavior
{
  [SerializeField] private GameObject projectilePrefab;
  public override IEnumerator UseSkill(Enemy enemy, BoardState board)
  {
    Debug.Log("Shooting projectile!");
    var player = GameContext.Instance.Player;
    var projectile = Instantiate(projectilePrefab, enemy.Position - Vector3.forward * 0.5f, Quaternion.identity).GetComponent<Projectile>();
    projectile.SetTarget(player, () => player.HealthComponent.TakeDamage(new DamageContext(enemy.Stats.Attack, null)));
    yield break;
  }


}