// Decompiled with JetBrains decompiler
// Type: KleiPermitVisUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public static class KleiPermitVisUtil
{
  public const float TILE_SIZE_UI = 176f;
  public static KleiPermitBuildingAnimateIn buildingAnimateIn;

  public static void ConfigureToRenderBuilding(
    KBatchedAnimController buildingKAnim,
    BuildingFacadeResource buildingPermit)
  {
    KAnimFile anim = Assets.GetAnim((HashedString) buildingPermit.AnimFile);
    buildingKAnim.Stop();
    buildingKAnim.SwapAnims(new KAnimFile[1]{ anim });
    buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(anim), KAnim.PlayMode.Loop);
    buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
  }

  public static void ConfigureToRenderBuilding(
    KBatchedAnimController buildingKAnim,
    BuildingDef buildingDef)
  {
    buildingKAnim.Stop();
    buildingKAnim.SwapAnims(buildingDef.AnimFiles);
    buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(buildingDef.AnimFiles[0]), KAnim.PlayMode.Loop);
    buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
  }

  public static void ConfigureToRenderBuilding(
    KBatchedAnimController buildingKAnim,
    ArtableStage artablePermit)
  {
    buildingKAnim.Stop();
    buildingKAnim.SwapAnims(new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) artablePermit.animFile)
    });
    buildingKAnim.Play((HashedString) artablePermit.anim);
    buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
  }

  public static void ConfigureToRenderBuilding(
    KBatchedAnimController buildingKAnim,
    DbStickerBomb artablePermit)
  {
    buildingKAnim.Stop();
    buildingKAnim.SwapAnims(new KAnimFile[1]
    {
      artablePermit.animFile
    });
    HashedString defaultStickerAnimHash = KleiPermitVisUtil.GetDefaultStickerAnimHash(artablePermit.animFile);
    if (defaultStickerAnimHash != (HashedString) (string) null)
    {
      buildingKAnim.Play(defaultStickerAnimHash);
    }
    else
    {
      Debug.Assert(false, (object) ("Couldn't find default sticker for sticker " + artablePermit.Id));
      buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(artablePermit.animFile));
    }
    buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
  }

  public static void ConfigureToRenderBuilding(
    KBatchedAnimController buildingKAnim,
    MonumentPartResource monumentPermit)
  {
    buildingKAnim.Stop();
    buildingKAnim.SwapAnims(new KAnimFile[1]
    {
      monumentPermit.AnimFile
    });
    buildingKAnim.Play((HashedString) monumentPermit.State);
    buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
  }

  public static void ConfigureBuildingPosition(
    RectTransform transform,
    PrefabDefinedUIPosition anchorPosition,
    BuildingDef buildingDef,
    Alignment alignment)
  {
    anchorPosition.SetOn((Component) transform);
    transform.anchoredPosition += new Vector2((float) (176.0 * (double) buildingDef.WidthInCells * -((double) alignment.x - 0.5)), (float) (176.0 * (double) buildingDef.HeightInCells * -(double) alignment.y));
  }

  public static void ConfigureBuildingPosition(
    RectTransform transform,
    Vector2 anchorPosition,
    BuildingDef buildingDef,
    Alignment alignment)
  {
    transform.anchoredPosition = anchorPosition + new Vector2((float) (176.0 * (double) buildingDef.WidthInCells * -((double) alignment.x - 0.5)), (float) (176.0 * (double) buildingDef.HeightInCells * -(double) alignment.y));
  }

  public static void ClearAnimation()
  {
    if (KleiPermitVisUtil.buildingAnimateIn.IsNullOrDestroyed())
      return;
    Object.Destroy((Object) KleiPermitVisUtil.buildingAnimateIn.gameObject);
  }

  public static void AnimateIn(
    KBatchedAnimController buildingKAnim,
    Updater extraUpdater = default (Updater),
    string place_anim = "place")
  {
    KleiPermitVisUtil.ClearAnimation();
    KleiPermitVisUtil.buildingAnimateIn = KleiPermitBuildingAnimateIn.MakeFor(buildingKAnim, extraUpdater, place_anim);
  }

  public static HashedString GetFirstAnimHash(KAnimFile animFile)
  {
    return animFile.GetData().GetAnim(0).hash;
  }

  public static HashedString GetDefaultStickerAnimHash(KAnimFile stickerAnimFile)
  {
    KAnimFileData data = stickerAnimFile.GetData();
    for (int index = 0; index < data.animCount; ++index)
    {
      KAnim.Anim anim = data.GetAnim(index);
      if (anim.name.StartsWith("idle_sticker"))
        return anim.hash;
    }
    return (HashedString) (string) null;
  }

  public static BuildLocationRule? GetBuildLocationRule(PermitResource permit)
  {
    BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
    return (Object) buildingDef == (Object) null ? new BuildLocationRule?() : new BuildLocationRule?(buildingDef.BuildLocationRule);
  }

  public static BuildingDef GetBuildingDef(PermitResource permit)
  {
    switch (permit)
    {
      case BuildingFacadeResource buildingFacadeResource:
        GameObject prefab = Assets.TryGetPrefab((Tag) buildingFacadeResource.PrefabID);
        if ((Object) prefab == (Object) null)
          return (BuildingDef) null;
        BuildingComplete component1 = prefab.GetComponent<BuildingComplete>();
        return (Object) component1 == (Object) null || !(bool) (Object) component1 ? (BuildingDef) null : component1.Def;
      case ArtableStage artableStage:
        BuildingComplete component2 = Assets.GetPrefab((Tag) artableStage.prefabId).GetComponent<BuildingComplete>();
        return (Object) component2 == (Object) null || !(bool) (Object) component2 ? (BuildingDef) null : component2.Def;
      case MonumentPartResource _:
        BuildingComplete component3 = Assets.GetPrefab((Tag) "MonumentBottom").GetComponent<BuildingComplete>();
        return (Object) component3 == (Object) null || !(bool) (Object) component3 ? (BuildingDef) null : component3.Def;
      default:
        return (BuildingDef) null;
    }
  }
}
