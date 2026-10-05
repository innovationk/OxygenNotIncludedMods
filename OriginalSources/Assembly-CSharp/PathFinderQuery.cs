// Decompiled with JetBrains decompiler
// Type: PathFinderQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class PathFinderQuery
{
  protected int resultCell;
  private NavType resultNavType;

  public virtual string Get_KProfilerName() => "";

  public virtual bool IsMatch(int cell, int parent_cell, int cost) => true;

  public void SetResult(int cell, int cost, NavType nav_type)
  {
    this.resultCell = cell;
    this.resultNavType = nav_type;
  }

  public void ClearResult() => this.resultCell = -1;

  public virtual int GetResultCell() => this.resultCell;

  public NavType GetResultNavType() => this.resultNavType;
}
