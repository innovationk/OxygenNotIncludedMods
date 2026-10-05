// Decompiled with JetBrains decompiler
// Type: TemplateContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using TemplateClasses;
using UnityEngine;

#nullable disable
[Serializable]
public class TemplateContainer
{
  public string name { get; set; }

  public int priority { get; set; }

  public TemplateContainer.Info info { get; set; }

  public List<TemplateClasses.Cell> cells { get; set; }

  public List<Prefab> buildings { get; set; }

  public List<Prefab> pickupables { get; set; }

  public List<Prefab> elementalOres { get; set; }

  public List<Prefab> backwallEntities { get; set; }

  public List<Prefab> otherEntities { get; set; }

  public void Init(
    List<TemplateClasses.Cell> _cells,
    List<Prefab> _buildings,
    List<Prefab> _pickupables,
    List<Prefab> _elementalOres,
    List<Prefab> _otherEntities)
  {
    if (_cells != null && _cells.Count > 0)
      this.cells = _cells;
    if (_buildings != null && _buildings.Count > 0)
      this.buildings = _buildings;
    if (_pickupables != null && _pickupables.Count > 0)
      this.pickupables = _pickupables;
    if (_elementalOres != null && _elementalOres.Count > 0)
      this.elementalOres = _elementalOres;
    if (_otherEntities != null && _otherEntities.Count > 0)
      this.otherEntities = _otherEntities;
    this.info = new TemplateContainer.Info();
    this.RefreshInfo();
  }

  public RectInt GetTemplateBounds(int padding = 0)
  {
    return this.GetTemplateBounds(Vector2I.zero, padding);
  }

  public RectInt GetTemplateBounds(Vector2 position, int padding = 0)
  {
    return this.GetTemplateBounds(new Vector2I((int) position.x, (int) position.y), padding);
  }

  public RectInt GetTemplateBounds(Vector2I position, int padding = 0)
  {
    if ((double) (this.info.min - new Vector2f(0, 0)).sqrMagnitude <= 9.9999999747524271E-07)
      this.RefreshInfo();
    return this.info.GetBounds(position, padding);
  }

  public void RefreshInfo()
  {
    if (this.cells == null)
      return;
    int x = 1;
    int num1 = -1;
    int y = 1;
    int num2 = -1;
    foreach (TemplateClasses.Cell cell in this.cells)
    {
      if (cell.location_x < x)
        x = cell.location_x;
      if (cell.location_x > num1)
        num1 = cell.location_x;
      if (cell.location_y < y)
        y = cell.location_y;
      if (cell.location_y > num2)
        num2 = cell.location_y;
    }
    this.info.size = (Vector2f) new Vector2((float) (1 + (num1 - x)), (float) (1 + (num2 - y)));
    this.info.min = (Vector2f) new Vector2((float) x, (float) y);
    this.info.area = this.cells.Count;
  }

  public void SaveToYaml(string save_name)
  {
    string path = TemplateCache.RewriteTemplatePath(save_name);
    if (!Directory.Exists(System.IO.Path.GetDirectoryName(path)))
      Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
    YamlIO.Save<TemplateContainer>(this, path + ".yaml");
  }

  [Serializable]
  public class Info
  {
    public Vector2f size { get; set; }

    public Vector2f min { get; set; }

    public int area { get; set; }

    public Tag[] tags { get; set; }

    public Tag[] discover_tags { get; set; }

    public RectInt GetBounds(Vector2I position, int padding)
    {
      return new RectInt(position.x + (int) this.min.x - padding, position.y + (int) this.min.y - padding, (int) this.size.x + padding * 2, (int) this.size.y + padding * 2);
    }
  }
}
