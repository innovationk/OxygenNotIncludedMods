// Decompiled with JetBrains decompiler
// Type: Klei.AI.CommonSickEffectSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Klei.AI;

public class CommonSickEffectSickness : Sickness.SicknessComponent
{
  public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
  {
    KBatchedAnimController effect = FXHelpers.CreateEffect("contaminated_crew_fx_kanim", go.transform.GetPosition() + new Vector3(0.0f, 0.0f, -0.1f), go.transform, true);
    effect.Play((HashedString) "fx_loop", KAnim.PlayMode.Loop);
    return (object) effect;
  }

  public override void OnCure(GameObject go, object instance_data)
  {
    ((Component) instance_data).gameObject.DeleteObject();
  }
}
