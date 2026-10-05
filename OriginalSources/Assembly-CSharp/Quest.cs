// Decompiled with JetBrains decompiler
// Type: Quest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Quest : Resource
{
  public const string STRINGS_PREFIX = "STRINGS.CODEX.QUESTS.";
  public readonly QuestCriteria[] Criteria;
  public readonly string Title;
  public readonly string CompletionText;

  public Quest(string id, QuestCriteria[] criteria)
    : base(id, id)
  {
    Debug.Assert(criteria.Length != 0);
    this.Criteria = criteria;
    string str = "STRINGS.CODEX.QUESTS." + id.ToUpperInvariant();
    StringEntry result;
    if (Strings.TryGet(str + ".NAME", out result))
      this.Title = result.String;
    if (Strings.TryGet(str + ".COMPLETE", out result))
      this.CompletionText = result.String;
    for (int index = 0; index < this.Criteria.Length; ++index)
      this.Criteria[index].PopulateStrings("STRINGS.CODEX.QUESTS.");
  }

  public struct ItemData
  {
    public int LocalCellId;
    public float CurrentValue;
    public Tag SatisfyingItem;
    public Tag QualifyingTag;
    public HashedString CriteriaId;
    private int valueHandle;

    public int ValueHandle
    {
      get => this.valueHandle - 1;
      set => this.valueHandle = value + 1;
    }
  }

  public enum State
  {
    NotStarted,
    InProgress,
    Completed,
  }
}
