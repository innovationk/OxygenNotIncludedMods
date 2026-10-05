// Decompiled with JetBrains decompiler
// Type: TrapTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TrapTrigger : KMonoBehaviour
{
  private HandleVector<int>.Handle partitionerEntry;
  public Func<GameObject, bool> customConditionsToTrap;
  public Tag[] trappableCreatures;
  public Vector2 trappedOffset = Vector2.zero;
  public bool addTrappedAnimationOffset = true;
  [MyCmpReq]
  private Storage storage;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetTriggerCell(Grid.PosToCell(this.gameObject));
    foreach (GameObject go in this.storage.items)
    {
      this.SetStoredPosition(go);
      KBoxCollider2D component = go.GetComponent<KBoxCollider2D>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.enabled = true;
    }
  }

  public void SetTriggerCell(int cell)
  {
    HandleVector<int>.Handle partitionerEntry = this.partitionerEntry;
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Trap", (object) this.gameObject, cell, GameScenePartitioner.Instance.trapsLayer, new Action<object>(this.OnCreatureOnTrap));
  }

  public void SetStoredPosition(GameObject go)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return;
    KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
    Vector3 posCbc = Grid.CellToPosCBC(Grid.PosToCell(this.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
    if (this.addTrappedAnimationOffset)
    {
      posCbc.x += this.trappedOffset.x - component.Offset.x;
      posCbc.y += this.trappedOffset.y - component.Offset.y;
    }
    else
    {
      posCbc.x += this.trappedOffset.x;
      posCbc.y += this.trappedOffset.y;
    }
    go.transform.SetPosition(posCbc);
    go.GetComponent<Pickupable>().UpdateCachedCell(Grid.PosToCell(posCbc));
    component.SetSceneLayer(Grid.SceneLayer.BuildingFront);
  }

  public void OnCreatureOnTrap(object data)
  {
    if (!this.enabled || !this.storage.IsEmpty())
      return;
    Trappable cmp = (Trappable) data;
    if (cmp.HasTag(GameTags.Stored) || cmp.HasTag(GameTags.Trapped) || cmp.HasTag(GameTags.Creatures.Bagged))
      return;
    bool flag = false;
    foreach (Tag trappableCreature in this.trappableCreatures)
    {
      if (cmp.HasTag(trappableCreature))
      {
        flag = true;
        break;
      }
    }
    if (!flag || this.customConditionsToTrap != null && !this.customConditionsToTrap(cmp.gameObject))
      return;
    this.storage.Store(cmp.gameObject, true);
    this.SetStoredPosition(cmp.gameObject);
    this.Trigger(-358342870, (object) cmp.gameObject);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }
}
