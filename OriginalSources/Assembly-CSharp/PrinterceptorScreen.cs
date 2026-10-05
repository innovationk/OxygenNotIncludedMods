// Decompiled with JetBrains decompiler
// Type: PrinterceptorScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PrinterceptorScreen : KModalScreen
{
  public static PrinterceptorScreen Instance;
  [SerializeField]
  private RectTransform optionGridContainer;
  [SerializeField]
  private GameObject optionButtonPrefab;
  [SerializeField]
  private LocText selectedTitleText;
  [SerializeField]
  private Image selectedIcon;
  [SerializeField]
  private Image selectedIconAlt;
  [SerializeField]
  private LocText selectedEffectsText;
  [SerializeField]
  private LocText selectedFlavourText;
  [SerializeField]
  private KButton printButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private LocText dataWalletLabel;
  [SerializeField]
  private Image[] dataWalletIcon;
  [SerializeField]
  private LocText selectedCostLabel;
  [SerializeField]
  private Image selectedCostIcon;
  private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";
  private const string MUSIC_PARAMETER = "SupplyClosetView";
  [SerializeField]
  private Material desatUIMaterial;
  private HijackedHeadquarters.Instance target;
  private Dictionary<Tag, MultiToggle> optionButtons = new Dictionary<Tag, MultiToggle>();

  public Tag selectedEntityTag { get; private set; }

  protected override void OnActivate()
  {
    PrinterceptorScreen.Instance = this;
    this.Show(false);
    this.closeButton.ClearOnClick();
    this.closeButton.onClick += (System.Action) (() => this.Show(false));
  }

  public void SetTarget(HijackedHeadquarters.Instance target)
  {
    this.target = target;
    this.printButton.ClearOnClick();
    this.printButton.onClick += (System.Action) (() =>
    {
      target.Trigger(1816718186);
      this.Show(false);
    });
  }

  public override float GetSortKey() => 40f;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  public override void Show(bool show = true)
  {
    base.Show(show);
    if (show)
    {
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
      MusicManager.instance.OnSupplyClosetMenu(true, 0.5f);
      MusicManager.instance.PlaySong("Music_SupplyCloset");
      foreach (Image image in this.dataWalletIcon)
        image.sprite = Def.GetUISprite((object) DatabankHelper.ID).first;
      this.dataWalletLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.PRINTERCEPTORSCREEN.DATABANKS_AVAILABLE, (object) this.target.GetComponent<Storage>().GetAmountAvailable((Tag) DatabankHelper.ID).ToString()));
      this.SelectEntity(this.selectedEntityTag);
      foreach (KeyValuePair<Tag, MultiToggle> optionButton in this.optionButtons)
        this.RefreshOptionButton(optionButton.Key);
    }
    else
    {
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
      MusicManager.instance.OnSupplyClosetMenu(false, 1f);
      if (!MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
        return;
      MusicManager.instance.StopSong("Music_SupplyCloset");
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SpawnOptionButtons();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Show(false);
    else
      base.OnKeyDown(e);
  }

  public override void Deactivate() => this.Show(false);

  private void SpawnOptionButtons()
  {
    foreach (KeyValuePair<Tag, List<EggCrackerConfig.EggData>> eggsBySpecy in EggCrackerConfig.EggsBySpecies)
    {
      foreach (EggCrackerConfig.EggData eggData in eggsBySpecy.Value)
      {
        if (eggData.isBaseMorph)
          this.SpawnOptionButton(eggData.id);
      }
    }
    List<Tag> tagList = new List<Tag>();
    tagList.AddRange(Assets.GetPrefabsWithTag(GameTags.Seed).Select<GameObject, Tag>((Func<GameObject, Tag>) (x => x.GetComponent<KPrefabID>().PrefabTag)));
    tagList.AddRange(Assets.GetPrefabsWithTag(GameTags.CropSeed).Select<GameObject, Tag>((Func<GameObject, Tag>) (x => x.GetComponent<KPrefabID>().PrefabTag)));
    foreach (Tag id in tagList)
      this.SpawnOptionButton(id);
    this.SelectEntity((Tag) "SquirrelEgg");
    this.SpawnOptionButton((Tag) "BeeBaby");
  }

  private void SpawnOptionButton(Tag id)
  {
    if (this.optionButtons.ContainsKey(id))
      return;
    GameObject prefab1 = Assets.TryGetPrefab(id);
    if ((UnityEngine.Object) prefab1 == (UnityEngine.Object) null || !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) prefab1.GetComponent<KPrefabID>()) || prefab1.HasTag(GameTags.DeprecatedContent))
      return;
    PlantableSeed component1 = prefab1.GetComponent<PlantableSeed>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      GameObject prefab2 = Assets.GetPrefab(component1.PlantID);
      if ((UnityEngine.Object) prefab2 != (UnityEngine.Object) null && prefab2.HasTag(GameTags.DeprecatedContent))
        return;
    }
    GameObject gameObject = Util.KInstantiateUI(this.optionButtonPrefab, this.optionGridContainer.gameObject, true);
    MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
    this.optionButtons.Add(id, component2);
    HierarchyReferences component3 = gameObject.GetComponent<HierarchyReferences>();
    component2.onClick += (System.Action) (() => this.SelectEntity(id));
    component3.GetReference<Image>("FGIcon").sprite = Def.GetUISprite((object) id).first;
    component3.GetReference<LocText>("NameLabel").text = id.ProperName();
    component3.GetReference<Image>("ProgressOverlay").fillAmount = 0.0f;
    component3.GetReference<LocText>("CostLabel").text = HijackedHeadquartersConfig.GetDataBankCost(id, this.GetPrintCount(id)).ToString();
    component3.GetReference<Image>("CostIcon").sprite = Def.GetUISprite((object) DatabankHelper.ID).first;
  }

  private void RefreshOptionButton(Tag id)
  {
    this.optionButtons[id].GetComponent<HierarchyReferences>().GetReference<LocText>("CostLabel").text = HijackedHeadquartersConfig.GetDataBankCost(id, this.GetPrintCount(id)).ToString();
  }

  private void SelectEntity(Tag id)
  {
    this.selectedEntityTag = id;
    GameObject prefab = Assets.GetPrefab(this.selectedEntityTag);
    this.selectedEffectsText.text = prefab.GetComponent<InfoDescription>().description;
    this.selectedTitleText.text = prefab.GetProperName();
    this.selectedIcon.sprite = Def.GetUISprite((object) this.selectedEntityTag).first;
    this.selectedIconAlt.sprite = !prefab.HasTag(GameTags.Egg) ? (!prefab.HasTag(GameTags.Seed) ? (!prefab.HasTag(GameTags.CropSeed) ? (!prefab.HasTag(GameTags.Creature) ? (Sprite) null : Def.GetUISprite((object) prefab.GetComponent<CreatureBrain>().species).first) : Def.GetUISprite((object) prefab.GetComponent<PlantableSeed>().PlantID).first) : Def.GetUISprite((object) prefab.GetComponent<PlantableSeed>().PlantID).first) : Def.GetUISprite((object) prefab.GetDef<IncubationMonitor.Def>().spawnedCreature).first;
    foreach (KeyValuePair<Tag, MultiToggle> optionButton in this.optionButtons)
      optionButton.Value.GetComponent<MultiToggle>().ChangeState(this.selectedEntityTag == optionButton.Key ? 1 : 0);
    this.selectedCostIcon.sprite = Def.GetUISprite((object) DatabankHelper.ID).first;
    this.selectedCostLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.PRINTERCEPTORSCREEN.DATABANKS_COST, (object) HijackedHeadquartersConfig.GetDataBankCost(this.selectedEntityTag, this.GetPrintCount(this.selectedEntityTag)).ToString()));
    this.printButton.isInteractable = this.target != null && (double) this.target.GetComponent<Storage>().GetAmountAvailable((Tag) DatabankHelper.ID) >= (double) HijackedHeadquartersConfig.GetDataBankCost(this.selectedEntityTag, this.GetPrintCount(this.selectedEntityTag));
    ToolTip component = this.printButton.GetComponent<ToolTip>();
    string message;
    if (!this.printButton.isInteractable)
      message = (string) STRINGS.UI.PRINTERCEPTORSCREEN.PRINT_TOOLTIP_DISABLED;
    else
      message = GameUtil.SafeStringFormat((string) STRINGS.UI.PRINTERCEPTORSCREEN.PRINT_TOOLTIP, (object) 25);
    component.SetSimpleTooltip(message);
  }

  private int GetPrintCount(Tag id)
  {
    return this.target == null || !this.target.printCounts.ContainsKey(id) ? 0 : this.target.printCounts[id];
  }
}
