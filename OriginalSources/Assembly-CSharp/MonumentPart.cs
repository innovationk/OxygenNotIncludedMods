// Decompiled with JetBrains decompiler
// Type: MonumentPart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using System.Runtime.Serialization;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/MonumentPart")]
public class MonumentPart : KMonoBehaviour
{
  public MonumentPartResource.Part part;
  public string stateUISymbol;
  [Serialize]
  private string chosenState;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.MonumentParts.Add(this);
    if (!string.IsNullOrEmpty(this.chosenState))
      this.SetState(this.chosenState);
    this.UpdateMonumentDecor();
  }

  [OnDeserialized]
  private void OnDeserializedMethod()
  {
    if (Db.GetMonumentParts().TryGet(this.chosenState) != null)
      return;
    string id = "";
    if (this.part == MonumentPartResource.Part.Bottom)
      id = "bottom_" + this.chosenState;
    else if (this.part == MonumentPartResource.Part.Middle)
      id = "mid_" + this.chosenState;
    else if (this.part == MonumentPartResource.Part.Top)
      id = "top_" + this.chosenState;
    if (Db.GetMonumentParts().TryGet(id) == null)
      return;
    this.chosenState = id;
  }

  protected override void OnCleanUp()
  {
    Components.MonumentParts.Remove(this);
    this.RemoveMonumentPiece();
    base.OnCleanUp();
  }

  public void SetState(string state)
  {
    MonumentPartResource monumentPartResource = Db.GetMonumentParts().TryGet(state);
    if (monumentPartResource == null)
      return;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.SwapAnims(new KAnimFile[1]
    {
      monumentPartResource.AnimFile
    });
    component.Play((HashedString) monumentPartResource.State);
    this.chosenState = state;
  }

  public bool IsMonumentCompleted()
  {
    int num1 = (Object) this.GetMonumentPart(MonumentPartResource.Part.Top) != (Object) null ? 1 : 0;
    bool flag = (Object) this.GetMonumentPart(MonumentPartResource.Part.Middle) != (Object) null;
    int num2 = (Object) this.GetMonumentPart(MonumentPartResource.Part.Bottom) != (Object) null ? 1 : 0;
    return (num1 & num2 & (flag ? 1 : 0)) != 0;
  }

  public void UpdateMonumentDecor()
  {
    GameObject monumentPart = this.GetMonumentPart(MonumentPartResource.Part.Middle);
    if (!this.IsMonumentCompleted())
      return;
    monumentPart.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.COMPLETE);
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      if ((Object) gameObject != (Object) monumentPart)
        gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.NONE);
    }
  }

  public void RemoveMonumentPiece()
  {
    if (!this.IsMonumentCompleted())
      return;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      if ((Object) gameObject.GetComponent<MonumentPart>() != (Object) this)
        gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
    }
  }

  private GameObject GetMonumentPart(MonumentPartResource.Part requestPart)
  {
    foreach (GameObject monumentPart in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      MonumentPart component = monumentPart.GetComponent<MonumentPart>();
      if (!((Object) component == (Object) null) && component.part == requestPart)
        return monumentPart;
    }
    return (GameObject) null;
  }
}
