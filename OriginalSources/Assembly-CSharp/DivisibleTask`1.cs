// Decompiled with JetBrains decompiler
// Type: DivisibleTask`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
internal abstract class DivisibleTask<SharedData> : IWorkItem<SharedData>
{
  public string name;
  public int start;
  public int end;

  public void Run(SharedData sharedData, int threadIndex) => this.RunDivision(sharedData);

  protected DivisibleTask(string name) => this.name = name;

  protected abstract void RunDivision(SharedData sharedData);
}
