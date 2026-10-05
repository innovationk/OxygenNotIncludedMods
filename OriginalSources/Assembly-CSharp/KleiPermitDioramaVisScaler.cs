// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVisScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[ExecuteAlways]
public class KleiPermitDioramaVisScaler : UIBehaviour
{
  public const float REFERENCE_WIDTH = 1700f;
  public const float REFERENCE_HEIGHT = 800f;
  [SerializeField]
  private RectTransform root;
  [SerializeField]
  private RectTransform scaleTarget;
  [SerializeField]
  private RectTransform slot;

  protected override void OnRectTransformDimensionsChange() => this.Layout();

  public void Layout() => KleiPermitDioramaVisScaler.Layout(this.root, this.scaleTarget, this.slot);

  public static void Layout(RectTransform root, RectTransform scaleTarget, RectTransform slot)
  {
    float num1 = 2.125f;
    AspectRatioFitter orAddComponent = slot.FindOrAddComponent<AspectRatioFitter>();
    orAddComponent.aspectRatio = num1;
    orAddComponent.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
    float num2 = 128f;
    float num3 = 128f;
    float num4 = 1700f;
    double a = (double) Mathf.Max(0.1f, root.rect.width - num2) / (double) num4;
    float num5 = 800f;
    double b = (double) (Mathf.Max(0.1f, root.rect.height - num3) / num5);
    float num6 = Mathf.Max((float) a, (float) b);
    scaleTarget.localScale = Vector3.one * num6;
    scaleTarget.sizeDelta = new Vector2(1700f, 800f);
    scaleTarget.anchorMin = Vector2.one * 0.5f;
    scaleTarget.anchorMax = Vector2.one * 0.5f;
    scaleTarget.pivot = Vector2.one * 0.5f;
    scaleTarget.anchoredPosition = Vector2.zero;
  }
}
