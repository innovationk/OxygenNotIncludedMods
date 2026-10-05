// Decompiled with JetBrains decompiler
// Type: IApproachableBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface IApproachableBehaviour
{
  bool IsValidTarget();

  GameObject GetTarget();

  StatusItem GetApproachStatusItem();

  StatusItem GetBehaviourStatusItem();

  CellOffset[] GetApproachOffsets() => OffsetGroups.Use;

  void OnArrive()
  {
  }

  void OnSuccess()
  {
  }

  void OnFailure()
  {
  }
}
