// Decompiled with JetBrains decompiler
// Type: SpaceArtifact
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SpaceArtifact")]
public class SpaceArtifact : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public const string ID = "SpaceArtifact";
  private const string charmedPrefix = "entombed_";
  private const string idlePrefix = "idle_";
  [SerializeField]
  private string ui_anim;
  [Serialize]
  private bool loadCharmed = true;
  public ArtifactTier artifactTier;
  public ArtifactType artifactType;
  public string uniqueAnimNameFragment;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.loadCharmed && DlcManager.IsExpansion1Active())
    {
      this.gameObject.AddTag(GameTags.CharmedArtifact);
      this.SetEntombedDecor();
    }
    else
    {
      this.loadCharmed = false;
      this.SetAnalyzedDecor();
    }
    this.UpdateStatusItem();
    Components.SpaceArtifacts.Add(this);
    this.UpdateAnim();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.SpaceArtifacts.Remove(this);
  }

  public void RemoveCharm()
  {
    this.gameObject.RemoveTag(GameTags.CharmedArtifact);
    this.UpdateStatusItem();
    this.loadCharmed = false;
    this.UpdateAnim();
    this.SetAnalyzedDecor();
  }

  private void SetEntombedDecor()
  {
    this.GetComponent<DecorProvider>().SetValues(TUNING.DECOR.BONUS.TIER0);
  }

  private void SetAnalyzedDecor()
  {
    this.GetComponent<DecorProvider>().SetValues(this.artifactTier.decorValues);
  }

  public void UpdateStatusItem()
  {
    if (this.gameObject.HasTag(GameTags.CharmedArtifact))
      this.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed);
    else
      this.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed);
  }

  public void SetArtifactTier(ArtifactTier tier) => this.artifactTier = tier;

  public ArtifactTier GetArtifactTier() => this.artifactTier;

  public void SetUIAnim(string anim) => this.ui_anim = anim;

  public string GetUIAnim() => this.ui_anim;

  public List<Descriptor> GetEffectDescriptions()
  {
    List<Descriptor> effectDescriptions = new List<Descriptor>();
    if (this.gameObject.HasTag(GameTags.CharmedArtifact))
    {
      Descriptor descriptor = new Descriptor(STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f)), STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE_TOOLTIP.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f)));
      effectDescriptions.Add(descriptor);
    }
    Descriptor descriptor1 = new Descriptor(string.Format("This is an artifact from space"), string.Format("This is the tooltip string"), Descriptor.DescriptorType.Information);
    effectDescriptions.Add(descriptor1);
    return effectDescriptions;
  }

  public List<Descriptor> GetDescriptors(GameObject go) => this.GetEffectDescriptions();

  private void UpdateAnim()
  {
    string anim_name = !this.gameObject.HasTag(GameTags.CharmedArtifact) ? this.uniqueAnimNameFragment : "entombed_" + this.uniqueAnimNameFragment.Replace("idle_", "");
    this.GetComponent<KBatchedAnimController>().Play((HashedString) anim_name, KAnim.PlayMode.Loop);
  }

  [OnDeserialized]
  public void OnDeserialize()
  {
    Pickupable component = this.GetComponent<Pickupable>();
    if (!((Object) component != (Object) null))
      return;
    component.deleteOffGrid = false;
  }
}
