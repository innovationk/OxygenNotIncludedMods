// Decompiled with JetBrains decompiler
// Type: UnderwaterVentConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class UnderwaterVentConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "UnderwaterVent";
  public static readonly UnderwaterVent.Data Data = new UnderwaterVent.Data(new Vector3(1f, 2.5f, 0.0f), new Vector3(1f, 1.5f, 0.0f), SimHashes.Methane, 373.15f, 0.0833333358f, SimHashes.Sulfur, 1000f, 373.15f, 1200f);

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.GEYSER.UNDERWATERVENT.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.GEYSER.UNDERWATERVENT.DESC;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    KAnimFile anim = Assets.GetAnim((HashedString) "underwater_vent_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = tieR5;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("UnderwaterVent", name, desc, 2000f, anim, "idle", Grid.SceneLayer.BuildingBack, 4, 4, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.GeyserFeature
    });
    placedEntity.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<EntombVulnerable>();
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Katairite);
    component.Temperature = 363.15f;
    placedEntity.AddOrGet<Submergable>().GetStatusItem = new Func<StatusItem>(UnderwaterVentConfig.GetSubmergableStatusItem);
    placedEntity.AddOrGetDef<UnderwaterVent.Def>().data = UnderwaterVentConfig.Data;
    placedEntity.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 0), GameTags.UnderwaterVentDrill, (AttachableBuilding) null)
    };
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<Submergable>().GetStatusItem = new Func<StatusItem>(UnderwaterVentConfig.GetSubmergableStatusItem);
  }

  public void OnSpawn(GameObject inst)
  {
  }

  private static StatusItem GetSubmergableStatusItem() => Db.Get().CreatureStatusItems.NotSubmerged;
}
