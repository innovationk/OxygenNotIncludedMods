// Decompiled with JetBrains decompiler
// Type: STRINGS.ELEMENTS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace STRINGS;

public class ELEMENTS
{
  public static LocString ELEMENTDESCSOLID = (LocString) "Resource Type: {0}\nMelting point: {1}\nHardness: {2}";
  public static LocString ELEMENTDESCLIQUID = (LocString) "Resource Type: {0}\nFreezing point: {1}\nEvaporation point: {2}";
  public static LocString ELEMENTDESCGAS = (LocString) "Resource Type: {0}\nCondensation point: {1}";
  public static LocString ELEMENTDESCVACUUM = (LocString) "Resource Type: {0}";
  public static LocString BREATHABLEDESC = (LocString) "<color=#{0}>({1})</color>";
  public static LocString THERMALPROPERTIES = (LocString) "\nSpecific Heat Capacity: {SPECIFIC_HEAT_CAPACITY}\nThermal Conductivity: {THERMAL_CONDUCTIVITY}";
  public static LocString RADIATIONPROPERTIES = (LocString) "Radiation Absorption Factor: {0}\nRadiation Emission/1000kg: {1}";
  public static LocString ELEMENTPROPERTIES = (LocString) "Properties: {0}";

  public class STATE
  {
    public static LocString SOLID = (LocString) "Solid";
    public static LocString LIQUID = (LocString) "Liquid";
    public static LocString GAS = (LocString) "Gas";
    public static LocString VACUUM = (LocString) "None";
  }

  public class MATERIAL_MODIFIERS
  {
    public static LocString EFFECTS_HEADER = (LocString) "<b>Resource Effects:</b>";
    public static LocString DECOR = (LocString) (UI.FormatAsLink("Decor", nameof (DECOR)) + ": {0}");
    public static LocString OVERHEATTEMPERATURE = (LocString) (UI.FormatAsLink("Overheat Temperature", "HEAT") + ": {0}");
    public static LocString HIGH_THERMAL_CONDUCTIVITY = (LocString) UI.FormatAsLink("High Thermal Conductivity", "HEAT");
    public static LocString LOW_THERMAL_CONDUCTIVITY = (LocString) UI.FormatAsLink("Insulator", "HEAT");
    public static LocString LOW_SPECIFIC_HEAT_CAPACITY = (LocString) UI.FormatAsLink("Thermally Reactive", "HEAT");
    public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = (LocString) UI.FormatAsLink("Slow Heating", "HEAT");
    public static LocString EXCELLENT_RADIATION_SHIELD = (LocString) UI.FormatAsLink("Excellent Radiation Shield", "RADIATION");

    public class TOOLTIP
    {
      public static LocString EFFECTS_HEADER = (LocString) "Buildings constructed from this material will have these properties";
      public static LocString DECOR = (LocString) $"This material will add <b>{{0}}</b> to the finished building's {UI.PRE_KEYWORD}Decor{UI.PST_KEYWORD}";
      public static LocString OVERHEATTEMPERATURE = (LocString) $"This material will add <b>{{0}}</b> to the finished building's {UI.PRE_KEYWORD}Overheat Temperature{UI.PST_KEYWORD}";
      public static LocString HIGH_THERMAL_CONDUCTIVITY = (LocString) $"This material disperses {UI.PRE_KEYWORD}Heat{UI.PST_KEYWORD} because energy transfers quickly through materials with high {UI.PRE_KEYWORD}Thermal Conductivity{UI.PST_KEYWORD}\n\nBetween two objects, the rate of {UI.PRE_KEYWORD}Heat{UI.PST_KEYWORD} transfer will be determined by the object with the <i>lowest</i> {UI.PRE_KEYWORD}Thermal Conductivity{UI.PST_KEYWORD}\n\nThermal Conductivity: {{1}} W per degree K difference (Oxygen: 0.024 W)";
      public static LocString LOW_THERMAL_CONDUCTIVITY = (LocString) $"This material retains {UI.PRE_KEYWORD}Heat{UI.PST_KEYWORD} because energy transfers slowly through materials with low {UI.PRE_KEYWORD}Thermal Conductivity{UI.PST_KEYWORD}\n\nBetween two objects, the rate of {UI.PRE_KEYWORD}Heat{UI.PST_KEYWORD} transfer will be determined by the object with the <i>lowest</i> {UI.PRE_KEYWORD}Thermal Conductivity{UI.PST_KEYWORD}\n\nThermal Conductivity: {{1}} W per degree K difference (Oxygen: 0.024 W)";
      public static LocString LOW_SPECIFIC_HEAT_CAPACITY = (LocString) $"{UI.PRE_KEYWORD}Thermally Reactive{UI.PST_KEYWORD} materials require little energy to raise in {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD}, and therefore heat and cool quickly\n\nSpecific Heat Capacity: {{1}} DTU to raise 1g by 1K";
      public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = (LocString) $"{UI.PRE_KEYWORD}Slow Heating{UI.PST_KEYWORD} materials require a large amount of energy to raise in {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD}, and therefore heat and cool slowly\n\nSpecific Heat Capacity: {{1}} DTU to raise 1g by 1K";
      public static LocString EXCELLENT_RADIATION_SHIELD = (LocString) $"{UI.PRE_KEYWORD}Excellent Radiation Shield{UI.PST_KEYWORD} radiation has a hard time passing through materials with a high {UI.PRE_KEYWORD}Radiation Absorption Factor{UI.PST_KEYWORD} value. \n\nRadiation Absorption Factor: {{1}}";
    }
  }

  public class HARDNESS
  {
    public static LocString NA = (LocString) "N/A";
    public static LocString SOFT = (LocString) $"{{0}} ({(string) ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.SOFT})";
    public static LocString VERYSOFT = (LocString) $"{{0}} ({(string) ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYSOFT})";
    public static LocString FIRM = (LocString) $"{{0}} ({(string) ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.FIRM})";
    public static LocString VERYFIRM = (LocString) $"{{0}} ({(string) ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM})";
    public static LocString NEARLYIMPENETRABLE = (LocString) $"{{0}} ({(string) ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE})";
    public static LocString IMPENETRABLE = (LocString) $"{{0}} ({(string) ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.IMPENETRABLE})";

    public class HARDNESS_DESCRIPTOR
    {
      public static LocString SOFT = (LocString) "Soft";
      public static LocString VERYSOFT = (LocString) "Very Soft";
      public static LocString FIRM = (LocString) "Firm";
      public static LocString VERYFIRM = (LocString) "Very Firm";
      public static LocString NEARLYIMPENETRABLE = (LocString) "Nearly Impenetrable";
      public static LocString IMPENETRABLE = (LocString) "Impenetrable";
    }
  }

