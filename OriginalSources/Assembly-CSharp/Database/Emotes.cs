// Decompiled with JetBrains decompiler
// Type: Database.Emotes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

#nullable disable
namespace Database;

public class Emotes : ResourceSet<Resource>
{
  public Emotes.MinionEmotes Minion;
  public Emotes.CritterEmotes Critter;

  public Emotes(ResourceSet parent)
    : base(nameof (Emotes), parent)
  {
    this.Minion = new Emotes.MinionEmotes((ResourceSet) this);
    this.Critter = new Emotes.CritterEmotes((ResourceSet) this);
  }

  public void ResetProblematicReferences()
  {
    for (int index = 0; index < this.Minion.resources.Count; ++index)
    {
      Emote resource = this.Minion.resources[index];
      for (int stepIdx = 0; stepIdx < resource.StepCount; ++stepIdx)
        resource[stepIdx].UnregisterAllCallbacks();
    }
    for (int index = 0; index < this.Critter.resources.Count; ++index)
    {
      Emote resource = this.Critter.resources[index];
      for (int stepIdx = 0; stepIdx < resource.StepCount; ++stepIdx)
        resource[stepIdx].UnregisterAllCallbacks();
    }
  }

  public class MinionEmotes : ResourceSet<Emote>
  {
    private static EmoteStep[] DEFAULT_STEPS = new EmoteStep[1]
    {
      new EmoteStep() { anim = (HashedString) "react" }
    };
    private static EmoteStep[] DEFAULT_IDLE_STEPS = new EmoteStep[3]
    {
      new EmoteStep() { anim = (HashedString) "idle_pre" },
      new EmoteStep() { anim = (HashedString) "idle_default" },
      new EmoteStep() { anim = (HashedString) "idle_pst" }
    };
    public Emote ClapCheer;
    public Emote Cheer;
    public Emote ProductiveCheer;
    public Emote ResearchComplete;
    public Emote ThumbsUp;
    public Emote CloseCall_Fall;
    public Emote Cold;
    public Emote Cough;
    public Emote Cough_Small;
    public Emote FoodPoisoning;
    public Emote Hot;
    public Emote IritatedEyes;
    public Emote MorningStretch;
    public Emote Radiation_Glare;
    public Emote Radiation_Itch;
    public Emote Sick;
    public Emote Sneeze;
    public Emote SoreBack;
    public Emote WaterDamage;
    public Emote Sneeze_Short;
    public Emote GrindingGears;
    public Emote Concern;
    public Emote Cringe;
    public Emote Disappointed;
    public Emote Shock;
    public Emote Sing;
    public Emote FingerGuns;
    public Emote Wave;
    public Emote Wave_Shy;

    public MinionEmotes(ResourceSet parent)
      : base("Minion", parent)
    {
      this.InitializeCelebrations();
      this.InitializePhysicalStatus();
      this.InitializeEmotionalStatus();
      this.InitializeGreetings();
    }

