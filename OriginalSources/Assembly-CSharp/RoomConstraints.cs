// Decompiled with JetBrains decompiler
// Type: RoomConstraints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
public static class RoomConstraints
{
  public static RoomConstraints.Constraint CEILING_HEIGHT_4 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => 1 + room.cavity.maxY - room.cavity.minY >= 4), name: string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.NAME, (object) "4"), description: string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, (object) "4"));
  public static RoomConstraints.Constraint CEILING_HEIGHT_6 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => 1 + room.cavity.maxY - room.cavity.minY >= 6), name: string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.NAME, (object) "6"), description: string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, (object) "6"));
  public static RoomConstraints.Constraint MINIMUM_SIZE_12 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.NumCells >= 12), name: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.NAME, (object) "12"), description: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, (object) "12"));
  public static RoomConstraints.Constraint MINIMUM_SIZE_24 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.NumCells >= 24), name: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.NAME, (object) "24"), description: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, (object) "24"));
  public static RoomConstraints.Constraint MINIMUM_SIZE_32 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.NumCells >= 32 /*0x20*/), name: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.NAME, (object) "32"), description: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, (object) "32"));
  public static RoomConstraints.Constraint MAXIMUM_SIZE_64 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.NumCells <= 64 /*0x40*/), name: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, (object) "64"), description: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, (object) "64"));
  public static RoomConstraints.Constraint MAXIMUM_SIZE_96 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.NumCells <= 96 /*0x60*/), name: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, (object) "96"), description: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, (object) "96"));
  public static RoomConstraints.Constraint MAXIMUM_SIZE_120 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.NumCells <= 120), name: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, (object) "120"), description: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, (object) "120"));
  public static RoomConstraints.Constraint NO_INDUSTRIAL_MACHINERY = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
        return false;
    }
    return true;
  }), name: (string) ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME, description: (string) ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.DESCRIPTION);
  public static RoomConstraints.Constraint NO_COTS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.BedType) && !building.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
        return false;
    }
    return true;
  }), name: (string) ROOMS.CRITERIA.NO_COTS.NAME, description: (string) ROOMS.CRITERIA.NO_COTS.DESCRIPTION);
  public static RoomConstraints.Constraint NO_LUXURY_BEDS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
        return false;
    }
    return true;
  }), name: (string) ROOMS.CRITERIA.NO_COTS.NAME, description: (string) ROOMS.CRITERIA.NO_COTS.DESCRIPTION);
  public static RoomConstraints.Constraint NO_OUTHOUSES = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.ToiletType) && !building.HasTag(RoomConstraints.ConstraintTags.FlushToiletType))
        return false;
    }
    return true;
  }), name: (string) ROOMS.CRITERIA.NO_OUTHOUSES.NAME, description: (string) ROOMS.CRITERIA.NO_OUTHOUSES.DESCRIPTION);
  public static RoomConstraints.Constraint NO_MESS_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    bool flag = false;
    for (int index = 0; !flag && index < room.buildings.Count; ++index)
      flag = room.buildings[index].HasTag(RoomConstraints.ConstraintTags.MessTable);
    return !flag;
  }), name: (string) ROOMS.CRITERIA.NO_MESS_STATION.NAME, description: (string) ROOMS.CRITERIA.NO_MESS_STATION.DESCRIPTION);
  public static RoomConstraints.Constraint NO_BASIC_MESS_STATIONS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    bool flag = false;
    for (int index = 0; !flag && index < room.buildings.Count; ++index)
    {
      flag = room.buildings[index].PrefabID() == (Tag) "DiningTable";
      if (flag)
        break;
    }
    return !flag;
  }), name: (string) ROOMS.CRITERIA.NO_BASIC_MESS_STATIONS.NAME, description: (string) ROOMS.CRITERIA.NO_BASIC_MESS_STATIONS.DESCRIPTION);
  public static RoomConstraints.Constraint HAS_LUXURY_BED = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.HAS_LUXURY_BED.NAME, description: (string) ROOMS.CRITERIA.HAS_LUXURY_BED.DESCRIPTION);
  public static RoomConstraints.Constraint HAS_BED = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.BedType) && !bc.HasTag(RoomConstraints.ConstraintTags.Clinic)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.HAS_BED.NAME, description: (string) ROOMS.CRITERIA.HAS_BED.DESCRIPTION);
  public static RoomConstraints.Constraint SCIENCE_BUILDINGS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.ScienceBuilding)), (Func<Room, bool>) null, 2, (string) ROOMS.CRITERIA.SCIENCE_BUILDINGS.NAME, (string) ROOMS.CRITERIA.SCIENCE_BUILDINGS.DESCRIPTION);
  public static RoomConstraints.Constraint BED_SINGLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.BedType) && !bc.HasTag(RoomConstraints.ConstraintTags.Clinic)), (Func<Room, bool>) (room =>
  {
    short num = 0;
    for (int index = 0; num < (short) 2 && index < room.buildings.Count; ++index)
    {
      if (room.buildings[index].HasTag(RoomConstraints.ConstraintTags.BedType))
        ++num;
    }
    return num == (short) 1;
  }), name: (string) ROOMS.CRITERIA.BED_SINGLE.NAME, description: (string) ROOMS.CRITERIA.BED_SINGLE.DESCRIPTION);
  public static RoomConstraints.Constraint LUXURY_BED_SINGLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType)), (Func<Room, bool>) (room =>
  {
    short num = 0;
    for (int index = 0; num <= (short) 2 && index < room.buildings.Count; ++index)
    {
      if (room.buildings[index].HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
        ++num;
    }
    return num == (short) 1;
  }), name: (string) ROOMS.CRITERIA.LUXURYBEDTYPE.NAME, description: (string) ROOMS.CRITERIA.LUXURYBEDTYPE.DESCRIPTION);
  public static RoomConstraints.Constraint BUILDING_DECOR_POSITIVE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc =>
  {
    DecorProvider component = bc.GetComponent<DecorProvider>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.baseDecor > 0.0;
  }), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME, description: (string) ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.DESCRIPTION);
  public static RoomConstraints.Constraint DECORATIVE_ITEM = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(GameTags.Decoration)), (Func<Room, bool>) null, name: string.Format((string) ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, (object) 1), description: string.Format((string) ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, (object) 1));
  public static RoomConstraints.Constraint DECORATIVE_ITEM_2 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(GameTags.Decoration)), (Func<Room, bool>) null, 2, string.Format((string) ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, (object) 2), string.Format((string) ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, (object) 2));
  public static RoomConstraints.Constraint ORNAMENTDISPLAYED = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    for (int index = 0; index < room.buildings.Count; ++index)
    {
      if (RoomConstraints.CheckOrnament(room.buildings[index]))
        return true;
    }
    for (int index = 0; index < room.otherEntities.Count; ++index)
    {
      if (RoomConstraints.CheckOrnament(room.otherEntities[index]))
        return true;
    }
    return false;
  }), name: (string) ROOMS.CRITERIA.ORNAMENT.NAME, description: (string) ROOMS.CRITERIA.ORNAMENT.DESCRIPTION);
  public static RoomConstraints.Constraint POWER_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType)), (Func<Room, bool>) (room =>
  {
    int num = 0;
    bool flag = false;
    foreach (KPrefabID building in room.buildings)
    {
      flag = flag || building.HasTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType);
      num += building.HasTag(RoomConstraints.ConstraintTags.PowerBuilding) ? 1 : 0;
    }
    return flag && num >= 2;
  }), name: (string) ROOMS.CRITERIA.POWERPLANT.NAME, description: (string) ROOMS.CRITERIA.POWERPLANT.DESCRIPTION, overrideConstraintConflictName: (string) ROOMS.CRITERIA.POWERPLANT.CONFLICT_DESCRIPTION);
  public static RoomConstraints.Constraint FARM_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.FarmStationType)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.FARMSTATIONTYPE.NAME, description: (string) ROOMS.CRITERIA.FARMSTATIONTYPE.DESCRIPTION);
  public static RoomConstraints.Constraint RANCH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.RanchStationType)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.RANCHSTATIONTYPE.NAME, description: (string) ROOMS.CRITERIA.RANCHSTATIONTYPE.DESCRIPTION);
  public static RoomConstraints.Constraint SPICE_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.SpiceStation)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.SPICESTATION.NAME, description: (string) ROOMS.CRITERIA.SPICESTATION.DESCRIPTION);
  public static RoomConstraints.Constraint COOK_TOP = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.CookTop)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.COOKTOP.NAME, description: (string) ROOMS.CRITERIA.COOKTOP.DESCRIPTION);
  public static RoomConstraints.Constraint KITCHENREFRIGERATOR = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.KitchenRefrigerator)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.KITCHENREFRIGERATOR.NAME, description: (string) ROOMS.CRITERIA.KITCHENREFRIGERATOR.DESCRIPTION);
  public static RoomConstraints.Constraint REC_BUILDING = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.RecBuilding)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.RECBUILDING.NAME, description: (string) ROOMS.CRITERIA.RECBUILDING.DESCRIPTION);
  public static RoomConstraints.Constraint MACHINE_SHOP = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.MachineShopType)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.MACHINESHOPTYPE.NAME, description: (string) ROOMS.CRITERIA.MACHINESHOPTYPE.DESCRIPTION);
  [Obsolete("The light requirement constraint in rooms has been removed. This is retained solely to avoid breaking mods")]
  public static RoomConstraints.Constraint LIGHT = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.LIGHTSOURCE.NAME, description: (string) ROOMS.CRITERIA.LIGHTSOURCE.DESCRIPTION);
  public static RoomConstraints.Constraint DESTRESSING_BUILDING = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.DeStressingBuilding)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.DESTRESSINGBUILDING.NAME, description: (string) ROOMS.CRITERIA.DESTRESSINGBUILDING.DESCRIPTION);
  public static RoomConstraints.Constraint MASSAGE_TABLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.IsPrefabID(RoomConstraints.ConstraintTags.MassageTable)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.MASSAGE_TABLE.NAME, description: (string) ROOMS.CRITERIA.MASSAGE_TABLE.DESCRIPTION);
  public static RoomConstraints.Constraint DINING_TABLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.DiningTableType)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.DININGTABLETYPE.NAME, description: (string) ROOMS.CRITERIA.DININGTABLETYPE.DESCRIPTION, stomp_in_conflict: new List<RoomConstraints.Constraint>()
  {
    RoomConstraints.REC_BUILDING,
    RoomConstraints.MESS_STATION_SINGLE,
    RoomConstraints.MULTI_MINION_DINING_TABLE
  });
  public static RoomConstraints.Constraint MESS_STATION_SINGLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.IsPrefabID((Tag) "DiningTable")), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.DININGTABLETYPE.NAME, description: (string) ROOMS.CRITERIA.DININGTABLETYPE.DESCRIPTION, stomp_in_conflict: new List<RoomConstraints.Constraint>()
  {
    RoomConstraints.REC_BUILDING,
    RoomConstraints.DINING_TABLE
  });
  public static RoomConstraints.Constraint MULTI_MINION_DINING_TABLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.IsPrefabID((Tag) "MultiMinionDiningTable") || bc.gameObject.name == "MultiMinionDiningSeat"), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.MULTI_MINION_DINING_TABLE.NAME, description: (string) ROOMS.CRITERIA.MULTI_MINION_DINING_TABLE.DESCRIPTION, stomp_in_conflict: new List<RoomConstraints.Constraint>()
  {
    RoomConstraints.REC_BUILDING,
    RoomConstraints.DINING_TABLE
  });
  public static RoomConstraints.Constraint TOILET = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.ToiletType)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.TOILETTYPE.NAME, description: (string) ROOMS.CRITERIA.TOILETTYPE.DESCRIPTION);
  public static RoomConstraints.Constraint BIONICUPKEEP = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.BionicUpkeepType)), (Func<Room, bool>) null, 2, (string) ROOMS.CRITERIA.BIONICUPKEEP.NAME, (string) ROOMS.CRITERIA.BIONICUPKEEP.DESCRIPTION);
  public static RoomConstraints.Constraint BIONIC_LUBRICATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag((Tag) "OilChanger")), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.BIONIC_LUBRICATION.NAME, description: (string) ROOMS.CRITERIA.BIONIC_LUBRICATION.DESCRIPTION);
  public static RoomConstraints.Constraint BIONIC_GUNKEMPTIER = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag((Tag) "GunkEmptier")), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.NAME, description: (string) ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.DESCRIPTION);
  public static RoomConstraints.Constraint FLUSH_TOILET = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.FlushToiletType)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.FLUSHTOILETTYPE.NAME, description: (string) ROOMS.CRITERIA.FLUSHTOILETTYPE.DESCRIPTION);
  public static RoomConstraints.Constraint WASH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.WashStation)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.WASHSTATION.NAME, description: (string) ROOMS.CRITERIA.WASHSTATION.DESCRIPTION);
  public static RoomConstraints.Constraint ADVANCEDWASHSTATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.AdvancedWashStation)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.ADVANCEDWASHSTATION.NAME, description: (string) ROOMS.CRITERIA.ADVANCEDWASHSTATION.DESCRIPTION);
  public static RoomConstraints.Constraint CLINIC = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.Clinic)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.CLINIC.NAME, description: (string) ROOMS.CRITERIA.CLINIC.DESCRIPTION, stomp_in_conflict: new List<RoomConstraints.Constraint>()
  {
    RoomConstraints.TOILET,
    RoomConstraints.FLUSH_TOILET,
    RoomConstraints.MESS_STATION_SINGLE
  });
  public static RoomConstraints.Constraint PARK_BUILDING = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.Park)), (Func<Room, bool>) null, name: (string) ROOMS.CRITERIA.PARK.NAME, description: (string) ROOMS.CRITERIA.PARK.DESCRIPTION);
  public static RoomConstraints.Constraint ORIGINALTILES = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => 1 + room.cavity.maxY - room.cavity.minY >= 4));
  public static RoomConstraints.Constraint IS_BACKWALLED = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    bool flag1 = true;
    int num = (room.cavity.maxX - room.cavity.minX + 1) / 2 + 1;
    for (int index = 0; flag1 && index < num; ++index)
    {
      int x1 = room.cavity.minX + index;
      int x2 = room.cavity.maxX - index;
      for (int minY = room.cavity.minY; flag1 && minY <= room.cavity.maxY; ++minY)
      {
        int cell1 = Grid.XYToCell(x1, minY);
        int cell2 = Grid.XYToCell(x2, minY);
        if (Game.Instance.roomProber.GetCavityForCell(cell1) == room.cavity)
        {
          bool flag2 = BackwallManager.HasBackwall(cell1) || (UnityEngine.Object) Grid.Objects[cell1, 2] != (UnityEngine.Object) null && !Grid.Objects[cell1, 2].HasTag(GameTags.UnderConstruction);
          flag1 &= flag2;
        }
        if (Game.Instance.roomProber.GetCavityForCell(cell2) == room.cavity)
        {
          bool flag3 = BackwallManager.HasBackwall(cell2) || (UnityEngine.Object) Grid.Objects[cell2, 2] != (UnityEngine.Object) null && !Grid.Objects[cell2, 2].HasTag(GameTags.UnderConstruction);
          flag1 &= flag3;
        }
        if (!flag1)
          return false;
      }
    }
    return flag1;
  }), name: (string) ROOMS.CRITERIA.IS_BACKWALLED.NAME, description: (string) ROOMS.CRITERIA.IS_BACKWALLED.DESCRIPTION);
  public static RoomConstraints.Constraint WILDANIMAL = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.creatures.Count + room.cavity.eggs.Count > 0), name: (string) ROOMS.CRITERIA.WILDANIMAL.NAME, description: (string) ROOMS.CRITERIA.WILDANIMAL.DESCRIPTION);
  public static RoomConstraints.Constraint WILDANIMALS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    int num = 0;
    foreach (KPrefabID creature in room.cavity.creatures)
    {
      if (creature.HasTag(GameTags.Creatures.Wild))
        ++num;
    }
    return num >= 2;
  }), name: (string) ROOMS.CRITERIA.WILDANIMALS.NAME, description: (string) ROOMS.CRITERIA.WILDANIMALS.DESCRIPTION);
  public static RoomConstraints.Constraint WILDPLANT = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    int num = 0;
    foreach (KPrefabID plant in room.cavity.plants)
    {
      if ((UnityEngine.Object) plant != (UnityEngine.Object) null && !plant.HasTag(GameTags.PlantBranch))
      {
        BasicForagePlantPlanted component1 = plant.GetComponent<BasicForagePlantPlanted>();
        ReceptacleMonitor component2 = plant.GetComponent<ReceptacleMonitor>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && !component2.Replanted)
          ++num;
        else if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          ++num;
      }
    }
    return num >= 2;
  }), name: (string) ROOMS.CRITERIA.WILDPLANT.NAME, description: (string) ROOMS.CRITERIA.WILDPLANT.DESCRIPTION);
  public static RoomConstraints.Constraint WILDPLANTS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    int num = 0;
    foreach (KPrefabID plant in room.cavity.plants)
    {
      if ((UnityEngine.Object) plant != (UnityEngine.Object) null && !plant.HasTag(GameTags.PlantBranch))
      {
        BasicForagePlantPlanted component3 = plant.GetComponent<BasicForagePlantPlanted>();
        ReceptacleMonitor component4 = plant.GetComponent<ReceptacleMonitor>();
        if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && !component4.Replanted)
          ++num;
        else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          ++num;
      }
    }
    return num >= 4;
  }), name: (string) ROOMS.CRITERIA.WILDPLANTS.NAME, description: (string) ROOMS.CRITERIA.WILDPLANTS.DESCRIPTION);

  public static Tag AddAndReturn(this List<Tag> list, Tag tag)
  {
    list.Add(tag);
    return tag;
  }

  public static string RoomCriteriaString(Room room)
  {
    string str1 = "";
    RoomType roomType = room.roomType;
    if (roomType != Db.Get().RoomTypes.Neutral)
    {
      string str2 = $"{$"{str1}<b>{(string) ROOMS.CRITERIA.HEADER}</b>"}\n    • {roomType.primary_constraint.name}";
      if (roomType.additional_constraints != null)
      {
        foreach (RoomConstraints.Constraint additionalConstraint in roomType.additional_constraints)
          str2 = !additionalConstraint.isSatisfied(room) ? $"{str2}\n<color=#F44A47FF>    • {additionalConstraint.name}</color>" : $"{str2}\n    • {additionalConstraint.name}";
      }
      return str2;
    }
    RoomTypes.RoomTypeQueryResult[] possibleRoomTypes = Db.Get().RoomTypes.GetPossibleRoomTypes(room);
    string str3 = str1 + (possibleRoomTypes.Length > 1 ? $"<b>{(string) ROOMS.CRITERIA.POSSIBLE_TYPES_HEADER}</b>" : "");
    foreach (RoomTypes.RoomTypeQueryResult roomTypeQueryResult1 in possibleRoomTypes)
    {
      RoomType type1 = roomTypeQueryResult1.Type;
      if (type1 != Db.Get().RoomTypes.Neutral)
      {
        if (str3 != "")
          str3 += "\n";
        str3 = $"{str3}<b><color=#BCBCBC>    • {type1.Name}</b> ({type1.primary_constraint.conflictDescription})</color>";
        if (roomTypeQueryResult1.SatisfactionRating == RoomType.RoomIdentificationResult.all_satisfied)
        {
          bool flag = false;
          foreach (RoomTypes.RoomTypeQueryResult roomTypeQueryResult2 in possibleRoomTypes)
          {
            RoomType type2 = roomTypeQueryResult2.Type;
            if (type2 != type1 && type2 != Db.Get().RoomTypes.Neutral && Db.Get().RoomTypes.HasAmbiguousRoomType(room, type1, type2))
            {
              flag = true;
              break;
            }
          }
          if (flag)
            str3 += $"\n<color=#F44A47FF>{"    "}{"    • "}{ROOMS.CRITERIA.NO_TYPE_CONFLICTS}</color>";
        }
        else
        {
          foreach (RoomConstraints.Constraint additionalConstraint in type1.additional_constraints)
          {
            if (!additionalConstraint.isSatisfied(room))
            {
              string empty = string.Empty;
              string str4 = additionalConstraint.building_criteria == null ? string.Format((string) ROOMS.CRITERIA.CRITERIA_FAILED.FAILED, (object) additionalConstraint.name) : string.Format((string) ROOMS.CRITERIA.CRITERIA_FAILED.MISSING_BUILDING, (object) additionalConstraint.name);
              str3 = $"{str3}\n<color=#F44A47FF>        • {str4}</color>";
            }
          }
        }
      }
    }
    return str3;
  }

  private static bool CheckOrnament(KPrefabID entityOrBuilding)
  {
    if ((UnityEngine.Object) entityOrBuilding == (UnityEngine.Object) null || !entityOrBuilding.HasTag(GameTags.OrnamentDisplayer))
      return false;
    OrnamentReceptacle component = entityOrBuilding.GetComponent<OrnamentReceptacle>();
    return (UnityEngine.Object) component.Occupant != (UnityEngine.Object) null && component.Occupant.HasTag(GameTags.Ornament) && ((UnityEngine.Object) component.operational == (UnityEngine.Object) null || component.operational.IsOperational);
  }

  public static class ConstraintTags
  {
    public static List<Tag> AllTags = new List<Tag>();
    public static Tag BedType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (BedType).ToTag());
    public static Tag LuxuryBedType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (LuxuryBedType).ToTag());
    public static Tag ToiletType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (ToiletType).ToTag());
    public static Tag BionicUpkeepType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("BionicUpkeep".ToTag());
    public static Tag FlushToiletType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (FlushToiletType).ToTag());
    public static Tag MessTable = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (MessTable).ToTag());
    public static Tag DiningTableType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (DiningTableType).ToTag());
    public static Tag Clinic = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (Clinic).ToTag());
    public static Tag WashStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (WashStation).ToTag());
    public static Tag AdvancedWashStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (AdvancedWashStation).ToTag());
    public static Tag ScienceBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (ScienceBuilding).ToTag());
    public static Tag MassageTable = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (MassageTable).ToTag());
    public static Tag DeStressingBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (DeStressingBuilding).ToTag());
    public static Tag IndustrialMachinery = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (IndustrialMachinery).ToTag());
    public static Tag GeneratorType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (GeneratorType).ToTag());
    public static Tag HeavyDutyGeneratorType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (HeavyDutyGeneratorType).ToTag());
    public static Tag LightDutyGeneratorType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (LightDutyGeneratorType).ToTag());
    public static Tag PowerBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (PowerBuilding).ToTag());
    public static Tag FarmStationType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (FarmStationType).ToTag());
    public static Tag RanchStationType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (RanchStationType).ToTag());
    public static Tag SpiceStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (SpiceStation).ToTag());
    public static Tag CookTop = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (CookTop).ToTag());
    public static Tag Refrigerator = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (Refrigerator).ToTag());
    public static Tag KitchenRefrigerator = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (KitchenRefrigerator).ToTag());
    public static Tag RecBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (RecBuilding).ToTag());
    public static Tag MachineShopType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (MachineShopType).ToTag());
    public static Tag Park = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (Park).ToTag());
    public static Tag NatureReserve = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (NatureReserve).ToTag());
    public static Tag RocketInterior = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (RocketInterior).ToTag());
    public static Tag Decoration = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(GameTags.Decoration);
    public static Tag Ornament = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (Ornament).ToTag());
    public static Tag WarmingStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (WarmingStation).ToTag());
    public static Tag Submergible = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(nameof (Submergible).ToTag());
    [Obsolete("The light requirement constraint in rooms has been removed. Please update any references of RoomConstraints.LightSource to GameTags.Lightsource")]
    public static Tag LightSource = nameof (LightSource).ToTag();

    public static Tag DecorFancy { get; internal set; }

    public static string GetRoomConstraintLabelText(Tag tag)
    {
      StringEntry result = (StringEntry) null;
      string str = $"STRINGS.ROOMS.CRITERIA.{tag.ToString().ToUpper()}.NAME";
      return !Strings.TryGet(new StringKey(str), out result) ? ROOMS.CRITERIA.IN_CODE_ERROR.text.Replace("{0}", str) : (string) result;
    }
  }

  public class Constraint
  {
    public string name;
    public string description;
    public string conflictDescription;
    public int times_required = 1;
    public Func<Room, bool> room_criteria;
    public Func<KPrefabID, bool> building_criteria;
    public Func<KPrefabID, bool> creature_criteria;
    public List<RoomConstraints.Constraint> stomp_in_conflict;

    public Constraint(
      Func<KPrefabID, bool> building_criteria,
      Func<Room, bool> room_criteria,
      int times_required = 1,
      string name = "",
      string description = "",
      List<RoomConstraints.Constraint> stomp_in_conflict = null,
      string overrideConstraintConflictName = null)
      : this((Func<KPrefabID, bool>) null, building_criteria, room_criteria, times_required, name, description, stomp_in_conflict, overrideConstraintConflictName)
    {
    }

    public Constraint(
      Func<KPrefabID, bool> creature_criteria,
      Func<KPrefabID, bool> building_criteria,
      Func<Room, bool> room_criteria,
      int times_required = 1,
      string name = "",
      string description = "",
      List<RoomConstraints.Constraint> stomp_in_conflict = null,
      string overrideConstraintConflictName = null)
    {
      this.creature_criteria = creature_criteria;
      this.room_criteria = room_criteria;
      this.building_criteria = building_criteria;
      this.times_required = times_required;
      this.name = name;
      this.description = description;
      this.stomp_in_conflict = stomp_in_conflict;
      this.conflictDescription = overrideConstraintConflictName == null ? name : overrideConstraintConflictName;
    }

    public bool isSatisfied(Room room)
    {
      int num = 0;
      if (this.room_criteria != null && !this.room_criteria(room))
        return false;
      if (this.building_criteria != null)
      {
        for (int index = 0; num < this.times_required && index < room.buildings.Count; ++index)
        {
          KPrefabID building = room.buildings[index];
          if (!((UnityEngine.Object) building == (UnityEngine.Object) null) && this.building_criteria(building))
            ++num;
        }
        for (int index = 0; num < this.times_required && index < room.plants.Count; ++index)
        {
          KPrefabID plant = room.plants[index];
          if (!((UnityEngine.Object) plant == (UnityEngine.Object) null) && this.building_criteria(plant))
            ++num;
        }
        return num >= this.times_required;
      }
      Func<KPrefabID, bool> creatureCriteria = this.creature_criteria;
      return true;
    }
  }
}
