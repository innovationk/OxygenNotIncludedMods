// Decompiled with JetBrains decompiler
// Type: MinionStorageDataHolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class MinionStorageDataHolder : KMonoBehaviour, StoredMinionIdentity.IStoredMinionExtension
{
  public Action<StoredMinionIdentity> OnCopyBegins;
  [Serialize]
  private List<MinionStorageDataHolder.DataPack> storedDataPacks;

  protected override void OnSpawn() => base.OnSpawn();

  public MinionStorageDataHolder.DataPack Internal_GetDataPack(string ID)
  {
    if (this.storedDataPacks != null)
    {
      MinionStorageDataHolder.DataPack dataPack = this.storedDataPacks.Find((Predicate<MinionStorageDataHolder.DataPack>) (d => d.ID == ID));
      if (dataPack != null)
        return dataPack;
    }
    return (MinionStorageDataHolder.DataPack) null;
  }

  public void Internal_UpdateData(string ID, MinionStorageDataHolder.DataPackData data)
  {
    this.SetData(ID, data, false);
  }

  private void SetData(
    string ID,
    MinionStorageDataHolder.DataPackData data,
    bool markAsNewDataToRead)
  {
    if (this.storedDataPacks == null)
      this.storedDataPacks = new List<MinionStorageDataHolder.DataPack>();
    MinionStorageDataHolder.DataPack dataPack = this.storedDataPacks.Find((Predicate<MinionStorageDataHolder.DataPack>) (d => d.ID == ID));
    if (dataPack == null)
    {
      dataPack = new MinionStorageDataHolder.DataPack(ID);
      this.storedDataPacks.Add(dataPack);
    }
    dataPack.SetData(data, markAsNewDataToRead);
  }

  public void PullFrom(StoredMinionIdentity source)
  {
    MinionStorageDataHolder component = source.GetComponent<MinionStorageDataHolder>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.storedDataPacks == null)
      return;
    for (int index = 0; index < component.storedDataPacks.Count; ++index)
    {
      MinionStorageDataHolder.DataPack storedDataPack = component.storedDataPacks[index];
      if (storedDataPack != null)
        this.SetData(storedDataPack.ID, storedDataPack.ReadData(), true);
    }
  }

  public void PushTo(StoredMinionIdentity destination)
  {
    Action<StoredMinionIdentity> onCopyBegins = this.OnCopyBegins;
    if (onCopyBegins != null)
      onCopyBegins(destination);
    this.AddStoredMinionGameObjectRequirements(destination.gameObject);
    MinionStorageDataHolder component = destination.gameObject.GetComponent<MinionStorageDataHolder>();
    if (this.storedDataPacks == null)
      return;
    for (int index = 0; index < this.storedDataPacks.Count; ++index)
    {
      MinionStorageDataHolder.DataPack storedDataPack = this.storedDataPacks[index];
      if (storedDataPack != null)
        component.SetData(storedDataPack.ID, storedDataPack.ReadData(), true);
    }
  }

  public void AddStoredMinionGameObjectRequirements(GameObject storedMinionGameObject)
  {
    storedMinionGameObject.AddOrGet<MinionStorageDataHolder>();
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class DataPackData
  {
    [Serialize]
    public bool[] Bools;
    [Serialize]
    public Tag[] Tags;
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class DataPack
  {
    [Serialize]
    private string id;
    [Serialize]
    private bool isStoringNewData;
    [Serialize]
    private MinionStorageDataHolder.DataPackData data;

    public bool IsStoringNewData => this.isStoringNewData;

    public string ID => this.id;

    public DataPack(string id) => this.id = id;

    public void SetData(MinionStorageDataHolder.DataPackData data, bool markAsNewDataToRead)
    {
      this.data = data;
      if (!markAsNewDataToRead)
        return;
      this.isStoringNewData = markAsNewDataToRead;
    }

    public MinionStorageDataHolder.DataPackData ReadData()
    {
      this.isStoringNewData = false;
      return this.data;
    }

    public MinionStorageDataHolder.DataPackData PeekData() => this.data;
  }
}
