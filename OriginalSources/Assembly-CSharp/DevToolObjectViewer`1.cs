// Decompiled with JetBrains decompiler
// Type: DevToolObjectViewer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class DevToolObjectViewer<T> : DevTool
{
  private Func<T> getValue;

  public DevToolObjectViewer(Func<T> getValue)
  {
    this.getValue = getValue;
    this.Name = typeof (T).Name;
  }

  protected override void RenderTo(DevPanel panel)
  {
    T obj = this.getValue();
    this.Name = obj.GetType().Name;
    ImGuiEx.DrawObject((object) obj);
  }
}
