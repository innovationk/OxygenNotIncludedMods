// Decompiled with JetBrains decompiler
// Type: VideoWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/VideoWidget")]
public class VideoWidget : KMonoBehaviour
{
  [SerializeField]
  private VideoClip clip;
  [SerializeField]
  private VideoPlayer thumbnailPlayer;
  [SerializeField]
  private KButton button;
  [SerializeField]
  private string overlayName;
  [SerializeField]
  private List<string> texts;
  private RenderTexture renderTexture;
  private RawImage rawImage;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.button.onClick += new System.Action(this.Clicked);
    this.rawImage = this.thumbnailPlayer.GetComponent<RawImage>();
  }

  private void Clicked()
  {
    VideoScreen.Instance.PlayVideo(this.clip);
    if (string.IsNullOrEmpty(this.overlayName))
      return;
    VideoScreen.Instance.SetOverlayText(this.overlayName, this.texts);
  }

  public void SetClip(VideoClip clip, string overlayName = null, List<string> texts = null)
  {
    if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "Tried to assign null video clip to VideoWidget");
    }
    else
    {
      this.clip = clip;
      this.overlayName = overlayName;
      this.texts = texts;
      this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16 /*0x10*/);
      this.thumbnailPlayer.targetTexture = this.renderTexture;
      this.rawImage.texture = (Texture) this.renderTexture;
      this.StartCoroutine(this.ConfigureThumbnail());
    }
  }

  private IEnumerator ConfigureThumbnail()
  {
    this.thumbnailPlayer.audioOutputMode = VideoAudioOutputMode.None;
    this.thumbnailPlayer.clip = this.clip;
    this.thumbnailPlayer.time = 0.0;
    this.thumbnailPlayer.Play();
    yield return (object) null;
  }

  private void Update()
  {
    if (!this.thumbnailPlayer.isPlaying || this.thumbnailPlayer.time <= 2.0)
      return;
    this.thumbnailPlayer.Pause();
  }
}
