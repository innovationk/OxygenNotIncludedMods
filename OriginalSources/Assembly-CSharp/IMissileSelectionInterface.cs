// Decompiled with JetBrains decompiler
// Type: IMissileSelectionInterface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public interface IMissileSelectionInterface
{
  bool AmmunitionIsAllowed(Tag tag);

  bool IsAnyCosmicBlastShotAllowed();

  void ChangeAmmunition(Tag tag, bool allowed);

  void OnRowToggleClick();

  List<Tag> GetValidAmmunitionTags();
}
