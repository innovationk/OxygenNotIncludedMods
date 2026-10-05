// Decompiled with JetBrains decompiler
// Type: SuitDiseaseHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SuitDiseaseHandler")]
public class SuitDiseaseHandler : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>((Action<SuitDiseaseHandler, object>) ((component, data) => component.OnEquipped(data)));
  private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>((Action<SuitDiseaseHandler, object>) ((component, data) => component.OnUnequipped(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<SuitDiseaseHandler>(-1617557748, SuitDiseaseHandler.OnEquippedDelegate);
    this.Subscribe<SuitDiseaseHandler>(-170173755, SuitDiseaseHandler.OnUnequippedDelegate);
  }

  private PrimaryElement GetPrimaryElement(object data)
  {
    GameObject targetGameObject = ((Component) data).GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    return (bool) (UnityEngine.Object) targetGameObject ? targetGameObject.GetComponent<PrimaryElement>() : (PrimaryElement) null;
  }

  private void OnEquipped(object data)
  {
    PrimaryElement primaryElement = this.GetPrimaryElement(data);
    if (!((UnityEngine.Object) primaryElement != (UnityEngine.Object) null))
      return;
    primaryElement.ForcePermanentDiseaseContainer(true);
    primaryElement.RedirectDisease(this.gameObject);
  }

  private void OnUnequipped(object data)
  {
    PrimaryElement primaryElement = this.GetPrimaryElement(data);
    if (!((UnityEngine.Object) primaryElement != (UnityEngine.Object) null))
      return;
    primaryElement.ForcePermanentDiseaseContainer(false);
    primaryElement.RedirectDisease((GameObject) null);
  }

  private void OnModifyDiseaseCount(int delta, string reason)
  {
    this.GetComponent<PrimaryElement>().ModifyDiseaseCount(delta, reason);
  }

  private void OnAddDisease(byte disease_idx, int delta, string reason)
  {
    this.GetComponent<PrimaryElement>().AddDisease(disease_idx, delta, reason);
  }
}
