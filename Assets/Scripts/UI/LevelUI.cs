using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;

    public void Init(int maxHealth)
    {
        waveText.text = "1";
    }

    public void OnWaveChange(int currentWave, int maxWaves)
    {
        waveText.text = $"{currentWave}";
    }



}
