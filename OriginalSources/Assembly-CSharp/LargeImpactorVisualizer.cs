// Decompiled with JetBrains decompiler
// Type: LargeImpactorVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class LargeImpactorVisualizer : KMonoBehaviour
{
  public bool Active;
  private const string SFX_Fold = "HUD_Demolior_LandingZone_close_fx";
  public Vector2I OriginOffset;
  public Vector2 ScreenSpaceNotificationTogglePosition = Vector2.zero;
  public Vector2I RangeMin;
  public Vector2I RangeMax;
  public Vector2I TexSize = new Vector2I(64 /*0x40*/, 64 /*0x40*/);
  public bool TestLineOfSight;
  public bool BlockingTileVisible;
  public Func<int, bool> BlockingVisibleCb;
  public Func<int, bool> BlockingCb = new Func<int, bool>(Grid.IsSolidCell);
  public bool AllowLineOfSightInvalidCells;

  public bool Visible => this.Active && !this.Folded;

  public bool Folded { private set; get; } = true;

  public float LastTimeSetToFolded { private set; get; }

  public bool ShouldResetEntryEffect { private set; get; }

  public float EntryEffectDuration { private set; get; } = 3f;

  public float FoldEffectDuration { private set; get; } = 1f;

  public void BeginEntryEffect(float duration)
  {
    this.EntryEffectDuration = duration;
    this.SetShouldResetEntryEffect(true);
  }

  public void SetShouldResetEntryEffect(bool shouldIt) => this.ShouldResetEntryEffect = shouldIt;

  public void SetFoldedState(bool shouldBeFolded)
  {
    if (!this.Folded & shouldBeFolded)
    {
      this.LastTimeSetToFolded = Time.unscaledTime;
      if (this.Active)
        KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Demolior_LandingZone_close_fx"));
    }
    this.Folded = shouldBeFolded;
    if (shouldBeFolded)
      return;
    this.LastTimeSetToFolded = float.MaxValue;
  }
}
