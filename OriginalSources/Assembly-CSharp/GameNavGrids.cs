// Decompiled with JetBrains decompiler
// Type: GameNavGrids
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GameNavGrids
{
  public NavGrid DuplicantGrid;
  public NavGrid WalkerGrid1x1;
  public NavGrid WalkerGrid1x1NoJump;
  public NavGrid WalkerBabyGrid1x1;
  public NavGrid WalkerGrid1x2;
  public NavGrid WalkerGrid2x2;
  public NavGrid DreckoGrid;
  public NavGrid DreckoBabyGrid;
  public NavGrid FloaterGrid;
  public NavGrid FlyerGrid1x2;
  public NavGrid FlyerGrid1x1;
  public NavGrid FlyerGrid2x2;
  public NavGrid SwimmerGrid;
  public NavGrid SwimmerGrid1x2;
  public NavGrid SwimmerGrid2x2;
  public NavGrid DiggerGrid;
  public NavGrid SquirrelGrid;
  public NavGrid RobotGrid;
  public NavGrid RobotFlyerGrid1x1;
  public const string WALKER_BABY = "WalkerBabyNavGrid";
  public const string WALKER_1X1 = "WalkerNavGrid1x1";
  public const string WALKER_2X2 = "WalkerNavGrid2x2";
  public const string WALKER_NOJUMP_1X1 = "WalkerNavGrid1x1NoJump";
  public const string WALKER_1X2 = "WalkerNavGrid1x2";
  public const string MINION = "MinionNavGrid";
  public const string ROBOT = "RobotNavGrid";
  public const string ROBOT_FLYER_1X1 = "RobotFlyerGrid1x1";
  public const string DIGGER = "DiggerNavGrid";
  public const string DRECKO = "DreckoNavGrid";
  public const string DRECKO_BABY = "DreckoBabyNavGrid";
  public const string FLYER_1X1 = "FlyerNavGrid1x1";
  public const string FLYER_1X2 = "FlyerNavGrid1x2";
  public const string FLYER_2X2 = "FlyerNavGrid2x2";
  public const string SLICKSTER = "FloaterNavGrid";
  public const string SWIMMER = "SwimmerNavGrid";
  public const string SWIMMER1X2 = "SwimmerNavGrid1x2";
  public const string SWIMMER2X2 = "SwimmerGrid2x2";
  public const string SQUIRREL = "SquirrelNavGrid";

  public GameNavGrids(Pathfinding pathfinding)
  {
    this.CreateDuplicantNavigation(pathfinding);
    this.WalkerGrid1x1 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x1", new CellOffset[1]
    {
      new CellOffset(0, 0)
    });
    this.WalkerBabyGrid1x1 = this.CreateWalkerBabyNavigation(pathfinding, "WalkerBabyNavGrid", new CellOffset[1]
    {
      new CellOffset(0, 0)
    });
    this.WalkerGrid1x1NoJump = this.Create1x1NoJumpNavigation(pathfinding, "WalkerNavGrid1x1NoJump", new CellOffset[0]);
    this.WalkerGrid1x2 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x2", new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    });
    this.WalkerGrid2x2 = this.CreateWalkerLargeNavigation(pathfinding, "WalkerNavGrid2x2", new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    });
    this.CreateDreckoNavigation(pathfinding);
    this.CreateDreckoBabyNavigation(pathfinding);
    this.CreateFloaterNavigation(pathfinding);
    this.FlyerGrid1x1 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x1", new CellOffset[1]
    {
      new CellOffset(0, 0)
    });
    this.RobotFlyerGrid1x1 = this.CreateFlyerNavigation(pathfinding, nameof (RobotFlyerGrid1x1), new CellOffset[1]
    {
      new CellOffset(0, 0)
    }, true);
    this.FlyerGrid1x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x2", new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    });
    this.FlyerGrid2x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid2x2", new CellOffset[4]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1),
      new CellOffset(1, 0),
      new CellOffset(1, 1)
    });
    this.SwimmerGrid = this.CreateSwimmerNavigation(pathfinding, "SwimmerNavGrid", new CellOffset[1]
    {
      new CellOffset(0, 0)
    });
    this.SwimmerGrid1x2 = this.CreateSwimmerNavigation1x2(pathfinding, "SwimmerNavGrid1x2", new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    });
    this.SwimmerGrid2x2 = this.CreateLargeSwimmerNavigation(pathfinding, nameof (SwimmerGrid2x2), new CellOffset[4]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1),
      new CellOffset(1, 0),
      new CellOffset(1, 1)
    });
    this.CreateDiggerNavigation(pathfinding);
    this.CreateSquirrelNavigation(pathfinding);
  }

  private void CreateDuplicantNavigation(Pathfinding pathfinding)
  {
    NavOffset[] invalid_nav_offsets = new NavOffset[3]
    {
      new NavOffset(NavType.Floor, 1, 0),
      new NavOffset(NavType.Ladder, 1, 0),
      new NavOffset(NavType.Pole, 1, 0)
    };
    CellOffset[] bounding_offsets = new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    };
    NavGrid.Transition[] setA = new NavGrid.Transition[133]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 20, "", new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1),
        new CellOffset(1, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[6]
      {
        new NavOffset(NavType.Floor, 1, 0),
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, 1),
        new NavOffset(NavType.Ladder, 1, 1),
        new NavOffset(NavType.Pole, 1, 1)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 20, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[6]
      {
        new NavOffset(NavType.Floor, 1, 0),
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, -1),
        new NavOffset(NavType.Ladder, 1, -1),
        new NavOffset(NavType.Pole, 1, -1)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 20, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 20, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Teleport, 0, 0, NavAxis.NA, false, false, false, 14, "fall_pre", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Teleport, NavType.Floor, 0, 0, NavAxis.NA, false, false, false, 1, "fall_pst", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 0, 0)
      }),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Ladder, 0, 1),
        new NavOffset(NavType.Floor, 0, 1)
      }),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Floor, 0, -1),
        new NavOffset(NavType.Ladder, 0, -1)
      }),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets),
      new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, 1, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, -1, NavAxis.NA, true, true, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 25, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 0, 0)
      }),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Pole, 0, 1),
        new NavOffset(NavType.Floor, 0, 1)
      }),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Floor, 0, -1),
        new NavOffset(NavType.Pole, 0, -1)
      }),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets),
      new NavGrid.Transition(NavType.Pole, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets),
      new NavGrid.Transition(NavType.Ladder, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets),
      new NavGrid.Transition(NavType.Pole, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, 1, NavAxis.NA, true, true, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, -1, NavAxis.NA, true, true, false, 6, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets),
      new NavGrid.Transition(NavType.Floor, NavType.Tube, 0, 2, NavAxis.NA, false, false, false, 40, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 1)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -1)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[4]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1),
        new CellOffset(2, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -2)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ladder, 0, 1)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 1)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -1)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[4]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1),
        new CellOffset(2, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -2)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Pole, 0, 1)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 1)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -1)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[4]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1),
        new CellOffset(2, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -2)
      }),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 0, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, 1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, -1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Tube, 0, 1)
      }, new NavOffset[0], animSpeed: 2.2f),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Tube, 1, 0)
      }, new NavOffset[0], animSpeed: 2.2f),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Tube, 0, -1)
      }, new NavOffset[0], animSpeed: 2.2f),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Tube, 1, 0)
      }, new NavOffset[0], animSpeed: 2.2f),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, false, 8, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, false, false, 8, "hover_hover_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, false, false, 8, "hover_hover_0_-1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, true, false, false, 9, "hover_hover_1_1", new CellOffset[0], new CellOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Hover, 1, 0),
        new NavOffset(NavType.Hover, 0, 1)
      }, new NavOffset[0]),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, true, false, false, 9, "hover_hover_1_-1", new CellOffset[0], new CellOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Hover, 1, 0),
        new NavOffset(NavType.Hover, 0, -1)
      }, new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Hover, 1, 0, NavAxis.NA, false, false, false, 8, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Hover, 0, 1, NavAxis.NA, false, false, false, 8, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Hover, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Hover, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, false, false, 8, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, false, false, 8, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, false, false, 8, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, false, false, false, 10, "swim_swim_1_1_loop", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, false, false, false, 10, "swim_swim_1_-1_loop", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Swim, 1, -1, NavAxis.NA, false, false, false, 11, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Swim, 1, -2, NavAxis.NA, false, false, false, 12, "", new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1),
        new CellOffset(1, -2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Swim, 0, 1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Swim, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Floor, 0, 0, NavAxis.NA, false, false, false, 17, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 17, "swim_floor_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 17, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Floor, 1, 1, NavAxis.NA, false, false, false, 17, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Floor, 1, 2, NavAxis.NA, false, false, false, 22, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Ladder, 0, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Swim, 0, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ladder, NavType.Swim, 0, -1, NavAxis.NA, false, false, false, 8, "ladder_swim_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Pole, 0, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 20, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Swim, 0, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Pole, NavType.Swim, 0, -1, NavAxis.NA, false, false, false, 20, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Tube, NavType.Swim, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0])
    };
    NavGrid.Transition[] setB1 = new NavGrid.Transition[2]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[1]{ new CellOffset(1, 1) }, new NavOffset[0], new NavOffset[6]
      {
        new NavOffset(NavType.Floor, 1, 0),
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, -1),
        new NavOffset(NavType.Ladder, 1, -1),
        new NavOffset(NavType.Pole, 1, -1)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[1]{ new CellOffset(1, 2) }, new NavOffset[0], new NavOffset[6]
      {
        new NavOffset(NavType.Floor, 1, 0),
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, 1),
        new NavOffset(NavType.Ladder, 1, 1),
        new NavOffset(NavType.Pole, 1, 1)
      })
    };
    NavGrid.Transition[] transitions = this.MirrorTransitions(this.CombineTransitions(setA, setB1));
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[7]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_default"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ladder,
        idleAnim = (HashedString) "ladder_idle"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Pole,
        idleAnim = (HashedString) "pole_idle"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Tube,
        idleAnim = (HashedString) "tube_idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Hover,
        idleAnim = (HashedString) "hover_idle"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Teleport,
        idleAnim = (HashedString) "idle_default"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Swim,
        idleAnim = (HashedString) "treading_loop"
      }
    };
    this.DuplicantGrid = new NavGrid("MinionNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[7]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(true),
      (NavTableValidator) new GameNavGrids.LadderValidator(),
      (NavTableValidator) new GameNavGrids.PoleValidator(),
      (NavTableValidator) new GameNavGrids.TubeValidator(),
      (NavTableValidator) new GameNavGrids.TeleporterValidator(),
      (NavTableValidator) new GameNavGrids.FlyingValidator(true, true, true),
      (NavTableValidator) new GameNavGrids.DupeSwimValidator()
    }, 2, 3, 55);
    this.DuplicantGrid.updateEveryFrame = true;
    pathfinding.AddNavGrid(this.DuplicantGrid);
    NavGrid.Transition[] setB2 = new NavGrid.Transition[2]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[1]{ new CellOffset(1, 1) }, new NavOffset[0], new NavOffset[6]
      {
        new NavOffset(NavType.Floor, 1, 0),
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, -1),
        new NavOffset(NavType.Ladder, 1, -1),
        new NavOffset(NavType.Pole, 1, -1)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "climb_up_2_1", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[1]{ new CellOffset(1, 2) }, new NavOffset[0], new NavOffset[6]
      {
        new NavOffset(NavType.Floor, 1, 0),
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, 1),
        new NavOffset(NavType.Ladder, 1, 1),
        new NavOffset(NavType.Pole, 1, 1)
      })
    };
    this.RobotGrid = new NavGrid("RobotNavGrid", this.MirrorTransitions(this.CombineTransitions(setA, setB2)), nav_type_data, bounding_offsets, new NavTableValidator[2]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(true),
      (NavTableValidator) new GameNavGrids.LadderValidator()
    }, 2, 3, 22);
    this.RobotGrid.updateEveryFrame = true;
    pathfinding.AddNavGrid(this.RobotGrid);
  }

  private NavGrid Create1x1NoJumpNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounds)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[3]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounds, (NavTableValidator[]) new GameNavGrids.FloorValidator[1]
    {
      new GameNavGrids.FloorValidator(false)
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private NavGrid CreateWalkerNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[6]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[1]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false)
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private NavGrid CreateWalkerBabyNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[1]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0])
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[1]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false)
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private NavGrid CreateWalkerLargeNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[3]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, useOffsetX: true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 1, "floor_floor_2_1", new CellOffset[4]
      {
        new CellOffset(1, 1),
        new CellOffset(1, 2),
        new CellOffset(2, 1),
        new CellOffset(0, 2)
      }, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0)
      }, new NavOffset[0], new NavOffset[0], true, useOffsetX: true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, true, 1, "floor_floor_2_-1", new CellOffset[4]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(2, -1),
        new CellOffset(1, -1)
      }, new CellOffset[2]
      {
        new CellOffset(1, -2),
        new CellOffset(2, -2)
      }, new NavOffset[0], new NavOffset[0], true, useOffsetX: true)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[1]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false)
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private void CreateDreckoNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    this.DreckoGrid = new NavGrid("DreckoNavGrid", this.MirrorTransitions(new NavGrid.Transition[24]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 4, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 1, -2)
      }, true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 2, NavAxis.NA, false, false, true, 5, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 2)
      }, new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, 0, 0)
      }, true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 4, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.RightWall, 0, 0),
        new NavOffset(NavType.Floor, 2, 2)
      }, true),
      new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, 0, 0)
      }, new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, 0, 1)
      }),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, -1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ceiling, 0, 0)
      }, new NavOffset[0]),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ceiling, -1, 0)
      }),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, -1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 0, 0)
      }, new NavOffset[0]),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 0, -1)
      }),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 0, 0)
      }, new NavOffset[0]),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 1, -1)
      }, new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 1, -2)
      }, true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -2, -1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[2]
      {
        new CellOffset(0, -1),
        new CellOffset(-1, -1)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ceiling, -1, -1)
      }, new NavOffset[0], true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ceiling, -2, -1)
      }, true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[2]
      {
        new CellOffset(-1, 0),
        new CellOffset(-1, 1)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, -1, 1)
      }, new NavOffset[0], true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(-1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, -1, 2)
      }, true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 1)
      }, new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 2, 1)
      }, true)
    }), new NavGrid.NavTypeData[4]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.RightWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.5f, -0.5f, 0.0f),
        rotation = -1.57079637f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ceiling,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.0f, -1f, 0.0f),
        rotation = -3.14159274f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.LeftWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(-0.5f, -0.5f, 0.0f),
        rotation = -4.712389f
      }
    }, bounding_offsets, new NavTableValidator[3]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false),
      (NavTableValidator) new GameNavGrids.WallValidator(),
      (NavTableValidator) new GameNavGrids.CeilingValidator()
    }, 2, 3, 16 /*0x10*/);
    pathfinding.AddNavGrid(this.DreckoGrid);
  }

  private void CreateDreckoBabyNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    this.DreckoBabyGrid = new NavGrid("DreckoBabyNavGrid", this.MirrorTransitions(new NavGrid.Transition[12]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(-1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true)
    }), new NavGrid.NavTypeData[4]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.RightWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.5f, -0.5f, 0.0f),
        rotation = -1.57079637f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ceiling,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.0f, -1f, 0.0f),
        rotation = -3.14159274f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.LeftWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(-0.5f, -0.5f, 0.0f),
        rotation = -4.712389f
      }
    }, bounding_offsets, new NavTableValidator[3]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false),
      (NavTableValidator) new GameNavGrids.WallValidator(),
      (NavTableValidator) new GameNavGrids.CeilingValidator()
    }, 2, 3, 16 /*0x10*/);
    pathfinding.AddNavGrid(this.DreckoBabyGrid);
  }

  private void CreateFloaterNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    this.FloaterGrid = new NavGrid("FloaterNavGrid", this.MirrorTransitions(new NavGrid.Transition[17]
    {
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, -1)
      }),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, 0)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, -2)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 0, 0)
      }),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 0, -2)
      }),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 1, NavAxis.NA, false, false, true, 3, "", new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 2, 0)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 2, -1)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, -1, NavAxis.NA, false, false, true, 3, "", new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1),
        new CellOffset(1, -2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 2, -2)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 2, NavAxis.NA, false, false, true, 3, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, 1)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -2, NavAxis.NA, false, false, true, 3, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, -3)
      }, true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0])
    }), new NavGrid.NavTypeData[2]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Hover,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Swim,
        idleAnim = (HashedString) "swim_idle_loop"
      }
    }, bounding_offsets, new NavTableValidator[2]
    {
      (NavTableValidator) new GameNavGrids.HoverValidator(),
      (NavTableValidator) new GameNavGrids.SwimValidator(true)
    }, 2, 3, 22);
    pathfinding.AddNavGrid(this.FloaterGrid);
  }

  private NavGrid CreateFlyerNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets,
    bool allow_door_traversal = false)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[12]
    {
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 2, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0])
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[2]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Hover,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Swim,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[2]
    {
      (NavTableValidator) new GameNavGrids.FlyingValidator(allow_door_traversal: allow_door_traversal),
      (NavTableValidator) new GameNavGrids.SwimValidator(true)
    }, 2, 2, 16 /*0x10*/);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private NavGrid CreateSwimmerNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets,
    bool use_x_offset = false)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[5]
    {
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], useOffsetX: use_x_offset),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, useOffsetX: (use_x_offset ? 1 : 0) != 0),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, useOffsetX: (use_x_offset ? 1 : 0) != 0),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0])
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Swim,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[1]
    {
      (NavTableValidator) new GameNavGrids.SwimValidator(false)
    }, 1, 2, transitions.Length);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private NavGrid CreateSwimmerNavigation1x2(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[5]
    {
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0])
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Swim,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[1]
    {
      (NavTableValidator) new GameNavGrids.SwimValidator(true)
    }, 1, 2, transitions.Length);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private NavGrid CreateLargeSwimmerNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets)
  {
    NavGrid.Transition[] transitions = new NavGrid.Transition[8]
    {
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 2, "", new CellOffset[4]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1),
        new CellOffset(2, 0),
        new CellOffset(2, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[4]
      {
        new CellOffset(1, 1),
        new CellOffset(1, 2),
        new CellOffset(2, 1),
        new CellOffset(2, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[5]
      {
        new CellOffset(0, -1),
        new CellOffset(1, 0),
        new CellOffset(1, -1),
        new CellOffset(2, 0),
        new CellOffset(2, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, -1, 0, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, -1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[4]
      {
        new CellOffset(0, 1),
        new CellOffset(-1, 1),
        new CellOffset(0, 2),
        new CellOffset(-1, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, -1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[3]
      {
        new CellOffset(-1, 0),
        new CellOffset(0, -1),
        new CellOffset(-1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 1),
        new CellOffset(1, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, -1),
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true)
    };
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Swim,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[1]
    {
      (NavTableValidator) new GameNavGrids.SwimValidator(true, true)
    }, 2, 2, 16 /*0x10*/);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private void CreateDiggerNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    this.DiggerGrid = new NavGrid("DiggerNavGrid", this.MirrorTransitions(new NavGrid.Transition[24]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(-1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 0, NavAxis.NA, false, false, true, 1, "idle1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 1, NavAxis.NA, false, false, true, 1, "idle2", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Solid, NavType.Solid, 0, 1, NavAxis.NA, false, false, true, 1, "idle3", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, -1, NavAxis.NA, false, false, true, 1, "idle4", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.Solid, 0, -1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Solid, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 1, "drill_out", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ceiling, NavType.Solid, 0, 1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Solid, NavType.Ceiling, 0, -1, NavAxis.NA, false, false, true, 1, "drill_out_ceiling", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Solid, NavType.LeftWall, 1, 0, NavAxis.NA, false, false, true, 1, "drill_out_left_wall", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.LeftWall, NavType.Solid, -1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Solid, NavType.RightWall, -1, 0, NavAxis.NA, false, false, true, 1, "drill_out_right_wall", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.RightWall, NavType.Solid, 1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0])
    }), new NavGrid.NavTypeData[5]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ceiling,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.0f, -1f, 0.0f),
        rotation = -3.14159274f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.RightWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.5f, -0.5f, 0.0f),
        rotation = -1.57079637f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.LeftWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(-0.5f, -0.5f, 0.0f),
        rotation = -4.712389f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Solid,
        idleAnim = (HashedString) "idle1"
      }
    }, bounding_offsets, new NavTableValidator[4]
    {
      (NavTableValidator) new GameNavGrids.SolidValidator(),
      (NavTableValidator) new GameNavGrids.FloorValidator(false),
      (NavTableValidator) new GameNavGrids.WallValidator(),
      (NavTableValidator) new GameNavGrids.CeilingValidator()
    }, 2, 3, 22);
    pathfinding.AddNavGrid(this.DiggerGrid);
  }

  private void CreateSquirrelNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    this.SquirrelGrid = new NavGrid("SquirrelNavGrid", this.MirrorTransitions(new NavGrid.Transition[17]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0]),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(-1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true)
    }), new NavGrid.NavTypeData[4]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ceiling,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.0f, -1f, 0.0f),
        rotation = -3.14159274f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.RightWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.5f, -0.5f, 0.0f),
        rotation = -1.57079637f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.LeftWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(-0.5f, -0.5f, 0.0f),
        rotation = -4.712389f
      }
    }, bounding_offsets, new NavTableValidator[3]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false),
      (NavTableValidator) new GameNavGrids.WallValidator(),
      (NavTableValidator) new GameNavGrids.CeilingValidator()
    }, 2, 3, 20);
    pathfinding.AddNavGrid(this.SquirrelGrid);
  }

  private CellOffset[] MirrorOffsets(CellOffset[] offsets)
  {
    List<CellOffset> cellOffsetList = new List<CellOffset>();
    foreach (CellOffset offset in offsets)
    {
      offset.x = -offset.x;
      cellOffsetList.Add(offset);
    }
    return cellOffsetList.ToArray();
  }

  private NavOffset[] MirrorNavOffsets(NavOffset[] offsets)
  {
    List<NavOffset> navOffsetList = new List<NavOffset>();
    foreach (NavOffset offset in offsets)
    {
      offset.navType = NavGrid.MirrorNavType(offset.navType);
      offset.offset.x = -offset.offset.x;
      navOffsetList.Add(offset);
    }
    return navOffsetList.ToArray();
  }

  private NavGrid.Transition[] MirrorTransitions(NavGrid.Transition[] transitions)
  {
    List<NavGrid.Transition> transitionList = new List<NavGrid.Transition>();
    foreach (NavGrid.Transition transition1 in transitions)
    {
      transitionList.Add(transition1);
      if (transition1.x != 0 || transition1.start == NavType.RightWall || transition1.end == NavType.RightWall || transition1.start == NavType.LeftWall || transition1.end == NavType.LeftWall)
      {
        NavGrid.Transition transition2 = transition1;
        transition2.x = -transition2.x;
        transition2.voidOffsets = this.MirrorOffsets(transition1.voidOffsets);
        transition2.solidOffsets = this.MirrorOffsets(transition1.solidOffsets);
        transition2.validNavOffsets = this.MirrorNavOffsets(transition1.validNavOffsets);
        transition2.invalidNavOffsets = this.MirrorNavOffsets(transition1.invalidNavOffsets);
        transition2.start = NavGrid.MirrorNavType(transition2.start);
        transition2.end = NavGrid.MirrorNavType(transition2.end);
        transitionList.Add(transition2);
      }
    }
    transitionList.Sort((Comparison<NavGrid.Transition>) ((x, y) => x.cost.CompareTo(y.cost)));
    return transitionList.ToArray();
  }

  private NavGrid.Transition[] CombineTransitions(
    NavGrid.Transition[] setA,
    NavGrid.Transition[] setB)
  {
    NavGrid.Transition[] transitionArray = new NavGrid.Transition[setA.Length + setB.Length];
    Array.Copy((Array) setA, (Array) transitionArray, setA.Length);
    Array.Copy((Array) setB, 0, (Array) transitionArray, setA.Length, setB.Length);
    Array.Sort<NavGrid.Transition>(transitionArray, (Comparison<NavGrid.Transition>) ((x, y) => x.cost.CompareTo(y.cost)));
    return transitionArray;
  }

  public class SwimValidator : NavTableValidator
  {
    private bool requireSubstantialLiquidAbove;
    private bool largeSwimmer;

    public SwimValidator(bool requireSubstantialLiquidAbove, bool large_swimmer = false)
    {
      this.requireSubstantialLiquidAbove = requireSubstantialLiquidAbove;
      this.largeSwimmer = large_swimmer;
      World.Instance.OnLiquidChanged += new Action<int>(this.OnLiquidChanged);
      GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[9], new Action<int, object>(this.OnFoundationTileChanged));
    }

    private void OnFoundationTileChanged(int cell, object unused)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool flag = Grid.IsSubstantialLiquid(cell);
      int cell1 = Grid.CellAbove(cell);
      if (!flag && Grid.IsValidCell(cell1))
        flag = this.requireSubstantialLiquidAbove ? Grid.IsSubstantialLiquid(cell1) : Grid.IsLiquid(cell1);
      bool is_valid = Grid.IsWorldValidCell(cell) & flag && this.IsSwimClear(cell, bounding_offsets);
      nav_table.SetValid(cell, NavType.Swim, is_valid);
    }

    private bool IsSwimClear(int cell, CellOffset[] bounding_offsets)
    {
      if (!this.largeSwimmer)
        return this.IsClear(cell, bounding_offsets, false);
      for (int index = 0; index < bounding_offsets.Length; ++index)
      {
        CellOffset boundingOffset = bounding_offsets[index];
        int cell1 = Grid.OffsetCell(cell, boundingOffset);
        if (!Grid.IsWorldValidCell(cell1) || boundingOffset.x == 0 && !NavTableValidator.IsCellPassable(cell1, false))
          return false;
      }
      return true;
    }

    private void OnLiquidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }
  }

  public class DupeSwimValidator : NavTableValidator
  {
    public DupeSwimValidator()
    {
      World.Instance.OnLiquidChanged += new Action<int>(this.OnLiquidChanged);
      GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[9], new Action<int, object>(this.OnFoundationTileChanged));
    }

    private void OnFoundationTileChanged(int cell, object unused)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool flag1 = this.IsClear(cell, bounding_offsets, true);
      bool flag2 = Grid.IsSubstantialLiquid(cell, 0.1f);
      int cell1 = Grid.CellAbove(cell);
      bool flag3 = Grid.IsWorldValidCell(cell1) && Grid.IsLiquid(cell1);
      int num1 = nav_table.IsValid(cell, NavType.Swim) ? 1 : 0;
      bool is_valid = flag1 && (flag2 || Grid.IsLiquid(cell) & flag3);
      nav_table.SetValid(cell, NavType.Swim, is_valid);
      int num2 = is_valid ? 1 : 0;
      if (num1 == num2)
        return;
      int x1;
      int y1;
      Grid.CellToXY(cell, out x1, out y1);
      int num3 = Grid.ClampX(x1 - 1);
      int num4 = Grid.ClampY(y1 - 2);
      int num5 = Grid.ClampX(x1 + 1);
      int num6 = Grid.ClampY(y1 + 2);
      for (int y2 = num4; y2 <= num6; ++y2)
      {
        for (int x2 = num3; x2 <= num5; ++x2)
          this.onDirty(Grid.XYToCell(x2, y2));
      }
    }

    private void OnLiquidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }
  }

  public class FloorValidator : NavTableValidator
  {
    private bool isDupe;

    public FloorValidator(bool is_dupe)
    {
      World.Instance.OnSolidChanged += new Action<int>(this.OnSolidChanged);
      Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
      this.isDupe = is_dupe;
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool flag = GameNavGrids.FloorValidator.IsWalkableCell(cell, Grid.CellBelow(cell), this.isDupe);
      nav_table.SetValid(cell, NavType.Floor, flag && this.IsClear(cell, bounding_offsets, this.isDupe));
    }

    public static bool IsWalkableCell(int cell, int anchor_cell, bool is_dupe)
    {
      if (!Grid.IsWorldValidCell(cell) || !Grid.IsWorldValidCell(anchor_cell) || !NavTableValidator.IsCellPassable(cell, is_dupe))
        return false;
      if (Grid.FakeFloor[anchor_cell])
        return true;
      return Grid.Solid[anchor_cell] ? !Grid.DupePassable[anchor_cell] : is_dupe && (Grid.NavValidatorMasks[cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) == (Grid.NavValidatorFlags) 0 && (Grid.NavValidatorMasks[anchor_cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) != 0;
    }

    private void OnAddLadder(Ladder ladder)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) ladder);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    private void OnRemoveLadder(Ladder ladder)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) ladder);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    private void OnSolidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new Action<int>(this.OnSolidChanged);
      Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
    }
  }

  public class WallValidator : NavTableValidator
  {
    public WallValidator() => World.Instance.OnSolidChanged += new Action<int>(this.OnSolidChanged);

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool flag1 = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellRight(cell));
      bool flag2 = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellLeft(cell));
      nav_table.SetValid(cell, NavType.RightWall, flag1 && this.IsClear(cell, bounding_offsets, false));
      nav_table.SetValid(cell, NavType.LeftWall, flag2 && this.IsClear(cell, bounding_offsets, false));
    }

    private static bool IsWalkableCell(int cell, int anchor_cell)
    {
      return Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell) && NavTableValidator.IsCellPassable(cell, false) && (Grid.Solid[anchor_cell] || Grid.CritterImpassable[anchor_cell]);
    }

    private void OnSolidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new Action<int>(this.OnSolidChanged);
    }
  }

  public class CeilingValidator : NavTableValidator
  {
    public CeilingValidator()
    {
      World.Instance.OnSolidChanged += new Action<int>(this.OnSolidChanged);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool flag = GameNavGrids.CeilingValidator.IsWalkableCell(cell, Grid.CellAbove(cell));
      nav_table.SetValid(cell, NavType.Ceiling, flag && this.IsClear(cell, bounding_offsets, false));
    }

    private static bool IsWalkableCell(int cell, int anchor_cell)
    {
      return Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell) && NavTableValidator.IsCellPassable(cell, false) && (Grid.Solid[anchor_cell] || (!Grid.HasDoor[cell] || Grid.FakeFloor[cell]) && (Grid.FakeFloor[anchor_cell] || Grid.HasDoor[anchor_cell]));
    }

    private void OnSolidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new Action<int>(this.OnSolidChanged);
    }
  }

  public class LadderValidator : NavTableValidator
  {
    public LadderValidator()
    {
      Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
    }

    private void OnAddLadder(Ladder ladder)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) ladder);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    private void OnRemoveLadder(Ladder ladder)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) ladder);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      nav_table.SetValid(cell, NavType.Ladder, this.IsClear(cell, bounding_offsets, true) && Grid.HasLadder[cell]);
    }

    public override void Clear()
    {
      Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
    }
  }

  public class PoleValidator : GameNavGrids.LadderValidator
  {
    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      nav_table.SetValid(cell, NavType.Pole, this.IsClear(cell, bounding_offsets, true) && Grid.HasPole[cell]);
    }
  }

  public class TubeValidator : NavTableValidator
  {
    public TubeValidator()
    {
      Components.ITravelTubePieces.Register(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
    }

    private void OnAddLadder(ITravelTubePiece tube)
    {
      int cell = Grid.PosToCell(tube.Position);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    private void OnRemoveLadder(ITravelTubePiece tube)
    {
      int cell = Grid.PosToCell(tube.Position);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      nav_table.SetValid(cell, NavType.Tube, Grid.HasTube[cell]);
    }

    public override void Clear()
    {
      Components.ITravelTubePieces.Unregister(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
    }
  }

  public class TeleporterValidator : NavTableValidator
  {
    public TeleporterValidator()
    {
      Components.NavTeleporters.Register(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
    }

    private void OnAddTeleporter(NavTeleporter teleporter)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) teleporter);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    private void OnRemoveTeleporter(NavTeleporter teleporter)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) teleporter);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool is_valid = Grid.IsWorldValidCell(cell) && Grid.HasNavTeleporter[cell];
      nav_table.SetValid(cell, NavType.Teleport, is_valid);
    }

    public override void Clear()
    {
      Components.NavTeleporters.Unregister(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
    }
  }

  public class FlyingValidator : NavTableValidator
  {
    private bool exclude_floor;
    private bool exclude_jet_suit_blockers;
    private bool allow_door_traversal;
    private HandleVector<int>.Handle buildingParititonerEntry;

    public FlyingValidator(
      bool exclude_floor = false,
      bool exclude_jet_suit_blockers = false,
      bool allow_door_traversal = false)
    {
      this.exclude_floor = exclude_floor;
      this.exclude_jet_suit_blockers = exclude_jet_suit_blockers;
      this.allow_door_traversal = allow_door_traversal;
      World.Instance.OnSolidChanged += new Action<int>(this.MarkCellDirty);
      World.Instance.OnLiquidChanged += new Action<int>(this.MarkCellDirty);
      GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool is_valid = false;
      if (Grid.IsWorldValidCell(Grid.CellAbove(cell)))
      {
        is_valid = !Grid.IsSubstantialLiquid(cell) && this.IsClear(cell, bounding_offsets, this.allow_door_traversal);
        if (is_valid && this.exclude_floor)
        {
          int cell1 = Grid.CellBelow(cell);
          if (Grid.IsWorldValidCell(cell1))
            is_valid = this.IsClear(cell1, bounding_offsets, this.allow_door_traversal);
        }
        if (is_valid && this.exclude_jet_suit_blockers)
        {
          GameObject go = Grid.Objects[cell, 1];
          is_valid = (UnityEngine.Object) go == (UnityEngine.Object) null || !go.HasTag(GameTags.JetSuitBlocker);
        }
      }
      nav_table.SetValid(cell, NavType.Hover, is_valid);
    }

    private void OnBuildingChange(int cell, object data) => this.MarkCellDirty(cell);

    private void MarkCellDirty(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new Action<int>(this.MarkCellDirty);
      World.Instance.OnLiquidChanged -= new Action<int>(this.MarkCellDirty);
      GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
    }
  }

  public class HoverValidator : NavTableValidator
  {
    public HoverValidator()
    {
      World.Instance.OnSolidChanged += new Action<int>(this.MarkCellDirty);
      World.Instance.OnLiquidChanged += new Action<int>(this.MarkCellDirty);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      int num = Grid.CellBelow(cell);
      if (!Grid.IsWorldValidCell(num))
        return;
      bool flag = Grid.Solid[num] || Grid.FakeFloor[num] || Grid.IsSubstantialLiquid(num);
      nav_table.SetValid(cell, NavType.Hover, !Grid.IsSubstantialLiquid(cell) & flag && this.IsClear(cell, bounding_offsets, false));
    }

    private void MarkCellDirty(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new Action<int>(this.MarkCellDirty);
      World.Instance.OnLiquidChanged -= new Action<int>(this.MarkCellDirty);
    }
  }

  public class SolidValidator : NavTableValidator
  {
    public SolidValidator()
    {
      World.Instance.OnSolidChanged += new Action<int>(this.OnSolidChanged);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool is_valid = GameNavGrids.SolidValidator.IsDiggable(cell, Grid.CellBelow(cell));
      nav_table.SetValid(cell, NavType.Solid, is_valid);
    }

    public static bool IsDiggable(int cell, int anchor_cell)
    {
      if (Grid.IsWorldValidCell(cell) && Grid.Solid[cell])
      {
        if (Grid.HasDoor[cell] || Grid.Foundation[cell])
        {
          GameObject gameObject = Grid.Objects[cell, 1];
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          {
            PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
            return Grid.Element[cell].hardness < (byte) 150 && !component.Element.HasTag(GameTags.RefinedMetal);
          }
        }
        else
        {
          ushort index = Grid.ElementIdx[cell];
          Element element = ElementLoader.elements[(int) index];
          return Grid.Element[cell].hardness < (byte) 150 && !element.HasTag(GameTags.RefinedMetal);
        }
      }
      return false;
    }

    private void OnSolidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new Action<int>(this.OnSolidChanged);
    }
  }
}
