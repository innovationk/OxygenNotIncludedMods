// Decompiled with JetBrains decompiler
// Type: IBuildingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public abstract class IBuildingConfig : IHasDlcRestrictions
{
  public abstract BuildingDef CreateBuildingDef();

  public virtual void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
  }

  public abstract void DoPostConfigureComplete(GameObject go);

  public virtual void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public virtual void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public virtual void ConfigurePost(BuildingDef def)
  {
  }

  [Obsolete("Implement GetRequiredDlcIds and/or GetForbiddenDlcIds instead")]
  public virtual string[] GetDlcIds() => (string[]) null;

  public virtual string[] GetRequiredDlcIds() => (string[]) null;

  public virtual string[] GetForbiddenDlcIds() => (string[]) null;

  public virtual bool ForbidFromLoading() => false;
}
