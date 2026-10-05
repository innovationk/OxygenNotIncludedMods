// Decompiled with JetBrains decompiler
// Type: LargeImpactorVanillaConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class LargeImpactorVanillaConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "LargeImpactorVanilla";
  public static string NAME = "LargestPotaytoeVanilla";

  public string[] GetRequiredDlcIds()
  {
    return new string[2]{ "", "DLC4_ID" };
  }

  public string[] GetForbiddenDlcIds() => (string[]) null;

  GameObject IEntityConfig.CreatePrefab()
  {
    return LargeImpactorVanillaConfig.ConfigCommon(LargeImpactorVanillaConfig.ID, LargeImpactorVanillaConfig.NAME);
  }

  public static GameObject ConfigCommon(string id, string name)
  {
    GameObject entity = EntityTemplates.CreateEntity(id, name);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<StateMachineController>();
    entity.AddOrGet<Notifier>();
    entity.AddOrGet<LoopingSounds>();
    LargeImpactorStatus.Def def = entity.AddOrGetDef<LargeImpactorStatus.Def>();
    def.MAX_HEALTH = 1000;
    def.EventID = "LargeImpactor";
    entity.AddOrGet<LargeImpactorVisualizer>();
    entity.AddOrGet<LargeImpactorCrashStamp>().largeStampTemplate = "dlc4::poi/asteroid_impacts/potato_large";
    entity.AddOrGetDef<LargeImpactorNotificationMonitor.Def>();
    entity.AddOrGet<ParallaxBackgroundObject>().Initialize("Demolior_final_whole");
    return entity;
  }

  void IEntityConfig.OnPrefabInit(GameObject inst)
  {
  }

  private static LargeImpactorStatus.Instance GetStatusMonitor()
  {
    return ((LargeImpactorEvent.StatesInstance) GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) Db.Get().GameplayEvents.LargeImpactor.Id).smi).impactorInstance.GetSMI<LargeImpactorStatus.Instance>();
  }

  public static void SpawnCommon(GameObject inst)
  {
    ParallaxBackgroundObject component = inst.GetComponent<ParallaxBackgroundObject>();
    component.motion = (ParallaxBackgroundObject.IMotion) new LargeImpactorVanillaConfig.BackgroundMotion();
    LargeImpactorStatus.Instance statusMonitor = LargeImpactorVanillaConfig.GetStatusMonitor();
    if (statusMonitor == null)
      return;
    statusMonitor.OnDamaged += new Action<int>(component.TriggerShaderDamagedEffect);
  }

  void IEntityConfig.OnSpawn(GameObject inst) => LargeImpactorVanillaConfig.SpawnCommon(inst);

  public class BackgroundMotion : ParallaxBackgroundObject.IMotion
  {
    private LargeImpactorStatus.Instance statusMonitor;

    private LargeImpactorStatus.Instance StatusMonitor
    {
      get
      {
        if (this.statusMonitor == null)
          this.statusMonitor = LargeImpactorVanillaConfig.GetStatusMonitor();
        return this.statusMonitor;
      }
    }

    public float GetETA()
    {
      return !this.StatusMonitor.IsRunning() ? this.GetDuration() : this.StatusMonitor.TimeRemainingBeforeCollision;
    }

    public float GetDuration() => LargeImpactorEvent.GetImpactTime();

    public void OnNormalizedDistanceChanged(float normalizedDistance)
    {
      foreach (AmbienceManager.Quadrant quadrant in Game.Instance.GetComponent<AmbienceManager>().quadrants)
        quadrant.spaceLayer.SetCustomParameter("distanceToMeteor", normalizedDistance);
    }
  }
}
