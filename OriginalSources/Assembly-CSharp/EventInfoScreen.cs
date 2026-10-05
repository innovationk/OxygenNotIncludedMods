// Decompiled with JetBrains decompiler
// Type: EventInfoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#nullable disable
public class EventInfoScreen : KModalScreen
{
  [SerializeField]
  private float baseCharacterScale = 0.0057f;
  [FormerlySerializedAs("midgroundPrefab")]
  [FormerlySerializedAs("mid")]
  [Header("Prefabs")]
  [SerializeField]
  private GameObject animPrefab;
  [SerializeField]
  private GameObject optionPrefab;
  [SerializeField]
  private GameObject optionIconPrefab;
  [SerializeField]
  private GameObject optionTextPrefab;
  [Header("Groups")]
  [SerializeField]
  private Transform artSection;
  [SerializeField]
  private Transform midgroundGroup;
  [SerializeField]
  private GameObject timeGroup;
  [SerializeField]
  private GameObject buttonsGroup;
  [SerializeField]
  private GameObject chainGroup;
  [Header("Text")]
  [SerializeField]
  private LocText eventHeader;
  [SerializeField]
  private LocText eventTimeLabel;
  [SerializeField]
  private LocText eventLocationLabel;
  [SerializeField]
  private LocText eventDescriptionLabel;
  [SerializeField]
  private bool loadMinionFromPersonalities = true;
  [SerializeField]
  private LocText chainCount;
  [Header("Button Colour Styles")]
  [SerializeField]
  private ColorStyleSetting neutralButtonSetting;
  [SerializeField]
  private ColorStyleSetting badButtonSetting;
  [SerializeField]
  private ColorStyleSetting goodButtonSetting;
  private List<KBatchedAnimController> createdAnimations = new List<KBatchedAnimController>();

  public override bool IsModal() => true;

  public void SetEventData(EventInfoData data)
  {
    data.FinalizeText();
    this.eventHeader.text = data.title;
    this.eventDescriptionLabel.text = data.description;
    this.eventLocationLabel.text = data.location;
    this.eventTimeLabel.text = data.whenDescription;
    if (data.location.IsNullOrWhiteSpace() && data.location.IsNullOrWhiteSpace())
      this.timeGroup.gameObject.SetActive(false);
    if (data.options.Count == 0)
      data.AddDefaultOption();
    this.artSection.gameObject.SetActive(data.animFileName != HashedString.Invalid);
    this.SetEventDataOptions(data);
    this.SetEventDataVisuals(data);
  }

  private void SetEventDataOptions(EventInfoData data)
  {
    foreach (EventInfoData.Option option1 in data.options)
    {
      EventInfoData.Option option = option1;
      GameObject gameObject = Util.KInstantiateUI(this.optionPrefab, this.buttonsGroup);
      gameObject.name = "Option: " + option.mainText;
      KButton component1 = gameObject.GetComponent<KButton>();
      component1.isInteractable = option.allowed;
      component1.onClick += (System.Action) (() =>
      {
        if (option.callback != null)
          option.callback();
        this.Deactivate();
      });
      if (!option.tooltip.IsNullOrWhiteSpace())
        gameObject.GetComponent<ToolTip>().SetSimpleTooltip(option.tooltip);
      else
        gameObject.GetComponent<ToolTip>().enabled = false;
      foreach (EventInfoData.OptionIcon informationIcon in option.informationIcons)
        this.CreateOptionIcon(gameObject, informationIcon);
      LocText component2 = Util.KInstantiateUI(this.optionTextPrefab, gameObject).GetComponent<LocText>();
      string str;
      if (option.description != null)
        str = $"<b>{option.mainText}</b>\n<i>({option.description})</i>";
      else
        str = $"<b>{option.mainText}</b>";
      component2.text = str;
      foreach (EventInfoData.OptionIcon consequenceIcon in option.consequenceIcons)
        this.CreateOptionIcon(gameObject, consequenceIcon);
      gameObject.SetActive(true);
    }
  }

