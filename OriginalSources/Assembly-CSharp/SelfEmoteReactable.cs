// Decompiled with JetBrains decompiler
// Type: SelfEmoteReactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelfEmoteReactable(
  GameObject gameObject,
  HashedString id,
  ChoreType chore_type,
  float globalCooldown = 0.0f,
  float localCooldown = 20f,
  float lifeSpan = float.PositiveInfinity,
  float max_initial_delay = 0.0f) : EmoteReactable(gameObject, id, chore_type, 3, 3, globalCooldown, localCooldown, lifeSpan, max_initial_delay)
{
  private EmoteChore chore;

  public override bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition)
  {
    if ((Object) reactor != (Object) this.gameObject)
      return false;
    Navigator component = reactor.GetComponent<Navigator>();
    return !((Object) component == (Object) null) && component.IsMoving();
  }

  public void PairEmote(EmoteChore emoteChore) => this.chore = emoteChore;

  protected override void InternalEnd()
  {
    if (this.chore != null && (Object) this.chore.driver != (Object) null)
    {
      this.chore.PairReactable((SelfEmoteReactable) null);
      this.chore.Cancel("Reactable ended");
      this.chore = (EmoteChore) null;
    }
    base.InternalEnd();
  }
}
