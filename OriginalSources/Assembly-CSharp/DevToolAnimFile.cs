// Decompiled with JetBrains decompiler
// Type: DevToolAnimFile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class DevToolAnimFile : DevTool
{
  private KAnimFile animFile;

  public DevToolAnimFile(KAnimFile animFile)
  {
    this.animFile = animFile;
    this.Name = $"Anim File: \"{animFile.name}\"";
  }

  protected override void RenderTo(DevPanel panel)
  {
    ImGuiEx.DrawObject((object) this.animFile);
    ImGuiEx.DrawObject((object) this.animFile.GetData());
  }
}
