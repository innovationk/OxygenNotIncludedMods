// Decompiled with JetBrains decompiler
// Type: PropGravitasFirstAidKitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PropGravitasFirstAidKitConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "gravitas_first_aid_kit_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropGravitasFirstAidKit", name, desc, 50f, anim, "off", Grid.SceneLayer.Building, 1, 1, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Granite);
    component.Temperature = 294.15f;
    Workable workable = placedEntity.AddOrGet<Workable>();
    workable.synchronizeAnims = false;
    workable.resetProgressOnStop = true;
    SetLocker setLocker = placedEntity.AddOrGet<SetLocker>();
    setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
    setLocker.dropOffset = new Vector2I(0, 1);
    placedEntity.AddOrGet<Demolishable>();
    return placedEntity;
  }

  public static string[][] GetLockerBaseContents()
  {
    string str = DlcManager.FeatureRadiationEnabled() ? "BasicRadPill" : "IntermediateCure";
    return new string[2][]
    {
      new string[3]{ "BasicCure", "BasicCure", "BasicCure" },
      new string[2]{ str, str }
    };
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    SetLocker component = inst.GetComponent<SetLocker>();
    component.possible_contents_ids = PropGravitasFirstAidKitConfig.GetLockerBaseContents();
    component.ChooseContents();
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
