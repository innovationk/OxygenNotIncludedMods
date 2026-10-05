// Decompiled with JetBrains decompiler
// Type: IEmptyableCargo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public interface IEmptyableCargo
{
  bool CanEmptyCargo();

  void EmptyCargo();

  IStateMachineTarget master { get; }

  bool CanAutoDeploy { get; }

  bool AutoDeploy { get; set; }

  bool ChooseDuplicant { get; }

  bool ModuleDeployed { get; }

  MinionIdentity ChosenDuplicant { get; set; }

  bool CanTargetClusterGridEntities => false;

  string GetButtonText => (string) UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.DEPLOY_BUTTON;

  string GetButtonToolip
  {
    get => (string) UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.DEPLOY_BUTTON_TOOLTIP;
  }
}
