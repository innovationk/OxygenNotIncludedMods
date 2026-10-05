// Decompiled with JetBrains decompiler
// Type: DevToolFuzzy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class DevToolFuzzy : DevTool
{
  private string searchText = "";
  private float mostRecentEditTime;
  private bool refresh;
  private const float REFRESH_DELAY = 0.4f;
  private int scoreThreshold = 79;
  private readonly List<SearchUtil.TechCache> techs = new List<SearchUtil.TechCache>();
  private readonly List<SearchUtil.BuildingDefCache> buildingDefs = new List<SearchUtil.BuildingDefCache>();

  public DevToolFuzzy() => this.mostRecentEditTime = Time.unscaledTime;

  private void RecipesUi(StringBuilder sb, string id, List<SearchUtil.NameDescCache> recipes)
  {
    int score = 0;
    foreach (SearchUtil.NameDescCache recipe in recipes)
    {
      if (recipe.Score > score)
        score = recipe.Score;
    }
    if (!this.IsPassingScore(score))
      return;
    sb.Clear();
    sb.AppendFormat("[{0}] Recipes##{1}", (object) score, (object) id);
    if (!ImGui.CollapsingHeader(sb.ToString()))
      return;
    ImGui.Indent();
    foreach (SearchUtil.NameDescCache recipe in recipes)
    {
      if (this.IsPassingScore(recipe.Score))
      {
        sb.Clear();
        sb.AppendFormat("{0}##{1}", (object) DevToolFuzzy.FormatScoreDisplay(recipe.Score, recipe.name.text), (object) id);
        if (ImGui.CollapsingHeader(sb.ToString()))
          this.DisplayIfScorePasses(recipe);
      }
    }
    ImGui.Unindent();
  }

  private void TechItemUi(
    StringBuilder sb,
    string id,
    SearchUtil.TechItemCache techItem,
    SearchUtil.TechCache parentTech = null)
  {
    if (!this.IsPassingScore(techItem.Score))
      return;
    sb.Clear();
    sb.AppendFormat("{0}##TechItem{1}", (object) DevToolFuzzy.FormatScoreDisplay(techItem.Score, techItem.nameDescSearchTerms.nameDesc.name.text), (object) id);
    string str = sb.ToString();
    if (!ImGui.CollapsingHeader(str))
      return;
    ImGui.Indent();
    if (parentTech != null)
      ImGui.LabelText("Parent Tech", parentTech.tech.nameDesc.name.text);
    this.DisplayIfScorePasses(techItem.nameDescSearchTerms);
    this.RecipesUi(sb, str, techItem.recipes);
    ImGui.Unindent();
  }

  protected override void RenderTo(DevPanel panel)
  {
    if (ImGui.InputText("Search Text", ref this.searchText, 30U))
    {
      this.refresh = true;
      this.mostRecentEditTime = Time.unscaledTime;
    }
    if (this.refresh && (double) Time.unscaledTime - (double) this.mostRecentEditTime > 0.40000000596046448)
    {
      this.Refresh();
      this.refresh = false;
    }
    ImGui.DragInt("Score Threshold", ref this.scoreThreshold, 0.5f, 0, 100);
    StringBuilder sb = new StringBuilder();
    if (ImGui.CollapsingHeader("Techs"))
    {
      ImGui.Indent();
      foreach (SearchUtil.TechCache tech in this.techs)
      {
        if (this.IsPassingScore(tech.Score))
        {
          sb.Clear();
          sb.AppendFormat("{0}##Tech", (object) DevToolFuzzy.FormatScoreDisplay(tech.Score, tech.tech.nameDesc.name.text));
          string str = sb.ToString();
          if (ImGui.CollapsingHeader(str))
          {
            ImGui.Indent();
            this.DisplayIfScorePasses(tech.tech);
            foreach (KeyValuePair<string, SearchUtil.TechItemCache> techItem in tech.techItems)
              this.TechItemUi(sb, str, techItem.Value);
            ImGui.Unindent();
          }
        }
      }
      ImGui.Unindent();
    }
    if (ImGui.CollapsingHeader("TechItems"))
    {
      ImGui.Indent();
      foreach (SearchUtil.TechCache tech in this.techs)
      {
        foreach (KeyValuePair<string, SearchUtil.TechItemCache> techItem in tech.techItems)
          this.TechItemUi(sb, "TechItem", techItem.Value, tech);
      }
      ImGui.Unindent();
    }
    if (!ImGui.CollapsingHeader("BuildingDefs"))
      return;
    ImGui.Indent();
    foreach (SearchUtil.BuildingDefCache buildingDef in this.buildingDefs)
    {
      if (this.IsPassingScore(buildingDef.Score))
      {
        sb.Clear();
        sb.AppendFormat("{0}##BuildingDef", (object) DevToolFuzzy.FormatScoreDisplay(buildingDef.Score, buildingDef.nameDescSearchTerms.nameDesc.name.text));
        string str = sb.ToString();
        if (ImGui.CollapsingHeader(str))
        {
          ImGui.Indent();
          this.DisplayIfScorePasses(buildingDef.nameDescSearchTerms);
          this.DisplayIfScorePasses("Effect", buildingDef.effect);
          this.RecipesUi(sb, str, buildingDef.recipes);
          ImGui.Unindent();
        }
      }
    }
    ImGui.Unindent();
  }

  private void Refresh()
  {
    string searchStringUpper = this.searchText.ToUpper().Trim();
    if (string.IsNullOrEmpty(searchStringUpper))
      return;
    if (this.techs.Count == 0)
    {
      foreach (KeyValuePair<string, SearchUtil.TechCache> cacheTech in SearchUtil.CacheTechs())
        this.techs.Add(cacheTech.Value);
    }
    foreach (SearchUtil.TechCache tech in this.techs)
      tech.Bind(searchStringUpper);
    this.techs.Sort();
    if (this.buildingDefs.Count == 0)
    {
      foreach (BuildingDef buildingDef in Assets.BuildingDefs)
        this.buildingDefs.Add(SearchUtil.MakeBuildingDefCache(buildingDef));
    }
    foreach (SearchUtil.BuildingDefCache buildingDef in this.buildingDefs)
      buildingDef.Bind(searchStringUpper);
    this.buildingDefs.Sort();
  }

  private bool IsPassingScore(int score) => score >= this.scoreThreshold;

  private static string FormatScoreDisplay(int score, string text)
  {
    return $"[{score}] {FuzzySearch.Canonicalize(text)}";
  }

  private static void DisplayScore(int score, string label, string token, string text)
  {
    ImGui.Text($"[{score}]");
    ImGui.SameLine();
    ImGui.Text(label);
    ImGui.SameLine();
    ImGui.Text($"({token})");
    ImGui.SameLine();
    ImGui.TextWrapped(text);
  }

  private static void DisplayScore(string label, SearchUtil.MatchCache match)
  {
    DevToolFuzzy.DisplayScore(match.Score, label, match.FuzzyMatch.token, match.text);
  }

  private void DisplayIfScorePasses(string label, SearchUtil.MatchCache match)
  {
    if (!this.IsPassingScore(match.Score))
      return;
    DevToolFuzzy.DisplayScore(label, match);
  }

  private void DisplayIfScorePasses(SearchUtil.NameDescCache nameDesc)
  {
    this.DisplayIfScorePasses("Name", nameDesc.name);
    this.DisplayIfScorePasses("Desc", nameDesc.desc);
  }

  private void DisplayIfScorePasses(
    SearchUtil.NameDescSearchTermsCache nameDescSearchTerms)
  {
    this.DisplayIfScorePasses(nameDescSearchTerms.nameDesc);
    if (!this.IsPassingScore(nameDescSearchTerms.SearchTermsScore.score))
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendJoin<string>(", ", (IEnumerable<string>) nameDescSearchTerms.searchTerms);
    DevToolFuzzy.DisplayScore(nameDescSearchTerms.SearchTermsScore.score, "SearchTerms", nameDescSearchTerms.SearchTermsScore.token, stringBuilder.ToString());
  }
}
