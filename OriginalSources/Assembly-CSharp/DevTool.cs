// Decompiled with JetBrains decompiler
// Type: DevTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;

#nullable disable
public abstract class DevTool
{
  public string Name;
  public bool RequiresGameRunning;
  public bool isRequestingToClosePanel;
  public ImGuiWindowFlags drawFlags;
  private bool didInit;

  public event System.Action OnInit;

  public event System.Action OnUpdate;

  public event System.Action OnUninit;

  public DevTool() => this.Name = DevToolUtil.GenerateDevToolName(this);

  public void DoImGui(DevPanel panel)
  {
    if (this.RequiresGameRunning && (UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      ImGui.Text("Game must be loaded to use this devtool.");
    else
      this.RenderTo(panel);
  }

  public void ClosePanel() => this.isRequestingToClosePanel = true;

  protected abstract void RenderTo(DevPanel panel);

  public void Internal_TryInit()
  {
    if (this.didInit)
      return;
    this.didInit = true;
    if (this.OnInit == null)
      return;
    this.OnInit();
  }

  public void Internal_Update()
  {
    if (this.OnUpdate == null)
      return;
    this.OnUpdate();
  }

  public void Internal_Uninit()
  {
    if (this.OnUninit == null)
      return;
    this.OnUninit();
  }
}
