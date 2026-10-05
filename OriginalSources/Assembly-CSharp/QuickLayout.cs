// Decompiled with JetBrains decompiler
// Type: QuickLayout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class QuickLayout : KMonoBehaviour
{
  [Header("Configuration")]
  [SerializeField]
  private int elementSize;
  [SerializeField]
  private int spacing;
  [SerializeField]
  private QuickLayout.LayoutDirection layoutDirection;
  [SerializeField]
  private Vector2 offset;
  [SerializeField]
  private RectTransform driveParentRectSize;
  private int _elementSize;
  private int _spacing;
  private QuickLayout.LayoutDirection _layoutDirection;
  private Vector2 _offset;
  private int oldActiveChildCount;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ForceUpdate();
  }

  private void OnEnable() => this.ForceUpdate();

  private void LateUpdate() => this.Run();

  public void ForceUpdate() => this.Run(true);

  private void Run(bool forceUpdate = false)
  {
    forceUpdate = forceUpdate || this._elementSize != this.elementSize;
    forceUpdate = forceUpdate || this._spacing != this.spacing;
    forceUpdate = forceUpdate || this._layoutDirection != this.layoutDirection;
    forceUpdate = forceUpdate || this._offset != this.offset;
    if (forceUpdate)
    {
      this._elementSize = this.elementSize;
      this._spacing = this.spacing;
      this._layoutDirection = this.layoutDirection;
      this._offset = this.offset;
    }
    int num = 0;
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      if (this.transform.GetChild(index).gameObject.activeInHierarchy)
        ++num;
    }
    if (!(num != this.oldActiveChildCount | forceUpdate))
      return;
    this.Layout();
    this.oldActiveChildCount = num;
  }

  public void Layout()
  {
    Vector3 offset = (Vector3) this._offset;
    bool flag = false;
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      if (this.transform.GetChild(index).gameObject.activeInHierarchy)
      {
        flag = true;
        this.transform.GetChild(index).rectTransform().anchoredPosition = (Vector2) offset;
        offset += (Vector3) ((float) (this._elementSize + this._spacing) * this.GetDirectionVector());
      }
    }
    if (!((Object) this.driveParentRectSize != (Object) null))
      return;
    if (!flag)
    {
      if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
      {
        this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(this.driveParentRectSize.sizeDelta.x), 0.0f);
      }
      else
      {
        if (this._layoutDirection != QuickLayout.LayoutDirection.LeftToRight && this._layoutDirection != QuickLayout.LayoutDirection.LeftToRight)
          return;
        this.driveParentRectSize.sizeDelta = new Vector2(0.0f, Mathf.Abs(this.driveParentRectSize.sizeDelta.y));
      }
    }
    else if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
    {
      this.driveParentRectSize.sizeDelta = new Vector2(this.driveParentRectSize.sizeDelta.x, Mathf.Abs(offset.y));
    }
    else
    {
      if (this._layoutDirection != QuickLayout.LayoutDirection.LeftToRight && this._layoutDirection != QuickLayout.LayoutDirection.LeftToRight)
        return;
      this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(offset.x), this.driveParentRectSize.sizeDelta.y);
    }
  }

  private Vector2 GetDirectionVector()
  {
    Vector2 directionVector = (Vector2) Vector3.zero;
    switch (this._layoutDirection)
    {
      case QuickLayout.LayoutDirection.TopToBottom:
        directionVector = Vector2.down;
        break;
      case QuickLayout.LayoutDirection.BottomToTop:
        directionVector = Vector2.up;
        break;
      case QuickLayout.LayoutDirection.LeftToRight:
        directionVector = Vector2.right;
        break;
      case QuickLayout.LayoutDirection.RightToLeft:
        directionVector = Vector2.left;
        break;
    }
    return directionVector;
  }

  private enum LayoutDirection
  {
    TopToBottom,
    BottomToTop,
    LeftToRight,
    RightToLeft,
  }
}
