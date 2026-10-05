// Decompiled with JetBrains decompiler
// Type: DescriptorPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/DescriptorPanel")]
public class DescriptorPanel : KMonoBehaviour
{
  [SerializeField]
  private GameObject customLabelPrefab;
  private List<GameObject> labels = new List<GameObject>();

  public bool HasDescriptors() => this.labels.Count > 0;

  public void SetDescriptors(IList<Descriptor> descriptors)
  {
    int index;
    for (index = 0; index < descriptors.Count; ++index)
    {
      GameObject gameObject;
      if (index >= this.labels.Count)
      {
        gameObject = Util.KInstantiate((Object) this.customLabelPrefab != (Object) null ? this.customLabelPrefab : ScreenPrefabs.Instance.DescriptionLabel, this.gameObject);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        this.labels.Add(gameObject);
      }
      else
        gameObject = this.labels[index];
      gameObject.GetComponent<LocText>().text = descriptors[index].IndentedText();
      gameObject.GetComponent<ToolTip>().toolTip = descriptors[index].tooltipText;
      gameObject.SetActive(true);
    }
    for (; index < this.labels.Count; ++index)
      this.labels[index].SetActive(false);
  }
}
