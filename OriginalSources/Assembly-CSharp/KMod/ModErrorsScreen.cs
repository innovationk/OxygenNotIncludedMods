// Decompiled with JetBrains decompiler
// Type: KMod.ModErrorsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace KMod;

public class ModErrorsScreen : KScreen
{
  [SerializeField]
  private KButton closeButtonTitle;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private GameObject entryPrefab;
  [SerializeField]
  private Transform entryParent;

  public static bool ShowErrors(List<Event> events)
  {
    if (Global.Instance.modManager.events.Count == 0)
      return false;
    ModErrorsScreen modErrorsScreen = Util.KInstantiateUI<ModErrorsScreen>(Global.Instance.modErrorsPrefab, GameObject.Find("Canvas"));
    modErrorsScreen.Initialize(events);
    modErrorsScreen.gameObject.SetActive(true);
    return true;
  }

  private void Initialize(List<Event> events)
  {
    foreach (Event @event in events)
    {
      HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject, true);
      LocText reference1 = hierarchyReferences.GetReference<LocText>("Title");
      LocText reference2 = hierarchyReferences.GetReference<LocText>("Description");
      KButton reference3 = hierarchyReferences.GetReference<KButton>("Details");
      string title;
      string title_tooltip;
      Event.GetUIStrings(@event.event_type, out title, out title_tooltip);
      reference1.text = title;
      reference1.GetComponent<ToolTip>().toolTip = title_tooltip;
      reference2.text = @event.mod.title;
      ToolTip component = reference2.GetComponent<ToolTip>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.toolTip = @event.mod.ToString();
      reference3.isInteractable = false;
      Mod mod = Global.Instance.modManager.FindMod(@event.mod);
      if (mod != null)
      {
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && !string.IsNullOrEmpty(mod.description))
        {
          StringEntry result;
          component.toolTip = !Strings.TryGet(mod.description, out result) ? mod.description : (string) result;
        }
        if (mod.on_managed != null)
        {
          reference3.onClick += mod.on_managed;
          reference3.isInteractable = true;
        }
      }
    }
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.closeButtonTitle.onClick += new System.Action(((KScreen) this).Deactivate);
    this.closeButton.onClick += new System.Action(((KScreen) this).Deactivate);
  }
}
