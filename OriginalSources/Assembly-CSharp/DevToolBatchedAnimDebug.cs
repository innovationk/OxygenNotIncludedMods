// Decompiled with JetBrains decompiler
// Type: DevToolBatchedAnimDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class DevToolBatchedAnimDebug : DevTool
{
  private GameObject Selection;
  private bool LockSelection;
  private string Filter = "";
  private int FrameIndex;

  public DevToolBatchedAnimDebug() => this.drawFlags = ImGuiWindowFlags.MenuBar;

  protected override void RenderTo(DevPanel panel)
  {
    if (ImGui.BeginMenuBar())
    {
      ImGui.Checkbox("Lock selection", ref this.LockSelection);
      ImGui.EndMenuBar();
    }
    if (!this.LockSelection)
      this.Selection = SelectTool.Instance?.selected?.gameObject;
    if ((UnityEngine.Object) this.Selection == (UnityEngine.Object) null)
    {
      ImGui.Text("No selection.");
    }
    else
    {
      KBatchedAnimController component1 = this.Selection.GetComponent<KBatchedAnimController>();
      if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      {
        ImGui.Text("No anim controller.");
      }
      else
      {
        KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(component1.batchGroupID);
        SymbolOverrideController component2 = this.Selection.GetComponent<SymbolOverrideController>();
        ImGui.Text($"Group: {component1.GetBatch().group.batchID.ToString()}, Build: {component1.curBuild.name}");
        if (!ImGui.BeginTabBar("##tabs", ImGuiTabBarFlags.None))
          return;
        if (ImGui.BeginTabItem("BatchGroup"))
        {
          KAnimBatchGroup group = component1.GetBatch().group;
          ImGui.BeginChild("ScrollRegion", new Vector2(0.0f, 0.0f), true, ImGuiWindowFlags.None);
          ImGui.Text($"Group mesh.vertices.Count: {((IEnumerable<Vector3>) group.mesh.vertices).Count<Vector3>()}");
          ImGui.Text($"Group data.maxVisibleSymbols: {group.data.maxVisibleSymbols}");
          ImGui.Text($"Group maxGroupSize: {group.maxGroupSize}");
          ImGui.EndChild();
          ImGui.EndTabItem();
        }
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && ImGui.BeginTabItem("SymbolOverrides"))
        {
          ImGui.InputText("Symbol Filter", ref this.Filter, 128U /*0x80*/);
          int num = Hash.SDBMLower(this.Filter);
          ImGui.LabelText("Filter Hash", "0x" + num.ToString("X"));
          SymbolOverrideController.SymbolEntry[] getSymbolOverrides = component2.GetSymbolOverrides;
          ImGui.BeginChild("ScrollRegion", new Vector2(0.0f, 0.0f), true, ImGuiWindowFlags.None);
          for (int index = 0; index < getSymbolOverrides.Length; ++index)
          {
            SymbolOverrideController.SymbolEntry symbolEntry = getSymbolOverrides[index];
            KAnim.Build.Symbol symbol = batchGroupData.GetSymbol((KAnimHashedString) symbolEntry.targetSymbol);
            if (symbolEntry.targetSymbol.HashValue == num || symbolEntry.sourceSymbol.hash.HashValue == num || this.StringContains(symbolEntry.sourceSymbol.hash.ToString(), this.Filter) || this.StringContains(symbol.hash.ToString(), this.Filter))
            {
              ImGui.Text($"[{index}] source: {symbolEntry.sourceSymbol.hash}, {symbolEntry.sourceSymbol.build.name}, ({symbolEntry.sourceSymbol.build.GetTexture(0).name}), priority: {symbolEntry.priority}");
              ImGui.Text($"       firstFrameIdx = {symbolEntry.sourceSymbol.firstFrameIdx}, numFrames = {symbolEntry.sourceSymbol.numFrames}");
              if (symbol != null)
              {
                ImGui.Text($"   target: {symbol.hash}");
                ImGui.Text($"       firstFrameIdx = {symbol.firstFrameIdx}, numFrames = {symbol.numFrames}");
              }
              else
                ImGui.Text($"   target: does not contain the symbol '{symbolEntry.sourceSymbol.hash}' to override");
            }
          }
          ImGui.EndChild();
          ImGui.EndTabItem();
        }
        if (ImGui.BeginTabItem("Build Symbols"))
        {
          ImGui.InputText("Symbol Filter", ref this.Filter, 128U /*0x80*/);
          int num = Hash.SDBMLower(this.Filter);
          ImGui.LabelText("Filter Hash", "0x" + num.ToString("X"));
          ImGui.BeginChild("ScrollRegion", new Vector2(0.0f, 0.0f), true, ImGuiWindowFlags.None);
          KBatchGroupData data = component1.GetBatch().group.data;
          for (int index = 0; index < data.GetSymbolCount(); ++index)
          {
            KAnim.Build.Symbol symbol = data.GetSymbol(index);
            if (symbol.hash.HashValue == num || this.StringContains(symbol.hash.ToString(), this.Filter))
              ImGui.Text($"[{symbol.symbolIndexInSourceBuild}]: {symbol.hash} ({symbol.hash.HashValue})");
          }
          ImGui.EndChild();
          ImGui.EndTabItem();
        }
        if (ImGui.BeginTabItem("Anim Frame Data"))
        {
          ImGui.Text("Current anim: " + component1.CurrentAnim.name);
          ImGui.Text("Current frame index: " + component1.GetCurrentFrameIndex().ToString());
          ImGuiEx.InputIntRange("Frame Index", ref this.FrameIndex, 0, batchGroupData.GetAnimFrames().Count - 1);
          KAnim.Anim.Frame frame;
          batchGroupData.TryGetFrame(this.FrameIndex, out frame);
          ImGui.Text($"Frame [{this.FrameIndex}]: firstElementIdx= {frame.firstElementIdx} numElements= {frame.numElements}");
          ImGui.Text("Frame Elements: ");
          for (int index = 0; index < frame.numElements; ++index)
          {
            KAnim.Anim.FrameElement frameElement = batchGroupData.GetFrameElement(frame.firstElementIdx + index);
            int symbolIndex = batchGroupData.GetSymbolIndex(frameElement.symbol);
            bool symbolVisiblity = component1.GetSymbolVisiblity(frameElement.symbol);
            bool flag = component1.symbolInstanceGpuData.IsVisible(symbolIndex);
            ImGui.Text($"FrameElement [{frame.firstElementIdx + index}]: symbolIdx= {symbolIndex} symbol= {frameElement.symbol} visible= {symbolVisiblity} gpu: {flag}");
          }
          ImGui.EndTabItem();
        }
        if (ImGui.BeginTabItem("Texture atlases"))
        {
          ImGui.BeginChild("ScrollRegion", new Vector2(0.0f, 0.0f), true, ImGuiWindowFlags.None);
          List<Texture2D> source = new List<Texture2D>((IEnumerable<Texture2D>) component1.GetBatch().atlases.GetTextures());
          int num = source.Count<Texture2D>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            source.AddRange((IEnumerable<Texture2D>) component2.GetAtlasList().GetTextures());
          for (int index = 0; index < source.Count; ++index)
          {
            Texture2D tex = source[index];
            string str = index >= num ? "symbol override" : "base";
            ImGui.Text($"[{index}]: {tex.name}, [{tex.width},{tex.height}] ({str})");
            if (ImGui.IsItemHovered())
            {
              ImGui.BeginTooltip();
              ImGuiEx.Image(tex, new Vector2((float) tex.width, (float) tex.height));
              ImGui.EndTooltip();
            }
          }
          ImGui.EndChild();
          ImGui.EndTabItem();
        }
        ImGui.EndTabBar();
      }
    }
  }

  private bool StringContains(string target, string query)
  {
    return this.Filter == "" || target.IndexOf(query, 0, StringComparison.CurrentCultureIgnoreCase) != -1;
  }
}
