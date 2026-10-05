// Decompiled with JetBrains decompiler
// Type: FilterSideScreenRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/FilterSideScreenRow")]
public class FilterSideScreenRow : SingleItemSelectionRow
{
  public override string InvalidTagTitle => (string) UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;

  protected override void SetIcon(Sprite sprite, Color color)
  {
    if (!((Object) this.icon != (Object) null))
      return;
    this.icon.gameObject.SetActive(false);
  }
}
