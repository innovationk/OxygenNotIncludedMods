// Decompiled with JetBrains decompiler
// Type: NotificationAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationAnimator : MonoBehaviour
{
  private const float START_SPEED = 1f;
  private const float ACCELERATION = 0.5f;
  private const float BOUNCE_DAMPEN = 2f;
  private const int BOUNCE_COUNT = 2;
  private const float OFFSETX = 100f;
  private float speed = 1f;
  private int bounceCount = 2;
  private LayoutElement layoutElement;
  [SerializeField]
  private bool animating = true;

  public void Begin(bool startOffset = true)
  {
    this.Reset();
    this.animating = true;
    if (startOffset)
    {
      this.layoutElement.minWidth = 100f;
    }
    else
    {
      this.layoutElement.minWidth = 1f;
      this.speed = -10f;
    }
  }

  private void Reset()
  {
    this.bounceCount = 2;
    this.layoutElement = this.GetComponent<LayoutElement>();
    this.layoutElement.minWidth = 0.0f;
    this.speed = 1f;
  }

  public void Stop()
  {
    this.Reset();
    this.animating = false;
  }

  private void LateUpdate()
  {
    if (!this.animating)
      return;
    this.layoutElement.minWidth -= this.speed;
    this.speed += 0.5f;
    if ((double) this.layoutElement.minWidth > 0.0)
      return;
    if (this.bounceCount > 0)
    {
      --this.bounceCount;
      this.speed = -this.speed / Mathf.Pow(2f, (float) (2 - this.bounceCount));
      this.layoutElement.minWidth = -this.speed;
    }
    else
    {
      this.layoutElement.minWidth = 0.0f;
      this.Stop();
    }
  }
}
