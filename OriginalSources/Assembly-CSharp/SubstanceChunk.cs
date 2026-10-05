// Decompiled with JetBrains decompiler
// Type: SubstanceChunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SubstanceChunk")]
public class SubstanceChunk : KMonoBehaviour, ISaveLoadable
{
  private const string symbolName = "substance_tinter";
  private static readonly KAnimHashedString symbolToTint = new KAnimHashedString("substance_tinter");
  private static readonly KAnimHashedString symbolToTint2 = new KAnimHashedString("substance_tinter_cap");

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Element element = this.GetComponent<PrimaryElement>().Element;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (element.IsLiquid)
    {
      GameUtil.TintLiquidSymbolOnBuilding("substance_tinter", component, element);
    }
    else
    {
      Color colour = (Color) element.substance.colour with
      {
        a = 1f
      };
      component.SetSymbolTint(SubstanceChunk.symbolToTint, colour);
      component.SetSymbolTint(SubstanceChunk.symbolToTint2, colour);
    }
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.RELEASEELEMENT.NAME, new System.Action(this.OnRelease), tooltipText: (string) UI.USERMENUACTIONS.RELEASEELEMENT.TOOLTIP));
  }

  private void OnRelease()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    if ((double) component.Mass > 0.0)
      SimMessages.AddRemoveSubstance(cell, component.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount);
    this.gameObject.DeleteObject();
  }
}
