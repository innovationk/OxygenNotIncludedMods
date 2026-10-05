// Decompiled with JetBrains decompiler
// Type: DeathLoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class DeathLoot : 
  GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>
{
  private StateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.BoolParameter WasLoopDropped;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.root;
  }

  public class Loot
  {
    public float Quantity;

    public Tag Id { private set; get; } = Tag.Invalid;

    public bool IsElement { private set; get; }

    public Loot(Tag tag)
    {
      this.Id = tag;
      this.IsElement = false;
      this.Quantity = 1f;
    }

    public Loot(SimHashes element, float quantity)
    {
      this.Id = element.CreateTag();
      this.IsElement = true;
      this.Quantity = quantity;
    }
  }

  public class Def : StateMachine.BaseDef
  {
    public DeathLoot.Loot[] loot;
    public CellOffset lootSpawnOffset;
  }

  public new class Instance : 
    GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.GameInstance
  {
    private int onDeathHandler = -1;

    public bool WasLoopDropped => this.sm.WasLoopDropped.Get(this.smi);

    public Instance(IStateMachineTarget master, DeathLoot.Def def)
      : base(master, def)
    {
      this.onDeathHandler = this.Subscribe(1623392196, new System.Action<object>(this.OnDeath));
    }

    private void OnDeath(object obj)
    {
      if (this.WasLoopDropped)
        return;
      this.sm.WasLoopDropped.Set(true, this);
      this.CreateLoot();
    }

    public GameObject[] CreateLoot()
    {
      if (this.def.loot == null)
        return (GameObject[]) null;
      GameObject[] loot1 = new GameObject[this.def.loot.Length];
      for (int index = 0; index < this.def.loot.Length; ++index)
      {
        DeathLoot.Loot loot2 = this.def.loot[index];
        if (!(loot2.Id == Tag.Invalid))
        {
          GameObject go = Scenario.SpawnPrefab(this.GetLootSpawnCell(), 0, 0, loot2.Id.ToString());
          go.SetActive(true);
          Edible component1 = go.GetComponent<Edible>();
          if ((bool) (UnityEngine.Object) component1)
            ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component1.Calories, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", go.GetProperName()), (string) UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
          if (loot2.IsElement)
          {
            PrimaryElement component2 = go.GetComponent<PrimaryElement>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
              component2.Mass = loot2.Quantity;
          }
          loot1[index] = go;
        }
      }
      return loot1;
    }

    public int GetLootSpawnCell()
    {
      int cell1 = Grid.PosToCell(this.gameObject);
      int cell2 = Grid.OffsetCell(cell1, this.def.lootSpawnOffset);
      return Grid.IsWorldValidCell(cell2) && Grid.IsValidCellInWorld(cell2, this.gameObject.GetMyWorldId()) ? cell2 : cell1;
    }

    protected override void OnCleanUp() => this.Unsubscribe(ref this.onDeathHandler);
  }
}
