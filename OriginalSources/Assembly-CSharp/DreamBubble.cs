// Decompiled with JetBrains decompiler
// Type: DreamBubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DreamBubble : KMonoBehaviour
{
  public KBatchedAnimController dreamBackgroundComponent;
  public KBatchedAnimController maskKanim;
  public KBatchedAnimController dreamBubbleBorderKanim;
  public KImage dreamContentComponent;
  private const string dreamBackgroundAnimationName = "dream_loop";
  private const string dreamMaskAnimationName = "dream_bubble_mask";
  private const string dreamBubbleBorderAnimationName = "dream_bubble_loop";
  private HashedString snapToPivotSymbol = new HashedString("snapto_pivot");
  private Dream _currentDream;
  private float _timePassedSinceDreamStarted;
  private Color _color = Color.white;
  private const float PI_2 = 6.28318548f;
  private const float HALF_PI = 1.57079637f;

  public bool IsVisible { private set; get; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.dreamBackgroundComponent.SetSymbolVisiblity((KAnimHashedString) this.snapToPivotSymbol, false);
    this.SetVisibility(false);
  }

  public void Tick(float dt)
  {
    if (this._currentDream == null || this._currentDream.Icons.Length == 0)
      return;
    double f = (double) this._timePassedSinceDreamStarted / (double) this._currentDream.secondPerImage;
    float num = (float) f - (float) Mathf.FloorToInt((float) f);
    int index = (int) Mathf.Repeat((float) Mathf.FloorToInt((float) f), (float) this._currentDream.Icons.Length);
    if ((Object) this.dreamContentComponent.sprite != (Object) this._currentDream.Icons[index])
      this.dreamContentComponent.sprite = this._currentDream.Icons[index];
    this.dreamContentComponent.rectTransform.localScale = Vector3.one * num;
    this._color.a = (float) (((double) Mathf.Sin((float) ((double) num * 6.2831854820251465 - 1.5707963705062866)) + 1.0) * 0.5);
    this.dreamContentComponent.color = this._color;
    this._timePassedSinceDreamStarted += dt;
  }

  public void SetDream(Dream dream)
  {
    this._currentDream = dream;
    this.dreamBackgroundComponent.Stop();
    this.dreamBackgroundComponent.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) dream.BackgroundAnim)
    };
    this.dreamContentComponent.color = this._color;
    this.dreamContentComponent.enabled = dream != null && dream.Icons != null && dream.Icons.Length != 0;
    this._timePassedSinceDreamStarted = 0.0f;
    this._color.a = 0.0f;
  }

  public void SetVisibility(bool visible)
  {
    this.IsVisible = visible;
    this.dreamBackgroundComponent.SetVisiblity(visible);
    this.dreamContentComponent.gameObject.SetActive(visible);
    if (visible)
    {
      if (this._currentDream != null)
        this.dreamBackgroundComponent.Play((HashedString) "dream_loop", KAnim.PlayMode.Loop);
      this.dreamBubbleBorderKanim.Play((HashedString) "dream_bubble_loop", KAnim.PlayMode.Loop);
      this.maskKanim.Play((HashedString) "dream_bubble_mask", KAnim.PlayMode.Loop);
    }
    else
    {
      this.dreamBackgroundComponent.Stop();
      this.maskKanim.Stop();
      this.dreamBubbleBorderKanim.Stop();
    }
  }

  public void StopDreaming()
  {
    this._currentDream = (Dream) null;
    this.SetVisibility(false);
  }
}
