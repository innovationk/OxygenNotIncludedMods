// Decompiled with JetBrains decompiler
// Type: CarePackageInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CarePackageInfo : ITelepadDeliverable
{
  public readonly string id;
  public readonly float quantity;
  public readonly Func<bool> requirement;
  public readonly string facadeID;

  public CarePackageInfo(string ID, float amount, Func<bool> requirement)
  {
    this.id = ID;
    this.quantity = amount;
    this.requirement = requirement;
  }

  public CarePackageInfo(string ID, float amount, Func<bool> requirement, string facadeID)
  {
    this.id = ID;
    this.quantity = amount;
    this.requirement = requirement;
    this.facadeID = facadeID;
  }

  public GameObject Deliver(Vector3 location)
  {
    location += Vector3.right / 2f;
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) CarePackageConfig.ID), location);
    gameObject.SetActive(true);
    gameObject.GetComponent<CarePackage>().SetInfo(this);
    return gameObject;
  }
}
