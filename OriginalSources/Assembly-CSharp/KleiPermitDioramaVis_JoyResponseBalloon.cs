// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_JoyResponseBalloon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

#nullable disable
public class KleiPermitDioramaVis_JoyResponseBalloon : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
  private const int FRAMES_TO_MAKE_BALLOON_IN_ANIM = 39;
  private const float SECONDS_TO_MAKE_BALLOON_IN_ANIM = 1.3f;
  private const float SECONDS_BETWEEN_BALLOONS = 1.618f;
  [SerializeField]
  private UIMinion minionUI;
  private bool didAddAnims;
  private const string TARGET_SYMBOL_TO_OVERRIDE = "body";
  private const int TARGET_OVERRIDE_PRIORITY = 0;
  private Option<Personality> specificPersonality;
  private Option<PermitResource> lastConfiguredPermit;
  private Option<Updater> updaterToRunOnStart;
  private Coroutine updaterRoutine;

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureSetup()
  {
    this.minionUI.transform.localScale = Vector3.one * 0.7f;
    this.minionUI.transform.localPosition = new Vector3(this.minionUI.transform.localPosition.x - 73f, (float) ((double) this.minionUI.transform.localPosition.y - 152.0 + 8.0), this.minionUI.transform.localPosition.z);
  }

  public void ConfigureWith(PermitResource permit)
  {
    this.ConfigureWith(Option.Some<BalloonArtistFacadeResource>((BalloonArtistFacadeResource) permit));
  }

  public void ConfigureWith(Option<BalloonArtistFacadeResource> permit)
  {
    KBatchedAnimController component = this.minionUI.SpawnedAvatar.GetComponent<KBatchedAnimController>();
    SymbolOverrideController minionSymbolOverrider = this.minionUI.SpawnedAvatar.GetComponent<SymbolOverrideController>();
    this.minionUI.SetMinion(this.specificPersonality.UnwrapOrElse((Func<Personality>) (() => Db.Get().Personalities.GetAll(true, true).Where<Personality>((Func<Personality, bool>) (p => p.joyTrait == "BalloonArtist")).GetRandom<Personality>())));
    if (!this.didAddAnims)
    {
      this.didAddAnims = true;
      component.AddAnimOverrides(Assets.GetAnim((HashedString) "anim_interacts_balloon_artist_kanim"));
    }
    component.Play((HashedString) "working_pre");
    component.Queue((HashedString) "working_loop", KAnim.PlayMode.Loop);
    DisplayNextBalloon();
    this.QueueUpdater(Updater.Series(Updater.WaitForSeconds(1.3f), Updater.Loop((Func<Updater>) (() => Updater.WaitForSeconds(1.618f)), (Func<Updater>) (() => Updater.Do(new System.Action(DisplayNextBalloon))))));

    void DisplayNextBalloon()
    {
      if (permit.IsSome())
        minionSymbolOverrider.AddSymbolOverride((HashedString) "body", permit.Unwrap().GetNextOverride().symbol.Unwrap());
      else
        minionSymbolOverrider.AddSymbolOverride((HashedString) "body", Assets.GetAnim((HashedString) "balloon_anim_kanim").GetData().build.GetSymbol((KAnimHashedString) "body"));
    }
  }

  public void SetMinion(Personality personality)
  {
    this.specificPersonality = (Option<Personality>) personality;
    if (!this.gameObject.activeInHierarchy)
      return;
    this.minionUI.SetMinion(personality);
  }

  private void QueueUpdater(Updater updater)
  {
    if (this.gameObject.activeInHierarchy)
      this.RunUpdater(updater);
    else
      this.updaterToRunOnStart = (Option<Updater>) updater;
  }

  private void RunUpdater(Updater updater)
  {
    if (this.updaterRoutine != null)
    {
      this.StopCoroutine(this.updaterRoutine);
      this.updaterRoutine = (Coroutine) null;
    }
    this.updaterRoutine = this.StartCoroutine((IEnumerator) updater);
  }

  private void OnEnable()
  {
    if (!this.updaterToRunOnStart.IsSome())
      return;
    this.RunUpdater(this.updaterToRunOnStart.Unwrap());
    this.updaterToRunOnStart = (Option<Updater>) Option.None;
  }
}
