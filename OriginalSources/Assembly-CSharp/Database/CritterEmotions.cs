// Decompiled with JetBrains decompiler
// Type: Database.CritterEmotions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public class CritterEmotions : ResourceSet<Thought>
{
  public CritterEmotion Hungry;
  public CritterEmotion Hot;
  public CritterEmotion Cold;
  public CritterEmotion Cramped;
  public CritterEmotion Crowded;
  public CritterEmotion Suffocating;
  public CritterEmotion WellFed;
  public CritterEmotion Happy;

  public CritterEmotions(ResourceSet parent)
    : base(nameof (CritterEmotions), parent)
  {
    this.Hungry = new CritterEmotion(nameof (Hungry), false, Assets.GetSprite((HashedString) "crew_state_hungry"));
    this.Hot = new CritterEmotion(nameof (Hot), false, Assets.GetSprite((HashedString) "crew_state_temp_up"));
    this.Cold = new CritterEmotion(nameof (Cold), false, Assets.GetSprite((HashedString) "crew_state_temp_down"));
    this.Cramped = new CritterEmotion(nameof (Cramped), false, Assets.GetSprite((HashedString) "crew_state_stress"));
    this.Crowded = new CritterEmotion(nameof (Crowded), false, Assets.GetSprite((HashedString) "crew_state_stress"));
    this.Suffocating = new CritterEmotion(nameof (Suffocating), false, Assets.GetSprite((HashedString) "crew_state_cantbreathe"));
    this.WellFed = new CritterEmotion(nameof (WellFed), true, Assets.GetSprite((HashedString) "crew_state_binge_eat"));
    this.Happy = new CritterEmotion(nameof (Happy), true, Assets.GetSprite((HashedString) "crew_state_happy"));
  }
}
