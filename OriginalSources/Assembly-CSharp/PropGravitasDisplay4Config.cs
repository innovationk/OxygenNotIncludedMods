// Decompiled with JetBrains decompiler
// Type: PropGravitasDisplay4Config
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PropGravitasDisplay4Config : IEntityConfig, IHasDlcRestrictions
{
  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDISPLAY4.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDISPLAY4.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "gravitas_display4_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropGravitasDisplay4", name, desc, 50f, anim, "off", Grid.SceneLayer.Building, 1, 3, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Granite);
    component.Temperature = 294.15f;
    LoreBearerUtil.AddLoreTo(placedEntity, new LoreBearerAction(LoreBearerUtil.UnlockNextDimensionalLore));
    placedEntity.AddOrGet<Demolishable>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
