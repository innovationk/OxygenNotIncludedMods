// Decompiled with JetBrains decompiler
// Type: Blueprints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Blueprints
{
  public BlueprintCollection all = new BlueprintCollection();
  public BlueprintCollection skinsRelease = new BlueprintCollection();
  public BlueprintProvider[] skinsReleaseProviders = new BlueprintProvider[8]
  {
    (BlueprintProvider) new Blueprints_U51AndBefore(),
    (BlueprintProvider) new Blueprints_DlcPack2(),
    (BlueprintProvider) new Blueprints_U53(),
    (BlueprintProvider) new Blueprints_DlcPack3(),
    (BlueprintProvider) new Blueprints_DlcPack4(),
    (BlueprintProvider) new Blueprints_U57(),
    (BlueprintProvider) new Blueprints_CosmeticPack1(),
    (BlueprintProvider) new Blueprints_DlcPack5()
  };
  private static Blueprints instance;

  public static Blueprints Get()
  {
    if (Blueprints.instance == null)
    {
      Blueprints.instance = new Blueprints();
      Blueprints.instance.all.AddBlueprintsFrom<Blueprints_Default>(new Blueprints_Default());
      foreach (BlueprintProvider skinsReleaseProvider in Blueprints.instance.skinsReleaseProviders)
        Blueprints.instance.skinsRelease.AddBlueprintsFrom<BlueprintProvider>(skinsReleaseProvider);
      Blueprints.instance.all.AddBlueprintsFrom(Blueprints.instance.skinsRelease);
      Blueprints.instance.skinsRelease.PostProcess();
      Blueprints.instance.all.PostProcess();
    }
    return Blueprints.instance;
  }
}
