// Decompiled with JetBrains decompiler
// Type: DevToolMenuFontSize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;

#nullable disable
public class DevToolMenuFontSize
{
  public const string SETTINGS_KEY_FONT_SIZE_CATEGORY = "Imgui_font_size_category";
  private DevToolMenuFontSize.FontSizeCategory fontSizeCategory;

  public bool initialized { private set; get; }

  public void RefreshFontSize()
  {
    this.SetFontSizeCategory((DevToolMenuFontSize.FontSizeCategory) KPlayerPrefs.GetInt("Imgui_font_size_category", 2));
  }

  public void InitializeIfNeeded()
  {
    if (this.initialized)
      return;
    this.initialized = true;
    this.RefreshFontSize();
  }

  public void DrawMenu()
  {
    if (!ImGui.BeginMenu("Settings"))
      return;
    bool v1 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Fabric;
    bool v2 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Small;
    bool v3 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Regular;
    bool v4 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Large;
    if (ImGui.BeginMenu("Size"))
    {
      if (ImGui.Checkbox("Original Font", ref v1) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Fabric)
        this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Fabric);
      if (ImGui.Checkbox("Small Text", ref v2) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Small)
        this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Small);
      if (ImGui.Checkbox("Regular Text", ref v3) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Regular)
        this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Regular);
      if (ImGui.Checkbox("Large Text", ref v4) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Large)
        this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Large);
      ImGui.EndMenu();
    }
    ImGui.EndMenu();
  }

  public unsafe void SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory size)
  {
    this.fontSizeCategory = size;
    KPlayerPrefs.SetInt("Imgui_font_size_category", (int) size);
    ImGuiIOPtr io = ImGui.GetIO();
    int index = (int) size;
    if (index >= io.Fonts.Fonts.Size)
      return;
    ImFontPtr imFontPtr = io.Fonts.Fonts[index];
    io.NativePtr->FontDefault = (ImFont*) imFontPtr;
  }

  public enum FontSizeCategory
  {
    Fabric,
    Small,
    Regular,
    Large,
  }
}
