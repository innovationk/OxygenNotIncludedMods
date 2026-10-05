// Decompiled with JetBrains decompiler
// Type: CarePackageContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CarePackageContainer : KScreen, ITelepadDeliverableContainer
{
  [Header("UI References")]
  [SerializeField]
  private GameObject contentBody;
  [SerializeField]
  private LocText characterName;
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
  private LocText itemName;
  [SerializeField]
  private LocText quantity;
  [SerializeField]
  private LocText currentQuantity;
  [SerializeField]
  private LocText description;
  [SerializeField]
  private LocText effects;
  [SerializeField]
  private KToggle selectButton;
  private CarePackageInfo info;
  public CarePackageContainer.CarePackageInstanceData carePackageInstanceData;
  private CharacterSelectionController controller;
  private static List<ITelepadDeliverableContainer> containers;
  [SerializeField]
  private Sprite enabledSpr;
  [SerializeField]
  private List<CarePackageContainer.ProfessionIcon> professionIcons;
  private Dictionary<string, Sprite> professionIconMap;
  public float baseCharacterScale = 0.38f;
  private List<GameObject> entryIcons = new List<GameObject>();

  public GameObject GetGameObject() => this.gameObject;

  public CarePackageInfo Info => this.info;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Initialize();
    this.StartCoroutine(this.DelayedGeneration());
  }

  public override float GetSortKey() => 50f;

  private IEnumerator DelayedGeneration()
  {
    yield return (object) SequenceUtil.WaitForEndOfFrame;
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
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
    this.professionIconMap = new Dictionary<string, Sprite>();
    this.professionIcons.ForEach((Action<CarePackageContainer.ProfessionIcon>) (ic => this.professionIconMap.Add(ic.professionName, ic.iconImg)));
    if (CarePackageContainer.containers == null)
      CarePackageContainer.containers = new List<ITelepadDeliverableContainer>();
    CarePackageContainer.containers.Add((ITelepadDeliverableContainer) this);
  }

  private void GenerateCharacter(bool is_starter)
  {
    int num = 0;
    do
    {
      this.info = Immigration.Instance.RandomCarePackage();
      ++num;
    }
    while (this.IsCharacterRedundant() && num < 20);
    if ((UnityEngine.Object) this.animController != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.animController.gameObject);
      this.animController = (KBatchedAnimController) null;
    }
    this.carePackageInstanceData = new CarePackageContainer.CarePackageInstanceData();
    this.carePackageInstanceData.info = this.info;
    this.carePackageInstanceData.facadeID = !(this.info.facadeID == "SELECTRANDOM") ? this.info.facadeID : Db.GetEquippableFacades().resources.FindAll((Predicate<EquippableFacadeResource>) (match => match.DefID == this.info.id)).GetRandom<EquippableFacadeResource>().Id;
    this.SetAnimator();
    this.SetInfoText();
    this.selectButton.ClearOnClick();
    if (this.controller.IsStarterMinion)
      return;
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
  }

  private void SetAnimator()
  {
    GameObject prefab = Assets.GetPrefab(this.info.id.ToTag());
    EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.info.id);
    int num1 = ElementLoader.FindElementByName(this.info.id) == null ? (foodInfo == null || (double) foodInfo.CaloriesPerUnit <= 0.0 ? (int) this.info.quantity : (int) Mathf.Max(1f, this.info.quantity % foodInfo.CaloriesPerUnit)) : 1;
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      for (int index = 0; index < num1; ++index)
      {
        GameObject gameObject = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject);
        gameObject.SetActive(true);
        Image component = gameObject.GetComponent<Image>();
        Tuple<Sprite, Color> tuple = this.carePackageInstanceData.facadeID.IsNullOrWhiteSpace() ? Def.GetUISprite((object) prefab) : Def.GetUISprite(prefab.PrefabID(), this.carePackageInstanceData.facadeID);
        component.sprite = tuple.first;
        component.color = tuple.second;
        this.entryIcons.Add(gameObject);
        if (num1 > 1)
        {
          int num2;
          int num3;
          int num4;
          if (num1 % 2 == 1)
          {
            num2 = Mathf.CeilToInt((float) (num1 / 2));
            int num5 = num2 - index;
            num3 = num5 > 0 ? 1 : -1;
            num4 = Mathf.Abs(num5);
          }
          else
          {
            num2 = num1 / 2 - 1;
            if (index <= num2)
            {
              num4 = Mathf.Abs(num2 - index);
              num3 = -1;
            }
            else
            {
              num4 = Mathf.Abs(num2 + 1 - index);
              num3 = 1;
            }
          }
          int x = 0;
          if (num1 % 2 == 0)
          {
            x = index <= num2 ? -6 : 6;
            gameObject.transform.SetPosition(gameObject.transform.position += new Vector3((float) x, 0.0f, 0.0f));
          }
          gameObject.transform.localScale = new Vector3((float) (1.0 - (double) num4 * 0.10000000149011612), (float) (1.0 - (double) num4 * 0.10000000149011612), 1f);
          gameObject.transform.Rotate(0.0f, 0.0f, 3f * (float) num4 * (float) num3);
          gameObject.transform.SetPosition(gameObject.transform.position + new Vector3(25f * (float) num4 * (float) num3, 5f * (float) num4) + new Vector3((float) x, 0.0f, 0.0f));
          gameObject.GetComponent<Canvas>().sortingOrder = num1 - num4;
        }
      }
    }
    else
    {
      GameObject gameObject = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject);
      gameObject.SetActive(true);
      Image component = gameObject.GetComponent<Image>();
      component.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(this.info.id.ToTag()).substance.anim);
      component.color = (Color) ElementLoader.GetElement(this.info.id.ToTag()).substance.uiColour;
      this.entryIcons.Add(gameObject);
    }
  }

  private string GetSpawnableName()
  {
    GameObject prefab = Assets.GetPrefab((Tag) this.info.id);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
    {
      Element elementByName = ElementLoader.FindElementByName(this.info.id);
      return elementByName != null ? elementByName.substance.name : "";
    }
    return string.IsNullOrEmpty(this.carePackageInstanceData.facadeID) ? prefab.GetProperName() : EquippableFacade.GetNameOverride(this.carePackageInstanceData.info.id, this.carePackageInstanceData.facadeID);
  }

  private string GetSpawnableQuantityOnly()
  {
    if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, (object) GameUtil.GetFormattedMass(this.info.quantity));
    EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.info.id);
    return foodInfo != null && (double) foodInfo.CaloriesPerUnit > 0.0 ? string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, (object) GameUtil.GetFormattedCaloriesForItem((Tag) this.info.id, this.info.quantity)) : string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, (object) this.info.quantity.ToString());
  }

  private string GetCurrentQuantity(WorldInventory inventory)
  {
    if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
    {
      float amount = inventory.GetAmount(this.info.id.ToTag(), false);
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, (object) GameUtil.GetFormattedMass(amount));
    }
    EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.info.id);
    if (foodInfo != null && (double) foodInfo.CaloriesPerUnit > 0.0)
    {
      float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(this.info.id, inventory);
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, (object) GameUtil.GetFormattedCalories(calories));
    }
    float amount1 = inventory.GetAmount(this.info.id.ToTag(), false);
    return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, (object) amount1.ToString());
  }

  private string GetSpawnableQuantity()
  {
    if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, (object) GameUtil.GetFormattedMass(this.info.quantity), (object) Assets.GetPrefab((Tag) this.info.id).GetProperName());
    return EdiblesManager.GetFoodInfo(this.info.id) != null ? string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, (object) GameUtil.GetFormattedCaloriesForItem((Tag) this.info.id, this.info.quantity), (object) Assets.GetPrefab((Tag) this.info.id).GetProperName()) : string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT, (object) Assets.GetPrefab((Tag) this.info.id).GetProperName(), (object) this.info.quantity.ToString());
  }

  private string GetSpawnableDescription()
  {
    Element element = ElementLoader.GetElement(this.info.id.ToTag());
    if (element != null)
      return element.Description();
    GameObject prefab = Assets.GetPrefab((Tag) this.info.id);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return "";
    InfoDescription component = prefab.GetComponent<InfoDescription>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? component.description : prefab.GetProperName();
  }

  private string GetSpawnableEffects()
  {
    GameObject prefab = Assets.GetPrefab((Tag) this.info.id);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return "";
    string spawnableEffects = "";
    IGameObjectEffectDescriptor[] components = prefab.GetComponents<IGameObjectEffectDescriptor>();
    if (components != null)
    {
      foreach (IGameObjectEffectDescriptor effectDescriptor in components)
      {
        List<Descriptor> descriptors = effectDescriptor.GetDescriptors(prefab);
        if (descriptors != null)
        {
          foreach (Descriptor descriptor in descriptors)
            spawnableEffects = $"{spawnableEffects}{descriptor.text}\n";
        }
      }
    }
    return spawnableEffects;
  }

  private void SetInfoText()
  {
    this.characterName.SetText(this.GetSpawnableName());
    this.effects.SetText(this.GetSpawnableEffects());
    this.description.SetText(this.GetSpawnableDescription());
    this.itemName.SetText(this.GetSpawnableName());
    this.quantity.SetText(this.GetSpawnableQuantityOnly());
    this.currentQuantity.SetText(this.GetCurrentQuantity(ClusterManager.Instance.activeWorld.worldInventory));
  }

  public void SelectDeliverable()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.AddDeliverable((ITelepadDeliverable) this.carePackageInstanceData);
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
  }

  public void DeselectDeliverable()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.RemoveDeliverable((ITelepadDeliverable) this.carePackageInstanceData);
    this.selectButton.GetComponent<ImageToggleState>().SetInactive();
    this.selectButton.Deselect();
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
    this.selectedBorder.SetActive(false);
    this.titleBar.color = this.deselectedTitleColor;
  }

  private void OnReplacedEvent(ITelepadDeliverable stats)
  {
    if (stats != this.carePackageInstanceData)
      return;
    this.DeselectDeliverable();
  }

  private void OnCharacterSelectionLimitReached()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.info))
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
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.info))
      return;
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
  }

  public void SetReshufflingState(bool enable) => this.reshuffleButton.gameObject.SetActive(enable);

  private void Reshuffle(bool is_starter)
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.info))
      this.DeselectDeliverable();
    this.ClearEntryIcons();
    this.GenerateCharacter(is_starter);
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

  private bool IsCharacterRedundant()
  {
    foreach (ITelepadDeliverableContainer container in CarePackageContainer.containers)
    {
      if (container != this)
      {
        CarePackageContainer packageContainer = container as CarePackageContainer;
        if ((UnityEngine.Object) packageContainer != (UnityEngine.Object) null && packageContainer.info == this.info)
          return true;
      }
    }
    return false;
  }

  public string GetValueColor(bool isPositive)
  {
    return !isPositive ? "<color=#ff2222ff>" : "<color=green>";
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.IsAction(Action.Escape))
      this.controller.OnPressBack();
    if (KInputManager.currentControllerIsGamepad)
      return;
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (KInputManager.currentControllerIsGamepad)
      return;
    e.Consumed = true;
  }

  protected override void OnCmpEnable()
  {
    this.OnActivate();
    if (this.info == null)
      return;
    this.ClearEntryIcons();
    this.SetAnimator();
    this.SetInfoText();
  }

  private void ClearEntryIcons()
  {
    for (int index = 0; index < this.entryIcons.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.entryIcons[index]);
  }

  [Serializable]
  public struct ProfessionIcon
  {
    public string professionName;
    public Sprite iconImg;
  }

  public class CarePackageInstanceData : ITelepadDeliverable
  {
    public CarePackageInfo info;
    public string facadeID;

    public GameObject Deliver(Vector3 position)
    {
      GameObject gameObject = this.info.Deliver(position);
      gameObject.GetComponent<CarePackage>().SetFacade(this.facadeID);
      return gameObject;
    }
  }
}
