// Decompiled with JetBrains decompiler
// Type: DevToolSceneBrowser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class DevToolSceneBrowser : DevTool
{
  private List<DevToolSceneBrowser.StackItem> Stack = new List<DevToolSceneBrowser.StackItem>();
  private int StackIndex;
  private static int SelectedIndex = -1;
  private static string SearchFilter = "";
  private static List<GameObject> SearchResults = new List<GameObject>();
  private static int SearchSelectedIndex = -1;

  public DevToolSceneBrowser()
  {
    this.drawFlags = ImGuiWindowFlags.MenuBar;
    this.Stack.Add(new DevToolSceneBrowser.StackItem()
    {
      SceneRoot = true,
      Filter = ""
    });
  }

  private void PushGameObject(GameObject go)
  {
    if (this.StackIndex < this.Stack.Count && (UnityEngine.Object) go == (UnityEngine.Object) this.Stack[this.StackIndex].Root)
      return;
    if (this.Stack.Count > this.StackIndex + 1)
      this.Stack.RemoveRange(this.StackIndex + 1, this.Stack.Count - (this.StackIndex + 1));
    this.Stack.Add(new DevToolSceneBrowser.StackItem()
    {
      SceneRoot = (UnityEngine.Object) go == (UnityEngine.Object) null,
      Root = go,
      Filter = ""
    });
    ++this.StackIndex;
  }

  protected override void RenderTo(DevPanel panel)
  {
    for (int index = this.Stack.Count - 1; index > 0; --index)
    {
      DevToolSceneBrowser.StackItem stackItem = this.Stack[index];
      if (!stackItem.SceneRoot && stackItem.Root.IsNullOrDestroyed())
      {
        this.Stack.RemoveAt(index);
        this.StackIndex = Math.Min(index - 1, this.StackIndex);
      }
    }
    bool flag1 = false;
    if (ImGui.BeginMenuBar())
    {
      if (ImGui.BeginMenu("Utils"))
      {
        if (ImGui.MenuItem("Goto current selection") && (UnityEngine.Object) SelectTool.Instance?.selected?.gameObject != (UnityEngine.Object) null)
          this.PushGameObject(SelectTool.Instance?.selected?.gameObject);
        if (ImGui.MenuItem("Search All"))
          flag1 = true;
        ImGui.EndMenu();
      }
      ImGui.EndMenuBar();
    }
    if (ImGui.Button(" < ") && this.StackIndex > 0)
      --this.StackIndex;
    ImGui.SameLine();
    if (ImGui.Button(" ^ ") && this.StackIndex > 0 && !this.Stack[this.StackIndex].SceneRoot)
      this.PushGameObject(this.Stack[this.StackIndex].Root.transform.parent?.gameObject);
    ImGui.SameLine();
    if (ImGui.Button(" > ") && this.StackIndex + 1 < this.Stack.Count)
      ++this.StackIndex;
    DevToolSceneBrowser.StackItem stackItem1 = this.Stack[this.StackIndex];
    if (!stackItem1.SceneRoot)
    {
      ImGui.SameLine();
      if (ImGui.Button("Inspect"))
        DevToolSceneInspector.Inspect((object) stackItem1.Root);
    }
    List<GameObject> rootGameObjects;
    if (stackItem1.SceneRoot)
    {
      ImGui.Text("Scene root");
      Scene activeScene = SceneManager.GetActiveScene();
      rootGameObjects = new List<GameObject>(activeScene.rootCount);
      activeScene.GetRootGameObjects(rootGameObjects);
    }
    else
    {
      ImGui.LabelText("Selected object", stackItem1.Root.name);
      rootGameObjects = new List<GameObject>();
      foreach (Transform transform in stackItem1.Root.transform)
      {
        if ((UnityEngine.Object) transform.gameObject != (UnityEngine.Object) stackItem1.Root)
          rootGameObjects.Add(transform.gameObject);
      }
    }
    if (ImGui.Button("Clear"))
      stackItem1.Filter = "";
    ImGui.SameLine();
    ImGui.InputText("Filter", ref stackItem1.Filter, 64U /*0x40*/);
    ImGui.BeginChild("ScrollRegion", new Vector2(0.0f, 0.0f), true, ImGuiWindowFlags.None);
    for (int index = 0; index < rootGameObjects.Count; ++index)
    {
      GameObject go = rootGameObjects[index];
      if (!(stackItem1.Filter != "") || go.name.IndexOf(stackItem1.Filter, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
      {
        if (ImGui.Selectable(go.name, false, ImGuiSelectableFlags.AllowDoubleClick) && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
          this.PushGameObject(go);
        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
        {
          DevToolSceneBrowser.SelectedIndex = index;
          ImGui.OpenPopup("RightClickMenu");
        }
      }
    }
    if (ImGui.BeginPopup("RightClickMenu"))
    {
      if (ImGui.MenuItem("Inspect"))
      {
        DevToolSceneInspector.Inspect((object) rootGameObjects[DevToolSceneBrowser.SelectedIndex]);
        DevToolSceneBrowser.SelectedIndex = -1;
      }
      ImGui.EndPopup();
    }
    ImGui.EndChild();
    if (flag1)
      ImGui.OpenPopup("SearchAll");
    if (!ImGui.BeginPopupModal("SearchAll"))
      return;
    ImGui.Text("Search all objects in the scene:");
    ImGui.Separator();
    if (ImGui.Button("Clear"))
      DevToolSceneBrowser.SearchFilter = "";
    ImGui.SameLine();
    if (ImGui.InputText("Filter", ref DevToolSceneBrowser.SearchFilter, 64U /*0x40*/))
      DevToolSceneBrowser.SearchResults = ((IEnumerable<GameObject>) UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID)).Where<GameObject>((Func<GameObject, bool>) (go => go.name.IndexOf(DevToolSceneBrowser.SearchFilter, 0, StringComparison.CurrentCultureIgnoreCase) != -1)).OrderBy<GameObject, string>((Func<GameObject, string>) (go => go.name)).ToList<GameObject>();
    ImGui.BeginChild("ScrollRegion", new Vector2(0.0f, 200f), true, ImGuiWindowFlags.None);
    int num = 0;
    foreach (UnityEngine.Object searchResult in DevToolSceneBrowser.SearchResults)
    {
      if (ImGui.Selectable(searchResult.name, DevToolSceneBrowser.SearchSelectedIndex == num))
        DevToolSceneBrowser.SearchSelectedIndex = num;
      ++num;
    }
    ImGui.EndChild();
    bool flag2 = false;
    if (ImGui.Button("Browse") && DevToolSceneBrowser.SearchSelectedIndex >= 0)
    {
      this.PushGameObject(DevToolSceneBrowser.SearchResults[DevToolSceneBrowser.SearchSelectedIndex]);
      flag2 = true;
    }
    ImGui.SameLine();
    if (ImGui.Button("Inspect") && DevToolSceneBrowser.SearchSelectedIndex >= 0)
    {
      DevToolSceneInspector.Inspect((object) DevToolSceneBrowser.SearchResults[DevToolSceneBrowser.SearchSelectedIndex]);
      flag2 = true;
    }
    ImGui.SameLine();
    if (ImGui.Button("Cancel"))
      flag2 = true;
    if (flag2)
    {
      DevToolSceneBrowser.SearchFilter = "";
      DevToolSceneBrowser.SearchResults.Clear();
      DevToolSceneBrowser.SearchSelectedIndex = -1;
      ImGui.CloseCurrentPopup();
    }
    ImGui.EndPopup();
  }

  private class StackItem
  {
    public bool SceneRoot;
    public GameObject Root;
    public string Filter;
  }
}
