// Decompiled with JetBrains decompiler
// Type: Brain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Brain")]
public class Brain : KMonoBehaviour
{
  private bool running;
  private bool suspend;
  protected KPrefabID prefabId;
  protected ChoreConsumer choreConsumer;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    this.prefabId = this.GetComponent<KPrefabID>();
    this.choreConsumer = this.GetComponent<ChoreConsumer>();
    this.running = true;
    Components.Brains.Add(this);
  }

  public event System.Action onPreUpdate;

  public virtual void UpdateBrain()
  {
    SuperluminalPerf.BeginEvent(nameof (UpdateBrain), this.name);
    if (this.onPreUpdate != null)
      this.onPreUpdate();
    if (this.IsRunning())
      this.UpdateChores();
    SuperluminalPerf.EndEvent();
  }

  private bool FindBetterChore(ref Chore.Precondition.Context context)
  {
    return this.choreConsumer.FindNextChore(ref context);
  }

  private void UpdateChores()
  {
    if (this.prefabId.HasTag(GameTags.PreventChoreInterruption))
      return;
    Chore.Precondition.Context context = new Chore.Precondition.Context();
    if (!this.FindBetterChore(ref context))
      return;
    if (this.prefabId.HasTag(GameTags.PerformingWorkRequest))
      this.Trigger(1485595942, (object) null);
    else
      this.choreConsumer.choreDriver.SetChore(context);
  }

  public bool IsRunning() => this.running && !this.suspend;

  public void Reset(string reason)
  {
    this.Stop(nameof (Reset));
    this.running = true;
  }

  public void Stop(string reason)
  {
    this.GetComponent<ChoreDriver>().StopChore();
    this.running = false;
  }

  public void Resume(string caller) => this.suspend = false;

  public void Suspend(string caller) => this.suspend = true;

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    this.Stop(nameof (OnCmpDisable));
  }

  protected override void OnCleanUp()
  {
    this.Stop(nameof (OnCleanUp));
    Components.Brains.Remove(this);
  }
}
