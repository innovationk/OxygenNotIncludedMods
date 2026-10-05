// Decompiled with JetBrains decompiler
// Type: FewOptionSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FewOptionSideScreen : SideScreenContent
{
  public GameObject rowPrefab;
  public RectTransform rowContainer;
  public Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
  private FewOptionSideScreen.IFewOptionSideScreen targetFewOptions;

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!show)
      return;
    this.RefreshOptions();
  }

  private void RefreshOptions()
  {
    foreach (KeyValuePair<Tag, GameObject> row in this.rows)
      row.Value.GetComponent<MultiToggle>().ChangeState(row.Key == this.targetFewOptions.GetSelectedOption() ? 1 : 0);
  }

  private void ClearRows()
  {
    for (int index = this.rowContainer.childCount - 1; index >= 0; --index)
      Util.KDestroyGameObject((Component) this.rowContainer.GetChild(index));
    this.rows.Clear();
  }

  private void SpawnRows()
  {
    foreach (FewOptionSideScreen.IFewOptionSideScreen.Option option1 in this.targetFewOptions.GetOptions())
    {
      FewOptionSideScreen.IFewOptionSideScreen.Option option = option1;
      GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("label").SetText(option.labelText);
      component.GetReference<Image>("icon").sprite = option.iconSpriteColorTuple.first;
      component.GetReference<Image>("icon").color = option.iconSpriteColorTuple.second;
      gameObject.GetComponent<ToolTip>().toolTip = option.tooltipText;
      gameObject.GetComponent<MultiToggle>().onClick = (System.Action) (() =>
      {
        this.targetFewOptions.OnOptionSelected(option);
        this.RefreshOptions();
      });
      this.rows.Add(option.tag, gameObject);
    }
    this.RefreshOptions();
  }

  public override void SetTarget(GameObject target)
  {
    this.ClearRows();
    this.targetFewOptions = target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>();
    this.SpawnRows();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>() != null;
  }

  public interface IFewOptionSideScreen
  {
    FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions();

    void OnOptionSelected(
      FewOptionSideScreen.IFewOptionSideScreen.Option option);

    Tag GetSelectedOption();

    struct Option(
      Tag tag,
      string labelText,
      Tuple<Sprite, Color> iconSpriteColorTuple,
      string tooltipText = "")
    {
      public Tag tag = tag;
      public string labelText = labelText;
      public string tooltipText = tooltipText;
      public Tuple<Sprite, Color> iconSpriteColorTuple = iconSpriteColorTuple;
    }
  }
}
