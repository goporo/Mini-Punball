using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textWaveNumber;
    [SerializeField] private GameObject panelNewMonsterInfo;
    [SerializeField] private TMP_Text textNewMonsterInfoName;
    [SerializeField] private TMP_Text textNewMonsterInfoDescription;
    [SerializeField] private Image enemyIcon;
    [SerializeField] private GameObject panelNewSkillInfo;
    [SerializeField] private TMP_Text textNewSkillName;
    [SerializeField] private TMP_Text textNewSkillDescription;
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
            textNewMonsterInfoName.text = e.AvailableEnemies[monsterIndex].Data.Name;
            textNewMonsterInfoDescription.text = e.AvailableEnemies[monsterIndex].Data.Description;
            enemyIcon.sprite = e.AvailableEnemies[monsterIndex].Data.Icon;
            panelNewMonsterInfo.SetActive(true);
            StartCoroutine(HideNewMonsterInfoAfterDelay(panelDisplayDuration));
        }
    }

    private void HandleSkillPicked(OnSkillPickedEvent e)
    {
        // Optionally handle skill picked UI updates here
        textNewSkillName.text = e.SkillData.skillName;
        textNewSkillDescription.text = e.SkillData.description;
        panelNewSkillInfo.SetActive(true);
        StartCoroutine(HideNewSkillInfoAfterDelay(panelDisplayDuration));

    }

    private IEnumerator HideNewSkillInfoAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        panelNewSkillInfo.SetActive(false);

    }

    private IEnumerator HideNewMonsterInfoAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        panelNewMonsterInfo.SetActive(false);
    }



}
