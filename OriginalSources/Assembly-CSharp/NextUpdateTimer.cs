// Decompiled with JetBrains decompiler
// Type: NextUpdateTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/NextUpdateTimer")]
public class NextUpdateTimer : KMonoBehaviour
{
  public LocText TimerText;
  public KBatchedAnimController UpdateAnimController;
  public KBatchedAnimController UpdateAnimMeterController;
  public float initialAnimScale;
  public System.DateTime nextReleaseDate;
  public System.DateTime currentReleaseDate;
  private string m_releaseTextOverride;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.initialAnimScale = this.UpdateAnimController.animScale;
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RefreshReleaseTimes();
  }

  public void UpdateReleaseTimes(string lastUpdateTime, string nextUpdateTime, string textOverride)
  {
    if (!System.DateTime.TryParse(lastUpdateTime, out this.currentReleaseDate))
      Debug.LogWarning((object) ("Failed to parse last_update_time: " + lastUpdateTime));
    if (!System.DateTime.TryParse(nextUpdateTime, out this.nextReleaseDate))
      Debug.LogWarning((object) ("Failed to parse next_update_time: " + nextUpdateTime));
    this.m_releaseTextOverride = textOverride;
    this.RefreshReleaseTimes();
  }

  private void RefreshReleaseTimes()
  {
    TimeSpan timeSpan1 = this.nextReleaseDate - this.currentReleaseDate;
    TimeSpan timeSpan2 = this.nextReleaseDate - System.DateTime.UtcNow;
    TimeSpan timeSpan3 = System.DateTime.UtcNow - this.currentReleaseDate;
    string anim_name = "4";
    string str;
    if (!string.IsNullOrEmpty(this.m_releaseTextOverride))
      str = this.m_releaseTextOverride;
    else if (timeSpan2.TotalHours < 8.0)
    {
      str = (string) UI.DEVELOPMENTBUILDS.UPDATES.TWENTY_FOUR_HOURS;
      anim_name = "4";
    }
    else if (timeSpan2.TotalDays < 1.0)
    {
      str = string.Format((string) UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, (object) 1);
      anim_name = "3";
    }
    else
    {
      int num1 = timeSpan2.Days % 7;
      int num2 = (timeSpan2.Days - num1) / 7;
      if (num2 <= 0)
      {
        str = string.Format((string) UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, (object) num1);
        anim_name = "2";
      }
      else
      {
        str = string.Format((string) UI.DEVELOPMENTBUILDS.UPDATES.BIGGER_TIMES, (object) num1, (object) num2);
        anim_name = "1";
      }
    }
    this.TimerText.text = str;
    this.UpdateAnimController.Play((HashedString) anim_name, KAnim.PlayMode.Loop);
    this.UpdateAnimMeterController.SetPositionPercent(Mathf.Clamp01((float) (timeSpan3.TotalSeconds / timeSpan1.TotalSeconds)));
  }
}
