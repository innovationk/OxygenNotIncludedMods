// Decompiled with JetBrains decompiler
// Type: Database.PermitPresentationInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
namespace Database;

public struct PermitPresentationInfo
{
  public Sprite sprite;

  public string facadeFor { get; private set; }

  public static Sprite GetUnknownSprite() => Assets.GetSprite((HashedString) "unknown");

  public void SetFacadeForPrefabName(string prefabName)
  {
    this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", prefabName);
  }

  public void SetFacadeForPrefabID(string prefabId)
  {
    if ((Object) Assets.TryGetPrefab((Tag) prefabId) == (Object) null)
      this.facadeFor = (string) UI.KLEI_INVENTORY_SCREEN.ITEM_DLC_REQUIRED;
    else
      this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab((Tag) prefabId).GetProperName());
  }

  public void SetFacadeForText(string text) => this.facadeFor = text;
}
