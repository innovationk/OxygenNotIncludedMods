// Decompiled with JetBrains decompiler
// Type: MessageTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class MessageTarget : ISaveLoadable
{
  [Serialize]
  private Ref<KPrefabID> prefabId = new Ref<KPrefabID>();
  [Serialize]
  private Vector3 position;
  [Serialize]
  private string name;

  public MessageTarget(KPrefabID prefab_id)
  {
    this.prefabId.Set(prefab_id);
    this.position = prefab_id.transform.GetPosition();
    this.name = "Unknown";
    KSelectable component = prefab_id.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      this.name = component.GetName();
    prefab_id.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
  }

  public Vector3 GetPosition()
  {
    return (UnityEngine.Object) this.prefabId.Get() != (UnityEngine.Object) null ? this.prefabId.Get().transform.GetPosition() : this.position;
  }

  public KSelectable GetSelectable()
  {
    return (UnityEngine.Object) this.prefabId.Get() != (UnityEngine.Object) null ? this.prefabId.Get().transform.GetComponent<KSelectable>() : (KSelectable) null;
  }

  public string GetName() => this.name;

  private void OnAbsorbedBy(object data)
  {
    if ((UnityEngine.Object) this.prefabId.Get() != (UnityEngine.Object) null)
      this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
    KPrefabID component = ((GameObject) data).GetComponent<KPrefabID>();
    component.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
    this.prefabId.Set(component);
  }

  public void OnCleanUp()
  {
    if (!((UnityEngine.Object) this.prefabId.Get() != (UnityEngine.Object) null))
      return;
    this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
    this.prefabId.Set((KPrefabID) null);
  }
}
