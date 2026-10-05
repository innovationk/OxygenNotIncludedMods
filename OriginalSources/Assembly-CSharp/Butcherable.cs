// Decompiled with JetBrains decompiler
// Type: Butcherable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Butcherable")]
public class Butcherable : Workable, ISaveLoadable
{
  [MyCmpGet]
  private KAnimControllerBase controller;
  [MyCmpGet]
  private Harvestable harvestable;
  private bool readyToButcher;
  private bool butchered;
  public Dictionary<string, float> drops;
  private Chore chore;
  private static readonly EventSystem.IntraObjectHandler<Butcherable> SetReadyToButcherDelegate = new EventSystem.IntraObjectHandler<Butcherable>((Action<Butcherable, object>) ((component, data) => component.SetReadyToButcher(data)));
  private static readonly EventSystem.IntraObjectHandler<Butcherable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Butcherable>((Action<Butcherable, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  public void SetDrops(string[] drops)
  {
    Dictionary<string, float> drops1 = new Dictionary<string, float>();
    for (int index = 0; index < drops.Length; ++index)
    {
      if (!drops1.ContainsKey(drops[index]))
        drops1.Add(drops[index], 0.0f);
      ++drops1[drops[index]];
    }
    this.SetDrops(drops1);
  }

  public void SetDrops(Dictionary<string, float> drops) => this.drops = drops;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Butcherable>(1272413801, Butcherable.SetReadyToButcherDelegate);
    this.Subscribe<Butcherable>(493375141, Butcherable.OnRefreshUserMenuDelegate);
    this.workTime = 3f;
    this.multitoolContext = (HashedString) "harvest";
    this.multitoolHitEffectTag = (Tag) "fx_harvest_splash";
  }

  public void SetReadyToButcher(object param) => this.readyToButcher = true;

  public void SetReadyToButcher(bool ready) => this.readyToButcher = ready;

  public void ActivateChore(object param)
  {
    if (this.chore != null)
      return;
    this.chore = (Chore) new WorkChore<Butcherable>(Db.Get().ChoreTypes.Harvest, (IStateMachineTarget) this);
    this.OnRefreshUserMenu((object) null);
  }

  public void CancelChore(object param)
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("User cancelled");
    this.chore = (Chore) null;
  }

  private void OnClickCancel() => this.CancelChore((object) null);

  private void OnClickButcher()
  {
    if (DebugHandler.InstantBuildMode)
      this.OnButcherComplete();
    else
      this.ActivateChore((object) null);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.readyToButcher)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.chore != null ? new KIconButtonMenu.ButtonInfo("action_harvest", "Cancel Meatify", new System.Action(this.OnClickCancel)) : new KIconButtonMenu.ButtonInfo("action_harvest", "Meatify", new System.Action(this.OnClickButcher)));
  }

  protected override void OnCompleteWork(WorkerBase worker) => this.OnButcherComplete();

  public GameObject[] CreateDrops(float multiplier = 1f)
  {
    GameObject[] drops = new GameObject[this.drops.Count];
    int index = 0;
    float temperature = this.GetComponent<PrimaryElement>().Temperature;
    foreach (KeyValuePair<string, float> drop in this.drops)
    {
      GameObject go = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, drop.Key);
      go.SetActive(true);
      PrimaryElement component1 = go.GetComponent<PrimaryElement>();
      component1.Mass = component1.Mass * multiplier * drop.Value;
      component1.Temperature = temperature;
      Edible component2 = go.GetComponent<Edible>();
      if ((bool) (UnityEngine.Object) component2)
        ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component2.Calories, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", go.GetProperName()), (string) UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
      drops[index] = go;
      ++index;
    }
    return drops;
  }

  public void OnButcherComplete()
  {
    if (this.butchered)
      return;
    KSelectable component1 = this.GetComponent<KSelectable>();
    if ((bool) (UnityEngine.Object) component1 && component1.IsSelected)
      SelectTool.Instance.Select((KSelectable) null);
    Pickupable component2 = this.GetComponent<Pickupable>();
    Storage storage = (UnityEngine.Object) component2 != (UnityEngine.Object) null ? component2.storage : (Storage) null;
    GameObject[] drops = this.CreateDrops();
    if (drops != null)
    {
      for (int index = 0; index < drops.Length; ++index)
      {
        if ((UnityEngine.Object) storage != (UnityEngine.Object) null && storage.storeDropsFromButcherables)
          storage.Store(drops[index]);
      }
    }
    this.chore = (Chore) null;
    this.butchered = true;
    this.readyToButcher = false;
    Game.Instance.userMenu.Refresh(this.gameObject);
    this.Trigger(395373363, (object) drops);
  }

  private int GetDropSpawnLocation()
  {
    int cell = Grid.PosToCell(this.gameObject);
    int num = Grid.CellAbove(cell);
    return Grid.IsValidCell(num) && !Grid.Solid[num] ? num : cell;
  }
}
