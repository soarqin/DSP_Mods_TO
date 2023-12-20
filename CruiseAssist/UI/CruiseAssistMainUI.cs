using System;
using System.Collections.Generic;
using System.Linq;
using CruiseAssist.Commons;
using CruiseAssist.Enums;
using UnityEngine;

namespace CruiseAssist.UI;

public static class CruiseAssistMainUI
{
    public static void OnGUI()
    {
        if (WhiteBorderBackgroundTexture == null)
        {
            WhiteBorderBackgroundTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color2 = new Color32(0, 0, 0, 224);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    var color3 = i <= 0 || j <= 0 || i >= 63 || j >= 63 ? color : color2;
                    WhiteBorderBackgroundTexture.SetPixel(j, i, color3);
                }
            }
            WhiteBorderBackgroundTexture.Apply();
        }

        if (GrayBorderBackgroundTexture == null)
        {
            GrayBorderBackgroundTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color4 = new Color32(64, 64, 64, byte.MaxValue);
            var color5 = new Color32(0, 0, 0, 224);
            for (var k = 0; k < 64; k++)
            {
                for (var l = 0; l < 64; l++)
                {
                    var color6 = k <= 0 || l <= 0 || k >= 63 || l >= 63 ? color4 : color5;
                    GrayBorderBackgroundTexture.SetPixel(l, k, color6);
                }
            }
            GrayBorderBackgroundTexture.Apply();
        }

        if (WhiteBorderTexture == null)
        {
            WhiteBorderTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color7 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color8 = new Color32(0, 0, 0, byte.MaxValue);
            for (var m = 0; m < 64; m++)
            {
                for (var n = 0; n < 64; n++)
                {
                    var color9 = m <= 0 || n <= 0 || m >= 63 || n >= 63 ? color7 : color8;
                    WhiteBorderTexture.SetPixel(n, m, color9);
                }
            }
            WhiteBorderTexture.Apply();
        }

        if (GrayBorderTexture == null)
        {
            GrayBorderTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color10 = new Color32(64, 64, 64, byte.MaxValue);
            var color11 = new Color32(0, 0, 0, byte.MaxValue);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    var color12 = i <= 0 || j <= 0 || i >= 63 || j >= 63 ? color10 : color11;
                    GrayBorderTexture.SetPixel(j, i, color12);
                }
            }
            GrayBorderTexture.Apply();
        }

        if (BlackTexture == null)
        {
            BlackTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color13 = new Color32(0, 0, 0, byte.MaxValue);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    BlackTexture.SetPixel(j, i, color13);
                }
            }
            BlackTexture.Apply();
        }

        if (WhiteTexture == null)
        {
            WhiteTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color14 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    WhiteTexture.SetPixel(j, i, color14);
                }
            }
            WhiteTexture.Apply();
        }

        if (ToggleOnTexture == null)
        {
            ToggleOnTexture = new Texture2D(16, 16, TextureFormat.RGBA32, false);
            var color15 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color16 = new Color32(0, 0, 0, 0);
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var color17 = j is >= 1 and <= 12 && i is >= 2 and <= 13 ? color15 : color16;
                    ToggleOnTexture.SetPixel(j, i, color17);
                }
            }
            ToggleOnTexture.Apply();
        }

        if (ToggleOffTexture == null)
        {
            ToggleOffTexture = new Texture2D(16, 16, TextureFormat.RGBA32, false);
            var color18 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color19 = new Color32(0, 0, 0, byte.MaxValue);
            var color20 = new Color32(0, 0, 0, 0);
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var color21 = j < 1 || j > 12 || i < 2 || i > 13 ? color20 : j is > 1 and < 12 && i is > 2 and < 13 ? color19 : color18;
                    ToggleOffTexture.SetPixel(j, i, color21);
                }
            }
            ToggleOffTexture.Apply();
        }

        if (CloseButtonGrayBorderTexture != null)
        {
            CloseButtonGrayBorderTexture = new Texture2D(16, 16, TextureFormat.RGBA32, false);
            var color22 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color23 = new Color32(64, 64, 64, byte.MaxValue);
            var color24 = new Color32(0, 0, 0, byte.MaxValue);
            var color25 = new Color32(0, 0, 0, 0);
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var color26 = j < 1 || j > 12 || i < 2 || i > 13 ? color25 : j is > 1 and < 12 && i is > 2 and < 13 ? color24 : color23;
                    CloseButtonGrayBorderTexture.SetPixel(j, i, color26);
                }
            }
            for (var i = 4; i <= 9; i++)
            {
                CloseButtonGrayBorderTexture.SetPixel(i, i + 1, color22);
                CloseButtonGrayBorderTexture.SetPixel(i, 14 - i, color22);
            }
            CloseButtonGrayBorderTexture.Apply();
        }

        if (CloseButtonWhiteBorderTexture == null)
        {
            CloseButtonWhiteBorderTexture = new Texture2D(16, 16, TextureFormat.RGBA32, false);
            var color27 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color28 = new Color32(0, 0, 0, byte.MaxValue);
            var color29 = new Color32(0, 0, 0, 0);
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var color30 = j < 1 || j > 12 || i < 2 || i > 13 ? color29 : j is > 1 and < 12 && i is > 2 and < 13 ? color28 : color27;
                    CloseButtonWhiteBorderTexture.SetPixel(j, i, color30);
                }
            }
            for (var i = 4; i <= 9; i++)
            {
                CloseButtonWhiteBorderTexture.SetPixel(i, i + 1, color27);
                CloseButtonWhiteBorderTexture.SetPixel(i, 14 - i, color27);
            }
            CloseButtonWhiteBorderTexture.Apply();
        }

        WindowStyle ??= new GUIStyle(GUI.skin.window)
        {
            fontSize = 11,
            normal =
            {
                textColor = Color.white,
                background = GrayBorderBackgroundTexture
            },
            hover =
            {
                textColor = Color.white,
                background = GrayBorderBackgroundTexture
            },
            active =
            {
                textColor = Color.white,
                background = GrayBorderBackgroundTexture
            },
            focused =
            {
                textColor = Color.white,
                background = GrayBorderBackgroundTexture
            },
            onNormal =
            {
                textColor = Color.white,
                background = WhiteBorderBackgroundTexture
            },
            onHover =
            {
                textColor = Color.white,
                background = WhiteBorderBackgroundTexture
            },
            onActive =
            {
                textColor = Color.white,
                background = WhiteBorderBackgroundTexture
            },
            onFocused =
            {
                textColor = Color.white,
                background = WhiteBorderBackgroundTexture
            }
        };

        BaseButtonStyle ??= new GUIStyle(GUI.skin.button)
        {
            normal =
            {
                textColor = Color.white,
                background = GrayBorderTexture
            },
            hover =
            {
                textColor = Color.white,
                background = WhiteBorderTexture
            },
            active =
            {
                textColor = Color.white,
                background = WhiteBorderTexture
            },
            focused =
            {
                textColor = Color.white,
                background = WhiteBorderTexture
            },
            onNormal =
            {
                textColor = Color.white,
                background = GrayBorderTexture
            },
            onHover =
            {
                textColor = Color.white,
                background = WhiteBorderTexture
            },
            onActive =
            {
                textColor = Color.white,
                background = WhiteBorderTexture
            },
            onFocused =
            {
                textColor = Color.white,
                background = WhiteBorderTexture
            }
        };

        BaseToolbarButtonStyle ??= new GUIStyle(BaseButtonStyle)
        {
            normal =
            {
                textColor = Color.gray
            },
            hover =
            {
                textColor = Color.gray
            },
            active =
            {
                textColor = Color.gray
            },
            focused =
            {
                textColor = Color.gray
            },
            onNormal =
            {
                background = WhiteBorderBackgroundTexture
            }
        };

        BaseVerticalScrollBarStyle ??= new GUIStyle(GUI.skin.verticalScrollbar)
        {
            name = "cruiseassist.verticalscrollbar",
            normal =
            {
                background = GrayBorderTexture
            },
            hover =
            {
                background = GrayBorderTexture
            },
            active =
            {
                background = GrayBorderTexture
            },
            focused =
            {
                background = GrayBorderTexture
            },
            onNormal =
            {
                background = GrayBorderTexture
            },
            onHover =
            {
                background = GrayBorderTexture
            },
            onActive =
            {
                background = GrayBorderTexture
            },
            onFocused =
            {
                background = GrayBorderTexture
            }
        };

        BaseHorizontalSliderStyle ??= new GUIStyle(GUI.skin.horizontalSlider)
        {
            normal =
            {
                background = GrayBorderTexture
            },
            hover =
            {
                background = GrayBorderTexture
            },
            active =
            {
                background = GrayBorderTexture
            },
            focused =
            {
                background = GrayBorderTexture
            },
            onNormal =
            {
                background = GrayBorderTexture
            },
            onHover =
            {
                background = GrayBorderTexture
            },
            onActive =
            {
                background = GrayBorderTexture
            },
            onFocused =
            {
                background = GrayBorderTexture
            }
        };

        BaseHorizontalSliderThumbStyle ??= new GUIStyle(GUI.skin.horizontalSliderThumb)
        {
            normal =
            {
                background = WhiteBorderTexture
            },
            hover =
            {
                background = WhiteBorderTexture
            },
            active =
            {
                background = WhiteBorderTexture
            },
            focused =
            {
                background = WhiteBorderTexture
            },
            onNormal =
            {
                background = WhiteBorderTexture
            },
            onHover =
            {
                background = WhiteBorderTexture
            },
            onActive =
            {
                background = WhiteBorderTexture
            },
            onFocused =
            {
                background = WhiteBorderTexture
            }
        };

        BaseToggleStyle ??= new GUIStyle(GUI.skin.toggle)
        {
            normal =
            {
                background = ToggleOffTexture
            },
            hover =
            {
                background = ToggleOffTexture
            },
            active =
            {
                background = ToggleOffTexture
            },
            focused =
            {
                background = ToggleOffTexture
            },
            onNormal =
            {
                background = ToggleOnTexture
            },
            onHover =
            {
                background = ToggleOnTexture
            },
            onActive =
            {
                background = ToggleOnTexture
            },
            onFocused =
            {
                background = ToggleOnTexture
            }
        };

        BaseTextFieldStyle ??= new GUIStyle(GUI.skin.textField)
        {
            normal =
            {
                background = WhiteBorderTexture
            },
            hover =
            {
                background = WhiteBorderTexture
            },
            active =
            {
                background = WhiteBorderTexture
            },
            focused =
            {
                background = WhiteBorderTexture
            },
            onNormal =
            {
                background = WhiteBorderTexture
            },
            onHover =
            {
                background = WhiteBorderTexture
            },
            onActive =
            {
                background = WhiteBorderTexture
            },
            onFocused =
            {
                background = WhiteBorderTexture
            }
        };

        CloseButtonStyle ??= new GUIStyle(GUI.skin.button)
        {
            normal =
            {
                background = CloseButtonGrayBorderTexture
            },
            hover =
            {
                background = CloseButtonWhiteBorderTexture
            },
            active =
            {
                background = CloseButtonWhiteBorderTexture
            },
            focused =
            {
                background = CloseButtonWhiteBorderTexture
            },
            onNormal =
            {
                background = CloseButtonGrayBorderTexture
            },
            onHover =
            {
                background = CloseButtonWhiteBorderTexture
            },
            onActive =
            {
                background = CloseButtonWhiteBorderTexture
            },
            onFocused =
            {
                background = CloseButtonWhiteBorderTexture
            }
        };

        if (_verticalScrollBarSkins == null)
        {
            _verticalScrollBarSkins = new List<GUIStyle>();
            var guistyle = new GUIStyle(GUI.skin.verticalScrollbarThumb)
            {
                name = "cruiseassist.verticalscrollbarthumb",
                normal =
                {
                    background = WhiteBorderTexture
                },
                hover =
                {
                    background = WhiteBorderTexture
                },
                active =
                {
                    background = WhiteBorderTexture
                },
                focused =
                {
                    background = WhiteBorderTexture
                },
                onNormal =
                {
                    background = WhiteBorderTexture
                },
                onHover =
                {
                    background = WhiteBorderTexture
                },
                onActive =
                {
                    background = WhiteBorderTexture
                },
                onFocused =
                {
                    background = WhiteBorderTexture
                }
            };
            _verticalScrollBarSkins.Add(guistyle);
            var guistyle2 = new GUIStyle(GUI.skin.verticalScrollbarUpButton)
            {
                name = "cruiseassist.verticalscrollbarupbutton",
                normal =
                {
                    background = BlackTexture
                },
                hover =
                {
                    background = BlackTexture
                },
                active =
                {
                    background = BlackTexture
                },
                focused =
                {
                    background = BlackTexture
                },
                onNormal =
                {
                    background = BlackTexture
                },
                onHover =
                {
                    background = BlackTexture
                },
                onActive =
                {
                    background = BlackTexture
                },
                onFocused =
                {
                    background = BlackTexture
                }
            };
            _verticalScrollBarSkins.Add(guistyle2);
            var guistyle3 = new GUIStyle(GUI.skin.verticalScrollbarDownButton)
            {
                name = "cruiseassist.verticalscrollbardownbutton",
                normal =
                {
                    background = BlackTexture
                },
                hover =
                {
                    background = BlackTexture
                },
                active =
                {
                    background = BlackTexture
                },
                focused =
                {
                    background = BlackTexture
                },
                onNormal =
                {
                    background = BlackTexture
                },
                onHover =
                {
                    background = BlackTexture
                },
                onActive =
                {
                    background = BlackTexture
                },
                onFocused =
                {
                    background = BlackTexture
                }
            };
            _verticalScrollBarSkins.Add(guistyle3);
            GUI.skin.customStyles = GUI.skin.customStyles.Concat(_verticalScrollBarSkins).ToArray();
        }
        switch (ViewMode)
        {
            case CruiseAssistMainUIViewMode.Full:
                Rect[WIdx].width = 398f;
                Rect[WIdx].height = 150f;
                break;
            case CruiseAssistMainUIViewMode.Mini:
                Rect[WIdx].width = 288f;
                Rect[WIdx].height = 70f;
                break;
        }
        Rect[WIdx] = GUILayout.Window(99030291, Rect[WIdx], WindowFunction, "CruiseAssist", WindowStyle, Array.Empty<GUILayoutOption>());
        var scale = Scale / 100f;
        if (Screen.width / scale < Rect[WIdx].xMax)
        {
            Rect[WIdx].x = Screen.width / scale - Rect[WIdx].width;
        }

        if (Rect[WIdx].x < 0f)
        {
            Rect[WIdx].x = 0f;
        }

        if (Screen.height / scale < Rect[WIdx].yMax)
        {
            Rect[WIdx].y = Screen.height / scale - Rect[WIdx].height;
        }

        if (Rect[WIdx].y < 0f)
        {
            Rect[WIdx].y = 0f;
        }

        if (LastCheckWindowLeft[WIdx] != float.MinValue)
        {
            if (Rect[WIdx].x != LastCheckWindowLeft[WIdx] || Rect[WIdx].y != LastCheckWindowTop[WIdx])
            {
                NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        LastCheckWindowLeft[WIdx] = Rect[WIdx].x;
        LastCheckWindowTop[WIdx] = Rect[WIdx].y;
        if (NextCheckGameTick <= GameMain.gameTick)
        {
            ConfigManager.CheckConfig(ConfigManager.Step.State);
        }
    }

    private static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        if (ViewMode == CruiseAssistMainUIViewMode.Full)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            var color = CruiseAssistPlugin.State == CruiseAssistState.ToStar ? Color.cyan : Color.white;
            var color2 = CruiseAssistPlugin.State == CruiseAssistState.ToPlanet ? Color.cyan : Color.white;
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            var guistyle = new GUIStyle(GUI.skin.label)
            {
                fixedWidth = 50f,
                fixedHeight = 36f,
                fontSize = 12,
                alignment = TextAnchor.UpperLeft
            };
            var guistyle2 = new GUIStyle(guistyle);
            guistyle.normal.textColor = color;
            GUILayout.Label("Target\n System:", guistyle, Array.Empty<GUILayoutOption>());
            guistyle2.normal.textColor = color2;
            GUILayout.Label("Target\n Planet:", guistyle2, Array.Empty<GUILayoutOption>());
            GUILayout.EndVertical();
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            var guistyle3 = new GUIStyle(GUI.skin.label)
            {
                fixedWidth = 240f,
                fixedHeight = 36f,
                fontSize = 14,
                alignment = TextAnchor.MiddleLeft
            };
            var guistyle4 = new GUIStyle(guistyle3);
            if (CruiseAssistPlugin.TargetStar != null && ((GameMain.localStar != null && CruiseAssistPlugin.TargetStar.id != GameMain.localStar.id) || CruiseAssistPlugin.TargetPlanet == null))
            {
                guistyle3.normal.textColor = color;
                GUILayout.Label(CruiseAssistPlugin.GetStarName(CruiseAssistPlugin.TargetStar), guistyle3, Array.Empty<GUILayoutOption>());
            }
            else
            {
                GUILayout.Label(" ", guistyle3, Array.Empty<GUILayoutOption>());
            }

            if (CruiseAssistPlugin.TargetPlanet != null)
            {
                guistyle4.normal.textColor = color2;
                GUILayout.Label(CruiseAssistPlugin.GetPlanetName(CruiseAssistPlugin.TargetPlanet), guistyle4, Array.Empty<GUILayoutOption>());
            }
            else
            {
                GUILayout.Label(" ", guistyle4, Array.Empty<GUILayoutOption>());
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            var actionSail = GameMain.mainPlayer.controller.actionSail;
            var visualUvel = actionSail.visual_uvel;
            var warping = GameMain.mainPlayer.warping;
            var magnitude = warping ? (visualUvel + actionSail.currentWarpVelocity).magnitude : visualUvel.magnitude;
            var guistyle5 = new GUIStyle(GUI.skin.label)
            {
                fixedWidth = 80f,
                fixedHeight = 36f,
                fontSize = 12,
                alignment = TextAnchor.MiddleRight
            };
            var guistyle6 = new GUIStyle(guistyle5);
            if (CruiseAssistPlugin.TargetStar != null && ((GameMain.localStar != null && CruiseAssistPlugin.TargetStar.id != GameMain.localStar.id) || CruiseAssistPlugin.TargetPlanet == null))
            {
                guistyle5.normal.textColor = color;
                var text = GameMain.mainPlayer.sailing ? TimeToString(CruiseAssistPlugin.TargetRange / magnitude) : "-- -- --";
                GUILayout.Label(RangeToString(CruiseAssistPlugin.TargetRange) + "\n" + text, guistyle5, Array.Empty<GUILayoutOption>());
            }
            else
            {
                GUILayout.Label(" \n ", guistyle5, Array.Empty<GUILayoutOption>());
            }

            if (CruiseAssistPlugin.TargetPlanet != null)
            {
                guistyle6.normal.textColor = color2;
                var text2 = GameMain.mainPlayer.sailing ? TimeToString(CruiseAssistPlugin.TargetRange / magnitude) : "-- -- --";
                GUILayout.Label(RangeToString(CruiseAssistPlugin.TargetRange) + "\n" + text2, guistyle6, Array.Empty<GUILayoutOption>());
            }
            else
            {
                GUILayout.Label(" \n ", guistyle6, Array.Empty<GUILayoutOption>());
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        var guistyle7 = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 160f,
            fixedHeight = 32f,
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft
        };
        if (!CruiseAssistPlugin.Enable)
        {
            GUILayout.Label("Cruise Assist Disabled.", guistyle7, Array.Empty<GUILayoutOption>());
        }
        else
        {
            if (CruiseAssistPlugin.State == CruiseAssistState.Inactive || CruiseAssistPlugin.Interrupt)
            {
                GUILayout.Label("Cruise Assist Inactive.", guistyle7, Array.Empty<GUILayoutOption>());
            }
            else
            {
                guistyle7.normal.textColor = Color.cyan;
                GUILayout.Label("Cruise Assist Active.", guistyle7, Array.Empty<GUILayoutOption>());
            }
        }
        GUILayout.FlexibleSpace();
        var guistyle8 = new GUIStyle(BaseButtonStyle)
        {
            fixedWidth = 50f,
            fixedHeight = 18f,
            fontSize = 11,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        if (GUILayout.Button("Config", guistyle8, Array.Empty<GUILayoutOption>()))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistConfigUI.Show[WIdx] = !CruiseAssistConfigUI.Show[WIdx];
            if (CruiseAssistConfigUI.Show[WIdx])
            {
                CruiseAssistConfigUI.TempScale = Scale;
            }
        }

        if (GUILayout.Button(CruiseAssistPlugin.Enable ? "Enable" : "Disable", guistyle8, Array.Empty<GUILayoutOption>()))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistPlugin.Enable = !CruiseAssistPlugin.Enable;
            NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        if (GUILayout.Button("StarList", guistyle8, Array.Empty<GUILayoutOption>()))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistStarListUI.Show[WIdx] = !CruiseAssistStarListUI.Show[WIdx];
        }

        if (GUILayout.Button("Cancel", guistyle8, Array.Empty<GUILayoutOption>()))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistStarListUI.SelectStar(null, null);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    public static string RangeToString(double range)
    {
        return range switch
        {
            < 10000.0 => (int)(range + 0.5) + "m ",
            < 600000.0 => (range / 40000.0).ToString("0.00") + "AU",
            _ => (range / 2400000.0).ToString("0.00") + "Ly"
        };
    }

    private static string TimeToString(double time)
    {
        var sec = (int)(time + 0.5);
        var min = sec / 60;
        var hour = min / 60;
        sec %= 60;
        min %= 60;
        return $"{hour:00} {min:00} {sec:00}";
    }

    public static float Scale = 150f;

    public static int WIdx = 0;

    public static CruiseAssistMainUIViewMode ViewMode = CruiseAssistMainUIViewMode.Full;

    public const float WindowWidthFull = 398f;

    public const float WindowHeightFull = 150f;

    public const float WindowWidthMini = 288f;

    public const float WindowHeightMini = 70f;

    public static readonly Rect[] Rect = {
        new Rect(0f, 0f, 398f, 150f),
        new Rect(0f, 0f, 398f, 150f)
    };

    private static readonly float[] LastCheckWindowLeft = { float.MinValue, float.MinValue };

    private static readonly float[] LastCheckWindowTop = { float.MinValue, float.MinValue };

    public static long NextCheckGameTick = long.MaxValue;

    public static Texture2D WhiteBorderBackgroundTexture;

    public static Texture2D GrayBorderBackgroundTexture;

    public static Texture2D WhiteBorderTexture;

    public static Texture2D GrayBorderTexture;

    public static Texture2D BlackTexture;

    public static Texture2D WhiteTexture;

    public static Texture2D ToggleOnTexture;

    public static Texture2D ToggleOffTexture;

    public static Texture2D CloseButtonGrayBorderTexture;

    public static Texture2D CloseButtonWhiteBorderTexture;

    public static GUIStyle WindowStyle;

    public static GUIStyle BaseButtonStyle;

    public static GUIStyle BaseToolbarButtonStyle;

    public static GUIStyle BaseVerticalScrollBarStyle;

    public static GUIStyle BaseHorizontalSliderStyle;

    public static GUIStyle BaseHorizontalSliderThumbStyle;

    public static GUIStyle BaseToggleStyle;

    public static GUIStyle BaseTextFieldStyle;

    public static GUIStyle CloseButtonStyle;

    private static List<GUIStyle> _verticalScrollBarSkins;
}