using System.Collections.Generic;
using System.Linq;
using CruiseAssist.Commons;
using CruiseAssist.Enums;
using UnityEngine;

namespace CruiseAssist.UI;

public static class CruiseAssistMainUI
{
    public static void OnInit()
    {
        if (_whiteBorderBackgroundTexture == null)
        {
            _whiteBorderBackgroundTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color2 = new Color32(0, 0, 0, 224);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    var color3 = i <= 0 || j <= 0 || i >= 63 || j >= 63 ? color : color2;
                    _whiteBorderBackgroundTexture.SetPixel(j, i, color3);
                }
            }

            _whiteBorderBackgroundTexture.Apply();
        }

        if (_grayBorderBackgroundTexture == null)
        {
            _grayBorderBackgroundTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color4 = new Color32(64, 64, 64, byte.MaxValue);
            var color5 = new Color32(0, 0, 0, 224);
            for (var k = 0; k < 64; k++)
            {
                for (var l = 0; l < 64; l++)
                {
                    var color6 = k <= 0 || l <= 0 || k >= 63 || l >= 63 ? color4 : color5;
                    _grayBorderBackgroundTexture.SetPixel(l, k, color6);
                }
            }

            _grayBorderBackgroundTexture.Apply();
        }

        if (_whiteBorderTexture == null)
        {
            _whiteBorderTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color7 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color8 = new Color32(0, 0, 0, byte.MaxValue);
            for (var m = 0; m < 64; m++)
            {
                for (var n = 0; n < 64; n++)
                {
                    var color9 = m <= 0 || n <= 0 || m >= 63 || n >= 63 ? color7 : color8;
                    _whiteBorderTexture.SetPixel(n, m, color9);
                }
            }

            _whiteBorderTexture.Apply();
        }

        if (_grayBorderTexture == null)
        {
            _grayBorderTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color10 = new Color32(64, 64, 64, byte.MaxValue);
            var color11 = new Color32(0, 0, 0, byte.MaxValue);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    var color12 = i <= 0 || j <= 0 || i >= 63 || j >= 63 ? color10 : color11;
                    _grayBorderTexture.SetPixel(j, i, color12);
                }
            }

            _grayBorderTexture.Apply();
        }

        if (_blackTexture == null)
        {
            _blackTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color13 = new Color32(0, 0, 0, byte.MaxValue);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    _blackTexture.SetPixel(j, i, color13);
                }
            }

            _blackTexture.Apply();
        }

        if (_whiteTexture == null)
        {
            _whiteTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            var color14 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    _whiteTexture.SetPixel(j, i, color14);
                }
            }

            _whiteTexture.Apply();
        }

        if (_toggleOnTexture == null)
        {
            _toggleOnTexture = new Texture2D(16, 16, TextureFormat.RGBA32, false);
            var color15 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color16 = new Color32(0, 0, 0, 0);
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var color17 = j is >= 1 and <= 12 && i is >= 2 and <= 13 ? color15 : color16;
                    _toggleOnTexture.SetPixel(j, i, color17);
                }
            }

            _toggleOnTexture.Apply();
        }

        if (_toggleOffTexture == null)
        {
            _toggleOffTexture = new Texture2D(16, 16, TextureFormat.RGBA32, false);
            var color18 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color19 = new Color32(0, 0, 0, byte.MaxValue);
            var color20 = new Color32(0, 0, 0, 0);
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var color21 = j < 1 || j > 12 || i < 2 || i > 13 ? color20 : j is > 1 and < 12 && i is > 2 and < 13 ? color19 : color18;
                    _toggleOffTexture.SetPixel(j, i, color21);
                }
            }

            _toggleOffTexture.Apply();
        }

        if (_closeButtonGrayBorderTexture != null)
        {
            _closeButtonGrayBorderTexture = new Texture2D(16, 16, TextureFormat.RGBA32, false);
            var color22 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color23 = new Color32(64, 64, 64, byte.MaxValue);
            var color24 = new Color32(0, 0, 0, byte.MaxValue);
            var color25 = new Color32(0, 0, 0, 0);
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var color26 = j < 1 || j > 12 || i < 2 || i > 13 ? color25 : j is > 1 and < 12 && i is > 2 and < 13 ? color24 : color23;
                    _closeButtonGrayBorderTexture.SetPixel(j, i, color26);
                }
            }

            for (var i = 4; i <= 9; i++)
            {
                _closeButtonGrayBorderTexture.SetPixel(i, i + 1, color22);
                _closeButtonGrayBorderTexture.SetPixel(i, 14 - i, color22);
            }

            _closeButtonGrayBorderTexture.Apply();
        }

        if (_closeButtonWhiteBorderTexture == null)
        {
            _closeButtonWhiteBorderTexture = new Texture2D(16, 16, TextureFormat.RGBA32, false);
            var color27 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            var color28 = new Color32(0, 0, 0, byte.MaxValue);
            var color29 = new Color32(0, 0, 0, 0);
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var color30 = j < 1 || j > 12 || i < 2 || i > 13 ? color29 : j is > 1 and < 12 && i is > 2 and < 13 ? color28 : color27;
                    _closeButtonWhiteBorderTexture.SetPixel(j, i, color30);
                }
            }

            for (var i = 4; i <= 9; i++)
            {
                _closeButtonWhiteBorderTexture.SetPixel(i, i + 1, color27);
                _closeButtonWhiteBorderTexture.SetPixel(i, 14 - i, color27);
            }

            _closeButtonWhiteBorderTexture.Apply();
        }

        WindowStyle ??= new GUIStyle(GUI.skin.window)
        {
            fontSize = 11,
            normal =
            {
                textColor = Color.white,
                background = _grayBorderBackgroundTexture
            },
            hover =
            {
                textColor = Color.white,
                background = _grayBorderBackgroundTexture
            },
            active =
            {
                textColor = Color.white,
                background = _grayBorderBackgroundTexture
            },
            focused =
            {
                textColor = Color.white,
                background = _grayBorderBackgroundTexture
            },
            onNormal =
            {
                textColor = Color.white,
                background = _whiteBorderBackgroundTexture
            },
            onHover =
            {
                textColor = Color.white,
                background = _whiteBorderBackgroundTexture
            },
            onActive =
            {
                textColor = Color.white,
                background = _whiteBorderBackgroundTexture
            },
            onFocused =
            {
                textColor = Color.white,
                background = _whiteBorderBackgroundTexture
            }
        };

        BaseButtonStyle ??= new GUIStyle(GUI.skin.button)
        {
            normal =
            {
                textColor = Color.white,
                background = _grayBorderTexture
            },
            hover =
            {
                textColor = Color.white,
                background = _whiteBorderTexture
            },
            active =
            {
                textColor = Color.white,
                background = _whiteBorderTexture
            },
            focused =
            {
                textColor = Color.white,
                background = _whiteBorderTexture
            },
            onNormal =
            {
                textColor = Color.white,
                background = _grayBorderTexture
            },
            onHover =
            {
                textColor = Color.white,
                background = _whiteBorderTexture
            },
            onActive =
            {
                textColor = Color.white,
                background = _whiteBorderTexture
            },
            onFocused =
            {
                textColor = Color.white,
                background = _whiteBorderTexture
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
                background = _whiteBorderBackgroundTexture
            }
        };

        BaseVerticalScrollBarStyle ??= new GUIStyle(GUI.skin.verticalScrollbar)
        {
            name = "cruiseassist.verticalscrollbar",
            normal =
            {
                background = _grayBorderTexture
            },
            hover =
            {
                background = _grayBorderTexture
            },
            active =
            {
                background = _grayBorderTexture
            },
            focused =
            {
                background = _grayBorderTexture
            },
            onNormal =
            {
                background = _grayBorderTexture
            },
            onHover =
            {
                background = _grayBorderTexture
            },
            onActive =
            {
                background = _grayBorderTexture
            },
            onFocused =
            {
                background = _grayBorderTexture
            }
        };

        BaseHorizontalSliderStyle ??= new GUIStyle(GUI.skin.horizontalSlider)
        {
            normal =
            {
                background = _grayBorderTexture
            },
            hover =
            {
                background = _grayBorderTexture
            },
            active =
            {
                background = _grayBorderTexture
            },
            focused =
            {
                background = _grayBorderTexture
            },
            onNormal =
            {
                background = _grayBorderTexture
            },
            onHover =
            {
                background = _grayBorderTexture
            },
            onActive =
            {
                background = _grayBorderTexture
            },
            onFocused =
            {
                background = _grayBorderTexture
            }
        };

        BaseHorizontalSliderThumbStyle ??= new GUIStyle(GUI.skin.horizontalSliderThumb)
        {
            normal =
            {
                background = _whiteBorderTexture
            },
            hover =
            {
                background = _whiteBorderTexture
            },
            active =
            {
                background = _whiteBorderTexture
            },
            focused =
            {
                background = _whiteBorderTexture
            },
            onNormal =
            {
                background = _whiteBorderTexture
            },
            onHover =
            {
                background = _whiteBorderTexture
            },
            onActive =
            {
                background = _whiteBorderTexture
            },
            onFocused =
            {
                background = _whiteBorderTexture
            }
        };

        BaseToggleStyle ??= new GUIStyle(GUI.skin.toggle)
        {
            normal =
            {
                background = _toggleOffTexture
            },
            hover =
            {
                background = _toggleOffTexture
            },
            active =
            {
                background = _toggleOffTexture
            },
            focused =
            {
                background = _toggleOffTexture
            },
            onNormal =
            {
                background = _toggleOnTexture
            },
            onHover =
            {
                background = _toggleOnTexture
            },
            onActive =
            {
                background = _toggleOnTexture
            },
            onFocused =
            {
                background = _toggleOnTexture
            }
        };

        BaseTextFieldStyle ??= new GUIStyle(GUI.skin.textField)
        {
            normal =
            {
                background = _whiteBorderTexture
            },
            hover =
            {
                background = _whiteBorderTexture
            },
            active =
            {
                background = _whiteBorderTexture
            },
            focused =
            {
                background = _whiteBorderTexture
            },
            onNormal =
            {
                background = _whiteBorderTexture
            },
            onHover =
            {
                background = _whiteBorderTexture
            },
            onActive =
            {
                background = _whiteBorderTexture
            },
            onFocused =
            {
                background = _whiteBorderTexture
            }
        };

        CloseButtonStyle ??= new GUIStyle(GUI.skin.button)
        {
            normal =
            {
                background = _closeButtonGrayBorderTexture
            },
            hover =
            {
                background = _closeButtonWhiteBorderTexture
            },
            active =
            {
                background = _closeButtonWhiteBorderTexture
            },
            focused =
            {
                background = _closeButtonWhiteBorderTexture
            },
            onNormal =
            {
                background = _closeButtonGrayBorderTexture
            },
            onHover =
            {
                background = _closeButtonWhiteBorderTexture
            },
            onActive =
            {
                background = _closeButtonWhiteBorderTexture
            },
            onFocused =
            {
                background = _closeButtonWhiteBorderTexture
            }
        };
        if (_verticalScrollBarSkins == null)
        {
            _verticalScrollBarSkins = [];
            var guistyle = new GUIStyle(GUI.skin.verticalScrollbarThumb)
            {
                name = "cruiseassist.verticalscrollbarthumb",
                normal =
                {
                    background = _whiteBorderTexture
                },
                hover =
                {
                    background = _whiteBorderTexture
                },
                active =
                {
                    background = _whiteBorderTexture
                },
                focused =
                {
                    background = _whiteBorderTexture
                },
                onNormal =
                {
                    background = _whiteBorderTexture
                },
                onHover =
                {
                    background = _whiteBorderTexture
                },
                onActive =
                {
                    background = _whiteBorderTexture
                },
                onFocused =
                {
                    background = _whiteBorderTexture
                }
            };
            _verticalScrollBarSkins.Add(guistyle);
            var guistyle2 = new GUIStyle(GUI.skin.verticalScrollbarUpButton)
            {
                name = "cruiseassist.verticalscrollbarupbutton",
                normal =
                {
                    background = _blackTexture
                },
                hover =
                {
                    background = _blackTexture
                },
                active =
                {
                    background = _blackTexture
                },
                focused =
                {
                    background = _blackTexture
                },
                onNormal =
                {
                    background = _blackTexture
                },
                onHover =
                {
                    background = _blackTexture
                },
                onActive =
                {
                    background = _blackTexture
                },
                onFocused =
                {
                    background = _blackTexture
                }
            };
            _verticalScrollBarSkins.Add(guistyle2);
            var guistyle3 = new GUIStyle(GUI.skin.verticalScrollbarDownButton)
            {
                name = "cruiseassist.verticalscrollbardownbutton",
                normal =
                {
                    background = _blackTexture
                },
                hover =
                {
                    background = _blackTexture
                },
                active =
                {
                    background = _blackTexture
                },
                focused =
                {
                    background = _blackTexture
                },
                onNormal =
                {
                    background = _blackTexture
                },
                onHover =
                {
                    background = _blackTexture
                },
                onActive =
                {
                    background = _blackTexture
                },
                onFocused =
                {
                    background = _blackTexture
                }
            };
            _verticalScrollBarSkins.Add(guistyle3);
            GUI.skin.customStyles = GUI.skin.customStyles.Concat(_verticalScrollBarSkins).ToArray();
        }
        _starLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 50f,
            fixedHeight = 36f,
            fontSize = 12,
            alignment = TextAnchor.UpperLeft
        };
        _planetLabelStyle = new GUIStyle(_starLabelStyle);
        _starLabelStyle2 = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 240f,
            fixedHeight = 36f,
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft
        };
        _planetLabelStyle2 = new GUIStyle(_starLabelStyle2);
        _starLabelStyle3 = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 80f,
            fixedHeight = 36f,
            fontSize = 12,
            alignment = TextAnchor.MiddleRight
        };
        _planetLabelStyle3 = new GUIStyle(_starLabelStyle3);
        _stateLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 160f,
            fixedHeight = 32f,
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft
        };
        _configButtonStyle = new GUIStyle(BaseButtonStyle)
        {
            fixedWidth = 55f,
            fixedHeight = 18f,
            fontSize = 11,
            alignment = TextAnchor.MiddleCenter
        };
    }

    public static void OnGUI()
    {
        switch (ViewMode)
        {
            case CruiseAssistMainUIViewMode.Full:
                Rect[WIdx].width = 408f;
                Rect[WIdx].height = 150f;
                break;
            case CruiseAssistMainUIViewMode.Mini:
                Rect[WIdx].width = 298f;
                Rect[WIdx].height = 70f;
                break;
        }
        Rect[WIdx] = GUILayout.Window(99030291, Rect[WIdx], WindowFunction, "CruiseAssist", WindowStyle);
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
                LastCheckWindowLeft[WIdx] = Rect[WIdx].x;
                LastCheckWindowTop[WIdx] = Rect[WIdx].y;
                NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        else
        {
            LastCheckWindowLeft[WIdx] = Rect[WIdx].x;
            LastCheckWindowTop[WIdx] = Rect[WIdx].y;
        }

        if (NextCheckGameTick <= GameMain.gameTick)
        {
            ConfigManager.CheckConfig(ConfigManager.Step.State);
        }
    }

    private static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical();
        if (ViewMode == CruiseAssistMainUIViewMode.Full)
        {
            GUILayout.BeginHorizontal();
            var starColor = CruiseAssistPlugin.State == CruiseAssistState.ToStar ? Color.cyan : Color.white;
            var planetColor = CruiseAssistPlugin.State == CruiseAssistState.ToPlanet ? Color.cyan : Color.white;
            GUILayout.BeginVertical();
            _starLabelStyle.normal.textColor = starColor;
            GUILayout.Label(Strings.Get(0), _starLabelStyle);
            _planetLabelStyle.normal.textColor = planetColor;
            GUILayout.Label(Strings.Get(1), _planetLabelStyle);
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (CruiseAssistPlugin.TargetStar != null && ((GameMain.localStar != null && CruiseAssistPlugin.TargetStar.id != GameMain.localStar.id) || CruiseAssistPlugin.TargetPlanet == null))
            {
                _starLabelStyle2.normal.textColor = starColor;
                GUILayout.Label(CruiseAssistPlugin.GetStarName(CruiseAssistPlugin.TargetStar), _starLabelStyle2);
            }
            else
            {
                GUILayout.Label(" ", _starLabelStyle2);
            }

            if (CruiseAssistPlugin.TargetPlanet != null)
            {
                _planetLabelStyle2.normal.textColor = planetColor;
                GUILayout.Label(CruiseAssistPlugin.GetPlanetName(CruiseAssistPlugin.TargetPlanet), _planetLabelStyle2);
            }
            else
            {
                GUILayout.Label(" ", _planetLabelStyle2);
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            var actionSail = GameMain.mainPlayer.controller.actionSail;
            var visualUvel = actionSail.visual_uvel;
            var warping = GameMain.mainPlayer.warping;
            var magnitude = warping ? (visualUvel + actionSail.currentWarpVelocity).magnitude : visualUvel.magnitude;
            if (CruiseAssistPlugin.TargetStar != null && ((GameMain.localStar != null && CruiseAssistPlugin.TargetStar.id != GameMain.localStar.id) || CruiseAssistPlugin.TargetPlanet == null))
            {
                _starLabelStyle3.normal.textColor = starColor;
                var text = GameMain.mainPlayer.sailing ? TimeToString(CruiseAssistPlugin.TargetRange / magnitude) : "-- -- --";
                GUILayout.Label(RangeToString(CruiseAssistPlugin.TargetRange) + "\n" + text, _starLabelStyle3);
            }
            else
            {
                GUILayout.Label(" \n ", _starLabelStyle3);
            }

            if (CruiseAssistPlugin.TargetPlanet != null)
            {
                _planetLabelStyle3.normal.textColor = planetColor;
                var text2 = GameMain.mainPlayer.sailing ? TimeToString(CruiseAssistPlugin.TargetRange / magnitude) : "-- -- --";
                GUILayout.Label(RangeToString(CruiseAssistPlugin.TargetRange) + "\n" + text2, _planetLabelStyle3);
            }
            else
            {
                GUILayout.Label(" \n ", _planetLabelStyle3);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
        GUILayout.BeginHorizontal();
        if (!CruiseAssistPlugin.Enable)
        {
            GUILayout.Label(Strings.Get(2), _stateLabelStyle);
        }
        else
        {
            if (CruiseAssistPlugin.State == CruiseAssistState.Inactive || CruiseAssistPlugin.Interrupt)
            {
                GUILayout.Label(Strings.Get(3), _stateLabelStyle);
            }
            else
            {
                _stateLabelStyle.normal.textColor = Color.cyan;
                GUILayout.Label(Strings.Get(4), _stateLabelStyle);
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        if (GUILayout.Button(Strings.Get(5), _configButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistConfigUI.Show[WIdx] = !CruiseAssistConfigUI.Show[WIdx];
            if (CruiseAssistConfigUI.Show[WIdx])
            {
                CruiseAssistConfigUI.TempScale = Scale;
            }
        }

        if (GUILayout.Button(CruiseAssistPlugin.Enable ? Strings.Get(6) : Strings.Get(7), _configButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistPlugin.Enable = !CruiseAssistPlugin.Enable;
            if (!CruiseAssistPlugin.Enable)
            {
                CruiseAssistPlugin.Extensions.ForEach(extension => extension.SetInactive());
            }
            NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        if (GUILayout.Button(Strings.Get(8), _configButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistStarListUI.Show[WIdx] = !CruiseAssistStarListUI.Show[WIdx];
        }

        if (GUILayout.Button(Strings.Get(9), _configButtonStyle))
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
            < 10000.0 => $"{(int)(range + 0.5)}m",
            < 600000.0 => $"{range / 40000.0:0.00}AU",
            _ => $"{range / 2400000.0:0.00}Ly"
        };
    }

    private static string TimeToString(double time)
    {
        var sec = (int)(time + 0.5);
        var min = sec / 60;
        var hour = min / 60;
        return $"{hour:00} {min % 60:00} {sec % 60:00}";
    }

    public static float Scale = 150f;
    public static int WIdx = 0;
    public static CruiseAssistMainUIViewMode ViewMode = CruiseAssistMainUIViewMode.Full;
    public const float WindowWidthFull = 398f;
    public const float WindowHeightFull = 150f;
    public const float WindowWidthMini = 288f;
    public const float WindowHeightMini = 70f;
    public static readonly Rect[] Rect =
    [
        new Rect(0f, 0f, 398f, 150f),
        new Rect(0f, 0f, 398f, 150f)
    ];
    private static readonly float[] LastCheckWindowLeft = [float.MinValue, float.MinValue];
    private static readonly float[] LastCheckWindowTop = [float.MinValue, float.MinValue];
    public static long NextCheckGameTick = long.MaxValue;
    private static Texture2D _whiteBorderBackgroundTexture;
    private static Texture2D _grayBorderBackgroundTexture;
    private static Texture2D _whiteBorderTexture;
    private static Texture2D _grayBorderTexture;
    private static Texture2D _blackTexture;
    private static Texture2D _whiteTexture;
    private static Texture2D _toggleOnTexture;
    private static Texture2D _toggleOffTexture;
    private static Texture2D _closeButtonGrayBorderTexture;
    private static Texture2D _closeButtonWhiteBorderTexture;
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
    private static GUIStyle _starLabelStyle;
    private static GUIStyle _planetLabelStyle;
    private static GUIStyle _starLabelStyle2;
    private static GUIStyle _planetLabelStyle2;
    private static GUIStyle _starLabelStyle3;
    private static GUIStyle _planetLabelStyle3;
    private static GUIStyle _stateLabelStyle;
    private static GUIStyle _configButtonStyle;
}
