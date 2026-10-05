// Decompiled with JetBrains decompiler
// Type: FossilBits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;

#nullable disable
public class FossilBits : FossilExcavationWorkable, ISidescreenButtonControl
{
  [Serialize]
  public bool MarkedForDig;
  private Chore chore;
  [MyCmpGet]
  private EntombVulnerable entombComponent;
  [MyCmpGet]
  private Operational operational;

  protected override bool IsMarkedForExcavation() => this.MarkedForDig;

  public void SetEntombStatusItemVisibility(bool visible)
  {
    this.entombComponent.SetShowStatusItemOnEntombed(visible);
  }

  public void CreateWorkableChore()
  {
    if (this.chore != null || !this.operational.IsOperational)
      return;
    this.chore = (Chore) new WorkChore<FossilBits>(Db.Get().ChoreTypes.ExcavateFossil, (IStateMachineTarget) this, only_when_operational: false);
  }

  public void CancelWorkChore()
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("FossilBits.CancelChore");
    this.chore = (Chore) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_sculpture_kanim")
    };
    this.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
    this.SetWorkTime(30f);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetEntombStatusItemVisibility(this.MarkedForDig);
    this.SetShouldShowSkillPerkStatusItem(this.IsMarkedForExcavation());
    if (!this.MarkedForDig)
      return;
    this.CreateWorkableChore();
  }

  private void OnOperationalChanged(object state)
  {
    if (((Boxed<bool>) state).value)
    {
      if (!this.MarkedForDig)
        return;
      this.CreateWorkableChore();
    }
    else
    {
      if (!this.MarkedForDig)
        return;
      this.CancelWorkChore();
    }
  }

  private void DropLoot()
  {
    PrimaryElement component = this.gameObject.GetComponent<PrimaryElement>();
    int cell = Grid.PosToCell(this.transform.GetPosition());
    Element element = ElementLoader.GetElement(component.Element.tag);
    if (element == null)
      return;
    float mass1 = component.Mass;
    for (int index = 0; (double) index < (double) component.Mass / 400.0; ++index)
    {
      float mass2 = mass1;
      if ((double) mass1 > 400.0)
      {
        mass2 = 400f;
        mass1 -= 400f;
      }
      int disease_count = (int) ((double) component.DiseaseCount * ((double) mass2 / (double) component.Mass));
      element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), mass2, component.Temperature, component.DiseaseIdx, disease_count);
    }
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    this.DropLoot();
    Util.KDestroyGameObject(this.gameObject);
  }

  public int HorizontalGroupID() => -1;

  public string SidescreenButtonText
  {
    get
    {
      return !this.MarkedForDig ? (string) CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON : (string) CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON;
    }
  }

  public string SidescreenButtonTooltip
  {
    get
    {
      return !this.MarkedForDig ? (string) CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON_TOOLTIP : (string) CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
    }
  }

  public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
  {
    throw new NotImplementedException();
  }

  public bool SidescreenEnabled() => true;

  public bool SidescreenButtonInteractable() => true;

  public void OnSidescreenButtonPressed()
  {
    this.MarkedForDig = !this.MarkedForDig;
    this.SetShouldShowSkillPerkStatusItem(this.MarkedForDig);
    this.SetEntombStatusItemVisibility(this.MarkedForDig);
    if (this.MarkedForDig)
      this.CreateWorkableChore();
    else
      this.CancelWorkChore();
    this.UpdateStatusItem((object) null);
  }

  public int ButtonSideScreenSortOrder() => 20;
}
