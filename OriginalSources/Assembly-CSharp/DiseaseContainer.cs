// Decompiled with JetBrains decompiler
// Type: DiseaseContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public struct DiseaseContainer
{
  public AutoDisinfectable autoDisinfectable;
  public ushort elemIdx;
  public bool isContainer;
  public ConduitType conduitType;
  public KBatchedAnimController controller;
  public GameObject visualDiseaseProvider;
  public int overpopulationCount;
  public float instanceGrowthRate;
  public float accumulatedError;

  public DiseaseContainer(GameObject go, ushort elemIdx)
  {
    this.elemIdx = elemIdx;
    this.isContainer = go.GetComponent<IUserControlledCapacity>() != null && (Object) go.GetComponent<Storage>() != (Object) null;
    Conduit component = go.GetComponent<Conduit>();
    this.conduitType = !((Object) component != (Object) null) ? ConduitType.None : component.type;
    this.controller = go.GetComponent<KBatchedAnimController>();
    this.overpopulationCount = 1;
    this.instanceGrowthRate = 1f;
    this.accumulatedError = 0.0f;
    this.visualDiseaseProvider = (GameObject) null;
    this.autoDisinfectable = go.GetComponent<AutoDisinfectable>();
    if (!((Object) this.autoDisinfectable != (Object) null))
      return;
    AutoDisinfectableManager.Instance.AddAutoDisinfectable(this.autoDisinfectable);
  }

  public void Clear() => this.controller = (KBatchedAnimController) null;
}
