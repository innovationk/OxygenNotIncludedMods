// Decompiled with JetBrains decompiler
// Type: NativeAnimBatchLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NativeAnimBatchLoader : MonoBehaviour
{
  public bool performTimeUpdate;
  public bool performUpdate;
  public bool performRender;
  public bool setTimeScale;
  public bool destroySelf;
  public bool generateObjects;
  public GameObject[] enableObjects;

  private void Start()
  {
    if (this.generateObjects)
    {
      for (int index = 0; index < this.enableObjects.Length; ++index)
      {
        if ((Object) this.enableObjects[index] != (Object) null)
        {
          this.enableObjects[index].GetComponent<KBatchedAnimController>().visibilityType = KAnimControllerBase.VisibilityType.Always;
          this.enableObjects[index].SetActive(true);
        }
      }
    }
    if (this.setTimeScale)
      Time.timeScale = 1f;
    if (!this.destroySelf)
      return;
    Object.Destroy((Object) this);
  }

  private void LateUpdate()
  {
    if (this.destroySelf)
      return;
    if (this.performUpdate)
    {
      KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
      KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
    }
    if (!this.performRender)
      return;
    KAnimBatchManager.Instance().Render();
  }
}
