// Decompiled with JetBrains decompiler
// Type: EntityLuminescence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class EntityLuminescence : 
  GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.root;
  }

  public class Def : StateMachine.BaseDef
  {
    public Color lightColor;
    public float lightRange;
    public float lightAngle;
    public Vector2 lightOffset;
    public Vector2 lightDirection;
    public LightShape lightShape;
  }

  public new class Instance : 
    GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>.GameInstance
  {
    [MyCmpAdd]
    private Light2D light;
    private AttributeInstance luminescence;

    public Instance(IStateMachineTarget master, EntityLuminescence.Def def)
      : base(master, def)
    {
      this.light.Color = def.lightColor;
      this.light.Range = def.lightRange;
      this.light.Angle = def.lightAngle;
      this.light.Direction = def.lightDirection;
      this.light.Offset = def.lightOffset;
      this.light.shape = def.lightShape;
    }

    public override void StartSM()
    {
      base.StartSM();
      this.luminescence = Db.Get().Attributes.Luminescence.Lookup(this.gameObject);
      this.luminescence.OnDirty += new System.Action(this.OnLuminescenceChanged);
      this.RefreshLight();
    }

    private void OnLuminescenceChanged() => this.RefreshLight();

    public void RefreshLight()
    {
      if (this.luminescence == null)
        return;
      int totalValue = (int) this.luminescence.GetTotalValue();
      this.light.Lux = totalValue;
      bool flag = totalValue > 0;
      if (this.light.enabled == flag)
        return;
      this.light.enabled = flag;
    }

    protected override void OnCleanUp()
    {
      if (this.luminescence != null)
        this.luminescence.OnDirty -= new System.Action(this.OnLuminescenceChanged);
      base.OnCleanUp();
    }
  }
}
