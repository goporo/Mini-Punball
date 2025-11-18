using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textWaveNumber;
    [SerializeField] private PanelMonsterInfo panelNewMonsterInfo;
    [SerializeField] private PanelNewSkillInfo panelNewSkillInfo;
    [SerializeField] private PanelLevelComplete panelLevelComplete;

    private float panelDisplayDuration = 3f;


    private void Awake()
    {
        textWaveNumber.text = "1";
    }

    void OnEnable()
    {
        EventBus.Subscribe<OnWaveStartEvent>(HandleWaveChange);
        EventBus.Subscribe<OnSkillPickedEvent>(HandleSkillPicked);
        EventBus.Subscribe<LevelCompleteEvent>(HandleLevelComplete);
    }
    void OnDisable()
    {
        EventBus.Unsubscribe<OnWaveStartEvent>(HandleWaveChange);
        EventBus.Unsubscribe<OnSkillPickedEvent>(HandleSkillPicked);
        EventBus.Unsubscribe<LevelCompleteEvent>(HandleLevelComplete);
    }

    private void HandleLevelComplete(LevelCompleteEvent e)
    {
        panelLevelComplete.Setup(e.Result);
    }

    private void HandleWaveChange(OnWaveStartEvent e)
    {
        textWaveNumber.text = e.WaveText;

        int monsterIndex = -1;
        if (e.WaveNumber == 1)
            monsterIndex = 0;
        else if (e.WaveNumber == 6)
            monsterIndex = 1;
        else if (e.WaveNumber == 12)
            monsterIndex = 2;

        if (monsterIndex >= 0 && monsterIndex < e.AvailableEnemies.Length)
        {
            var enemyData = e.AvailableEnemies[monsterIndex].Data;
            panelNewMonsterInfo.Setup(enemyData.Name, enemyData.Description, enemyData.Icon);
            panelNewMonsterInfo.ShowFor(panelDisplayDuration);
        }
    }

    private void HandleSkillPicked(OnSkillPickedEvent e)
    {
        panelNewSkillInfo.Setup(e.SkillData.skillName, e.SkillData.description);
        panelNewSkillInfo.ShowFor(panelDisplayDuration);
    }

    // Panels now manage their own hide timing
}
