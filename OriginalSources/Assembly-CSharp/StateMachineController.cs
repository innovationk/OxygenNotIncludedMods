// Decompiled with JetBrains decompiler
// Type: StateMachineController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StateMachineController")]
public class StateMachineController : 
  KMonoBehaviour,
  ISaveLoadableDetails,
  IStateMachineControllerHack
{
  public DefHandle defHandle;
  private List<StateMachine.Instance> stateMachines = new List<StateMachine.Instance>();
  private LoggerFSSSS log = new LoggerFSSSS(nameof (StateMachineController));
  private StateMachineSerializer serializer = new StateMachineSerializer();
  private static readonly EventSystem.IntraObjectHandler<StateMachineController> OnTargetDestroyedDelegate = new EventSystem.IntraObjectHandler<StateMachineController>((Action<StateMachineController, object>) ((component, data) => component.OnTargetDestroyed(data)));

  public StateMachineController.CmpDef cmpdef
  {
    get => this.defHandle.Get<StateMachineController.CmpDef>();
  }

  public IEnumerator<StateMachine.Instance> GetEnumerator()
  {
    return (IEnumerator<StateMachine.Instance>) this.stateMachines.GetEnumerator();
  }

  public void AddStateMachineInstance(StateMachine.Instance state_machine)
  {
    if (this.stateMachines.Contains(state_machine))
      return;
    this.stateMachines.Add(state_machine);
    MyAttributes.OnAwake((object) state_machine, (KMonoBehaviour) this);
  }

  public void RemoveStateMachineInstance(StateMachine.Instance state_machine)
  {
    if (state_machine.GetStateMachine().saveHistory || state_machine.GetStateMachine().debugSettings.saveHistory)
      return;
    this.stateMachines.Remove(state_machine);
  }

  public bool HasStateMachineInstance(StateMachine.Instance state_machine)
  {
    return this.stateMachines.Contains(state_machine);
  }

  public void AddDef(StateMachine.BaseDef def) => this.cmpdef.defs.Add(def);

  public LoggerFSSSS GetLog() => this.log;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<StateMachineController>(1969584890, StateMachineController.OnTargetDestroyedDelegate);
    this.Subscribe<StateMachineController>(1502190696, StateMachineController.OnTargetDestroyedDelegate);
  }

  private void OnTargetDestroyed(object data)
  {
    while (this.stateMachines.Count > 0)
    {
      StateMachine.Instance stateMachine = this.stateMachines[0];
      stateMachine.StopSM("StateMachineController.OnCleanUp");
      this.stateMachines.Remove(stateMachine);
    }
    this.Unsubscribe<StateMachineController>(1969584890, StateMachineController.OnTargetDestroyedDelegate);
    this.Unsubscribe<StateMachineController>(1502190696, StateMachineController.OnTargetDestroyedDelegate);
  }

  protected override void OnLoadLevel()
  {
    while (this.stateMachines.Count > 0)
    {
      StateMachine.Instance stateMachine = this.stateMachines[0];
      stateMachine.FreeResources();
      this.stateMachines.Remove(stateMachine);
    }
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    while (this.stateMachines.Count > 0)
    {
      StateMachine.Instance stateMachine = this.stateMachines[0];
      stateMachine.StopSM("StateMachineController.OnCleanUp");
      this.stateMachines.Remove(stateMachine);
    }
  }

  public void CreateSMIS()
  {
    if (!this.defHandle.IsValid())
      return;
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
      def.CreateSMI((IStateMachineTarget) this);
  }

  public void StartSMIS()
  {
    if (!this.defHandle.IsValid())
      return;
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
    {
      if (!def.preventStartSMIOnSpawn)
      {
        StateMachine.Instance smi = this.GetSMI(Singleton<StateMachineManager>.Instance.CreateStateMachine(def.GetStateMachineType()).GetStateMachineInstanceType());
        if (smi != null && !smi.IsRunning())
          smi.StartSM();
      }
    }
  }

  public void Serialize(BinaryWriter writer)
  {
    this.serializer.Serialize(this.stateMachines, writer);
  }

  public void Deserialize(IReader reader) => this.serializer.Deserialize(reader);

  public bool Restore(StateMachine.Instance smi) => this.serializer.Restore(smi);

  public DefType GetDef<DefType>() where DefType : StateMachine.BaseDef
  {
    if (!this.defHandle.IsValid())
      return default (DefType);
    foreach (StateMachine.BaseDef def1 in this.cmpdef.defs)
    {
      if (def1 is DefType def2)
        return def2;
    }
    return default (DefType);
  }

  public InterfaceType GetDefImplementingInterfaceOfType<InterfaceType>() where InterfaceType : class
  {
    if (!this.defHandle.IsValid())
      return default (InterfaceType);
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
    {
      if (def is InterfaceType implementingInterfaceOfType)
        return implementingInterfaceOfType;
    }
    return default (InterfaceType);
  }

  public List<DefType> GetDefs<DefType>() where DefType : StateMachine.BaseDef
  {
    List<DefType> defs = new List<DefType>();
    if (!this.defHandle.IsValid())
      return defs;
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
    {
      if (def is DefType defType)
        defs.Add(defType);
    }
    return defs;
  }

  public StateMachine.Instance GetSMI(System.Type type)
  {
    for (int index = 0; index < this.stateMachines.Count; ++index)
    {
      StateMachine.Instance stateMachine = this.stateMachines[index];
      if (type.IsAssignableFrom(stateMachine.GetType()))
        return stateMachine;
    }
    return (StateMachine.Instance) null;
  }

  public StateMachineInstanceType GetSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
  {
    return this.GetSMI(typeof (StateMachineInstanceType)) as StateMachineInstanceType;
  }

  public List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
  {
    List<StateMachineInstanceType> allSmi = new List<StateMachineInstanceType>();
    foreach (StateMachine.Instance stateMachine in this.stateMachines)
    {
      if (stateMachine is StateMachineInstanceType machineInstanceType)
        allSmi.Add(machineInstanceType);
    }
    return allSmi;
  }

  public List<IGameObjectEffectDescriptor> GetDescriptors()
  {
    List<IGameObjectEffectDescriptor> descriptors = new List<IGameObjectEffectDescriptor>();
    if (!this.defHandle.IsValid())
      return descriptors;
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
    {
      if (def is IGameObjectEffectDescriptor)
        descriptors.Add(def as IGameObjectEffectDescriptor);
    }
    return descriptors;
  }

  public class CmpDef
  {
    public List<StateMachine.BaseDef> defs = new List<StateMachine.BaseDef>();
  }
}
