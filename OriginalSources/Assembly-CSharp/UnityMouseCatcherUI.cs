// Decompiled with JetBrains decompiler
// Type: UnityMouseCatcherUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UnityMouseCatcherUI
{
  private static Canvas m_instance_canvas;

  public static Canvas ManifestCanvas()
  {
    if ((UnityEngine.Object) UnityMouseCatcherUI.m_instance_canvas != (UnityEngine.Object) null && (bool) (UnityEngine.Object) UnityMouseCatcherUI.m_instance_canvas)
      return UnityMouseCatcherUI.m_instance_canvas;
    GameObject target = new GameObject("UnityMouseCatcherUI Canvas");
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
    Canvas canvas = target.AddComponent<Canvas>();
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    canvas.sortingOrder = (int) short.MaxValue;
    canvas.pixelPerfect = false;
    UnityMouseCatcherUI.m_instance_canvas = canvas;
    target.AddComponent<GraphicRaycaster>();
    GameObject gameObject = new GameObject("ImGui Consume Input", new System.Type[1]
    {
      typeof (RectTransform)
    });
    gameObject.transform.SetParent(target.transform, false);
    RectTransform component = gameObject.GetComponent<RectTransform>();
    component.anchorMin = Vector2.zero;
    component.anchorMax = Vector2.one;
    component.sizeDelta = Vector2.zero;
    component.anchoredPosition = Vector2.zero;
    Image image = gameObject.AddComponent<Image>();
    image.sprite = UnityEngine.Resources.Load<Sprite>("1x1_white");
    image.color = new Color(1f, 1f, 1f, 0.0f);
    image.raycastTarget = true;
    return UnityMouseCatcherUI.m_instance_canvas;
  }

  public static void SetEnabled(bool is_enabled)
  {
    Canvas canvas = UnityMouseCatcherUI.ManifestCanvas();
    if (canvas.gameObject.activeSelf != is_enabled)
      canvas.gameObject.SetActive(is_enabled);
    if (canvas.enabled == is_enabled)
      return;
    canvas.enabled = is_enabled;
  }
}
