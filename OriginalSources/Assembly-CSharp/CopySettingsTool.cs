// Decompiled with JetBrains decompiler
// Type: CopySettingsTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CopySettingsTool : DragTool
{
  public static CopySettingsTool Instance;
  public GameObject Placer;
  private GameObject sourceGameObject;
  private readonly Dictionary<GameObject, KPrefabID> targets = new Dictionary<GameObject, KPrefabID>();

  public static void DestroyInstance() => CopySettingsTool.Instance = (CopySettingsTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    CopySettingsTool.Instance = this;
  }

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  public void SetSourceObject(GameObject sourceGameObject)
  {
    this.sourceGameObject = sourceGameObject;
  }

  protected override void OnDragTool(int cell, int _distFromOrigin)
  {
    if ((Object) this.sourceGameObject == (Object) null)
      return;
    DebugUtil.DevAssert(Grid.IsValidCell(cell), "DragTool only calls us with valid cells");
    KPrefabID kprefabId = CopyBuildingSettings.ResolveTarget(CopyBuildingSettings.ResolveLayer(this.sourceGameObject), cell);
    if (!((Object) kprefabId != (Object) null) || !((Object) kprefabId.gameObject != (Object) this.sourceGameObject))
      return;
    this.targets.TryAdd(kprefabId.gameObject, kprefabId);
  }

  protected override void OnDragComplete(Vector3 _cursorDown, Vector3 _cursorUp)
  {
    if ((Object) this.sourceGameObject != (Object) null)
    {
      KPrefabID component1;
      this.sourceGameObject.TryGetComponent<KPrefabID>(out component1);
      CopyBuildingSettings component2;
      this.sourceGameObject.TryGetComponent<CopyBuildingSettings>(out component2);
      if ((Object) component1 != (Object) null && (Object) component2 != (Object) null)
      {
        foreach (KPrefabID other_id in this.targets.Values)
          CopyBuildingSettings.ApplyCopy(other_id, this.sourceGameObject, component1, component2);
      }
    }
    this.targets.Clear();
  }

  protected override void OnActivateTool() => base.OnActivateTool();

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    this.sourceGameObject = (GameObject) null;
  }
}
