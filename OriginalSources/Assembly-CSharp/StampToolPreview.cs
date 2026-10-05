// Decompiled with JetBrains decompiler
// Type: StampToolPreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class StampToolPreview
{
  private IStampToolPreviewPlugin[] plugins;
  private StampToolPreviewContext context;
  private int prevOriginCell;

  public StampToolPreview(InterfaceTool tool, params IStampToolPreviewPlugin[] plugins)
  {
    this.context = new StampToolPreviewContext();
    this.context.previewParent = new GameObject("StampToolPreview::Preview").transform;
    this.context.tool = tool;
    this.plugins = plugins;
  }

  public IEnumerator Setup(TemplateContainer stampTemplate)
  {
    this.Cleanup();
    this.context.stampTemplate = stampTemplate;
    if (this.plugins != null)
    {
      foreach (IStampToolPreviewPlugin plugin in this.plugins)
        plugin.Setup(this.context);
    }
    yield return (object) null;
    if (this.context.frameAfterSetupFn != null)
      this.context.frameAfterSetupFn();
  }

  public void Refresh(int originCell)
  {
    if (this.context.stampTemplate == null || originCell == this.prevOriginCell)
      return;
    this.prevOriginCell = originCell;
    if (!Grid.IsValidCell(originCell))
      return;
    if (this.context.refreshFn != null)
      this.context.refreshFn(originCell);
    this.context.previewParent.transform.SetPosition(Grid.CellToPosCBC(originCell, this.context.tool.visualizerLayer));
    this.context.previewParent.gameObject.SetActive(true);
  }

  public void OnErrorChange(string error)
  {
    if (this.context.onErrorChangeFn == null)
      return;
    this.context.onErrorChangeFn(error);
  }

  public void OnPlace()
  {
    if (this.context.onPlaceFn == null)
      return;
    this.context.onPlaceFn();
  }

  public void Cleanup()
  {
    if (this.context.cleanupFn != null)
      this.context.cleanupFn();
    this.prevOriginCell = Grid.InvalidCell;
    this.context.stampTemplate = (TemplateContainer) null;
    this.context.frameAfterSetupFn = (System.Action) null;
    this.context.refreshFn = (Action<int>) null;
    this.context.onPlaceFn = (System.Action) null;
    this.context.cleanupFn = (System.Action) null;
  }
}
