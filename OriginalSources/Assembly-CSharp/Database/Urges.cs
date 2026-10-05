// Decompiled with JetBrains decompiler
// Type: Database.Urges
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public class Urges : ResourceSet<Urge>
{
  public Urge BeIncapacitated;
  public Urge BeOffline;
  public Urge Sleep;
  public Urge Narcolepsy;
  public Urge Eat;
  public Urge ReloadElectrobank;
  public Urge WashHands;
  public Urge Shower;
  public Urge Pee;
  public Urge MoveToQuarantine;
  public Urge HealCritical;
  public Urge RecoverBreath;
  public Urge FindOxygenRefill;
  public Urge RecoverWarmth;
  public Urge Emote;
  public Urge Feed;
  public Urge Doctor;
  public Urge Flee;
  public Urge Heal;
  public Urge PacifyIdle;
  public Urge PacifyEat;
  public Urge PacifySleep;
  public Urge PacifyRelocate;
  public Urge RestDueToDisease;
  public Urge EmoteHighPriority;
  public Urge Aggression;
  public Urge MoveToSafety;
  public Urge WarmUp;
  public Urge CoolDown;
  public Urge LearnSkill;
  public Urge EmoteIdle;
  public Urge OilRefill;
  public Urge GunkPee;
  public Urge Fart;

  public Urges()
  {
    this.HealCritical = this.Add(new Urge(nameof (HealCritical)));
    this.BeOffline = this.Add(new Urge(nameof (BeOffline)));
    this.BeIncapacitated = this.Add(new Urge(nameof (BeIncapacitated)));
    this.PacifyEat = this.Add(new Urge(nameof (PacifyEat)));
    this.PacifySleep = this.Add(new Urge(nameof (PacifySleep)));
    this.PacifyIdle = this.Add(new Urge(nameof (PacifyIdle)));
    this.EmoteHighPriority = this.Add(new Urge(nameof (EmoteHighPriority)));
    this.RecoverBreath = this.Add(new Urge(nameof (RecoverBreath)));
    this.RecoverWarmth = this.Add(new Urge(nameof (RecoverWarmth)));
    this.Aggression = this.Add(new Urge(nameof (Aggression)));
    this.MoveToQuarantine = this.Add(new Urge(nameof (MoveToQuarantine)));
    this.WashHands = this.Add(new Urge(nameof (WashHands)));
    this.Shower = this.Add(new Urge(nameof (Shower)));
    this.Eat = this.Add(new Urge(nameof (Eat)));
    this.ReloadElectrobank = this.Add(new Urge(nameof (ReloadElectrobank)));
    this.Pee = this.Add(new Urge(nameof (Pee)));
    this.RestDueToDisease = this.Add(new Urge(nameof (RestDueToDisease)));
    this.Sleep = this.Add(new Urge(nameof (Sleep)));
    this.Narcolepsy = this.Add(new Urge(nameof (Narcolepsy)));
    this.Doctor = this.Add(new Urge(nameof (Doctor)));
    this.Heal = this.Add(new Urge(nameof (Heal)));
    this.Feed = this.Add(new Urge(nameof (Feed)));
    this.PacifyRelocate = this.Add(new Urge(nameof (PacifyRelocate)));
    this.Emote = this.Add(new Urge(nameof (Emote)));
    this.MoveToSafety = this.Add(new Urge(nameof (MoveToSafety)));
    this.WarmUp = this.Add(new Urge(nameof (WarmUp)));
    this.CoolDown = this.Add(new Urge(nameof (CoolDown)));
    this.LearnSkill = this.Add(new Urge(nameof (LearnSkill)));
    this.EmoteIdle = this.Add(new Urge(nameof (EmoteIdle)));
    this.OilRefill = this.Add(new Urge(nameof (OilRefill)));
    this.GunkPee = this.Add(new Urge(nameof (GunkPee)));
    this.FindOxygenRefill = this.Add(new Urge(nameof (FindOxygenRefill)));
    this.Fart = this.Add(new Urge(nameof (Fart)));
  }
}
