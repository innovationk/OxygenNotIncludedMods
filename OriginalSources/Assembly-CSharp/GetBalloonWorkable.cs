// Decompiled with JetBrains decompiler
// Type: GetBalloonWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/GetBalloonWorkable")]
public class GetBalloonWorkable : Workable
{
  private static readonly HashedString[] GET_BALLOON_ANIMS = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString PST_ANIM = new HashedString("working_pst");
  private BalloonArtistChore.StatesInstance balloonArtist;
  private const string TARGET_SYMBOL_TO_OVERRIDE = "body";
  private const int TARGET_OVERRIDE_PRIORITY = 0;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.faceTargetWhenWorking = true;
    this.workerStatusItem = (StatusItem) null;
    this.workingStatusItem = (StatusItem) null;
    this.workAnims = GetBalloonWorkable.GET_BALLOON_ANIMS;
    this.workingPstComplete = new HashedString[1]
    {
      GetBalloonWorkable.PST_ANIM
    };
    this.workingPstFailed = new HashedString[1]
    {
      GetBalloonWorkable.PST_ANIM
    };
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
    if (balloonOverride.animFile.IsNone())
      worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) "body", Assets.GetAnim((HashedString) "balloon_anim_kanim").GetData().build.GetSymbol((KAnimHashedString) "body"));
    else
      worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) "body", balloonOverride.symbol.Unwrap());
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) "EquippableBalloon"), worker.transform.GetPosition());
    gameObject.GetComponent<Equippable>().Assign((IAssignableIdentity) worker.GetComponent<MinionIdentity>());
    gameObject.GetComponent<Equippable>().isEquipped = true;
    gameObject.SetActive(true);
    base.OnCompleteWork(worker);
    BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
    this.balloonArtist.GiveBalloon(balloonOverride);
    gameObject.GetComponent<EquippableBalloon>().SetBalloonOverride(balloonOverride);
  }

  public override Vector3 GetFacingTarget() => this.balloonArtist.master.transform.GetPosition();

  public void SetBalloonArtist(BalloonArtistChore.StatesInstance chore)
  {
    this.balloonArtist = chore;
  }

  public BalloonArtistChore.StatesInstance GetBalloonArtist() => this.balloonArtist;
}
