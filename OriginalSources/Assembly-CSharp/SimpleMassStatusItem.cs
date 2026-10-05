// Decompiled with JetBrains decompiler
// Type: SimpleMassStatusItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SimpleMassStatusItem")]
public class SimpleMassStatusItem : KMonoBehaviour
{
  public string symbolPrefix = "";

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OreMass, (object) this.gameObject);
  }
}
