// Decompiled with JetBrains decompiler
// Type: NameDisplayScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NameDisplayScreen : KScreen
{
  [SerializeField]
  private float HideDistance;
  public static NameDisplayScreen Instance;
  [SerializeField]
  private Canvas nameDisplayCanvas;
  [SerializeField]
  private Canvas areaTextDisplayCanvas;
  public GameObject nameAndBarsPrefab;
  public GameObject barsPrefab;
  public TextStyleSetting ToolTipStyle_Property;
  [SerializeField]
  private Color selectedColor;
  [SerializeField]
  private Color defaultColor;
  public int fontsize_min = 14;
  public int fontsize_max = 32 /*0x20*/;
  public float cameraDistance_fontsize_min = 6f;
  public float cameraDistance_fontsize_max = 4f;
  public List<NameDisplayScreen.Entry> entries = new List<NameDisplayScreen.Entry>();
  public List<NameDisplayScreen.TextEntry> textEntries = new List<NameDisplayScreen.TextEntry>();
  public bool worldSpace = true;
  private bool isOverlayChangeBound;
  private HashedString lastKnownOverlayID = OverlayModes.None.ID;
  private int currentUpdateIndex;

  public static void DestroyInstance() => NameDisplayScreen.Instance = (NameDisplayScreen) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    NameDisplayScreen.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Health.Register(new Action<Health>(this.OnHealthAdded), (Action<Health>) null);
    Components.Equipment.Register(new Action<Equipment>(this.OnEquipmentAdded), (Action<Equipment>) null);
    this.BindOnOverlayChange();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!this.isOverlayChangeBound || !((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null))
      return;
    OverlayScreen.Instance.OnOverlayChanged -= new Action<HashedString>(this.OnOverlayChanged);
    this.isOverlayChangeBound = false;
  }

  private void BindOnOverlayChange()
  {
    if (this.isOverlayChangeBound || !((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null))
      return;
    OverlayScreen.Instance.OnOverlayChanged += new Action<HashedString>(this.OnOverlayChanged);
    this.isOverlayChangeBound = true;
  }

  public void RemoveWorldEntries(int worldId)
  {
    this.entries.RemoveAll((Predicate<NameDisplayScreen.Entry>) (entry => entry.world_go.IsNullOrDestroyed() || entry.world_go.GetMyWorldId() == worldId));
  }

  private void OnOverlayChanged(HashedString new_mode)
  {
    HashedString lastKnownOverlayId = this.lastKnownOverlayID;
    this.lastKnownOverlayID = new_mode;
    this.nameDisplayCanvas.enabled = this.lastKnownOverlayID == OverlayModes.None.ID;
  }

  private void OnHealthAdded(Health health)
  {
    this.RegisterComponent(health.gameObject, (object) health);
  }

  private void OnEquipmentAdded(Equipment equipment)
  {
    MinionAssignablesProxy component = equipment.GetComponent<MinionAssignablesProxy>();
    GameObject targetGameObject = component.GetTargetGameObject();
    if ((bool) (UnityEngine.Object) targetGameObject)
      this.RegisterComponent(targetGameObject, (object) equipment);
    else
      Debug.LogWarningFormat("OnEquipmentAdded proxy target {0} was null.", (object) component.TargetInstanceID);
  }

  private bool ShouldShowName(GameObject representedObject)
  {
    CharacterOverlay component = representedObject.GetComponent<CharacterOverlay>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.shouldShowName;
  }

  public Guid AddAreaText(string initialText, GameObject prefab)
  {
    NameDisplayScreen.TextEntry textEntry = new NameDisplayScreen.TextEntry();
    textEntry.guid = Guid.NewGuid();
    textEntry.display_go = Util.KInstantiateUI(prefab, this.areaTextDisplayCanvas.gameObject, true);
    textEntry.display_go.GetComponentInChildren<LocText>().text = initialText;
    this.textEntries.Add(textEntry);
    return textEntry.guid;
  }

  public GameObject GetWorldText(Guid guid)
  {
    GameObject worldText = (GameObject) null;
    foreach (NameDisplayScreen.TextEntry textEntry in this.textEntries)
    {
      if (textEntry.guid == guid)
      {
        worldText = textEntry.display_go;
        break;
      }
    }
    return worldText;
  }

  public void RemoveWorldText(Guid guid)
  {
    int index1 = -1;
    for (int index2 = 0; index2 < this.textEntries.Count; ++index2)
    {
      if (this.textEntries[index2].guid == guid)
      {
        index1 = index2;
        break;
      }
    }
    if (index1 < 0)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.textEntries[index1].display_go);
    this.textEntries.RemoveAt(index1);
  }

  public void AddNewEntry(GameObject representedObject)
  {
    NameDisplayScreen.Entry entry = new NameDisplayScreen.Entry();
    entry.world_go = representedObject;
    entry.world_go_anim_controller = representedObject.GetComponent<KAnimControllerBase>();
    GameObject original = this.ShouldShowName(representedObject) ? this.nameAndBarsPrefab : this.barsPrefab;
    entry.kprfabID = representedObject.GetComponent<KPrefabID>();
    entry.collider = representedObject.GetComponent<KBoxCollider2D>();
    GameObject gameObject1 = this.nameDisplayCanvas.gameObject;
    GameObject gameObject2 = Util.KInstantiateUI(original, gameObject1, true);
    entry.display_go = gameObject2;
    entry.display_go_rect = gameObject2.GetComponent<RectTransform>();
    entry.nameLabel = entry.display_go.GetComponentInChildren<LocText>();
    entry.display_go.SetActive(false);
    if (this.worldSpace)
      entry.display_go.transform.localScale = Vector3.one * 0.01f;
    gameObject2.name = representedObject.name + " character overlay";
    entry.Name = representedObject.name;
    entry.refs = gameObject2.GetComponent<HierarchyReferences>();
    this.entries.Add(entry);
    KSelectable component1 = representedObject.GetComponent<KSelectable>();
    FactionAlignment component2 = representedObject.GetComponent<FactionAlignment>();
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      if (component2.Alignment != FactionManager.FactionID.Friendly && component2.Alignment != FactionManager.FactionID.Duplicant)
        return;
      this.UpdateName(representedObject);
    }
    else
      this.UpdateName(representedObject);
  }

  public void RegisterComponent(
    GameObject representedObject,
    object component,
    bool force_new_entry = false)
  {
    NameDisplayScreen.Entry entry = force_new_entry ? (NameDisplayScreen.Entry) null : this.GetEntry(representedObject);
    if (entry == null)
    {
      CharacterOverlay component1 = representedObject.GetComponent<CharacterOverlay>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        component1.Register();
        entry = this.GetEntry(representedObject);
      }
    }
    if (entry == null)
      return;
    Transform reference = entry.refs.GetReference<Transform>("Bars");
    entry.bars_go = reference.gameObject;
    switch (component)
    {
      case Health _:
        if (!(bool) (UnityEngine.Object) entry.healthBar)
        {
          Health health = (Health) component;
          GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.healthBarPrefab, reference.gameObject);
          gameObject.name = "Health Bar";
          health.healthBar = gameObject.GetComponent<HealthBar>();
          health.healthBar.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.HEALTH.TOOLTIP;
          health.healthBar.GetComponent<KSelectableHealthBar>().IsSelectable = (UnityEngine.Object) representedObject.GetComponent<MinionBrain>() != (UnityEngine.Object) null;
          entry.healthBar = health.healthBar;
          entry.healthBar.autoHide = false;
          gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
          break;
        }
        Debug.LogWarningFormat("Health added twice {0}", component);
        break;
      case OxygenBreather _:
        if (!(bool) (UnityEngine.Object) entry.breathBar)
        {
          GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);
          entry.breathBar = gameObject.GetComponent<ProgressBar>();
          entry.breathBar.autoHide = false;
          gameObject.gameObject.GetComponent<ToolTip>().AddMultiStringTooltip("Breath", this.ToolTipStyle_Property);
          gameObject.name = "Breath Bar";
          gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BreathBar");
          gameObject.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.BREATH.TOOLTIP;
          break;
        }
        Debug.LogWarningFormat("OxygenBreather added twice {0}", component);
        break;
      case BionicOxygenTankMonitor.Instance _:
        if (!(bool) (UnityEngine.Object) entry.bionicOxygenTankBar)
        {
          GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);
          entry.bionicOxygenTankBar = gameObject.GetComponent<ProgressBar>();
          entry.bionicOxygenTankBar.autoHide = false;
          gameObject.name = "Bionic Oxygen Tank Bar";
          gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
          gameObject.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.BREATH.TOOLTIP;
          break;
        }
        Debug.LogWarningFormat("BionicOxygenTankBar added twice {0}", component);
        break;
      case Equipment _:
        if (!(bool) (UnityEngine.Object) entry.suitBar)
        {
          GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);
          entry.suitBar = gameObject.GetComponent<ProgressBar>();
          entry.suitBar.autoHide = false;
          gameObject.name = "Suit Tank Bar";
          gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
          gameObject.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.BREATH.TOOLTIP;
        }
        else
          Debug.LogWarningFormat("SuitBar added twice {0}", component);
        if (!(bool) (UnityEngine.Object) entry.suitFuelBar)
        {
          GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);
          entry.suitFuelBar = gameObject.GetComponent<ProgressBar>();
          entry.suitFuelBar.autoHide = false;
          gameObject.name = "Suit Fuel Bar";
          gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("FuelTankBar");
          gameObject.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.FUEL.TOOLTIP;
        }
        else
          Debug.LogWarningFormat("FuelBar added twice {0}", component);
        if (!(bool) (UnityEngine.Object) entry.suitBatteryBar)
        {
          GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);
          entry.suitBatteryBar = gameObject.GetComponent<ProgressBar>();
          entry.suitBatteryBar.autoHide = false;
          gameObject.name = "Suit Battery Bar";
          gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BatteryBar");
          gameObject.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.BATTERY.TOOLTIP;
          break;
        }
        Debug.LogWarningFormat("CoolantBar added twice {0}", component);
        break;
      case ThoughtGraph.Instance _:
      case CreatureThoughtGraph.Instance _:
        if (!(bool) (UnityEngine.Object) entry.thoughtBubble)
        {
          GameObject gameObject1 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubble, entry.display_go);
          entry.thoughtBubble = gameObject1.GetComponent<HierarchyReferences>();
          gameObject1.name = (component is CreatureThoughtGraph.Instance ? "Creature " : "") + "Thought Bubble";
          GameObject gameObject2 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubbleConvo, entry.display_go);
          entry.thoughtBubbleConvo = gameObject2.GetComponent<HierarchyReferences>();
          gameObject2.name = (component is CreatureThoughtGraph.Instance ? "Creature " : "") + "Thought Bubble Convo";
          break;
        }
        Debug.LogWarningFormat("ThoughtGraph added twice {0}", component);
        break;
      case GameplayEventMonitor.Instance _:
        if (!(bool) (UnityEngine.Object) entry.gameplayEventDisplay)
        {
          GameObject gameObject = Util.KInstantiateUI(EffectPrefabs.Instance.GameplayEventDisplay, entry.display_go);
          entry.gameplayEventDisplay = gameObject.GetComponent<HierarchyReferences>();
          gameObject.name = "Gameplay Event Display";
          break;
        }
        Debug.LogWarningFormat("GameplayEventDisplay added twice {0}", component);
        break;
      case Dreamer.Instance _:
        if ((bool) (UnityEngine.Object) entry.dreamBubble)
          break;
        GameObject gameObject3 = Util.KInstantiateUI(EffectPrefabs.Instance.DreamBubble, entry.display_go);
        gameObject3.name = "Dream Bubble";
        entry.dreamBubble = gameObject3.GetComponent<DreamBubble>();
        break;
    }
  }

  public bool IsVisibleToZoom()
  {
    return !((UnityEngine.Object) Game.MainCamera == (UnityEngine.Object) null) && (double) Game.MainCamera.orthographicSize < (double) this.HideDistance;
  }

  private void LateUpdate()
  {
    if (App.isLoading || App.IsExiting)
      return;
    this.BindOnOverlayChange();
    if ((UnityEngine.Object) Game.MainCamera == (UnityEngine.Object) null || this.lastKnownOverlayID != OverlayModes.None.ID)
      return;
    int count = this.entries.Count;
    int num = this.IsVisibleToZoom() ? 1 : 0;
    bool flag = num != 0 && this.lastKnownOverlayID == OverlayModes.None.ID;
    if (this.nameDisplayCanvas.enabled != flag)
      this.nameDisplayCanvas.enabled = flag;
    if (num == 0)
      return;
    this.RemoveDestroyedEntries();
    this.Culling();
    this.UpdatePos();
    this.HideDeadProgressBars();
  }

  private void Culling()
  {
    if (this.entries.Count == 0)
      return;
    Vector2I min;
    Vector2I max;
    Grid.GetVisibleCellRangeInActiveWorld(out min, out max);
    int num = Mathf.Min(500, this.entries.Count);
    for (int index = 0; index < num; ++index)
    {
      NameDisplayScreen.Entry entry = this.entries[(this.currentUpdateIndex + index) % this.entries.Count];
      Vector3 position = entry.world_go.transform.GetPosition();
      bool flag = (double) position.x >= (double) min.x && (double) position.y >= (double) min.y && (double) position.x < (double) max.x && (double) position.y < (double) max.y;
      if (entry.visible != flag)
        entry.display_go.SetActive(flag);
      entry.visible = flag;
    }
    this.currentUpdateIndex = (this.currentUpdateIndex + num) % this.entries.Count;
  }

  private void UpdatePos()
  {
    CameraController instance = CameraController.Instance;
    Transform followTarget = instance.followTarget;
    int count = this.entries.Count;
    for (int index = 0; index < count; ++index)
    {
      NameDisplayScreen.Entry entry = this.entries[index];
      if (entry.visible)
      {
        GameObject worldGo = entry.world_go;
        if (!((UnityEngine.Object) worldGo == (UnityEngine.Object) null))
        {
          Vector3 pos = worldGo.transform.GetPosition();
          if ((UnityEngine.Object) followTarget == (UnityEngine.Object) worldGo.transform)
            pos = instance.followTargetPos;
          else if ((UnityEngine.Object) entry.world_go_anim_controller != (UnityEngine.Object) null && (UnityEngine.Object) entry.collider != (UnityEngine.Object) null)
          {
            pos.x += entry.collider.offset.x;
            pos.y += entry.collider.offset.y - entry.collider.size.y / 2f;
          }
          entry.display_go_rect.anchoredPosition = (Vector2) (this.worldSpace ? pos : this.WorldToScreen(pos));
        }
      }
    }
  }

  private void RemoveDestroyedEntries()
  {
    int count = this.entries.Count;
    int index = 0;
    while (index < count)
    {
      if ((UnityEngine.Object) this.entries[index].world_go == (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.entries[index].display_go);
        --count;
        this.entries[index] = this.entries[count];
      }
      else
        ++index;
    }
    this.entries.RemoveRange(count, this.entries.Count - count);
  }

  private void HideDeadProgressBars()
  {
    int count = this.entries.Count;
    for (int index = 0; index < count; ++index)
    {
      if (this.entries[index].visible && !((UnityEngine.Object) this.entries[index].world_go == (UnityEngine.Object) null) && this.entries[index].kprfabID.HasTag(GameTags.Dead) && this.entries[index].bars_go.activeSelf)
        this.entries[index].bars_go.SetActive(false);
    }
  }

  public void UpdateName(GameObject representedObject)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(representedObject);
    if (entry == null)
      return;
    KSelectable component = representedObject.GetComponent<KSelectable>();
    entry.display_go.name = component.GetProperName() + " character overlay";
    if (!((UnityEngine.Object) entry.nameLabel != (UnityEngine.Object) null))
      return;
    entry.nameLabel.text = component.GetProperName();
    if (!((UnityEngine.Object) representedObject.GetComponent<RocketModule>() != (UnityEngine.Object) null))
      return;
    entry.nameLabel.text = representedObject.GetComponent<RocketModule>().GetParentRocketName();
  }

  public void SetDream(GameObject minion_go, Dream dream)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.dreamBubble == (UnityEngine.Object) null)
      return;
    entry.dreamBubble.SetDream(dream);
    entry.dreamBubble.GetComponent<KSelectable>().entityName = "Dreaming";
    entry.dreamBubble.gameObject.SetActive(true);
    entry.dreamBubble.SetVisibility(true);
  }

  public void StopDreaming(GameObject minion_go)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.dreamBubble == (UnityEngine.Object) null)
      return;
    entry.dreamBubble.StopDreaming();
    entry.dreamBubble.gameObject.SetActive(false);
  }

  public void DreamTick(GameObject minion_go, float dt)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.dreamBubble == (UnityEngine.Object) null)
      return;
    entry.dreamBubble.Tick(dt);
  }

  public void SetThoughtBubbleDisplay(
    GameObject minion_go,
    bool bVisible,
    string hover_text,
    Sprite bubble_sprite,
    Sprite topic_sprite)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.thoughtBubble == (UnityEngine.Object) null)
      return;
    this.ApplyThoughtSprite(entry.thoughtBubble, bubble_sprite, nameof (bubble_sprite));
    this.ApplyThoughtSprite(entry.thoughtBubble, topic_sprite, "icon_sprite");
    entry.thoughtBubble.GetComponent<KSelectable>().entityName = hover_text;
    entry.thoughtBubble.gameObject.SetActive(bVisible);
  }

  public void SetThoughtBubbleConvoDisplay(
    GameObject minion_go,
    bool bVisible,
    string hover_text,
    Sprite bubble_sprite,
    Sprite topic_sprite,
    Sprite mode_sprite)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.thoughtBubble == (UnityEngine.Object) null)
      return;
    this.ApplyThoughtSprite(entry.thoughtBubbleConvo, bubble_sprite, nameof (bubble_sprite));
    this.ApplyThoughtSprite(entry.thoughtBubbleConvo, topic_sprite, "icon_sprite");
    this.ApplyThoughtSprite(entry.thoughtBubbleConvo, mode_sprite, "icon_sprite_mode");
    entry.thoughtBubbleConvo.GetComponent<KSelectable>().entityName = hover_text;
    entry.thoughtBubbleConvo.gameObject.SetActive(bVisible);
  }

  private void ApplyThoughtSprite(HierarchyReferences active_bubble, Sprite sprite, string target)
  {
    active_bubble.GetReference<Image>(target).sprite = sprite;
  }

  public void SetGameplayEventDisplay(
    GameObject minion_go,
    bool bVisible,
    string hover_text,
    Sprite sprite)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.gameplayEventDisplay == (UnityEngine.Object) null)
      return;
    entry.gameplayEventDisplay.GetReference<Image>("icon_sprite").sprite = sprite;
    entry.gameplayEventDisplay.GetComponent<KSelectable>().entityName = hover_text;
    entry.gameplayEventDisplay.gameObject.SetActive(bVisible);
  }

  public void SetBreathDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.breathBar == (UnityEngine.Object) null)
      return;
    entry.breathBar.SetUpdateFunc(updatePercentFull);
    entry.breathBar.SetVisibility(bVisible);
  }

  public void SetHealthDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.healthBar == (UnityEngine.Object) null)
      return;
    entry.healthBar.OnChange();
    entry.healthBar.SetUpdateFunc(updatePercentFull);
    if (entry.healthBar.gameObject.activeSelf == bVisible)
      return;
    entry.healthBar.SetVisibility(bVisible);
  }

  public void SetSuitTankDisplay(
    GameObject minion_go,
    Func<float> updatePercentFull,
    bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.suitBar == (UnityEngine.Object) null)
      return;
    entry.suitBar.SetUpdateFunc(updatePercentFull);
    entry.suitBar.SetVisibility(bVisible);
  }

  public void SetBionicOxygenTankDisplay(
    GameObject minion_go,
    Func<float> updatePercentFull,
    bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.bionicOxygenTankBar == (UnityEngine.Object) null)
      return;
    entry.bionicOxygenTankBar.SetUpdateFunc(updatePercentFull);
    entry.bionicOxygenTankBar.SetVisibility(bVisible);
  }

  public void SetSuitFuelDisplay(
    GameObject minion_go,
    Func<float> updatePercentFull,
    bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.suitFuelBar == (UnityEngine.Object) null)
      return;
    entry.suitFuelBar.SetUpdateFunc(updatePercentFull);
    entry.suitFuelBar.SetVisibility(bVisible);
  }

  public void SetSuitBatteryDisplay(
    GameObject minion_go,
    Func<float> updatePercentFull,
    bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.suitBatteryBar == (UnityEngine.Object) null)
      return;
    entry.suitBatteryBar.SetUpdateFunc(updatePercentFull);
    entry.suitBatteryBar.SetVisibility(bVisible);
  }

  private NameDisplayScreen.Entry GetEntry(GameObject worldObject)
  {
    foreach (NameDisplayScreen.Entry entry in this.entries)
    {
      if ((UnityEngine.Object) entry.world_go == (UnityEngine.Object) worldObject)
        return entry;
    }
    return (NameDisplayScreen.Entry) null;
  }

  [Serializable]
  public class Entry
  {
    public string Name;
    public bool visible;
    public GameObject world_go;
    public GameObject display_go;
    public GameObject bars_go;
    public KPrefabID kprfabID;
    public KBoxCollider2D collider;
    public KAnimControllerBase world_go_anim_controller;
    public RectTransform display_go_rect;
    public LocText nameLabel;
    public HealthBar healthBar;
    public ProgressBar breathBar;
    public ProgressBar suitBar;
    public ProgressBar bionicOxygenTankBar;
    public ProgressBar suitFuelBar;
    public ProgressBar suitBatteryBar;
    public DreamBubble dreamBubble;
    public HierarchyReferences thoughtBubble;
    public HierarchyReferences thoughtBubbleConvo;
    public HierarchyReferences gameplayEventDisplay;
    public HierarchyReferences refs;
  }

  public class TextEntry
  {
    public Guid guid;
    public GameObject display_go;
  }
}
