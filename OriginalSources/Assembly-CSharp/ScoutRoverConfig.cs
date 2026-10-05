// Decompiled with JetBrains decompiler
// Type: ScoutRoverConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class ScoutRoverConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "ScoutRover";
  public const float MASS = 100f;
  private const float WIDTH = 1f;
  private const float HEIGHT = 2f;
  public const int MAXIMUM_TECH_CONSTRUCTION_TIER = 2;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return BaseRoverConfig.BaseRover("ScoutRover", (string) STRINGS.ROBOTS.MODELS.SCOUT.NAME, GameTags.Robots.Models.ScoutRover, (string) STRINGS.ROBOTS.MODELS.SCOUT.DESC, "scout_bot_kanim", 100f, 1f, 2f, TUNING.ROBOTS.SCOUTBOT.CARRY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.DIGGING, TUNING.ROBOTS.SCOUTBOT.CONSTRUCTION, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, TUNING.ROBOTS.SCOUTBOT.HIT_POINTS, TUNING.ROBOTS.SCOUTBOT.BATTERY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE, Db.Get().Amounts.InternalChemicalBattery, false);
  }

  public void OnPrefabInit(GameObject inst)
  {
    BaseRoverConfig.OnPrefabInit(inst, Db.Get().Amounts.InternalChemicalBattery);
  }

  public void OnSpawn(GameObject inst)
  {
    BaseRoverConfig.OnSpawn(inst);
    Effects effects = inst.GetComponent<Effects>();
    if ((UnityEngine.Object) inst.transform.parent == (UnityEngine.Object) null)
    {
      if (effects.HasEffect("ScoutBotCharging"))
        effects.Remove("ScoutBotCharging");
    }
    else if (!effects.HasEffect("ScoutBotCharging"))
      effects.Add("ScoutBotCharging", false);
    inst.Subscribe(856640610, (Action<object>) (_ =>
    {
      if ((UnityEngine.Object) inst.transform.parent == (UnityEngine.Object) null)
      {
        if (!effects.HasEffect("ScoutBotCharging"))
          return;
        effects.Remove("ScoutBotCharging");
      }
      else
      {
        if (effects.HasEffect("ScoutBotCharging"))
          return;
        effects.Add("ScoutBotCharging", false);
      }
    }));
  }
}
