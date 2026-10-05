// Decompiled with JetBrains decompiler
// Type: UnderwaterSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UnderwaterSoundEvent : SoundEvent
{
  public const string POSTFIX = "_uw";
  private readonly string underwaterSound;

  public UnderwaterSoundEvent(
    string file_name,
    string sound_name,
    int frame,
    bool do_load,
    bool is_looping,
    float min_interval,
    bool is_dynamic)
    : base(file_name, sound_name, frame, do_load, is_looping, min_interval, is_dynamic)
  {
    this.underwaterSound = StringFormatter.Combine(this.sound, "_uw");
  }

  public static bool IsVisiblyInLiquid(Vector3 position)
  {
    int cell1 = Grid.PosToCell(new Vector2(position.x, position.y - 0.05f));
    if (!Grid.IsValidCell(cell1) || !Grid.IsLiquid(cell1))
      return false;
    int cell2 = Grid.CellAbove(cell1);
    if (Grid.IsValidCell(cell2) && Grid.IsLiquid(cell2))
      return true;
    return (double) Grid.Mass[cell1] / 1000.0 >= (double) (position.y - (float) (int) position.y);
  }

  private bool TryResolveMultitool(AnimEventManager.EventPlayerData behaviour, out string result)
  {
    KBatchedAnimEventToggler componentInParent = behaviour.controller.GetComponentInParent<KBatchedAnimEventToggler>();
    if ((Object) componentInParent != (Object) null && componentInParent.gameObject.name == "LaserEffect")
    {
      bool flag = UnderwaterSoundEvent.IsVisiblyInLiquid(behaviour.controller.transform.GetPosition());
      result = flag ? this.underwaterSound : this.sound;
      return true;
    }
    result = (string) null;
    return false;
  }

  private string Resolve(AnimEventManager.EventPlayerData behaviour)
  {
    string result;
    if (this.TryResolveMultitool(behaviour, out result))
      return result;
    if (Grid.IsNavigatableLiquid(Grid.PosToCell(behaviour.position)))
      return this.underwaterSound;
    Navigator component;
    if (!behaviour.controller.transform.root.TryGetComponent<Navigator>(out component))
      return this.sound;
    return component.CurrentNavType != NavType.Swim ? this.sound : this.underwaterSound;
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    this.PlaySound(behaviour, this.Resolve(behaviour));
  }

  public override void Stop(AnimEventManager.EventPlayerData behaviour)
  {
    LoopingSounds component;
    if (!this.looping || !behaviour.controller.TryGetComponent<LoopingSounds>(out component))
      return;
    component.StopSound(this.sound);
    component.StopSound(this.underwaterSound);
  }
}
