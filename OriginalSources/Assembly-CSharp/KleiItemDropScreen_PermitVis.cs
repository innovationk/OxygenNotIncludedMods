// Decompiled with JetBrains decompiler
// Type: KleiItemDropScreen_PermitVis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class KleiItemDropScreen_PermitVis : KMonoBehaviour
{
  [SerializeField]
  private RectTransform root;
  [Header("Different Permit Visualizers")]
  [SerializeField]
  private KleiItemDropScreen_PermitVis_Fallback fallbackVis;
  [SerializeField]
  private KleiItemDropScreen_PermitVis_DupeEquipment equipmentVis;

  public void ConfigureWith(DropScreenPresentationInfo info)
  {
    this.ResetState();
    this.equipmentVis.gameObject.SetActive(false);
    this.fallbackVis.gameObject.SetActive(false);
    if (info.UseEquipmentVis)
    {
      this.equipmentVis.gameObject.SetActive(true);
      this.equipmentVis.ConfigureWith(info);
    }
    else
    {
      this.fallbackVis.gameObject.SetActive(true);
      this.fallbackVis.ConfigureWith(info);
    }
  }

  public Promise AnimateIn() => Updater.RunRoutine((MonoBehaviour) this, this.AnimateInRoutine());

  public Promise AnimateOut() => Updater.RunRoutine((MonoBehaviour) this, this.AnimateOutRoutine());

  private IEnumerator AnimateInRoutine()
  {
    this.root.gameObject.SetActive(true);
    yield return (object) Updater.Ease((Action<Vector3>) (v3 => this.root.transform.localScale = v3), this.root.transform.localScale, Vector3.one, 0.5f, Easing.EaseOutBack);
  }

  private IEnumerator AnimateOutRoutine()
  {
    yield return (object) Updater.Ease((Action<Vector3>) (v3 => this.root.transform.localScale = v3), this.root.transform.localScale, Vector3.zero, 0.25f);
    this.root.gameObject.SetActive(true);
  }

  public void ResetState() => this.root.transform.localScale = Vector3.zero;
}
