// Decompiled with JetBrains decompiler
// Type: SearchUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
public static class SearchUtil
{
  public const int MATCH_SCORE_MIN = 0;
  public const int MATCH_SCORE_MAX = 100;
  public const int MATCH_SCORE_THRESHOLD = 79;
  private static readonly HashSet<string> MeaninglessWords = new HashSet<string>();
  private static readonly char[] COMMA_DELIMETERS = new char[2]
  {
    ' ',
    ','
  };
  private const int LHS_GT_RHS = -1;
  private const int RHS_GT_LHS = 1;

  private static void CacheMeaninglessWords()
  {
    if (SearchUtil.MeaninglessWords.Count != 0)
      return;
    ListPool<string, SearchUtil.MatchCache>.PooledList searchTerms = ListPool<string, SearchUtil.MatchCache>.Allocate();
    SearchUtil.AddCommaDelimitedSearchTerms((string) SEARCH_TERMS.SUPPRESSED, (List<string>) searchTerms);
    foreach (string str in (List<string>) searchTerms)
      SearchUtil.MeaninglessWords.Add(str);
    searchTerms.Recycle();
  }

  public static bool IsPassingScore(int score) => score >= 79;

  public static string Canonicalize(string s) => FuzzySearch.Canonicalize(s).ToUpper();

  public static string CanonicalizePhrase(string s)
  {
    string upper = FuzzySearch.Canonicalize(s).ToUpper();
    FuzzySearch.Features features = FuzzySearch.GetFeatures();
    if ((features & (FuzzySearch.Features.Suppress1And2LetterWords | FuzzySearch.Features.SuppressMeaninglessWords)) == (FuzzySearch.Features) 0)
      return upper;
    string[] strArray = upper.Split(FuzzySearch.TOKEN_SEPARATORS);
    StringBuilder stringBuilder = new StringBuilder();
    bool flag1 = (features & FuzzySearch.Features.Suppress1And2LetterWords) != 0;
    bool flag2 = (features & FuzzySearch.Features.SuppressMeaninglessWords) != 0;
    if (flag2)
      SearchUtil.CacheMeaninglessWords();
    foreach (string str in strArray)
    {
      if (((!flag1 ? 0 : (str.Length <= 2 ? 1 : 0)) != 0 ? 1 : (!flag2 ? 0 : (SearchUtil.MeaninglessWords.Contains(str) ? 1 : 0))) == 0)
      {
        if (stringBuilder.Length != 0)
          stringBuilder.AppendFormat(" {0}", (object) str);
        else
          stringBuilder.Append(str);
      }
    }
    return stringBuilder.ToString();
  }

  public static void AddCommaDelimitedSearchTerms(
    string commaDelimitedSearchTerms,
    List<string> searchTerms)
  {
    foreach (string str in commaDelimitedSearchTerms.ToUpper().Split(SearchUtil.COMMA_DELIMETERS, StringSplitOptions.RemoveEmptyEntries))
      searchTerms.Add(str);
  }

