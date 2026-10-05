// Decompiled with JetBrains decompiler
// Type: SeaFairyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class SeaFairyConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SeaFairy";
  public const string BASE_TRAIT_ID = "SeaFairyBaseTrait";

  public static GameObject CreateSeaFairy(string id, string name, string desc, string anim_file)
  {
    GameObject go = BaseSeaFairyConfig.BaseSeaFairy(id, name, desc, anim_file, "SeaFairyBaseTrait");
    go.AddOrGetDef<AgeMonitor.Def>();
    go.AddOrGetDef<FixedCapturableMonitor.Def>();
    Trait trait = Db.Get().CreateTrait("SeaFairyBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 5f, name));
    go.AddTag(GameTags.OriginalCreature);
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return SeaFairyConfig.CreateSeaFairy("SeaFairy", (string) CREATURES.SPECIES.SEAFAIRY.NAME, (string) CREATURES.SPECIES.SEAFAIRY.DESC, "sea_fairy_kanim");
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
