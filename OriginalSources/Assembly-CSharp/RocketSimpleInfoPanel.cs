// Decompiled with JetBrains decompiler
// Type: RocketSimpleInfoPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RocketSimpleInfoPanel(SimpleInfoScreen simpleInfoScreen) : SimpleInfoPanel(simpleInfoScreen)
{
  private Dictionary<string, GameObject> cargoBayLabels = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> artifactModuleLabels = new Dictionary<string, GameObject>();

  public override void Refresh(
    CollapsibleDetailContentPanel rocketStatusContainer,
    GameObject selectedTarget)
  {
    if ((Object) selectedTarget == (Object) null)
    {
      this.simpleInfoRoot.StoragePanel.gameObject.SetActive(false);
    }
    else
    {
      RocketModuleCluster rocketModuleCluster = (RocketModuleCluster) null;
      Clustercraft clusterCraft = (Clustercraft) null;
      CraftModuleInterface craftModuleInterface = (CraftModuleInterface) null;
      RocketSimpleInfoPanel.GetRocketStuffFromTarget(selectedTarget, ref rocketModuleCluster, ref clusterCraft, ref craftModuleInterface);
      rocketStatusContainer.gameObject.SetActive((Object) craftModuleInterface != (Object) null || (Object) rocketModuleCluster != (Object) null);
      if ((Object) craftModuleInterface != (Object) null)
      {
        RocketEngineCluster engine = craftModuleInterface.GetEngine();
        string str1;
        string str2;
        if ((Object) engine != (Object) null && (Object) engine.GetComponent<HEPFuelTank>() != (Object) null)
        {
          str1 = GameUtil.GetFormattedHighEnergyParticles(craftModuleInterface.FuelPerHex);
          str2 = GameUtil.GetFormattedHighEnergyParticles(craftModuleInterface.FuelRemaining);
        }
        else
        {
          str1 = GameUtil.GetFormattedMass(craftModuleInterface.FuelPerHex);
          str2 = GameUtil.GetFormattedMass(craftModuleInterface.FuelRemaining);
        }
        string tooltip1 = $"{(string) UI.CLUSTERMAP.ROCKETS.RANGE.TOOLTIP}\n    • {string.Format((string) UI.CLUSTERMAP.ROCKETS.FUEL_PER_HEX.NAME, (object) str1)}\n    • {(string) UI.CLUSTERMAP.ROCKETS.FUEL_REMAINING.NAME}{str2}\n    • {(string) UI.CLUSTERMAP.ROCKETS.OXIDIZER_REMAINING.NAME}{GameUtil.GetFormattedMass(craftModuleInterface.OxidizerPowerRemaining)}";
        bool is_robo_pilot;
        RocketModuleCluster primaryPilotModule = craftModuleInterface.GetPrimaryPilotModule(out is_robo_pilot);
        if (is_robo_pilot)
        {
          RoboPilotModule component = primaryPilotModule.GetComponent<RoboPilotModule>();
          tooltip1 = $"{tooltip1}\n{string.Format((string) UI.CLUSTERMAP.ROCKETS.RANGE.ROBO_PILOTED_TOOLTIP, (object) component.dataBankConsumption, (object) component.GetDataBanksStored())}";
        }
        rocketStatusContainer.SetLabel("RangeRemaining", (string) UI.CLUSTERMAP.ROCKETS.RANGE.NAME + GameUtil.GetFormattedRocketRange(craftModuleInterface.RangeInTiles), tooltip1);
        string tooltip2 = $"{(string) UI.CLUSTERMAP.ROCKETS.SPEED.TOOLTIP}\n    • {(string) UI.CLUSTERMAP.ROCKETS.POWER_TOTAL.NAME}{craftModuleInterface.EnginePower.ToString()}\n    • {(string) UI.CLUSTERMAP.ROCKETS.BURDEN_TOTAL.NAME}{craftModuleInterface.TotalBurden.ToString()}";
        Clustercraft component1 = craftModuleInterface.GetComponent<Clustercraft>();
        if ((Object) component1 != (Object) null)
        {
          tooltip2 += (string) UI.CLUSTERMAP.ROCKETS.SPEED.PILOT_SPEED_MODIFIER;
          bool passengerModule = (bool) (Object) craftModuleInterface.GetPassengerModule();
          bool robotPilotModule = (bool) (Object) craftModuleInterface.GetRobotPilotModule();
          bool dupe_piloted;
          bool robo_piloted;
          component1.GetPilotedStatus(out dupe_piloted, out robo_piloted);
          if (dupe_piloted)
            tooltip2 = $"{tooltip2}\n    • {UI.CLUSTERMAP.ROCKETS.SPEED.DUPEPILOT_SPEED_TOOLTIP.Replace("{speed_boost}", GameUtil.GetFormattedPercent(component1.PilotSkillMultiplier - 1f))}";
          if (dupe_piloted & robo_piloted)
            tooltip2 = $"{tooltip2}\n    • {UI.CLUSTERMAP.ROCKETS.SPEED.SUPERPILOTED_SPEED_TOOLTIP.Replace("{speed_boost}", GameUtil.GetFormattedPercent(50f))}";
          else if (passengerModule && !dupe_piloted)
            tooltip2 = $"{tooltip2}\n    • {UI.CLUSTERMAP.ROCKETS.SPEED.UNPILOTED_SPEED_TOOLTIP.Replace("{speed_boost}", GameUtil.GetFormattedPercent(50f))}";
          else if (robo_piloted)
            tooltip2 = $"{tooltip2}\n    • {(string) UI.CLUSTERMAP.ROCKETS.SPEED.ROBO_PILOT_ONLY_SPEED_TOOLTIP}";
          else if (robotPilotModule && !passengerModule)
            tooltip2 = $"{tooltip2}\n    • {(string) UI.CLUSTERMAP.ROCKETS.SPEED.DEAD_ROBO_PILOT_ONLY_SPEED_TOOLTIP}";
        }
        rocketStatusContainer.SetLabel("Speed", (string) UI.CLUSTERMAP.ROCKETS.SPEED.NAME + GameUtil.GetFormattedRocketRangePerCycle(craftModuleInterface.Speed), tooltip2);
        if ((Object) craftModuleInterface.GetEngine() != (Object) null)
        {
          string tooltip3 = string.Format((string) UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.TOOLTIP, (object) craftModuleInterface.GetEngine().GetProperName(), (object) craftModuleInterface.MaxHeight.ToString());
          rocketStatusContainer.SetLabel("MaxHeight", string.Format((string) UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.NAME, (object) craftModuleInterface.RocketHeight.ToString(), (object) craftModuleInterface.MaxHeight.ToString()), tooltip3);
        }
        rocketStatusContainer.SetLabel("RocketSpacer2", "", "");
        if ((Object) clusterCraft != (Object) null)
        {
          foreach (KeyValuePair<string, GameObject> artifactModuleLabel in this.artifactModuleLabels)
            artifactModuleLabel.Value.SetActive(false);
          int num1 = 0;
          foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) clusterCraft.ModuleInterface.ClusterModules)
          {
            ArtifactModule component2 = clusterModule.Get().GetComponent<ArtifactModule>();
            if ((Object) component2 != (Object) null)
            {
              string text = !((Object) component2.Occupant != (Object) null) ? $"{component2.GetProperName()}: {UI.CLUSTERMAP.ROCKETS.ARTIFACT_MODULE.EMPTY}" : $"{component2.GetProperName()}: {component2.Occupant.GetProperName()}";
              rocketStatusContainer.SetLabel("artifactModule_" + num1.ToString(), text, "");
              ++num1;
            }
          }
          List<CargoBayCluster> allCargoBays = clusterCraft.GetAllCargoBays();
          bool flag = allCargoBays != null && allCargoBays.Count > 0;
          foreach (KeyValuePair<string, GameObject> cargoBayLabel in this.cargoBayLabels)
            cargoBayLabel.Value.SetActive(false);
          if (flag)
          {
            ListPool<Tuple<string, TextStyleSetting>, SimpleInfoScreen>.PooledList pooledList = ListPool<Tuple<string, TextStyleSetting>, SimpleInfoScreen>.Allocate();
            int num2 = 0;
            foreach (CargoBayCluster cargoBayCluster in allCargoBays)
            {
              pooledList.Clear();
              Storage storage = cargoBayCluster.storage;
              string text = $"{cargoBayCluster.GetComponent<KPrefabID>().GetProperName()}: {GameUtil.GetFormattedMass(storage.MassStored())}/{GameUtil.GetFormattedMass(storage.capacityKg)}";
              foreach (GameObject gameObject in storage.GetItems())
              {
                KPrefabID component3 = gameObject.GetComponent<KPrefabID>();
                PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
                string a = $"{component3.GetProperName()} : {GameUtil.GetFormattedMass(component4.Mass)}";
                pooledList.Add(new Tuple<string, TextStyleSetting>(a, PluginAssets.Instance.defaultTextStyleSetting));
              }
              string tooltip4 = "";
              for (int index = 0; index < pooledList.Count; ++index)
              {
                tooltip4 += pooledList[index].first;
                if (index != pooledList.Count - 1)
                  tooltip4 += "\n";
              }
              rocketStatusContainer.SetLabel("cargoBay_" + num2.ToString(), text, tooltip4);
              ++num2;
            }
            pooledList.Recycle();
          }
        }
      }
      if ((Object) rocketModuleCluster != (Object) null)
      {
        rocketStatusContainer.SetLabel("ModuleStats", (string) UI.CLUSTERMAP.ROCKETS.MODULE_STATS.NAME + selectedTarget.GetProperName(), (string) UI.CLUSTERMAP.ROCKETS.MODULE_STATS.TOOLTIP);
        float burden = rocketModuleCluster.performanceStats.Burden;
        float enginePower = rocketModuleCluster.performanceStats.EnginePower;
        if ((double) burden != 0.0)
          rocketStatusContainer.SetLabel("LocalBurden", $"    • {(string) UI.CLUSTERMAP.ROCKETS.BURDEN_MODULE.NAME}{burden.ToString()}", string.Format((string) UI.CLUSTERMAP.ROCKETS.BURDEN_MODULE.TOOLTIP, (object) burden));
        if ((double) enginePower != 0.0)
          rocketStatusContainer.SetLabel("LocalPower", $"    • {(string) UI.CLUSTERMAP.ROCKETS.POWER_MODULE.NAME}{enginePower.ToString()}", string.Format((string) UI.CLUSTERMAP.ROCKETS.POWER_MODULE.TOOLTIP, (object) enginePower));
      }
      rocketStatusContainer.Commit();
    }
  }

  public static void GetRocketStuffFromTarget(
    GameObject selectedTarget,
    ref RocketModuleCluster rocketModuleCluster,
    ref Clustercraft clusterCraft,
    ref CraftModuleInterface craftModuleInterface)
  {
    rocketModuleCluster = selectedTarget.GetComponent<RocketModuleCluster>();
    clusterCraft = selectedTarget.GetComponent<Clustercraft>();
    craftModuleInterface = (CraftModuleInterface) null;
    if ((Object) rocketModuleCluster != (Object) null)
    {
      craftModuleInterface = rocketModuleCluster.CraftInterface;
      if (!((Object) clusterCraft == (Object) null))
        return;
      clusterCraft = craftModuleInterface.GetComponent<Clustercraft>();
    }
    else
    {
      if (!((Object) clusterCraft != (Object) null))
        return;
      craftModuleInterface = clusterCraft.ModuleInterface;
    }
  }
}
