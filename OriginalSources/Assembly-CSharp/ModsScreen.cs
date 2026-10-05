// Decompiled with JetBrains decompiler
// Type: ModsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KMod;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ModsScreen : KModalScreen
{
  [SerializeField]
  private KButton closeButtonTitle;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton toggleAllButton;
  [SerializeField]
  private KButton workshopButton;
  [SerializeField]
  private GameObject entryPrefab;
  [SerializeField]
  private Transform entryParent;
  [SerializeField]
  private LocText windowTitle;
  private List<ModsScreen.DisplayedMod> displayedMods = new List<ModsScreen.DisplayedMod>();
  private List<KMod.Label> mod_footprint = new List<KMod.Label>();

  protected override void OnActivate()
  {
    base.OnActivate();
    if (Global.Instance.modManager.safe_mode_enabled)
    {
      this.windowTitle.text = (string) UI.FRONTEND.MODS.TITLE_SAFE_MODE;
      this.windowTitle.ApplySettings();
      Global.Instance.modManager.ShowSafeModeDialog(this.gameObject);
    }
    else
    {
      this.windowTitle.text = (string) UI.FRONTEND.MODS.TITLE;
      this.windowTitle.ApplySettings();
    }
    this.closeButtonTitle.onClick += new System.Action(this.Exit);
    this.closeButton.onClick += new System.Action(this.Exit);
    this.workshopButton.onClick += (System.Action) (() => App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140"));
    this.UpdateToggleAllButton();
    this.toggleAllButton.onClick += new System.Action(this.OnToggleAllClicked);
    Global.Instance.modManager.Sanitize(this.gameObject);
    this.mod_footprint.Clear();
    foreach (KMod.Mod mod in Global.Instance.modManager.mods)
    {
      if (mod.IsEnabledForActiveDlc())
      {
        this.mod_footprint.Add(mod.label);
        if ((mod.loaded_content & (Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation)) == (mod.available_content & (Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation)))
          mod.Uncrash();
      }
    }
    this.BuildDisplay();
    Global.Instance.modManager.on_update += new KMod.Manager.OnUpdate(this.RebuildDisplay);
  }

  protected override void OnDeactivate()
  {
    Global.Instance.modManager.on_update -= new KMod.Manager.OnUpdate(this.RebuildDisplay);
    base.OnDeactivate();
  }

  private void Exit()
  {
    Global.Instance.modManager.Save();
    if (!Global.Instance.modManager.MatchFootprint(this.mod_footprint, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
      Global.Instance.modManager.RestartDialog((string) UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.TITLE, (string) UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.MESSAGE, new System.Action(((KScreen) this).Deactivate), true, this.gameObject);
    else
      this.Deactivate();
    Global.Instance.modManager.events.Clear();
  }

  private void RebuildDisplay(object change_source)
  {
    if (change_source == this)
      return;
    this.BuildDisplay();
  }

  private bool ShouldDisplayMod(KMod.Mod mod)
  {
    return mod.status != KMod.Mod.Status.NotInstalled && mod.status != KMod.Mod.Status.UninstallPending && !mod.HasOnlyTranslationContent();
  }

  private void BuildDisplay()
  {
    foreach (ModsScreen.DisplayedMod displayedMod in this.displayedMods)
    {
      if ((UnityEngine.Object) displayedMod.rect_transform != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) displayedMod.rect_transform.gameObject);
    }
    this.displayedMods.Clear();
    ModsScreen.ModOrderingDragListener orderingDragListener = new ModsScreen.ModOrderingDragListener(this, this.displayedMods);
    for (int index = 0; index != Global.Instance.modManager.mods.Count; ++index)
    {
      KMod.Mod mod = Global.Instance.modManager.mods[index];
      if (this.ShouldDisplayMod(mod))
      {
        HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject);
        this.displayedMods.Add(new ModsScreen.DisplayedMod()
        {
          rect_transform = hierarchyReferences.gameObject.GetComponent<RectTransform>(),
          mod_index = index
        });
        hierarchyReferences.GetComponent<DragMe>().listener = (DragMe.IDragListener) orderingDragListener;
        LocText reference1 = hierarchyReferences.GetReference<LocText>("Title");
        string str1 = mod.title;
        StringEntry result1;
        if (Strings.TryGet(mod.title, out result1))
          str1 = (string) result1;
        hierarchyReferences.name = mod.title;
        ToolTip reference2 = hierarchyReferences.GetReference<ToolTip>("Description");
        if (mod.available_content == (Content) 0)
        {
          switch (mod.contentCompatability)
          {
            case ModContentCompatability.NoContent:
              str1 += (string) UI.FRONTEND.MODS.CONTENT_FAILURE.NO_CONTENT;
              reference2.toolTip = (string) UI.FRONTEND.MODS.CONTENT_FAILURE.NO_CONTENT_TOOLTIP;
              break;
            case ModContentCompatability.OldAPI:
              str1 += (string) UI.FRONTEND.MODS.CONTENT_FAILURE.OLD_API;
              reference2.toolTip = (string) UI.FRONTEND.MODS.CONTENT_FAILURE.OLD_API_TOOLTIP;
              break;
            default:
              string hexString = GlobalAssets.Instance.colorSet.GetColorByName("statusItemBad").ToHexString();
              string str2 = (string) UI.FRONTEND.MODS.CONTENT_FAILURE.DISABLED_CONTENT_TOOLTIP + "\n\n";
              if (mod.GetRequiredDlcIds() != null)
              {
                str2 += (string) UI.FRONTEND.MODS.CONTENT_FAILURE.DISABLED_CONTENT_TOOLTIP_REQUIRED;
                foreach (string requiredDlcId in mod.GetRequiredDlcIds())
                {
                  if (DlcManager.IsContentSubscribed(requiredDlcId))
                    str2 = $"{str2}\n     •  <i>{DlcManager.GetDlcTitleNoFormatting(requiredDlcId)}</i>";
                  else
                    str2 = $"{str2}\n     •  <i><color=#{hexString}>{DlcManager.GetDlcTitleNoFormatting(requiredDlcId)}</color></i>";
                }
                if (mod.GetForbiddenDlcIds() != null)
                  str2 += "\n\n";
              }
              if (mod.GetForbiddenDlcIds() != null)
              {
                str2 += (string) UI.FRONTEND.MODS.CONTENT_FAILURE.DISABLED_CONTENT_TOOLTIP_FORBIDDEN_DLC;
                foreach (string forbiddenDlcId in mod.GetForbiddenDlcIds())
                {
                  if (!DlcManager.IsContentSubscribed(forbiddenDlcId))
                    str2 = $"{str2}\n     •  <i>{DlcManager.GetDlcTitleNoFormatting(forbiddenDlcId)}</i>";
                  else
                    str2 = $"{str2}\n     •  <i><color=#{hexString}>{DlcManager.GetDlcTitleNoFormatting(forbiddenDlcId)}</color></i>";
                }
              }
              reference2.toolTip = str2;
              str1 += (string) UI.FRONTEND.MODS.CONTENT_FAILURE.DISABLED_CONTENT;
              break;
          }
        }
        reference1.text = str1;
        LocText reference3 = hierarchyReferences.GetReference<LocText>("Version");
        if (mod.packagedModInfo != null && mod.packagedModInfo.version != null && mod.packagedModInfo.version.Length > 0)
        {
          string str3;
          if (mod.status == KMod.Mod.Status.ReinstallPending)
          {
            str3 = (string) UI.FRONTEND.MODS.INSTALLED_VERSION_NEWER_THAN_LOADED_VERSION;
          }
          else
          {
            str3 = mod.packagedModInfo.version;
            if (str3.StartsWith("V"))
              str3 = "v" + str3.Substring(1, str3.Length - 1);
            else if (!str3.StartsWith("v"))
              str3 = "v" + str3;
          }
          reference3.text = str3;
          reference3.gameObject.SetActive(true);
        }
        else
          reference3.gameObject.SetActive(false);
        if (mod.available_content > (Content) 0)
        {
          StringEntry result2;
          reference2.toolTip = !Strings.TryGet(mod.description, out result2) ? mod.description : (string) result2;
        }
        if (mod.crash_count != 0)
          reference1.color = Color.Lerp(Color.white, Color.red, (float) mod.crash_count / 3f);
        KButton reference4 = hierarchyReferences.GetReference<KButton>("ManageButton");
        reference4.GetComponentInChildren<LocText>().text = (string) (mod.IsLocal ? UI.FRONTEND.MODS.MANAGE_LOCAL : UI.FRONTEND.MODS.MANAGE);
        reference4.isInteractable = mod.is_managed;
        if (reference4.isInteractable)
        {
          reference4.GetComponent<ToolTip>().toolTip = (string) mod.manage_tooltip;
          reference4.onClick += mod.on_managed;
        }
        KImage reference5 = hierarchyReferences.GetReference<KImage>("BG");
        MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
        toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
        if (mod.available_content != (Content) 0)
        {
          reference5.defaultState = KImage.ColorSelector.Inactive;
          reference5.ColorState = KImage.ColorSelector.Inactive;
          toggle.onClick += (System.Action) (() => this.OnToggleClicked(toggle, mod.label));
          toggle.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => (string) (mod.IsEnabledForActiveDlc() ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED));
        }
        else
        {
          reference5.defaultState = KImage.ColorSelector.Disabled;
          reference5.ColorState = KImage.ColorSelector.Disabled;
        }
        hierarchyReferences.gameObject.SetActive(true);
      }
    }
    foreach (ModsScreen.DisplayedMod displayedMod in this.displayedMods)
      displayedMod.rect_transform.gameObject.SetActive(true);
    int count = this.displayedMods.Count;
  }

  private void OnToggleClicked(MultiToggle toggle, KMod.Label mod)
  {
    KMod.Manager modManager = Global.Instance.modManager;
    bool enabled = !modManager.IsModEnabled(mod);
    toggle.ChangeState(enabled ? 1 : 0);
    modManager.EnableMod(mod, enabled, (object) this);
    this.UpdateToggleAllButton();
  }

  private bool AreAnyModsDisabled()
  {
    return Global.Instance.modManager.mods.Any<KMod.Mod>((Func<KMod.Mod, bool>) (mod => !mod.IsEmpty() && !mod.IsEnabledForActiveDlc() && this.ShouldDisplayMod(mod)));
  }

  private void UpdateToggleAllButton()
  {
    this.toggleAllButton.GetComponentInChildren<LocText>().text = (string) (this.AreAnyModsDisabled() ? UI.FRONTEND.MODS.ENABLE_ALL : UI.FRONTEND.MODS.DISABLE_ALL);
  }

  private void OnToggleAllClicked()
  {
    bool enabled = this.AreAnyModsDisabled();
    KMod.Manager modManager = Global.Instance.modManager;
    foreach (KMod.Mod mod in modManager.mods)
    {
      if (this.ShouldDisplayMod(mod))
        modManager.EnableMod(mod.label, enabled, (object) this);
    }
    this.BuildDisplay();
    this.UpdateToggleAllButton();
  }

  private struct DisplayedMod
  {
    public RectTransform rect_transform;
    public int mod_index;
  }

  private class ModOrderingDragListener : DragMe.IDragListener
  {
    private List<ModsScreen.DisplayedMod> mods;
    private ModsScreen screen;
    private int startDragIdx = -1;

    public ModOrderingDragListener(ModsScreen screen, List<ModsScreen.DisplayedMod> mods)
    {
      this.screen = screen;
      this.mods = mods;
    }

    public void OnBeginDrag(Vector2 pos) => this.startDragIdx = this.GetDragIdx(pos, false);

    public void OnEndDrag(Vector2 pos)
    {
      if (this.startDragIdx < 0)
        return;
      int dragIdx = this.GetDragIdx(pos, true);
      if (dragIdx == this.startDragIdx)
        return;
      Global.Instance.modManager.Reinsert(this.mods[this.startDragIdx].mod_index, 0 > dragIdx || dragIdx >= this.mods.Count ? -1 : this.mods[dragIdx].mod_index, dragIdx >= this.mods.Count, (object) this);
      this.screen.BuildDisplay();
    }

    private int GetDragIdx(Vector2 pos, bool halfPosition)
    {
      int dragIdx = -1;
      for (int index = 0; index < this.mods.Count; ++index)
      {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.mods[index].rect_transform, pos, (Camera) null, out localPoint);
        if (!halfPosition)
          localPoint += this.mods[index].rect_transform.rect.min;
        if ((double) localPoint.y < 0.0)
          dragIdx = index;
        else
          break;
      }
      return dragIdx;
    }
  }
}
