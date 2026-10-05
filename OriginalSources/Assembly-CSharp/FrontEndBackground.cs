// Decompiled with JetBrains decompiler
// Type: FrontEndBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FrontEndBackground : UIDupeRandomizer
{
  private KBatchedAnimController dreckoController;
  private float nextDreckoTime;
  private FrontEndBackground.Tuning tuning;

  protected override void Start()
  {
    this.tuning = TuningData<FrontEndBackground.Tuning>.Get();
    base.Start();
    for (int minion_idx = 0; minion_idx < this.anims.Length; ++minion_idx)
    {
      int minionIndex = minion_idx;
      KBatchedAnimController minion = this.anims[minion_idx].minions[0];
      if (minion.gameObject.activeInHierarchy)
      {
        minion.onAnimComplete += (KAnimControllerBase.KAnimEvent) (name => this.WaitForABit(minionIndex, name));
        this.WaitForABit(minion_idx, HashedString.Invalid);
      }
    }
    this.dreckoController = this.transform.GetChild(0).Find("startmenu_drecko").GetComponent<KBatchedAnimController>();
    if (!this.dreckoController.gameObject.activeInHierarchy)
      return;
    this.dreckoController.enabled = false;
    this.nextDreckoTime = Random.Range(this.tuning.minFirstDreckoInterval, this.tuning.maxFirstDreckoInterval) + Time.unscaledTime;
  }

  protected override void Update()
  {
    base.Update();
    this.UpdateDrecko();
  }

  private void UpdateDrecko()
  {
    if (!this.dreckoController.gameObject.activeInHierarchy || (double) Time.unscaledTime <= (double) this.nextDreckoTime)
      return;
    this.dreckoController.enabled = true;
    this.dreckoController.Play((HashedString) "idle");
    this.nextDreckoTime = Random.Range(this.tuning.minDreckoInterval, this.tuning.maxDreckoInterval) + Time.unscaledTime;
  }

  private void WaitForABit(int minion_idx, HashedString name)
  {
    this.StartCoroutine(this.WaitForTime(minion_idx));
  }

  private IEnumerator WaitForTime(int minion_idx)
  {
    this.anims[minion_idx].lastWaitTime = Random.Range(this.anims[minion_idx].minSecondsBetweenAction, this.anims[minion_idx].maxSecondsBetweenAction);
    yield return (object) new WaitForSecondsRealtime(this.anims[minion_idx].lastWaitTime);
    this.GetNewBody(minion_idx);
    foreach (KBatchedAnimController minion in this.anims[minion_idx].minions)
    {
      minion.ClearQueue();
      minion.Play((HashedString) this.anims[minion_idx].anim_name);
    }
  }

  public class Tuning : TuningData<FrontEndBackground.Tuning>
  {
    public float minDreckoInterval;
    public float maxDreckoInterval;
    public float minFirstDreckoInterval;
    public float maxFirstDreckoInterval;
  }
}
