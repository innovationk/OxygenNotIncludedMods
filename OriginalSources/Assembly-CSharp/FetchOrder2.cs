// Decompiled with JetBrains decompiler
// Type: FetchOrder2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FetchOrder2
{
  public Action<FetchOrder2, Pickupable> OnComplete;
  public Action<FetchOrder2, Pickupable> OnBegin;
  public bool validateRequiredTagOnTagChange;
  public List<FetchChore> Chores = new List<FetchChore>();
  private ChoreType choreType;
  private float _UnfetchedAmount;
  private bool checkStorageContents;
  private Operational.State operationalRequirement = Operational.State.None;

  public float TotalAmount { get; set; }

  public int PriorityMod { get; set; }

  public HashSet<Tag> Tags { get; protected set; }

  public FetchChore.MatchCriteria Criteria { get; protected set; }

  public Tag RequiredTag { get; protected set; }

  public Tag[] ForbiddenTags { get; protected set; }

  public Storage Destination { get; set; }

  private float UnfetchedAmount
  {
    get => this._UnfetchedAmount;
    set
    {
      this._UnfetchedAmount = value;
      this.Assert((double) this._UnfetchedAmount <= (double) this.TotalAmount, "_UnfetchedAmount <= TotalAmount");
      this.Assert((double) this._UnfetchedAmount >= 0.0, "_UnfetchedAmount >= 0");
    }
  }

  public FetchOrder2(
    ChoreType chore_type,
    HashSet<Tag> tags,
    FetchChore.MatchCriteria criteria,
    Tag required_tag,
    Tag[] forbidden_tags,
    Storage destination,
    float amount,
    Operational.State operationalRequirementDEPRECATED = Operational.State.None,
    int priorityMod = 0)
  {
    if ((double) amount <= (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
      DebugUtil.LogWarningArgs((object) $"FetchOrder2 {chore_type.Id} is requesting {tags} {amount} to {((UnityEngine.Object) destination != (UnityEngine.Object) null ? (object) destination.name : (object) "to nowhere")}");
    this.choreType = chore_type;
    this.Tags = tags;
    this.Criteria = criteria;
    this.RequiredTag = required_tag;
    this.ForbiddenTags = forbidden_tags;
    this.Destination = destination;
    this.TotalAmount = amount;
    this.UnfetchedAmount = amount;
    this.PriorityMod = priorityMod;
    this.operationalRequirement = operationalRequirementDEPRECATED;
  }

  public bool InProgress
  {
    get
    {
      bool inProgress = false;
      foreach (Chore chore in this.Chores)
      {
        if (chore.InProgress())
        {
          inProgress = true;
          break;
        }
      }
      return inProgress;
    }
  }

  private void IssueTask()
  {
    if ((double) this.UnfetchedAmount <= 0.0)
      return;
    this.SetFetchTask(this.UnfetchedAmount);
    this.UnfetchedAmount = 0.0f;
  }

  public void SetPriorityMod(int priorityMod)
  {
    this.PriorityMod = priorityMod;
    for (int index = 0; index < this.Chores.Count; ++index)
      this.Chores[index].SetPriorityMod(this.PriorityMod);
  }

  private void SetFetchTask(float amount)
  {
    this.Chores.Add(new FetchChore(this.choreType, this.Destination, amount, this.Tags, this.Criteria, this.RequiredTag, this.ForbiddenTags, on_complete: new Action<Chore>(this.OnFetchChoreComplete), on_begin: new Action<Chore>(this.OnFetchChoreBegin), on_end: new Action<Chore>(this.OnFetchChoreEnd), operational_requirement: this.operationalRequirement, priority_mod: this.PriorityMod)
    {
      validateRequiredTagOnTagChange = this.validateRequiredTagOnTagChange
    });
  }

  private void OnFetchChoreEnd(Chore chore)
  {
    FetchChore fetchChore = (FetchChore) chore;
    if (!this.Chores.Contains(fetchChore))
      return;
    this.UnfetchedAmount += fetchChore.amount;
    fetchChore.Cancel("FetchChore Redistribution");
    this.Chores.Remove(fetchChore);
    this.IssueTask();
  }

  private void OnFetchChoreComplete(Chore chore)
  {
    FetchChore fetchChore = (FetchChore) chore;
    this.Chores.Remove(fetchChore);
    if (this.Chores.Count != 0 || this.OnComplete == null)
      return;
    this.OnComplete(this, fetchChore.fetchTarget);
  }

  private void OnFetchChoreBegin(Chore chore)
  {
    FetchChore fetchChore = (FetchChore) chore;
    this.UnfetchedAmount += fetchChore.originalAmount - fetchChore.amount;
    this.IssueTask();
    if (this.OnBegin == null)
      return;
    this.OnBegin(this, fetchChore.fetchTarget);
  }

  public void Cancel(string reason)
  {
    while (this.Chores.Count > 0)
    {
      FetchChore chore = this.Chores[0];
      chore.Cancel(reason);
      this.Chores.Remove(chore);
    }
  }

  public void Suspend(string reason) => Debug.LogError((object) "UNIMPLEMENTED!");

  public void Resume(string reason) => Debug.LogError((object) "UNIMPLEMENTED!");

  public void Submit(
    Action<FetchOrder2, Pickupable> on_complete,
    bool check_storage_contents,
    Action<FetchOrder2, Pickupable> on_begin = null)
  {
    this.OnComplete = on_complete;
    this.OnBegin = on_begin;
    this.checkStorageContents = check_storage_contents;
    if (check_storage_contents)
    {
      Pickupable out_item = (Pickupable) null;
      this.UnfetchedAmount = this.GetRemaining(out out_item);
      if ((double) this.UnfetchedAmount <= (double) this.Destination.storageFullMargin)
      {
        if (this.OnComplete == null)
          return;
        this.OnComplete(this, out_item);
      }
      else
        this.IssueTask();
    }
    else
      this.IssueTask();
  }

  public bool IsMaterialOnStorage(Storage storage, ref float amount, ref Pickupable out_item)
  {
    foreach (GameObject gameObject in this.Destination.items)
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        Pickupable component = gameObject.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          KPrefabID kprefabId = component.KPrefabID;
          foreach (Tag tag in this.Tags)
          {
            if (kprefabId.HasTag(tag))
            {
              amount = component.FetchTotalAmount;
              out_item = component;
              return true;
            }
          }
        }
      }
    }
    return false;
  }

  public float AmountWaitingToFetch()
  {
    if (this.checkStorageContents)
      return this.GetRemaining(out Pickupable _);
    float unfetchedAmount = this.UnfetchedAmount;
    for (int index = 0; index < this.Chores.Count; ++index)
      unfetchedAmount += this.Chores[index].AmountWaitingToFetch();
    return unfetchedAmount;
  }

  public float GetRemaining(out Pickupable out_item)
  {
    float remaining = this.TotalAmount;
    float amount = 0.0f;
    out_item = (Pickupable) null;
    if (this.IsMaterialOnStorage(this.Destination, ref amount, ref out_item))
      remaining = Math.Max(remaining - amount, 0.0f);
    return remaining;
  }

  public bool IsComplete()
  {
    for (int index = 0; index < this.Chores.Count; ++index)
    {
      if (!this.Chores[index].isComplete)
        return false;
    }
    return true;
  }

  private void Assert(bool condition, string message)
  {
    if (condition)
      return;
    string str = "FetchOrder error: " + message;
    Debug.LogError((object) $"{$"{(!((UnityEngine.Object) this.Destination == (UnityEngine.Object) null) ? $"{str}\nDestination: {this.Destination.name}" : str + "\nDestination: None")}\nTotal Amount: {this.TotalAmount.ToString()}"}\nUnfetched Amount: {this._UnfetchedAmount.ToString()}");
  }
}
