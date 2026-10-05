// Decompiled with JetBrains decompiler
// Type: ComplexFabricatorLayeredWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class ComplexFabricatorLayeredWorkable : ComplexFabricatorWorkable
{
  public Grid.SceneLayer foregroundLayer = Grid.SceneLayer.NoLayer;

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    Workable.AnimInfo anim = base.GetAnim(worker);
    if (this.foregroundLayer != Grid.SceneLayer.NoLayer)
      anim.smi = (StateMachine.Instance) new SimpleLayeredAnimWork.Instance(this.gameObject, worker, this.foregroundLayer, this.synchronizeAnims, anim.overrideAnims);
    return anim;
  }
}
