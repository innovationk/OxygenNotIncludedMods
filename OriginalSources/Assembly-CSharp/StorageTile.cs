// Decompiled with JetBrains decompiler
// Type: StorageTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StorageTile : 
  GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>
{
  public const string METER_TARGET = "meter_target";
  public const string METER_ANIMATION = "meter";
  public static HashedString DOOR_SYMBOL_NAME = new HashedString("storage_door");
  public static HashedString ITEM_SYMBOL_TARGET = new HashedString("meter_target_object");
  public static HashedString ITEM_SYMBOL_NAME = new HashedString("object");
  public const string ITEM_SYMBOL_ANIMATION = "meter_object";
  public static HashedString ITEM_PREVIEW_SYMBOL_TARGET = new HashedString("meter_target_object_ui");
  public static HashedString ITEM_PREVIEW_SYMBOL_NAME = new HashedString("object_ui");
  public const string ITEM_PREVIEW_SYMBOL_ANIMATION = "meter_object_ui";
  public static HashedString ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME = new HashedString("placeholder");
  public const string DEFAULT_ANIMATION_NAME = "on";
  public const string STORAGE_CHANGE_ANIMATION_NAME = "door";
  public const string SYMBOL_ANIMATION_NAME_AWAITING_DELIVERY = "ui";
  public static Tag INVALID_TAG = GameTags.Void;
  private StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter TargetItemTag = new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter(StorageTile.INVALID_TAG);
  public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State idle;
  public StorageTile.SettingsChangeStates change;
  public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingDelivery;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.root.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.OnStorageChanged)).EventHandler(GameHashes.StorageTileTargetItemChanged, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals));
    this.idle.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery)).EventTransition(GameHashes.StorageTileTargetItemChanged, (GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State) this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
    this.change.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.NoLongerAwaitingForSettingChange)).DefaultState(this.change.awaitingSettingsChange);
    this.change.awaitingSettingsChange.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.StartWorkChore)).Exit(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.CancelWorkChore)).ToggleStatusItem(Db.Get().BuildingStatusItems.ChangeStorageTileTarget).WorkableCompleteTransition((Func<StorageTile.Instance, Workable>) (smi => (Workable) smi.GetWorkable()), this.change.complete);
    this.change.complete.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.ApplySettings)).Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.DropUndesiredItems)).EnterTransition(this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EnterTransition(this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery));
    this.awaitingDelivery.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EventTransition(GameHashes.StorageTileTargetItemChanged, (GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State) this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
  }

  public static void DropUndesiredItems(StorageTile.Instance smi) => smi.DropUndesiredItems();

  public static void ApplySettings(StorageTile.Instance smi) => smi.ApplySettings();

  public static void StartWorkChore(StorageTile.Instance smi) => smi.StartChangeSettingChore();

  public static void CancelWorkChore(StorageTile.Instance smi) => smi.CanceChangeSettingChore();

  public static void RefreshContentVisuals(StorageTile.Instance smi) => smi.UpdateContentSymbol();

  public static bool IsAwaitingForSettingChange(StorageTile.Instance smi) => smi.IsPendingChange;

  public static bool NoLongerAwaitingForSettingChange(StorageTile.Instance smi)
  {
    return !smi.IsPendingChange;
  }

  public static bool HasAnyDesiredItemStored(StorageTile.Instance smi) => smi.HasAnyDesiredContents;

  public static void OnStorageChanged(StorageTile.Instance smi)
  {
    smi.PlayDoorAnimation();
    StorageTile.RefreshContentVisuals(smi);
  }

  public static bool IsAwaitingDelivery(StorageTile.Instance smi)
  {
    return !smi.IsPendingChange && !smi.HasAnyDesiredContents;
  }

  public class SpecificItemTagSizeInstruction
  {
    public Tag tag;
    public float sizeMultiplier;

    public SpecificItemTagSizeInstruction(Tag tag, float size)
    {
      this.tag = tag;
      this.sizeMultiplier = size;
    }
  }

  public class Def : StateMachine.BaseDef
  {
    public float MaxCapacity;
    public StorageTile.SpecificItemTagSizeInstruction[] specialItemCases;

    public StorageTile.SpecificItemTagSizeInstruction GetSizeInstructionForObject(GameObject obj)
    {
      if (this.specialItemCases == null)
        return (StorageTile.SpecificItemTagSizeInstruction) null;
      KPrefabID component = obj.GetComponent<KPrefabID>();
      foreach (StorageTile.SpecificItemTagSizeInstruction specialItemCase in this.specialItemCases)
      {
        if (component.HasTag(specialItemCase.tag))
          return specialItemCase;
      }
      return (StorageTile.SpecificItemTagSizeInstruction) null;
    }
  }

  public class SettingsChangeStates : 
    GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State
  {
    public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingSettingsChange;
    public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State complete;
  }

  public new class Instance : 
    GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.GameInstance,
    IUserControlledCapacity
  {
    [Serialize]
    private float userMaxCapacity = float.PositiveInfinity;
    [MyCmpGet]
    private Storage storage;
    [MyCmpGet]
    private KBatchedAnimController animController;
    [MyCmpGet]
    private TreeFilterable treeFilterable;
    private FilteredStorage filteredStorage;
    private Chore chore;
    private MeterController amountMeter;
    private KBatchedAnimController doorSymbol;
    private KBatchedAnimController itemSymbol;
    private SymbolOverrideController itemSymbolOverrideController;
    private KBatchedAnimController itemPreviewSymbol;
    private KAnimLink doorAnimLink;
    private string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;
    private float defaultItemSymbolScale = -1f;
    private Vector3 defaultItemLocalPosition = Vector3.zero;

    public Tag TargetTag => this.smi.sm.TargetItemTag.Get(this.smi);

    public bool HasContents => (double) this.storage.MassStored() > 0.0;

    public bool HasAnyDesiredContents
    {
      get
      {
        return !(this.TargetTag == StorageTile.INVALID_TAG) ? (double) this.AmountOfDesiredContentStored > 0.0 : !this.HasContents;
      }
    }

    public float AmountOfDesiredContentStored
    {
      get
      {
        return !(this.TargetTag == StorageTile.INVALID_TAG) ? this.storage.GetMassAvailable(this.TargetTag) : 0.0f;
      }
    }

    public bool IsPendingChange => this.GetTreeFilterableCurrentTag() != this.TargetTag;

    public float UserMaxCapacity
    {
      get => Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
      set
      {
        this.userMaxCapacity = value;
        this.filteredStorage.FilterChanged();
        this.RefreshAmountMeter();
      }
    }

    public float AmountStored => this.storage.MassStored();

    public float MinCapacity => 0.0f;

    public float MaxCapacity => this.def.MaxCapacity;

    public bool WholeValues => false;

    public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();

    private Tag GetTreeFilterableCurrentTag()
    {
      return this.treeFilterable.GetTags() != null && this.treeFilterable.GetTags().Count != 0 ? this.treeFilterable.GetTags().GetRandom<Tag>() : StorageTile.INVALID_TAG;
    }

    public StorageTileSwitchItemWorkable GetWorkable()
    {
      return this.smi.gameObject.GetComponent<StorageTileSwitchItemWorkable>();
    }

    public Instance(IStateMachineTarget master, StorageTile.Def def)
      : base(master, def)
    {
      this.itemSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_SYMBOL_TARGET, StorageTile.ITEM_SYMBOL_NAME, "meter_object");
      this.itemSymbol.usingNewSymbolOverrideSystem = true;
      this.itemSymbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(this.itemSymbol.gameObject);
      this.itemPreviewSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_PREVIEW_SYMBOL_TARGET, StorageTile.ITEM_PREVIEW_SYMBOL_NAME, "meter_object_ui");
      this.defaultItemSymbolScale = this.itemSymbol.transform.localScale.x;
      this.defaultItemLocalPosition = this.itemSymbol.transform.localPosition;
      this.doorSymbol = this.CreateEmptyKAnimController(StorageTile.DOOR_SYMBOL_NAME.ToString());
      this.doorSymbol.initialAnim = "on";
      foreach (KAnim.Build.Symbol symbol in this.doorSymbol.AnimFiles[0].GetData().build.symbols)
        this.doorSymbol.SetSymbolVisiblity(symbol.hash, symbol.hash == StorageTile.DOOR_SYMBOL_NAME);
      this.doorSymbol.transform.SetParent(this.animController.transform, false);
      this.doorSymbol.transform.SetLocalPosition(-Vector3.forward * 0.05f);
      this.doorSymbol.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.OnDoorAnimationCompleted);
      this.doorSymbol.gameObject.SetActive(true);
      this.animController.SetSymbolVisiblity((KAnimHashedString) StorageTile.DOOR_SYMBOL_NAME, false);
      this.doorAnimLink = new KAnimLink((KAnimControllerBase) this.animController, (KAnimControllerBase) this.doorSymbol);
      this.amountMeter = new MeterController((KAnimControllerBase) this.animController, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
      this.filteredStorage = new FilteredStorage((KMonoBehaviour) this.storage, (Tag[]) null, (IUserControlledCapacity) this, false, Db.Get().ChoreTypes.Get(this.choreTypeID));
      this.Subscribe(-905833192, new System.Action<object>(this.OnCopySettings));
      this.Subscribe(1606648047, new System.Action<object>(this.OnObjectReplaced));
    }

    public override void StartSM()
    {
      base.StartSM();
      this.filteredStorage.FilterChanged();
    }

    public override void PostParamsInitialized()
    {
      if (this.TargetTag != StorageTile.INVALID_TAG && (UnityEngine.Object) Assets.GetPrefab(this.TargetTag) == (UnityEngine.Object) null)
      {
        this.SetTargetItem(StorageTile.INVALID_TAG);
        this.DropUndesiredItems();
      }
      base.PostParamsInitialized();
    }

    private void OnObjectReplaced(object data)
    {
      Constructable.ReplaceCallbackParameters callbackParameters = ((Boxed<Constructable.ReplaceCallbackParameters>) data).value;
      List<GameObject> gameObjectList1 = new List<GameObject>();
      Storage storage = this.storage;
      List<GameObject> gameObjectList2 = gameObjectList1;
      Vector3 offset = new Vector3();
      List<GameObject> collect_dropped_items = gameObjectList2;
      storage.DropAll(false, false, offset, true, collect_dropped_items);
      if (!((UnityEngine.Object) callbackParameters.Worker != (UnityEngine.Object) null))
        return;
      foreach (GameObject gameObject in gameObjectList1)
        gameObject.GetComponent<Pickupable>().Trigger(580035959, (object) callbackParameters.Worker);
    }

    private void OnDoorAnimationCompleted(HashedString animName)
    {
      if (!(animName == (HashedString) "door"))
        return;
      this.doorSymbol.Play((HashedString) "on");
    }

    private KBatchedAnimController CreateEmptyKAnimController(string name)
    {
      GameObject gameObject = new GameObject($"{this.gameObject.name}-{name}");
      gameObject.SetActive(false);
      KBatchedAnimController emptyKanimController = gameObject.AddComponent<KBatchedAnimController>();
      emptyKanimController.AnimFiles = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "storagetile_kanim")
      };
      emptyKanimController.sceneLayer = Grid.SceneLayer.BuildingFront;
      emptyKanimController.CopyBlendValue((KAnimControllerBase) this.animController);
      return emptyKanimController;
    }

    private KBatchedAnimController CreateSymbolOverrideCapsule(
      HashedString symbolTarget,
      HashedString symbolName,
      string animationName)
    {
      KBatchedAnimController emptyKanimController = this.CreateEmptyKAnimController(symbolTarget.ToString());
      emptyKanimController.initialAnim = animationName;
      Matrix4x4 symbolTransform = this.animController.GetSymbolTransform(symbolTarget, out bool _);
      Matrix2x3 symbolLocalTransform = this.animController.GetSymbolLocalTransform(symbolTarget, out bool _);
      Vector3 column = (Vector3) symbolTransform.GetColumn(3);
      Vector3 vector3 = Vector3.one * symbolLocalTransform.m00;
      emptyKanimController.transform.SetParent(this.transform, false);
      emptyKanimController.transform.SetPosition(column);
      emptyKanimController.transform.localPosition = emptyKanimController.transform.localPosition with
      {
        z = -1f / 400f
      };
      emptyKanimController.transform.localScale = vector3;
      emptyKanimController.gameObject.SetActive(false);
      this.animController.SetSymbolVisiblity((KAnimHashedString) symbolTarget, false);
      return emptyKanimController;
    }

    private void OnCopySettings(object sourceOBJ)
    {
      if (sourceOBJ == null)
        return;
      StorageTile.Instance smi = ((GameObject) sourceOBJ).GetSMI<StorageTile.Instance>();
      if (smi == null)
        return;
      this.SetTargetItem(smi.TargetTag);
      this.UserMaxCapacity = smi.UserMaxCapacity;
    }

    public void RefreshAmountMeter()
    {
      this.amountMeter.SetPositionPercent((double) this.UserMaxCapacity == 0.0 ? 0.0f : Mathf.Clamp(this.AmountOfDesiredContentStored / this.UserMaxCapacity, 0.0f, 1f));
    }

    public void PlayDoorAnimation() => this.doorSymbol.Play((HashedString) "door");

    public void SetTargetItem(Tag tag)
    {
      this.sm.TargetItemTag.Set(tag, this);
      this.gameObject.Trigger(-2076953849);
    }

    public void ApplySettings()
    {
      this.treeFilterable.RemoveTagFromFilter(this.GetTreeFilterableCurrentTag());
    }

    public void DropUndesiredItems()
    {
      Vector3 position = (Grid.CellToPos(this.GetWorkable().LastCellWorkerUsed) + Vector3.right * Grid.CellSizeInMeters * 0.5f + Vector3.up * Grid.CellSizeInMeters * 0.5f) with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Ore)
      };
      if (this.TargetTag != StorageTile.INVALID_TAG)
      {
        this.treeFilterable.AddTagToFilter(this.TargetTag);
        GameObject[] gameObjectArray = this.storage.DropUnlessHasTag(this.TargetTag);
        if (gameObjectArray != null)
        {
          foreach (GameObject gameObject in gameObjectArray)
            gameObject.transform.SetPosition(position);
        }
      }
      else
        this.storage.DropAll(position);
      this.storage.DropUnlessHasTag(this.TargetTag);
    }

    public void UpdateContentSymbol()
    {
      this.RefreshAmountMeter();
      bool flag = this.TargetTag == StorageTile.INVALID_TAG;
      if ((!flag ? 1 : (this.HasContents ? 1 : 0)) != 0)
      {
        bool is_visible = !flag && (this.IsPendingChange || !this.HasAnyDesiredContents);
        string animName = "";
        GameObject gameObject = this.TargetTag == StorageTile.INVALID_TAG ? Assets.GetPrefab(this.storage.items[0].PrefabID()) : Assets.GetPrefab(this.TargetTag);
        KAnimFile fromPrefabWithTag = global::Def.GetAnimFileFromPrefabWithTag(gameObject, is_visible ? "ui" : "", out animName);
        this.animController.SetSymbolVisiblity((KAnimHashedString) StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, is_visible);
        this.itemPreviewSymbol.gameObject.SetActive(is_visible);
        this.itemSymbol.gameObject.SetActive(!is_visible);
        if (is_visible)
        {
          this.itemPreviewSymbol.SwapAnims(new KAnimFile[1]
          {
            fromPrefabWithTag
          });
          this.itemPreviewSymbol.Play((HashedString) animName);
        }
        else
        {
          string initialAnim;
          if (gameObject.HasTag(GameTags.Egg))
          {
            string prefix = animName;
            if (!string.IsNullOrEmpty(prefix))
              this.itemSymbolOverrideController.ApplySymbolOverridesByAffix(fromPrefabWithTag, prefix);
            initialAnim = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
          }
          else
          {
            this.itemSymbolOverrideController.RemoveAllSymbolOverrides();
            initialAnim = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
          }
          this.itemSymbol.SwapAnims(new KAnimFile[1]
          {
            fromPrefabWithTag
          });
          this.itemSymbol.Play((HashedString) initialAnim);
          StorageTile.SpecificItemTagSizeInstruction instructionForObject = this.def.GetSizeInstructionForObject(gameObject);
          this.itemSymbol.transform.localScale = Vector3.one * (instructionForObject != null ? instructionForObject.sizeMultiplier : this.defaultItemSymbolScale);
          KCollider2D component = gameObject.GetComponent<KCollider2D>();
          Vector3 itemLocalPosition = this.defaultItemLocalPosition;
          itemLocalPosition.y += (UnityEngine.Object) component == (UnityEngine.Object) null || component is KCircleCollider2D ? 0.0f : (float) (-(double) component.offset.y * 0.5);
          this.itemSymbol.transform.localPosition = itemLocalPosition;
        }
      }
      else
      {
        this.itemSymbol.gameObject.SetActive(false);
        this.itemPreviewSymbol.gameObject.SetActive(false);
        this.animController.SetSymbolVisiblity((KAnimHashedString) StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, false);
      }
    }

    private void AbortChore()
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("Change settings Chore aborted");
      this.chore = (Chore) null;
    }

    public void StartChangeSettingChore()
    {
      this.AbortChore();
      this.chore = (Chore) new WorkChore<StorageTileSwitchItemWorkable>(Db.Get().ChoreTypes.Toggle, (IStateMachineTarget) this.GetWorkable());
    }

    public void CanceChangeSettingChore() => this.AbortChore();
  }
}
