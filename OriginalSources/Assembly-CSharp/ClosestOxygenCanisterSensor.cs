// Decompiled with JetBrains decompiler
// Type: ClosestOxygenCanisterSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class ClosestOxygenCanisterSensor : ClosestPickupableSensor<Pickupable>
{
  public static readonly Tag GenericBreathableGassesTankTag = new Tag("BreathableGasTank");
  private List<Element> BreathableGasses;

  public ClosestOxygenCanisterSensor(Sensors sensors, bool shouldStartActive)
    : base(sensors, GameTags.Gas, shouldStartActive)
  {
    this.requiredTags = new Tag[1]{ GameTags.Breathable };
    this.BreathableGasses = ElementLoader.FindElements((Func<Element, bool>) (element => element.HasTag(GameTags.Breathable) && element.HasTag(GameTags.Gas)));
  }

  public override HashSet<Tag> GetForbbidenTags()
  {
    if ((UnityEngine.Object) this.consumableConsumer == (UnityEngine.Object) null)
      return new HashSet<Tag>(0);
    HashSet<Tag> forbbidenTags1 = base.GetForbbidenTags();
    if (forbbidenTags1 == null || forbbidenTags1.Count <= 0)
      return forbbidenTags1;
    Tag[] array = new Tag[forbbidenTags1.Count];
    base.GetForbbidenTags().CopyTo(array);
    HashSet<Tag> forbbidenTags2 = new HashSet<Tag>();
    for (int index = 0; index < array.Length; ++index)
    {
      Tag tag = array[index];
      if (tag == ClosestOxygenCanisterSensor.GenericBreathableGassesTankTag)
      {
        foreach (Element breathableGass in this.BreathableGasses)
          forbbidenTags2.Add((Tag) breathableGass.id.ToString());
      }
      else
        forbbidenTags2.Add(tag);
    }
    return forbbidenTags2;
  }
}
