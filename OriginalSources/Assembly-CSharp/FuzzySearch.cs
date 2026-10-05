// Decompiled with JetBrains decompiler
// Type: FuzzySearch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FuzzySharp;
using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
public class FuzzySearch
{
  public const FuzzySearch.Features PHRASE_MUTATION_FEATURES = FuzzySearch.Features.Suppress1And2LetterWords | FuzzySearch.Features.SuppressMeaninglessWords;
  public static readonly char[] TOKEN_SEPARATORS = new char[15]
  {
    ' ',
    '.',
    '\n',
    ',',
    ';',
    ':',
    '?',
    '!',
    '-',
    '(',
    ')',
    '[',
    ']',
    '{',
    '}'
  };

  public static FuzzySearch.Features GetFeatures()
  {
    FuzzySearch.Features features = FuzzySearch.Features.Initialism;
    if (Localization.GetLocale() == null)
      features = features | FuzzySearch.Features.Suppress1And2LetterWords | FuzzySearch.Features.SuppressMeaninglessWords;
    return features;
  }

  public static string Canonicalize(string s) => UI.StripLinkFormatting(UI.StripStyleFormatting(s));

  private static int ScoreImpl_Unchecked(string searchString, string candidate)
  {
    return Fuzz.Ratio(searchString, candidate);
  }

  private static int ScoreImpl(string searchString, string candidate)
  {
    return FuzzySearch.ScoreImpl_Unchecked(searchString, candidate);
  }

  private static bool IsUpper(string s)
  {
    foreach (char c in s)
    {
      if (char.IsLetter(c) && !char.IsUpper(c))
        return false;
    }
    return true;
  }

  private static FuzzySearch.Match ScoreTokens_Unchecked(string searchStringUpper, string[] tokens)
  {
    if (tokens.Length == 0)
      return FuzzySearch.Match.NONE;
    int? nullable1 = new int?();
    string str = (string) null;
    foreach (string token in tokens)
    {
      int num1 = FuzzySearch.ScoreImpl_Unchecked(searchStringUpper, token);
      if (nullable1.HasValue)
      {
        int num2 = num1;
        int? nullable2 = nullable1;
        int valueOrDefault = nullable2.GetValueOrDefault();
        if (!(num2 > valueOrDefault & nullable2.HasValue))
          continue;
      }
      nullable1 = new int?(num1);
      str = token;
    }
    return new FuzzySearch.Match()
    {
      score = nullable1.Value,
      token = str
    };
  }

  private static FuzzySearch.Match ScoreTokens_Unchecked(
    string searchStringUpper,
    IReadOnlyList<string> tokens)
  {
    if (tokens.Count == 0)
      return FuzzySearch.Match.NONE;
    int? nullable1 = new int?();
    string str = (string) null;
    foreach (string token in (IEnumerable<string>) tokens)
    {
      int num1 = FuzzySearch.ScoreImpl_Unchecked(searchStringUpper, token);
      if (nullable1.HasValue)
      {
        int num2 = num1;
        int? nullable2 = nullable1;
        int valueOrDefault = nullable2.GetValueOrDefault();
        if (!(num2 > valueOrDefault & nullable2.HasValue))
          continue;
      }
      nullable1 = new int?(num1);
      str = token;
    }
    return new FuzzySearch.Match()
    {
      score = nullable1.Value,
      token = str
    };
  }

  public static FuzzySearch.Match ScoreTokens(string searchStringUpper, string[] tokens)
  {
    return FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
  }

  public static FuzzySearch.Match ScoreTokens(
    string searchStringUpper,
    IReadOnlyList<string> tokens)
  {
    return FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
  }

  public static FuzzySearch.Match ScoreCanonicalCandidate(
    string searchStringUpper,
    string canonicalCandidate,
    string candidate = null)
  {
    FuzzySearch.Match match1 = new FuzzySearch.Match()
    {
      score = Fuzz.WeightedRatio(searchStringUpper, canonicalCandidate),
      token = candidate ?? canonicalCandidate
    };
    if ((FuzzySearch.GetFeatures() & FuzzySearch.Features.Initialism) != (FuzzySearch.Features) 0)
    {
      int num = Fuzz.TokenInitialismRatio(searchStringUpper, canonicalCandidate);
      if (num > match1.score)
        match1.score = num;
    }
    string[] tokens = canonicalCandidate.Split(FuzzySearch.TOKEN_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
    FuzzySearch.Match match2 = FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
    return match2.score <= match1.score ? match1 : match2;
  }

  public static FuzzySearch.Match CanonicalizeAndScore(string searchStringUpper, string candidate)
  {
    return FuzzySearch.ScoreCanonicalCandidate(searchStringUpper, FuzzySearch.Canonicalize(candidate).ToUpper(), candidate);
  }

  [Flags]
  public enum Features
  {
    Suppress1And2LetterWords = 1,
    SuppressMeaninglessWords = 2,
    Initialism = 4,
  }

  public struct Match
  {
    public int score;
    public string token;
    public static readonly FuzzySearch.Match NONE = new FuzzySearch.Match()
    {
      score = 0,
      token = string.Empty
    };
  }
}
