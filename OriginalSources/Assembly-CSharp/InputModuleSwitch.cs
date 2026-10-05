// Decompiled with JetBrains decompiler
// Type: InputModuleSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class InputModuleSwitch : MonoBehaviour
{
  public VirtualInputModule virtualInput;
  public StandaloneInputModule standaloneInput;
  private Vector3 lastMousePosition;

  private void Update()
  {
    if (this.lastMousePosition != Input.mousePosition && KInputManager.currentControllerIsGamepad)
    {
      KInputManager.currentControllerIsGamepad = false;
      KInputManager.InputChange.Invoke();
    }
    if (KInputManager.currentControllerIsGamepad)
    {
      this.virtualInput.enabled = KInputManager.currentControllerIsGamepad;
      if (!this.standaloneInput.enabled)
        return;
      this.standaloneInput.enabled = false;
      this.ChangeInputHandler();
    }
    else
    {
      this.lastMousePosition = Input.mousePosition;
      this.standaloneInput.enabled = true;
      if (!this.virtualInput.enabled)
        return;
      this.virtualInput.enabled = false;
      this.ChangeInputHandler();
    }
  }

  private void ChangeInputHandler()
  {
    GameInputManager inputManager = Global.GetInputManager();
    for (int index = 0; index < inputManager.usedMenus.Count; ++index)
    {
      if (inputManager.usedMenus[index].Equals((object) null))
        inputManager.usedMenus.RemoveAt(index);
    }
    if (inputManager.GetControllerCount() <= 1)
      return;
    if (KInputManager.currentControllerIsGamepad)
    {
      Cursor.visible = false;
      inputManager.GetController(1).inputHandler.TransferHandles(inputManager.GetController(0).inputHandler);
    }
    else
    {
      Cursor.visible = true;
      inputManager.GetController(0).inputHandler.TransferHandles(inputManager.GetController(1).inputHandler);
    }
  }
}
