// Decompiled with JetBrains decompiler
// Type: DevToolLoreHookup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DevToolLoreHookup : DevTool
{
  private string cachedUnlockId;
  private string unlockIdStatus;
  private Vector4 unlockIdStatusColor;
  private string cachedDisplayText;
  private string displayTextPreview;
  private bool displayTextValid;
  private string cachedNextCollectionId;
  private string nextCollectionIdStatus;
  private Vector4 nextCollectionIdStatusColor;

  public DevToolLoreHookup() => this.RequiresGameRunning = true;

  private void ValidateUnlockId(string unlockId)
  {
    this.cachedUnlockId = unlockId;
    if (unlockId.IsNullOrWhiteSpace())
    {
      this.unlockIdStatus = (string) null;
    }
    else
    {
      foreach (KeyValuePair<string, string[]> lockCollection in Game.Instance.unlocks.lockCollections)
      {
        foreach (string str in lockCollection.Value)
        {
          if (str == unlockId)
          {
            this.unlockIdStatus = $"[{lockCollection.Key}]";
            this.unlockIdStatusColor = new Vector4(0.5f, 1f, 0.5f, 1f);
            return;
          }
        }
      }
      if (CodexCache.GetEntryForLock(unlockId) != null)
      {
        this.unlockIdStatus = "[codex]";
        this.unlockIdStatusColor = new Vector4(0.5f, 0.8f, 1f, 1f);
      }
      else
      {
        this.unlockIdStatus = "[unknown ID]";
        this.unlockIdStatusColor = new Vector4(1f, 0.4f, 0.4f, 1f);
      }
    }
  }

  private void ValidateNextCollectionId(string collectionId)
  {
    this.cachedNextCollectionId = collectionId;
    if (collectionId.IsNullOrWhiteSpace())
    {
      this.nextCollectionIdStatus = (string) null;
    }
    else
    {
      string[] strArray;
      if (Game.Instance.unlocks.lockCollections.TryGetValue(collectionId, out strArray))
      {
        this.nextCollectionIdStatus = $"[{strArray.Length} entries]";
        this.nextCollectionIdStatusColor = new Vector4(0.5f, 1f, 0.5f, 1f);
      }
      else
      {
        this.nextCollectionIdStatus = "[unknown collection]";
        this.nextCollectionIdStatusColor = new Vector4(1f, 0.4f, 0.4f, 1f);
      }
    }
  }

  private void ValidateDisplayText(string displayText)
  {
    this.cachedDisplayText = displayText;
    if (displayText.IsNullOrWhiteSpace())
    {
      this.displayTextPreview = (string) null;
    }
    else
    {
      StringEntry result;
      if (Strings.TryGet(displayText, out result))
      {
        this.displayTextValid = true;
        this.displayTextPreview = result.String;
      }
      else
      {
        this.displayTextValid = false;
        this.displayTextPreview = "[string key not found]";
      }
    }
  }

  protected override void RenderTo(DevPanel panel)
  {
    if ((Object) SelectTool.Instance == (Object) null || (Object) SelectTool.Instance.selected == (Object) null)
    {
      ImGui.Text("Select an entity in-game.");
    }
    else
    {
      GameObject gameObject = SelectTool.Instance.selected.gameObject;
      LoreBearer component = gameObject.GetComponent<LoreBearer>();
      ImGui.Text("Selected: " + gameObject.name);
      ImGui.Separator();
      if ((Object) component == (Object) null)
      {
        ImGui.Text("No LoreBearer component on this entity.");
      }
      else
      {
        ImGui.Text("LoreBearer Fields");
        ImGui.Separator();
        string input1 = component.poiOverrideLoreUnlockId ?? "";
        bool flag = false;
        if (ImGui.InputText("Override Unlock ID", ref input1, 256U /*0x0100*/))
        {
          component.poiOverrideLoreUnlockId = string.IsNullOrWhiteSpace(input1) ? (string) null : input1;
          flag = true;
        }
        if (input1 != this.cachedUnlockId)
          this.ValidateUnlockId(input1);
        if (this.unlockIdStatus != null)
        {
          ImGui.SameLine();
          ImGui.TextColored(this.unlockIdStatusColor, this.unlockIdStatus);
        }
        string input2 = component.poiOverrideLoreDisplayText ?? "";
        if (ImGui.InputText("Override Display Text (string key)", ref input2, 1024U /*0x0400*/))
        {
          component.poiOverrideLoreDisplayText = string.IsNullOrWhiteSpace(input2) ? (string) null : input2;
          flag = true;
        }
        if (input2 != this.cachedDisplayText)
          this.ValidateDisplayText(input2);
        if (this.displayTextPreview != null)
        {
          if (this.displayTextValid)
          {
            ImGui.TextWrapped(this.displayTextPreview);
          }
          else
          {
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(1f, 0.4f, 0.4f, 1f), this.displayTextPreview);
          }
        }
        string input3 = component.poiOverrideNextCollectionId ?? "";
        if (ImGui.InputText("Override Next Collection ID (Optional)", ref input3, 256U /*0x0100*/))
        {
          component.poiOverrideNextCollectionId = string.IsNullOrWhiteSpace(input3) ? (string) null : input3;
          flag = true;
        }
        if (input3 != this.cachedNextCollectionId)
          this.ValidateNextCollectionId(input3);
        if (this.nextCollectionIdStatus != null)
        {
          ImGui.SameLine();
          ImGui.TextColored(this.nextCollectionIdStatusColor, this.nextCollectionIdStatus);
        }
        if (flag)
        {
          if (!string.IsNullOrEmpty(component.poiOverrideLoreUnlockId))
          {
            if (!string.IsNullOrEmpty(component.poiOverrideNextCollectionId))
              component.Internal_SetContent(LoreBearerUtil.UnlockSpecificEntryThenNext(component.poiOverrideLoreUnlockId, component.poiOverrideLoreDisplayText, LoreBearerUtil.GetUnlockActionForCollection(component.poiOverrideNextCollectionId)));
            else
              component.Internal_SetContent(LoreBearerUtil.UnlockSpecificEntry(component.poiOverrideLoreUnlockId, (string) Strings.Get(component.poiOverrideLoreDisplayText)));
          }
          else
            component.Internal_SetContent((LoreBearerAction) null);
        }
        ImGui.Separator();
        if (ImGui.Button("Make Uninspected"))
          component.Debug_ResetSearched();
        ImGui.SameLine();
        if (!ImGui.Button("Clear Override"))
          return;
        component.poiOverrideLoreUnlockId = (string) null;
        component.poiOverrideLoreDisplayText = (string) null;
        component.poiOverrideNextCollectionId = (string) null;
        component.Internal_SetContent((LoreBearerAction) null);
      }
    }
  }
}
