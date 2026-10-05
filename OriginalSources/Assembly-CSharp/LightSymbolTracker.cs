// Decompiled with JetBrains decompiler
// Type: LightSymbolTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/LightSymbolTracker")]
public class LightSymbolTracker : KMonoBehaviour, IRenderEveryTick
{
  public HashedString targetSymbol;
  private KBatchedAnimController animController;
  private Light2D light2D;
  private Pickupable pickupable;

  protected override void OnSpawn()
  {
    this.animController = this.GetComponent<KBatchedAnimController>();
    this.light2D = this.GetComponent<Light2D>();
    this.pickupable = this.GetComponent<Pickupable>();
  }

  public bool IsEnableAndVisible()
  {
    return CameraController.Instance.VisibleArea.CurrentAreaExtended.Contains(this.pickupable.cachedCell) && this.enabled;
  }

  public void RenderEveryTick(float dt)
  {
    if (!this.IsEnableAndVisible())
      return;
    Vector3 zero = Vector3.zero;
    this.light2D.Offset = (Vector2) ((this.animController.GetTransformMatrix() * this.animController.GetSymbolLocalTransform(this.targetSymbol, out bool _)).MultiplyPoint(Vector3.zero) - this.transform.position);
  }
}
