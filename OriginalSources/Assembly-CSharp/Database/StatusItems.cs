// Decompiled with JetBrains decompiler
// Type: Database.StatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

#nullable disable
namespace Database;

public class StatusItems(string id, ResourceSet parent) : ResourceSet<StatusItem>(id, parent)
{
  [DebuggerDisplay("{Id}")]
  public class StatusItemInfo : Resource
  {
    public string Type;
    public string Tooltip;
    public bool IsIconTinted;
    public StatusItem.IconType IconType;
    public string Icon;
    public string SoundPath;
    public bool ShouldNotify;
    public float NotificationDelay;
    public NotificationType NotificationType;
    public bool AllowMultiples;
    public string Effect;
    public HashedString Overlay;
    public HashedString SecondOverlay;
  }
}
