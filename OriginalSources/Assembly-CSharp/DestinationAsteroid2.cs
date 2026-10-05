// Decompiled with JetBrains decompiler
// Type: DestinationAsteroid2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/DestinationAsteroid2")]
public class DestinationAsteroid2 : KMonoBehaviour
{
  [SerializeField]
  private Image asteroidImage;
  [SerializeField]
  private KButton button;
  [SerializeField]
  private KBatchedAnimController animController;
  [SerializeField]
  private Image imageDlcFrom;
  private ColonyDestinationAsteroidBeltData asteroidData;

  public event Action<ColonyDestinationAsteroidBeltData> OnClicked;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.button.onClick += new System.Action(this.OnClickInternal);
  }

  public void SetAsteroid(ColonyDestinationAsteroidBeltData newAsteroidData)
  {
    if (this.asteroidData != null && !(newAsteroidData.beltPath != this.asteroidData.beltPath))
      return;
    this.asteroidData = newAsteroidData;
    ProcGen.World getStartWorld = newAsteroidData.GetStartWorld;
    KAnimFile anim;
    Assets.TryGetAnim((HashedString) (getStartWorld.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : getStartWorld.asteroidIcon), out anim);
    if ((UnityEngine.Object) anim != (UnityEngine.Object) null)
    {
      this.asteroidImage.gameObject.SetActive(false);
      this.animController.AnimFiles = new KAnimFile[1]
      {
        anim
      };
      this.animController.initialMode = KAnim.PlayMode.Loop;
      this.animController.initialAnim = "idle_loop";
      this.animController.gameObject.SetActive(true);
      if (this.animController.HasAnimation((HashedString) this.animController.initialAnim))
        this.animController.Play((HashedString) this.animController.initialAnim, KAnim.PlayMode.Loop);
    }
    else
    {
      this.animController.gameObject.SetActive(false);
      this.asteroidImage.gameObject.SetActive(true);
      this.asteroidImage.sprite = this.asteroidData.sprite;
      this.imageDlcFrom.gameObject.SetActive(false);
    }
    Sprite sprite = (Sprite) null;
    if (DlcManager.IsDlcId(this.asteroidData.Layout.dlcIdFrom))
      sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(this.asteroidData.Layout.dlcIdFrom));
    if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
    {
      this.imageDlcFrom.gameObject.SetActive(true);
      this.imageDlcFrom.sprite = sprite;
    }
    else
    {
      this.imageDlcFrom.gameObject.SetActive(false);
      this.imageDlcFrom.sprite = sprite;
    }
  }

  private void OnClickInternal()
  {
    DebugUtil.LogArgs((object) "Clicked asteroid belt", (object) this.asteroidData.beltPath);
    this.OnClicked(this.asteroidData);
  }
}
