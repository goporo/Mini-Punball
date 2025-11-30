using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
  public GameObject skillSelectionUI;
  public List<SkillRuntime> activeSkills = new();


  private void Awake()
  {
    skillSelectionUI.SetActive(false);
  }

  private void OnEnable()
  {
    EventBus.Subscribe<SkillSelectedEvent>(OnSkillSelected);
    EventBus.Subscribe<PickupBoxEvent>(OnPickupBoxEvent);
  }

  private void OnDisable()
  {
    EventBus.Unsubscribe<SkillSelectedEvent>(OnSkillSelected);
    EventBus.Unsubscribe<PickupBoxEvent>(OnPickupBoxEvent);
  }

  public List<PlayerSkillSO> GetActiveSkills()
  {
    List<PlayerSkillSO> skills = new List<PlayerSkillSO>();
    foreach (var runtime in activeSkills)
    {
      skills.Add(runtime.skill);
    }
    return skills;

  }


  public void AddSkill(PlayerSkillSO skill)
  {
    if (skill == null)
    {
      Debug.LogError("[SkillManager] Attempted to add null skill!");
      return;
    }

    // Check if skill already exists
    var existingSkill = activeSkills.Find(s => s.skill.SkillID == skill.SkillID);

    if (existingSkill != null)
    {
      if (existingSkill.stackCount < skill.maxStacks)
      {
        existingSkill.stackCount++;
      }
      else
      {
        Debug.LogWarning($"[SkillManager] Skill already at max stacks or not stackable: {skill.skillName}");
        return;
      }
    }

    Debug.Log($"[SkillManager] Added skill: {skill.skillName}");

    try
    {
      var runtime = new SkillRuntime(skill);
      void effectAction(IEffectContext ctx)
      {
        // Check all conditions first
        if (!CheckConditions(skill.conditions, ctx))
          return;

        // Execute all effects immediately if conditions pass
        foreach (var effect in skill.effects)
        {
          EffectExecutor.Instance.Execute(ctx, effect);

        }
      }

      foreach (var trigger in skill.triggers)
      {
        var disposable = trigger.Subscribe(runtime, effectAction);
        runtime.subscriptions.Add(disposable);
      }

      activeSkills.Add(runtime);
    }
    catch (System.Exception ex)
    {
      Debug.LogError($"[SkillManager] Failed to add skill {skill.skillName}: {ex.Message}");
    }
  }

  private bool CheckConditions(List<ConditionSO> conditions, IEffectContext ctx)
  {
    if (conditions == null || conditions.Count == 0)
      return true;

    foreach (var condition in conditions)
    {
      if (condition == null)
        continue; // Skip null conditions silently

      if (!condition.Evaluate(ctx))
        return false;
    }
    return true;
  }


  public void ClearSkills()
  {
    foreach (var skill in activeSkills)
      skill.Dispose();
    activeSkills.Clear();
  }

  void OnDestroy()
  {
    ClearSkills();
  }

  private void OnPickupBoxEvent(PickupBoxEvent e)
  {
    skillSelectionUI.SetActive(true);
  }


  private void OnSkillSelected(SkillSelectedEvent e)
  {
    skillSelectionUI.SetActive(false);
    EventBus.Publish(new OnSkillPickedEvent(e.PlayerSkillSO));
    AddSkill(e.PlayerSkillSO);
  }
}