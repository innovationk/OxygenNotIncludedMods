// Decompiled with JetBrains decompiler
// Type: Klei.AI.Effect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace Klei.AI;

[DebuggerDisplay("{Id}")]
public class Effect : Modifier
{
  public float duration;
  public bool showInUI;
  public bool triggerFloatingText;
  public bool isBad;
  public bool showStatusInWorld;
  public string customIcon;
  public string[] immunityEffectsNames;
  public Tag? tag;
  public string emoteAnim;
  public Emote emote;
  public float emoteCooldown;
  public float maxInitialDelay;
  public List<Reactable.ReactablePrecondition> emotePreconditions;
  public string stompGroup;
  public int stompPriority;

  public Effect(
    string id,
    string name,
    string description,
    float duration,
    bool show_in_ui,
    bool trigger_floating_text,
    bool is_bad,
    Emote emote = null,
    float emote_cooldown = -1f,
    float max_initial_delay = 0.0f,
    string stompGroup = null,
    string custom_icon = "")
    : this(id, name, description, duration, show_in_ui, trigger_floating_text, is_bad, emote, max_initial_delay, stompGroup, false, custom_icon, emote_cooldown)
  {
  }

  public Effect(
    string id,
    string name,
    string description,
    float duration,
    bool show_in_ui,
    bool trigger_floating_text,
    bool is_bad,
    Emote emote,
    float max_initial_delay,
    string stompGroup,
    bool showStatusInWorld,
    string custom_icon = "",
    float emote_cooldown = -1f)
    : this(id, name, description, duration, (string[]) null, show_in_ui, trigger_floating_text, is_bad, emote, max_initial_delay, stompGroup, showStatusInWorld, custom_icon, emote_cooldown)
  {
  }

  public Effect(
    string id,
    string name,
    string description,
    float duration,
    string[] immunityEffects,
    bool show_in_ui,
    bool trigger_floating_text,
    bool is_bad,
    Emote emote,
    float max_initial_delay,
    string stompGroup,
    bool showStatusInWorld,
    string custom_icon = "",
    float emote_cooldown = -1f)
    : base(id, name, description)
  {
    this.duration = duration;
    this.showInUI = show_in_ui;
    this.triggerFloatingText = trigger_floating_text;
    this.isBad = is_bad;
    this.emote = emote;
    this.emoteCooldown = emote_cooldown;
    this.maxInitialDelay = max_initial_delay;
    this.stompGroup = stompGroup;
    this.customIcon = custom_icon;
    this.showStatusInWorld = showStatusInWorld;
    this.immunityEffectsNames = immunityEffects;
  }

  public Effect(
    string id,
    string name,
    string description,
    float duration,
    bool show_in_ui,
    bool trigger_floating_text,
    bool is_bad,
    string emoteAnim,
    float emote_cooldown = -1f,
    string stompGroup = null,
    string custom_icon = "")
    : base(id, name, description)
  {
    this.duration = duration;
    this.showInUI = show_in_ui;
    this.triggerFloatingText = trigger_floating_text;
    this.isBad = is_bad;
    this.emoteAnim = emoteAnim;
    this.emoteCooldown = emote_cooldown;
    this.stompGroup = stompGroup;
    this.customIcon = custom_icon;
  }

  public override void AddTo(Attributes attributes) => base.AddTo(attributes);

  public override void RemoveFrom(Attributes attributes) => base.RemoveFrom(attributes);

  public void SetEmote(Emote emote, float emoteCooldown = -1f)
  {
    this.emote = emote;
    this.emoteCooldown = emoteCooldown;
  }

  public void AddEmotePrecondition(Reactable.ReactablePrecondition precon)
  {
    if (this.emotePreconditions == null)
      this.emotePreconditions = new List<Reactable.ReactablePrecondition>();
    this.emotePreconditions.Add(precon);
  }

  public static string CreateTooltip(
    Effect effect,
    bool showDuration,
    string linePrefix = "\n    • ",
    bool showHeader = true)
  {
    StringEntry result;
    Strings.TryGet($"STRINGS.DUPLICANTS.MODIFIERS.{effect.Id.ToUpper()}.ADDITIONAL_EFFECTS", out result);
    string tooltip = !showHeader || effect.SelfModifiers.Count <= 0 && result == null ? "" : DUPLICANTS.MODIFIERS.EFFECT_HEADER.text;
    foreach (AttributeModifier selfModifier in effect.SelfModifiers)
    {
      Attribute attribute = AttributeModifier.FetchAttribute(selfModifier.AttributeId);
      if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
        tooltip = tooltip + linePrefix + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) selfModifier.GetName(), (object) selfModifier.GetFormattedString());
    }
    if (effect.immunityEffectsNames != null)
    {
      tooltip = tooltip + (string.IsNullOrEmpty(tooltip) ? "" : linePrefix + linePrefix) + (!showHeader || effect.immunityEffectsNames == null || effect.immunityEffectsNames.Length == 0 ? "" : DUPLICANTS.MODIFIERS.EFFECT_IMMUNITIES_HEADER.text);
      foreach (string immunityEffectsName in effect.immunityEffectsNames)
      {
        Effect effect1 = Db.Get().effects.TryGet(immunityEffectsName);
        if (effect1 != null)
          tooltip = tooltip + linePrefix + string.Format((string) DUPLICANTS.MODIFIERS.IMMUNITY_FORMAT, (object) effect1.Name);
      }
    }
    if (result != null)
      tooltip = tooltip + linePrefix + (string) result;
    if (showDuration && (double) effect.duration > 0.0)
      tooltip = $"{tooltip}\n{string.Format((string) DUPLICANTS.MODIFIERS.TIME_TOTAL, (object) GameUtil.GetFormattedCycles(effect.duration))}";
    return tooltip;
  }

  public static string CreateFullTooltip(Effect effect, bool showDuration)
  {
    return $"{effect.Name}\n\n{effect.description}\n\n{Effect.CreateTooltip(effect, showDuration)}";
  }

  public static void AddModifierDescriptions(
    GameObject parent,
    List<Descriptor> descs,
    string effect_id,
    bool increase_indent = false)
  {
    Effect.AddModifierDescriptions(descs, effect_id, increase_indent);
  }

  public static void AddModifierDescriptions(
    List<Descriptor> descs,
    string effect_id,
    bool increase_indent = false,
    string prefix = "STRINGS.DUPLICANTS.ATTRIBUTES.")
  {
    foreach (AttributeModifier selfModifier in Db.Get().effects.Get(effect_id).SelfModifiers)
    {
      Descriptor descriptor = new Descriptor($"{(string) Strings.Get($"{prefix}{selfModifier.AttributeId.ToUpper()}.NAME")}: {selfModifier.GetFormattedString()}", "");
      if (increase_indent)
        descriptor.IncreaseIndent();
      descs.Add(descriptor);
    }
  }
}
