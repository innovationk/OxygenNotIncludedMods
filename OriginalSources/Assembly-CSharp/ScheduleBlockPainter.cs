// Decompiled with JetBrains decompiler
// Type: ScheduleBlockPainter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockPainter")]
public class ScheduleBlockPainter : 
  KMonoBehaviour,
  IPointerDownHandler,
  IEventSystemHandler,
  IBeginDragHandler,
  IDragHandler,
  IEndDragHandler
{
  private ScheduleScreenEntry entry;
  private static int paintCounter;
  private GameObject previousBlockTriedPainted;

  public void SetEntry(ScheduleScreenEntry entry) => this.entry = entry;

  public void OnBeginDrag(PointerEventData eventData) => this.PaintBlocksBelow(eventData);

  public void OnDrag(PointerEventData eventData) => this.PaintBlocksBelow(eventData);

  public void OnEndDrag(PointerEventData eventData) => this.PaintBlocksBelow(eventData);

  public void OnPointerDown(PointerEventData eventData)
  {
    ScheduleBlockPainter.paintCounter = 0;
    this.PaintBlocksBelow(eventData);
  }

  private void PaintBlocksBelow(PointerEventData eventData)
  {
    if (eventData.button != PointerEventData.InputButton.Left || ScheduleScreen.Instance.SelectedPaint.IsNullOrWhiteSpace())
      return;
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, raycastResults);
    if (raycastResults == null || raycastResults.Count <= 0)
      return;
    ScheduleBlockButton component = raycastResults[0].gameObject.GetComponent<ScheduleBlockButton>();
    if (!((Object) component != (Object) null))
      return;
    if (this.entry.PaintBlock(component))
    {
      string sound = GlobalAssets.GetSound("ScheduleMenu_Select");
      if (sound == null)
        return;
      EventInstance instance = SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition());
      int num = (int) instance.setParameterByName("Drag_Count", (float) ScheduleBlockPainter.paintCounter);
      ++ScheduleBlockPainter.paintCounter;
      SoundEvent.EndOneShot(instance);
      this.previousBlockTriedPainted = component.gameObject;
    }
    else
    {
      if (!((Object) this.previousBlockTriedPainted != (Object) component.gameObject))
        return;
      this.previousBlockTriedPainted = component.gameObject;
      string sound = GlobalAssets.GetSound("ScheduleMenu_Select_none");
      if (sound == null)
        return;
      SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition()));
    }
  }
}
