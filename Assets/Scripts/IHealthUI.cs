public interface IHealthUI
{
  void Init(int maxHealth);
  void OnTakeDamage(int currentHealth, int maxHealth);
}