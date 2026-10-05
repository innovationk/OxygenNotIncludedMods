// Decompiled with JetBrains decompiler
// Type: CancellableMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CancellableMove : Cancellable
{
  [Serialize]
  private List<Ref<Movable>> movables = new List<Ref<Movable>>();
  private MovePickupableChore fetchChore;

  public List<Ref<Movable>> movingObjects => this.movables;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Prioritizable component = this.GetComponent<Prioritizable>();
    if (!component.IsPrioritizable())
      component.AddRef();
    if (this.fetchChore == null)
    {
      GameObject nextTarget = this.GetNextTarget();
      if ((UnityEngine.Object) nextTarget != (UnityEngine.Object) null && !nextTarget.IsNullOrDestroyed())
      {
        this.fetchChore = new MovePickupableChore((IStateMachineTarget) this, nextTarget, new Action<Chore>(this.OnChoreEnd));
      }
      else
      {
        Debug.LogWarning((object) "MovePickupable spawned with no objects to move. Destroying placer.");
        Util.KDestroyGameObject(this.gameObject);
        return;
      }
    }
    this.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
    this.Subscribe(2127324410, new Action<object>(((Cancellable) this).OnCancel));
    this.GetComponent<KPrefabID>().AddTag(GameTags.HasChores);
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    Grid.Objects[cell, 44] = this.gameObject;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    Grid.Objects[cell, 44] = (GameObject) null;
    Prioritizable.RemoveRef(this.gameObject);
  }

  public void CancelAll() => this.OnCancel();

  public void OnCancel(Movable cancel_movable = null)
  {
    for (int index = this.movables.Count - 1; index >= 0; --index)
    {
      Ref<Movable> movable1 = this.movables[index];
      if (movable1 != null)
      {
        Movable movable2 = movable1.Get();
        if ((UnityEngine.Object) cancel_movable == (UnityEngine.Object) null || (UnityEngine.Object) movable2 == (UnityEngine.Object) cancel_movable)
        {
          movable2.ClearMove();
          this.movables.RemoveAt(index);
        }
      }
    }
    if (this.fetchChore == null)
      return;
    this.fetchChore.Cancel("CancelMove");
    if (!((UnityEngine.Object) this.fetchChore.driver == (UnityEngine.Object) null) || this.movables.Count > 0)
      return;
    Util.KDestroyGameObject(this.gameObject);
  }

  protected override void OnCancel(object data) => this.OnCancel();

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_control", (string) UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.CancelAll), tooltipText: (string) UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF));
  }

  public void SetMovable(Movable movable)
  {
    if (this.fetchChore == null)
      this.fetchChore = new MovePickupableChore((IStateMachineTarget) this, movable.gameObject, new Action<Chore>(this.OnChoreEnd));
    if (this.movables.Find((Predicate<Ref<Movable>>) (move => (UnityEngine.Object) move.Get() == (UnityEngine.Object) movable)) != null)
      return;
    this.movables.Add(new Ref<Movable>(movable));
  }

  public void OnChoreEnd(Chore chore)
  {
    GameObject nextTarget = this.GetNextTarget();
    if ((UnityEngine.Object) nextTarget == (UnityEngine.Object) null)
      Util.KDestroyGameObject(this.gameObject);
    else
      this.fetchChore = new MovePickupableChore((IStateMachineTarget) this, nextTarget, new Action<Chore>(this.OnChoreEnd));
  }

  public bool IsDeliveryComplete()
  {
    this.ValidateMovables();
    return this.movables.Count <= 0;
  }

  public void RemoveMovable(Movable moved)
  {
    for (int index = this.movables.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.movables[index].Get() == (UnityEngine.Object) null || (UnityEngine.Object) this.movables[index].Get() == (UnityEngine.Object) moved)
        this.movables.RemoveAt(index);
    }
    if (this.movables.Count > 0)
      return;
    this.OnCancel();
  }

  public GameObject GetNextTarget()
  {
    this.ValidateMovables();
    return this.movables.Count > 0 ? this.movables[0].Get().gameObject : (GameObject) null;
  }

  private void ValidateMovables()
  {
    for (int index = this.movables.Count - 1; index >= 0; --index)
    {
      if (this.movables[index] == null)
      {
        this.movables.RemoveAt(index);
      }
      else
      {
        Movable cmp = this.movables[index].Get();
        if ((UnityEngine.Object) cmp == (UnityEngine.Object) null)
          this.movables.RemoveAt(index);
        else if (Grid.PosToCell((KMonoBehaviour) cmp) == Grid.PosToCell((KMonoBehaviour) this))
        {
          cmp.ClearMove();
          this.movables.RemoveAt(index);
        }
      }
    }
  }
}
