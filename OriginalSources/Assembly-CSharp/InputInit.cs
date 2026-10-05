// Decompiled with JetBrains decompiler
// Type: InputInit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
internal class InputInit : MonoBehaviour
{
  private void Awake()
  {
    GameInputManager inputManager = Global.GetInputManager();
    for (int controller_index = 0; controller_index < inputManager.GetControllerCount(); ++controller_index)
    {
      KInputController controller = inputManager.GetController(controller_index);
      if (controller.IsGamepad)
      {
        foreach (Component component in this.gameObject.GetComponents<Component>())
        {
          if (component is IInputHandler child)
          {
            KInputHandler.Add((IInputHandler) controller, child);
            inputManager.usedMenus.Add(child);
          }
        }
      }
    }
    if (KInputManager.currentController != null)
      KInputHandler.Add((IInputHandler) KInputManager.currentController, (IInputHandler) KScreenManager.Instance, 10);
    else
      KInputHandler.Add((IInputHandler) inputManager.GetDefaultController(), (IInputHandler) KScreenManager.Instance, 10);
    inputManager.usedMenus.Add((IInputHandler) KScreenManager.Instance);
    DebugHandler child1 = new DebugHandler();
    if (KInputManager.currentController != null)
      KInputHandler.Add((IInputHandler) KInputManager.currentController, (IInputHandler) child1, -1);
    else
      KInputHandler.Add((IInputHandler) inputManager.GetDefaultController(), (IInputHandler) child1, -1);
    inputManager.usedMenus.Add((IInputHandler) child1);
  }
}
