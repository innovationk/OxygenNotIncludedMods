// Decompiled with JetBrains decompiler
// Type: HugMinionReactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class HugMinionReactable(GameObject gameObject) : Reactable(gameObject, (HashedString) nameof (HugMinionReactable), Db.Get().ChoreTypes.Hug, 1, 1, true, 1f, overrideLayer: ObjectLayer.Minion)
{
  public override bool InternalCanBegin(
    GameObject newReactor,
    Navigator.ActiveTransition transition)
  {
    if ((Object) this.reactor != (Object) null)
      return false;
    Navigator component = newReactor.GetComponent<Navigator>();
    return !((Object) component == (Object) null) && component.IsMoving();
  }

  public override void Update(float dt)
  {
    this.gameObject.GetComponent<Facing>().SetFacing(this.reactor.GetComponent<Facing>().GetFacing());
  }

  protected override void InternalBegin()
  {
    KAnimControllerBase component = this.reactor.GetComponent<KAnimControllerBase>();
    component.AddAnimOverrides(Assets.GetAnim((HashedString) "anim_react_pip_kanim"));
    component.Play((HashedString) "hug_dupe_pre");
    component.Queue((HashedString) "hug_dupe_loop");
    component.Queue((HashedString) "hug_dupe_pst");
    component.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.Finish);
    this.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(new HashedString[3]
    {
      (HashedString) "hug_dupe_pre",
      (HashedString) "hug_dupe_loop",
      (HashedString) "hug_dupe_pst"
    });
  }

  private void Finish(HashedString anim)
  {
    if (!(anim == (HashedString) "hug_dupe_pst"))
      return;
    if ((Object) this.reactor != (Object) null)
    {
      this.reactor.GetComponent<KAnimControllerBase>().onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.Finish);
      this.ApplyEffects();
    }
    else
      DebugUtil.LogWarningArgs((object) "HugMinionReactable finishing without adding a Hugged effect.");
    this.End();
  }

  private void ApplyEffects()
  {
    this.reactor.GetComponent<Effects>().Add("Hugged", true);
    this.gameObject.GetSMI<HugMonitor.Instance>()?.EnterHuggingFrenzy();
  }

  protected override void InternalEnd()
  {
  }

  protected override void InternalCleanup()
  {
  }
}
