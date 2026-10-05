// Decompiled with JetBrains decompiler
// Type: CharacterOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/CharacterOverlay")]
public class CharacterOverlay : KMonoBehaviour
{
  public bool shouldShowName;
  private bool registered;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Register();
  }

  public void Register()
  {
    if (this.registered)
      return;
    this.registered = true;
    NameDisplayScreen.Instance.AddNewEntry(this.gameObject);
  }
}
