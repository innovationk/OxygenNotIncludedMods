// Decompiled with JetBrains decompiler
// Type: CopyBuildingSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/CopyBuildingSettings")]
public class CopyBuildingSettings : KMonoBehaviour
{
  [MyCmpReq]
  private KPrefabID id;
  public Tag copyGroupTag;
  private static readonly EventSystem.IntraObjectHandler<CopyBuildingSettings> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CopyBuildingSettings>((Action<CopyBuildingSettings, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<CopyBuildingSettings>(493375141, CopyBuildingSettings.OnRefreshUserMenuDelegate);
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_mirror", (string) UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.NAME, new System.Action(this.ActivateCopyTool), Action.BuildingUtility1, tooltipText: (string) UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.TOOLTIP));
  }

  private void ActivateCopyTool()
  {
    CopySettingsTool.Instance.SetSourceObject(this.gameObject);
    PlayerController.Instance.ActivateTool((InterfaceTool) CopySettingsTool.Instance);
  }

  public static ObjectLayer ResolveLayer(GameObject sourceGameObject)
  {
    ObjectLayer objectLayer = ObjectLayer.Building;
    if (sourceGameObject.TryGetComponent<MoverLayerOccupier>(out MoverLayerOccupier _))
      objectLayer = ObjectLayer.Mover;
    BuildingComplete component;
    if (sourceGameObject.TryGetComponent<BuildingComplete>(out component))
      objectLayer = component.Def.ObjectLayer;
    return objectLayer;
  }

  public static KPrefabID ResolveTarget(ObjectLayer layer, int targetCell)
  {
    GameObject gameObject = Grid.Objects[targetCell, (int) layer];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return (KPrefabID) null;
    KPrefabID component;
    gameObject.TryGetComponent<KPrefabID>(out component);
    return component;
  }

  public static bool ApplyCopy(
    KPrefabID other_id,
    GameObject sourceGameObject,
    KPrefabID source_id,
    CopyBuildingSettings source_settings)
  {
    DebugUtil.DevAssert((UnityEngine.Object) other_id.gameObject != (UnityEngine.Object) sourceGameObject, "source and target must not be equal");
    CopyBuildingSettings component;
    if ((UnityEngine.Object) other_id.gameObject == (UnityEngine.Object) sourceGameObject || !other_id.gameObject.TryGetComponent<CopyBuildingSettings>(out component))
      return false;
    if (source_settings.copyGroupTag != Tag.Invalid)
    {
      if (source_settings.copyGroupTag != component.copyGroupTag)
        return false;
    }
    else if (other_id.PrefabID() != source_id.PrefabID())
      return false;
    other_id.Trigger(-905833192, (object) sourceGameObject);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, (string) UI.COPIED_SETTINGS, other_id.gameObject.transform, new Vector3(0.0f, 0.5f, 0.0f));
    return true;
  }
}
