// Decompiled with JetBrains decompiler
// Type: WarpPortalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class WarpPortalConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "WarpPortal";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.WARPPORTAL.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.WARPPORTAL.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "warp_portal_sender_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("WarpPortal", name, desc, 2000f, anim, "idle", Grid.SceneLayer.Building, 3, 3, decor, noise);
    placedEntity.AddTag(GameTags.NotRoomAssignable);
    placedEntity.AddTag(GameTags.WarpTech);
    placedEntity.AddTag(GameTags.Gravitas);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    placedEntity.AddOrGet<Operational>();
    placedEntity.AddOrGet<Notifier>();
    placedEntity.AddOrGet<WarpPortal>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<Ownable>().tintWhenUnassigned = false;
    LoreBearerUtil.AddLoreTo(placedEntity, LoreBearerUtil.UnlockSpecificEntry("notes_teleportation", (string) UI.USERMENUACTIONS.READLORE.SEARCH_TELEPORTER_SENDER));
    placedEntity.AddOrGet<Prioritizable>();
    KBatchedAnimController kbatchedAnimController = placedEntity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
    kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<WarpPortal>().workLayer = Grid.SceneLayer.Building;
    inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.WarpPortal.Id;
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    inst.GetComponent<Deconstructable>();
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
