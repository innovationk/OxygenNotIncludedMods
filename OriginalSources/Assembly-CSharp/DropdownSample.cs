// Decompiled with JetBrains decompiler
// Type: DropdownSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class DropdownSample : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI text;
  [SerializeField]
  private TMP_Dropdown dropdownWithoutPlaceholder;
  [SerializeField]
  private TMP_Dropdown dropdownWithPlaceholder;

  public void OnButtonClick()
  {
    this.text.text = this.dropdownWithPlaceholder.value > -1 ? $"Selected values:\n{this.dropdownWithoutPlaceholder.value.ToString()} - {this.dropdownWithPlaceholder.value.ToString()}" : "Error: Please make a selection";
  }
}
