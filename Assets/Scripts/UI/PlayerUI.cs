using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textHealth;
    [SerializeField] private Image barHealth;
    [SerializeField] private TMP_Text textBall;


    public void Init(int maxHealth)
    {
        textHealth.text = $"{maxHealth}";
        barHealth.fillAmount = 1f;
        textBall.text = GameManager.Instance.CharacterSO.BaseBallsCount.ToString();
    }

    public void OnBallCountChanged(int currentBallCount)
    {
        textBall.text = $"{currentBallCount}";
    }

    public void OnTakeDamage(int currentHealth, int maxHealth)
    {
        textHealth.text = $"{currentHealth}";
        barHealth.fillAmount = (float)currentHealth / maxHealth;
    }



}
