// Decompiled with JetBrains decompiler
// Type: KBatchedAnimTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class KBatchedAnimTracker : MonoBehaviour
{
  public KBatchedAnimController controller;
  public Vector3 offset = Vector3.zero;
  public HashedString symbol;
  public Vector3 targetPoint = Vector3.zero;
  public Vector3 previousTargetPoint;
  public bool useTargetPoint;
  public bool fadeOut = true;
  public bool forceAlwaysVisible;
  public bool matchParentOffset;
  public bool forceAlwaysAlive;
  private bool alive = true;
  private bool forceUpdate;
  private Matrix2x3 previousMatrix;
  private Vector3 previousPosition;
  public bool synchronizeEnabledState = true;
  [SerializeField]
  private KBatchedAnimController myAnim;

  private void Start()
  {
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
    {
      for (Transform parent = this.transform.parent; (UnityEngine.Object) parent != (UnityEngine.Object) null; parent = parent.parent)
      {
        this.controller = parent.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
          break;
      }
    }
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
    {
      Debug.Log((object) ("Controller Null for tracker on " + this.gameObject.name), (UnityEngine.Object) this.gameObject);
      this.enabled = false;
    }
    else
    {
      this.controller.onAnimEnter += new KAnimControllerBase.KAnimEvent(this.OnAnimStart);
      this.controller.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.OnAnimStop);
      this.controller.onLayerChanged += new Action<int>(this.OnLayerChanged);
      this.forceUpdate = true;
      if ((UnityEngine.Object) this.myAnim != (UnityEngine.Object) null)
        return;
      this.myAnim = this.GetComponent<KBatchedAnimController>();
      this.myAnim.getPositionDataFunctionInUse += new Func<Vector4>(this.MyAnimGetPosition);
    }
  }

  private Vector4 MyAnimGetPosition()
  {
    if (!((UnityEngine.Object) this.myAnim != (UnityEngine.Object) null) || !((UnityEngine.Object) this.controller != (UnityEngine.Object) null) || !((UnityEngine.Object) this.controller.transform == (UnityEngine.Object) this.myAnim.transform.parent))
      return (Vector4) this.transform.GetPosition();
    Vector3 pivotSymbolPosition = this.myAnim.GetPivotSymbolPosition();
    return new Vector4(pivotSymbolPosition.x - this.controller.Offset.x, pivotSymbolPosition.y - this.controller.Offset.y, pivotSymbolPosition.x, pivotSymbolPosition.y);
  }

  private void OnDestroy()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
    {
      this.controller.onAnimEnter -= new KAnimControllerBase.KAnimEvent(this.OnAnimStart);
      this.controller.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.OnAnimStop);
      this.controller.onLayerChanged -= new Action<int>(this.OnLayerChanged);
      this.controller = (KBatchedAnimController) null;
    }
    if ((UnityEngine.Object) this.myAnim != (UnityEngine.Object) null)
      this.myAnim.getPositionDataFunctionInUse -= new Func<Vector4>(this.MyAnimGetPosition);
    this.myAnim = (KBatchedAnimController) null;
  }

  private void LateUpdate()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && (this.controller.IsVisible() || this.forceAlwaysVisible || this.forceUpdate))
      this.UpdateFrame();
    if (this.alive)
      return;
    this.enabled = false;
  }

  public void SetAnimControllers(
    KBatchedAnimController controller,
    KBatchedAnimController parentController)
  {
    this.myAnim = controller;
    this.controller = parentController;
  }

  private void UpdateFrame()
  {
    this.forceUpdate = false;
    bool symbolVisible = false;
    if (this.controller.CurrentAnim != null)
    {
      Matrix2x3 symbolLocalTransform = this.controller.GetSymbolLocalTransform(this.symbol, out symbolVisible);
      Vector3 position1 = this.controller.transform.GetPosition();
      if (symbolVisible && (this.previousMatrix != symbolLocalTransform || position1 != this.previousPosition || this.useTargetPoint && this.targetPoint != this.previousTargetPoint || this.matchParentOffset && this.myAnim.Offset != this.controller.Offset))
      {
        this.previousMatrix = symbolLocalTransform;
        this.previousPosition = position1;
        Matrix2x3 transform_matrix = ((this.useTargetPoint ? 1 : ((UnityEngine.Object) this.myAnim == (UnityEngine.Object) null ? 1 : 0)) != 0 ? this.controller.GetTransformMatrix() : this.controller.GetTransformMatrix(new Vector2(this.myAnim.animWidth * this.myAnim.animScale, -this.myAnim.animHeight * this.myAnim.animScale))) * symbolLocalTransform;
        float z = this.transform.GetPosition().z;
        this.transform.SetPosition(transform_matrix.MultiplyPoint(this.offset));
        if (this.useTargetPoint)
        {
          this.previousTargetPoint = this.targetPoint;
          Vector3 position2 = this.transform.GetPosition() with
          {
            z = 0.0f
          };
          Vector3 from = this.targetPoint - position2;
          float angle = Vector3.Angle(from, Vector3.right);
          if ((double) from.y < 0.0)
            angle = 360f - angle;
          this.transform.localRotation = Quaternion.identity;
          this.transform.RotateAround(position2, new Vector3(0.0f, 0.0f, 1f), angle);
          float sqrMagnitude = from.sqrMagnitude;
          this.myAnim.GetBatchInstanceData().SetClipRadius(this.transform.GetPosition().x, this.transform.GetPosition().y, sqrMagnitude, true);
        }
        else
        {
          Vector3 v1 = this.controller.FlipX ? Vector3.left : Vector3.right;
          Vector3 v2 = this.controller.FlipY ? Vector3.down : Vector3.up;
          this.transform.up = transform_matrix.MultiplyVector(v2);
          this.transform.right = transform_matrix.MultiplyVector(v1);
          if ((UnityEngine.Object) this.myAnim != (UnityEngine.Object) null)
            this.myAnim.GetBatchInstanceData()?.SetOverrideTransformMatrix(transform_matrix);
        }
        this.transform.SetPosition(new Vector3(this.transform.GetPosition().x, this.transform.GetPosition().y, z));
        if (this.matchParentOffset)
          this.myAnim.Offset = this.controller.Offset;
        this.myAnim.SetDirty();
      }
    }
    if (!((UnityEngine.Object) this.myAnim != (UnityEngine.Object) null) || symbolVisible == this.myAnim.enabled || !this.synchronizeEnabledState)
      return;
    this.myAnim.enabled = symbolVisible;
  }

  [ContextMenu("ForceAlive")]
  private void OnAnimStart(HashedString name)
  {
    this.alive = true;
    this.enabled = true;
    this.forceUpdate = true;
  }

  private void OnAnimStop(HashedString name)
  {
    if (this.forceAlwaysAlive)
      return;
    this.alive = false;
  }

  private void OnLayerChanged(int layer) => this.myAnim.SetLayer(layer);

  public void SetTarget(Vector3 target)
  {
    this.targetPoint = target;
    this.targetPoint.z = 0.0f;
  }
}
