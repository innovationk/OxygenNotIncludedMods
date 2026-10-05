// Decompiled with JetBrains decompiler
// Type: ISidescreenButtonControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public interface ISidescreenButtonControl
{
  string SidescreenButtonText { get; }

  string SidescreenButtonTooltip { get; }

  void SetButtonTextOverride(ButtonMenuTextOverride textOverride);

  bool SidescreenEnabled();

  bool SidescreenButtonInteractable();

  void OnSidescreenButtonPressed();

  int HorizontalGroupID();

  int ButtonSideScreenSortOrder();

  string SidescreenTitle => (string) null;
}
