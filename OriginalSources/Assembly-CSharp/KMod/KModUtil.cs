// Decompiled with JetBrains decompiler
// Type: KMod.KModUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System.IO;

#nullable disable
namespace KMod;

public class KModUtil
{
  public static KModHeader GetHeader(
    IFileSource file_source,
    string defaultStaticID,
    string defaultTitle,
    string defaultDescription,
    bool devMod)
  {
    string str = "mod.yaml";
    string readText = file_source.Read(str);
    YamlIO.ErrorHandler handle_error = (YamlIO.ErrorHandler) ((e, force_warning) => YamlIO.LogError(e, !devMod));
    KModHeader kmodHeader;
    if (string.IsNullOrEmpty(readText))
      kmodHeader = (KModHeader) null;
    else
      kmodHeader = YamlIO.Parse<KModHeader>(readText, new FileHandle()
      {
        full_path = Path.Combine(file_source.GetRoot(), str)
      }, handle_error);
    KModHeader header = kmodHeader;
    if (header == null)
      header = new KModHeader()
      {
        title = defaultTitle,
        description = defaultDescription,
        staticID = defaultStaticID
      };
    if (string.IsNullOrEmpty(header.staticID))
      header.staticID = defaultStaticID;
    if (header.title == null)
      header.title = defaultTitle;
    if (header.description == null)
      header.description = (string) UI.FRONTEND.MODS.NO_DESCRIPTION;
    return header;
  }
}
