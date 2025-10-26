using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textWaveNumber;

    private void Awake()
    {
        textWaveNumber.text = "1";
    }

    void OnEnable()
    {
        WaveController.OnWaveChange += HandleWaveChange;
    }
    void OnDisable()
    {
        WaveController.OnWaveChange -= HandleWaveChange;
    }

    public void HandleWaveChange(int newWave)
    {
        textWaveNumber.text = $"{newWave}";
    }



}
