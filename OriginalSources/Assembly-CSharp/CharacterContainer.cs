// Decompiled with JetBrains decompiler
// Type: CharacterContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class CharacterContainer : KScreen, ITelepadDeliverableContainer
{
  public const string SHUFFLE_BUTTON_DEFAULT_SOUND_NAME_ON_USE = "DupeShuffle";
  public const string SHUFFLE_BUTTON_BIONIC_SOUND_NAME_ON_USE = "DupeShuffle_bionic";
  private static readonly Dictionary<int, string> defaultShirtIdxToDefaultOutfitID = new Dictionary<int, string>()
  {
    {
      1,
      "StandardRed"
    },
    {
      2,
      "StandardBlue"
    },
    {
      3,
      "StandardYellow"
    },
    {
      4,
      "StandardGreen"
    },
    {
      5,
      "permit_standard_bionic_outfit"
    },
    {
      414842661,
      "permit_standard_regal_neutronium_outfit"
    },
    {
      890344243,
      "permit_standard_swim_outfit_red"
    }
  };
  [SerializeField]
  private GameObject contentBody;
  [SerializeField]
  private LocText characterName;
  [SerializeField]
  private EditableTitleBar characterNameTitle;
  [SerializeField]
  private LocText characterJob;
  [SerializeField]
  private LocText traitHeaderLabel;
  public GameObject selectedBorder;
  [SerializeField]
  private Image titleBar;
  [SerializeField]
  private Color selectedTitleColor;
  [SerializeField]
  private Color deselectedTitleColor;
  [SerializeField]
  private KButton reshuffleButton;
  private KBatchedAnimController animController;
  [SerializeField]
  private KBatchedAnimController bgAnimController;
  [SerializeField]
  private KBatchedAnimController fgAnimController;
  [SerializeField]
  private GameObject iconGroup;
  private List<GameObject> iconGroups;
  [SerializeField]
  private LocText goodTrait;
  [SerializeField]
  private LocText badTrait;
  [SerializeField]
  private GameObject aptitudeContainer;
  [SerializeField]
  private GameObject aptitudeEntry;
  [SerializeField]
  private Transform aptitudeLabel;
  [SerializeField]
  private Transform attributeLabelAptitude;
  [SerializeField]
  private Transform attributeLabelTrait;
  [SerializeField]
  private LocText expectationRight;
  private List<LocText> expectationLabels;
  [SerializeField]
  private DropDown archetypeDropDown;
  [SerializeField]
  private Image selectedArchetypeIcon;
  [SerializeField]
  private Sprite noArchetypeIcon;
  [SerializeField]
  private Sprite dropdownArrowIcon;
  private string guaranteedAptitudeID;
  private List<GameObject> aptitudeEntries;
  private List<GameObject> traitEntries;
  [SerializeField]
  private LocText description;
  [SerializeField]
  private Image selectedModelIcon;
  [SerializeField]
  private DropDown modelDropDown;
  [SerializeField]
  private HierarchyReferences outfitSelectorReferences;
  private List<Tag> permittedModels = new List<Tag>()
  {
    GameTags.Minions.Models.Standard,
    GameTags.Minions.Models.Bionic
  };
  [SerializeField]
  private KToggle selectButton;
  [SerializeField]
  private KBatchedAnimController fxAnim;
  private string allModelSprite = "ui_duplicant_any_selection";
  private static Dictionary<Tag, CharacterContainer.PortraitBgAnimInfo> portraitBGAnimsByModel = new Dictionary<Tag, CharacterContainer.PortraitBgAnimInfo>()
  {
    {
      GameTags.Minions.Models.Standard,
      new CharacterContainer.PortraitBgAnimInfo()
      {
        animFileName = "crewselect_backdrop_kanim",
        hasPreAnim = false,
        foregroundAnimFileName = ""
      }
    },
    {
      GameTags.Minions.Models.Bionic,
      new CharacterContainer.PortraitBgAnimInfo()
      {
        animFileName = "updated_crewSelect_bionic_backdrop_kanim",
        hasPreAnim = false,
        foregroundAnimFileName = ""
      }
    }
  };
  private MinionStartingStats stats;
  private CharacterSelectionController controller;
  private static List<CharacterContainer> containers;
  private KAnimFile idle_anim;
  [HideInInspector]
  public bool addMinionToIdentityList = true;
  [SerializeField]
  private Sprite enabledSpr;
  [SerializeField]
  private KScrollRect scroll_rect;
  private static readonly Dictionary<HashedString, string[]> traitForcedIdleAnims = new Dictionary<HashedString, string[]>()
  {
    {
      (HashedString) "character_select_swim_kanim",
      new string[2]
      {
        "GrantSkill_Swimming",
        "GrantSkill_Swimming2"
      }
    }
  };
  private static readonly Dictionary<HashedString, string[]> traitIdleAnims = new Dictionary<HashedString, string[]>()
  {
    {
      (HashedString) "anim_idle_food_kanim",
      new string[1]{ "Foodie" }
    },
    {
      (HashedString) "anim_idle_animal_lover_kanim",
      new string[1]{ "RanchingUp" }
    },
    {
      (HashedString) "anim_idle_loner_kanim",
      new string[1]{ "Loner" }
    },
    {
      (HashedString) "anim_idle_mole_hands_kanim",
      new string[1]{ "MoleHands" }
    },
    {
      (HashedString) "anim_idle_buff_kanim",
      new string[1]{ "StrongArm" }
    },
    {
      (HashedString) "anim_idle_distracted_kanim",
      new string[4]
      {
        "CantResearch",
        "CantBuild",
        "CantCook",
        "CantDig"
      }
    },
    {
      (HashedString) "anim_idle_coaster_kanim",
      new string[1]{ "HappySinger" }
    }
  };
  private List<Tag> allMinionModels = new List<Tag>()
  {
    GameTags.Minions.Models.Standard,
    GameTags.Minions.Models.Bionic
  };
  private static readonly HashedString[] idleAnims = new HashedString[6]
  {
    (HashedString) "anim_idle_healthy_kanim",
    (HashedString) "anim_idle_susceptible_kanim",
    (HashedString) "anim_idle_keener_kanim",
    (HashedString) "anim_idle_fastfeet_kanim",
    (HashedString) "anim_idle_breatherdeep_kanim",
    (HashedString) "anim_idle_breathershallow_kanim"
  };
  public float baseCharacterScale = 0.38f;
  private List<Option<ClothingOutfitTarget>> allAvailableClothingOutfits;
  private int outfitSelectorIndex;
  private bool outfitSelectorExpanded;

  public GameObject GetGameObject() => this.gameObject;

  public MinionStartingStats Stats => this.stats;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.allAvailableClothingOutfits = new List<Option<ClothingOutfitTarget>>();
    foreach (ClothingOutfitTarget clothingOutfitTarget in ClothingOutfitTarget.GetAllTemplates().Where<ClothingOutfitTarget>((Func<ClothingOutfitTarget, bool>) (outfit => outfit.OutfitType == ClothingOutfitUtility.OutfitType.Clothing)))
    {
      bool flag = false;
      foreach (string readItem in clothingOutfitTarget.ReadItems())
      {
        ClothingItemResource clothingItemResource = Db.Get().Permits.ClothingItems.TryGet(readItem);
        if (clothingItemResource != null && !clothingItemResource.IsUnlocked())
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.allAvailableClothingOutfits.Add((Option<ClothingOutfitTarget>) clothingOutfitTarget);
    }
    this.Initialize();
    this.characterNameTitle.OnStartedEditing += new System.Action(this.OnStartedEditing);
    this.characterNameTitle.OnNameChanged += new Action<string>(this.OnNameChanged);
    this.reshuffleButton.onClick += (System.Action) (() => this.Reshuffle(true));
    List<IListableOption> contentKeys1 = new List<IListableOption>();
    foreach (SkillGroup skillGroup in new List<SkillGroup>((IEnumerable<SkillGroup>) Db.Get().SkillGroups.resources))
    {
      if (skillGroup.allowAsAptitude)
        contentKeys1.Add((IListableOption) skillGroup);
    }
    this.archetypeDropDown.Initialize((IEnumerable<IListableOption>) contentKeys1, new Action<IListableOption, object>(this.OnArchetypeEntryClick), new Func<IListableOption, IListableOption, object, int>(this.archetypeDropDownSort), new Action<DropDownEntry, object>(this.archetypeDropEntryRefreshAction), false);
    this.archetypeDropDown.CustomizeEmptyRow((string) Strings.Get("STRINGS.UI.CHARACTERCONTAINER_NOARCHETYPESELECTED"), this.noArchetypeIcon);
    List<IListableOption> contentKeys2 = new List<IListableOption>();
    string name1 = (string) DUPLICANTS.MODEL.STANDARD.NAME;
    List<Tag> permittedModels1 = new List<Tag>();
    permittedModels1.Add(GameTags.Minions.Models.Standard);
    Sprite sprite1 = Assets.GetSprite((HashedString) "ui_duplicant_minion_selection");
    contentKeys2.Add((IListableOption) new CharacterContainer.MinionModelOption(name1, permittedModels1, sprite1));
    string name2 = (string) DUPLICANTS.MODEL.BIONIC.NAME;
    List<Tag> permittedModels2 = new List<Tag>();
    permittedModels2.Add(GameTags.Minions.Models.Bionic);
    Sprite sprite2 = Assets.GetSprite((HashedString) "ui_duplicant_bionicminion_selection");
    contentKeys2.Add((IListableOption) new CharacterContainer.MinionModelOption(name2, permittedModels2, sprite2));
    this.modelDropDown.Initialize((IEnumerable<IListableOption>) contentKeys2, new Action<IListableOption, object>(this.OnModelEntryClick), new Func<IListableOption, IListableOption, object, int>(this.modelDropDownSort), new Action<DropDownEntry, object>(this.modelDropEntryRefreshAction));
    this.modelDropDown.CustomizeEmptyRow((string) STRINGS.UI.CHARACTERCONTAINER_ALL_MODELS, Assets.GetSprite((HashedString) this.allModelSprite));
    this.StartCoroutine(this.DelayedGeneration());
  }

  public void ForceStopEditingTitle() => this.characterNameTitle.ForceStopEditing();

  public override float GetSortKey() => 50f;

  private IEnumerator DelayedGeneration()
  {
    yield return (object) SequenceUtil.WaitForEndOfFrame;
    this.GenerateCharacter(this.controller.IsStarterMinion);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if (!((UnityEngine.Object) this.animController != (UnityEngine.Object) null))
      return;
    this.animController.gameObject.DeleteObject();
    this.animController = (KBatchedAnimController) null;
  }

  protected override void OnForcedCleanUp()
  {
    CharacterContainer.containers.Remove(this);
    base.OnForcedCleanUp();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!((UnityEngine.Object) this.controller != (UnityEngine.Object) null))
      return;
    this.controller.OnLimitReachedEvent -= new System.Action(this.OnCharacterSelectionLimitReached);
    this.controller.OnLimitUnreachedEvent -= new System.Action(this.OnCharacterSelectionLimitUnReached);
    this.controller.OnReshuffleEvent -= new Action<bool>(this.Reshuffle);
  }

  private void Initialize()
  {
    this.iconGroups = new List<GameObject>();
    this.traitEntries = new List<GameObject>();
    this.expectationLabels = new List<LocText>();
    this.aptitudeEntries = new List<GameObject>();
    if (CharacterContainer.containers == null)
      CharacterContainer.containers = new List<CharacterContainer>();
    CharacterContainer.containers.Add(this);
  }

  private void OnNameChanged(string newName)
  {
    this.stats.Name = newName;
    this.stats.personality.Name = newName;
    this.description.text = this.stats.personality.description;
  }

  private void OnStartedEditing() => KScreenManager.Instance.RefreshStack();

  public void SetMinion(MinionStartingStats statsProposed)
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.stats))
      this.DeselectDeliverable();
    this.stats = statsProposed;
    if ((UnityEngine.Object) this.animController != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.animController.gameObject);
      this.animController = (KBatchedAnimController) null;
    }
    this.SetAnimator();
    this.SetInfoText();
    this.StartCoroutine(this.SetAttributes());
    this.selectButton.ClearOnClick();
    if (!this.controller.IsStarterMinion)
    {
      this.selectButton.enabled = true;
      this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
    }
    this.UpdateDefaultOutfitSelector(this.stats.personality);
  }

  public void GenerateCharacter(bool is_starter, string guaranteedAptitudeID = null)
  {
    int num = 0;
    do
    {
      this.stats = new MinionStartingStats(this.permittedModels, is_starter, guaranteedAptitudeID);
      ++num;
    }
    while (this.IsCharacterInvalid() && num < 20);
    if ((UnityEngine.Object) this.animController != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.animController.gameObject);
      this.animController = (KBatchedAnimController) null;
    }
    this.SetAnimator();
    this.SetInfoText();
    this.StartCoroutine(this.SetAttributes());
    this.selectButton.ClearOnClick();
    if (!this.controller.IsStarterMinion)
    {
      this.selectButton.enabled = true;
      this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
    }
    this.UpdateDefaultOutfitSelector(this.stats.personality);
  }

  private void UpdateDefaultOutfitSelector(Personality personality)
  {
    Option<ClothingOutfitTarget> selectedOutfit = ClothingOutfitTarget.TryFromTemplateId(this.stats.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.Clothing));
    this.outfitSelectorIndex = !selectedOutfit.IsSome() ? this.allAvailableClothingOutfits.FindIndex((Predicate<Option<ClothingOutfitTarget>>) (outfit => outfit.Unwrap().OutfitId == this.stats.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.Clothing))) : this.allAvailableClothingOutfits.FindIndex((Predicate<Option<ClothingOutfitTarget>>) (outfit =>
    {
      ClothingOutfitTarget clothingOutfitTarget = outfit.Unwrap();
      string outfitId1 = clothingOutfitTarget.OutfitId;
      clothingOutfitTarget = selectedOutfit.Unwrap();
      string outfitId2 = clothingOutfitTarget.OutfitId;
      return outfitId1 == outfitId2;
    }));
    if (this.outfitSelectorIndex == -1)
      this.outfitSelectorIndex = this.allAvailableClothingOutfits.FindIndex((Predicate<Option<ClothingOutfitTarget>>) (outfit => outfit.Unwrap().OutfitId == CharacterContainer.defaultShirtIdxToDefaultOutfitID[this.stats.personality.body]));
    this.RefreshOutfitSelector();
  }

  private void SetAnimator()
  {
    if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
    {
      this.animController = Util.KInstantiateUI(Assets.GetPrefab(GameTags.MinionSelectPreview), this.contentBody.gameObject).GetComponent<KBatchedAnimController>();
      this.animController.gameObject.SetActive(true);
      this.animController.animScale = this.baseCharacterScale;
    }
    BaseMinionConfig.ConfigureSymbols(this.animController.gameObject);
    this.stats.ApplyTraits(this.animController.gameObject);
    this.stats.ApplyRace(this.animController.gameObject);
    this.stats.ApplyAccessories(this.animController.gameObject);
    this.stats.ApplyOutfit(this.stats.personality, this.animController.gameObject, this.stats.GetSelectedOutfitOption());
    this.stats.ApplyJoyResponseOutfit(this.stats.personality, this.animController.gameObject);
    this.stats.ApplyExperience(this.animController.gameObject);
    this.idle_anim = Assets.GetAnim(this.GetIdleAnim(this.stats));
    if ((UnityEngine.Object) this.idle_anim != (UnityEngine.Object) null)
    {
      this.animController.AddAnimOverrides(this.idle_anim);
      this.animController.randomiseLoopedOffset = true;
    }
    KAnimFile anim1 = Assets.GetAnim(new HashedString("crewSelect_fx_kanim"));
    CharacterContainer.PortraitBgAnimInfo bgAnimInfo = this.GetBGAnimInfo(this.stats);
    KAnimFile anim2 = Assets.GetAnim((HashedString) bgAnimInfo.animFileName);
    KAnimFile anim3 = string.IsNullOrEmpty(bgAnimInfo.foregroundAnimFileName) ? (KAnimFile) null : Assets.GetAnim((HashedString) bgAnimInfo.foregroundAnimFileName);
    bool flag1 = (UnityEngine.Object) anim3 != (UnityEngine.Object) null;
    this.bgAnimController.SwapAnims(new KAnimFile[1]
    {
      anim2
    });
    if (flag1)
    {
      this.fgAnimController.gameObject.SetActive(true);
      this.fgAnimController.SwapAnims(new KAnimFile[1]
      {
        anim3
      });
    }
    else
      this.fgAnimController.gameObject.SetActive(false);
    bool flag2 = false;
    KAnimFileData data = anim2.GetData();
    if (data != null)
    {
      for (int index = 0; index < data.animCount; ++index)
      {
        KAnim.Anim anim4 = data.GetAnim(index);
        if (anim4.name.EndsWith("_pre"))
        {
          this.bgAnimController.Play((HashedString) anim4.name);
          if (flag1)
            this.fgAnimController.Play((HashedString) anim4.name);
          flag2 = true;
          break;
        }
      }
    }
    if (flag2)
    {
      this.bgAnimController.Queue((HashedString) "crewSelect_bg", KAnim.PlayMode.Loop);
      if (flag1)
        this.fgAnimController.Queue((HashedString) "crewSelect_bg", KAnim.PlayMode.Loop);
    }
    else
    {
      this.bgAnimController.Play((HashedString) "crewSelect_bg", KAnim.PlayMode.Loop);
      if (flag1)
        this.fgAnimController.Play((HashedString) "crewSelect_bg", KAnim.PlayMode.Loop);
    }
    if ((UnityEngine.Object) anim1 != (UnityEngine.Object) null)
      this.animController.AddAnimOverrides(anim1);
    this.animController.Queue((HashedString) "idle_default", KAnim.PlayMode.Loop);
  }

  private CharacterContainer.PortraitBgAnimInfo GetBGAnimInfo(
    MinionStartingStats minionStartingStats)
  {
    if (minionStartingStats.personality.model == GameTags.Minions.Models.Standard)
    {
      foreach (Trait trait in minionStartingStats.Traits)
      {
        if (trait.Id == "GrantSkill_Swimming" || trait.Id == "GrantSkill_Swimming2")
        {
          CharacterContainer.PortraitBgAnimInfo bgAnimInfo = new CharacterContainer.PortraitBgAnimInfo();
          bgAnimInfo.animFileName = "crewselect_backdrop_swim_kanim";
          bgAnimInfo.hasPreAnim = true;
          bgAnimInfo.foregroundAnimFileName = "crewselect_backdrop_swim_fg_kanim";
          bgAnimInfo = bgAnimInfo;
          return bgAnimInfo;
        }
      }
    }
    return CharacterContainer.portraitBGAnimsByModel[minionStartingStats.personality.model];
  }

  private HashedString GetIdleAnim(MinionStartingStats minionStartingStats)
  {
    List<HashedString> hashedStringList = new List<HashedString>();
    foreach (KeyValuePair<HashedString, string[]> traitForcedIdleAnim in CharacterContainer.traitForcedIdleAnims)
    {
      foreach (Trait trait in minionStartingStats.Traits)
      {
        if (((IEnumerable<string>) traitForcedIdleAnim.Value).Contains<string>(trait.Id))
          return traitForcedIdleAnim.Key;
      }
      if (((IEnumerable<string>) traitForcedIdleAnim.Value).Contains<string>(minionStartingStats.joyTrait.Id) || ((IEnumerable<string>) traitForcedIdleAnim.Value).Contains<string>(minionStartingStats.stressTrait.Id))
        return traitForcedIdleAnim.Key;
    }
    foreach (KeyValuePair<HashedString, string[]> traitIdleAnim in CharacterContainer.traitIdleAnims)
    {
      foreach (Trait trait in minionStartingStats.Traits)
      {
        if (((IEnumerable<string>) traitIdleAnim.Value).Contains<string>(trait.Id))
          hashedStringList.Add(traitIdleAnim.Key);
      }
      if (((IEnumerable<string>) traitIdleAnim.Value).Contains<string>(minionStartingStats.joyTrait.Id) || ((IEnumerable<string>) traitIdleAnim.Value).Contains<string>(minionStartingStats.stressTrait.Id))
        hashedStringList.Add(traitIdleAnim.Key);
    }
    return hashedStringList.Count > 0 ? hashedStringList.ToArray()[UnityEngine.Random.Range(0, hashedStringList.Count)] : CharacterContainer.idleAnims[UnityEngine.Random.Range(0, CharacterContainer.idleAnims.Length)];
  }

  private string GetOutfitName(int index)
  {
    return index == -1 ? (string) Strings.Get("STRINGS.UI.CHARACTERCONTAINER_NO_OUTFIT") : this.allAvailableClothingOutfits[index].Unwrap().ReadName();
  }

  private void RefreshOutfitSelector()
  {
    Image reference1 = this.outfitSelectorReferences.GetReference<Image>("CurrentOutfitIcon");
    Image reference2 = this.outfitSelectorReferences.GetReference<Image>("NextOutfitIcon");
    MultiToggle component1 = reference2.transform.parent.GetComponent<MultiToggle>();
    Image reference3 = this.outfitSelectorReferences.GetReference<Image>("PreviousOutfitIcon");
    MultiToggle component2 = reference3.transform.parent.GetComponent<MultiToggle>();
    MultiToggle reference4 = this.outfitSelectorReferences.GetReference<MultiToggle>("PreviousOutfitButton");
    MultiToggle reference5 = this.outfitSelectorReferences.GetReference<MultiToggle>("NextOutfitButton");
    RectTransform expandedMenu = this.outfitSelectorReferences.GetReference<RectTransform>("ExpandedMenu");
    MultiToggle expandButton = this.outfitSelectorReferences.GetReference<MultiToggle>("CollapsedButton");
    expandButton.onClick = (System.Action) null;
    expandButton.onClick += (System.Action) (() =>
    {
      this.outfitSelectorExpanded = !this.outfitSelectorExpanded;
      expandButton.gameObject.SetActive(!this.outfitSelectorExpanded);
      expandedMenu.gameObject.SetActive(this.outfitSelectorExpanded);
      this.RefreshOutfitSelector();
    });
    MultiToggle reference6 = this.outfitSelectorReferences.GetReference<MultiToggle>("CurrentOutfitButton");
    reference6.onClick = (System.Action) null;
    reference6.onClick += (System.Action) (() =>
    {
      this.outfitSelectorExpanded = !this.outfitSelectorExpanded;
      expandButton.gameObject.SetActive(!this.outfitSelectorExpanded);
      expandedMenu.gameObject.SetActive(this.outfitSelectorExpanded);
      this.RefreshOutfitSelector();
    });
    reference1.sprite = GetClothingIcon(0);
    expandButton.gameObject.GetComponentInChildrenOnly<Image>().sprite = reference1.sprite;
    reference2.sprite = GetClothingIcon(-1);
    reference3.sprite = GetClothingIcon(1);
    expandButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.SafeStringFormat((string) Strings.Get("STRINGS.UI.CHARACTERCONTAINER_EXPAND_OUTFIT_SELECTOR_BUTTON"), (object) this.GetOutfitName(this.outfitSelectorIndex)));
    string outfitName1 = this.GetOutfitName(this.outfitSelectorIndex);
    reference1.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip($"{outfitName1}\n\n{(string) STRINGS.UI.CHARACTERCONTAINER_CONFIRM_OUTFIT_SELECTION_TOOLTIP}");
    reference4.onClick = (System.Action) null;
    reference4.onClick += (System.Action) (() =>
    {
      this.outfitSelectorIndex = this.GetOutfitSelectorIndex(1);
      this.RefreshOutfitSelector();
      this.stats.ApplyOutfit(this.stats.personality, this.animController.gameObject, this.outfitSelectorIndex == -1 ? new Option<ClothingOutfitTarget>() : this.allAvailableClothingOutfits[this.outfitSelectorIndex]);
      if ((UnityEngine.Object) this.fxAnim != (UnityEngine.Object) null)
        this.fxAnim.Play((HashedString) "loop");
      UISounds.Instance.PlaySound3D(GlobalAssets.GetSound("DupeShuffle"));
    });
    component2.onClick = reference4.onClick;
    string outfitName2 = this.GetOutfitName(this.GetOutfitSelectorIndex(1));
    reference3.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(outfitName2);
    reference5.onClick = (System.Action) null;
    reference5.onClick += (System.Action) (() =>
    {
      this.outfitSelectorIndex = this.GetOutfitSelectorIndex(-1);
      this.RefreshOutfitSelector();
      this.stats.ApplyOutfit(this.stats.personality, this.animController.gameObject, this.outfitSelectorIndex == -1 ? new Option<ClothingOutfitTarget>() : this.allAvailableClothingOutfits[this.outfitSelectorIndex]);
      if ((UnityEngine.Object) this.fxAnim != (UnityEngine.Object) null)
        this.fxAnim.Play((HashedString) "loop");
      UISounds.Instance.PlaySound3D(GlobalAssets.GetSound("DupeShuffle"));
    });
    component1.onClick = reference5.onClick;
    string outfitName3 = this.GetOutfitName(this.GetOutfitSelectorIndex(-1));
    reference2.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(outfitName3);
    this.stats.overrideOutfitID = this.outfitSelectorIndex == -1 ? (string) null : this.allAvailableClothingOutfits[this.outfitSelectorIndex].Unwrap().OutfitId;

    Sprite GetClothingIcon(int offset)
    {
      int outfitSelectorIndex = this.GetOutfitSelectorIndex(offset);
      if (outfitSelectorIndex == -1)
        return KleiItemsUI.GetNoneClothingItemIcon(PermitCategory.DupeTops, (Option<Personality>) this.stats.personality);
      ClothingOutfitTarget clothingOutfitTarget = this.allAvailableClothingOutfits[outfitSelectorIndex].Unwrap();
      string first = clothingOutfitTarget.ReadItems().FindFirst<string>((Func<string, bool>) (item => Db.Get().Permits.ClothingItems.Get(item).Category == PermitCategory.DupeTops));
      if (first == null)
      {
        clothingOutfitTarget = this.allAvailableClothingOutfits[outfitSelectorIndex].Unwrap();
        string[] strArray = clothingOutfitTarget.ReadItems();
        if (strArray == null || strArray.Length == 0)
          return KleiItemsUI.GetNoneClothingItemIcon(PermitCategory.DupeTops, (Option<Personality>) this.stats.personality);
        first = strArray[0];
      }
      return Db.Get().Permits.ClothingItems.Get(first).GetPermitPresentationInfo().sprite;
    }
  }

  private int GetOutfitSelectorIndex(int indexOffset)
  {
    int count = this.allAvailableClothingOutfits.Count;
    int outfitSelectorIndex = this.outfitSelectorIndex + indexOffset;
    if (outfitSelectorIndex >= count)
      outfitSelectorIndex = -1;
    if (outfitSelectorIndex < -1)
      outfitSelectorIndex = count - 1;
    return outfitSelectorIndex;
  }

  private void SetInfoText()
  {
    this.traitEntries.ForEach((Action<GameObject>) (tl => UnityEngine.Object.Destroy((UnityEngine.Object) tl.gameObject)));
    this.traitEntries.Clear();
    this.characterNameTitle.SetTitle(this.stats.Name);
    this.traitHeaderLabel.SetText((string) (this.stats.personality.model == GameTags.Minions.Models.Bionic ? STRINGS.UI.CHARACTERCONTAINER_TRAITS_TITLE_BIONIC : STRINGS.UI.CHARACTERCONTAINER_TRAITS_TITLE));
    for (int index1 = 1; index1 < this.stats.Traits.Count; ++index1)
    {
      Trait trait = this.stats.Traits[index1];
      LocText locText1 = trait.PositiveTrait ? this.goodTrait : this.badTrait;
      LocText locText2 = Util.KInstantiateUI<LocText>(locText1.gameObject, locText1.transform.parent.gameObject);
      locText2.gameObject.SetActive(true);
      locText2.text = this.stats.Traits[index1].GetName();
      locText2.color = trait.PositiveTrait ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR;
      locText2.GetComponent<ToolTip>().SetSimpleTooltip(trait.GetTooltip());
      for (int index2 = 0; index2 < trait.SelfModifiers.Count; ++index2)
      {
        GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText1.transform.parent.gameObject);
        gameObject.SetActive(true);
        LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
        string format = (string) ((double) trait.SelfModifiers[index2].Value > 0.0 ? STRINGS.UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED : STRINGS.UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED);
        componentInChildren.text = string.Format(format, (object) Strings.Get($"STRINGS.DUPLICANTS.ATTRIBUTES.{trait.SelfModifiers[index2].AttributeId.ToUpper()}.NAME"));
        int num = trait.SelfModifiers[index2].AttributeId == "GermResistance" ? 1 : 0;
        Klei.AI.Attribute attrib = Db.Get().Attributes.Get(trait.SelfModifiers[index2].AttributeId);
        string message = $"{attrib.Description}\n\n{(string) Strings.Get($"STRINGS.DUPLICANTS.ATTRIBUTES.{trait.SelfModifiers[index2].AttributeId.ToUpper()}.NAME")}: {trait.SelfModifiers[index2].GetFormattedString()}";
        List<AttributeConverter> convertersForAttribute = Db.Get().AttributeConverters.GetConvertersForAttribute(attrib);
        for (int index3 = 0; index3 < convertersForAttribute.Count; ++index3)
        {
          string str = convertersForAttribute[index3].DescriptionFromAttribute(convertersForAttribute[index3].multiplier * trait.SelfModifiers[index2].Value, (GameObject) null);
          if (str != "")
            message = $"{message}\n    • {str}";
        }
        componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(message);
        this.traitEntries.Add(gameObject);
      }
      if (trait.disabledChoreGroups != null)
      {
        GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText1.transform.parent.gameObject);
        gameObject.SetActive(true);
        LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
        componentInChildren.text = trait.GetDisabledChoresString(false);
        string str1 = "";
        string str2 = "";
        for (int index4 = 0; index4 < trait.disabledChoreGroups.Length; ++index4)
        {
          if (index4 > 0)
          {
            str1 += ", ";
            str2 += "\n";
          }
          str1 += trait.disabledChoreGroups[index4].Name;
          str2 += trait.disabledChoreGroups[index4].description;
        }
        componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) DUPLICANTS.TRAITS.CANNOT_DO_TASK_TOOLTIP, (object) str1, (object) str2));
        this.traitEntries.Add(gameObject);
      }
      if (trait.ignoredEffects != null && trait.ignoredEffects.Length != 0)
      {
        GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText1.transform.parent.gameObject);
        gameObject.SetActive(true);
        LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
        componentInChildren.text = trait.GetIgnoredEffectsString(false);
        string message = "";
        for (int index5 = 0; index5 < trait.ignoredEffects.Length; ++index5)
        {
          if (index5 > 0)
            message += "\n";
          message += string.Format((string) DUPLICANTS.TRAITS.IGNORED_EFFECTS_TOOLTIP, (object) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{trait.ignoredEffects[index5].ToUpper()}.NAME"), (object) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{trait.ignoredEffects[index5].ToUpper()}.CAUSE"));
          if (index5 < trait.ignoredEffects.Length - 1)
            message += ",";
        }
        componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(message);
        this.traitEntries.Add(gameObject);
      }
      StringEntry result = (StringEntry) null;
      if (trait.ShortDescCB != null || Strings.TryGet($"STRINGS.DUPLICANTS.TRAITS.{trait.Id.ToUpper()}.SHORT_DESC", out result))
      {
        string str = trait.ShortDescCB != null ? trait.ShortDescCB() : result.String;
        string message = trait.ShortDescTooltipCB != null ? trait.ShortDescTooltipCB() : (string) Strings.Get($"STRINGS.DUPLICANTS.TRAITS.{trait.Id.ToUpper()}.SHORT_DESC_TOOLTIP");
        GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText1.transform.parent.gameObject);
        gameObject.SetActive(true);
        LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
        componentInChildren.text = str;
        componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(message);
        this.traitEntries.Add(gameObject);
      }
      this.traitEntries.Add(locText2.gameObject);
    }
    this.aptitudeEntries.ForEach((Action<GameObject>) (al => UnityEngine.Object.Destroy((UnityEngine.Object) al.gameObject)));
    this.aptitudeEntries.Clear();
    this.expectationLabels.ForEach((Action<LocText>) (el => UnityEngine.Object.Destroy((UnityEngine.Object) el.gameObject)));
    this.expectationLabels.Clear();
    if (this.stats.personality.model == GameTags.Minions.Models.Bionic)
    {
      this.aptitudeContainer.SetActive(false);
    }
    else
    {
      this.aptitudeContainer.SetActive(true);
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<SkillGroup, float> skillAptitude in this.stats.skillAptitudes)
      {
        if ((double) skillAptitude.Value != 0.0)
        {
          SkillGroup skillGroup = Db.Get().SkillGroups.Get(skillAptitude.Key.IdHash);
          if (skillGroup == null)
          {
            Debug.LogWarningFormat("Role group not found for aptitude: {0}", (object) skillAptitude.Key);
          }
          else
          {
            GameObject parent = Util.KInstantiateUI(this.aptitudeEntry.gameObject, this.aptitudeContainer);
            LocText locText3 = Util.KInstantiateUI<LocText>(this.aptitudeLabel.gameObject, parent);
            locText3.gameObject.SetActive(true);
            locText3.text = skillGroup.Name;
            string message1;
            if (skillGroup.choreGroupID != "")
            {
              ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(skillGroup.choreGroupID);
              message1 = string.Format((string) DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION_CHOREGROUP, (object) skillGroup.Name, (object) DUPLICANTSTATS.APTITUDE_BONUS, (object) choreGroup.description);
            }
            else
              message1 = string.Format((string) DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, (object) skillGroup.Name, (object) DUPLICANTSTATS.APTITUDE_BONUS);
            locText3.GetComponent<ToolTip>().SetSimpleTooltip(message1);
            string id = skillAptitude.Key.relevantAttributes[0].Id;
            float startingLevel = (float) this.stats.StartingLevels[id];
            LocText locText4 = Util.KInstantiateUI<LocText>(this.attributeLabelAptitude.gameObject, parent);
            locText4.gameObject.SetActive(!stringList.Contains(id));
            locText4.text = $"+{startingLevel.ToString()} {skillAptitude.Key.relevantAttributes[0].Name}";
            string message2 = $"{skillAptitude.Key.relevantAttributes[0].Description}\n\n{skillAptitude.Key.relevantAttributes[0].Name}: +{startingLevel.ToString()}";
            List<AttributeConverter> convertersForAttribute = Db.Get().AttributeConverters.GetConvertersForAttribute(skillAptitude.Key.relevantAttributes[0]);
            for (int index = 0; index < convertersForAttribute.Count; ++index)
              message2 = $"{message2}\n    • {convertersForAttribute[index].DescriptionFromAttribute(convertersForAttribute[index].multiplier * startingLevel, (GameObject) null)}";
            stringList.Add(id);
            locText4.GetComponent<ToolTip>().SetSimpleTooltip(message2);
            parent.gameObject.SetActive(true);
            this.aptitudeEntries.Add(parent);
          }
        }
      }
    }
    if (this.stats.stressTrait != null)
    {
      LocText locText = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject);
      locText.gameObject.SetActive(true);
      locText.text = string.Format((string) STRINGS.UI.CHARACTERCONTAINER_STRESSTRAIT, (object) this.stats.stressTrait.GetName());
      locText.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.stressTrait.GetTooltip());
      this.expectationLabels.Add(locText);
    }
    if (this.stats.joyTrait != null)
    {
      LocText locText = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject);
      locText.gameObject.SetActive(true);
      locText.text = string.Format((string) STRINGS.UI.CHARACTERCONTAINER_JOYTRAIT, (object) this.stats.joyTrait.GetName());
      locText.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.joyTrait.GetTooltip());
      this.expectationLabels.Add(locText);
    }
    this.description.text = this.stats.personality.description;
  }

  private IEnumerator SetAttributes()
  {
    yield return (object) null;
    this.iconGroups.ForEach((Action<GameObject>) (icg => UnityEngine.Object.Destroy((UnityEngine.Object) icg)));
    this.iconGroups.Clear();
    List<AttributeInstance> source = new List<AttributeInstance>((IEnumerable<AttributeInstance>) this.animController.gameObject.GetAttributes().AttributeTable);
    source.RemoveAll((Predicate<AttributeInstance>) (at => at.Attribute.ShowInUI != Klei.AI.Attribute.Display.Skill));
    List<AttributeInstance> list = source.OrderBy<AttributeInstance, string>((Func<AttributeInstance, string>) (at => at.Name)).ToList<AttributeInstance>();
    for (int index = 0; index < list.Count; ++index)
    {
      GameObject gameObject = Util.KInstantiateUI(this.iconGroup.gameObject, this.iconGroup.transform.parent.gameObject);
      LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
      gameObject.SetActive(true);
      float totalValue = list[index].GetTotalValue();
      if ((double) totalValue > 0.0)
        componentInChildren.color = Constants.POSITIVE_COLOR;
      else if ((double) totalValue == 0.0)
        componentInChildren.color = Constants.NEUTRAL_COLOR;
      else
        componentInChildren.color = Constants.NEGATIVE_COLOR;
      componentInChildren.text = string.Format((string) STRINGS.UI.CHARACTERCONTAINER_SKILL_VALUE, (object) GameUtil.AddPositiveSign(totalValue.ToString(), (double) totalValue > 0.0), (object) list[index].Name);
      AttributeInstance attributeInstance = list[index];
      string message = attributeInstance.Description;
      if (attributeInstance.Attribute.converters.Count > 0)
      {
        message += "\n";
        foreach (AttributeConverter converter1 in attributeInstance.Attribute.converters)
        {
          AttributeConverterInstance converter2 = this.animController.gameObject.GetComponent<Klei.AI.AttributeConverters>().GetConverter(converter1.Id);
          string str = converter2.DescriptionFromAttribute(converter2.Evaluate(), converter2.gameObject);
          if (str != null)
            message = $"{message}\n{str}";
        }
      }
      gameObject.GetComponent<ToolTip>().SetSimpleTooltip(message);
      this.iconGroups.Add(gameObject);
    }
  }

  public void SelectDeliverable()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.AddDeliverable((ITelepadDeliverable) this.stats);
    if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
      MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f);
    this.selectButton.GetComponent<ImageToggleState>().SetActive();
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() =>
    {
      this.DeselectDeliverable();
      if (!MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
        return;
      MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0.0f);
    });
    this.selectedBorder.SetActive(true);
    this.titleBar.color = this.selectedTitleColor;
    this.animController.Play((HashedString) "cheer_pre");
    this.animController.Play((HashedString) "cheer_loop", KAnim.PlayMode.Loop);
  }

  public void DeselectDeliverable()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.RemoveDeliverable((ITelepadDeliverable) this.stats);
    this.selectButton.GetComponent<ImageToggleState>().SetInactive();
    this.selectButton.Deselect();
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
    this.selectedBorder.SetActive(false);
    this.titleBar.color = this.deselectedTitleColor;
    this.animController.Queue((HashedString) "cheer_pst");
    this.animController.Queue((HashedString) "idle_default", KAnim.PlayMode.Loop);
  }

  private void OnReplacedEvent(ITelepadDeliverable deliverable)
  {
    if (deliverable != this.stats)
      return;
    this.DeselectDeliverable();
  }

  private void OnCharacterSelectionLimitReached()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.stats))
      return;
    this.selectButton.ClearOnClick();
    if (this.controller.AllowsReplacing)
      this.selectButton.onClick += new System.Action(this.ReplaceCharacterSelection);
    else
      this.selectButton.onClick += new System.Action(this.CantSelectCharacter);
  }

  private void CantSelectCharacter() => KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));

  private void ReplaceCharacterSelection()
  {
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
      return;
    this.controller.RemoveLast();
    this.SelectDeliverable();
  }

  private void OnCharacterSelectionLimitUnReached()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.stats))
      return;
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
  }

  public void SetReshufflingState(bool enable)
  {
    this.reshuffleButton.gameObject.SetActive(enable);
    this.archetypeDropDown.gameObject.SetActive(enable);
    this.modelDropDown.transform.parent.gameObject.SetActive(enable && Game.IsDlcActiveForCurrentSave("DLC3_ID"));
  }

  public event Action<CharacterContainer> OnReshuffled;

  public void Reshuffle(bool is_starter)
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.stats))
      this.DeselectDeliverable();
    if ((UnityEngine.Object) this.fxAnim != (UnityEngine.Object) null)
      this.fxAnim.Play((HashedString) "loop");
    this.GenerateCharacter(is_starter, this.guaranteedAptitudeID);
    Action<CharacterContainer> onReshuffled = this.OnReshuffled;
    if (onReshuffled == null)
      return;
    onReshuffled(this);
  }

  public void SetController(CharacterSelectionController csc)
  {
    if ((UnityEngine.Object) csc == (UnityEngine.Object) this.controller)
      return;
    this.controller = csc;
    this.controller.OnLimitReachedEvent += new System.Action(this.OnCharacterSelectionLimitReached);
    this.controller.OnLimitUnreachedEvent += new System.Action(this.OnCharacterSelectionLimitUnReached);
    this.controller.OnReshuffleEvent += new Action<bool>(this.Reshuffle);
    this.controller.OnReplacedEvent += new Action<ITelepadDeliverable>(this.OnReplacedEvent);
  }

  public void DisableSelectButton()
  {
    this.selectButton.soundPlayer.AcceptClickCondition = (Func<bool>) (() => false);
    this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
    this.selectButton.soundPlayer.Enabled = false;
  }

  private bool IsCharacterInvalid()
  {
    return (UnityEngine.Object) CharacterContainer.containers.Find((Predicate<CharacterContainer>) (container => (UnityEngine.Object) container != (UnityEngine.Object) null && container.stats != null && (UnityEngine.Object) container != (UnityEngine.Object) this && container.stats.personality.Id == this.stats.personality.Id && container.stats.IsValid)) != (UnityEngine.Object) null || (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null && !Game.IsDlcActiveForCurrentSave(this.stats.personality.requiredDlcId) || this.stats.personality.model != GameTags.Minions.Models.Bionic && Components.LiveMinionIdentities.Items.Any<MinionIdentity>((Func<MinionIdentity, bool>) (id => id.personalityResourceId == (HashedString) this.stats.personality.Id));
  }

  public string GetValueColor(bool isPositive)
  {
    return !isPositive ? "<color=#ff2222ff>" : "<color=green>";
  }

  public override void OnPointerEnter(PointerEventData eventData)
  {
    this.scroll_rect.mouseIsOver = true;
    base.OnPointerEnter(eventData);
  }

  public override void OnPointerExit(PointerEventData eventData)
  {
    this.scroll_rect.mouseIsOver = false;
    base.OnPointerExit(eventData);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.IsAction(Action.Escape) || e.IsAction(Action.MouseRight))
    {
      this.characterNameTitle.ForceStopEditing();
      this.controller.OnPressBack();
      this.archetypeDropDown.scrollRect.gameObject.SetActive(false);
    }
    if (KInputManager.currentControllerIsGamepad)
    {
      if (this.archetypeDropDown.scrollRect.activeInHierarchy)
      {
        KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
        Vector2 point = (Vector2) component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
        UnityEngine.Rect rect = component.rectTransform().rect;
        component.mouseIsOver = rect.Contains(point);
        component.OnKeyDown(e);
      }
      else
        this.scroll_rect.OnKeyDown(e);
    }
    else
      e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (KInputManager.currentControllerIsGamepad)
    {
      if (this.archetypeDropDown.scrollRect.activeInHierarchy)
      {
        KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
        Vector2 point = (Vector2) component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
        UnityEngine.Rect rect = component.rectTransform().rect;
        component.mouseIsOver = rect.Contains(point);
        component.OnKeyUp(e);
      }
      else
        this.scroll_rect.OnKeyUp(e);
    }
    else
      e.Consumed = true;
  }

  protected override void OnCmpEnable()
  {
    this.OnActivate();
    if (this.stats == null)
      return;
    this.SetAnimator();
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.characterNameTitle.ForceStopEditing();
  }

  private void OnArchetypeEntryClick(IListableOption skill, object data)
  {
    if (skill != null)
    {
      SkillGroup skillGroup = skill as SkillGroup;
      this.guaranteedAptitudeID = skillGroup.Id;
      this.selectedArchetypeIcon.sprite = Assets.GetSprite((HashedString) skillGroup.archetypeIcon);
      this.Reshuffle(true);
    }
    else
    {
      this.guaranteedAptitudeID = (string) null;
      this.selectedArchetypeIcon.sprite = this.dropdownArrowIcon;
      this.Reshuffle(true);
    }
  }

  private int archetypeDropDownSort(IListableOption a, IListableOption b, object targetData)
  {
    return b.Equals((object) "Random") ? -1 : b.GetProperName().CompareTo(a.GetProperName());
  }

  private void archetypeDropEntryRefreshAction(DropDownEntry entry, object targetData)
  {
    if (entry.entryData == null)
      return;
    SkillGroup entryData = entry.entryData as SkillGroup;
    entry.image.sprite = Assets.GetSprite((HashedString) entryData.archetypeIcon);
  }

  private void OnModelEntryClick(IListableOption listItem, object data)
  {
    bool flag = false;
    if (listItem == null)
    {
      this.permittedModels = this.allMinionModels;
      this.selectedModelIcon.sprite = Assets.GetSprite((HashedString) this.allModelSprite);
      this.Reshuffle(true);
    }
    else if (listItem is CharacterContainer.MinionModelOption minionModelOption)
    {
      flag = minionModelOption.permittedModels.Count == 1 && minionModelOption.permittedModels[0] == GameTags.Minions.Models.Bionic;
      this.permittedModels = minionModelOption.permittedModels;
      this.selectedModelIcon.sprite = minionModelOption.sprite;
      this.Reshuffle(true);
    }
    this.reshuffleButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = flag ? "DupeShuffle_bionic" : "DupeShuffle";
  }

  private int modelDropDownSort(IListableOption a, IListableOption b, object targetData)
  {
    return a.GetProperName().CompareTo(b.GetProperName());
  }

  private void modelDropEntryRefreshAction(DropDownEntry entry, object targetData)
  {
    if (entry.entryData == null)
      return;
    CharacterContainer.MinionModelOption entryData = entry.entryData as CharacterContainer.MinionModelOption;
    entry.image.sprite = entryData.sprite;
  }

  [Serializable]
  public struct ProfessionIcon
  {
    public string professionName;
    public Sprite iconImg;
  }

  private struct PortraitBgAnimInfo
  {
    public string animFileName;
    public string foregroundAnimFileName;
    public bool hasPreAnim;
  }

  private class MinionModelOption : IListableOption
  {
    private string properName;
    public List<Tag> permittedModels;
    public Sprite sprite;

    public MinionModelOption(string name, List<Tag> permittedModels, Sprite sprite)
    {
      this.properName = name;
      this.permittedModels = permittedModels;
      this.sprite = sprite;
    }

    public string GetProperName() => this.properName;
  }
}
