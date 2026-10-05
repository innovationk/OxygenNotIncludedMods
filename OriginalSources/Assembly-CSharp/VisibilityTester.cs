// Decompiled with JetBrains decompiler
// Type: VisibilityTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/VisibilityTester")]
public class VisibilityTester : KMonoBehaviour
{
  public static VisibilityTester Instance;
  public bool enableTesting;

  public static void DestroyInstance() => VisibilityTester.Instance = (VisibilityTester) null;

  protected override void OnPrefabInit() => VisibilityTester.Instance = this;

  private void Update()
  {
    if ((Object) SelectTool.Instance == (Object) null || (Object) SelectTool.Instance.selected == (Object) null || !this.enableTesting)
      return;
    int cell = Grid.PosToCell((KMonoBehaviour) SelectTool.Instance.selected);
    int mouseCell = DebugHandler.GetMouseCell();
    string text = $"{$"{$"Source Cell: {cell.ToString()}\n"}Target Cell: {mouseCell.ToString()}\n"}Visible: {Grid.VisibilityTest(cell, mouseCell).ToString()}";
    for (int index = 0; index < 10000; ++index)
      Grid.VisibilityTest(cell, mouseCell);
    DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
  }
}
