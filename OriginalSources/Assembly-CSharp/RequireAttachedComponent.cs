// Decompiled with JetBrains decompiler
// Type: RequireAttachedComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class RequireAttachedComponent : ProcessCondition
{
  private string typeNameString;
  private System.Type requiredType;
  private AttachableBuilding myAttachable;

  public System.Type RequiredType
  {
    get => this.requiredType;
    set
    {
      this.requiredType = value;
      this.typeNameString = this.requiredType.Name;
    }
  }

  public RequireAttachedComponent(
    AttachableBuilding myAttachable,
    System.Type required_type,
    string type_name_string)
  {
    this.myAttachable = myAttachable;
    this.requiredType = required_type;
    this.typeNameString = type_name_string;
  }

  public override ProcessCondition.Status EvaluateCondition()
  {
    if ((UnityEngine.Object) this.myAttachable != (UnityEngine.Object) null)
    {
      foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.myAttachable))
      {
        if ((bool) (UnityEngine.Object) gameObject.GetComponent(this.requiredType))
          return ProcessCondition.Status.Ready;
      }
    }
    return ProcessCondition.Status.Failure;
  }

  public override string GetStatusMessage(ProcessCondition.Status status) => this.typeNameString;

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    return status == ProcessCondition.Status.Ready ? string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.INSTALLED_TOOLTIP, (object) this.typeNameString.ToLower()) : string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.MISSING_TOOLTIP, (object) this.typeNameString.ToLower());
  }

  public override bool ShowInUI() => true;
}
