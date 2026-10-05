// Decompiled with JetBrains decompiler
// Type: HelmetController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/HelmetController")]
public class HelmetController : KMonoBehaviour
{
  public const string JET_LEFT_SYMBOL_NAME = "left_fire";
  public const string JET_RIGHT_SYMBOL_NAME = "right_fire";
  public string anim_file;
  public bool has_jets;
  private bool is_shown;
  private bool in_tube;
  private bool is_flying;
  private Navigator owner_navigator;
  public KBatchedAnimController jet_anim;
  public KBatchedAnimController glow_anim;
  private static readonly EventSystem.IntraObjectHandler<HelmetController> OnEquippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>((Action<HelmetController, object>) ((component, data) => component.OnEquipped(data)));
  private static readonly EventSystem.IntraObjectHandler<HelmetController> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>((Action<HelmetController, object>) ((component, data) => component.OnUnequipped(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<HelmetController>(-1617557748, HelmetController.OnEquippedDelegate);
    this.Subscribe<HelmetController>(-170173755, HelmetController.OnUnequippedDelegate);
  }

  private KBatchedAnimController GetAssigneeController()
  {
    Equippable component = this.GetComponent<Equippable>();
    if (component.assignee != null)
    {
      GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
      if ((bool) (UnityEngine.Object) assigneeGameObject)
        return assigneeGameObject.GetComponent<KBatchedAnimController>();
    }
    return (KBatchedAnimController) null;
  }

  private GameObject GetAssigneeGameObject(IAssignableIdentity ass_id)
  {
    GameObject assigneeGameObject = (GameObject) null;
    MinionAssignablesProxy assignablesProxy = ass_id as MinionAssignablesProxy;
    if ((bool) (UnityEngine.Object) assignablesProxy)
    {
      assigneeGameObject = assignablesProxy.GetTargetGameObject();
    }
    else
    {
      MinionIdentity minionIdentity = ass_id as MinionIdentity;
      if ((bool) (UnityEngine.Object) minionIdentity)
        assigneeGameObject = minionIdentity.gameObject;
    }
    return assigneeGameObject;
  }

  private void OnEquipped(object data)
  {
    Equippable component = this.GetComponent<Equippable>();
    this.ShowHelmet();
    GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
    assigneeGameObject.Subscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
    assigneeGameObject.Subscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
    assigneeGameObject.Subscribe(1347184327, new Action<object>(this.OnPathAdvanced));
    this.in_tube = false;
    this.is_flying = false;
    this.owner_navigator = assigneeGameObject.GetComponent<Navigator>();
  }

  private void OnUnequipped(object data)
  {
    this.owner_navigator = (Navigator) null;
    Equippable component = this.GetComponent<Equippable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.HideHelmet();
    if (component.assignee == null)
      return;
    GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
    if (!(bool) (UnityEngine.Object) assigneeGameObject)
      return;
    assigneeGameObject.Unsubscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
    assigneeGameObject.Unsubscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
    assigneeGameObject.Unsubscribe(1347184327, new Action<object>(this.OnPathAdvanced));
  }

  private void ShowHelmet()
  {
    KBatchedAnimController assigneeController = this.GetAssigneeController();
    if ((UnityEngine.Object) assigneeController == (UnityEngine.Object) null)
      return;
    KAnimHashedString kanimHashedString = new KAnimHashedString("snapTo_neck");
    if (!string.IsNullOrEmpty(this.anim_file))
    {
      KAnimFile anim = Assets.GetAnim((HashedString) this.anim_file);
      assigneeController.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) kanimHashedString, anim.GetData().build.GetSymbol(kanimHashedString), 6);
    }
    assigneeController.SetSymbolVisiblity(kanimHashedString, true);
    this.is_shown = true;
    this.UpdateJets();
  }

