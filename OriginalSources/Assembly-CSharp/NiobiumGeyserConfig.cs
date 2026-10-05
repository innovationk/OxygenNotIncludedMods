// Decompiled with JetBrains decompiler
// Type: NiobiumGeyserConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class NiobiumGeyserConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "NiobiumGeyser";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GeyserConfigurator.GeyserType geyserType = new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 3500f, 800f, 1600f, 150f, (string[]) null, minIterationLength: 6000f, maxIterationLength: 12000f, minIterationPercent: 0.005f, maxIterationPercent: 0.01f);
    GameObject geyser = GeyserGenericConfig.CreateGeyser("NiobiumGeyser", "geyser_molten_niobium_kanim", 3, 3, (string) CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.NAME, (string) CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.DESC, geyserType.idHash, geyserType.geyserTemperature, DlcManager.EXPANSION1, (string[]) null);
    geyser.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent);
    return geyser;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
