// Decompiled with JetBrains decompiler
// Type: StatusItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class StatusItem : Resource
{
  public string tooltipText;
  public string notificationText;
  public string notificationTooltipText;
  public string soundPath;
  public string iconName;
  public bool unique;
  public TintedSprite sprite;
  public bool shouldNotify;
  public StatusItem.IconType iconType;
  public NotificationType notificationType;
  public Notification.ClickCallback notificationClickCallback;
  public Func<string, object, string> resolveStringCallback;
  public Func<string, object, string> resolveTooltipCallback;
  public bool resolveStringCallback_shouldStillCallIfDataIsNull;
  public bool resolveTooltipCallback_shouldStillCallIfDataIsNull;
  public bool allowMultiples;
  public Func<HashedString, object, bool> conditionalOverlayCallback;
  public HashedString render_overlay;
  public int status_overlays;
  public Action<object> statusItemClickCallback;
  private string composedPrefix;
  private bool showShowWorldIcon = true;
  public bool showInHoverCardOnly;
  public const int ALL_OVERLAYS = 129022;
  private static Dictionary<HashedString, StatusItem.StatusItemOverlays> overlayBitfieldMap = new Dictionary<HashedString, StatusItem.StatusItemOverlays>()
  {
    {
      OverlayModes.None.ID,
      StatusItem.StatusItemOverlays.None
    },
    {
      OverlayModes.Power.ID,
      StatusItem.StatusItemOverlays.PowerMap
    },
    {
      OverlayModes.Temperature.ID,
      StatusItem.StatusItemOverlays.Temperature
    },
    {
      OverlayModes.ThermalConductivity.ID,
      StatusItem.StatusItemOverlays.ThermalComfort
    },
    {
      OverlayModes.Light.ID,
      StatusItem.StatusItemOverlays.Light
    },
    {
      OverlayModes.LiquidConduits.ID,
      StatusItem.StatusItemOverlays.LiquidPlumbing
    },
    {
      OverlayModes.GasConduits.ID,
      StatusItem.StatusItemOverlays.GasPlumbing
    },
    {
      OverlayModes.SolidConveyor.ID,
      StatusItem.StatusItemOverlays.Conveyor
    },
    {
      OverlayModes.Decor.ID,
      StatusItem.StatusItemOverlays.Decor
    },
    {
      OverlayModes.Disease.ID,
      StatusItem.StatusItemOverlays.Pathogens
    },
    {
      OverlayModes.Crop.ID,
      StatusItem.StatusItemOverlays.Farming
    },
    {
      OverlayModes.Rooms.ID,
      StatusItem.StatusItemOverlays.Rooms
    },
    {
      OverlayModes.Suit.ID,
      StatusItem.StatusItemOverlays.Suits
    },
    {
      OverlayModes.Logic.ID,
      StatusItem.StatusItemOverlays.Logic
    },
    {
      OverlayModes.Oxygen.ID,
      StatusItem.StatusItemOverlays.None
    },
    {
      OverlayModes.TileMode.ID,
      StatusItem.StatusItemOverlays.None
    },
    {
      OverlayModes.Radiation.ID,
      StatusItem.StatusItemOverlays.Radiation
    }
  };

  private StatusItem(string id, string composed_prefix)
    : base(id, (string) Strings.Get(composed_prefix + ".NAME"))
  {
    this.composedPrefix = composed_prefix;
    this.tooltipText = (string) Strings.Get(composed_prefix + ".TOOLTIP");
  }

  private void SetIcon(
    string icon,
    StatusItem.IconType icon_type,
    NotificationType notification_type,
    bool allow_multiples,
    HashedString render_overlay,
    bool show_world_icon = true,
    int status_overlays = 129022,
    Func<string, object, string> resolve_string_callback = null)
  {
    switch (icon_type)
    {
      case StatusItem.IconType.Info:
        icon = "dash";
        break;
      case StatusItem.IconType.Exclamation:
        icon = "status_item_exclamation";
        break;
    }
    this.iconName = icon;
    this.notificationType = notification_type;
    this.sprite = Assets.GetTintedSprite(icon);
    if (this.sprite == null)
    {
      this.sprite = new TintedSprite();
      this.sprite.sprite = Assets.GetSprite((HashedString) icon);
      this.sprite.color = new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue);
    }
    this.iconType = icon_type;
    this.allowMultiples = allow_multiples;
    this.render_overlay = render_overlay;
    this.showShowWorldIcon = show_world_icon;
    this.status_overlays = status_overlays;
    this.resolveStringCallback = resolve_string_callback;
    if (this.sprite != null)
      return;
    Debug.LogWarning((object) $"Status item '{this.Id}' references a missing icon: {icon}");
  }

  public StatusItem(
    string id,
    string prefix,
    string icon,
    StatusItem.IconType icon_type,
    NotificationType notification_type,
    bool allow_multiples,
    HashedString render_overlay,
    bool showWorldIcon = true,
    int status_overlays = 129022,
    Func<string, object, string> resolve_string_callback = null)
    : this(id, $"STRINGS.{prefix}.STATUSITEMS.{id.ToUpper()}")
  {
    this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
  }

  public StatusItem(
    string id,
    string name,
    string tooltip,
    string icon,
    StatusItem.IconType icon_type,
    NotificationType notification_type,
    bool allow_multiples,
    HashedString render_overlay,
    int status_overlays = 129022,
    bool showWorldIcon = true,
    Func<string, object, string> resolve_string_callback = null)
    : base(id, name)
  {
    this.tooltipText = tooltip;
    this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
  }

  public void AddNotification(
    string sound_path = null,
    string notification_text = null,
    string notification_tooltip = null)
  {
    this.shouldNotify = true;
    this.soundPath = sound_path != null ? sound_path : (this.notificationType != NotificationType.Bad ? "Notification" : "Warning");
    if (notification_text != null)
    {
      this.notificationText = notification_text;
    }
    else
    {
      DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
      this.notificationText = (string) Strings.Get(this.composedPrefix + ".NOTIFICATION_NAME");
    }
    if (notification_tooltip != null)
    {
      this.notificationTooltipText = notification_tooltip;
    }
    else
    {
      DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
      this.notificationTooltipText = (string) Strings.Get(this.composedPrefix + ".NOTIFICATION_TOOLTIP");
    }
  }

  public virtual string GetName(object data) => this.ResolveString(this.Name, data);

  public virtual string GetTooltip(object data) => this.ResolveTooltip(this.tooltipText, data);

  private string ResolveString(string str, object data)
  {
    return this.resolveStringCallback != null && (data != null || this.resolveStringCallback_shouldStillCallIfDataIsNull) ? this.resolveStringCallback(str, data) : str;
  }

  private string ResolveTooltip(string str, object data)
  {
    if (data != null)
    {
      if (this.resolveTooltipCallback != null)
        return this.resolveTooltipCallback(str, data);
      if (this.resolveStringCallback != null)
        return this.resolveStringCallback(str, data);
    }
    else
    {
      if (this.resolveStringCallback_shouldStillCallIfDataIsNull && this.resolveStringCallback != null)
        return this.resolveStringCallback(str, data);
      if (this.resolveTooltipCallback_shouldStillCallIfDataIsNull && this.resolveTooltipCallback != null)
        return this.resolveTooltipCallback(str, data);
    }
    return str;
  }

  public bool ShouldShowIcon()
  {
    return this.iconType == StatusItem.IconType.Custom && this.showShowWorldIcon;
  }

  public virtual void ShowToolTip(
    ToolTip tooltip_widget,
    object data,
    TextStyleSetting property_style)
  {
    tooltip_widget.ClearMultiStringTooltip();
    string tooltip = this.GetTooltip(data);
    tooltip_widget.AddMultiStringTooltip(tooltip, property_style);
  }

  public void SetIcon(Image image, object data)
  {
    if (this.sprite == null)
      return;
    image.color = this.sprite.color;
    image.sprite = this.sprite.sprite;
  }

  public bool UseConditionalCallback(HashedString overlay, Transform transform)
  {
    return overlay != OverlayModes.None.ID && this.conditionalOverlayCallback != null && this.conditionalOverlayCallback(overlay, (object) transform);
  }

  public StatusItem SetResolveStringCallback(Func<string, object, string> cb)
  {
    this.resolveStringCallback = cb;
    return this;
  }

  public void OnClick(object data)
  {
    if (this.statusItemClickCallback == null)
      return;
    this.statusItemClickCallback(data);
  }

  public static StatusItem.StatusItemOverlays GetStatusItemOverlayBySimViewMode(HashedString mode)
  {
    StatusItem.StatusItemOverlays overlayBySimViewMode;
    if (!StatusItem.overlayBitfieldMap.TryGetValue(mode, out overlayBySimViewMode))
    {
      Debug.LogWarning((object) $"ViewMode {mode.ToString()} has no StatusItemOverlay value");
      overlayBySimViewMode = StatusItem.StatusItemOverlays.None;
    }
    return overlayBySimViewMode;
  }

  public enum IconType
  {
    Info,
    Exclamation,
    Custom,
  }

  [Flags]
  public enum StatusItemOverlays
  {
    None = 2,
    PowerMap = 4,
    Temperature = 8,
    ThermalComfort = 16, // 0x00000010
    Light = 32, // 0x00000020
    LiquidPlumbing = 64, // 0x00000040
    GasPlumbing = 128, // 0x00000080
    Decor = 256, // 0x00000100
    Pathogens = 512, // 0x00000200
    Farming = 1024, // 0x00000400
    Rooms = 4096, // 0x00001000
    Suits = 8192, // 0x00002000
    Logic = 16384, // 0x00004000
    Conveyor = 32768, // 0x00008000
    Radiation = 65536, // 0x00010000
  }
}
