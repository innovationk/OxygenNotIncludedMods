// Decompiled with JetBrains decompiler
// Type: SafeCellSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

#nullable disable
public class SafeCellSensor : Sensor
{
  private MinionBrain brain;
  private Navigator navigator;
  private KPrefabID prefabid;
  private Traits traits;
  private int cell = Grid.InvalidCell;
  private Dictionary<string, SafeCellQuery.SafeFlags> ignoredFlagsSets = new Dictionary<string, SafeCellQuery.SafeFlags>();

  private SafeCellQuery.SafeFlags GetIgnoredFlags()
  {
    SafeCellQuery.SafeFlags ignoredFlags = (SafeCellQuery.SafeFlags) 0;
    foreach (string key in this.ignoredFlagsSets.Keys)
    {
      SafeCellQuery.SafeFlags ignoredFlagsSet = this.ignoredFlagsSets[key];
      ignoredFlags |= ignoredFlagsSet;
    }
    return ignoredFlags;
  }

  public void AddIgnoredFlagsSet(string setID, SafeCellQuery.SafeFlags flagsToIgnore)
  {
    if (this.ignoredFlagsSets.ContainsKey(setID))
      this.ignoredFlagsSets[setID] = flagsToIgnore;
    else
      this.ignoredFlagsSets.Add(setID, flagsToIgnore);
  }

  public void RemoveIgnoredFlagsSet(string setID)
  {
    if (!this.ignoredFlagsSets.ContainsKey(setID))
      return;
    this.ignoredFlagsSets.Remove(setID);
  }

  public SafeCellSensor(Sensors sensors, bool startEnabled = true)
    : base(sensors, startEnabled)
  {
    this.navigator = this.GetComponent<Navigator>();
    this.brain = this.GetComponent<MinionBrain>();
    this.prefabid = this.GetComponent<KPrefabID>();
    this.traits = this.GetComponent<Traits>();
  }

  public override void Update()
  {
    if (!this.prefabid.HasTag(GameTags.Idle))
    {
      this.cell = Grid.InvalidCell;
    }
    else
    {
      bool flag1 = this.HasSafeCell();
      this.RunSafeCellQuery(false);
      bool flag2 = this.HasSafeCell();
      if (flag2 == flag1)
        return;
      if (flag2)
        this.sensors.Trigger(982561777, (object) null);
      else
        this.sensors.Trigger(506919987, (object) null);
    }
  }

  public void RunSafeCellQuery(bool avoid_light)
  {
    this.cell = this.RunAndGetSafeCellQueryResult(avoid_light);
    if (this.cell != Grid.PosToCell((KMonoBehaviour) this.navigator))
      return;
    this.cell = Grid.InvalidCell;
  }

  public int RunAndGetSafeCellQueryResult(bool avoid_light)
  {
    MinionPathFinderAbilities currentAbilities = (MinionPathFinderAbilities) this.navigator.GetCurrentAbilities();
    currentAbilities.SetIdleNavMaskEnabled(true);
    PathFinderQuery query = (PathFinderQuery) PathFinderQueries.safeCellQuery.Reset(this.brain, avoid_light, this.GetIgnoredFlags());
    this.navigator.RunQuery(query);
    currentAbilities.SetIdleNavMaskEnabled(false);
    this.cell = query.GetResultCell();
    return this.cell;
  }

  public int GetSensorCell() => this.cell;

  public int GetCellQuery()
  {
    if (this.cell == Grid.InvalidCell)
      this.RunSafeCellQuery(false);
    return this.cell;
  }

  public int GetSleepCellQuery()
  {
    if (this.cell == Grid.InvalidCell)
      this.RunSafeCellQuery(!this.traits.HasTrait("NightLight"));
    return this.cell;
  }

  public bool HasSafeCell()
  {
    return this.cell != Grid.InvalidCell && this.cell != Grid.PosToCell((KMonoBehaviour) this.sensors);
  }
}