    public void InitializeCelebrations()
    {
      this.ClapCheer = new Emote((ResourceSet) this, "ClapCheer", new EmoteStep[3]
      {
        new EmoteStep() { anim = (HashedString) "clapcheer_pre" },
        new EmoteStep() { anim = (HashedString) "clapcheer_loop" },
        new EmoteStep() { anim = (HashedString) "clapcheer_pst" }
      }, "anim_clapcheer_kanim", "anim_clapcheer_swim_kanim");
      this.Cheer = new Emote((ResourceSet) this, "Cheer", new EmoteStep[3]
      {
        new EmoteStep() { anim = (HashedString) "cheer_pre" },
        new EmoteStep() { anim = (HashedString) "cheer_loop" },
        new EmoteStep() { anim = (HashedString) "cheer_pst" }
      }, "anim_cheer_kanim");
      this.ProductiveCheer = new Emote((ResourceSet) this, "Productive Cheer", new EmoteStep[1]
      {
        new EmoteStep() { anim = (HashedString) "productive" }
      }, "anim_productive_kanim", "anim_productive_swim_kanim");
      this.ResearchComplete = new Emote((ResourceSet) this, "ResearchComplete", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_research_complete_kanim");
      this.ThumbsUp = new Emote((ResourceSet) this, "ThumbsUp", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_thumbsup_kanim", "anim_react_thumbsup_swim_kanim");
    }

    private void InitializePhysicalStatus()
    {
      this.CloseCall_Fall = new Emote((ResourceSet) this, "Near Fall", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_floor_missing_kanim");
      this.Cold = new Emote((ResourceSet) this, "Cold", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_cold_kanim", "anim_idle_cold_swim_kanim");
      this.Cough = new Emote((ResourceSet) this, "Cough", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_slimelungcough_kanim", "anim_slimelungcough_swim_kanim");
      this.Cough_Small = new Emote((ResourceSet) this, "Small Cough", new EmoteStep[1]
      {
        new EmoteStep() { anim = (HashedString) "react_small" }
      }, "anim_slimelungcough_kanim");
      this.FoodPoisoning = new Emote((ResourceSet) this, "Food Poisoning", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_contaminated_food_kanim", "anim_react_contaminated_food_swim_kanim");
      this.Hot = new Emote((ResourceSet) this, "Hot", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_hot_kanim", "anim_idle_hot_swim_kanim");
      this.IritatedEyes = new Emote((ResourceSet) this, "Irritated Eyes", new EmoteStep[1]
      {
        new EmoteStep() { anim = (HashedString) "irritated_eyes" }
      }, "anim_irritated_eyes_kanim", "anim_irritated_eyes_swim_kanim");
      this.MorningStretch = new Emote((ResourceSet) this, "Morning Stretch", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_morning_stretch_kanim", "anim_react_morning_stretch_swim_kanim");
      this.Radiation_Glare = new Emote((ResourceSet) this, "Radiation Glare", new EmoteStep[1]
      {
        new EmoteStep()
        {
          anim = (HashedString) "react_radiation_glare"
        }
      }, "anim_react_radiation_kanim");
      this.Radiation_Itch = new Emote((ResourceSet) this, "Radiation Itch", new EmoteStep[1]
      {
        new EmoteStep()
        {
          anim = (HashedString) "react_radiation_itch"
        }
      }, "anim_react_radiation_kanim");
      this.Sick = new Emote((ResourceSet) this, "Sick", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_sick_kanim", "anim_idle_sick_swim_kanim");
      this.SoreBack = new Emote((ResourceSet) this, "SoreBack", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_sore_back_kanim", "anim_react_sore_back_swim_kanim");
      this.Sneeze = new Emote((ResourceSet) this, "Sneeze", new EmoteStep[2]
      {
        new EmoteStep() { anim = (HashedString) "sneeze" },
        new EmoteStep() { anim = (HashedString) "sneeze_pst" }
      }, "anim_sneeze_kanim", "anim_sneeze_swim_kanim");
      this.WaterDamage = new Emote((ResourceSet) this, "WaterDamage", new EmoteStep[1]
      {
        new EmoteStep() { anim = (HashedString) "zapped" }
      }, "anim_bionic_kanim");
      this.GrindingGears = new Emote((ResourceSet) this, "GrindingGears", new EmoteStep[1]
      {
        new EmoteStep() { anim = (HashedString) "react" }
      }, "anim_bionic_react_grinding_gears_kanim");
      this.Sneeze_Short = new Emote((ResourceSet) this, "Short Sneeze", new EmoteStep[2]
      {
        new EmoteStep() { anim = (HashedString) "sneeze_short" },
        new EmoteStep() { anim = (HashedString) "sneeze_short_pst" }
      }, "anim_sneeze_kanim");
    }

    private void InitializeEmotionalStatus()
    {
      this.Concern = new Emote((ResourceSet) this, "Concern", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_concern_kanim");
      this.Cringe = new Emote((ResourceSet) this, "Cringe", new EmoteStep[3]
      {
        new EmoteStep() { anim = (HashedString) "cringe_pre" },
        new EmoteStep() { anim = (HashedString) "cringe_loop" },
        new EmoteStep() { anim = (HashedString) "cringe_pst" }
      }, "anim_cringe_kanim");
      this.Disappointed = new Emote((ResourceSet) this, "Disappointed", new EmoteStep[3]
      {
        new EmoteStep() { anim = (HashedString) "disappointed_pre" },
        new EmoteStep()
        {
          anim = (HashedString) "disappointed_loop"
        },
        new EmoteStep() { anim = (HashedString) "disappointed_pst" }
      }, "anim_disappointed_kanim");
      this.Shock = new Emote((ResourceSet) this, "Shock", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_shock_kanim");
      this.Sing = new Emote((ResourceSet) this, "Sing", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_singer_kanim");
    }

    private void InitializeGreetings()
    {
      this.FingerGuns = new Emote((ResourceSet) this, "Finger Guns", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_fingerguns_kanim");
      this.Wave = new Emote((ResourceSet) this, "Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_kanim");
      this.Wave_Shy = new Emote((ResourceSet) this, "Shy Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_shy_kanim");
    }
  }

  public class CritterEmotes : ResourceSet<Emote>
  {
    public Emote Positive;
    public Emote Negative;
    public Emote Roar;
    public Emote RaptorSignal;

    public CritterEmotes(ResourceSet parent)
      : base("Critter", parent)
    {
      this.InitializeEmotes();
    }

    private void InitializeEmotes()
    {
      this.Positive = new Emote((ResourceSet) this, "Positive", new EmoteStep[1]
      {
        new EmoteStep() { anim = (HashedString) "react_pos" }
      });
      this.Negative = new Emote((ResourceSet) this, "Negative", new EmoteStep[1]
      {
        new EmoteStep() { anim = (HashedString) "react_neg" }
      });
      this.Roar = new Emote((ResourceSet) this, "Roar", new EmoteStep[1]
      {
        new EmoteStep() { anim = (HashedString) "roar" }
      });
      this.RaptorSignal = new Emote((ResourceSet) this, "Signal", new EmoteStep[1]
      {
        new EmoteStep() { anim = (HashedString) "signal" }
      });
    }
  }
}
