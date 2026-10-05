// Decompiled with JetBrains decompiler
// Type: PropGravitasSmallSeedLockerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PropGravitasSmallSeedLockerConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSMALLSEEDLOCKER.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSMALLSEEDLOCKER.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "gravitas_medical_locker_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropGravitasSmallSeedLocker", name, desc, 50f, anim, "on", Grid.SceneLayer.Building, 1, 1, decor, PermittedRotations.R90, noise: noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    Workable workable = placedEntity.AddOrGet<Workable>();
    workable.synchronizeAnims = false;
    workable.resetProgressOnStop = true;
    GravitasLocker.Def def = placedEntity.AddOrGetDef<GravitasLocker.Def>();
    def.CanBeClosed = false;
    def.SideScreen_OpenButtonText = (string) UI.USERMENUACTIONS.EMPTYSTORAGE.NAME;
    def.SideScreen_OpenButtonTooltip = (string) UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP;
    def.SideScreen_CancelOpenButtonText = (string) UI.USERMENUACTIONS.EMPTYSTORAGE.NAME_OFF;
    def.SideScreen_CancelOpenButtonTooltip = (string) UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP_OFF;
    def.SideScreen_CloseButtonText = (string) UI.USERMENUACTIONS.CLOSESTORAGE.NAME;
    def.SideScreen_CloseButtonTooltip = (string) UI.USERMENUACTIONS.CLOSESTORAGE.TOOLTIP;
    def.SideScreen_CancelCloseButtonText = (string) UI.USERMENUACTIONS.CLOSESTORAGE.NAME_OFF;
    def.SideScreen_CancelCloseButtonTooltip = (string) UI.USERMENUACTIONS.CLOSESTORAGE.TOOLTIP_OFF;
    def.ObjectsToSpawn = new string[2]
    {
      "EvilFlowerSeed",
      "EvilFlowerSeed"
    };
    def.LootSymbols = new string[2]{ "seed1", "seed2" };
    LoreBearerUtil.AddLoreTo(placedEntity, LoreBearerUtil.UnlockSpecificEntry("story_trait_morbrover_locker", (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.LOCKER.DESCRIPTION));
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<Demolishable>();
    SymbolOverrideControllerUtil.AddToPrefab(placedEntity);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<Workable>().SetOffsets(new CellOffset[2]
    {
      new CellOffset(0, 0),
      CellOffset.down
    });
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
