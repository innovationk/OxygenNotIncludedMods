// Decompiled with JetBrains decompiler
// Type: MaterialsStatusItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class MaterialsStatusItem(
  string id,
  string prefix,
  string icon,
  StatusItem.IconType icon_type,
  NotificationType notification_type,
  bool allow_multiples,
  HashedString overlay) : StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, overlay)
{
}
