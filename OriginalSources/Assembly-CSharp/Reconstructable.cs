// Decompiled with JetBrains decompiler
// Type: Reconstructable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Reconstructable : KMonoBehaviour
{
  [MyCmpReq]
  private Deconstructable deconstructable;
  [MyCmpReq]
  private Building building;
  [Serialize]
  private Tag[] selectedElementsTags;
  [Serialize]
  private bool reconstructRequested;

  public bool AllowReconstruct
  {
    get
    {
      if (!this.deconstructable.allowDeconstruction)
        return false;
      return this.building.Def.ShowInBuildMenu || SelectModuleSideScreen.moduleButtonSortOrder.Contains(this.building.Def.PrefabID);
    }
  }

  public Tag PrimarySelectedElementTag => this.selectedElementsTags[0];

  public bool ReconstructRequested => this.reconstructRequested;

  protected override void OnSpawn() => base.OnSpawn();

  public void RequestReconstruct(Tag newElement)
  {
    if (!this.deconstructable.allowDeconstruction)
      return;
    this.reconstructRequested = !this.reconstructRequested;
    if (this.reconstructRequested)
    {
      this.deconstructable.QueueDeconstruction(false);
      this.selectedElementsTags = new Tag[1]{ newElement };
    }
    else
      this.deconstructable.CancelDeconstruction();
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  public void CancelReconstructOrder()
  {
    this.reconstructRequested = false;
    this.deconstructable.CancelDeconstruction();
    this.Trigger(954267658, (object) null);
  }

  public void TryCommenceReconstruct()
  {
    if (!this.deconstructable.allowDeconstruction || !this.reconstructRequested)
      return;
    string facadeID = this.building.GetComponent<BuildingFacade>().CurrentFacade;
    Vector3 position = this.building.transform.position;
    Orientation orientation = this.building.Orientation;
    GameScheduler.Instance.ScheduleNextFrame("Reconstruct", (Action<object>) (data => this.building.Def.TryPlace((GameObject) null, position, orientation, (IList<Tag>) this.selectedElementsTags, facadeID, false)));
  }
}
