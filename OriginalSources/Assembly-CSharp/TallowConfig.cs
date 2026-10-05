// Decompiled with JetBrains decompiler
// Type: TallowConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TallowConfig : IOreConfig
{
  public const string ID = "Tallow";
  public static readonly Tag TAG = TagManager.Create("Tallow");

  public SimHashes ElementID => SimHashes.Tallow;

  public GameObject CreatePrefab() => EntityTemplates.CreateSolidOreEntity(this.ElementID);
}
