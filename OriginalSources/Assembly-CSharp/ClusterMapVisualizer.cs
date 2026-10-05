// Decompiled with JetBrains decompiler
// Type: ClusterMapVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ClusterMapVisualizer : KMonoBehaviour
{
  public KBatchedAnimController animControllerPrefab;
  public KBatchedAnimController peekControllerPrefab;
  public Transform nameTarget;
  public AlertVignette alertVignette;
  public bool doesTransitionAnimation;
  [HideInInspector]
  public Transform animContainer;
  private ClusterGridEntity entity;
  private ClusterMapPathDrawer pathDrawer;
  private ClusterMapPath mapPath;
  private List<KBatchedAnimController> animControllers;
  private bool isSelected;
  private ClusterRevealLevel lastRevealLevel;

  public void Init(ClusterGridEntity entity, ClusterMapPathDrawer pathDrawer)
  {
    this.entity = entity;
    this.pathDrawer = pathDrawer;
    this.animControllers = new List<KBatchedAnimController>();
    if ((UnityEngine.Object) this.animContainer == (UnityEngine.Object) null)
    {
      GameObject gameObject = new GameObject("AnimContainer", new System.Type[1]
      {
        typeof (RectTransform)
      });
      RectTransform component1 = this.GetComponent<RectTransform>();
      RectTransform component2 = gameObject.GetComponent<RectTransform>();
      component2.SetParent((Transform) component1, false);
      component2.SetLocalPosition(new Vector3(0.0f, 0.0f, 0.0f));
      component2.sizeDelta = component1.sizeDelta;
      component2.localScale = Vector3.one;
      this.animContainer = (Transform) component2;
    }
    Vector3 position = ClusterGrid.Instance.GetPosition(entity);
    this.rectTransform().SetLocalPosition(position);
    this.RefreshPathDrawing();
    entity.Subscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (!this.doesTransitionAnimation)
      return;
    new ClusterMapTravelAnimator.StatesInstance(this, this.entity).StartSM();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!((UnityEngine.Object) this.entity != (UnityEngine.Object) null))
      return;
    if (this.doesTransitionAnimation)
      this.gameObject.GetSMI<ClusterMapTravelAnimator.StatesInstance>().keepRotationOnIdle = this.entity.KeepRotationWhenSpacingOutInHex();
    if (this.entity is Clustercraft)
      new ClusterMapRocketAnimator.StatesInstance(this, this.entity).StartSM();
    else if (this.entity is ClusterMapLongRangeMissileGridEntity)
      new ClusterMapLongRangeMissileAnimator.StatesInstance(this, this.entity).StartSM();
    else if (this.entity is BallisticClusterGridEntity)
    {
      new ClusterMapBallisticAnimator.StatesInstance(this, this.entity).StartSM();
    }
    else
    {
      if (this.entity.Layer != EntityLayer.FX)
        return;
      new ClusterMapFXAnimator.StatesInstance(this, this.entity).StartSM();
    }
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.mapPath != (UnityEngine.Object) null)
      Util.KDestroyGameObject((Component) this.mapPath);
    if ((UnityEngine.Object) this.entity != (UnityEngine.Object) null)
      this.entity.Unsubscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
    base.OnCleanUp();
  }

  private void OnClusterDestinationChanged(object _) => this.RefreshPathDrawing();

  public void Select(bool selected)
  {
    if (this.animControllers == null || this.animControllers.Count == 0)
      return;
    if (!selected == this.isSelected)
    {
      this.isSelected = selected;
      this.RefreshPathDrawing();
    }
    this.GetFirstAnimController().SetSymbolVisiblity((KAnimHashedString) nameof (selected), selected);
  }

  public void PlayAnim(string animName, KAnim.PlayMode playMode)
  {
    if (this.animControllers.Count <= 0)
      return;
    this.GetFirstAnimController().Play((HashedString) animName, playMode);
  }

  public KBatchedAnimController GetFirstAnimController() => this.GetAnimController(0);

  public KBatchedAnimController GetAnimController(int index)
  {
    return index < this.animControllers.Count ? this.animControllers[index] : (KBatchedAnimController) null;
  }

  public void ManualAddAnimController(KBatchedAnimController externalAnimController)
  {
    this.animControllers.Add(externalAnimController);
  }

  public void Show(ClusterRevealLevel level)
  {
    if (!this.entity.IsVisible)
      level = ClusterRevealLevel.Hidden;
    if (level == this.lastRevealLevel)
      return;
    this.lastRevealLevel = level;
    switch (level)
    {
      case ClusterRevealLevel.Hidden:
        this.gameObject.SetActive(false);
        break;
      case ClusterRevealLevel.Peeked:
        this.ClearAnimControllers();
        KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.peekControllerPrefab, this.animContainer);
        kbatchedAnimController.gameObject.SetActive(true);
        this.animControllers.Add(kbatchedAnimController);
        this.gameObject.SetActive(true);
        break;
      case ClusterRevealLevel.Visible:
        this.ClearAnimControllers();
        if ((UnityEngine.Object) this.animControllerPrefab != (UnityEngine.Object) null && this.entity.AnimConfigs != null)
        {
          foreach (ClusterGridEntity.AnimConfig animConfig in this.entity.AnimConfigs)
          {
            KBatchedAnimController controller = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.animControllerPrefab, this.animContainer);
            controller.SwapAnims(new KAnimFile[1]
            {
              animConfig.animFile
            });
            controller.initialMode = animConfig.playMode;
            controller.initialAnim = animConfig.initialAnim;
            controller.Offset = animConfig.animOffset;
            controller.gameObject.AddComponent<LoopingSounds>();
            if ((double) animConfig.animPlaySpeedModifier != 0.0)
              controller.PlaySpeedMultiplier = animConfig.animPlaySpeedModifier;
            if (!string.IsNullOrEmpty(animConfig.symbolSwapTarget) && !string.IsNullOrEmpty(animConfig.symbolSwapSymbol))
            {
              SymbolOverrideController component = controller.GetComponent<SymbolOverrideController>();
              KAnim.Build.Symbol symbol = controller.AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) animConfig.symbolSwapSymbol);
              HashedString symbolSwapTarget = (HashedString) animConfig.symbolSwapTarget;
              KAnim.Build.Symbol source_symbol = symbol;
              component.AddSymbolOverride(symbolSwapTarget, source_symbol);
            }
            controller.gameObject.SetActive(true);
            this.animControllers.Add(controller);
            this.entity.onClustermapVisualizerAnimCreated(controller, animConfig);
          }
        }
        this.gameObject.SetActive(true);
        break;
    }
    this.entity.OnClusterMapIconShown(level);
  }

  public void RefreshPathDrawing()
  {
    if ((UnityEngine.Object) this.entity == (UnityEngine.Object) null)
      return;
    ClusterTraveler component = this.entity.GetComponent<ClusterTraveler>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    List<AxialI> currentPath = !this.entity.IsVisible || !component.IsTraveling() ? (List<AxialI>) null : component.CurrentPath;
    if (currentPath != null && currentPath.Count > 0)
    {
      if ((UnityEngine.Object) this.mapPath == (UnityEngine.Object) null)
        this.mapPath = this.pathDrawer.AddPath();
      this.mapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList((Vector2) this.transform.GetLocalPosition(), currentPath));
      this.mapPath.SetColor(!this.isSelected ? (!this.entity.ShowPath() ? new Color(0.0f, 0.0f, 0.0f, 0.0f) : ClusterMapScreen.Instance.rocketPathColor) : ClusterMapScreen.Instance.rocketSelectedPathColor);
    }
    else
    {
      if (!((UnityEngine.Object) this.mapPath != (UnityEngine.Object) null))
        return;
      Util.KDestroyGameObject((Component) this.mapPath);
      this.mapPath = (ClusterMapPath) null;
    }
  }

  public void SetAnimRotation(float rotation)
  {
    this.animContainer.localRotation = Quaternion.Euler(0.0f, 0.0f, rotation);
  }

  public float GetPathAngle()
  {
    return (UnityEngine.Object) this.mapPath == (UnityEngine.Object) null ? 0.0f : this.mapPath.GetRotationForNextSegment();
  }

  private void ClearAnimControllers()
  {
    if (this.animControllers == null)
      return;
    foreach (Component animController in this.animControllers)
      Util.KDestroyGameObject(animController.gameObject);
    this.animControllers.Clear();
  }

  private class UpdateXPositionParameter : LoopingSoundParameterUpdater
  {
    private List<ClusterMapVisualizer.UpdateXPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateXPositionParameter.Entry>();

    public UpdateXPositionParameter()
      : base((HashedString) "Starmap_Position_X")
    {
    }

    public override void Add(LoopingSoundParameterUpdater.Sound sound)
    {
      this.entries.Add(new ClusterMapVisualizer.UpdateXPositionParameter.Entry()
      {
        transform = sound.transform,
        ev = sound.ev,
        parameterId = sound.description.GetParameterId(this.parameter)
      });
    }

    public override void Update(float dt)
    {
      foreach (ClusterMapVisualizer.UpdateXPositionParameter.Entry entry in this.entries)
      {
        if (!((UnityEngine.Object) entry.transform == (UnityEngine.Object) null))
        {
          int num = (int) entry.ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().x / (float) Screen.width);
        }
      }
    }

    public override void Remove(LoopingSoundParameterUpdater.Sound sound)
    {
      for (int index = 0; index < this.entries.Count; ++index)
      {
        if (this.entries[index].ev.handle == sound.ev.handle)
        {
          this.entries.RemoveAt(index);
          break;
        }
      }
    }

    private struct Entry
    {
      public Transform transform;
      public EventInstance ev;
      public PARAMETER_ID parameterId;
    }
  }

  private class UpdateYPositionParameter : LoopingSoundParameterUpdater
  {
    private List<ClusterMapVisualizer.UpdateYPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateYPositionParameter.Entry>();

    public UpdateYPositionParameter()
      : base((HashedString) "Starmap_Position_Y")
    {
    }

    public override void Add(LoopingSoundParameterUpdater.Sound sound)
    {
      this.entries.Add(new ClusterMapVisualizer.UpdateYPositionParameter.Entry()
      {
        transform = sound.transform,
        ev = sound.ev,
        parameterId = sound.description.GetParameterId(this.parameter)
      });
    }

    public override void Update(float dt)
    {
      foreach (ClusterMapVisualizer.UpdateYPositionParameter.Entry entry in this.entries)
      {
        if (!((UnityEngine.Object) entry.transform == (UnityEngine.Object) null))
        {
          int num = (int) entry.ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().y / (float) Screen.height);
        }
      }
    }

    public override void Remove(LoopingSoundParameterUpdater.Sound sound)
    {
      for (int index = 0; index < this.entries.Count; ++index)
      {
        if (this.entries[index].ev.handle == sound.ev.handle)
        {
          this.entries.RemoveAt(index);
          break;
        }
      }
    }

    private struct Entry
    {
      public Transform transform;
      public EventInstance ev;
      public PARAMETER_ID parameterId;
    }
  }

  private class UpdateZoomPercentageParameter : LoopingSoundParameterUpdater
  {
    private List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry>();

    public UpdateZoomPercentageParameter()
      : base((HashedString) "Starmap_Zoom_Percentage")
    {
    }

    public override void Add(LoopingSoundParameterUpdater.Sound sound)
    {
      this.entries.Add(new ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry()
      {
        ev = sound.ev,
        parameterId = sound.description.GetParameterId(this.parameter)
      });
    }

    public override void Update(float dt)
    {
      foreach (ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry entry in this.entries)
      {
        int num = (int) entry.ev.setParameterByID(entry.parameterId, ClusterMapScreen.Instance.CurrentZoomPercentage());
      }
    }

    public override void Remove(LoopingSoundParameterUpdater.Sound sound)
    {
      for (int index = 0; index < this.entries.Count; ++index)
      {
        if (this.entries[index].ev.handle == sound.ev.handle)
        {
          this.entries.RemoveAt(index);
          break;
        }
      }
    }

    private struct Entry
    {
      public Transform transform;
      public EventInstance ev;
      public PARAMETER_ID parameterId;
    }
  }
}
