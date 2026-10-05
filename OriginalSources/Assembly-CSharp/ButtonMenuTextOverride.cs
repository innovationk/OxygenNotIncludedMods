// Decompiled with JetBrains decompiler
// Type: ButtonMenuTextOverride
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public struct ButtonMenuTextOverride
{
  public LocString Text;
  public LocString CancelText;
  public LocString ToolTip;
  public LocString CancelToolTip;

  public bool IsValid
  {
    get
    {
      return !string.IsNullOrEmpty((string) this.Text) && !string.IsNullOrEmpty((string) this.ToolTip);
    }
  }

  public bool HasCancelText
  {
    get
    {
      return !string.IsNullOrEmpty((string) this.CancelText) && !string.IsNullOrEmpty((string) this.CancelToolTip);
    }
  }
}
