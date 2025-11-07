using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
  [SerializeField] private TMP_Text textDamage;
  private float floatUpSpeed = .5f;
  private float duration = .5f;
  private UIPool pool;

  public void Setup(Vector3 position, string text, Color textColor, UIPool poolRef)
  {
    pool = poolRef;
    textDamage.text = text;
    textDamage.color = textColor;

    float xOffset = Random.Range(-0.5f, 0.5f);
    Vector3 spawnPos = position + new Vector3(xOffset, 0f, 0f);

    transform.localScale = Vector3.one * 0.5f;
    transform.position = spawnPos;

    // Stomp sequence: scale up and move Z, then quickly scale down (stomp), then fade
    Sequence seq = DOTween.Sequence();
    seq.Append(transform.DOScale(1.5f, 0.15f).SetEase(Ease.OutBack))
       .Join(transform.DOMoveZ(transform.position.z + floatUpSpeed, 0.15f).SetEase(Ease.OutQuad)) // move Z during scale up
       .Append(transform.DOScale(1f, 0.12f).SetEase(Ease.InQuad));  // stomp down

    textDamage.DOFade(0f, duration).SetEase(Ease.InQuad)
        .OnComplete(() => pool.ReturnDamagePopup(gameObject));
  }
}