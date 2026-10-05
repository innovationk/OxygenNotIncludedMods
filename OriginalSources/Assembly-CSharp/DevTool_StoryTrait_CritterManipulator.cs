// Decompiled with JetBrains decompiler
// Type: DevTool_StoryTrait_CritterManipulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using STRINGS;
using System.Collections.Generic;

#nullable disable
public class DevTool_StoryTrait_CritterManipulator : DevTool
{
  protected override void RenderTo(DevPanel panel)
  {
    if (!ImGui.CollapsingHeader("Debug species lore unlock popup", ImGuiTreeNodeFlags.DefaultOpen))
      return;
    this.Button_OpenSpecies(Tag.Invalid, "Unknown Species");
    ImGui.Separator();
    foreach (Tag critterSpeciesTag in this.GetCritterSpeciesTags())
      this.Button_OpenSpecies(critterSpeciesTag, GravitasCreatureManipulatorConfig.GetNameForSpeciesTag(critterSpeciesTag).Unwrap());
  }

  public void Button_OpenSpecies(Tag species, string speciesName = null)
  {
    speciesName = speciesName != null ? $"\"{UI.StripLinkFormatting(speciesName)}\" (ID: {species})" : species.Name;
    if (!ImGui.Button("Show popup for: " + speciesName))
      return;
    GravitasCreatureManipulator.Instance.ShowLoreUnlockedPopup(species);
  }

  public IEnumerable<Tag> GetCritterSpeciesTags()
  {
    yield return GameTags.Creatures.Species.HatchSpecies;
    yield return GameTags.Creatures.Species.LightBugSpecies;
    yield return GameTags.Creatures.Species.OilFloaterSpecies;
    yield return GameTags.Creatures.Species.DreckoSpecies;
    yield return GameTags.Creatures.Species.GlomSpecies;
    yield return GameTags.Creatures.Species.PuftSpecies;
    yield return GameTags.Creatures.Species.PacuSpecies;
    yield return GameTags.Creatures.Species.MooSpecies;
    yield return GameTags.Creatures.Species.MoleSpecies;
    yield return GameTags.Creatures.Species.SquirrelSpecies;
    yield return GameTags.Creatures.Species.CrabSpecies;
    yield return GameTags.Creatures.Species.DivergentSpecies;
    yield return GameTags.Creatures.Species.StaterpillarSpecies;
    yield return GameTags.Creatures.Species.BeetaSpecies;
    yield return GameTags.Creatures.Species.SnailSpecies;
  }
}
