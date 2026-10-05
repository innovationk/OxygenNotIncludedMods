// Decompiled with JetBrains decompiler
// Type: GraphLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (GraphBase))]
[AddComponentMenu("KMonoBehaviour/scripts/GraphLayer")]
public class GraphLayer : KMonoBehaviour
{
  [MyCmpReq]
  private GraphBase graph_base;

  public GraphBase graph
  {
    get
    {
      if ((Object) this.graph_base == (Object) null)
        this.graph_base = this.GetComponent<GraphBase>();
      return this.graph_base;
    }
  }
}