  public class AEROGEL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Aerogel", nameof (AEROGEL));
    public static LocString DESC = (LocString) "";
  }

  public class ALGAE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Algae", nameof (ALGAE));
    public static LocString DESC = (LocString) $"Algae is a cluster of non-motile, single-celled lifeforms.\n\nIt can be used to produce {(string) ELEMENTS.OXYGEN.NAME} when used in an {(string) BUILDINGS.PREFABS.MINERALDEOXIDIZER.NAME}.";
  }

  public class ALUMINUMORE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Aluminum Ore", nameof (ALUMINUMORE));
    public static LocString DESC = (LocString) $"Aluminum ore, also known as Bauxite, is a sedimentary rock high in metal content.\n\nIt can be refined into {UI.FormatAsLink("Aluminum", "ALUMINUM")}.";
  }

  public class ALUMINUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Aluminum", nameof (ALUMINUM));
    public static LocString DESC = (LocString) $"(Al) Aluminum is a low density {UI.FormatAsLink("Metal", "REFINEDMETAL")}.\n\nIt has high Thermal Conductivity and is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class MOLTENALUMINUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Aluminum", nameof (MOLTENALUMINUM));
    public static LocString DESC = (LocString) $"(Al) Molten Aluminum is a low density {UI.FormatAsLink("Metal", "REFINEDMETAL")} heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class ALUMINUMGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Aluminum Gas", nameof (ALUMINUMGAS));
    public static LocString DESC = (LocString) $"(Al) Aluminum Gas is a low density {UI.FormatAsLink("Metal", "REFINEDMETAL")} heated into a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class BLEACHSTONE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Bleach Stone", nameof (BLEACHSTONE));
    public static LocString DESC = (LocString) $"Bleach stone is an unstable compound that emits unbreathable {UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS")}.\n\nIt is often used in {UI.FormatAsLink("Hygienic", "HANDSANITIZER")} processes.";
  }

  public class BITUMEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Bitumen", nameof (BITUMEN));
    public static LocString DESC = (LocString) $"Bitumen is a sticky viscous residue left behind from {(string) ELEMENTS.PETROLEUM.NAME} production.";
  }

  public class BOTTLEDWATER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Water", nameof (BOTTLEDWATER));
    public static LocString DESC = (LocString) $"(H<sub>2</sub>O) Clean {(string) ELEMENTS.WATER.NAME}, prepped for transport.";
  }

  public class BRINEICE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Brine Ice", nameof (BRINEICE));
    public static LocString DESC = (LocString) $"Brine Ice is a natural, highly concentrated solution of {UI.FormatAsLink("Salt", "SALT")} dissolved in {UI.FormatAsLink("Water", "WATER")} and frozen into a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.\n\nIt can be used in desalination processes, separating out usable salt.";
  }

  public class MURKYBRINEICE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Polluted Brine Ice", nameof (MURKYBRINEICE));
    public static LocString DESC = (LocString) $"Polluted Brine Ice is a murky, concentrated solution of {UI.FormatAsLink("Salt", "SALT")} dissolved in {UI.FormatAsLink("Polluted Water", "DIRTYWATER")} and frozen into a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class MILKICE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Frozen Brackene", nameof (MILKICE));
    public static LocString DESC = (LocString) $"Frozen Brackene is {UI.FormatAsLink("Brackene", "MILK")} frozen into a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class BASALT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Basalt", nameof (BASALT));
    public static LocString DESC = (LocString) $"Basalt is a natural volcanic rock that forms when {UI.FormatAsLink("Magma", "MAGMA")} cools rapidly.";
  }

  public class BRINE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Brine", nameof (BRINE));
    public static LocString DESC = (LocString) $"Brine is a natural, highly concentrated solution of {UI.FormatAsLink("Salt", "SALT")} dissolved in {UI.FormatAsLink("Water", "WATER")}.\n\nIt can be used in desalination processes, separating out usable salt.";
  }

  public class MURKYBRINE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Polluted Brine", nameof (MURKYBRINE));
    public static LocString DESC = (LocString) $"Polluted Brine is a murky, concentrated solution of {UI.FormatAsLink("Salt", "SALT")} dissolved in {UI.FormatAsLink("Polluted Water", "DIRTYWATER")}.\n\nIt can be used in desalination processes, separating out usable salt.";
  }

  public class CARBON
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Coal", nameof (CARBON));
    public static LocString DESC = (LocString) $"(C) Coal is a combustible fossil fuel composed of carbon.\n\nIt is useful in {UI.FormatAsLink("Power", "POWER")} production.";
  }

  public class CORALLIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Corallium", nameof (CORALLIUM));
    public static LocString DESC = (LocString) $"Corallium is a dense coral primarily composed of (CaCO<sub>3</sub>) {UI.FormatAsLink("Lime", "LIME")}.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class COQUINA
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Coquina", nameof (COQUINA));
    public static LocString DESC = (LocString) $"Coquina is a brittle {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} mineral made up of the calcified remains of aquatic invertebrates.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class REFINEDCARBON
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Refined Carbon", nameof (REFINEDCARBON));
    public static LocString DESC = (LocString) $"(C) Refined carbon is solid element purified from raw {(string) ELEMENTS.CARBON.NAME}.";
  }

  public class PEAT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Peat", nameof (PEAT));
    public static LocString DESC = (LocString) $"Peat is a densely packed material made up of partially decomposed organic matter.\n\nIt is a combustible fuel, useful in {UI.FormatAsLink("Power", "POWER")} production.";
  }

  public class ETHANOLGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Ethanol Gas", nameof (ETHANOLGAS));
    public static LocString DESC = (LocString) $"(C<sub>2</sub>H<sub>6</sub>O) Ethanol Gas is an advanced chemical compound heated into a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class ETHANOL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Ethanol", nameof (ETHANOL));
    public static LocString DESC = (LocString) $"(C<sub>2</sub>H<sub>6</sub>O) Ethanol is an advanced chemical compound in a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.\n\nIt can be used as a highly effective fuel source when burned.";
  }

  public class SOLIDETHANOL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Ethanol", nameof (SOLIDETHANOL));
    public static LocString DESC = (LocString) "(C<sub>2</sub>H<sub>6</sub>O) Solid Ethanol is an advanced chemical compound.\n\nIt can be used as a highly effective fuel source when burned.";
  }

  public class CARBONDIOXIDE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Carbon Dioxide", nameof (CARBONDIOXIDE));
    public static LocString DESC = (LocString) $"(CO<sub>2</sub>) Carbon Dioxide is an atomically heavy chemical compound in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.\n\nIt tends to sink below other gases.";
  }

  public class CARBONFIBRE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Carbon Fiber", nameof (CARBONFIBRE));
    public static LocString DESC = (LocString) $"Carbon Fiber is a {UI.FormatAsLink("Manufactured Material", "REFINEDMINERAL")} with high tensile strength.";
  }

  public class CARBONGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Carbon Gas", nameof (CARBONGAS));
    public static LocString DESC = (LocString) $"(C) Carbon is an abundant, versatile element heated into a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class CHLORINE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Chlorine", nameof (CHLORINE));
    public static LocString DESC = (LocString) $"(Cl) Chlorine is a natural {UI.FormatAsLink("Germ", "DISEASE")}-killing element in a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class CHLORINEGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Chlorine Gas", nameof (CHLORINEGAS));
    public static LocString DESC = (LocString) $"(Cl) Chlorine is a natural {UI.FormatAsLink("Germ", "DISEASE")}-killing element in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class CLAY
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Clay", nameof (CLAY));
    public static LocString DESC = (LocString) $"Clay is a soft, naturally occurring composite of stone and soil that hardens at high {UI.FormatAsLink("Temperatures", "HEAT")}.\n\nIt is a reliable <b>Construction Material</b>.";
  }

  public class BRICK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Brick", nameof (BRICK));
    public static LocString DESC = (LocString) $"Brick is a hard, brittle material formed from heated {(string) ELEMENTS.CLAY.NAME}.\n\nIt is a reliable <b>Construction Material</b>.";
  }

  public class CERAMIC
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Ceramic", nameof (CERAMIC));
    public static LocString DESC = (LocString) $"Ceramic is a hard, brittle material formed from heated {(string) ELEMENTS.CLAY.NAME}.\n\nIt is a reliable <b>Construction Material</b>.";
  }

  public class CEMENT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Cement", nameof (CEMENT));
    public static LocString DESC = (LocString) "Cement is a refined building material used for assembling advanced buildings.";
  }

  public class CEMENTMIX
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Cement Mix", nameof (CEMENTMIX));
    public static LocString DESC = (LocString) $"Cement Mix can be used to create {(string) ELEMENTS.CEMENT.NAME} for advanced building assembly.";
  }

  public class CONTAMINATEDOXYGEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Polluted Oxygen", nameof (CONTAMINATEDOXYGEN));
    public static LocString DESC = (LocString) "(O<sub>2</sub>) Polluted Oxygen is dirty, unfiltered air.\n\nIt is breathable.";
  }

  public class COPPER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Copper", nameof (COPPER));
    public static LocString DESC = (LocString) $"(Cu) Copper is a conductive {UI.FormatAsLink("Metal", "METAL")}.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class COPPERGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Copper Gas", nameof (COPPERGAS));
    public static LocString DESC = (LocString) $"(Cu) Copper Gas is a conductive {UI.FormatAsLink("Metal", "METAL")} heated into a {UI.FormatAsLink("Gas", "ELEMENTS_GAS")} state.";
  }

  public class NICKELORE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Nickel Ore", nameof (NICKELORE));
    public static LocString DESC = (LocString) $"(Ni) Nickel Ore is a conductive {UI.FormatAsLink("Metal", "METAL")}.\n\nIt can be refined into {UI.FormatAsLink("Nickel", "NICKEL")} and is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class NICKEL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Nickel", nameof (NICKEL));
    public static LocString DESC = (LocString) $"(Ni) Nickel is a conductive {UI.FormatAsLink("Metal", "METAL")}.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class MOLTENNICKEL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Nickel", nameof (MOLTENNICKEL));
    public static LocString DESC = (LocString) $"(Ni) Molten Nickel is a conductive {UI.FormatAsLink("Metal", "METAL")} heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class NICKELGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Nickel Gas", nameof (NICKELGAS));
    public static LocString DESC = (LocString) $"(Ni) Nickel Gas is a conductive {UI.FormatAsLink("Metal", "METAL")} heated into a {UI.FormatAsLink("Gas", "ELEMENTS_GAS")} state.";
  }

  public class CREATURE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Genetic Ooze", nameof (CREATURE));
    public static LocString DESC = (LocString) "(DuPe) Ooze is a slurry of water, carbon, and dozens and dozens of trace elements.\n\nDuplicants are printed from pure Ooze.";
  }

  public class PHYTOOIL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Phyto Oil", nameof (PHYTOOIL));
    public static LocString DESC = (LocString) $"Phyto Oil is a thick, slippery {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} extracted from pureed {UI.FormatAsLink("Slime", "SLIME")}.";
  }

  public class REFINEDLIPID
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Biodiesel", nameof (REFINEDLIPID));
    public static LocString DESC = (LocString) $"Biodiesel is a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} composed of highly processed fatty acids derived from purified natural oils.\n\nIts {UI.FormatAsLink("combustibility", "COMBUSTIBLELIQUID")} makes it useful for {UI.FormatAsLink("Power", "POWER")} production.";
  }

  public class FROZENPHYTOOIL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Frozen Phyto Oil", nameof (FROZENPHYTOOIL));
    public static LocString DESC = (LocString) $"Frozen Phyto Oil is thick, slippery {UI.FormatAsLink("Slime", "SLIME")} extract, frozen into a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class CRUDEOIL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Crude Oil", nameof (CRUDEOIL));
    public static LocString DESC = (LocString) $"Crude Oil is a raw potential {UI.FormatAsLink("Power", "POWER")} source composed of billions of dead, primordial organisms.\n\nIt is also a useful lubricant for certain types of machinery.";
  }

  public class PETROLEUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Petroleum", nameof (PETROLEUM));
    public static LocString NAME_TWO = (LocString) UI.FormatAsLink("Petroleum", nameof (PETROLEUM));
    public static LocString DESC = (LocString) $"Petroleum is a {UI.FormatAsLink("Power", "POWER")} source refined from {UI.FormatAsLink("Crude Oil", "CRUDEOIL")}.\n\nIt is also an essential ingredient in the production of {UI.FormatAsLink("Plastic", "POLYPROPYLENE")}.";
  }

  public class SOURGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Sour Gas", nameof (SOURGAS));
    public static LocString NAME_TWO = (LocString) UI.FormatAsLink("Sour Gas", nameof (SOURGAS));
    public static LocString DESC = (LocString) $"Sour Gas is a hydrocarbon {UI.FormatAsLink("Gas", "ELEMENTS_GAS")} containing high concentrations of hydrogen sulfide.\n\nIt is a byproduct of highly heated {UI.FormatAsLink("Petroleum", "PETROLEUM")}.";
  }

  public class CRUSHEDICE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Crushed Ice", nameof (CRUSHEDICE));
    public static LocString DESC = (LocString) "(H<sub>2</sub>O) A slush of crushed, semi-solid ice.";
  }

  public class CRUSHEDROCK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Crushed Rock", nameof (CRUSHEDROCK));
    public static LocString DESC = (LocString) $"Crushed Rock is {UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK")} crushed into a mechanical mixture.";
  }

  public class CUPRITE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Copper Ore", nameof (CUPRITE));
    public static LocString DESC = (LocString) $"(Cu<sub>2</sub>O) Copper Ore is a conductive {UI.FormatAsLink("Metal", "METAL")}.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class DEPLETEDURANIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Depleted Uranium", nameof (DEPLETEDURANIUM));
    public static LocString DESC = (LocString) $"(U) Depleted Uranium is {UI.FormatAsLink("Uranium", "URANIUMORE")} with a low U-235 content.\n\nIt is created as a byproduct of {UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM")} and is no longer suitable as fuel.";
  }

  public class DIAMOND
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Diamond", nameof (DIAMOND));
    public static LocString DESC = (LocString) "(C) Diamond is industrial-grade, high density carbon.\n\nIt is very difficult to excavate.";
  }

  public class DIRT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Dirt", nameof (DIRT));
    public static LocString DESC = (LocString) $"Dirt is a soft, nutrient-rich substance capable of supporting life.\n\nIt is necessary in some forms of {UI.FormatAsLink("Food", "FOOD")} production.";
  }

  public class DIRTYICE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Polluted Ice", nameof (DIRTYICE));
    public static LocString DESC = (LocString) $"Polluted Ice is dirty, unfiltered water frozen into a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class DIRTYWATER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Polluted Water", nameof (DIRTYWATER));
    public static LocString DESC = (LocString) $"Polluted Water is dirty, unfiltered {UI.FormatAsLink("Water", "WATER")}.\n\nIt is not fit for consumption.";
  }

  public class ELECTRUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Electrum", nameof (ELECTRUM));
    public static LocString DESC = (LocString) $"Electrum is a conductive {UI.FormatAsLink("Metal", "METAL")} alloy composed of gold and silver.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class ENRICHEDURANIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Enriched Uranium", nameof (ENRICHEDURANIUM));
    public static LocString DESC = (LocString) $"(U) Enriched Uranium is a refined substance primarily used to {UI.FormatAsLink("Power", "POWER")} potent research reactors.\n\nIt becomes highly {UI.FormatAsLink("Radioactive", "RADIATION")} when consumed.";
  }

  public class FERTILIZER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Fertilizer", nameof (FERTILIZER));
    public static LocString DESC = (LocString) $"Fertilizer is a processed mixture of biological nutrients.\n\nIt aids in the growth of certain {UI.FormatAsLink("Plants", "PLANTS")}.";
  }

  public class PONDSCUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Pondscum", nameof (PONDSCUM));
    public static LocString DESC = (LocString) $"Pondscum is a soft, naturally occurring composite of biological nutrients.\n\nIt may be processed into {UI.FormatAsLink("Fertilizer", "FERTILIZER")} and aids in the growth of certain {UI.FormatAsLink("Plants", "PLANTS")}.";
  }

  public class FALLOUT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Nuclear Fallout", nameof (FALLOUT));
    public static LocString DESC = (LocString) $"Nuclear Fallout is a highly toxic gas full of {UI.FormatAsLink("Radioactive Contaminants", "RADIATION")}. Condenses into {UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE")}.";
  }

  public class FOOLSGOLD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Pyrite", nameof (FOOLSGOLD));
    public static LocString DESC = (LocString) $"(FeS<sub>2</sub>) Pyrite is a conductive {UI.FormatAsLink("Metal", "METAL")}.\n\nAlso known as \"Fool's Gold\", is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class FULLERENE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Fullerene", nameof (FULLERENE));
    public static LocString DESC = (LocString) $"(C<sub>60</sub>) Fullerene is a form of {UI.FormatAsLink("Coal", "CARBON")} consisting of spherical molecules.";
  }

  public class GALENA
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Galena", nameof (GALENA));
    public static LocString DESC = (LocString) $"(PbS) Galena is a raw {UI.FormatAsLink("Metal Ore", "METAL")} primarily composed of {UI.FormatAsLink("Lead", "LEAD")} with {UI.FormatAsLink("Sulfur", "SULFUR")} impurities.";
  }

  public class GLASS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Glass", nameof (GLASS));
    public static LocString DESC = (LocString) $"Glass is a brittle, transparent substance formed from {UI.FormatAsLink("Sand", "SAND")} fired at high temperatures.";
  }

  public class GOLD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Gold", nameof (GOLD));
    public static LocString DESC = (LocString) $"(Au) Gold is a conductive precious {UI.FormatAsLink("Metal", "REFINEDMETAL")}.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class GOLDAMALGAM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Gold Amalgam", nameof (GOLDAMALGAM));
    public static LocString DESC = (LocString) $"Gold Amalgam is a conductive amalgam of gold and mercury.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class GOLDGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Gold Gas", nameof (GOLDGAS));
    public static LocString DESC = (LocString) $"(Au) Gold Gas is a conductive precious {UI.FormatAsLink("Metal", "REFINEDMETAL")}, heated into a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class GRANITE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Granite", nameof (GRANITE));
    public static LocString DESC = (LocString) $"Granite is a dense composite of {UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK")}.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class GRAPHITE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Graphite", nameof (GRAPHITE));
    public static LocString DESC = (LocString) "(C) Graphite is the most stable form of carbon.\n\nIt has high thermal conductivity and is useful as a <b>Construction Material</b>.";
  }

  public class LIQUIDGUNK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Gunk", nameof (LIQUIDGUNK));
    public static LocString DESC = (LocString) "Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms.\n\nIt is unpleasantly viscous.";
  }

  public class GUNK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Gunk", nameof (GUNK));
    public static LocString DESC = (LocString) $"Solid Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms, which has been frozen into a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class LATEX
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Latex", nameof (LATEX));
    public static LocString DESC = (LocString) $"Latex is a naturally derived colloidal dispersion of polymer particles in {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")}.\n\nIt can be used to produce {UI.FormatAsLink("Rubber", "RUBBER")}.";
  }

  public class SOLIDNUCLEARWASTE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Nuclear Waste", nameof (SOLIDNUCLEARWASTE));
    public static LocString DESC = (LocString) $"Highly toxic solid full of {UI.FormatAsLink("Radioactive Contaminants", "RADIATION")}.";
  }

  public class HELIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Helium", nameof (HELIUM));
    public static LocString DESC = (LocString) $"(He) Helium is an atomically lightweight, chemical {UI.FormatAsLink("Gas", "ELEMENTS_GAS")}.";
  }

  public class HYDROGEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Hydrogen Gas", nameof (HYDROGEN));
    public static LocString DESC = (LocString) $"(H) Hydrogen Gas is the universe's most common and atomically light element in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class ICE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Ice", nameof (ICE));
    public static LocString DESC = (LocString) $"(H<sub>2</sub>O) Ice is clean water frozen into a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class INK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Squid Ink", nameof (INK));
    public static LocString DESC = (LocString) $"Squid Ink is a heavily pigmented organic {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} composed of {UI.FormatAsLink("Mucin", "MUCUS")}, melanin, and {UI.FormatAsLink("Refined Carbon", "REFINEDCARBON")}.\n\nIt possesses useful {UI.FormatAsLink("Radiation", "RADIATION")} absorption properties.";
  }

  public class FROZENINK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Frozen Squid Ink", nameof (FROZENINK));
    public static LocString DESC = (LocString) $"Frozen Ink is {UI.FormatAsLink("Squid Ink", "INK")} cooled to a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class IGNEOUSROCK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Igneous Rock", nameof (IGNEOUSROCK));
    public static LocString DESC = (LocString) "Igneous Rock is a composite of solidified volcanic rock.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class IRIDIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Iridium", nameof (IRIDIUM));
    public static LocString DESC = (LocString) $"(Ir) Iridium is a firm and highly conductive {UI.FormatAsLink("Metal", "METAL")} that can withstand extreme  {UI.FormatAsLink("Heat", "HEAT")}.";
  }

  public class MOLTENIRIDIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Iridium", nameof (MOLTENIRIDIUM));
    public static LocString DESC = (LocString) $"(Ir) Molten Iridium is a highly conductive {UI.FormatAsLink("Metal", "METAL")} heated to a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class IRIDIUMGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Iridium Gas", nameof (IRIDIUMGAS));
    public static LocString DESC = (LocString) $"(Ir) Iridium Gas is a highly conductive {UI.FormatAsLink("Metal", "METAL")} heated into a  {UI.FormatAsLink("Gas", "ELEMENTS_GAS")} state.";
  }

  public class AMBER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Amber", nameof (AMBER));
    public static LocString DESC = (LocString) $"Amber is a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} organic composite of {UI.FormatAsLink("Resin", "NATURALRESIN")} and {UI.FormatAsLink("Fossil", "FOSSIL")}.";
  }

  public class NATURALRESIN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Resin", nameof (NATURALRESIN));
    public static LocString DESC = (LocString) $"Resin is a viscous organic {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")}.\n\nIt can be treated to become {UI.FormatAsLink("Plastic", "POLYPROPYLENE")} in the {UI.FormatAsLink("Polymer Press", "POLYMERIZER")}.";
  }

  public class NATURALSOLIDRESIN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Resin", nameof (NATURALSOLIDRESIN));
    public static LocString DESC = (LocString) $"Resin that has been cooled to a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.\n\nIt can be treated to become {UI.FormatAsLink("Plastic", "POLYPROPYLENE")} in the {UI.FormatAsLink("Polymer Press", "POLYMERIZER")}.";
  }

  public class ISORESIN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Isosap", nameof (ISORESIN));
    public static LocString DESC = (LocString) "Isosap is a crystallized sap composed of long-chain polymers.\n\nIt is used in the production of rare, high grade materials.";
  }

  public class RESIN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Sap", nameof (RESIN));
    public static LocString DESC = (LocString) $"Sticky goo harvested from a grumpy tree.\n\nIt can be polymerized into {UI.FormatAsLink("Isosap", "ISORESIN")} by boiling away its excess moisture.";
  }

  public class SOLIDRESIN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Sap", nameof (SOLIDRESIN));
    public static LocString DESC = (LocString) $"Solidified goo harvested from a grumpy tree.\n\nIt is used in the production of {UI.FormatAsLink("Isosap", "ISORESIN")}.";
  }

  public class IRON
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Iron", nameof (IRON));
    public static LocString DESC = (LocString) $"(Fe) Iron is a common industrial {UI.FormatAsLink("Metal", "REFINEDMETAL")}.";
  }

  public class IRONGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Iron Gas", nameof (IRONGAS));
    public static LocString DESC = (LocString) $"(Fe) Iron Gas is a common industrial {UI.FormatAsLink("Metal", "METAL")}, heated into a {UI.FormatAsLink("Gas", "ELEMENTS_GAS")}.";
  }

  public class IRONORE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Iron Ore", nameof (IRONORE));
    public static LocString DESC = (LocString) $"(Fe) Iron Ore is a soft {UI.FormatAsLink("Metal", "METAL")}.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class COBALTGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Cobalt Gas", nameof (COBALTGAS));
    public static LocString DESC = (LocString) $"(Co) Cobalt is a {UI.FormatAsLink("Refined Metal", "REFINEDMETAL")}, heated into a {UI.FormatAsLink("Gas", "ELEMENTS_GAS")}.";
  }

  public class COBALT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Cobalt", nameof (COBALT));
    public static LocString DESC = (LocString) $"(Co) Cobalt is a {UI.FormatAsLink("Refined Metal", "REFINEDMETAL")} made from {UI.FormatAsLink("Cobalt Ore", "COBALTITE")}.";
  }

  public class COBALTITE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Cobalt Ore", nameof (COBALTITE));
    public static LocString DESC = (LocString) $"(Co) Cobalt Ore is a blue-hued {UI.FormatAsLink("Metal", "BUILDINGMATERIALCLASSES")}.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class KATAIRITE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Abyssalite", nameof (KATAIRITE));
    public static LocString DESC = (LocString) "(Ab) Abyssalite is a resilient, crystalline element.";
  }

  public class LIME
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Lime", nameof (LIME));
    public static LocString DESC = (LocString) $"(CaCO<sub>3</sub>) Lime is a mineral commonly found in {UI.FormatAsLink("Critter", "CREATURES")} egg shells.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class FOSSIL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Fossil", nameof (FOSSIL));
    public static LocString DESC = (LocString) "Fossil is organic matter, highly compressed and hardened into a mineral state.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class LEADGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Lead Gas", nameof (LEADGAS));
    public static LocString DESC = (LocString) $"(Pb) Lead Gas is a soft yet extremely dense {UI.FormatAsLink("Refined Metal", "REFINEDMETAL")} heated into a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")}.";
  }

  public class LEAD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Lead", nameof (LEAD));
    public static LocString DESC = (LocString) $"(Pb) Lead is a soft yet extremely dense {UI.FormatAsLink("Refined Metal", "REFINEDMETAL")}.\n\nIt has a low Overheat Temperature and is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class LIQUIDCARBONDIOXIDE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Carbon Dioxide", nameof (LIQUIDCARBONDIOXIDE));
    public static LocString DESC = (LocString) $"(CO<sub>2</sub>) Carbon Dioxide is an unbreathable chemical compound.\n\nThis selection is currently in a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class LIQUIDHELIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Helium", nameof (LIQUIDHELIUM));
    public static LocString DESC = (LocString) $"(He) Helium is an atomically lightweight chemical element cooled into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class LIQUIDHYDROGEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Hydrogen", nameof (LIQUIDHYDROGEN));
    public static LocString DESC = (LocString) $"(H) Hydrogen in its {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.\n\nIt freezes most substances that come into contact with it.";
  }

  public class LIQUIDOXYGEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Oxygen", nameof (LIQUIDOXYGEN));
    public static LocString DESC = (LocString) $"(O<sub>2</sub>) Oxygen is a breathable chemical.\n\nThis selection is in a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class LIQUIDMETHANE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Methane", nameof (LIQUIDMETHANE));
    public static LocString DESC = (LocString) $"(CH<sub>4</sub>) Methane is an alkane.\n\nThis selection is in a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class LIQUIDPHOSPHORUS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Phosphorus", nameof (LIQUIDPHOSPHORUS));
    public static LocString DESC = (LocString) $"(P) Phosphorus is a chemical element.\n\nThis selection is in a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class LIQUIDPROPANE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Propane", nameof (LIQUIDPROPANE));
    public static LocString DESC = (LocString) $"(C<sub>3</sub>H<sub>8</sub>) Propane is an alkane.\n\nThis selection is in a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.\n\nIt is useful in {UI.FormatAsLink("Power", "POWER")} production.";
  }

  public class LIQUIDSULFUR
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Sulfur", nameof (LIQUIDSULFUR));
    public static LocString DESC = (LocString) $"(S) Sulfur is a common chemical element and byproduct of {UI.FormatAsLink("Natural Gas", "METHANE")} production.\n\nThis selection is in a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MAFICROCK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Mafic Rock", nameof (MAFICROCK));
    public static LocString DESC = (LocString) $"Mafic Rock is a variation of {UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK")} that is rich in {UI.FormatAsLink("Iron", "IRON")}.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class MAGMA
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Magma", nameof (MAGMA));
    public static LocString DESC = (LocString) $"Magma is a composite of {UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK")} heated into a molten, {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class WOODLOG
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Wood", "WOOD");
    public static LocString DESC = (LocString) $"Wood is a good source of {UI.FormatAsLink("Heat", "HEAT")} and {UI.FormatAsLink("Power", "POWER")}.\n\nIts insulation properties and positive {UI.FormatAsLink("Decor", "DECOR")} also make it a useful <b>Construction Material</b>.";
  }

  public class FABRICATEDWOOD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Plywood", nameof (FABRICATEDWOOD));
    public static LocString DESC = (LocString) $"Plywood is a good source of {UI.FormatAsLink("Heat", "HEAT")} and {UI.FormatAsLink("Power", "POWER")}.\n\nIts insulation properties make it a useful <b>Construction Material</b>.";
  }

  public class CINNABAR
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Cinnabar Ore", nameof (CINNABAR));
    public static LocString DESC = (LocString) $"(HgS) Cinnabar Ore, also known as mercury sulfide, is a conductive {UI.FormatAsLink("Metal", "METAL")} that can be refined into {UI.FormatAsLink("Mercury", "MERCURY")}.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class TALLOW
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Tallow", nameof (TALLOW));
    public static LocString DESC = (LocString) $"A chunk of raw grease that can be used in {UI.FormatAsLink("Food", "FOOD")} production or industrial processes.";
  }

  public class MERCURY
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Mercury", nameof (MERCURY));
    public static LocString DESC = (LocString) $"(Hg) Mercury is a metallic {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")}.";
  }

  public class MERCURYGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Mercury Gas", nameof (MERCURYGAS));
    public static LocString DESC = (LocString) $"(Hg) Mercury Gas is a {UI.FormatAsLink("Metal", "REFINEDMETAL")} heated into a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class METHANE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Natural Gas", nameof (METHANE));
    public static LocString DESC = (LocString) $"Natural Gas is a mixture of various alkanes in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.\n\nIt is useful in {UI.FormatAsLink("Power", "POWER")} production.";
  }

  public class MILK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Brackene", nameof (MILK));
    public static LocString DESC = (LocString) $"Brackene is a sodium-rich {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")}.\n\nIt is useful in {UI.FormatAsLink("Ranching", "RANCHING")}.";
  }

  public class MILKFAT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Brackwax", nameof (MILKFAT));
    public static LocString DESC = (LocString) $"Brackwax is a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} byproduct of {UI.FormatAsLink("Brackene", "MILK")}.";
  }

  public class FISHMILK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Ovolene", nameof (FISHMILK));
    public static LocString DESC = (LocString) $"Ovolene is a protein-rich {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} produced by aquatic critters.\n\nIt can be processed into {UI.FormatAsLink("Caviar", "CAVIAR")} and {UI.FormatAsLink("Mucin", "MUCUS")}.";
  }

  public class FISHMILKICE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Frozen Ovolene", nameof (FISHMILKICE));
    public static LocString DESC = (LocString) $"Frozen Ovolene is {UI.FormatAsLink("Ovolene", "FISHMILK")} frozen into a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class MOLTENCARBON
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Carbon", nameof (MOLTENCARBON));
    public static LocString DESC = (LocString) $"(C) Liquid Carbon is an abundant, versatile element heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENCOPPER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Copper", nameof (MOLTENCOPPER));
    public static LocString DESC = (LocString) $"(Cu) Molten Copper is a conductive {UI.FormatAsLink("Metal", "METAL")} heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENGLASS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Glass", nameof (MOLTENGLASS));
    public static LocString DESC = (LocString) $"Molten Glass is a composite of granular rock, heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENGOLD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Gold", nameof (MOLTENGOLD));
    public static LocString DESC = (LocString) $"(Au) Gold, a conductive precious {UI.FormatAsLink("Metal", "REFINEDMETAL")}, heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENIRON
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Iron", nameof (MOLTENIRON));
    public static LocString DESC = (LocString) $"(Fe) Molten Iron is a common industrial {UI.FormatAsLink("Metal", "METAL")} heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENCOBALT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Cobalt", nameof (MOLTENCOBALT));
    public static LocString DESC = (LocString) $"(Co) Molten Cobalt is a {UI.FormatAsLink("Refined Metal", "REFINEDMETAL")} heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENLEAD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Lead", nameof (MOLTENLEAD));
    public static LocString DESC = (LocString) $"(Pb) Lead is an extremely dense {UI.FormatAsLink("Refined Metal", "REFINEDMETAL")} heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENNIOBIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Niobium", nameof (MOLTENNIOBIUM));
    public static LocString DESC = (LocString) $"(Nb) Molten Niobium is a rare metal heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENTUNGSTEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Tungsten", nameof (MOLTENTUNGSTEN));
    public static LocString DESC = (LocString) $"(W) Molten Tungsten is a crystalline {UI.FormatAsLink("Metal", "REFINEDMETAL")} heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENTUNGSTENDISELENIDE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Tungsten Diselenide", nameof (MOLTENTUNGSTENDISELENIDE));
    public static LocString DESC = (LocString) $"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic {UI.FormatAsLink("Metal", "METAL")} compound heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENSTEEL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Steel", nameof (MOLTENSTEEL));
    public static LocString DESC = (LocString) $"Molten Steel is a {UI.FormatAsLink("Metal", "METAL")} alloy of iron and carbon, heated into a hazardous {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MOLTENURANIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Uranium", nameof (MOLTENURANIUM));
    public static LocString DESC = (LocString) $"(U) Liquid Uranium is a highly {UI.FormatAsLink("Radioactive", "RADIATION")} substance, heated into a hazardous {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.\n\nIt is a byproduct of {UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM")}.";
  }

  public class MOLTENZINC
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Zinc", nameof (MOLTENZINC));
    public static LocString DESC = (LocString) $"Molten Zinc is {UI.FormatAsLink("Zinc", "ZINC")} heated into {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class MUCUS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Mucin", nameof (MUCUS));
    public static LocString DESC = (LocString) $"Mucin is an extremely viscous {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} gel composed of natural enzymes and polysaccharides.\n\nIts unusually thick consistency can result in formations atypical of most liquids.";
  }

  public class SOLIDMUCUS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Frozen Mucin", nameof (SOLIDMUCUS));
    public static LocString DESC = (LocString) $"Frozen Mucin is a chunk of {UI.FormatAsLink("Mucin", "MUCUS")} cooled to a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class NIOBIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Niobium", nameof (NIOBIUM));
    public static LocString DESC = (LocString) $"(Nb) Niobium is a rare metal with many practical applications in metallurgy and superconductor {UI.FormatAsLink("Research", "RESEARCH")}.";
  }

  public class NIOBIUMGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Niobium Gas", nameof (NIOBIUMGAS));
    public static LocString DESC = (LocString) $"(Nb) Niobium Gas is a rare metal.\n\nThis selection is in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class NUCLEARWASTE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Nuclear Waste", nameof (NUCLEARWASTE));
    public static LocString DESC = (LocString) $"Highly toxic liquid full of {UI.FormatAsLink("Radioactive Contaminants", "RADIATION")} which emit {UI.FormatAsLink("Radiation", "RADIATION")} that can be absorbed by {UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER")}.";
  }

  public class OBSIDIAN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Obsidian", nameof (OBSIDIAN));
    public static LocString DESC = (LocString) $"Obsidian is a brittle composite of volcanic {UI.FormatAsLink("Glass", "GLASS")}.";
  }

  public class OXYGEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Oxygen", nameof (OXYGEN));
    public static LocString DESC = (LocString) $"(O<sub>2</sub>) Oxygen is an atomically lightweight and breathable {UI.FormatAsLink("Gas", "ELEMENTS_GAS")}, necessary for sustaining life.\n\nIt tends to rise above other gases.";
  }

  public class OXYROCK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Oxylite", nameof (OXYROCK));
    public static LocString DESC = (LocString) $"(Ir<sub>3</sub>O<sub>2</sub>) Oxylite is a chemical compound that slowly emits breathable {UI.FormatAsLink("Oxygen", "OXYGEN")}.\n\nExcavating {(string) ELEMENTS.OXYROCK.NAME} increases its emission rate, but depletes the ore more rapidly.";
  }

  public class PHOSPHATENODULES
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Phosphate Nodules", nameof (PHOSPHATENODULES));
    public static LocString DESC = (LocString) "(PO<sup>3-</sup><sub>4</sub>) Nodules of sedimentary rock containing high concentrations of phosphate.";
  }

  public class PHOSPHORITE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Phosphorite", nameof (PHOSPHORITE));
    public static LocString DESC = (LocString) "Phosphorite is a composite of sedimentary rock, saturated with phosphate.";
  }

  public class PHOSPHORUS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Refined Phosphorus", nameof (PHOSPHORUS));
    public static LocString DESC = (LocString) $"(P) Refined Phosphorus is a chemical element in its {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class PHOSPHORUSGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Phosphorus Gas", nameof (PHOSPHORUSGAS));
    public static LocString DESC = (LocString) $"(P) Phosphorus Gas is the {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state of {UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS")}.";
  }

  public class PROPANE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Propane Gas", nameof (PROPANE));
    public static LocString DESC = (LocString) $"(C<sub>3</sub>H<sub>8</sub>) Propane Gas is a natural alkane.\n\nThis selection is in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.\n\nIt is useful in {UI.FormatAsLink("Power", "POWER")} production.";
  }

  public class RADIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Radium", nameof (RADIUM));
    public static LocString DESC = (LocString) $"(Ra) Radium is a {UI.FormatAsLink("Light", "LIGHT")} emitting radioactive substance.\n\nIt is useful as a {UI.FormatAsLink("Power", "POWER")} source.";
  }

  public class YELLOWCAKE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Yellowcake", nameof (YELLOWCAKE));
    public static LocString DESC = (LocString) $"(U<sub>3</sub>O<sub>8</sub>) Yellowcake is a byproduct of {UI.FormatAsLink("Uranium", "URANIUM")} mining.\n\nIt is useful in preparing fuel for {UI.FormatAsLink("Research Reactors", "NUCLEARREACTOR")}.\n\nNote: Do not eat.";
  }

  public class ROCKGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Rock Gas", nameof (ROCKGAS));
    public static LocString DESC = (LocString) $"Rock Gas is rock that has been superheated into a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class RUBBER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Rubber", nameof (RUBBER));
    public static LocString DESC = (LocString) $"Rubber is a viscoelastic {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} made from vulcanized {UI.FormatAsLink("Latex", "LATEX")}.\n\nIt can be used to make {UI.FormatAsLink("Gaskets", "BUILDINGGASKET")}.";
  }

  public class RUST
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Rust", nameof (RUST));
    public static LocString DESC = (LocString) $"Rust is an iron oxide that forms from the breakdown of {UI.FormatAsLink("Iron", "IRON")}.\n\nIt is useful in some {UI.FormatAsLink("Oxygen", "OXYGEN")} production processes.";
  }

  public class REGOLITH
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Regolith", nameof (REGOLITH));
    public static LocString DESC = (LocString) $"Regolith is a sandy substance composed of the various particles that collect atop terrestrial objects.\n\nIt is useful as a {UI.FormatAsLink("Filtration Medium", "FILTER")}.";
  }

  public class SALTGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Salt Gas", nameof (SALTGAS));
    public static LocString DESC = (LocString) $"(NaCl) Salt Gas is an edible chemical compound that has been superheated into a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class MOLTENSALT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Salt", nameof (MOLTENSALT));
    public static LocString DESC = (LocString) $"(NaCl) Molten Salt is an edible chemical compound that has been heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class SALT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Salt", nameof (SALT));
    public static LocString DESC = (LocString) $"(NaCl) Salt, also known as sodium chloride, is an edible chemical compound.\n\nWhen refined, it can be eaten with meals to increase Duplicant {UI.FormatAsLink("Morale", "MORALE")}.";
  }

  public class SALTWATER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Salt Water", nameof (SALTWATER));
    public static LocString DESC = (LocString) $"Salt Water is a natural, lightly concentrated solution of {UI.FormatAsLink("Salt", "SALT")} dissolved in {UI.FormatAsLink("Water", "WATER")}.\n\nIt can be used in desalination processes, separating out usable salt.";
  }

  public class SAND
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Sand", nameof (SAND));
    public static LocString DESC = (LocString) $"Sand is a composite of granular rock.\n\nIt is useful as a {UI.FormatAsLink("Filtration Medium", "FILTER")}.";
  }

  public class SANDCEMENT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Sand Cement", nameof (SANDCEMENT));
    public static LocString DESC = (LocString) "";
  }

  public class SANDSTONE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Sandstone", nameof (SANDSTONE));
    public static LocString DESC = (LocString) "Sandstone is a composite of relatively soft sedimentary rock.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class SEDIMENTARYROCK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Sedimentary Rock", nameof (SEDIMENTARYROCK));
    public static LocString DESC = (LocString) "Sedimentary Rock is a hardened composite of sediment layers.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class SHALE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Shale", nameof (SHALE));
    public static LocString DESC = (LocString) "Shale is a brittle composite of sediment layers.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class SILTSTONE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Siltstone", nameof (SILTSTONE));
    public static LocString DESC = (LocString) "Siltstone is a chalky mudrock made primarily of compressed silt.\n\nIt is useful as a <b>Construction Material</b>.";
  }

  public class PALMWOOD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Gum Wood", nameof (PALMWOOD));
    public static LocString DESC = (LocString) $"Gum Wood can be processed at the {UI.FormatAsLink("Plant Pulverizer", "MILKPRESS")} to yield {UI.FormatAsLink("Latex", "LATEX")}, which is essential for {UI.FormatAsLink("Rubber", "RUBBER")} production.\n\nIt can also be used for {UI.FormatAsLink("Heat", "HEAT")} and {UI.FormatAsLink("Power", "POWER")}, or as a <b>Construction Material</b>.";
  }

  public class PEARL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Pearl", nameof (PEARL));
    public static LocString DESC = (LocString) $"A perfectly smooth gem.\n\nIt can be eaten by {(string) CREATURES.FAMILY_PLURAL.SEAHORSESPECIES} or used as an attractive <b>Construction Material</b>.";
  }

  public class SLIMEMOLD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Slime", nameof (SLIMEMOLD));
    public static LocString DESC = (LocString) $"Slime is a thick biomixture of algae, fungi, and mucopolysaccharides.\n\nIt can be distilled into {UI.FormatAsLink("Algae", "ALGAE")} and emits {(string) ELEMENTS.CONTAMINATEDOXYGEN.NAME} once dug up.";
  }

  public class SNOW
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Snow", nameof (SNOW));
    public static LocString DESC = (LocString) $"(H<sub>2</sub>O) Snow is a mass of loose, crystalline ice particles.\n\nIt becomes {UI.FormatAsLink("Water", "WATER")} when melted.";
  }

  public class STABLESNOW
  {
    public static LocString NAME = (LocString) ("Packed " + (string) ELEMENTS.SNOW.NAME);
    public static LocString DESC = ELEMENTS.SNOW.DESC;
  }

  public class SOLIDCARBONDIOXIDE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Carbon Dioxide", nameof (SOLIDCARBONDIOXIDE));
    public static LocString DESC = (LocString) $"(CO<sub>2</sub>) Carbon Dioxide is an unbreathable compound in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SOLIDCHLORINE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Chlorine", nameof (SOLIDCHLORINE));
    public static LocString DESC = (LocString) $"(Cl) Chlorine is a natural {UI.FormatAsLink("Germ", "DISEASE")}-killing element in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SOLIDCRUDEOIL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Crude Oil", nameof (SOLIDCRUDEOIL));
    public static LocString DESC = (LocString) "";
  }

  public class SOLIDHYDROGEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Hydrogen", nameof (SOLIDHYDROGEN));
    public static LocString DESC = (LocString) $"(H) Solid Hydrogen is the universe's most common element in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SOLIDMERCURY
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Mercury", nameof (SOLIDMERCURY));
    public static LocString DESC = (LocString) $"(Hg) Mercury is a rare {UI.FormatAsLink("Metal", "REFINEDMETAL")} in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SOLIDOXYGEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Oxygen", nameof (SOLIDOXYGEN));
    public static LocString DESC = (LocString) $"(O<sub>2</sub>) Solid Oxygen is a breathable element in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SOLIDMETHANE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Methane", nameof (SOLIDMETHANE));
    public static LocString DESC = (LocString) $"(CH<sub>4</sub>) Methane is an alkane in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SOLIDNAPHTHA
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Naphtha", nameof (SOLIDNAPHTHA));
    public static LocString DESC = (LocString) $"Naphtha is a distilled hydrocarbon mixture in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class CORIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Corium", nameof (CORIUM));
    public static LocString DESC = (LocString) $"A radioactive mixture of nuclear waste and melted reactor materials.\n\nReleases {UI.FormatAsLink("Nuclear Fallout", "FALLOUT")} gas.";
  }

  public class SOLIDPETROLEUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Petroleum", nameof (SOLIDPETROLEUM));
    public static LocString DESC = (LocString) $"Petroleum is a {UI.FormatAsLink("Power", "POWER")} source.\n\nThis selection is in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SOLIDPROPANE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Propane", nameof (SOLIDPROPANE));
    public static LocString DESC = (LocString) $"(C<sub>3</sub>H<sub>8</sub>) Solid Propane is a natural gas in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SOLIDSUPERCOOLANT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Super Coolant", nameof (SOLIDSUPERCOOLANT));
    public static LocString DESC = (LocString) $"Super Coolant is an industrial-grade {UI.FormatAsLink("Fullerene", "FULLERENE")} coolant.\n\nThis selection is in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SOLIDVISCOGEL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Visco-Gel", nameof (SOLIDVISCOGEL));
    public static LocString DESC = (LocString) $"Visco-Gel is a polymer that has high surface tension when in {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} form.\n\nThis selection is in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SYNGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Synthesis Gas", nameof (SYNGAS));
    public static LocString DESC = (LocString) $"Synthesis Gas is an artificial, unbreathable {UI.FormatAsLink("Gas", "ELEMENTS_GAS")}.\n\nIt can be converted into an efficient fuel.";
  }

  public class MOLTENSYNGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Molten Synthesis Gas", "SYNGAS");
    public static LocString DESC = (LocString) $"Molten Synthesis Gas is an artificial, unbreathable {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")}.\n\nIt can be converted into an efficient fuel.";
  }

  public class SOLIDSYNGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Solid Synthesis Gas", "SYNGAS");
    public static LocString DESC = (LocString) $"Solid Synthesis Gas is an artificial, unbreathable {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")}.\n\nIt can be converted into an efficient fuel.";
  }

  public class STEAM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Steam", nameof (STEAM));
    public static LocString DESC = (LocString) $"(H<sub>2</sub>O) Steam is {(string) ELEMENTS.WATER.NAME} that has been heated into a scalding {UI.FormatAsLink("Gas", "ELEMENTS_GAS")}.";
  }

  public class STEEL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Steel", nameof (STEEL));
    public static LocString DESC = (LocString) $"Steel is a {UI.FormatAsLink("Metal Alloy", "REFINEDMETAL")} composed of iron and carbon.";
  }

  public class STEELGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Steel Gas", nameof (STEELGAS));
    public static LocString DESC = (LocString) $"Steel Gas is a superheated {UI.FormatAsLink("Metal", "METAL")} {UI.FormatAsLink("Gas", "ELEMENTS_GAS")} composed of iron and carbon.";
  }

  public class SUGARWATER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Nectar", nameof (SUGARWATER));
    public static LocString DESC = (LocString) $"Nectar is a natural, lightly concentrated solution of {UI.FormatAsLink("Sucrose", "SUCROSE")} dissolved in {UI.FormatAsLink("Water", "WATER")}.";
  }

  public class SULFUR
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Sulfur", nameof (SULFUR));
    public static LocString DESC = (LocString) $"(S) Sulfur is a common chemical element and byproduct of {UI.FormatAsLink("Natural Gas", "METHANE")} production.\n\nThis selection is in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.";
  }

  public class SULFURGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Sulfur Gas", nameof (SULFURGAS));
    public static LocString DESC = (LocString) $"(S) Sulfur is a common chemical element and byproduct of {UI.FormatAsLink("Natural Gas", "METHANE")} production.\n\nThis selection is in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class SUPERCOOLANT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Super Coolant", nameof (SUPERCOOLANT));
    public static LocString DESC = (LocString) $"Super Coolant is an industrial-grade coolant that utilizes the unusual energy states of {UI.FormatAsLink("Fullerene", "FULLERENE")}.\n\nThis selection is in a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class SUPERCOOLANTGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Super Coolant Gas", nameof (SUPERCOOLANTGAS));
    public static LocString DESC = (LocString) $"Super Coolant is an industrial-grade {UI.FormatAsLink("Fullerene", "FULLERENE")} coolant.\n\nThis selection is in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class SUPERINSULATOR
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Insulite", nameof (SUPERINSULATOR));
    public static LocString DESC = (LocString) $"Insulite reduces {UI.FormatAsLink("Heat Transfer", "HEAT")} and is composed of recrystallized {UI.FormatAsLink("Abyssalite", "KATAIRITE")}.";
  }

  public class TEMPCONDUCTORSOLID
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Thermium", nameof (TEMPCONDUCTORSOLID));
    public static LocString DESC = (LocString) $"Thermium is an industrial metal alloy formulated to maximize {UI.FormatAsLink("Heat Transfer", "HEAT")} and thermal dispersion.";
  }

  public class TUNGSTEN
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Tungsten", nameof (TUNGSTEN));
    public static LocString DESC = (LocString) $"(W) Tungsten is an extremely tough crystalline {UI.FormatAsLink("Metal", "REFINEDMETAL")}.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class TUNGSTENGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Tungsten Gas", nameof (TUNGSTENGAS));
    public static LocString DESC = (LocString) $"(W) Tungsten is a superheated crystalline {UI.FormatAsLink("Metal", "REFINEDMETAL")}.\n\nThis selection is in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class TUNGSTENDISELENIDE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Tungsten Diselenide", nameof (TUNGSTENDISELENIDE));
    public static LocString DESC = (LocString) $"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic {UI.FormatAsLink("Metal", "REFINEDMETAL")} compound with a crystalline structure.\n\nIt is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class TUNGSTENDISELENIDEGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Tungsten Diselenide Gas", nameof (TUNGSTENDISELENIDEGAS));
    public static LocString DESC = (LocString) $"(WSe<sub>2</sub>) Tungsten Diselenide Gasis a superheated {UI.FormatAsLink("Metal", "REFINEDMETAL")} compound in a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class TOXICSAND
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Polluted Dirt", nameof (TOXICSAND));
    public static LocString DESC = (LocString) $"Polluted Dirt is unprocessed biological waste.\n\nIt emits {UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN")} over time.";
  }

  public class UNOBTANIUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Neutronium", nameof (UNOBTANIUM));
    public static LocString DESC = (LocString) "(Nt) Neutronium is a mysterious and extremely resilient element.\n\nIt cannot be excavated by any Duplicant mining tool.";
  }

  public class URANIUMORE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Uranium Ore", nameof (URANIUMORE));
    public static LocString DESC = (LocString) $"(U) Uranium Ore is a highly {UI.FormatAsLink("Radioactive", "RADIATION")} substance.\n\nIt can be refined into fuel for research reactors.";
  }

  public class VACUUM
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Vacuum", nameof (VACUUM));
    public static LocString DESC = (LocString) "A vacuum is a space devoid of all matter.";
  }

  public class VISCOGEL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Visco-Gel Fluid", nameof (VISCOGEL));
    public static LocString DESC = (LocString) $"Visco-Gel Fluid is a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} polymer with high surface tension, preventing typical liquid flow and allowing for unusual configurations.";
  }

  public class VOID
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Void", nameof (VOID));
    public static LocString DESC = (LocString) "Cold, infinite nothingness.";
  }

  public class COMPOSITION
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Composition", nameof (COMPOSITION));
    public static LocString DESC = (LocString) "A mixture of two or more elements.";
  }

  public class WATER
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Water", nameof (WATER));
    public static LocString DESC = (LocString) $"(H<sub>2</sub>O) Clean {UI.FormatAsLink("Water", nameof (WATER))}, suitable for consumption.";
  }

  public class WOLFRAMITE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Wolframite", nameof (WOLFRAMITE));
    public static LocString DESC = (LocString) $"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.\n\nIt is a source of {UI.FormatAsLink("Tungsten", "TUNGSTEN")} and is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class TESTELEMENT
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Test Element", nameof (TESTELEMENT));
    public static LocString DESC = (LocString) $"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} state.\n\nIt is a source of {UI.FormatAsLink("Tungsten", "TUNGSTEN")} and is suitable for building {UI.FormatAsLink("Power", "POWER")} systems.";
  }

  public class POLYPROPYLENE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Plastic", nameof (POLYPROPYLENE));
    public static LocString DESC = (LocString) $"(C<sub>3</sub>H<sub>6</sub>)<sub>n</sub> {(string) ELEMENTS.POLYPROPYLENE.NAME} is a thermoplastic polymer.\n\nIt is useful for constructing a variety of advanced buildings and equipment.";
    public static LocString BUILD_DESC = (LocString) $"Buildings made of this {(string) ELEMENTS.POLYPROPYLENE.NAME} have antiseptic properties";
  }

  public class HARDPOLYPROPYLENE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Plastium", nameof (HARDPOLYPROPYLENE));
    public static LocString DESC = (LocString) $"{(string) ELEMENTS.HARDPOLYPROPYLENE.NAME} is an advanced thermoplastic polymer made from {UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID")}, {UI.FormatAsLink("Plastic", "POLYPROPYLENE")} and {UI.FormatAsLink("Brackwax", "MILKFAT")}.\n\nIt is highly heat-resistant and suitable for use in space buildings.";
  }

  public class NAPHTHA
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Naphtha", nameof (NAPHTHA));
    public static LocString DESC = (LocString) $"Naphtha a distilled hydrocarbon mixture produced from the burning of {UI.FormatAsLink("Plastic", "POLYPROPYLENE")}.";
  }

  public class SLABS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Building Slab", nameof (SLABS));
    public static LocString DESC = (LocString) "Slabs are a refined mineral building block used for assembling advanced buildings.";
  }

  public class TOXICMUD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Polluted Mud", nameof (TOXICMUD));
    public static LocString DESC = (LocString) $"A mixture of {UI.FormatAsLink("Polluted Dirt", "TOXICSAND")} and {UI.FormatAsLink("Polluted Water", "DIRTYWATER")}.\n\nCan be separated into its base elements using a {UI.FormatAsLink("Sludge Press", "SLUDGEPRESS")}.";
  }

  public class MUD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Mud", nameof (MUD));
    public static LocString DESC = (LocString) $"A mixture of {UI.FormatAsLink("Dirt", "DIRT")} and {UI.FormatAsLink("Water", "WATER")}.\n\nCan be separated into its base elements using a {UI.FormatAsLink("Sludge Press", "SLUDGEPRESS")}.";
  }

  public class SUCROSE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Sucrose", nameof (SUCROSE));
    public static LocString DESC = (LocString) $"(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Sucrose is the raw form of sugar.\n\nIt can be used for cooking higher-quality {UI.FormatAsLink("Food", "FOOD")}.";
  }

  public class MOLTENSUCROSE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Sucrose", nameof (MOLTENSUCROSE));
    public static LocString DESC = (LocString) $"(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Liquid Sucrose is the raw form of sugar, heated into a {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} state.";
  }

  public class ZINC
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Zinc", nameof (ZINC));
    public static LocString DESC = (LocString) $"(Zn) Zinc is a very malleable {UI.FormatAsLink("Metal Alloy", "REFINEDMETAL")}.";
  }

  public class ZINCGAS
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Zinc Gas", nameof (ZINCGAS));
    public static LocString DESC = (LocString) $"(Zn) Zinc Gas is {UI.FormatAsLink("Zinc", "ZINC")} heated into a {UI.FormatAsLink("Gaseous", "ELEMENTS_GAS")} state.";
  }

  public class ZINCORE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Zinc Ore", nameof (ZINCORE));
    public static LocString DESC = (LocString) $"(Zn) Zinc Ore is a malleable raw {UI.FormatAsLink("Metal", "METALORE")}.\n\nIt is suitable for building <link=\"POWER\">Power</link> systems.";
  }
}
