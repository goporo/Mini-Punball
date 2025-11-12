using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class EnemyUI : MonoBehaviour, IHealthUI
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private HealthComponent healthComponent;

    public void Init(int maxHealth)
    {
        healthText.text = GameUtils.FormatHealthText(maxHealth);
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
        UpdateHealth(e.Current, e.Max);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthText.text = GameUtils.FormatHealthText(currentHealth);
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }





}