  public static Dictionary<string, SearchUtil.TechCache> CacheTechs()
  {
    Dictionary<string, SearchUtil.TechCache> dictionary1 = new Dictionary<string, SearchUtil.TechCache>();
    ListPool<ComplexRecipe, SearchUtil.TechCache>.PooledList recipes = ListPool<ComplexRecipe, SearchUtil.TechCache>.Allocate();
    Techs techs = Db.Get().Techs;
    for (int idx = 0; idx != techs.Count; ++idx)
    {
      Tech resource = (Tech) techs.GetResource(idx);
      Dictionary<string, SearchUtil.TechItemCache> dictionary2 = new Dictionary<string, SearchUtil.TechItemCache>();
      foreach (TechItem unlockedItem in resource.unlockedItems)
      {
        recipes.Clear();
        BuildingDef.CollectFabricationRecipes((Tag) unlockedItem.Id, (List<ComplexRecipe>) recipes);
        List<SearchUtil.NameDescCache> nameDescCacheList = new List<SearchUtil.NameDescCache>();
        foreach (ComplexRecipe complexRecipe in (List<ComplexRecipe>) recipes)
          nameDescCacheList.Add(new SearchUtil.NameDescCache()
          {
            name = new SearchUtil.MatchCache()
            {
              text = SearchUtil.Canonicalize(complexRecipe.GetUIName(false))
            },
            desc = new SearchUtil.MatchCache()
            {
              text = SearchUtil.CanonicalizePhrase(complexRecipe.description)
            }
          });
        TechItem techItem = Db.Get().TechItems.Get(unlockedItem.Id);
        SearchUtil.TechItemCache techItemCache = new SearchUtil.TechItemCache()
        {
          nameDescSearchTerms = new SearchUtil.NameDescSearchTermsCache()
          {
            nameDesc = new SearchUtil.NameDescCache()
            {
              name = new SearchUtil.MatchCache()
              {
                text = SearchUtil.Canonicalize(techItem.Name)
              },
              desc = new SearchUtil.MatchCache()
              {
                text = SearchUtil.CanonicalizePhrase(techItem.description)
              }
            },
            searchTerms = (IReadOnlyList<string>) techItem.searchTerms
          },
          recipes = nameDescCacheList,
          tier = resource.tier
        };
        dictionary2[unlockedItem.Id] = techItemCache;
      }
      SearchUtil.TechCache techCache = new SearchUtil.TechCache()
      {
        tech = new SearchUtil.NameDescSearchTermsCache()
        {
          nameDesc = new SearchUtil.NameDescCache()
          {
            name = new SearchUtil.MatchCache()
            {
              text = SearchUtil.Canonicalize(resource.Name)
            },
            desc = new SearchUtil.MatchCache()
            {
              text = SearchUtil.CanonicalizePhrase(resource.desc)
            }
          },
          searchTerms = (IReadOnlyList<string>) resource.searchTerms
        },
        techItems = dictionary2,
        tier = resource.tier
      };
      dictionary1[resource.Id] = techCache;
    }
    recipes.Recycle();
    return dictionary1;
  }

  public static SearchUtil.BuildingDefCache MakeBuildingDefCache(BuildingDef def)
  {
    SearchUtil.NameDescSearchTermsCache searchTermsCache = new SearchUtil.NameDescSearchTermsCache()
    {
      nameDesc = new SearchUtil.NameDescCache()
      {
        name = new SearchUtil.MatchCache()
        {
          text = SearchUtil.Canonicalize(def.Name)
        },
        desc = new SearchUtil.MatchCache()
        {
          text = SearchUtil.CanonicalizePhrase(def.Desc)
        }
      },
      searchTerms = def.SearchTerms
    };
    SearchUtil.MatchCache matchCache = new SearchUtil.MatchCache()
    {
      text = SearchUtil.CanonicalizePhrase(def.Effect)
    };
    List<SearchUtil.NameDescCache> nameDescCacheList = new List<SearchUtil.NameDescCache>();
    ListPool<ComplexRecipe, PlanBuildingToggle>.PooledList recipes = ListPool<ComplexRecipe, PlanBuildingToggle>.Allocate();
    BuildingDef.CollectFabricationRecipes((Tag) def.PrefabID, (List<ComplexRecipe>) recipes);
    foreach (ComplexRecipe complexRecipe in (List<ComplexRecipe>) recipes)
      nameDescCacheList.Add(new SearchUtil.NameDescCache()
      {
        name = new SearchUtil.MatchCache()
        {
          text = SearchUtil.Canonicalize(complexRecipe.GetUIName(false))
        },
        desc = new SearchUtil.MatchCache()
        {
          text = SearchUtil.CanonicalizePhrase(complexRecipe.description)
        }
      });
    recipes.Recycle();
    return new SearchUtil.BuildingDefCache()
    {
      nameDescSearchTerms = searchTermsCache,
      effect = matchCache,
      recipes = nameDescCacheList
    };
  }

  private interface IScore
  {
    int Score { get; }
  }

  private struct TieBreaker(int _globalMax)
  {
    private readonly int globalMax = _globalMax;
    private int globalMaxCmp = 0;
    private int localMaxScore = -1;
    private int localMaxCmp = 0;

    public readonly bool IsTieBroken => this.globalMaxCmp != 0;

    private int CacheLocalScore(int score, int cmp)
    {
      if (this.localMaxScore == -1 || this.localMaxScore < score)
      {
        this.localMaxScore = score;
        this.localMaxCmp = cmp;
      }
      return this.localMaxCmp;
    }

    private int CacheScore(int score, int cmp)
    {
      if (score != this.globalMax)
        return this.CacheLocalScore(score, cmp);
      this.globalMaxCmp = cmp;
      return this.globalMaxCmp;
    }

