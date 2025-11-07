using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class EnemyUI : MonoBehaviour, IHealthUI
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private HealthComponent healthComponent;

    public void Init(int maxHealth)
    {
        healthText.text = FormatHealthText(maxHealth);
        healthBar.fillAmount = 1f;
    }

    void OnEnable()
    {
        healthComponent.OnHealthChanged += HandleTakeDamage;
    }

    void OnDisable()
    {
        healthComponent.OnHealthChanged -= HandleTakeDamage;
    }

    private void HandleTakeDamage(HealthChangedEvent e)
    {
        OnTakeDamage(e.Current, e.Max);
    }

    public void OnTakeDamage(int currentHealth, int maxHealth)
    {
        healthText.text = FormatHealthText(currentHealth);
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    private string FormatHealthText(int health)
    {
        if (health < 10000)
            return health.ToString();

        string[] suffixes = { "K", "M", "B", "T" };
        double value = health;
        int suffixIndex = -1;

        while (value >= 10000 && suffixIndex < suffixes.Length - 1)
        {
            value /= 1000;
            suffixIndex++;
        }

        if (suffixIndex == -1)
            return health.ToString();

        // Show one decimal if value < 100, else no decimal
        if (value < 100)
            return $"{value:0.0}{suffixes[suffixIndex]}";
        else
            return $"{value:0}{suffixes[suffixIndex]}";
    }



}
