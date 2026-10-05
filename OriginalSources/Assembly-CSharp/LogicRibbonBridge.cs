// Decompiled with JetBrains decompiler
// Type: LogicRibbonBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class LogicRibbonBridge : KMonoBehaviour
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    switch (this.GetComponent<Rotatable>().GetOrientation())
    {
      case Orientation.Neutral:
        component.Play((HashedString) "0");
        break;
      case Orientation.R90:
        component.Play((HashedString) "90");
        break;
      case Orientation.R180:
        component.Play((HashedString) "180");
        break;
      case Orientation.R270:
        component.Play((HashedString) "270");
        break;
    }
  }
}