  private void HideHelmet()
  {
    this.is_shown = false;
    KBatchedAnimController assigneeController = this.GetAssigneeController();
    if ((UnityEngine.Object) assigneeController == (UnityEngine.Object) null)
      return;
    KAnimHashedString kanimHashedString = (KAnimHashedString) "snapTo_neck";
    if (!string.IsNullOrEmpty(this.anim_file))
    {
      SymbolOverrideController component = assigneeController.GetComponent<SymbolOverrideController>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      component.RemoveSymbolOverride((HashedString) kanimHashedString, 6);
    }
    assigneeController.SetSymbolVisiblity(kanimHashedString, false);
    this.UpdateJets();
  }

  private void UpdateJets()
  {
    if (this.is_shown && this.is_flying)
      this.EnableJets();
    else
      this.DisableJets();
  }

  private void EnableJets()
  {
    if (!this.has_jets || (UnityEngine.Object) this.jet_anim != (UnityEngine.Object) null)
      return;
    this.jet_anim = this.AddTrackedAnim("jet", Assets.GetAnim((HashedString) "jetsuit_thruster_fx_kanim"), "loop", Grid.SceneLayer.Creatures, "snapTo_neck", true);
    this.glow_anim = this.AddTrackedAnim("glow", Assets.GetAnim((HashedString) "jetsuit_thruster_glow_fx_kanim"), "loop", Grid.SceneLayer.Front, "snapTo_neck");
  }

  private void DisableJets()
  {
    if (!this.has_jets)
      return;
    if ((UnityEngine.Object) this.jet_anim != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.jet_anim.gameObject);
      this.jet_anim = (KBatchedAnimController) null;
    }
    if (!((UnityEngine.Object) this.glow_anim != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.glow_anim.gameObject);
    this.glow_anim = (KBatchedAnimController) null;
  }

  private KBatchedAnimController AddTrackedAnim(
    string name,
    KAnimFile tracked_anim_file,
    string anim_clip,
    Grid.SceneLayer layer,
    string symbol_name,
    bool require_looping_sound = false)
  {
    KBatchedAnimController assigneeController = this.GetAssigneeController();
    if ((UnityEngine.Object) assigneeController == (UnityEngine.Object) null)
      return (KBatchedAnimController) null;
    string name1 = $"{assigneeController.name}.{name}";
    GameObject gameObject = new GameObject(name1);
    gameObject.SetActive(false);
    gameObject.transform.parent = assigneeController.transform;
    gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name1);
    KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      tracked_anim_file
    };
    kbatchedAnimController.initialAnim = anim_clip;
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.sceneLayer = layer;
    if (require_looping_sound)
      gameObject.AddComponent<LoopingSounds>();
    gameObject.AddComponent<KBatchedAnimTracker>().symbol = (HashedString) symbol_name;
    Vector3 column = (Vector3) assigneeController.GetSymbolTransform((HashedString) symbol_name, out bool _).GetColumn(3) with
    {
      z = Grid.GetLayerZ(layer)
    };
    gameObject.transform.SetPosition(column);
    gameObject.SetActive(true);
    kbatchedAnimController.Play((HashedString) anim_clip, KAnim.PlayMode.Loop);
    return kbatchedAnimController;
  }

  private void OnBeginRecoverBreath(object data) => this.HideHelmet();

  private void OnEndRecoverBreath(object data) => this.ShowHelmet();

  private void OnPathAdvanced(object data)
  {
    if ((UnityEngine.Object) this.owner_navigator == (UnityEngine.Object) null)
      return;
    bool flag1 = this.owner_navigator.CurrentNavType == NavType.Hover;
    bool flag2 = this.owner_navigator.CurrentNavType == NavType.Tube;
    if (flag2 != this.in_tube)
    {
      this.in_tube = flag2;
      if (this.in_tube)
        this.HideHelmet();
      else
        this.ShowHelmet();
    }
    if (flag1 == this.is_flying)
      return;
    this.is_flying = flag1;
    this.UpdateJets();
  }
}
