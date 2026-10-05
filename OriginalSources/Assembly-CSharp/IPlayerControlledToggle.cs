// Decompiled with JetBrains decompiler
// Type: IPlayerControlledToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public interface IPlayerControlledToggle
{
  void ToggledByPlayer();

  bool ToggledOn();

  KSelectable GetSelectable();

  string SideScreenTitleKey { get; }

  bool ToggleRequested { get; set; }
}
