// Decompiled with JetBrains decompiler
// Type: Dumpable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Dumpable")]
public class Dumpable : Workable
{
  private Chore chore;
  [Serialize]
  private bool isMarkedForDumping;
  private static readonly EventSystem.IntraObjectHandler<Dumpable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Dumpable>((Action<Dumpable, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Dumpable>(493375141, Dumpable.OnRefreshUserMenuDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.isMarkedForDumping)
      this.CreateChore();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_dumpable_kanim")
    };
    this.workAnims = new HashedString[1]
    {
      (HashedString) "working"
    };
    this.synchronizeAnims = false;
    this.SetWorkTime(1f);
  }

  public void ToggleDumping()
  {
    if (DebugHandler.InstantBuildMode)
      this.OnCompleteWork((WorkerBase) null);
    else if (this.isMarkedForDumping)
    {
      this.isMarkedForDumping = false;
      this.chore.Cancel("Cancel Dumping!");
      Prioritizable.RemoveRef(this.gameObject);
      this.chore = (Chore) null;
      this.ShowProgressBar(false);
    }
    else
    {
      this.isMarkedForDumping = true;
      this.CreateChore();
    }
  }

  private void CreateChore()
  {
    if (this.chore != null)
      return;
    Prioritizable.AddRef(this.gameObject);
    this.chore = (Chore) new WorkChore<Dumpable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    this.isMarkedForDumping = false;
    this.chore = (Chore) null;
    this.Dump();
    Prioritizable.RemoveRef(this.gameObject);
  }

  public void Dump() => this.Dump(this.transform.GetPosition());

  public void Dump(Vector3 pos)
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    if ((double) component.Mass > 0.0)
    {
      if (component.Element.IsLiquid)
        FallingWater.instance.AddParticle(Grid.PosToCell(pos), component.Element.idx, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true);
      else
        SimMessages.AddRemoveSubstance(Grid.PosToCell(pos), component.ElementID, CellEventLogger.Instance.Dumpable, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount);
    }
    Util.KDestroyGameObject(this.gameObject);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (this.HasTag(GameTags.Stored))
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.isMarkedForDumping ? new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.DUMP.NAME_OFF, new System.Action(this.ToggleDumping), tooltipText: (string) UI.USERMENUACTIONS.DUMP.TOOLTIP_OFF) : new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.DUMP.NAME, new System.Action(this.ToggleDumping), tooltipText: (string) UI.USERMENUACTIONS.DUMP.TOOLTIP));
  }
}
