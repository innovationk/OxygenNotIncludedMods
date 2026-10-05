// Decompiled with JetBrains decompiler
// Type: CropTendingStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CropTendingStates : 
  GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>
{
  private const int MAX_NAVIGATE_DISTANCE = 100;
  private const int MAX_SQR_EUCLIDEAN_DISTANCE = 625;
  private static CropTendingStates.AnimSet defaultAnimSet = new CropTendingStates.AnimSet()
  {
    crop_tending_pre = "crop_tending_pre",
    crop_tending = "crop_tending_loop",
    crop_tending_pst = "crop_tending_pst"
  };
  public StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.TargetParameter targetCrop;
  private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State findCrop;
  private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State moveToCrop;
  private CropTendingStates.TendingStates tendCrop;
  private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.findCrop;
    this.root.Exit((StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State.Callback) (smi =>
    {
      this.UnreserveCrop(smi);
      if (smi.tendedSucceeded)
        return;
      this.RestoreSymbolsVisibility(smi);
    }));
    this.findCrop.Enter((StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State.Callback) (smi =>
    {
      this.FindCrop(smi);
      if ((UnityEngine.Object) smi.sm.targetCrop.Get(smi) == (UnityEngine.Object) null)
      {
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
      }
      else
      {
        this.ReserverCrop(smi);
        smi.GoTo((StateMachine.BaseState) this.moveToCrop);
      }
    }));
    GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State moveToCrop = this.moveToCrop;
    string name1 = (string) CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    moveToCrop.ToggleStatusItem(name1, tooltip1, render_overlay: render_overlay1, category: category1).MoveTo((Func<CropTendingStates.Instance, int>) (smi => smi.moveCell), (GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State) this.tendCrop, this.behaviourcomplete).ParamTransition<GameObject>((StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.Parameter<GameObject>) this.targetCrop, this.behaviourcomplete, (StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) this.targetCrop.Get(smi) == (UnityEngine.Object) null));
    GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State state = this.tendCrop.DefaultState(this.tendCrop.pre);
    string name2 = (string) CREATURES.STATUSITEMS.DIVERGENT_TENDING.NAME;
    string tooltip2 = (string) CREATURES.STATUSITEMS.DIVERGENT_TENDING.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, category: category2).ParamTransition<GameObject>((StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.Parameter<GameObject>) this.targetCrop, this.behaviourcomplete, (StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) this.targetCrop.Get(smi) == (UnityEngine.Object) null)).Enter((StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State.Callback) (smi =>
    {
      smi.animSet = this.GetCropTendingAnimSet(smi);
      this.StoreSymbolsVisibility(smi);
    }));
    this.tendCrop.pre.Face(this.targetCrop).PlayAnim((Func<CropTendingStates.Instance, string>) (smi => smi.animSet.crop_tending_pre)).OnAnimQueueComplete(this.tendCrop.tend);
    this.tendCrop.tend.Enter((StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State.Callback) (smi => this.SetSymbolsVisibility(smi, false))).QueueAnim((Func<CropTendingStates.Instance, string>) (smi => smi.animSet.crop_tending)).OnAnimQueueComplete(this.tendCrop.pst);
    this.tendCrop.pst.QueueAnim((Func<CropTendingStates.Instance, string>) (smi => smi.animSet.crop_tending_pst)).OnAnimQueueComplete(this.behaviourcomplete).Exit((StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State.Callback) (smi =>
    {
      GameObject gameObject = smi.sm.targetCrop.Get(smi);
      if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
        return;
      if (smi.effect != null)
        gameObject.GetComponent<Effects>().Add(smi.effect, true);
      smi.tendedSucceeded = true;
      CropTendingStates.CropTendingEventData data = new CropTendingStates.CropTendingEventData()
      {
        source = smi.gameObject,
        cropId = smi.sm.targetCrop.Get(smi).PrefabID()
      };
      smi.sm.targetCrop.Get(smi).Trigger(90606262, (object) data);
      smi.Trigger(90606262, (object) data);
    }));
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToTendCrops);
  }

  private CropTendingStates.AnimSet GetCropTendingAnimSet(CropTendingStates.Instance smi)
  {
    CropTendingStates.AnimSet animSet;
    return smi.def.animSetOverrides.TryGetValue(this.targetCrop.Get(smi).PrefabID(), out animSet) ? animSet : CropTendingStates.defaultAnimSet;
  }

  private void FindCrop(CropTendingStates.Instance smi)
  {
    Navigator component1 = smi.GetComponent<Navigator>();
    Crop crop = (Crop) null;
    int num1 = Grid.InvalidCell;
    int num2 = 100;
    int num3 = -1;
    foreach (Crop worldItem in Components.Crops.GetWorldItems(smi.gameObject.GetMyWorldId()))
    {
      if ((double) Vector2.SqrMagnitude((Vector2) (worldItem.transform.position - smi.transform.position)) <= 625.0)
      {
        if (smi.effect != null)
        {
          Effects component2 = worldItem.GetComponent<Effects>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          {
            bool flag = false;
            for (int index = 0; index < smi.def.ignoreEffectGroup.Length; ++index)
            {
              HashedString effect_id = smi.def.ignoreEffectGroup[index];
              if (component2.HasEffect(effect_id))
              {
                flag = true;
                break;
              }
            }
            if (flag)
              continue;
          }
        }
        KPrefabID component3 = worldItem.GetComponent<KPrefabID>();
        if (!component3.HasTag(GameTags.FullyGrown) && !component3.HasTag(GameTags.Creatures.ReservedByCreature))
        {
          int num4;
          smi.def.interests.TryGetValue(worldItem.PrefabID(), out num4);
          if (num4 >= num3)
          {
            bool flag = num4 > num3;
            int cell = Grid.PosToCell((KMonoBehaviour) worldItem);
            int[] numArray = new int[2]
            {
              Grid.CellLeft(cell),
              Grid.CellRight(cell)
            };
            if (component3.HasTag(GameTags.PlantedOnFloorVessel))
              numArray = new int[4]
              {
                Grid.CellLeft(cell),
                Grid.CellRight(cell),
                Grid.CellDownLeft(cell),
                Grid.CellDownRight(cell)
              };
            int num5 = 100;
            int invalidCell = Grid.InvalidCell;
            for (int index = 0; index < numArray.Length; ++index)
            {
              if (Grid.IsValidCell(numArray[index]))
              {
                int navigationCost = component1.GetNavigationCost(numArray[index]);
                if (navigationCost != -1 && navigationCost < num5)
                {
                  num5 = navigationCost;
                  invalidCell = numArray[index];
                }
              }
            }
            if (num5 != -1 && invalidCell != Grid.InvalidCell && (flag || num5 < num2))
            {
              num1 = invalidCell;
              num2 = num5;
              num3 = num4;
              crop = worldItem;
            }
          }
        }
      }
    }
    GameObject gameObject = (UnityEngine.Object) crop != (UnityEngine.Object) null ? crop.gameObject : (GameObject) null;
    smi.sm.targetCrop.Set(gameObject, smi, false);
    smi.moveCell = num1;
  }

  private void ReserverCrop(CropTendingStates.Instance smi)
  {
    GameObject go = smi.sm.targetCrop.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    DebugUtil.Assert(!go.HasTag(GameTags.Creatures.ReservedByCreature));
    go.AddTag(GameTags.Creatures.ReservedByCreature);
  }

  private void UnreserveCrop(CropTendingStates.Instance smi)
  {
    GameObject go = smi.sm.targetCrop.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    go.RemoveTag(GameTags.Creatures.ReservedByCreature);
  }

  private void SetSymbolsVisibility(CropTendingStates.Instance smi, bool isVisible)
  {
    if (!((UnityEngine.Object) this.targetCrop.Get(smi) != (UnityEngine.Object) null))
      return;
    string[] hideSymbolsAfterPre = smi.animSet.hide_symbols_after_pre;
    if (hideSymbolsAfterPre == null)
      return;
    KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    foreach (string symbol in hideSymbolsAfterPre)
      component.SetSymbolVisiblity((KAnimHashedString) symbol, isVisible);
  }

  private void StoreSymbolsVisibility(CropTendingStates.Instance smi)
  {
    if (!((UnityEngine.Object) this.targetCrop.Get(smi) != (UnityEngine.Object) null))
      return;
    string[] hideSymbolsAfterPre = smi.animSet.hide_symbols_after_pre;
    if (hideSymbolsAfterPre == null)
      return;
    KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    smi.symbolStates = new bool[hideSymbolsAfterPre.Length];
    for (int index = 0; index < hideSymbolsAfterPre.Length; ++index)
      smi.symbolStates[index] = component.GetSymbolVisiblity((KAnimHashedString) hideSymbolsAfterPre[index]);
  }

  private void RestoreSymbolsVisibility(CropTendingStates.Instance smi)
  {
    if (!((UnityEngine.Object) this.targetCrop.Get(smi) != (UnityEngine.Object) null) || smi.symbolStates == null)
      return;
    string[] hideSymbolsAfterPre = smi.animSet.hide_symbols_after_pre;
    if (hideSymbolsAfterPre == null)
      return;
    KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    for (int index = 0; index < hideSymbolsAfterPre.Length; ++index)
      component.SetSymbolVisiblity((KAnimHashedString) hideSymbolsAfterPre[index], smi.symbolStates[index]);
  }

  public class AnimSet
  {
    public string crop_tending_pre;
    public string crop_tending;
    public string crop_tending_pst;
    public string[] hide_symbols_after_pre;
  }

  public class CropTendingEventData
  {
    public GameObject source;
    public Tag cropId;
  }

  public class Def : StateMachine.BaseDef
  {
    public string effectId;
    public HashedString[] ignoreEffectGroup;
    public Dictionary<Tag, int> interests = new Dictionary<Tag, int>();
    public Dictionary<Tag, CropTendingStates.AnimSet> animSetOverrides = new Dictionary<Tag, CropTendingStates.AnimSet>();
  }

  public new class Instance : 
    GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.GameInstance
  {
    public Effect effect;
    public int moveCell;
    public CropTendingStates.AnimSet animSet;
    public bool tendedSucceeded;
    public bool[] symbolStates;

    public Instance(Chore<CropTendingStates.Instance> chore, CropTendingStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToTendCrops);
      this.effect = Db.Get().effects.TryGet(this.smi.def.effectId);
    }
  }

  public class TendingStates : 
    GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State
  {
    public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pre;
    public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State tend;
    public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pst;
  }
}
