// Decompiled with JetBrains decompiler
// Type: BipedTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class BipedTransitionLayer : TransitionDriver.OverrideLayer
{
  private bool isWalking;
  private float floorSpeed;
  private float ladderSpeed;
  private float startTime;
  private bool isInLiquid;
  private float jetPackSpeed;
  private const float downPoleSpeed = 15f;
  private const float WATER_SPEED_PENALTY = 0.5f;
  private const float SUIT_SWIM_SPEED_PENALTY = 0.3f;
  private const float SUIT_SWIM_ANIM_PENALTY = 0.3f;
  private AttributeConverterInstance movementSpeed;
  private AttributeLevels attributeLevels;
  private Attributes attributes;

  public BipedTransitionLayer(Navigator navigator, float floor_speed, float ladder_speed)
    : base(navigator)
  {
    navigator.Subscribe(1773898642, (Action<object>) (data => this.isWalking = true));
    navigator.Subscribe(1597112836, (Action<object>) (data => this.isWalking = false));
    this.floorSpeed = floor_speed;
    this.ladderSpeed = ladder_speed;
    this.jetPackSpeed = 7f;
    this.movementSpeed = Db.Get().AttributeConverters.MovementSpeed.Lookup(navigator.gameObject);
    this.attributeLevels = navigator.GetComponent<AttributeLevels>();
    this.attributes = navigator.gameObject.GetAttributes();
  }

  public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.BeginTransition(navigator, transition);
    float num1 = 1f;
    bool flag1 = (transition.start == NavType.Pole || transition.end == NavType.Pole) && transition.y < 0 && transition.x == 0;
    bool flag2 = transition.start == NavType.Tube || transition.end == NavType.Tube;
    bool flag3 = transition.start == NavType.Hover || transition.end == NavType.Hover;
    bool flag4 = transition.start == NavType.Swim || transition.end == NavType.Swim;
    int num2 = flag1 || flag2 ? 0 : (!flag3 ? 1 : 0);
    int cell = Grid.PosToCell((KMonoBehaviour) navigator);
    this.isInLiquid = navigator.CurrentNavType == NavType.Swim || Grid.IsSubstantialLiquid(cell);
    if (num2 != 0)
    {
      if (this.isWalking)
        return;
      num1 = this.GetMovementSpeedMultiplier();
    }
    float num3 = 1f;
    bool flag5 = (navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) != 0;
    int num4 = (navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) != 0 ? 1 : 0;
    bool flag6 = (navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) != 0;
    int num5 = flag5 ? 1 : 0;
    bool flag7 = (num4 | num5 | (flag6 ? 1 : 0)) != 0;
    if (!flag7 && !flag4 && Grid.IsSubstantialLiquid(cell))
      num3 = 0.5f;
    else if (flag7 & flag4)
    {
      num3 = 0.3f;
      transition.animSpeed = BipedTransitionLayer.GetSwimmingInSuitAnimSpeed(transition);
    }
    float num6 = num1 * num3;
    if (transition.x == 0 && (transition.start == NavType.Ladder || transition.start == NavType.Pole) && transition.start == transition.end)
    {
      if (flag1)
      {
        transition.speed = 15f * num6;
      }
      else
      {
        transition.speed = this.ladderSpeed * num6;
        GameObject gameObject = Grid.Objects[cell, 1];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          Ladder component = gameObject.GetComponent<Ladder>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            float movementSpeedMultiplier = component.upwardsMovementSpeedMultiplier;
            if (transition.y < 0)
              movementSpeedMultiplier = component.downwardsMovementSpeedMultiplier;
            transition.speed *= movementSpeedMultiplier;
            transition.animSpeed *= movementSpeedMultiplier;
          }
        }
      }
    }
    else if (flag2)
      transition.speed = this.GetTubeTravellingSpeedMultiplier(navigator);
    else if (flag3)
    {
      transition.speed = this.jetPackSpeed;
      if (transition.x == 0 && transition.y == -1)
        transition.speed *= 0.75f;
      transition.animSpeed = transition.speed;
    }
    else
      transition.speed = this.floorSpeed * num6;
    float num7 = num6 - 1f;
    transition.animSpeed += (float) ((double) transition.animSpeed * (double) num7 / 2.0);
    if (transition.start == NavType.Floor && transition.end == NavType.Floor)
    {
      int num8 = Grid.CellBelow(cell);
      if (Grid.Foundation[num8])
      {
        GameObject gameObject = Grid.Objects[num8, 1];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          SimCellOccupier component = gameObject.GetComponent<SimCellOccupier>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            transition.speed *= component.movementSpeedMultiplier;
            transition.animSpeed *= component.movementSpeedMultiplier;
          }
        }
      }
    }
    this.startTime = Time.time;
  }

  public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.EndTransition(navigator, transition);
    bool flag1 = (transition.start == NavType.Pole || transition.end == NavType.Pole) && transition.y < 0 && transition.x == 0;
    bool flag2 = transition.start == NavType.Tube || transition.end == NavType.Tube;
    if (!this.isWalking && !flag1 && !flag2 && (UnityEngine.Object) this.attributeLevels != (UnityEngine.Object) null)
      this.attributeLevels.AddExperience(Db.Get().Attributes.Athletics.Id, Time.time - this.startTime, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
    int cell = Grid.OffsetCell(navigator.cachedCell, transition.x, transition.y);
    int num1 = transition.end == NavType.Swim ? 1 : (Grid.IsSubstantialLiquid(cell) ? 1 : 0);
    int num2 = this.isInLiquid ? 1 : 0;
  }

  public float GetTubeTravellingSpeedMultiplier(Navigator navigator)
  {
    AttributeInstance attributeInstance = Db.Get().Attributes.TransitTubeTravelSpeed.Lookup(navigator.gameObject);
    return attributeInstance != null ? attributeInstance.GetTotalValue() : DUPLICANTSTATS.STANDARD.BaseStats.TRANSIT_TUBE_TRAVEL_SPEED;
  }

  public static float GetMovementSpeedMultiplier(AttributeConverterInstance movementSpeed)
  {
    float b = 1f;
    if (movementSpeed != null)
      b += movementSpeed.Evaluate();
    return Mathf.Max(0.1f, b);
  }

  public static float GetSwimmingInSuitAnimSpeed(Navigator.ActiveTransition transition)
  {
    return !transition.isLooping && transition.x != 0 && transition.y != 0 && transition.start == NavType.Swim && transition.end == NavType.Swim ? 0.3f : transition.animSpeed;
  }

  public float GetMovementSpeedMultiplier()
  {
    return BipedTransitionLayer.GetMovementSpeedMultiplier(this.movementSpeed);
  }
}
