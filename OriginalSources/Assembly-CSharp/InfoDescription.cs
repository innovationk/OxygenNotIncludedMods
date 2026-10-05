// Decompiled with JetBrains decompiler
// Type: InfoDescription
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InfoDescription")]
public class InfoDescription : KMonoBehaviour
{
  public string nameLocString = "";
  private string descriptionLocString = "";
  public string description;
  public string effect = "";
  public string displayName;

  public string DescriptionLocString
  {
    set
    {
      this.descriptionLocString = value;
      if (this.descriptionLocString == null)
        return;
      this.description = (string) Strings.Get(this.descriptionLocString);
    }
    get => this.descriptionLocString;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (!string.IsNullOrEmpty(this.nameLocString))
      this.displayName = (string) Strings.Get(this.nameLocString);
    if (string.IsNullOrEmpty(this.descriptionLocString))
      return;
    this.description = (string) Strings.Get(this.descriptionLocString);
  }
}
