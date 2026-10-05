// Decompiled with JetBrains decompiler
// Type: IncrementorToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class IncrementorToggle : MultiToggle
{
  private float timeBetweenIncrementsMin = 0.033f;
  private float timeBetweenIncrementsMax = 0.25f;
  private const float incrementAccelerationScale = 2.5f;
  private float timeToNextIncrement;

  protected override void Update()
  {
    if (!this.clickHeldDown)
      return;
    this.totalHeldTime += Time.unscaledDeltaTime;
    if ((double) this.timeToNextIncrement <= 0.0)
    {
      this.PlayClickSound();
      this.onClick();
      this.timeToNextIncrement = Mathf.Lerp(this.timeBetweenIncrementsMax, this.timeBetweenIncrementsMin, this.totalHeldTime / 2.5f);
    }
    else
      this.timeToNextIncrement -= Time.unscaledDeltaTime;
  }

  private void PlayClickSound()
  {
    if (!this.play_sound_on_click)
      return;
    string name = this.states[this.state].on_click_override_sound_path;
    if (name == "")
      name = "HUD_Click";
    KFMOD.PlayUISound(GlobalAssets.GetSound(name));
  }

  public override void OnPointerUp(PointerEventData eventData)
  {
    base.OnPointerUp(eventData);
    if (!this.AllowClick(eventData))
      return;
    this.timeToNextIncrement = this.timeBetweenIncrementsMax;
  }

  public override void OnPointerDown(PointerEventData eventData)
  {
    if (!this.AllowClick(eventData))
      return;
    if (!this.clickHeldDown)
    {
      this.clickHeldDown = true;
      this.PlayClickSound();
      if (this.onClick != null)
        this.onClick();
    }
    if (this.states.Length - 1 < this.state)
      Debug.LogWarning((object) "Multi toggle has too few / no states");
    this.RefreshHoverColor();
  }

  public override void OnPointerClick(PointerEventData eventData) => this.RefreshHoverColor();
}
