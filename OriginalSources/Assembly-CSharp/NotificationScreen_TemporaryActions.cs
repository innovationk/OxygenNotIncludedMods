// Decompiled with JetBrains decompiler
// Type: NotificationScreen_TemporaryActions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class NotificationScreen_TemporaryActions : KMonoBehaviour
{
  public TemporaryActionRow originalRow;
  private List<TemporaryActionRow> rows = new List<TemporaryActionRow>();
  private TemporaryActionRow cameraReturnRow;
  private Vector3 cameraPositionToReturnTo = Vector3.zero;
  private const float CAMERA_RETURN_BUTTON_LIFETIME = 10f;

  public static NotificationScreen_TemporaryActions Instance { get; private set; }

  public static void DestroyInstance()
  {
    NotificationScreen_TemporaryActions.Instance = (NotificationScreen_TemporaryActions) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    NotificationScreen_TemporaryActions.Instance = this;
    this.originalRow.gameObject.SetActive(false);
  }

  private TemporaryActionRow CreateActionRow()
  {
    TemporaryActionRow actionRow = Util.KInstantiateUI<TemporaryActionRow>(this.originalRow.gameObject, this.originalRow.transform.parent.gameObject);
    actionRow.gameObject.SetActive(true);
    actionRow.transform.SetAsLastSibling();
    this.rows.Add(actionRow);
    return actionRow;
  }

  private void RemoveRow(TemporaryActionRow row)
  {
    if (this.rows.Contains(row))
      this.rows.Remove(row);
    row.OnRowHidden = (Action<TemporaryActionRow>) null;
    row.gameObject.DeleteObject();
  }

  protected override void OnCleanUp()
  {
    if (this.rows != null)
    {
      foreach (TemporaryActionRow row in this.rows.ToArray())
      {
        if ((UnityEngine.Object) row != (UnityEngine.Object) null)
          this.RemoveRow(row);
      }
      this.rows.Clear();
    }
    base.OnCleanUp();
  }

  public void CreateCameraReturnActionButton(Vector3 positionToReturnTo)
  {
    if ((UnityEngine.Object) this.cameraReturnRow == (UnityEngine.Object) null)
    {
      this.cameraReturnRow = this.CreateActionRow();
      this.cameraReturnRow.Setup((string) UI.TEMPORARY_ACTIONS.CAMERA_RETURN.NAME, (string) UI.TEMPORARY_ACTIONS.CAMERA_RETURN.TOOLTIP, Assets.GetSprite((HashedString) "action_follow_cam"));
      this.cameraReturnRow.gameObject.name = "TemporaryActionRow_CameraReturn";
      this.cameraPositionToReturnTo = positionToReturnTo;
      this.cameraReturnRow.OnRowHidden = new Action<TemporaryActionRow>(this.RemoveRow);
      this.cameraReturnRow.OnRowClicked = new Action<TemporaryActionRow>(this.OnCameraReturnActionButtonClicked);
    }
    this.cameraReturnRow.SetLifetime(10f);
  }

  private void OnCameraReturnActionButtonClicked(TemporaryActionRow row)
  {
    if (!(this.cameraPositionToReturnTo != Vector3.zero))
      return;
    GameUtil.FocusCamera(this.cameraPositionToReturnTo, show_back_button: false);
  }
}
