// Decompiled with JetBrains decompiler
// Type: SaveActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class SaveActive : KScreen
{
  [MyCmpGet]
  private KBatchedAnimController controller;
  private Game.CansaveCB readyForSaveCallback;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.SetAutoSaveCallbacks(new Game.SavingPreCB(this.ActiveateSaveIndicator), new Game.SavingActiveCB(this.SetActiveSaveIndicator), new Game.SavingPostCB(this.DeactivateSaveIndicator));
  }

  private void DoCallBack(HashedString name)
  {
    this.controller.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.DoCallBack);
    this.readyForSaveCallback();
    this.readyForSaveCallback = (Game.CansaveCB) null;
  }

  private void ActiveateSaveIndicator(Game.CansaveCB cb)
  {
    this.readyForSaveCallback = cb;
    this.controller.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.DoCallBack);
    this.controller.Play((HashedString) "working_pre");
  }

  private void SetActiveSaveIndicator() => this.controller.Play((HashedString) "working_loop");

  private void DeactivateSaveIndicator() => this.controller.Play((HashedString) "working_pst");

  public override void OnKeyDown(KButtonEvent e)
  {
  }
}
