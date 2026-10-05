// Decompiled with JetBrains decompiler
// Type: DissolvingAlgae
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class DissolvingAlgae : DissolvingElementDiseaseEmitter
{
  private const int LIGHT_THRESHOLD = 500;

  public DissolvingAlgae()
    : base(SimHashes.Oxygen)
  {
  }

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void EvaluateEmissionCondition()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    bool enable = Grid.IsValidCell(cell) && Grid.IsLiquid(cell) && Grid.Element[cell].HasTag(GameTags.AnyWater) && Grid.LightIntensity[cell] > 500;
    if (this.enableEmitter == enable)
      return;
    this.SetEnable(enable);
    this.UpdateStatusItem();
    if (!enable)
      return;
    this.SpawnVisualFX();
  }
}
