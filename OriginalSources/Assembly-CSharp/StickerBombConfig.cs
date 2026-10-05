// Decompiled with JetBrains decompiler
// Type: StickerBombConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class StickerBombConfig : IEntityConfig
{
  public const string ID = "StickerBomb";

  public GameObject CreatePrefab()
  {
    GameObject basicEntity = EntityTemplates.CreateBasicEntity("StickerBomb", (string) STRINGS.BUILDINGS.PREFABS.STICKERBOMB.NAME, (string) STRINGS.BUILDINGS.PREFABS.STICKERBOMB.DESC, 1f, true, Assets.GetAnim((HashedString) "sticker_a_kanim"), "off", Grid.SceneLayer.Backwall);
    EntityTemplates.AddCollision(basicEntity, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f);
    basicEntity.AddOrGet<StickerBomb>();
    return basicEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
    inst.AddComponent<Modifiers>();
    inst.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.BONUS.TIER2);
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
