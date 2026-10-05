// Decompiled with JetBrains decompiler
// Type: AlertVignette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AlertVignette : KMonoBehaviour
{
  public Image image;
  public int worldID;

  protected override void OnSpawn() => base.OnSpawn();

  private void Update()
  {
    Color color = this.image.color;
    if ((Object) ClusterManager.Instance.GetWorld(this.worldID) == (Object) null)
    {
      this.image.color = Color.clear;
    }
    else
    {
      if (ClusterManager.Instance.GetWorld(this.worldID).IsRedAlert())
      {
        if ((double) color.r != (double) Vignette.Instance.redAlertColor.r || (double) color.g != (double) Vignette.Instance.redAlertColor.g || (double) color.b != (double) Vignette.Instance.redAlertColor.b)
          color = Vignette.Instance.redAlertColor;
      }
      else if (ClusterManager.Instance.GetWorld(this.worldID).IsYellowAlert())
      {
        if ((double) color.r != (double) Vignette.Instance.yellowAlertColor.r || (double) color.g != (double) Vignette.Instance.yellowAlertColor.g || (double) color.b != (double) Vignette.Instance.yellowAlertColor.b)
          color = Vignette.Instance.yellowAlertColor;
      }
      else
        color = Color.clear;
      if (color != Color.clear)
        color.a = (float) (0.20000000298023224 + (0.5 + (double) Mathf.Sin((float) ((double) Time.unscaledTime * 4.0 - 1.0)) / 2.0) * 0.5);
      if (!(this.image.color != color))
        return;
      this.image.color = color;
    }
  }
}
