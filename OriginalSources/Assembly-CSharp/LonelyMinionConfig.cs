// Decompiled with JetBrains decompiler
// Type: LonelyMinionConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class LonelyMinionConfig : IEntityConfig
{
  public static string ID = "LonelyMinion";
  public const int VOICE_IDX = -2;
  public const int STARTING_SKILL_POINTS = 3;
  public const int BASE_ATTRIBUTE_LEVEL = 7;
  public const int AGE_MIN = 2190;
  public const int AGE_MAX = 3102;
  public const float MIN_IDLE_DELAY = 20f;
  public const float MAX_IDLE_DELAY = 40f;
  public const string IDLE_PREFIX = "idle_blinds";
  public static readonly HashedString GreetingCriteraId = (HashedString) "Neighbor";
  public static readonly HashedString FoodCriteriaId = (HashedString) "FoodQuality";
  public static readonly HashedString DecorCriteriaId = (HashedString) "Decor";
  public static readonly HashedString PowerCriteriaId = (HashedString) "SuppliedPower";
  public static readonly HashedString CHECK_MAIL = (HashedString) "mail_pre";
  public static readonly HashedString CHECK_MAIL_SUCCESS = (HashedString) "mail_success_pst";
  public static readonly HashedString CHECK_MAIL_FAILURE = (HashedString) "mail_failure_pst";
  public static readonly HashedString CHECK_MAIL_DUPLICATE = (HashedString) "mail_duplicate_pst";
  public static readonly HashedString FOOD_SUCCESS = (HashedString) "food_like_loop";
  public static readonly HashedString FOOD_FAILURE = (HashedString) "food_dislike_loop";
  public static readonly HashedString FOOD_DUPLICATE = (HashedString) "food_duplicate_loop";
  public static readonly HashedString FOOD_IDLE = (HashedString) "idle_food_quest";
  public static readonly HashedString DECOR_IDLE = (HashedString) "idle_decor_quest";
  public static readonly HashedString POWER_IDLE = (HashedString) "idle_power_quest";
  public static readonly HashedString BLINDS_IDLE_0 = (HashedString) "idle_blinds_0";
  public static readonly HashedString PARCEL_SNAPTO = (HashedString) "parcel_snapTo";
  public const string PERSONALITY_ID = "JORGE";
  public const string BODY_ANIM_FILE = "body_lonelyminion_kanim";

  public GameObject CreatePrefab()
  {
    string name = (string) DUPLICANTS.MODEL.STANDARD.NAME;
    GameObject entity = EntityTemplates.CreateEntity(LonelyMinionConfig.ID, name);
    entity.AddComponent<Accessorizer>();
    entity.AddOrGet<WearableAccessorizer>();
    entity.AddComponent<Storage>().doDiseaseTransfer = false;
    entity.AddComponent<StateMachineController>();
    LonelyMinion.Def def = entity.AddOrGetDef<LonelyMinion.Def>();
    def.Personality = Db.Get().Personalities.Get("JORGE");
    def.Personality.Disabled = true;
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.defaultAnim = "idle_default";
    kbatchedAnimController.initialAnim = "idle_default";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.AnimFiles = new KAnimFile[3]
    {
      Assets.GetAnim((HashedString) "body_comp_default_kanim"),
      Assets.GetAnim((HashedString) "anim_idles_default_kanim"),
      Assets.GetAnim((HashedString) "anim_interacts_lonely_dupe_kanim")
    };
    this.ConfigurePackageOverride(entity);
    SymbolOverrideController prefab = SymbolOverrideControllerUtil.AddToPrefab(entity);
    prefab.applySymbolOverridesEveryFrame = true;
    prefab.AddSymbolOverride((HashedString) "snapto_cheek", Assets.GetAnim((HashedString) "head_swap_kanim").GetData().build.GetSymbol((KAnimHashedString) $"cheek_00{def.Personality.headShape}"), 1);
    BaseMinionConfig.ConfigureSymbols(entity);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }

  private void ConfigurePackageOverride(GameObject go)
  {
    GameObject go1 = new GameObject("PackageSnapPoint");
    go1.transform.SetParent(go.transform);
    KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
    KBatchedAnimController kbatchedAnimController = go1.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.transform.position = Vector3.forward * -0.1f;
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "mushbar_kanim")
    };
    kbatchedAnimController.initialAnim = "object";
    component.SetSymbolVisiblity((KAnimHashedString) LonelyMinionConfig.PARCEL_SNAPTO, false);
    KBatchedAnimTracker kbatchedAnimTracker = go1.AddOrGet<KBatchedAnimTracker>();
    kbatchedAnimTracker.controller = component;
    kbatchedAnimTracker.symbol = LonelyMinionConfig.PARCEL_SNAPTO;
  }
}
