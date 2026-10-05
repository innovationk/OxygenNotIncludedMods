// Decompiled with JetBrains decompiler
// Type: TreeFilterableSideScreenElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenElement")]
public class TreeFilterableSideScreenElement : KMonoBehaviour
{
  [SerializeField]
  private LocText elementName;
  [SerializeField]
  private MultiToggle checkBox;
  [SerializeField]
  private KImage elementImg;
  private KImage checkBoxImg;
  private Tag elementTag;
  public Action<Tag, bool> OnSelectionChanged;
  private TreeFilterableSideScreen parent;
  private bool initialized;

  public Tag GetElementTag() => this.elementTag;

  public bool IsSelected => this.checkBox.CurrentState == 1;

  public MultiToggle GetCheckboxToggle() => this.checkBox;

  public TreeFilterableSideScreen Parent
  {
    get => this.parent;
    set => this.parent = value;
  }

  private void Initialize()
  {
    if (this.initialized)
      return;
    this.checkBoxImg = this.checkBox.gameObject.GetComponentInChildrenOnly<KImage>();
    this.checkBox.onClick = new System.Action(this.CheckBoxClicked);
    this.initialized = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Initialize();
  }

  public Sprite GetStorageObjectSprite(Tag t)
  {
    Sprite storageObjectSprite = (Sprite) null;
    GameObject prefab = Assets.GetPrefab(t);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        storageObjectSprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0]);
    }
    return storageObjectSprite;
  }

  public void SetSprite(Tag t)
  {
    Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) t);
    this.elementImg.sprite = uiSprite.first;
    this.elementImg.color = uiSprite.second;
    this.elementImg.gameObject.SetActive(true);
  }

  public void SetTag(Tag newTag)
  {
    this.Initialize();
    this.elementTag = newTag;
    this.SetSprite(this.elementTag);
    string str = this.elementTag.ProperName();
    if (this.parent.IsStorage)
    {
      float amountInStorage = this.parent.GetAmountInStorage(this.elementTag);
      str = $"{str}: {GameUtil.GetFormattedMass(amountInStorage)}";
    }
    this.elementName.text = str;
  }

  private void CheckBoxClicked()
  {
    this.SetCheckBox(!this.parent.IsTagAllowed(this.GetElementTag()));
  }

  public void SetCheckBox(bool checkBoxState)
  {
    this.checkBox.ChangeState(checkBoxState ? 1 : 0);
    this.checkBoxImg.enabled = checkBoxState;
    if (this.OnSelectionChanged == null)
      return;
    this.OnSelectionChanged(this.GetElementTag(), checkBoxState);
  }
}
