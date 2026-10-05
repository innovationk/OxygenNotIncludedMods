// Decompiled with JetBrains decompiler
// Type: AssignmentGroupController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

#nullable disable
public class AssignmentGroupController : KMonoBehaviour
{
  public bool generateGroupOnStart;
  [Serialize]
  private string _assignmentGroupID;
  [Serialize]
  private Ref<MinionAssignablesProxy>[] minionsInGroupAtLoad;

  public string AssignmentGroupID
  {
    get => this._assignmentGroupID;
    private set => this._assignmentGroupID = value;
  }

  protected override void OnPrefabInit() => base.OnPrefabInit();

  [OnDeserialized]
  protected void CreateOrRestoreGroupID()
  {
    if (string.IsNullOrEmpty(this.AssignmentGroupID))
      this.GenerateGroupID();
    else
      Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], this.gameObject.GetProperName());
  }

  public void SetGroupID(string id)
  {
    DebugUtil.DevAssert(!string.IsNullOrEmpty(id), $"Trying to set Assignment group on {this.gameObject.name} to null or empty.");
    if (string.IsNullOrEmpty(id))
      return;
    this.AssignmentGroupID = id;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RestoreGroupAssignees();
  }

  private void GenerateGroupID()
  {
    if (!this.generateGroupOnStart || !string.IsNullOrEmpty(this.AssignmentGroupID))
      return;
    this.SetGroupID($"{this.GetComponent<KPrefabID>().PrefabID().ToString()}_{this.GetComponent<KPrefabID>().InstanceID.ToString()}_assignmentGroup");
    Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], this.gameObject.GetProperName());
  }

  private void RestoreGroupAssignees()
  {
    if (!this.generateGroupOnStart)
      return;
    this.CreateOrRestoreGroupID();
    if (this.minionsInGroupAtLoad == null)
      this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[0];
    for (int index = 0; index < this.minionsInGroupAtLoad.Length; ++index)
      Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, (IAssignableIdentity) this.minionsInGroupAtLoad[index].Get());
    Ownable component = this.GetComponent<Ownable>();
    if (!((Object) component != (Object) null))
      return;
    component.Assign((IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID]);
    component.SetCanBeAssigned(false);
  }

  public bool CheckMinionIsMember(MinionAssignablesProxy minion)
  {
    if (string.IsNullOrEmpty(this.AssignmentGroupID))
      this.GenerateGroupID();
    return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].HasMember((IAssignableIdentity) minion);
  }

  public void SetMember(MinionAssignablesProxy minion, bool isAllowed)
  {
    Debug.Assert(DlcManager.IsExpansion1Active());
    if (!isAllowed)
    {
      Game.Instance.assignmentManager.RemoveFromAssignmentGroup(this.AssignmentGroupID, (IAssignableIdentity) minion);
    }
    else
    {
      if (this.CheckMinionIsMember(minion))
        return;
      Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, (IAssignableIdentity) minion);
    }
  }

  protected override void OnCleanUp()
  {
    if (this.generateGroupOnStart)
      Game.Instance.assignmentManager.RemoveAssignmentGroup(this.AssignmentGroupID);
    base.OnCleanUp();
  }

  [OnSerializing]
  private void OnSerialize()
  {
    Debug.Assert(!string.IsNullOrEmpty(this.AssignmentGroupID), (object) $"Assignment group on {this.gameObject.name} has null or empty ID");
    List<IAssignableIdentity> members = Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
    this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[members.Count];
    for (int index = 0; index < members.Count; ++index)
      this.minionsInGroupAtLoad[index] = new Ref<MinionAssignablesProxy>((MinionAssignablesProxy) members[index]);
  }

  public List<IAssignableIdentity> GetMembers()
  {
    if (string.IsNullOrEmpty(this.AssignmentGroupID))
      this.GenerateGroupID();
    return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
  }
}
