using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class EnemyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthBar;

    public void Init(int maxHealth)
    {
        healthText.text = $"{maxHealth}";
        healthBar.fillAmount = 1f;
    }

    public void OnTakeDamage(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}";
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }



}
