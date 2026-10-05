// Decompiled with JetBrains decompiler
// Type: WorldgenOnlyLoreBearer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

#nullable disable
public class WorldgenOnlyLoreBearer : KMonoBehaviour
{
  [MyCmpReq]
  private LoreBearer loreBearer;
  [Serialize]
  public bool hasLore;

  protected override void OnPrefabInit()
  {
    this.Subscribe(1119167081, new Action<object>(this.OnNewGameSpawn));
  }

  private void UpdateLore() => this.loreBearer.hideLore = !this.hasLore;

  protected override void OnSpawn() => this.UpdateLore();

  private void OnNewGameSpawn(object obj)
  {
    this.hasLore = true;
    this.UpdateLore();
  }
}
