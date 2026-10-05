// Decompiled with JetBrains decompiler
// Type: SimpleUIShowHide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SimpleUIShowHide")]
public class SimpleUIShowHide : KMonoBehaviour
{
  [MyCmpReq]
  private MultiToggle toggle;
  [SerializeField]
  public GameObject content;
  [SerializeField]
  private string saveStatePreferenceKey;
  private const int onState = 0;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.toggle.onClick += new System.Action(this.OnClick);
    if (this.saveStatePreferenceKey.IsNullOrWhiteSpace() || KPlayerPrefs.GetInt(this.saveStatePreferenceKey, 1) == 1 || this.toggle.CurrentState != 0)
      return;
    this.OnClick();
  }

  private void OnClick()
  {
    this.toggle.NextState();
    this.content.SetActive(this.toggle.CurrentState == 0);
    if (this.saveStatePreferenceKey.IsNullOrWhiteSpace())
      return;
    KPlayerPrefs.SetInt(this.saveStatePreferenceKey, this.toggle.CurrentState == 0 ? 1 : 0);
  }
}
