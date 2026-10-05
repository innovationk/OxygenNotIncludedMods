// Decompiled with JetBrains decompiler
// Type: PropClothesHanger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PropClothesHanger : IEntityConfig, IHasDlcRestrictions
{
  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "unlock_clothing_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(nameof (PropClothesHanger), name, desc, 50f, anim, "on", Grid.SceneLayer.Building, 1, 2, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas,
      GameTags.RoomProberBuilding
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Cinnabar);
    component.Temperature = 294.15f;
    placedEntity.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    Workable workable = placedEntity.AddOrGet<Workable>();
    workable.synchronizeAnims = false;
    workable.resetProgressOnStop = true;
    SetLocker setLocker = placedEntity.AddOrGet<SetLocker>();
    setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
    setLocker.dropOffset = new Vector2I(0, 1);
    setLocker.dropOnDeconstruct = true;
    placedEntity.AddOrGet<Deconstructable>().audioSize = "small";
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    SetLocker component = inst.GetComponent<SetLocker>();
    component.possible_contents_ids = new string[1][]
    {
      new string[1]{ "Warm_Vest" }
    };
    component.ChooseContents();
  }

  public void OnSpawn(GameObject inst) => inst.GetComponent<Deconstructable>().SetWorkTime(5f);
}
