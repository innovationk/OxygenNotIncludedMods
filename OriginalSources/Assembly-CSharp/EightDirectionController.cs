// Decompiled with JetBrains decompiler
// Type: EightDirectionController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EightDirectionController
{
  public GameObject gameObject;
  private string defaultAnim;
  private KAnimLink link;

  public KBatchedAnimController controller { get; private set; }

  public EightDirectionController(
    KAnimControllerBase buildingController,
    string targetSymbol,
    string defaultAnim,
    EightDirectionController.Offset frontBank)
  {
    this.Initialize(buildingController, targetSymbol, defaultAnim, frontBank, Grid.SceneLayer.NoLayer);
  }

  private void Initialize(
    KAnimControllerBase buildingController,
    string targetSymbol,
    string defaultAnim,
    EightDirectionController.Offset frontBack,
    Grid.SceneLayer userSpecifiedRenderLayer)
  {
    string name = buildingController.name + ".eight_direction";
    this.gameObject = new GameObject(name);
    this.gameObject.SetActive(false);
    this.gameObject.transform.parent = buildingController.transform;
    this.gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
    this.defaultAnim = defaultAnim;
    this.controller = this.gameObject.AddOrGet<KBatchedAnimController>();
    this.controller.AnimFiles = new KAnimFile[1]
    {
      buildingController.AnimFiles[0]
    };
    this.controller.initialAnim = defaultAnim;
    this.controller.isMovable = true;
    this.controller.sceneLayer = Grid.SceneLayer.NoLayer;
    if (EightDirectionController.Offset.UserSpecified == frontBack)
      this.controller.sceneLayer = userSpecifiedRenderLayer;
    buildingController.SetSymbolVisiblity((KAnimHashedString) targetSymbol, false);
    Vector3 column = (Vector3) buildingController.GetSymbolTransform(new HashedString(targetSymbol), out bool _).GetColumn(3);
    switch (frontBack)
    {
      case EightDirectionController.Offset.Infront:
        column.z = buildingController.transform.GetPosition().z - 0.1f;
        break;
      case EightDirectionController.Offset.Behind:
        column.z = buildingController.transform.GetPosition().z + 0.1f;
        break;
      case EightDirectionController.Offset.UserSpecified:
        column.z = Grid.GetLayerZ(userSpecifiedRenderLayer);
        break;
    }
    this.gameObject.transform.SetPosition(column);
    this.gameObject.SetActive(true);
    this.link = new KAnimLink(buildingController, (KAnimControllerBase) this.controller);
  }

  public void SetPositionPercent(float percent_full)
  {
    if ((Object) this.controller == (Object) null)
      return;
    this.controller.SetPositionPercent(percent_full);
  }

  public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
  {
    if (!((Object) this.controller != (Object) null))
      return;
    this.controller.SetSymbolTint(symbol, (Color) colour);
  }

  public void SetRotation(float rot)
  {
    if ((Object) this.controller == (Object) null)
      return;
    this.controller.Rotation = rot;
  }

  public void PlayAnim(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
  {
    this.controller.Play((HashedString) anim, mode);
  }

  public enum Offset
  {
    Infront,
    Behind,
    UserSpecified,
  }
}
