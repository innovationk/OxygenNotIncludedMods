// Decompiled with JetBrains decompiler
// Type: MajorDigSiteWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class MajorDigSiteWorkable : FossilExcavationWorkable
{
  private MajorFossilDigSite.Instance digsite;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetWorkTime(90f);
  }

  protected override void OnSpawn()
  {
    this.digsite = this.gameObject.GetSMI<MajorFossilDigSite.Instance>();
    base.OnSpawn();
  }

  protected override bool IsMarkedForExcavation()
  {
    return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
  }
}
