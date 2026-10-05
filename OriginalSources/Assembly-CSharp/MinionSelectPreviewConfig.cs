// Decompiled with JetBrains decompiler
// Type: MinionSelectPreviewConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MinionSelectPreviewConfig : IEntityConfig
{
  public static string ID = "MinionSelectPreview";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(MinionSelectPreviewConfig.ID, MinionSelectPreviewConfig.ID);
    RectTransform rectTransform = entity.AddOrGet<RectTransform>();
    rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
    rectTransform.anchorMax = new Vector2(1f, 1f);
    rectTransform.pivot = new Vector2(0.5f, 0.0f);
    rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
    rectTransform.sizeDelta = new Vector2(0.0f, 0.0f);
    LayoutElement layoutElement = entity.AddOrGet<LayoutElement>();
    layoutElement.preferredHeight = 100f;
    layoutElement.preferredWidth = 100f;
    entity.AddOrGet<Effects>();
    entity.AddOrGet<Traits>();
    MinionModifiers minionModifiers = entity.AddOrGet<MinionModifiers>();
    minionModifiers.initialTraits.Add(BaseMinionConfig.GetMinionBaseTraitIDForModel(MinionConfig.MODEL));
    BaseMinionConfig.AddMinionAttributes((Modifiers) minionModifiers, MinionConfig.GetAttributes());
    BaseMinionConfig.AddMinionAmounts((Modifiers) minionModifiers, MinionConfig.GetAmounts());
    entity.AddOrGet<AttributeLevels>();
    entity.AddOrGet<AttributeConverters>();
    entity.AddOrGet<MinionIdentity>().addToIdentityList = false;
    entity.AddOrGet<BoxCollider2D>().size = new Vector2(1f, 1f);
    entity.AddOrGet<FaceGraph>();
    entity.AddOrGet<Accessorizer>();
    entity.AddOrGet<WearableAccessorizer>();
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
    kbatchedAnimController.animScale = 0.5f;
    kbatchedAnimController.setScaleFromAnim = false;
    kbatchedAnimController.animOverrideSize = new Vector2(100f, 120f);
    kbatchedAnimController.AnimFiles = new KAnimFile[4]
    {
      Assets.GetAnim((HashedString) "body_comp_default_kanim"),
      Assets.GetAnim((HashedString) "anim_construction_default_kanim"),
      Assets.GetAnim((HashedString) "anim_idles_default_kanim"),
      Assets.GetAnim((HashedString) "anim_cheer_kanim")
    };
    SymbolOverrideControllerUtil.AddToPrefab(entity);
    BaseMinionConfig.ConfigureSymbols(entity, false);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
