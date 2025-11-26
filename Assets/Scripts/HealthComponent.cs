using System;
using DG.Tweening;
using UnityEngine;


public struct HealthChangedEvent
{
  public int Current;
  public int Max;

  public HealthChangedEvent(int current, int max)
  {
    Current = current;
    Max = max;
  }
}

public class HealthComponent : MonoBehaviour, IDamageable
{
  [Tooltip("The mesh that will have the bounce animation applied")]
  [SerializeField] private Transform targetMesh;
  private int maxHealth;
  private int currentHealth;
  public event Action<HealthChangedEvent> OnHealthChanged;
  public event Action OnDied;
  private bool isBeingDestroyed = false;
  private bool isDead = false;
  private bool isInvincible = false;
  private int invincibleRoundsLeft = 0;
  private int reviveCount = 0;

  public int CurrentHealth => currentHealth;
  public int MaxHealth => maxHealth;
  [SerializeField] private PlayerVisual playerVisual;


  void OnEnable()
  {
    EventBus.Subscribe<OnWaveStartEvent>(OnWaveStart);
  }

  void OnDisable()
  {
    EventBus.Unsubscribe<OnWaveStartEvent>(OnWaveStart);
  }

  public void Init(int maxHealth)
  {
    this.maxHealth = maxHealth;
    currentHealth = maxHealth;
    isDead = false;
    isBeingDestroyed = false;
    OnHealthChanged?.Invoke(new HealthChangedEvent(currentHealth, maxHealth));
  }

  private void OnWaveStart(OnWaveStartEvent e)
  {
    if (isInvincible)
    {
      invincibleRoundsLeft--;
      if (invincibleRoundsLeft <= 0)
      {
        isInvincible = false;
        invincibleRoundsLeft = 0;
        playerVisual.SetInvincible(false);
      }
    }
  }

  public void ApplyInvincible(int durationRounds)
  {
    isInvincible = true;
    invincibleRoundsLeft = durationRounds;
    playerVisual.SetInvincible(true);

  }

  public void AddReviveCount(int count)
  {
    reviveCount += count;
  }

  public void TryRevive()
  {
    if (reviveCount > 0)
    {
      reviveCount--;
      currentHealth = maxHealth;
      isDead = false;
      isBeingDestroyed = false;
      OnHealthChanged?.Invoke(new HealthChangedEvent(currentHealth, maxHealth));
    }
    else
    {
      EventBus.Publish(new GameLostEvent());
    }
  }

  public void SetMaxHealth(int maxHealth)
  {
    int diff = maxHealth - this.maxHealth;
    currentHealth += diff;
    this.maxHealth = maxHealth;
    if (currentHealth > maxHealth)
    {
      currentHealth = maxHealth;
    }
    OnHealthChanged?.Invoke(new HealthChangedEvent(currentHealth, maxHealth));
  }

  public bool TakeDamage(DamageContext context)
  {
    if (isDead) return true;
    var damage = context.FinalDamage;
    if (isInvincible) damage = 0;
    currentHealth -= damage;

    if (!isBeingDestroyed && damage > 0) AnimationUtility.PlayBounce(targetMesh != null ? targetMesh : transform);
    if (currentHealth <= 0)
    {
      currentHealth = 0;
      HandleDeath();
    }
    OnHealthChanged?.Invoke(new HealthChangedEvent(currentHealth, maxHealth));
    return currentHealth <= 0;
  }

  public bool PlayerTakeDamage(PlayerDamageContext context)
  {
    if (isDead) return true;
    var damage = context.FinalDamage;
    if (isInvincible) damage = 0;
    currentHealth -= damage;

    if (!isBeingDestroyed && damage > 0) AnimationUtility.PlayBounce(targetMesh != null ? targetMesh : transform);
    if (currentHealth <= 0)
    {
      currentHealth = 0;
      HandleDeath();
    }
    OnHealthChanged?.Invoke(new HealthChangedEvent(currentHealth, maxHealth));

    var damageText = "-" + GameUtils.FormatHealthText(damage);
    CombatResolver.SpawnDamagePopup(LevelContext.Instance.Player.Position, damageText);

    return currentHealth <= 0;
  }

  public void Heal(int amount)
  {
    if (isDead) return;

    currentHealth += amount;
    if (currentHealth > maxHealth)
    {
      currentHealth = maxHealth;
    }
    OnHealthChanged?.Invoke(new HealthChangedEvent(currentHealth, maxHealth));

    string healthText = "+" + amount.ToString();
    CombatResolver.SpawnDamagePopup(transform.position + Vector3.up * 1.0f, healthText, DamageType.Heal);
  }

  private void HandleDeath()
  {
    if (isDead) return;
    isDead = true;
    OnDied?.Invoke();
    isBeingDestroyed = true;
  }

  private void OnDestroy()
  {
    (targetMesh != null ? targetMesh : transform).DOKill();
  }


}


