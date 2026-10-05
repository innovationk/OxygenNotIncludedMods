// Decompiled with JetBrains decompiler
// Type: TemplateCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using ProcGen;
using System.Collections.Generic;

#nullable disable
public static class TemplateCache
{
  private const string defaultAssetFolder = "bases";
  private static Dictionary<string, TemplateContainer> templates;

  public static bool Initted { get; private set; }

  public static void Init()
  {
    if (TemplateCache.Initted)
      return;
    TemplateCache.templates = new Dictionary<string, TemplateContainer>();
    TemplateCache.Initted = true;
  }

  public static void Clear()
  {
    TemplateCache.templates = (Dictionary<string, TemplateContainer>) null;
    TemplateCache.Initted = false;
  }

  public static string RewriteTemplatePath(string scopePath)
  {
    string dlcId;
    string path;
    SettingsCache.GetDlcIdAndPath(scopePath, out dlcId, out path);
    return SettingsCache.GetAbsoluteContentPath(dlcId, "templates/" + path);
  }

  public static string RewriteTemplateYaml(string scopePath)
  {
    return TemplateCache.RewriteTemplatePath(scopePath) + ".yaml";
  }

  public static TemplateContainer GetTemplate(string templatePath)
  {
    if (!TemplateCache.templates.ContainsKey(templatePath))
      TemplateCache.templates.Add(templatePath, (TemplateContainer) null);
    if (TemplateCache.templates[templatePath] == null)
    {
      string filename = TemplateCache.RewriteTemplateYaml(templatePath);
      TemplateContainer templateContainer = YamlIO.LoadFile<TemplateContainer>(filename);
      if (templateContainer == null)
        Debug.LogWarning((object) $"Missing template [{filename}]");
      templateContainer.name = templatePath;
      TemplateCache.templates[templatePath] = templateContainer;
    }
    return TemplateCache.templates[templatePath];
  }

  public static bool TemplateExists(string templatePath)
  {
    return FileSystem.FileExists(TemplateCache.RewriteTemplateYaml(templatePath));
  }
}
