// Decompiled with JetBrains decompiler
// Type: RailModUploadScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using TMPro;
using UnityEngine;

#nullable disable
public class RailModUploadScreen : KModalScreen
{
  [SerializeField]
  private KButton[] closeButtons;
  [SerializeField]
  private KButton submitButton;
  [SerializeField]
  private ToolTip submitButtonTooltip;
  [SerializeField]
  private TMP_InputField modName;
  [SerializeField]
  private TMP_InputField modDesc;
  [SerializeField]
  private TMP_InputField modVersion;
  [SerializeField]
  private TMP_InputField contentFolder;
  [SerializeField]
  private TMP_InputField previewImage;
  [SerializeField]
  private MultiToggle[] shareTypeToggles;
  [Serialize]
  private string previousFolderPath;
}
