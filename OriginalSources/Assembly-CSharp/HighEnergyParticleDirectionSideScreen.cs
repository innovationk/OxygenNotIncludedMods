// Decompiled with JetBrains decompiler
// Type: HighEnergyParticleDirectionSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HighEnergyParticleDirectionSideScreen : SideScreenContent
{
  private IHighEnergyParticleDirection target;
  public List<KButton> Buttons;
  private KButton activeButton;
  public LocText directionLabel;
  private string[] directionStrings = new string[8]
  {
    (string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_N,
    (string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NW,
    (string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_W,
    (string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SW,
    (string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_S,
    (string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SE,
    (string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_E,
    (string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NE
  };

  public override string GetTitle()
  {
    return (string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.TITLE;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    for (int index = 0; index < this.Buttons.Count; ++index)
    {
      KButton button = this.Buttons[index];
      button.onClick += (System.Action) (() =>
      {
        int num = this.Buttons.IndexOf(button);
        if ((UnityEngine.Object) this.activeButton != (UnityEngine.Object) null)
          this.activeButton.isInteractable = true;
        button.isInteractable = false;
        this.activeButton = button;
        if (this.target == null)
          return;
        this.target.Direction = EightDirectionUtil.AngleToDirection(num * 45);
        Game.Instance.ForceOverlayUpdate(true);
        this.Refresh();
      });
    }
  }

  public override int GetSideScreenSortOrder() => 10;

  public override bool IsValidForTarget(GameObject target)
  {
    HighEnergyParticleRedirector component = target.GetComponent<HighEnergyParticleRedirector>();
    bool flag1 = (UnityEngine.Object) component != (UnityEngine.Object) null;
    if (flag1)
      flag1 = flag1 && component.directionControllable;
    bool flag2 = (UnityEngine.Object) target.GetComponent<HighEnergyParticleSpawner>() != (UnityEngine.Object) null || (UnityEngine.Object) target.GetComponent<ManualHighEnergyParticleSpawner>() != (UnityEngine.Object) null || (UnityEngine.Object) target.GetComponent<DevHEPSpawner>() != (UnityEngine.Object) null;
    return flag1 | flag2 && target.GetComponent<IHighEnergyParticleDirection>() != null;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<IHighEnergyParticleDirection>();
      if (this.target == null)
        Debug.LogError((object) "The gameObject received does not contain IHighEnergyParticleDirection component");
      else
        this.Refresh();
    }
  }

  private void Refresh()
  {
    int directionIndex = EightDirectionUtil.GetDirectionIndex(this.target.Direction);
    if (directionIndex >= 0 && directionIndex < this.Buttons.Count)
    {
      this.Buttons[directionIndex].SignalClick(KKeyCode.Mouse0);
    }
    else
    {
      if ((bool) (UnityEngine.Object) this.activeButton)
        this.activeButton.isInteractable = true;
      this.activeButton = (KButton) null;
    }
    this.directionLabel.SetText(string.Format((string) UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.SELECTED_DIRECTION, (object) this.directionStrings[directionIndex]));
  }
}
