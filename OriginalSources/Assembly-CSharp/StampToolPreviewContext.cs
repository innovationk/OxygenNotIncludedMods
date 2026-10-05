// Decompiled with JetBrains decompiler
// Type: StampToolPreviewContext
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class StampToolPreviewContext
{
  public Transform previewParent;
  public InterfaceTool tool;
  public TemplateContainer stampTemplate;
  public System.Action frameAfterSetupFn;
  public Action<int> refreshFn;
  public System.Action onPlaceFn;
  public Action<string> onErrorChangeFn;
  public System.Action cleanupFn;
}
