// Decompiled with JetBrains decompiler
// Type: RemoteWorkerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class RemoteWorkerConfig : IEntityConfig, IHasDlcRestrictions
{
  public static readonly string ID = "RemoteWorker";
  public const float MASS_KG = 200f;
  public const float DEBRIS_MASS_KG = 42f;
  public static readonly string DOCK_ANIM_OVERRIDES = "anim_interacts_remote_work_dock_kanim";
  public static readonly string IDLE_IN_DOCK_ANIM = "in_dock_idle";
  public static readonly string BUILD_MATERIAL = "Steel";
  public static readonly Tag BUILD_MATERIAL_TAG = new Tag(RemoteWorkerConfig.BUILD_MATERIAL);

  public string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) DUPLICANTS.MODEL.REMOTEWORKER.NAME;
    string desc = (string) DUPLICANTS.MODEL.REMOTEWORKER.DESC;
    GameObject entity = EntityTemplates.CreateEntity(RemoteWorkerConfig.ID, name);
    entity.AddOrGet<InfoDescription>().description = desc;
    entity.AddComponent<Accessorizer>();
    entity.AddOrGet<WearableAccessorizer>();
    entity.AddComponent<StateMachineController>();
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.defaultAnim = "in_dock_idle";
    kbatchedAnimController.initialAnim = "in_dock_idle";
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.AnimFiles = new KAnimFile[4]
    {
      Assets.GetAnim((HashedString) "body_comp_default_kanim"),
      Assets.GetAnim((HashedString) "anim_idles_default_kanim"),
      Assets.GetAnim((HashedString) "anim_loco_new_kanim"),
      Assets.GetAnim((HashedString) RemoteWorkerConfig.DOCK_ANIM_OVERRIDES)
    };
    entity.AddOrGet<AnimEventHandler>();
    SymbolOverrideController prefab = SymbolOverrideControllerUtil.AddToPrefab(entity);
    prefab.applySymbolOverridesEveryFrame = true;
    prefab.AddSymbolOverride((HashedString) "snapto_cheek", Assets.GetAnim((HashedString) "head_swap_kanim").GetData().build.GetSymbol((KAnimHashedString) "cheek_007"), 1);
    BaseMinionConfig.ConfigureSymbols(entity);
    Accessorizer component = entity.GetComponent<Accessorizer>();
    component.ApplyBodyData(RemoteWorkerConfig.CreateBodyData());
    component.ApplyAccessories();
    entity.AddTag(GameTags.Experimental);
    entity.AddTag(GameTags.Robot);
    KBoxCollider2D kboxCollider2D1 = entity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D1.size = (Vector2) new Vector2f(1f, 2f);
    kboxCollider2D1.offset = (Vector2) new Vector2f(0.0f, 1f);
    KBoxCollider2D kboxCollider2D2 = entity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D2.offset = new Vector2(0.0f, 0.75f);
    kboxCollider2D2.size = new Vector2(1f, 1.5f);
    Navigator navigator = entity.AddOrGet<Navigator>();
    navigator.NavGridName = "WalkerBabyNavGrid";
    navigator.CurrentNavType = NavType.Floor;
    navigator.defaultSpeed = 1f;
    navigator.updateProber = true;
    navigator.maxProbeRadiusX = 12;
    navigator.maxProbeRadiusY = 1;
    navigator.sceneLayer = Grid.SceneLayer.Creatures;
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.ElementID = SimHashes.Steel;
    primaryElement.Mass = 200f;
    entity.AddComponent<RemoteWorkerExperienceProxy>();
    entity.AddComponent<RemoteWorker>();
    entity.AddComponent<RemoteWorkerSM>();
    entity.AddComponent<ChoreConsumer>();
    entity.AddComponent<Pickupable>();
    entity.AddComponent<SaveLoadRoot>();
    entity.AddComponent<Storage>().SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    entity.AddOrGet<Clearable>().isClearable = false;
    entity.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = false;
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
    Navigator navigator = go.AddOrGet<Navigator>();
    navigator.SetAbilities((PathFinderAbilities) new CreaturePathFinderAbilities(navigator));
  }

  public void OnSpawn(GameObject go)
  {
  }

  public static KCompBuilder.BodyData CreateBodyData()
  {
    KCompBuilder.BodyData bodyData = new KCompBuilder.BodyData()
    {
      eyes = HashCache.Get().Add("eyes_014"),
      hair = HashCache.Get().Add("hair_051"),
      headShape = HashCache.Get().Add("headshape_006"),
      mouth = HashCache.Get().Add("mouth_007"),
      neck = HashCache.Get().Add("neck"),
      arms = HashCache.Get().Add("arm_sleeve_006"),
      armslower = HashCache.Get().Add("arm_lower_sleeve_006"),
      body = HashCache.Get().Add("torso_006"),
      hat = HashedString.Invalid,
      faceFX = HashedString.Invalid,
      armLowerSkin = HashCache.Get().Add("arm_lower_001"),
      armUpperSkin = HashCache.Get().Add("arm_upper_001"),
      legSkin = HashCache.Get().Add("leg_skin_001")
    };
    bodyData.neck = HashCache.Get().Add("neck_006");
    bodyData.legs = HashCache.Get().Add("leg_006");
    bodyData.belt = HashCache.Get().Add("belt_006");
    bodyData.pelvis = HashCache.Get().Add("pelvis_006");
    bodyData.foot = HashCache.Get().Add("foot_006");
    bodyData.hand = HashCache.Get().Add("hand_paint_006");
    bodyData.cuff = HashCache.Get().Add("cuff_006");
    return bodyData;
  }
}
