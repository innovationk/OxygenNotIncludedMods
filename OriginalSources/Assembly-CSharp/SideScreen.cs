// Decompiled with JetBrains decompiler
// Type: SideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SideScreen : KScreen
{
  [SerializeField]
  private GameObject contentBody;

  public void SetContent(SideScreenContent sideScreenContent, GameObject target)
  {
    if ((Object) sideScreenContent.transform.parent != (Object) this.contentBody.transform)
      sideScreenContent.transform.SetParent(this.contentBody.transform);
    sideScreenContent.SetTarget(target);
  }
}
