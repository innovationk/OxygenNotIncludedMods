// Decompiled with JetBrains decompiler
// Type: MissileSetLockerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class MissileSetLockerConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.MISSILESETLOCKER.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.MISSILESETLOCKER.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "armoury_locker_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("MissileSetLocker", name, desc, 100f, anim, "on", Grid.SceneLayer.Building, 1, 2, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas,
      GameTags.TemplateBuilding
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    Workable workable = placedEntity.AddOrGet<Workable>();
    workable.synchronizeAnims = true;
    workable.resetProgressOnStop = true;
    SetLocker setLocker = placedEntity.AddOrGet<SetLocker>();
    setLocker.overrideAnim = "anim_interacts_locker_kanim";
    setLocker.skipAnim = true;
    setLocker.dropOffset = new Vector2I(0, 1);
    setLocker.numDataBanks = new int[2]{ 1, 4 };
    LoreBearerUtil.AddLoreTo(placedEntity);
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<Demolishable>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    SetLocker component = inst.GetComponent<SetLocker>();
    component.possible_contents_ids = new string[1][]
    {
      new string[1]{ "MissileLongRange" }
    };
    component.ChooseContents();
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
