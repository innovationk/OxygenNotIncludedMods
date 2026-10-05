// Decompiled with JetBrains decompiler
// Type: BuildingFacadeAnimateIn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildingFacadeAnimateIn : MonoBehaviour
{
  private KBatchedAnimController sourceAnimController;
  private KBatchedAnimController placeAnimController;
  private KBatchedAnimController colorAnimController;
  private Updater updater;

  private void Awake()
  {
    this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 1);
    this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 1);
    this.updater = Updater.Series(KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController), Updater.Do((System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject))));
  }

  private void Update()
  {
    if (this.sourceAnimController.IsNullOrDestroyed())
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, false);
      int num = (int) this.updater.Internal_Update(Time.unscaledDeltaTime);
    }
  }

  private void OnDisable()
  {
    if (!this.sourceAnimController.IsNullOrDestroyed())
      BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, true);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.placeAnimController.gameObject);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.colorAnimController.gameObject);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public static BuildingFacadeAnimateIn MakeFor(KBatchedAnimController sourceAnimController)
  {
    BuildingFacadeAnimateIn.SetVisibilityOn(sourceAnimController, false);
    KBatchedAnimController kbatchedAnimController1 = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
    kbatchedAnimController1.gameObject.name = "BuildingFacadeAnimateIn.placeAnimController";
    kbatchedAnimController1.initialAnim = "place";
    KBatchedAnimController kbatchedAnimController2 = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
    kbatchedAnimController2.gameObject.name = "BuildingFacadeAnimateIn.colorAnimController";
    kbatchedAnimController2.initialAnim = sourceAnimController.CurrentAnim != null ? sourceAnimController.CurrentAnim.name : sourceAnimController.AnimFiles[0].GetData().GetAnim(0).name;
    GameObject gameObject = new GameObject(nameof (BuildingFacadeAnimateIn));
    gameObject.SetActive(false);
    gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
    BuildingFacadeAnimateIn buildingFacadeAnimateIn = gameObject.AddComponent<BuildingFacadeAnimateIn>();
    buildingFacadeAnimateIn.sourceAnimController = sourceAnimController;
    buildingFacadeAnimateIn.placeAnimController = kbatchedAnimController1;
    buildingFacadeAnimateIn.colorAnimController = kbatchedAnimController2;
    kbatchedAnimController1.gameObject.SetActive(true);
    kbatchedAnimController2.gameObject.SetActive(true);
    gameObject.SetActive(true);
    return buildingFacadeAnimateIn;
  }

  private static void SetVisibilityOn(KBatchedAnimController animController, bool isVisible)
  {
    animController.SetVisiblity(isVisible);
    foreach (KBatchedAnimController componentsInChild in animController.GetComponentsInChildren<KBatchedAnimController>(true))
    {
      if (componentsInChild.batchGroupID == animController.batchGroupID)
        componentsInChild.SetVisiblity(isVisible);
    }
  }

  private static KBatchedAnimController SpawnAnimFrom(KBatchedAnimController sourceAnimController)
  {
    GameObject gameObject = new GameObject();
    gameObject.SetActive(false);
    gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
    gameObject.transform.localPosition = sourceAnimController.transform.localPosition;
    gameObject.transform.localRotation = sourceAnimController.transform.localRotation;
    gameObject.transform.localScale = sourceAnimController.transform.localScale;
    gameObject.layer = sourceAnimController.gameObject.layer;
    KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.materialType = sourceAnimController.materialType;
    kbatchedAnimController.initialMode = sourceAnimController.initialMode;
    kbatchedAnimController.AnimFiles = sourceAnimController.AnimFiles;
    kbatchedAnimController.Offset = sourceAnimController.Offset;
    kbatchedAnimController.animWidth = sourceAnimController.animWidth;
    kbatchedAnimController.animHeight = sourceAnimController.animHeight;
    kbatchedAnimController.animScale = sourceAnimController.animScale;
    kbatchedAnimController.sceneLayer = sourceAnimController.sceneLayer;
    kbatchedAnimController.fgLayer = sourceAnimController.fgLayer;
    kbatchedAnimController.FlipX = sourceAnimController.FlipX;
    kbatchedAnimController.FlipY = sourceAnimController.FlipY;
    kbatchedAnimController.Rotation = sourceAnimController.Rotation;
    kbatchedAnimController.Pivot = sourceAnimController.Pivot;
    return kbatchedAnimController;
  }
}
