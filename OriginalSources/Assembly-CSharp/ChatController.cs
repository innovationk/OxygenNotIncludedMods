// Decompiled with JetBrains decompiler
// Type: ChatController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ChatController : MonoBehaviour
{
  public TMP_InputField ChatInputField;
  public TMP_Text ChatDisplayOutput;
  public Scrollbar ChatScrollbar;

  private void OnEnable()
  {
    this.ChatInputField.onSubmit.AddListener(new UnityAction<string>(this.AddToChatOutput));
  }

  private void OnDisable()
  {
    this.ChatInputField.onSubmit.RemoveListener(new UnityAction<string>(this.AddToChatOutput));
  }

  private void AddToChatOutput(string newText)
  {
    this.ChatInputField.text = string.Empty;
    System.DateTime now = System.DateTime.Now;
    string str = $"[<#FFFF80>{now.Hour.ToString("d2")}:{now.Minute.ToString("d2")}:{now.Second.ToString("d2")}</color>] {newText}";
    if ((UnityEngine.Object) this.ChatDisplayOutput != (UnityEngine.Object) null)
    {
      if (this.ChatDisplayOutput.text == string.Empty)
      {
        this.ChatDisplayOutput.text = str;
      }
      else
      {
        TMP_Text chatDisplayOutput = this.ChatDisplayOutput;
        chatDisplayOutput.text = $"{chatDisplayOutput.text}\n{str}";
      }
    }
    this.ChatInputField.ActivateInputField();
    this.ChatScrollbar.value = 0.0f;
  }
}
