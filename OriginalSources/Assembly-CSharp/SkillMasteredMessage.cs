// Decompiled with JetBrains decompiler
// Type: SkillMasteredMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;

#nullable disable
public class SkillMasteredMessage : Message
{
  [Serialize]
  private string minionName;

  public SkillMasteredMessage()
  {
  }

  public SkillMasteredMessage(MinionResume resume) => this.minionName = resume.GetProperName();

  public override string GetSound() => "AI_Notification_ResearchComplete";

  public override string GetMessageBody()
  {
    Debug.Assert(this.minionName != null);
    string str = string.Format((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.LINE, (object) this.minionName);
    return string.Format((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.MESSAGEBODY, (object) str);
  }

  public override string GetTitle()
  {
    return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.minionName);
  }

  public override string GetTooltip()
  {
    return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", this.minionName);
  }

  public override bool IsValid() => this.minionName != null;
}
