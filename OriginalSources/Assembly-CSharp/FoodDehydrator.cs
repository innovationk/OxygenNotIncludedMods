// Decompiled with JetBrains decompiler
// Type: FoodDehydrator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FoodDehydrator : 
  GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>
{
  private StatusItem waitingForFuelStatus = new StatusItem(nameof (waitingForFuelStatus), (string) BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, (string) BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
  private static readonly Operational.Flag foodDehydratorFlag = new Operational.Flag("food_dehydrator", Operational.Flag.Type.Requirement);
  private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State waitingForFuel;
  private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State working;
  private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State requestEmpty;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.waitingForFuelStatus.resolveStringCallback = (Func<string, object, string>) ((str, obj) => string.Format(str, (object) FOODDEHYDRATORTUNING.FUEL_TAG.ProperName(), (object) GameUtil.GetFormattedMass(5.00000048f)));
    default_state = (StateMachine.BaseState) this.waitingForFuel;
    this.waitingForFuel.Enter((StateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State.Callback) (smi => smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false))).EventTransition(GameHashes.OnStorageChange, this.working, (StateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.Transition.ConditionCallback) (smi => (double) smi.GetAvailableFuel() >= 5.0000004768371582)).ToggleStatusItem(this.waitingForFuelStatus);
    this.working.Enter((StateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State.Callback) (smi =>
    {
      smi.complexFabricator.SetQueueDirty();
      smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, true);
    })).EventHandler(GameHashes.FabricatorOrdersUpdated, (StateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State.Callback) (smi => smi.UpdateFoodSymbol())).EnterTransition(this.requestEmpty, (StateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.Transition.ConditionCallback) (smi => smi.RequiresEmptying())).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (StateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.Transition.ConditionCallback) (smi => (double) smi.GetAvailableFuel() <= 0.0)).EventHandlerTransition(GameHashes.FabricatorOrderCompleted, this.requestEmpty, (Func<FoodDehydrator.StatesInstance, object, bool>) ((smi, data) => smi.RequiresEmptying())).EventHandler(GameHashes.FabricatorOrderStarted, (StateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State.Callback) (smi => smi.UpdateFoodSymbol()));
    this.requestEmpty.ToggleRecurringChore(new Func<FoodDehydrator.StatesInstance, Chore>(this.CreateChore), (Func<FoodDehydrator.StatesInstance, bool>) (smi => smi.RequiresEmptying())).EventHandlerTransition(GameHashes.OnStorageChange, this.working, (Func<FoodDehydrator.StatesInstance, object, bool>) ((smi, data) => !smi.RequiresEmptying())).Enter((StateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State.Callback) (smi =>
    {
      smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false);
      smi.UpdateFoodSymbol();
    })).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding);
  }

  private Chore CreateChore(FoodDehydrator.StatesInstance smi)
  {
    WorkChore<FoodDehydratorWorkableEmpty> chore = new WorkChore<FoodDehydratorWorkableEmpty>(Db.Get().ChoreTypes.FoodFetch, (IStateMachineTarget) smi.master.GetComponent<FoodDehydratorWorkableEmpty>(), on_complete: new System.Action<Chore>(smi.OnEmptyComplete), only_when_operational: false);
    chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
    return (Chore) chore;
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public List<Descriptor> GetDescriptors(GameObject go)
    {
      return new List<Descriptor>()
      {
        new Descriptor((string) UI.BUILDINGEFFECTS.FOOD_DEHYDRATOR_WATER_OUTPUT, (string) UI.BUILDINGEFFECTS.TOOLTIPS.FOOD_DEHYDRATOR_WATER_OUTPUT)
      };
    }
  }

  public class StatesInstance : 
    GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.GameInstance
  {
    [MyCmpReq]
    public Operational operational;
    [MyCmpReq]
    public ComplexFabricator complexFabricator;
    private static string HASH_FOOD = "food";
    private KBatchedAnimController foodKBAC;
    private int foodIngredientIdx;

    public StatesInstance(IStateMachineTarget master, FoodDehydrator.Def def)
      : base(master, def)
    {
      this.SetupFoodSymbol();
    }

    public float GetAvailableFuel()
    {
      return this.complexFabricator.inStorage.GetMassAvailable(FOODDEHYDRATORTUNING.FUEL_TAG);
    }

    public bool RequiresEmptying() => !this.complexFabricator.outStorage.IsEmpty();

    public void OnEmptyComplete(Chore obj)
    {
      this.complexFabricator.outStorage.DropAll(Grid.CellToPosLCC(Grid.PosToCell((StateMachine.Instance) this), Grid.SceneLayer.Ore), dump_liquid: true);
    }

    public void SetupFoodSymbol()
    {
      GameObject gameObject = Util.NewGameObject(this.gameObject, "food_symbol");
      gameObject.SetActive(false);
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      Vector3 column = (Vector3) component.GetSymbolTransform((HashedString) FoodDehydrator.StatesInstance.HASH_FOOD, out bool _).GetColumn(3) with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse)
      };
      gameObject.transform.SetPosition(column);
      this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
      this.foodKBAC.AnimFiles = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "mushbar_kanim")
      };
      this.foodKBAC.initialAnim = "object";
      component.SetSymbolVisiblity((KAnimHashedString) FoodDehydrator.StatesInstance.HASH_FOOD, false);
      this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
      KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
      kbatchedAnimTracker.symbol = new HashedString("food");
      kbatchedAnimTracker.offset = Vector3.zero;
    }

    public void UpdateFoodSymbol()
    {
      ComplexRecipe currentWorkingOrder = this.complexFabricator.CurrentWorkingOrder;
      if (this.complexFabricator.CurrentWorkingOrder != null)
      {
        this.foodKBAC.gameObject.SetActive(true);
        this.foodKBAC.SwapAnims(Assets.GetPrefab(currentWorkingOrder.ingredients[this.foodIngredientIdx].material).GetComponent<KBatchedAnimController>().AnimFiles);
        this.foodKBAC.Play((HashedString) "object", KAnim.PlayMode.Loop);
      }
      else if (this.complexFabricator.outStorage.items.Count > 0)
      {
        this.foodKBAC.gameObject.SetActive(true);
        this.foodKBAC.SwapAnims(this.complexFabricator.outStorage.items[0].GetComponent<KBatchedAnimController>().AnimFiles);
        this.foodKBAC.Play((HashedString) "object", KAnim.PlayMode.Loop);
      }
      else
        this.foodKBAC.gameObject.SetActive(false);
    }
  }
}