    public int Consider(int lhs, int rhs)
    {
      if (this.IsTieBroken)
        return this.globalMaxCmp;
      switch (-lhs.CompareTo(rhs))
      {
        case -1:
          return this.CacheScore(lhs, -1);
        case 0:
          return this.localMaxScore != -1 ? this.localMaxCmp : 0;
        case 1:
          return this.CacheScore(rhs, 1);
        default:
          Debug.Assert(false);
          return 0;
      }
    }

    public int Consider<T>(T lhs, T rhs) where T : IComparable, SearchUtil.IScore
    {
      if (this.IsTieBroken)
        return this.globalMaxCmp;
      if ((object) lhs == null)
      {
        if ((object) rhs != null)
          return this.CacheScore(rhs.Score, 1);
        return this.localMaxScore != -1 ? this.localMaxCmp : 0;
      }
      if ((object) rhs == null)
        return this.CacheScore(lhs.Score, -1);
      switch (lhs.CompareTo((object) rhs))
      {
        case -1:
          return this.CacheScore(lhs.Score, -1);
        case 0:
          return this.localMaxScore != -1 ? this.localMaxCmp : 0;
        case 1:
          return this.CacheScore(rhs.Score, 1);
        default:
          Debug.Assert(false);
          return 0;
      }
    }
  }

  public class MatchCache : IComparable, SearchUtil.IScore
  {
    public string text;

    public int Score => this.FuzzyMatch.score;

    public bool IsPassingScore() => this.Score >= 79;

    public FuzzySearch.Match FuzzyMatch { get; private set; }

    public void Bind(string searchStringUpper)
    {
      try
      {
        this.FuzzyMatch = FuzzySearch.ScoreCanonicalCandidate(searchStringUpper, this.text);
      }
      catch (Exception ex)
      {
        throw new Exception($"searchStringUpper: {searchStringUpper}, text: {this.text}", ex);
      }
    }

    public void Reset() => this.FuzzyMatch = FuzzySearch.Match.NONE;

    public int CompareTo(object obj) => -this.Score.CompareTo(((SearchUtil.MatchCache) obj).Score);
  }

  public class NameDescCache : IComparable, SearchUtil.IScore
  {
    public SearchUtil.MatchCache name;
    public SearchUtil.MatchCache desc;

    public void Bind(string searchStringUpper)
    {
      this.name.Bind(searchStringUpper);
      this.desc.Bind(searchStringUpper);
    }

    public void Reset()
    {
      this.name.Reset();
      this.desc.Reset();
    }

    public int Score => Math.Max(this.name.Score, this.desc.Score);

