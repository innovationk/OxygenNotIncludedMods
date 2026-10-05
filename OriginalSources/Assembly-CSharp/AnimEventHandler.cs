// Decompiled with JetBrains decompiler
// Type: AnimEventHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AnimEventHandler")]
public class AnimEventHandler : KMonoBehaviour
{
  [MyCmpGet]
  private KBatchedAnimController controller;
  [MyCmpGet]
  private KBoxCollider2D animCollider;
  [MyCmpGet]
  private Navigator navigator;
  private Pickupable pickupable;
  private Vector3 targetPos;
  public Transform cachedTransform;
  public Vector2 baseOffset;
  private HashedString context;

  private event AnimEventHandler.SetPos onWorkTargetSet;

  public int GetCachedCell() => this.pickupable.cachedCell;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.cachedTransform = this.transform;
    this.pickupable = this.GetComponent<Pickupable>();
    foreach (KBatchedAnimTracker componentsInChild in this.GetComponentsInChildren<KBatchedAnimTracker>(true))
    {
      if (componentsInChild.useTargetPoint)
        this.onWorkTargetSet += new AnimEventHandler.SetPos(componentsInChild.SetTarget);
    }
    this.baseOffset = this.animCollider.offset;
    AnimEventHandlerManager.Instance.Add(this);
  }

  protected override void OnCleanUp() => AnimEventHandlerManager.Instance.Remove(this);

  protected override void OnForcedCleanUp()
  {
    this.navigator = (Navigator) null;
    base.OnForcedCleanUp();
  }

  public HashedString GetContext() => this.context;

  public void UpdateWorkTarget(Vector3 pos)
  {
    if (this.onWorkTargetSet == null)
      return;
    this.onWorkTargetSet(pos);
  }

  public void SetContext(HashedString context) => this.context = context;

  public void SetTargetPos(Vector3 target_pos) => this.targetPos = target_pos;

  public Vector3 GetTargetPos() => this.targetPos;

  public void ClearContext() => this.context = new HashedString();

  public void UpdateOffset()
  {
    Vector3 pivotSymbolPosition = this.controller.GetPivotSymbolPosition();
    Vector3 controllerOffset = (Vector3) this.navigator.NavGrid.GetNavTypeData(this.navigator.CurrentNavType).animControllerOffset;
    Vector3 position = this.cachedTransform.position;
    Vector2 vector2 = new Vector2(this.baseOffset.x + pivotSymbolPosition.x - position.x - controllerOffset.x, this.baseOffset.y + pivotSymbolPosition.y - position.y + controllerOffset.y);
    if (!(this.animCollider.offset != vector2))
      return;
    this.animCollider.offset = vector2;
  }

  private delegate void SetPos(Vector3 pos);
}
