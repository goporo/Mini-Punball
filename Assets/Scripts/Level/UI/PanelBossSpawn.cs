using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class PanelBossSpawn : CanvasGroupUIBase
{
  [SerializeField] private TMP_Text textBossName;
  [SerializeField] private TMP_Text textBossDescription;
  [SerializeField] private Image bossIcon;

  protected override void Awake()
  {
    base.Awake();
    Hide();
  }

  private void OnEnable()
  {
    EventBus.Subscribe<OnBossWaveStartEvent>(HandleOnBossWaveStart);
  }

  void OnDisable()
  {
    EventBus.Unsubscribe<OnBossWaveStartEvent>(HandleOnBossWaveStart);
  }

  private void HandleOnBossWaveStart(OnBossWaveStartEvent e)
  {
    SetupBossInfo(e.BossEnemy);
    StartCoroutine(ShowAndHide());
  }

  private void SetupBossInfo(Enemy bossInfo)
  {
    textBossName.text = bossInfo.Data.Name;
    textBossDescription.text = bossInfo.Data.Description;
    bossIcon.sprite = bossInfo.Data.Icon;
  }

  private IEnumerator ShowAndHide()
  {
    float waitTime = 2.25f;
    RectTransform rt = (RectTransform)transform;
    Vector2 hiddenPos = new Vector2(rt.anchoredPosition.x, 400); // Off-screen (adjust as needed)
    Vector2 shownPos = new Vector2(rt.anchoredPosition.x, 0);    // Center

    rt.anchoredPosition = hiddenPos;
    Show();
    rt.DOAnchorPos(shownPos, 0.4f).SetEase(Ease.OutBack);

    yield return new WaitForSeconds(waitTime);

    rt.DOAnchorPos(hiddenPos, 0.4f).SetEase(Ease.InBack);
    yield return new WaitForSeconds(0.4f);
    Hide();
  }


}