    public int CompareTo(object obj)
    {
      SearchUtil.NameDescCache nameDescCache = (SearchUtil.NameDescCache) obj;
      int score1 = this.Score;
      int score2 = nameDescCache.Score;
      int num = -score1.CompareTo(score2);
      if (num != 0)
        return num;
      SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score1);
      tieBreaker.Consider<SearchUtil.MatchCache>(this.name, nameDescCache.name);
      return tieBreaker.Consider<SearchUtil.MatchCache>(this.desc, nameDescCache.desc);
    }
  }

  public class NameDescSearchTermsCache : IComparable, SearchUtil.IScore
  {
    public SearchUtil.NameDescCache nameDesc;
    public IReadOnlyList<string> searchTerms;

    public FuzzySearch.Match SearchTermsScore { get; private set; }

    public void Bind(string searchStringUpper)
    {
      this.nameDesc.Bind(searchStringUpper);
      this.SearchTermsScore = FuzzySearch.ScoreTokens(searchStringUpper, this.searchTerms);
    }

    public void Reset()
    {
      this.nameDesc.Reset();
      this.SearchTermsScore = FuzzySearch.Match.NONE;
    }

    public int Score => Math.Max(this.nameDesc.Score, this.SearchTermsScore.score);

    public bool IsPassingScore() => this.Score >= 79;

    public int CompareTo(object obj)
    {
      SearchUtil.NameDescSearchTermsCache searchTermsCache = (SearchUtil.NameDescSearchTermsCache) obj;
      int score1 = this.Score;
      int score2 = searchTermsCache.Score;
      int num = -score1.CompareTo(score2);
      if (num != 0)
        return num;
      SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score1);
      tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDesc.name, searchTermsCache.nameDesc.name);
      tieBreaker.Consider(this.SearchTermsScore.score, searchTermsCache.SearchTermsScore.score);
      return tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDesc.desc, searchTermsCache.nameDesc.desc);
    }
  }

  public class BuildingDefCache : IComparable, SearchUtil.IScore
  {
    public SearchUtil.NameDescSearchTermsCache nameDescSearchTerms;
    public SearchUtil.MatchCache effect;
    public List<SearchUtil.NameDescCache> recipes;

    public SearchUtil.NameDescCache BestRecipe { get; private set; }

    public void Bind(string searchStringUpper)
    {
      this.nameDescSearchTerms.Bind(searchStringUpper);
      this.effect.Bind(searchStringUpper);
      this.BestRecipe = (SearchUtil.NameDescCache) null;
      foreach (SearchUtil.NameDescCache recipe in this.recipes)
      {
        recipe.Bind(searchStringUpper);
        if (this.BestRecipe == null || recipe.CompareTo((object) this.BestRecipe) == -1)
          this.BestRecipe = recipe;
      }
    }

    public void Reset()
    {
      this.nameDescSearchTerms.Reset();
      this.effect.Reset();
      foreach (SearchUtil.NameDescCache recipe in this.recipes)
        recipe.Reset();
      this.BestRecipe = (SearchUtil.NameDescCache) null;
    }

    public int Score
    {
      get
      {
        return Math.Max(this.nameDescSearchTerms.Score, Math.Max(this.effect.Score, this.BestRecipe == null ? 0 : this.BestRecipe.Score));
      }
    }

    public bool IsPassingScore() => this.Score >= 79;

    public int CompareTo(object obj)
    {
      SearchUtil.BuildingDefCache buildingDefCache = (SearchUtil.BuildingDefCache) obj;
      int score1 = this.Score;
      int score2 = buildingDefCache.Score;
      int num = -score1.CompareTo(score2);
      if (num != 0)
        return num;
      SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score1);
      tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.name, buildingDefCache.nameDescSearchTerms.nameDesc.name);
      tieBreaker.Consider(this.nameDescSearchTerms.SearchTermsScore.score, buildingDefCache.nameDescSearchTerms.SearchTermsScore.score);
      tieBreaker.Consider<SearchUtil.MatchCache>(this.effect, buildingDefCache.effect);
      return tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.desc, buildingDefCache.nameDescSearchTerms.nameDesc.desc);
    }
  }

  public class TechItemCache : IComparable, SearchUtil.IScore
  {
    public SearchUtil.NameDescSearchTermsCache nameDescSearchTerms;
    public List<SearchUtil.NameDescCache> recipes;
    public int tier;

    public SearchUtil.NameDescCache BestRecipe { get; private set; }

    public void Bind(string searchStringUpper)
    {
      this.nameDescSearchTerms.Bind(searchStringUpper);
      this.BestRecipe = (SearchUtil.NameDescCache) null;
      foreach (SearchUtil.NameDescCache recipe in this.recipes)
      {
        recipe.Bind(searchStringUpper);
        if (this.BestRecipe == null || recipe.CompareTo((object) this.BestRecipe) == -1)
          this.BestRecipe = recipe;
      }
    }

    public void Reset()
    {
      this.nameDescSearchTerms.Reset();
      foreach (SearchUtil.NameDescCache recipe in this.recipes)
        recipe.Reset();
      this.BestRecipe = (SearchUtil.NameDescCache) null;
    }

    public int Score
    {
      get
      {
        return Math.Max(this.nameDescSearchTerms.Score, this.BestRecipe == null ? 0 : this.BestRecipe.Score);
      }
    }

    public bool IsPassingScore() => this.Score >= 79;

    public int CompareTo(object obj)
    {
      SearchUtil.TechItemCache techItemCache = (SearchUtil.TechItemCache) obj;
      int score1 = this.Score;
      int score2 = techItemCache.Score;
      int num1 = -score1.CompareTo(score2);
      if (num1 != 0)
        return num1;
      SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score1);
      tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.name, techItemCache.nameDescSearchTerms.nameDesc.name);
      tieBreaker.Consider(this.nameDescSearchTerms.SearchTermsScore.score, techItemCache.nameDescSearchTerms.SearchTermsScore.score);
      if (!tieBreaker.IsTieBroken)
      {
        int num2 = this.tier.CompareTo(techItemCache.tier);
        if (num2 != 0)
          return num2;
      }
      tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.desc, techItemCache.nameDescSearchTerms.nameDesc.desc);
      return tieBreaker.Consider<SearchUtil.NameDescCache>(this.BestRecipe, techItemCache.BestRecipe);
    }
  }

  public class TechCache : IComparable
  {
    public SearchUtil.NameDescSearchTermsCache tech;
    public Dictionary<string, SearchUtil.TechItemCache> techItems;
    public int tier;

    public SearchUtil.TechItemCache BestItem { get; private set; }

    public void Bind(string searchStringUpper)
    {
      this.tech.Bind(searchStringUpper);
      this.BestItem = (SearchUtil.TechItemCache) null;
      foreach (KeyValuePair<string, SearchUtil.TechItemCache> techItem in this.techItems)
      {
        techItem.Value.Bind(searchStringUpper);
        if (this.BestItem == null || techItem.Value.CompareTo((object) this.BestItem) == -1)
          this.BestItem = techItem.Value;
      }
    }

    public void Reset()
    {
      this.tech.Reset();
      foreach (KeyValuePair<string, SearchUtil.TechItemCache> techItem in this.techItems)
        techItem.Value.Reset();
      this.BestItem = (SearchUtil.TechItemCache) null;
    }

    public int Score => Math.Max(this.tech.Score, this.BestItem == null ? 0 : this.BestItem.Score);

    public bool IsPassingScore() => this.Score >= 79;

    public int CompareTo(object obj)
    {
      SearchUtil.TechCache techCache = (SearchUtil.TechCache) obj;
      int score1 = this.Score;
      int score2 = techCache.Score;
      int num1 = -score1.CompareTo(score2);
      if (num1 != 0)
        return num1;
      SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score1);
      tieBreaker.Consider<SearchUtil.MatchCache>(this.tech.nameDesc.name, techCache.tech.nameDesc.name);
      tieBreaker.Consider(this.tech.SearchTermsScore.score, techCache.tech.SearchTermsScore.score);
      if (!tieBreaker.IsTieBroken)
      {
        int num2 = this.tier.CompareTo(techCache.tier);
        if (num2 != 0)
          return num2;
      }
      tieBreaker.Consider<SearchUtil.MatchCache>(this.tech.nameDesc.desc, techCache.tech.nameDesc.desc);
      return tieBreaker.Consider<SearchUtil.TechItemCache>(this.BestItem, techCache.BestItem);
    }
  }

  public class SubcategoryCache : IComparable
  {
    public SearchUtil.MatchCache subcategory;
    public HashSet<SearchUtil.BuildingDefCache> buildingDefs;

    public SearchUtil.BuildingDefCache BestBuildingDef { get; private set; }

    public void Bind(string searchStringUpper)
    {
      this.subcategory.Bind(searchStringUpper);
      this.BestBuildingDef = (SearchUtil.BuildingDefCache) null;
      foreach (SearchUtil.BuildingDefCache buildingDef in this.buildingDefs)
      {
        buildingDef.Bind(searchStringUpper);
        if (this.BestBuildingDef == null || buildingDef.CompareTo((object) this.BestBuildingDef) == -1)
          this.BestBuildingDef = buildingDef;
      }
    }

    public void Reset()
    {
      this.subcategory.Reset();
      foreach (SearchUtil.BuildingDefCache buildingDef in this.buildingDefs)
        buildingDef.Reset();
      this.BestBuildingDef = (SearchUtil.BuildingDefCache) null;
    }

    public int Score
    {
      get
      {
        return Math.Max(this.subcategory.Score, this.BestBuildingDef == null ? 0 : this.BestBuildingDef.Score);
      }
    }

    public bool IsPassingScore() => this.Score >= 79;

    public int CompareTo(object obj)
    {
      SearchUtil.SubcategoryCache subcategoryCache = (SearchUtil.SubcategoryCache) obj;
      int score1 = this.Score;
      int score2 = subcategoryCache.Score;
      int num = -score1.CompareTo(score2);
      if (num != 0)
        return num;
      SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score1);
      tieBreaker.Consider<SearchUtil.MatchCache>(this.subcategory, subcategoryCache.subcategory);
      return tieBreaker.Consider<SearchUtil.BuildingDefCache>(this.BestBuildingDef, subcategoryCache.BestBuildingDef);
    }
  }
}
