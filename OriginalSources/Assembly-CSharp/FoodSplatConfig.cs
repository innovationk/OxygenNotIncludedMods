// Decompiled with JetBrains decompiler
// Type: FoodSplatConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class FoodSplatConfig : IEntityConfig
{
  public const string ID = "FoodSplat";

  public GameObject CreatePrefab()
  {
    return EntityTemplates.CreateBasicEntity("FoodSplat", (string) STRINGS.ITEMS.FOOD.FOODSPLAT.NAME, (string) STRINGS.ITEMS.FOOD.FOODSPLAT.DESC, 1f, true, Assets.GetAnim((HashedString) "sticker_a_kanim"), "idle_sticker_a", Grid.SceneLayer.Backwall);
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
    inst.AddComponent<Modifiers>();
    inst.AddOrGet<KSelectable>();
    inst.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.PENALTY.TIER2);
    inst.AddOrGetDef<Splat.Def>();
    inst.AddOrGet<SplatWorkable>();
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
