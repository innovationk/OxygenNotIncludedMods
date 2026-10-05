// Decompiled with JetBrains decompiler
// Type: ClosestEdibleSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ClosestEdibleSensor(Sensors sensors) : Sensor(sensors)
{
  private Edible edible;
  private bool hasEdible;
  public bool edibleInReachButNotPermitted;
  public static Tag[] requiredSearchTags = new Tag[1]
  {
    GameTags.Edible
  };

  public override void Update()
  {
    HashSet<Tag> forbiddenTagSet = this.GetComponent<ConsumableConsumer>().forbiddenTagSet;
    Pickupable edibleFetchTarget = Game.Instance.fetchManager.FindEdibleFetchTarget(this.GetComponent<Storage>(), forbiddenTagSet, ClosestEdibleSensor.requiredSearchTags);
    bool reachButNotPermitted = this.edibleInReachButNotPermitted;
    Edible edible = (Edible) null;
    bool flag1 = false;
    bool flag2;
    if ((Object) edibleFetchTarget != (Object) null)
    {
      edible = edibleFetchTarget.GetComponent<Edible>();
      flag1 = true;
      flag2 = false;
    }
    else
      flag2 = (Object) Game.Instance.fetchManager.FindEdibleFetchTarget(this.GetComponent<Storage>(), new HashSet<Tag>(), ClosestEdibleSensor.requiredSearchTags) != (Object) null;
    if (!((Object) edible != (Object) this.edible) && this.hasEdible == flag1)
      return;
    this.edible = edible;
    this.hasEdible = flag1;
    this.edibleInReachButNotPermitted = flag2;
    this.Trigger(86328522, (object) this.edible);
  }

  public Edible GetEdible() => this.edible;
}
