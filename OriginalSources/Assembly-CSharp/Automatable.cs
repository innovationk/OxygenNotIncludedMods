// Decompiled with JetBrains decompiler
// Type: Automatable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Automatable")]
public class Automatable : KMonoBehaviour
{
  [Serialize]
  private bool automationOnly = true;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<Automatable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Automatable>((Action<Automatable, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Automatable>(-905833192, Automatable.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    Automatable component = ((GameObject) data).GetComponent<Automatable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.automationOnly = component.automationOnly;
  }

  public bool GetAutomationOnly() => this.automationOnly;

  public void SetAutomationOnly(bool only) => this.automationOnly = only;

  public bool AllowedByAutomation(bool is_transfer_arm)
  {
    return !this.GetAutomationOnly() | is_transfer_arm;
  }
}
