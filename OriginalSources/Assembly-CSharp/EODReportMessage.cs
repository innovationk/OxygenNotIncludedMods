// Decompiled with JetBrains decompiler
// Type: EODReportMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
public class EODReportMessage : Message
{
  [Serialize]
  private int day;
  [Serialize]
  private string title;
  [Serialize]
  private string tooltip;

  public EODReportMessage(string title, string tooltip)
  {
    this.day = GameUtil.GetCurrentCycle();
    this.title = title;
    this.tooltip = tooltip;
  }

  public EODReportMessage()
  {
  }

  public override string GetSound() => (string) null;

  public override string GetMessageBody() => "";

  public override string GetTooltip() => this.tooltip;

  public override string GetTitle() => this.title;

  public void OpenReport() => ManagementMenu.Instance.OpenReports(this.day);

  public override bool ShowDialog() => false;

  public override void OnClick() => this.OpenReport();
}
