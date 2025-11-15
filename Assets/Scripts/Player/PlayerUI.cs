using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PlayerUI : MonoBehaviour, IHealthUI
{
    [SerializeField] private TMP_Text textHealth;
    [SerializeField] private Image barHealth;
    [SerializeField] private TMP_Text textBall;
    [SerializeField] private GameObject textBallContainer;
    [SerializeField] private HealthComponent healthComponent;



    void OnEnable()
    {
        EventBus.Subscribe<BallCountChangedEvent>(OnBallCountChanged);
        EventBus.Subscribe<AllBallShotEvent>(HandleAllBallShot);
        healthComponent.OnHealthChanged += HandleHealthChange;
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<BallCountChangedEvent>(OnBallCountChanged);
        EventBus.Unsubscribe<AllBallShotEvent>(HandleAllBallShot);
        healthComponent.OnHealthChanged -= HandleHealthChange;
    }

    private void HandleAllBallShot(AllBallShotEvent e)
    {
        EnableTextBall(false);
    }

    public void EnableTextBall(bool enable)
    {
        textBallContainer.SetActive(enable);
    }

    public void Init(int maxHealth)
    {
        textHealth.text = $"{maxHealth}";
        barHealth.fillAmount = 1f;
        textBall.text = GlobalContext.Instance.CharacterSO.BaseBallsCount.ToString();
    }

    public void OnBallCountChanged(BallCountChangedEvent e)
    {
        int currentBallCount = e.CurrentBallCount;
        UpdateBallCount(currentBallCount);
    }

    public void UpdateBallCount(int count)
    {
        textBall.text = $"{count}";
    }

    private void HandleHealthChange(HealthChangedEvent e)
    {
        UpdateHealth(e.Current, e.Max);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        textHealth.text = $"{currentHealth}";
        barHealth.fillAmount = (float)currentHealth / maxHealth;
    }





}
