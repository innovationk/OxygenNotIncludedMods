// Decompiled with JetBrains decompiler
// Type: BionicMinionConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BionicMinionConfig : IEntityConfig, IHasDlcRestrictions
{
  public static Tag MODEL = GameTags.Minions.Models.Bionic;
  public static string NAME = (string) DUPLICANTS.MODEL.BIONIC.NAME;
  public static string ID = BionicMinionConfig.MODEL.ToString();
  public static string[] DEFAULT_BIONIC_TRAITS = new string[1]
  {
    "BionicBaseline"
  };
  public Func<RationalAi.Instance, StateMachine.Instance>[] RATIONAL_AI_STATE_MACHINES = BaseMinionConfig.BaseRationalAiStateMachines().Append<Func<RationalAi.Instance, StateMachine.Instance>>(new Func<RationalAi.Instance, StateMachine.Instance>[10]
  {
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BreathMonitor.Instance(smi.master)
    {
      canRecoverBreath = false
    }),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SteppedInMonitor.Instance(smi.master, new string[1]
    {
      "CarpetFeet"
    })),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicBatteryMonitor.Instance(smi.master, new BionicBatteryMonitor.Def())),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicBedTimeMonitor.Instance(smi.master, new BionicBedTimeMonitor.Def())),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicMicrochipMonitor.Instance(smi.master, new BionicMicrochipMonitor.Def())),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicOilMonitor.Instance(smi.master, new BionicOilMonitor.Def())),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new GunkMonitor.Instance(smi.master, new GunkMonitor.Def())),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicWaterDamageMonitor.Instance(smi.master, new BionicWaterDamageMonitor.Def())),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgradesMonitor.Instance(smi.master, new BionicUpgradesMonitor.Def())),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicOxygenTankMonitor.Instance(smi.master, new BionicOxygenTankMonitor.Def()))
  });

  public static string[] GetAttributes()
  {
    return BaseMinionConfig.BaseMinionAttributes().Append<string>(new string[2]
    {
      Db.Get().Attributes.BionicBoosterSlots.Id,
      Db.Get().Attributes.BionicBatteryCountCapacity.Id
    });
  }

  public static string[] GetAmounts()
  {
    return BaseMinionConfig.BaseMinionAmounts().Append<string>(new string[4]
    {
      Db.Get().Amounts.BionicOil.Id,
      Db.Get().Amounts.BionicGunk.Id,
      Db.Get().Amounts.BionicInternalBattery.Id,
      Db.Get().Amounts.BionicOxygenTank.Id
    });
  }

  public static AttributeModifier[] GetTraits()
  {
    return BaseMinionConfig.BaseMinionTraits(BionicMinionConfig.MODEL);
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject go = BaseMinionConfig.BaseMinion(BionicMinionConfig.MODEL, BionicMinionConfig.GetAttributes(), BionicMinionConfig.GetAmounts(), BionicMinionConfig.GetTraits());
    go.AddOrGet<CodexEntryRedirector>().CodexID = "DUPLICANTS";
    go.AddOrGet<AttributeLevels>().maxAttributeLevel = 0;
    Storage storage1 = go.AddComponent<Storage>();
    storage1.storageID = GameTags.StoragesIds.BionicBatteryStorage;
    storage1.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Preserve,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    storage1.storageFilters = new List<Tag>((IEnumerable<Tag>) GameTags.BionicCompatibleBatteries);
    storage1.allowItemRemoval = false;
    storage1.showInUI = false;
    Storage storage2 = go.AddComponent<Storage>();
    storage2.storageID = GameTags.StoragesIds.BionicUpgradeStorage;
    storage2.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Preserve,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    storage2.storageFilters = new List<Tag>()
    {
      GameTags.BionicUpgrade
    };
    storage2.allowItemRemoval = false;
    storage2.showInUI = false;
    Storage storage3 = go.AddComponent<Storage>();
    storage3.capacityKg = BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG;
    storage3.storageID = GameTags.StoragesIds.BionicOxygenTankStorage;
    storage3.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Preserve,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    storage3.allowItemRemoval = false;
    storage3.showInUI = false;
    ManualDeliveryKG manualDeliveryKg = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage1);
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    manualDeliveryKg.capacity = 0.0f;
    manualDeliveryKg.refillMass = 0.0f;
    manualDeliveryKg.handlePrioritizable = false;
    go.AddOrGet<ReanimateBionicWorkable>();
    go.AddOrGet<WarmBlooded>().complexity = WarmBlooded.ComplexityType.HomeostasisWithoutCaloriesImpact;
    go.AddOrGet<BionicMinionStorageExtension>();
    go.AddOrGet<MinionStorageDataHolder>();
    return go;
  }

  public void OnPrefabInit(GameObject go)
  {
    BaseMinionConfig.BasePrefabInit(go, BionicMinionConfig.MODEL);
    AmountInstance amountInstance1 = Db.Get().Amounts.BionicOil.Lookup(go);
    amountInstance1.value = amountInstance1.GetMax();
    AmountInstance amountInstance2 = Db.Get().Amounts.BionicGunk.Lookup(go);
    amountInstance2.value = amountInstance2.GetMin();
  }

  public void OnSpawn(GameObject go)
  {
    Sensors component = go.GetComponent<Sensors>();
    component.Add((Sensor) new ClosestElectrobankSensor(component, true));
    component.Add((Sensor) new ClosestOxygenCanisterSensor(component, false));
    component.Add((Sensor) new ClosestLubricantSensor(component, false));
    BaseMinionConfig.BaseOnSpawn(go, BionicMinionConfig.MODEL, this.RATIONAL_AI_STATE_MACHINES);
    component.GetSensor<SafeCellSensor>().AddIgnoredFlagsSet(BionicMinionConfig.ID, SafeCellQuery.SafeFlags.IsBreathable);
    BionicOxygenTankMonitor.Instance smi = go.GetSMI<BionicOxygenTankMonitor.Instance>();
    if (smi != null)
      go.GetComponent<OxygenBreather>().AddGasProvider((OxygenBreather.IGasProvider) smi);
    this.BionicFreeDiscoveries(go);
    go.Trigger(1589886948, (object) go);
  }

  private void BionicFreeDiscoveries(GameObject instance)
  {
    GameScheduler.Instance.Schedule("BionicUnlockCraftingTable", 8f, (Action<object>) (data1 =>
    {
      TechItem techItem = Db.Get().TechItems.Get("CraftingTable");
      if (!techItem.IsComplete())
      {
        Game.Instance.GetComponent<Notifier>().Add(new Notification((string) MISC.NOTIFICATIONS.BIONICRESEARCHUNLOCK.NAME, NotificationType.MessageImportant, (Func<List<Notification>, object, string>) ((notificationList, data2) => MISC.NOTIFICATIONS.BIONICRESEARCHUNLOCK.MESSAGEBODY.Replace("{0}", Assets.GetPrefab((Tag) "CraftingTable").GetProperName())), (object) Assets.GetPrefab((Tag) "CraftingTable").GetProperName(), clear_on_click: true));
        techItem.POIUnlocked();
      }
      DiscoveredResources.Instance.Discover(PowerControlStationConfig.TINKER_TOOLS);
    }), (object) null, (SchedulerGroup) null);
  }
}
