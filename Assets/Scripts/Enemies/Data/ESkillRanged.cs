using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/ESkillRanged")]
public class ESkillRanged : EnemySkillBehavior
{
  [SerializeField] private GameObject projectilePrefab;
  public override IEnumerator UseSkill(Enemy enemy, BoardState board)
  {
    var player = LevelContext.Instance.Player;
    var projectile = Instantiate(projectilePrefab, enemy.Position - Vector3.forward * 0.5f, Quaternion.identity).GetComponent<Projectile>();
    projectile.SetTarget(player, (System.Action)(() =>
    {
      CombatResolver.Instance.PlayerTakeDamage(new PlayerDamageContext { FinalDamage = enemy.WaveStats.Attack });
    }));
    yield break;
  }


}