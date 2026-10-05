// Decompiled with JetBrains decompiler
// Type: CopyTextFieldToClipboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/CopyTextFieldToClipboard")]
public class CopyTextFieldToClipboard : KMonoBehaviour
{
  public KButton button;
  public Func<string> GetText;

  protected override void OnPrefabInit() => this.button.onClick += new System.Action(this.OnClick);

  private void OnClick()
  {
    TextEditor textEditor = new TextEditor();
    textEditor.text = this.GetText();
    textEditor.SelectAll();
    textEditor.Copy();
  }
}
