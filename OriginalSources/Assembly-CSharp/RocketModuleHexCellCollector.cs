// Decompiled with JetBrains decompiler
// Type: RocketModuleHexCellCollector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RocketModuleHexCellCollector : 
  GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>
{
  public GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State ground;
  public RocketModuleHexCellCollector.InSpaceStates space;
  public StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.Signal HexCellInventoryChangedSignal;
  public StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.TargetParameter ClusterCraft;
  public StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.TargetParameter HexCellInventory;
  private static List<RocketModuleHexCellCollector.ItemData> ItemDataObjects = new List<RocketModuleHexCellCollector.ItemData>()
  {
    new RocketModuleHexCellCollector.ItemData(),
    new RocketModuleHexCellCollector.ItemData(),
    new RocketModuleHexCellCollector.ItemData(),
    new RocketModuleHexCellCollector.ItemData(),
    new RocketModuleHexCellCollector.ItemData(),
    new RocketModuleHexCellCollector.ItemData()
  };

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.Never;
    default_state = (StateMachine.BaseState) this.ground;
    this.ground.TagTransition(GameTags.RocketNotOnGround, (GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State) this.space).Enter(new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.ClearHexCellInventoryChangeCallbacks));
    this.space.TagTransition(GameTags.RocketNotOnGround, this.ground, true).Enter(new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.RefreshHexCellInventoryChangeCallbacks)).EventHandler(GameHashes.ClusterLocationChanged, new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.RefreshHexCellInventoryChangeCallbacks)).EventHandler(GameHashes.ClusterDestinationReached, new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.RefreshHexCellInventoryChangeCallbacks)).DefaultState(this.space.idle);
    this.space.idle.OnSignal(this.HexCellInventoryChangedSignal, this.space.collecting, new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.Parameter<StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.SignalParameter>.Callback(RocketModuleHexCellCollector.CanCollect)).EventHandlerTransition(GameHashes.ClusterLocationChanged, this.space.collecting, new Func<RocketModuleHexCellCollector.Instance, object, bool>(RocketModuleHexCellCollector.CanCollect)).EventHandlerTransition(GameHashes.ClusterDestinationReached, this.space.collecting, new Func<RocketModuleHexCellCollector.Instance, object, bool>(RocketModuleHexCellCollector.CanCollect)).Target(this.ClusterCraft).EventHandlerTransition(GameHashes.ClusterDestinationChanged, this.space.collecting, new Func<RocketModuleHexCellCollector.Instance, object, bool>(RocketModuleHexCellCollector.CanCollect));
    this.space.collecting.Toggle("ToggleCollectingTag", new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.AddCollectingTag), new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.RemoveCollectingTag)).UpdateTransition(this.space.idle, new Func<RocketModuleHexCellCollector.Instance, float, bool>(RocketModuleHexCellCollector.CollectUpdate), UpdateRate.SIM_1000ms).Exit(new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.ClearMassCharge));
  }

  public static void ClearHexCellInventoryChangeCallbacks(RocketModuleHexCellCollector.Instance smi)
  {
    GameObject go = smi.sm.HexCellInventory.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    go.Unsubscribe(-1697596308, new System.Action<object>(smi.TriggerHexCellStorageChangeEvent));
    smi.sm.HexCellInventory.Set((KMonoBehaviour) null, smi);
  }

  public static void RefreshHexCellInventoryChangeCallbacks(
    RocketModuleHexCellCollector.Instance smi)
  {
    GameObject go = smi.sm.HexCellInventory.Get(smi);
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      go.Unsubscribe(-1697596308, new System.Action<object>(smi.TriggerHexCellStorageChangeEvent));
    StarmapHexCellInventory hexCellInventory = ClusterGrid.Instance.AddOrGetHexCellInventory(smi.StarmapLocation);
    smi.sm.HexCellInventory.Set(hexCellInventory.gameObject, smi, false);
    if (!((UnityEngine.Object) hexCellInventory != (UnityEngine.Object) null))
      return;
    hexCellInventory.gameObject.Subscribe(-1697596308, new System.Action<object>(smi.TriggerHexCellStorageChangeEvent));
  }

  public static bool CanCollect(RocketModuleHexCellCollector.Instance smi, object o)
  {
    return RocketModuleHexCellCollector.CanCollect(smi);
  }

  public static bool CanCollect(RocketModuleHexCellCollector.Instance smi)
  {
    if ((double) smi.storage.RemainingCapacity() <= 0.0)
      return false;
    StarmapHexCellInventory hexCellInventory = ClusterGrid.Instance.AddOrGetHexCellInventory(smi.StarmapLocation);
    bool flag = (double) hexCellInventory.TotalMass > 0.0;
    if (smi.IsSpaceshipMoving || !flag)
      return false;
    foreach (StarmapHexCellInventory.SerializedItem serializedItem in hexCellInventory.Items)
    {
      if (RocketModuleHexCellCollector.CanHexCellItemBeStored(serializedItem, smi, out bool _, out float _))
        return true;
    }
    return false;
  }

  public static bool CollectUpdate(RocketModuleHexCellCollector.Instance smi, float dt)
  {
    if ((double) dt == 0.0)
      return false;
    Storage storage = smi.storage;
    float a = storage.RemainingCapacity();
    if ((double) a <= 0.0)
      return true;
    StarmapHexCellInventory hexCellInventory = ClusterGrid.Instance.AddOrGetHexCellInventory(smi.StarmapLocation);
    bool flag1 = (double) hexCellInventory.TotalMass > 0.0;
    if (smi.IsSpaceshipMoving || !flag1)
      return true;
    float b = smi.MassCharge + dt * smi.def.collectSpeed;
    smi.MassCharge = 0.0f;
    float num1 = Mathf.Min(a, b);
    int count = hexCellInventory.Items.Count;
    float num2 = num1;
    float num3 = 0.0f;
    bool flag2 = false;
    RocketModuleHexCellCollector.ClearAllItemData();
    if (RocketModuleHexCellCollector.ItemDataObjects.Count < count)
    {
      int num4 = count - RocketModuleHexCellCollector.ItemDataObjects.Count;
      for (int index = 0; index < num4; ++index)
        RocketModuleHexCellCollector.ItemDataObjects.Add(new RocketModuleHexCellCollector.ItemData());
    }
    float num5 = 0.0f;
    for (int index = 0; index < count; ++index)
    {
      RocketModuleHexCellCollector.ItemData itemDataObject = RocketModuleHexCellCollector.ItemDataObjects[index];
      itemDataObject.Clear();
      StarmapHexCellInventory.SerializedItem serializedItem = hexCellInventory.Items[index];
      bool itemUsesUnits = false;
      float massPerUnit = 1f;
      bool flag3 = RocketModuleHexCellCollector.CanHexCellItemBeStored(serializedItem, smi, out itemUsesUnits, out massPerUnit);
      itemDataObject.ItemID = serializedItem.ID;
      itemDataObject.Mass = serializedItem.Mass;
      itemDataObject.massPerUnit = massPerUnit;
      itemDataObject.usesUnits = itemUsesUnits;
      itemDataObject.isValid = flag3;
      num5 += flag3 ? serializedItem.Mass : 0.0f;
    }
    for (int index = 0; index < count; ++index)
    {
      RocketModuleHexCellCollector.ItemData itemDataObject = RocketModuleHexCellCollector.ItemDataObjects[index];
      if (itemDataObject.isValid)
      {
        itemDataObject.Proportion = itemDataObject.Mass / num5;
        float num6 = itemDataObject.Proportion * num1;
        if ((!itemDataObject.usesUnits ? 1 : ((double) num6 >= (double) itemDataObject.massPerUnit ? 1 : 0)) != 0)
        {
          float mass = num6;
          if (itemDataObject.usesUnits)
            mass = (float) Mathf.FloorToInt(num6 / itemDataObject.massPerUnit) * itemDataObject.massPerUnit;
          float andStoreItemMass = hexCellInventory.ExtractAndStoreItemMass(itemDataObject.ItemID, mass, storage);
          num2 -= andStoreItemMass;
          num3 += andStoreItemMass;
        }
        else
          flag2 = true;
      }
    }
    if (flag2)
      smi.MassCharge += num2;
    if ((double) storage.RemainingCapacity() <= 0.0)
      return true;
    return !flag2 && (double) num3 <= 0.0;
  }

  private static bool CanHexCellItemBeStored(
    StarmapHexCellInventory.SerializedItem item,
    RocketModuleHexCellCollector.Instance smi,
    out bool itemUsesUnits,
    out float massPerUnit)
  {
    itemUsesUnits = false;
    massPerUnit = 1f;
    GameObject prefab = Assets.GetPrefab(item.ID);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      KPrefabID component1 = prefab.GetComponent<KPrefabID>();
      bool flag = false;
      if ((UnityEngine.Object) smi.treeFilterable != (UnityEngine.Object) null)
      {
        foreach (Tag tag in smi.treeFilterable.GetTags())
        {
          if (component1.HasTag(tag))
          {
            flag = true;
            break;
          }
        }
      }
      else
        flag = component1.HasAnyTags(smi.storage.storageFilters);
      if (flag && smi.def.forbiddenTags != null)
      {
        foreach (Tag forbiddenTag in smi.def.forbiddenTags)
        {
          if (component1.HasTag(forbiddenTag))
          {
            flag = false;
            break;
          }
        }
      }
      if (flag)
      {
        Element element = ElementLoader.GetElement(component1.PrefabID());
        PrimaryElement component2 = prefab.GetComponent<PrimaryElement>();
        itemUsesUnits = element == null && (UnityEngine.Object) component2 != (UnityEngine.Object) null && GameTags.DisplayAsUnits.Contains(item.ID);
        massPerUnit = (UnityEngine.Object) component2 == (UnityEngine.Object) null ? 1f : component2.MassPerUnit;
        if (!itemUsesUnits || (double) item.Mass >= (double) component2.MassPerUnit && (double) smi.storage.RemainingCapacity() >= (double) component2.MassPerUnit)
          return true;
      }
    }
    return false;
  }

  public static void RemoveCollectingTag(RocketModuleHexCellCollector.Instance smi)
  {
    RocketModuleHexCellCollector.ToggleCollectingTag(smi, false);
  }

  public static void AddCollectingTag(RocketModuleHexCellCollector.Instance smi)
  {
    RocketModuleHexCellCollector.ToggleCollectingTag(smi, true);
  }

  public static void ToggleCollectingTag(RocketModuleHexCellCollector.Instance smi, bool v)
  {
    smi.ToggleCollectingTag(v);
  }

  public static void ClearMassCharge(RocketModuleHexCellCollector.Instance smi)
  {
    smi.MassCharge = 0.0f;
  }

  private static void ClearAllItemData()
  {
    foreach (RocketModuleHexCellCollector.ItemData itemDataObject in RocketModuleHexCellCollector.ItemDataObjects)
      itemDataObject.Clear();
  }

  public class Def : StateMachine.BaseDef
  {
    public float collectSpeed;
    public bool formatCapacityBarAsUnits;
    public List<Tag> forbiddenTags;
  }

  public class InSpaceStates : 
    GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State
  {
    public GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State idle;
    public GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State collecting;
  }

  private class ItemData
  {
    public Tag ItemID;
    public float Mass;
    public float Proportion;
    public float massPerUnit;
    public bool usesUnits;
    public bool isValid;

    public void Clear()
    {
      this.ItemID = (Tag) (string) null;
      this.Mass = 0.0f;
      this.Proportion = 0.0f;
      this.isValid = false;
      this.usesUnits = false;
      this.massPerUnit = 1f;
    }
  }

  public new class Instance : 
    GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.GameInstance,
    IHexCellCollector
  {
    [Serialize]
    public int LastCollectedIndex;
    [Serialize]
    public float MassCharge;
    public Storage storage;
    public TreeFilterable treeFilterable;
    private Clustercraft clustercraft;

    public bool IsCollecting
    {
      get => this.IsInsideState((StateMachine.BaseState) this.sm.space.collecting);
    }

    public bool IsSpaceshipMoving => this.clustercraft.IsFlightInProgress();

    public AxialI StarmapLocation => this.clustercraft.Location;

    public Instance(IStateMachineTarget master, RocketModuleHexCellCollector.Def def)
      : base(master, def)
    {
      this.storage = this.GetComponent<Storage>();
      this.treeFilterable = (TreeFilterable) null;
    }

    public override void StartSM()
    {
      this.clustercraft = this.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
      this.sm.ClusterCraft.Set(this.clustercraft.gameObject, this, false);
      base.StartSM();
    }

    public void TriggerHexCellStorageChangeEvent(object o)
    {
      this.sm.HexCellInventoryChangedSignal.Trigger(this.smi);
    }

    public void ToggleCollectingTag(bool collecting)
    {
      if (collecting)
      {
        this.clustercraft.AddTag(GameTags.RocketCollectingResources);
      }
      else
      {
        List<RocketModuleHexCellCollector.Instance> collectorModules = this.clustercraft.GetAllHexCellCollectorModules();
        bool flag = false;
        foreach (RocketModuleHexCellCollector.Instance instance in collectorModules)
        {
          if (instance != this && instance != null && instance.IsCollecting)
          {
            flag = true;
            break;
          }
        }
        if (flag)
          return;
        this.clustercraft.RemoveTag(GameTags.RocketCollectingResources);
      }
    }

    public bool CheckIsCollecting()
    {
      return this.IsInsideState((StateMachine.BaseState) this.sm.space.collecting);
    }

    public string GetProperName() => this.GetComponent<RocketModuleCluster>().GetProperName();

    public Sprite GetUISprite()
    {
      return global::Def.GetUISprite((object) this.master.gameObject.GetComponent<KPrefabID>().PrefabID()).first;
    }

    public float GetCapacity() => this.storage.Capacity();

    public float GetMassStored() => this.storage.MassStored();

    public float TimeInState() => this.timeinstate;

    public string GetCapacityBarText()
    {
      return this.def.formatCapacityBarAsUnits ? $"{GameUtil.GetFormattedUnits(this.GetMassStored())} / {GameUtil.GetFormattedUnits(this.GetCapacity())}" : $"{GameUtil.GetFormattedMass(this.GetMassStored())} / {GameUtil.GetFormattedMass(this.GetCapacity())}";
    }
  }
}
