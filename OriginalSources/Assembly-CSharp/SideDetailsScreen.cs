// Decompiled with JetBrains decompiler
// Type: SideDetailsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SideDetailsScreen : KScreen
{
  [SerializeField]
  private List<SideTargetScreen> screens;
  [SerializeField]
  private LocText title;
  [SerializeField]
  private KButton backButton;
  [SerializeField]
  private RectTransform body;
  private RectTransform rectTransform;
  private Dictionary<string, SideTargetScreen> screenMap;
  private SideTargetScreen activeScreen;
  public static SideDetailsScreen Instance;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    SideDetailsScreen.Instance = this;
    this.Initialize();
    this.gameObject.SetActive(false);
  }

  protected override void OnForcedCleanUp()
  {
    SideDetailsScreen.Instance = (SideDetailsScreen) null;
    base.OnForcedCleanUp();
  }

  private void Initialize()
  {
    if (this.screens == null)
      return;
    this.rectTransform = this.GetComponent<RectTransform>();
    this.screenMap = new Dictionary<string, SideTargetScreen>();
    List<SideTargetScreen> sideTargetScreenList = new List<SideTargetScreen>();
    foreach (Component screen in this.screens)
    {
      SideTargetScreen sideTargetScreen = Util.KInstantiateUI<SideTargetScreen>(screen.gameObject, this.body.gameObject);
      sideTargetScreen.gameObject.SetActive(false);
      sideTargetScreenList.Add(sideTargetScreen);
    }
    sideTargetScreenList.ForEach((Action<SideTargetScreen>) (s => this.screenMap.Add(s.name, s)));
    this.backButton.onClick += (System.Action) (() => this.Show(false));
  }

  public void SetTitle(string newTitle) => this.title.text = newTitle;

  public void SetScreen(string screenName, object content, float x)
  {
    if (!this.screenMap.ContainsKey(screenName))
      Debug.LogError((object) "Tried to open a screen that does exist on the manager!");
    else if (content == null)
    {
      Debug.LogError((object) $"Tried to set {screenName} with null content!");
    }
    else
    {
      if (!this.gameObject.activeInHierarchy)
        this.gameObject.SetActive(true);
      UnityEngine.Rect rect = this.rectTransform.rect;
      this.rectTransform.offsetMin = new Vector2(x, this.rectTransform.offsetMin.y);
      this.rectTransform.offsetMax = new Vector2(x + rect.width, this.rectTransform.offsetMax.y);
      if ((UnityEngine.Object) this.activeScreen != (UnityEngine.Object) null)
        this.activeScreen.gameObject.SetActive(false);
      this.activeScreen = this.screenMap[screenName];
      this.activeScreen.gameObject.SetActive(true);
      this.SetTitle(this.activeScreen.displayName);
      this.activeScreen.SetTarget(content);
    }
  }
}
