// Decompiled with JetBrains decompiler
// Type: EggShellConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class EggShellConfig : IEntityConfig
{
  public const string ID = "EggShell";
  public static readonly Tag TAG = TagManager.Create("EggShell");
  public const float EGG_TO_SHELL_RATIO = 0.5f;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("EggShell", (string) ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.DESC, 1f, false, Assets.GetAnim((HashedString) "eggshells_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true);
    looseEntity.GetComponent<KPrefabID>().AddTag(GameTags.Organics);
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<SimpleMassStatusItem>();
    EntityTemplates.CreateAndRegisterCompostableFromPrefab(looseEntity);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
