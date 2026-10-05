// Decompiled with JetBrains decompiler
// Type: DragMe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class DragMe : 
  MonoBehaviour,
  IBeginDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IEndDragHandler
{
  public bool dragOnSurfaces = true;
  private GameObject m_DraggingIcon;
  private RectTransform m_DraggingPlane;
  private float x;
  public DragMe.IDragListener listener;

  public void OnBeginDrag(PointerEventData eventData)
  {
    if (eventData.button != PointerEventData.InputButton.Left)
      return;
    Canvas inParents = DragMe.FindInParents<Canvas>(this.gameObject);
    if ((Object) inParents == (Object) null)
      return;
    this.m_DraggingIcon = Object.Instantiate<GameObject>(this.gameObject, inParents.transform, false);
    GraphicRaycaster component1 = this.m_DraggingIcon.GetComponent<GraphicRaycaster>();
    if ((Object) component1 != (Object) null)
      component1.enabled = false;
    this.m_DraggingIcon.name = "dragObj";
    this.m_DraggingIcon.transform.SetAsLastSibling();
    RectTransform component2 = this.m_DraggingIcon.GetComponent<RectTransform>();
    component2.pivot = Vector2.zero;
    component2.sizeDelta = this.GetComponent<RectTransform>().rect.size;
    this.x = this.m_DraggingIcon.transform.position.x;
    Canvas component3 = this.m_DraggingIcon.GetComponent<Canvas>();
    component3.overrideSorting = true;
    component3.sortingOrder = 99;
    this.m_DraggingPlane = !this.dragOnSurfaces ? inParents.transform as RectTransform : this.transform as RectTransform;
    this.SetDraggedPosition(eventData);
    this.listener.OnBeginDrag(eventData.position);
  }

  public void OnDrag(PointerEventData data)
  {
    if (!((Object) this.m_DraggingIcon != (Object) null))
      return;
    this.SetDraggedPosition(data);
  }

  private void SetDraggedPosition(PointerEventData data)
  {
    if (this.dragOnSurfaces && (Object) data.pointerEnter != (Object) null && (Object) (data.pointerEnter.transform as RectTransform) != (Object) null)
      this.m_DraggingPlane = data.pointerEnter.transform as RectTransform;
    RectTransform component = this.m_DraggingIcon.GetComponent<RectTransform>();
    Vector3 worldPoint;
    if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, data.position, data.pressEventCamera, out worldPoint))
      return;
    worldPoint.x = this.x + 5f;
    worldPoint.y -= component.sizeDelta.y / 2f;
    component.position = worldPoint;
    component.rotation = this.m_DraggingPlane.rotation;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    this.listener.OnEndDrag(eventData.position);
    if (!((Object) this.m_DraggingIcon != (Object) null))
      return;
    Object.Destroy((Object) this.m_DraggingIcon);
  }

  public static T FindInParents<T>(GameObject go) where T : Component
  {
    if ((Object) go == (Object) null)
      return default (T);
    T inParents = default (T);
    for (Transform parent = go.transform.parent; (Object) parent != (Object) null && (Object) inParents == (Object) null; parent = parent.parent)
      inParents = parent.gameObject.GetComponent<T>();
    return inParents;
  }

  public interface IDragListener
  {
    void OnBeginDrag(Vector2 position);

    void OnEndDrag(Vector2 position);
  }
}
