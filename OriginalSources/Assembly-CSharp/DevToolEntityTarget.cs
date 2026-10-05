// Decompiled with JetBrains decompiler
// Type: DevToolEntityTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class DevToolEntityTarget
{
  public abstract string GetTag();

  public abstract Option<(Vector2 cornerA, Vector2 cornerB)> GetScreenRect();

  public string GetDebugName() => $"[{this.GetTag()}] {this.ToString()}";

  public class ForUIGameObject : DevToolEntityTarget
  {
    public GameObject gameObject;

    public ForUIGameObject(GameObject gameObject) => this.gameObject = gameObject;

    public override Option<(Vector2 cornerA, Vector2 cornerB)> GetScreenRect()
    {
      if (this.gameObject.IsNullOrDestroyed())
        return (Option<(Vector2, Vector2)>) Option.None;
      RectTransform component = this.gameObject.GetComponent<RectTransform>();
      if (component.IsNullOrDestroyed())
        return (Option<(Vector2, Vector2)>) Option.None;
      Canvas componentInParent = this.gameObject.GetComponentInParent<Canvas>();
      if (component.IsNullOrDestroyed())
        return (Option<(Vector2, Vector2)>) Option.None;
      if (!componentInParent.worldCamera.IsNullOrDestroyed())
      {
        Camera camera = componentInParent.worldCamera;
        Vector3[] fourCornersArray = new Vector3[4];
        component.GetWorldCorners(fourCornersArray);
        return (Option<(Vector2, Vector2)>) (ScreenPointToScreenPosition((Vector2) camera.WorldToScreenPoint(fourCornersArray[0])), ScreenPointToScreenPosition((Vector2) camera.WorldToScreenPoint(fourCornersArray[2])));

        Vector2 ScreenPointToScreenPosition(Vector2 coord)
        {
          return new Vector2(coord.x, (float) camera.pixelHeight - coord.y);
        }
      }
      if (componentInParent.renderMode != RenderMode.ScreenSpaceOverlay)
        return (Option<(Vector2, Vector2)>) Option.None;
      Vector3[] fourCornersArray1 = new Vector3[4];
      component.GetWorldCorners(fourCornersArray1);
      return (Option<(Vector2, Vector2)>) (ScreenPointToScreenPosition((Vector2) fourCornersArray1[0]), ScreenPointToScreenPosition((Vector2) fourCornersArray1[2]));

      static Vector2 ScreenPointToScreenPosition(Vector2 coord)
      {
        return new Vector2(coord.x, (float) Screen.height - coord.y);
      }
    }

    public override string GetTag() => "UI";

    public override string ToString() => DevToolEntity.GetNameFor(this.gameObject);
  }

  public class ForWorldGameObject : DevToolEntityTarget
  {
    public GameObject gameObject;

    public ForWorldGameObject(GameObject gameObject) => this.gameObject = gameObject;

    public override Option<(Vector2 cornerA, Vector2 cornerB)> GetScreenRect()
    {
      if (this.gameObject.IsNullOrDestroyed())
        return (Option<(Vector2, Vector2)>) Option.None;
      Camera camera = Camera.main;
      if (camera.IsNullOrDestroyed())
        return (Option<(Vector2, Vector2)>) Option.None;
      KCollider2D component = this.gameObject.GetComponent<KCollider2D>();
      return component.IsNullOrDestroyed() ? (Option<(Vector2, Vector2)>) Option.None : (Option<(Vector2, Vector2)>) (ScreenPointToScreenPosition((Vector2) camera.WorldToScreenPoint(component.bounds.min)), ScreenPointToScreenPosition((Vector2) camera.WorldToScreenPoint(component.bounds.max)));

      Vector2 ScreenPointToScreenPosition(Vector2 coord)
      {
        return new Vector2(coord.x, (float) camera.pixelHeight - coord.y);
      }
    }

    public override string GetTag() => "World";

    public override string ToString() => DevToolEntity.GetNameFor(this.gameObject);
  }

  public class ForSimCell : DevToolEntityTarget
  {
    public int cellIndex;

    public ForSimCell(int cellIndex) => this.cellIndex = cellIndex;

    public override Option<(Vector2 cornerA, Vector2 cornerB)> GetScreenRect()
    {
      Camera camera = Camera.main;
      if (camera.IsNullOrDestroyed())
        return (Option<(Vector2, Vector2)>) Option.None;
      Vector2 posCcc = (Vector2) Grid.CellToPosCCC(this.cellIndex, Grid.SceneLayer.Background);
      Vector2 vector2 = Grid.HalfCellSizeInMeters * Vector2.one;
      Vector2 position1 = posCcc - vector2;
      Vector2 position2 = posCcc + vector2;
      return (Option<(Vector2, Vector2)>) (ScreenPointToScreenPosition((Vector2) camera.WorldToScreenPoint((Vector3) position1)), ScreenPointToScreenPosition((Vector2) camera.WorldToScreenPoint((Vector3) position2)));

      Vector2 ScreenPointToScreenPosition(Vector2 coord)
      {
        return new Vector2(coord.x, (float) camera.pixelHeight - coord.y);
      }
    }

    public override string GetTag() => "Sim Cell";

    public override string ToString() => this.cellIndex.ToString();
  }
}
