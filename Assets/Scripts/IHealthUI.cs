public interface IHealthUI
{
  void Init(int maxHealth);
  void UpdateHealth(int currentHealth, int maxHealth);
}