// Decompiled with JetBrains decompiler
// Type: MinionIdentity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionIdentity")]
public class MinionIdentity : 
  KMonoBehaviour,
  ISaveLoadable,
  IAssignableIdentity,
  IListableOption,
  ISim1000ms
{
  public const string HairAlwaysSymbol = "snapto_hair_always";
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpReq]
  public Modifiers modifiers;
  public int femaleVoiceCount;
  public int maleVoiceCount;
  [Serialize]
  public Tag model;
  [Serialize]
  private string name;
  [Serialize]
  public string gender;
  [Serialize]
  public string stickerType;
  [Serialize]
  [ReadOnly]
  public float arrivalTime;
  [Serialize]
  public int voiceIdx;
  [Serialize]
  public Ref<MinionAssignablesProxy> assignableProxy;
  private Navigator navigator;
  private ChoreDriver choreDriver;
  public float timeLastSpoke;
  private string voiceId;
  private KAnimHashedString overrideExpression;
  private KAnimHashedString expression;
  public bool addToIdentityList = true;
  private static MinionIdentity.NameList maleNameList;
  private static MinionIdentity.NameList femaleNameList;
  private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<MinionIdentity>(GameTags.Dead, (Action<MinionIdentity, object>) ((component, data) => component.OnDied(data)));
  private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<MinionIdentity>((Action<MinionIdentity, object>) ((component, data) => component.OnQueueDestroyObject()));

  [Serialize]
  public string genderStringKey { get; set; }

  [Serialize]
  public string nameStringKey { get; set; }

  [Serialize]
  public HashedString personalityResourceId { get; set; }

  public static void DestroyStatics()
  {
    MinionIdentity.maleNameList = (MinionIdentity.NameList) null;
    MinionIdentity.femaleNameList = (MinionIdentity.NameList) null;
  }

  protected override void OnPrefabInit()
  {
    if (this.name == null)
      this.name = MinionIdentity.ChooseRandomName();
    if ((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null)
      this.arrivalTime = (float) GameClock.Instance.GetCycle();
    KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.OnUpdateBounds += new Action<Bounds>(this.OnUpdateBounds);
    GameUtil.SubscribeToTags<MinionIdentity>(this, MinionIdentity.OnDeadTagAddedDelegate, true);
    this.Subscribe<MinionIdentity>(1502190696, MinionIdentity.OnQueueDestroyObjectDelegate);
  }

  protected override void OnSpawn()
  {
    if (this.addToIdentityList)
    {
      this.ValidateProxy();
      this.CleanupLimboMinions();
    }
    Navigator component1 = this.GetComponent<Navigator>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.reportOccupation = true;
    this.SetName(this.name);
    if (this.nameStringKey == null)
      this.nameStringKey = this.name;
    this.SetGender(this.gender);
    if (this.genderStringKey == null)
      this.genderStringKey = "NB";
    if (this.personalityResourceId == HashedString.Invalid)
    {
      Personality fromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(this.nameStringKey);
      if (fromNameStringKey != null)
        this.personalityResourceId = (HashedString) fromNameStringKey.Id;
    }
    if (!this.model.IsValid)
    {
      Personality personality = Db.Get().Personalities.Get(this.personalityResourceId);
      if (personality != null)
        this.model = personality.model;
    }
    if (this.addToIdentityList)
    {
      Components.MinionIdentities.Add(this);
      if (!Components.MinionIdentitiesByModel.ContainsKey(this.model))
        Components.MinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
      Components.MinionIdentitiesByModel[this.model].Add(this);
      if (!this.gameObject.HasTag(GameTags.Dead))
      {
        Components.LiveMinionIdentities.Add(this);
        if (!Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
          Components.LiveMinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
        Components.LiveMinionIdentitiesByModel[this.model].Add(this);
        Game.Instance.Trigger(2144209314, (object) this);
      }
    }
    SymbolOverrideController component2 = this.GetComponent<SymbolOverrideController>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      Accessorizer component3 = this.gameObject.GetComponent<Accessorizer>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      {
        string symbol_name = HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol.hash).Replace("mouth", "cheek");
        component2.AddSymbolOverride((HashedString) "snapto_cheek", Assets.GetAnim((HashedString) "head_swap_kanim").GetData().build.GetSymbol((KAnimHashedString) symbol_name), 1);
        component2.AddSymbolOverride((HashedString) "snapto_hair_always", component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol, 1);
        component2.AddSymbolOverride((HashedString) Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
      }
    }
    this.voiceId = (this.voiceIdx + 1).ToString("D2");
    Prioritizable component4 = this.GetComponent<Prioritizable>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      component4.showIcon = false;
    Pickupable component5 = this.GetComponent<Pickupable>();
    if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
      component5.carryAnimOverride = Assets.GetAnim((HashedString) "anim_incapacitated_carrier_kanim");
    this.ApplyCustomGameSettings();
  }

  public void ValidateProxy()
  {
    this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, (IAssignableIdentity) this);
  }

  private void CleanupLimboMinions()
  {
    KPrefabID component = this.GetComponent<KPrefabID>();
    if (component.InstanceID == -1)
    {
      DebugUtil.LogWarningArgs((object) "Minion with an invalid kpid! Attempting to recover...", (object) this.name);
      if ((UnityEngine.Object) KPrefabIDTracker.Get().GetInstance(component.InstanceID) != (UnityEngine.Object) null)
        KPrefabIDTracker.Get().Unregister(component);
      component.InstanceID = KPrefabID.GetUniqueID();
      KPrefabIDTracker.Get().Register(component);
      DebugUtil.LogWarningArgs((object) "Restored as:", (object) component.InstanceID);
    }
    if (component.conflicted)
    {
      DebugUtil.LogWarningArgs((object) "Minion with a conflicted kpid! Attempting to recover... ", (object) component.InstanceID, (object) this.name);
      if ((UnityEngine.Object) KPrefabIDTracker.Get().GetInstance(component.InstanceID) != (UnityEngine.Object) null)
        KPrefabIDTracker.Get().Unregister(component);
      component.InstanceID = KPrefabID.GetUniqueID();
      KPrefabIDTracker.Get().Register(component);
      DebugUtil.LogWarningArgs((object) "Restored as:", (object) component.InstanceID);
    }
    this.assignableProxy.Get().SetTarget((IAssignableIdentity) this, this.gameObject);
  }

  public string GetProperName() => this.selectable.GetProperName();

  public string GetVoiceId() => this.voiceId;

  public void SetName(string name)
  {
    this.name = name;
    if ((UnityEngine.Object) this.selectable != (UnityEngine.Object) null)
      this.selectable.SetName(name);
    this.gameObject.name = name;
    NameDisplayScreen.Instance.UpdateName(this.gameObject);
  }

  public void SetStickerType(string stickerType) => this.stickerType = stickerType;

  public bool IsNull() => (UnityEngine.Object) this == (UnityEngine.Object) null;

  public void SetGender(string gender)
  {
    this.gender = gender;
    this.selectable.SetGender(gender);
  }

  public static string ChooseRandomName()
  {
    if (MinionIdentity.femaleNameList == null)
    {
      MinionIdentity.maleNameList = new MinionIdentity.NameList(Game.Instance.maleNamesFile);
      MinionIdentity.femaleNameList = new MinionIdentity.NameList(Game.Instance.femaleNamesFile);
    }
    return (double) UnityEngine.Random.value > 0.5 ? MinionIdentity.maleNameList.Next() : MinionIdentity.femaleNameList.Next();
  }

  private void OnQueueDestroyObject() => this.RemoveFromComponentsLists();

  private void RemoveFromComponentsLists()
  {
    Components.MinionIdentities.Remove(this);
    if (Components.MinionIdentitiesByModel.ContainsKey(this.model))
      Components.MinionIdentitiesByModel[this.model].Remove(this);
    Components.LiveMinionIdentities.Remove(this);
    if (!Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
      return;
    Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
  }

  protected override void OnCleanUp()
  {
    if (this.assignableProxy != null)
    {
      MinionAssignablesProxy assignablesProxy = this.assignableProxy.Get();
      if ((bool) (UnityEngine.Object) assignablesProxy && assignablesProxy.target == this)
        Util.KDestroyGameObject(assignablesProxy.gameObject);
    }
    this.RemoveFromComponentsLists();
    Game.Instance.Trigger(2144209314, (object) this);
  }

  private void OnUpdateBounds(Bounds bounds)
  {
    KBoxCollider2D component = this.GetComponent<KBoxCollider2D>();
    component.offset = (Vector2) bounds.center;
    component.size = (Vector2) bounds.extents;
  }

  private void OnDied(object data)
  {
    this.GetSoleOwner().UnassignAll();
    this.GetEquipment().UnequipAll();
    Components.LiveMinionIdentities.Remove(this);
    if (Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
      Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
    Game.Instance.Trigger(-1523247426, (object) this);
    Game.Instance.Trigger(2144209314, (object) this);
  }

  public List<Ownables> GetOwners() => this.assignableProxy.Get().ownables;

  public Ownables GetSoleOwner() => this.assignableProxy.Get().GetComponent<Ownables>();

  public bool HasOwner(Assignables owner) => this.GetOwners().Contains(owner as Ownables);

  public int NumOwners() => this.GetOwners().Count;

  public Equipment GetEquipment() => this.assignableProxy.Get().GetComponent<Equipment>();

  public void Sim1000ms(float dt)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.navigator == (UnityEngine.Object) null)
      this.navigator = this.GetComponent<Navigator>();
    if ((UnityEngine.Object) this.navigator != (UnityEngine.Object) null && !this.navigator.IsMoving())
      return;
    if ((UnityEngine.Object) this.choreDriver == (UnityEngine.Object) null)
      this.choreDriver = this.GetComponent<ChoreDriver>();
    if (!((UnityEngine.Object) this.choreDriver != (UnityEngine.Object) null))
      return;
    Chore currentChore = this.choreDriver.GetCurrentChore();
    if (currentChore == null || !(currentChore is FetchAreaChore))
      return;
    MinionResume component = this.GetComponent<MinionResume>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.AddExperienceWithAptitude(Db.Get().SkillGroups.Hauling.Id, dt, SKILLS.ALL_DAY_EXPERIENCE);
  }

  private void ApplyCustomGameSettings()
  {
    SettingLevel currentQualitySetting1 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ImmuneSystem);
    if (currentQualitySetting1.id == "Compromised")
    {
      Db.Get().Attributes.DiseaseCureSpeed.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, -0.3333f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME));
      Db.Get().Attributes.GermResistance.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -2f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME));
    }
    else if (currentQualitySetting1.id == "Weak")
      Db.Get().Attributes.GermResistance.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -1f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.WEAK.ATTRIBUTE_MODIFIER_NAME));
    else if (currentQualitySetting1.id == "Strong")
    {
      Db.Get().Attributes.DiseaseCureSpeed.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 2f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME));
      Db.Get().Attributes.GermResistance.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 2f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME));
    }
    else if (currentQualitySetting1.id == "Invincible")
    {
      Db.Get().Attributes.DiseaseCureSpeed.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 1E+08f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME));
      Db.Get().Attributes.GermResistance.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 200f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME));
    }
    SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Stress);
    if (currentQualitySetting2.id == "Doomed")
      Db.Get().Amounts.Stress.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.0333333351f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DOOMED.ATTRIBUTE_MODIFIER_NAME));
    else if (currentQualitySetting2.id == "Pessimistic")
      Db.Get().Amounts.Stress.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.0166666675f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.PESSIMISTIC.ATTRIBUTE_MODIFIER_NAME));
    else if (currentQualitySetting2.id == "Optimistic")
      Db.Get().Amounts.Stress.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.0166666675f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.OPTIMISTIC.ATTRIBUTE_MODIFIER_NAME));
    else if (currentQualitySetting2.id == "Indomitable")
      Db.Get().Amounts.Stress.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, float.NegativeInfinity, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.INDOMITABLE.ATTRIBUTE_MODIFIER_NAME));
    SettingLevel currentQualitySetting3 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
    if (currentQualitySetting3.id == "VeryHard")
      Db.Get().Amounts.Calories.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 1f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.VERYHARD.ATTRIBUTE_MODIFIER_NAME));
    else if (currentQualitySetting3.id == "Hard")
      Db.Get().Amounts.Calories.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 0.5f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.HARD.ATTRIBUTE_MODIFIER_NAME));
    else if (currentQualitySetting3.id == "Easy")
    {
      Db.Get().Amounts.Calories.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * -0.5f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.EASY.ATTRIBUTE_MODIFIER_NAME));
    }
    else
    {
      if (!(currentQualitySetting3.id == "Disabled"))
        return;
      Db.Get().Amounts.Calories.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, float.PositiveInfinity, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DISABLED.ATTRIBUTE_MODIFIER_NAME));
    }
  }

  public static float GetCalorieBurnMultiplier()
  {
    float calorieBurnMultiplier = 1f;
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
    if (currentQualitySetting.id == "VeryHard")
      calorieBurnMultiplier = 2f;
    else if (currentQualitySetting.id == "Hard")
      calorieBurnMultiplier = 1.5f;
    else if (currentQualitySetting.id == "Easy")
      calorieBurnMultiplier = 0.5f;
    else if (currentQualitySetting.id == "Disabled")
      calorieBurnMultiplier = 0.0f;
    return calorieBurnMultiplier;
  }

  private class NameList
  {
    private List<string> names = new List<string>();
    private int idx;

    public NameList(TextAsset file)
    {
      foreach (string str in file.text.Replace("  ", " ").Replace("\r\n", "\n").Split('\n', StringSplitOptions.None))
      {
        string[] strArray = str.Split(' ', StringSplitOptions.None);
        if (strArray[strArray.Length - 1] != "" && strArray[strArray.Length - 1] != null)
          this.names.Add(strArray[strArray.Length - 1]);
      }
      this.names.Shuffle<string>();
    }

    public string Next() => this.names[this.idx++ % this.names.Count];
  }
}
