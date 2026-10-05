// Decompiled with JetBrains decompiler
// Type: IdleCellSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class IdleCellSensor : Sensor
{
  private MinionBrain brain;
  private Navigator navigator;
  private SwimMonitor.Instance swimMonitor;
  private KPrefabID prefabid;
  private int cell;
  private bool canSwim;

  public IdleCellSensor(Sensors sensors)
    : base(sensors)
  {
    this.navigator = this.GetComponent<Navigator>();
    this.brain = this.GetComponent<MinionBrain>();
    this.prefabid = this.GetComponent<KPrefabID>();
    this.brain.Subscribe(1589886948, new Action<object>(this.OnMinionSpawned));
  }

  private void OnMinionSpawned(object obj)
  {
    this.swimMonitor = this.brain.GetSMI<SwimMonitor.Instance>();
    this.canSwim = this.swimMonitor != null && this.swimMonitor.CanSwim();
    this.brain.Unsubscribe(1589886948);
  }

  public override void Update()
  {
    if (!this.prefabid.HasTag(GameTags.Idle))
    {
      this.cell = Grid.InvalidCell;
    }
    else
    {
      this.canSwim = this.swimMonitor != null && this.swimMonitor.CanSwim();
      MinionPathFinderAbilities currentAbilities = (MinionPathFinderAbilities) this.navigator.GetCurrentAbilities();
      currentAbilities.SetIdleNavMaskEnabled(true);
      IdleCellQuery query = PathFinderQueries.idleCellQuery.Reset(this.brain, UnityEngine.Random.Range(30, 60), this.canSwim);
      this.navigator.RunQuery((PathFinderQuery) query);
      currentAbilities.SetIdleNavMaskEnabled(false);
      this.cell = query.GetResultCell();
    }
  }

  public int GetCell() => this.cell;
}