  public override void Deactivate()
  {
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().EventPopupSnapshot);
    base.Deactivate();
  }

  private void CreateOptionIcon(GameObject option, EventInfoData.OptionIcon optionIcon)
  {
    GameObject gameObject = Util.KInstantiateUI(this.optionIconPrefab, option);
    gameObject.GetComponent<ToolTip>().SetSimpleTooltip(optionIcon.tooltip);
    HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
    Image reference1 = component.GetReference<Image>("Mask");
    Image reference2 = component.GetReference<Image>("Border");
    Image reference3 = component.GetReference<Image>("Icon");
    if ((UnityEngine.Object) optionIcon.sprite != (UnityEngine.Object) null)
      reference3.transform.localScale *= optionIcon.scale;
    Color32 color32 = (Color32) Color.white;
    switch (optionIcon.containerType)
    {
      case EventInfoData.OptionIcon.ContainerType.Neutral:
        reference1.sprite = Assets.GetSprite((HashedString) "container_fill_neutral");
        reference2.sprite = Assets.GetSprite((HashedString) "container_border_neutral");
        if ((UnityEngine.Object) optionIcon.sprite == (UnityEngine.Object) null)
          optionIcon.sprite = Assets.GetSprite((HashedString) "knob");
        color32 = GlobalAssets.Instance.colorSet.eventNeutral;
        break;
      case EventInfoData.OptionIcon.ContainerType.Positive:
        reference1.sprite = Assets.GetSprite((HashedString) "container_fill_positive");
        reference2.sprite = Assets.GetSprite((HashedString) "container_border_positive");
        RectTransform rectTransform1 = reference3.rectTransform;
        rectTransform1.localPosition = rectTransform1.localPosition + Vector3.down * 1f;
        if ((UnityEngine.Object) optionIcon.sprite == (UnityEngine.Object) null)
          optionIcon.sprite = Assets.GetSprite((HashedString) "icon_positive");
        color32 = GlobalAssets.Instance.colorSet.eventPositive;
        break;
      case EventInfoData.OptionIcon.ContainerType.Negative:
        reference1.sprite = Assets.GetSprite((HashedString) "container_fill_negative");
        reference2.sprite = Assets.GetSprite((HashedString) "container_border_negative");
        RectTransform rectTransform2 = reference3.rectTransform;
        rectTransform2.localPosition = rectTransform2.localPosition + Vector3.up * 1f;
        color32 = GlobalAssets.Instance.colorSet.eventNegative;
        if ((UnityEngine.Object) optionIcon.sprite == (UnityEngine.Object) null)
        {
          optionIcon.sprite = Assets.GetSprite((HashedString) "cancel");
          break;
        }
        break;
      case EventInfoData.OptionIcon.ContainerType.Information:
        reference1.sprite = Assets.GetSprite((HashedString) "requirements");
        reference2.enabled = false;
        break;
    }
    reference1.color = (Color) color32;
    reference3.sprite = optionIcon.sprite;
    if (!((UnityEngine.Object) optionIcon.sprite == (UnityEngine.Object) null))
      return;
    reference3.gameObject.SetActive(false);
  }

  private void SetEventDataVisuals(EventInfoData data)
  {
    this.createdAnimations.ForEach((Action<KBatchedAnimController>) (x => UnityEngine.Object.Destroy((UnityEngine.Object) x)));
    this.createdAnimations.Clear();
    KAnimFile anim = Assets.GetAnim(data.animFileName);
    if ((UnityEngine.Object) anim == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) $"Event {data.title} has no anim data");
    }
    else
    {
      KBatchedAnimController component = this.CreateAnimLayer(this.midgroundGroup, anim, data.mainAnim).transform.GetComponent<KBatchedAnimController>();
      if (data.minions != null)
      {
        for (int index = 0; index < data.minions.Length; ++index)
        {
          if ((UnityEngine.Object) data.minions[index] == (UnityEngine.Object) null)
            DebugUtil.LogWarningArgs((object) $"EventInfoScreen unable to display minion {index}");
          string str = $"dupe{index + 1:D2}";
          if (component.HasAnimation((HashedString) str))
            this.CreateAnimLayer(this.midgroundGroup, anim, (HashedString) str, data.minions[index]);
        }
      }
      if (!((UnityEngine.Object) data.artifact != (UnityEngine.Object) null))
        return;
      string str1 = "artifact";
      if (!component.HasAnimation((HashedString) str1))
        return;
      this.CreateAnimLayer(this.midgroundGroup, anim, (HashedString) str1, artifact: data.artifact);
    }
  }

  private GameObject CreateAnimLayer(
    Transform parent,
    KAnimFile animFile,
    HashedString animName,
    GameObject minion = null,
    GameObject artifact = null,
    string targetSymbol = null)
  {
    GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.animPrefab, parent);
    KBatchedAnimController component1 = go.GetComponent<KBatchedAnimController>();
    this.createdAnimations.Add(component1);
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
      component1.AnimFiles = new KAnimFile[4]
      {
        Assets.GetAnim((HashedString) "body_comp_default_kanim"),
        Assets.GetAnim((HashedString) "head_swap_kanim"),
        Assets.GetAnim((HashedString) "body_swap_kanim"),
        animFile
      };
    else
      component1.AnimFiles = new KAnimFile[1]{ animFile };
    go.SetActive(true);
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
    {
      if (this.loadMinionFromPersonalities)
      {
        component1.GetComponent<UIDupeSymbolOverride>().Apply(minion.GetComponent<MinionIdentity>());
      }
      else
      {
        SymbolOverrideController component2 = component1.GetComponent<SymbolOverrideController>();
        foreach (SymbolOverrideController.SymbolEntry getSymbolOverride in minion.GetComponent<SymbolOverrideController>().GetSymbolOverrides)
          component2.AddSymbolOverride(getSymbolOverride.targetSymbol, getSymbolOverride.sourceSymbol, getSymbolOverride.priority);
      }
      BaseMinionConfig.CopyVisibleSymbols(go, minion);
    }
    if ((UnityEngine.Object) artifact != (UnityEngine.Object) null)
    {
      SymbolOverrideController component3 = component1.GetComponent<SymbolOverrideController>();
      KBatchedAnimController component4 = artifact.GetComponent<KBatchedAnimController>();
      string symbol_name = component4.initialAnim.Replace("idle_", "artifact_").Replace("_loop", "");
      KAnim.Build.Symbol symbol = component4.AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) symbol_name);
      if (symbol != null)
        component3.AddSymbolOverride((HashedString) "snapTo_artifact", symbol);
    }
    if (targetSymbol != null)
      go.AddOrGet<KBatchedAnimTracker>().symbol = (HashedString) targetSymbol;
    component1.Play(animName, KAnim.PlayMode.Loop);
    component1.animScale = this.baseCharacterScale;
    return go;
  }

  public static EventInfoScreen ShowPopup(EventInfoData eventInfoData)
  {
    EventInfoScreen eventInfoScreen = (EventInfoScreen) KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.eventInfoScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
    eventInfoScreen.SetEventData(eventInfoData);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().EventPopupSnapshot);
    KFMOD.PlayUISound(GlobalAssets.GetSound("StoryTrait_Activation_Popup_short"));
    if (eventInfoData.showCallback != null)
      eventInfoData.showCallback();
    if (!((UnityEngine.Object) eventInfoData.clickFocus != (UnityEngine.Object) null))
      return eventInfoScreen;
    WorldContainer myWorld = eventInfoData.clickFocus.gameObject.GetMyWorld();
    if (!((UnityEngine.Object) myWorld != (UnityEngine.Object) null) || !myWorld.IsDiscovered)
      return eventInfoScreen;
    GameUtil.FocusCameraOnWorld(myWorld.id, eventInfoData.clickFocus.position);
    return eventInfoScreen;
  }

  public static Notification CreateNotification(
    EventInfoData eventInfoData,
    Notification.ClickCallback clickCallback = null)
  {
    if (eventInfoData == null)
    {
      DebugUtil.LogWarningArgs((object) "eventPopup is null in CreateStandardEventNotification");
      return (Notification) null;
    }
    eventInfoData.FinalizeText();
    return new Notification(eventInfoData.title, NotificationType.Event, expires: false, click_focus: eventInfoData.clickFocus)
    {
      customClickCallback = clickCallback != null ? clickCallback : (Notification.ClickCallback) (data => EventInfoScreen.ShowPopup(eventInfoData))
    };
  }
}
