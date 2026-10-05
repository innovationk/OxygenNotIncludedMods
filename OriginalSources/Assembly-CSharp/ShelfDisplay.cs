// Decompiled with JetBrains decompiler
// Type: ShelfDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ShelfDisplay : KMonoBehaviour
{
  [MyCmpReq]
  private SymbolOverrideController symbolOverrideController;
  [MyCmpReq]
  private KBatchedAnimController kbac;
  [MyCmpReq]
  private Storage storage;
  private KBatchedAnimController[] shelfItems;
  public static Vector3[] offsets = new Vector3[4]
  {
    new Vector3(-0.1f, 0.1f, 0.1f),
    new Vector3(-0.1f, 0.4f, 0.2f),
    new Vector3(0.2f, 0.1f, 0.05f),
    new Vector3(0.2f, 0.4f, 0.15f)
  };

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.shelfItems = new KBatchedAnimController[4];
    this.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
    this.OnStorageChange((object) null);
  }

  private KBatchedAnimController CreateShelfItem(int index, KAnimFile animFile, bool show)
  {
    if ((UnityEngine.Object) animFile == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "Mini-fridge: shelf item anim file is null");
      return (KBatchedAnimController) null;
    }
    KBatchedAnimController shelfItem1 = this.shelfItems[index];
    if ((UnityEngine.Object) shelfItem1 != (UnityEngine.Object) null)
    {
      shelfItem1.enabled = show;
      if (show)
        shelfItem1.SwapAnims(new KAnimFile[1]{ animFile });
      return shelfItem1;
    }
    Vector3 position = this.transform.position;
    GameObject gameObject = new GameObject("Fridge shelf display");
    gameObject.SetActive(false);
    KBatchedAnimController shelfItem2 = gameObject.AddComponent<KBatchedAnimController>();
    shelfItem2.AnimFiles = new KAnimFile[1]{ animFile };
    shelfItem2.initialAnim = "ui";
    shelfItem2.sceneLayer = Grid.SceneLayer.Building;
    shelfItem2.animScale *= 0.4f;
    shelfItem2.enabled = show;
    gameObject.transform.parent = this.gameObject.transform;
    gameObject.transform.position = position + ShelfDisplay.offsets[index];
    gameObject.SetActive(true);
    this.shelfItems[index] = shelfItem2;
    return shelfItem2;
  }

  private void OnStorageChange(object obj)
  {
    List<GameObject> items = this.storage.GetItems();
    items.OrderBy<GameObject, float>((Func<GameObject, float>) (i => i.GetComponent<PrimaryElement>().Mass));
    for (int index = 0; index < 4; ++index)
    {
      if (items.Count > index)
      {
        GameObject gameObject = items[index];
        this.ShowItem(index, gameObject);
      }
      else
        this.HideItem(index);
    }
  }

  private void HideItem(int index)
  {
    if (!((UnityEngine.Object) this.shelfItems[index] != (UnityEngine.Object) null))
      return;
    this.shelfItems[index].enabled = false;
  }

  private void ShowItem(int index, GameObject item)
  {
    KBatchedAnimController component;
    if (!item.TryGetComponent<KBatchedAnimController>(out component))
      return;
    if (component.AnimFiles == null || component.AnimFiles.Length == 0)
    {
      Debug.LogWarning((object) $"ShelfDisplay: anim files null or empty {item.PrefabID()}");
      this.HideItem(index);
    }
    else
      this.CreateShelfItem(index, component.AnimFiles[0], true);
  }
}
