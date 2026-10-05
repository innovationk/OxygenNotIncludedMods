// Decompiled with JetBrains decompiler
// Type: CodexElementCategoryList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CodexElementCategoryList : CodexCollapsibleHeader
{
  private List<GameObject> rows = new List<GameObject>();

  public Tag categoryTag { get; set; }

  public CodexElementCategoryList()
    : base((string) STRINGS.UI.CODEX.CATEGORYNAMES.ELEMENTS, (ContentContainer) null)
  {
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
    this.ContentsGameObject = component.GetReference<RectTransform>("ContentContainer").gameObject;
    base.Configure(contentGameObject, displayPane, textStyles);
    RectTransform reference1 = component.GetReference<RectTransform>("HeaderLabel");
    RectTransform reference2 = component.GetReference<RectTransform>("PrefabLabelWithIcon");
    this.ClearPanel(reference2.transform.parent, (Transform) reference2);
    reference1.GetComponent<LocText>().SetText((string) STRINGS.UI.CODEX.CATEGORYNAMES.ELEMENTS);
    foreach (GameObject go in Assets.GetPrefabsWithTag(this.categoryTag))
    {
      GameObject gameObject = Util.KInstantiateUI(reference2.gameObject, reference2.parent.gameObject, true);
      Image componentInChildren = gameObject.GetComponentInChildren<Image>();
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) go);
      componentInChildren.sprite = uiSprite.first;
      componentInChildren.color = uiSprite.second;
      gameObject.GetComponentInChildren<LocText>().SetText(go.GetProperName());
      this.rows.Add(gameObject);
    }
  }

  private void ClearPanel(Transform containerToClear, Transform skipDestroyingPrefab)
  {
    skipDestroyingPrefab.SetAsFirstSibling();
    for (int index = containerToClear.childCount - 1; index >= 1; --index)
      Object.Destroy((Object) containerToClear.GetChild(index).gameObject);
    for (int index = this.rows.Count - 1; index >= 0; --index)
      Object.Destroy((Object) this.rows[index].gameObject);
    this.rows.Clear();
  }
}
