// Decompiled with JetBrains decompiler
// Type: Klei.AI.EffectInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace Klei.AI;

[DebuggerDisplay("{effect.Id}")]
public class EffectInstance : ModifierInstance<Effect>
{
  public Effect effect;
  public bool shouldSave;
  public StatusItem statusItem;
  public float timeRemaining;
  public EmoteReactable reactable;
  protected Effect[] immunityEffects;

  public EffectInstance(
    GameObject game_object,
    Effect effect,
    bool should_save,
    Func<string, object, string> resolveTooltipCallback = null)
    : base(game_object, effect)
  {
    this.effect = effect;
    this.shouldSave = should_save;
    this.DefineEffectImmunities();
    this.ApplyImmunities();
    this.ConfigureStatusItem(resolveTooltipCallback);
    if (effect.showInUI)
    {
      KSelectable component = this.gameObject.GetComponent<KSelectable>();
      if (!component.GetStatusItemGroup().HasStatusItem(this.statusItem))
        component.AddStatusItem(this.statusItem, (object) this);
    }
    if (effect.triggerFloatingText && (UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null)
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, effect.Name, game_object.transform);
    if (effect.emote != null)
      this.RegisterEmote(effect.emote, effect.emoteCooldown);
    if (string.IsNullOrEmpty(effect.emoteAnim))
      return;
    this.RegisterEmote(effect.emoteAnim, effect.emoteCooldown);
  }

  protected void DefineEffectImmunities()
  {
    if (this.immunityEffects != null || this.effect.immunityEffectsNames == null)
      return;
    this.immunityEffects = new Effect[this.effect.immunityEffectsNames.Length];
    for (int index = 0; index < this.immunityEffects.Length; ++index)
      this.immunityEffects[index] = Db.Get().effects.Get(this.effect.immunityEffectsNames[index]);
  }

  protected void ApplyImmunities()
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) || this.immunityEffects == null)
      return;
    Effects component = this.gameObject.GetComponent<Effects>();
    for (int index = 0; index < this.immunityEffects.Length; ++index)
    {
      component.Remove(this.immunityEffects[index]);
      component.AddImmunity(this.immunityEffects[index], this.effect.IdHash.ToString(), false);
    }
  }

  protected void RemoveImmunities()
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) || this.immunityEffects == null)
      return;
    Effects component = this.gameObject.GetComponent<Effects>();
    for (int index = 0; index < this.immunityEffects.Length; ++index)
      component.RemoveImmunity(this.immunityEffects[index], this.effect.IdHash.ToString());
  }

  public void RegisterEmote(string emoteAnim, float cooldown = -1f)
  {
    ReactionMonitor.Instance smi = this.gameObject.GetSMI<ReactionMonitor.Instance>();
    if (smi == null)
      return;
    bool isOneShot = (double) cooldown < 0.0;
    float globalCooldown = isOneShot ? 100000f : cooldown;
    EmoteReactable emoteReactable = (EmoteReactable) smi.AddSelfEmoteReactable(this.gameObject, this.effect.Name + "_Emote", emoteAnim, isOneShot, Db.Get().ChoreTypes.Emote, globalCooldown, maxInitialDelay: this.effect.maxInitialDelay, emotePreconditions: this.effect.emotePreconditions);
    if (emoteReactable == null)
      return;
    emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
    if (isOneShot)
      return;
    this.reactable = emoteReactable;
  }

  public void RegisterEmote(Emote emote, float cooldown = -1f)
  {
    ReactionMonitor.Instance smi = this.gameObject.GetSMI<ReactionMonitor.Instance>();
    if (smi == null)
      return;
    bool isOneShot = (double) cooldown < 0.0;
    float globalCooldown = isOneShot ? 100000f : cooldown;
    EmoteReactable emoteReactable = (EmoteReactable) smi.AddSelfEmoteReactable(this.gameObject, (HashedString) (this.effect.Name + "_Emote"), emote, isOneShot, Db.Get().ChoreTypes.Emote, globalCooldown, maxInitialDelay: this.effect.maxInitialDelay, emotePreconditions: this.effect.emotePreconditions);
    if (emoteReactable == null)
      return;
    emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
    if (isOneShot)
      return;
    this.reactable = emoteReactable;
  }

  private bool NotInATube(GameObject go, Navigator.ActiveTransition transition)
  {
    return transition.navGridTransition.start != NavType.Tube && transition.navGridTransition.end != NavType.Tube;
  }

  public override void OnCleanUp()
  {
    if (this.statusItem != null)
    {
      this.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.statusItem);
      this.statusItem = (StatusItem) null;
    }
    if (this.reactable != null)
    {
      this.reactable.Cleanup();
      this.reactable = (EmoteReactable) null;
    }
    this.RemoveImmunities();
  }

  public float GetTimeRemaining() => this.timeRemaining;

  public bool IsExpired()
  {
    return (double) this.effect.duration > 0.0 && (double) this.timeRemaining <= 0.0;
  }

  private void ConfigureStatusItem(
    Func<string, object, string> resolveTooltipCallback)
  {
    StatusItem.IconType iconType = this.effect.isBad ? StatusItem.IconType.Exclamation : StatusItem.IconType.Info;
    if (!this.effect.customIcon.IsNullOrWhiteSpace())
      iconType = StatusItem.IconType.Custom;
    string id1 = this.effect.Id;
    string name = this.effect.Name;
    string description = this.effect.description;
    string customIcon = this.effect.customIcon;
    int icon_type = (int) iconType;
    int notification_type = this.effect.isBad ? 1 : 4;
    bool showStatusInWorld = this.effect.showStatusInWorld;
    HashedString id2 = OverlayModes.None.ID;
    int num = showStatusInWorld ? 1 : 0;
    this.statusItem = new StatusItem(id1, name, description, customIcon, (StatusItem.IconType) icon_type, (NotificationType) notification_type, false, id2, 2, num != 0);
    this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveString);
    this.statusItem.resolveTooltipCallback = resolveTooltipCallback ?? new Func<string, object, string>(this.ResolveTooltip);
  }

  private string ResolveString(string str, object data) => str;

  public string ResolveTooltip(string str, object data)
  {
    string str1 = str;
    EffectInstance effectInstance = (EffectInstance) data;
    string tooltip = Effect.CreateTooltip(effectInstance.effect, false);
    if (!string.IsNullOrEmpty(tooltip))
      str1 = $"{str1}\n\n{tooltip}";
    if ((double) effectInstance.effect.duration > 0.0)
      str1 = $"{str1}\n\n{string.Format((string) DUPLICANTS.MODIFIERS.TIME_REMAINING, (object) GameUtil.GetFormattedCycles(this.GetTimeRemaining()))}";
    return str1;
  }
}
