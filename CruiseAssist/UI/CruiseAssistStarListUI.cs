using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace CruiseAssist.UI;

public class CruiseAssistStarListUI
{
    public static void OnGUI()
    {
        wIdx = CruiseAssistMainUI.WIdx;
        Rect[wIdx] = GUILayout.Window(99030292, Rect[wIdx], WindowFunction, "CruiseAssist - StarList", CruiseAssistMainUI.WindowStyle, Array.Empty<GUILayoutOption>());
        var scale = CruiseAssistMainUI.Scale / 100f;
        if (Screen.width / scale < Rect[wIdx].xMax)
        {
            Rect[wIdx].x = Screen.width / scale - Rect[wIdx].width;
        }

        if (Rect[wIdx].x < 0f)
        {
            Rect[wIdx].x = 0f;
        }

        if (Screen.height / scale < Rect[wIdx].yMax)
        {
            Rect[wIdx].y = Screen.height / scale - Rect[wIdx].height;
        }

        if (Rect[wIdx].y < 0f)
        {
            Rect[wIdx].y = 0f;
        }

        if (lastCheckWindowLeft[wIdx] != float.MinValue)
        {
            if (Rect[wIdx].x != lastCheckWindowLeft[wIdx] || Rect[wIdx].y != lastCheckWindowTop[wIdx])
            {
                CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        lastCheckWindowLeft[wIdx] = Rect[wIdx].x;
        lastCheckWindowTop[wIdx] = Rect[wIdx].y;
    }

    public static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        var guistyle = new GUIStyle(CruiseAssistMainUI.BaseToolbarButtonStyle)
        {
            fixedWidth = 80f,
            fixedHeight = 20f,
            fontSize = 12
        };
        var array = new[] { "Normal", "History", "Bookmark" };
        GUI.changed = false;
        var selectedIndex = GUILayout.Toolbar(ListSelected, array, guistyle, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
        }

        if (selectedIndex != ListSelected)
        {
            ListSelected = selectedIndex;
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUILayout.EndHorizontal();
        scrollPos[ListSelected] = GUILayout.BeginScrollView(scrollPos[ListSelected], GUIStyle.none, new GUIStyle(CruiseAssistMainUI.BaseVerticalScrollBarStyle), Array.Empty<GUILayoutOption>());
        var nameLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 240f,
            stretchHeight = true,
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft
        };
        var nRangeLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 60f,
            fixedHeight = 20f,
            fontSize = 14,
            alignment = TextAnchor.MiddleRight
        };
        var hRangeLabelStyle = new GUIStyle(nRangeLabelStyle)
        {
            fixedHeight = 40f
        };
        var nActionButtonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 40f,
            fixedHeight = 18f,
            margin =
            {
                top = 6
            },
            fontSize = 12
        };
        var hActionButtonStyle = new GUIStyle(nActionButtonStyle)
        {
            margin =
            {
                top = 16
            }
        };
        var nSortButtonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 20f,
            fixedHeight = 18f,
            margin =
            {
                top = 6
            },
            fontSize = 12
        };
        var hSortButtonStyle = new GUIStyle(nSortButtonStyle)
        {
            margin =
            {
                top = 16
            }
        };
        switch (ListSelected)
        {
            case 0:
                GameMain.galaxy.stars.Select(star => new Commons.Tuple<StarData, double>(star, (star.uPosition - GameMain.mainPlayer.uPosition).magnitude)).OrderBy(tuple => tuple.Item2).Do(delegate(Commons.Tuple<StarData, double> tuple)
                {
                    var star = tuple.Item1;
                    var range = tuple.Item2;
                    var starName = CruiseAssistPlugin.GetStarName(star);
                    var ok = false;
                    if (GameMain.localStar != null && star.id == GameMain.localStar.id)
                    {
                        ok = true;
                    }
                    else
                    {
                        if (CruiseAssistPlugin.SelectTargetStar != null && star.id == CruiseAssistPlugin.SelectTargetStar.id && GameMain.history.universeObserveLevel >= (range >= 14400000.0 ? 4 : 3))
                        {
                            ok = true;
                        }
                    }

                    if (ok)
                    {
                        star.planets.Select(planet => new Commons.Tuple<PlanetData, double>(planet, (planet.uPosition - GameMain.mainPlayer.uPosition).magnitude)).AddItem(new Commons.Tuple<PlanetData, double>(null, (star.uPosition - GameMain.mainPlayer.uPosition).magnitude)).OrderBy(tuple2 => tuple2.Item2).Do(delegate(Commons.Tuple<PlanetData, double> tuple2)
                        {
                            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                            var planetData = tuple2.Item1;
                            var distance = tuple2.Item2;
                            nameLabelStyle.normal.textColor = Color.white;
                            nRangeLabelStyle.normal.textColor = Color.white;
                            float height;
                            if (planetData == null)
                            {
                                if (CruiseAssistPlugin.SelectTargetPlanet == null && CruiseAssistPlugin.SelectTargetStar != null && star.id == CruiseAssistPlugin.SelectTargetStar.id)
                                {
                                    nameLabelStyle.normal.textColor = Color.cyan;
                                    nRangeLabelStyle.normal.textColor = Color.cyan;
                                }
                                var name = starName;
                                if (CruiseAssistPlugin.Conf.MarkVisitedFlag)
                                {
                                    name = (star.planets.Where(p => p.factory != null).Count() > 0 ? "● " : "") + name;
                                }
                                GUILayout.Label(name, nameLabelStyle, Array.Empty<GUILayoutOption>());
                                height = nameLabelStyle.CalcHeight(new GUIContent(name), nameLabelStyle.fixedWidth);
                            }
                            else
                            {
                                if (CruiseAssistPlugin.SelectTargetPlanet != null && planetData.id == CruiseAssistPlugin.SelectTargetPlanet.id)
                                {
                                    nameLabelStyle.normal.textColor = Color.cyan;
                                    nRangeLabelStyle.normal.textColor = Color.cyan;
                                }
                                var name = starName + " - " + CruiseAssistPlugin.GetPlanetName(planetData);
                                if (CruiseAssistPlugin.Conf.MarkVisitedFlag)
                                {
                                    name = (planetData.factory != null ? "● " : "") + name;
                                }
                                GUILayout.Label(name, nameLabelStyle, Array.Empty<GUILayoutOption>());
                                height = nameLabelStyle.CalcHeight(new GUIContent(name), nameLabelStyle.fixedWidth);
                            }
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(CruiseAssistMainUI.RangeToString(planetData == null ? range : distance), height < 30f ? nRangeLabelStyle : hRangeLabelStyle, Array.Empty<GUILayoutOption>());
                            if (GUILayout.Button(actionSelected[ListSelected] == 0 ? "SET" : planetData == null ? "-" : CruiseAssistPlugin.Bookmark.Contains(planetData.id) ? "DEL" : "ADD", height < 30f ? nActionButtonStyle : hActionButtonStyle, Array.Empty<GUILayoutOption>()))
                            {
                                VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                                if (actionSelected[ListSelected] == 0)
                                {
                                    SelectStar(star, planetData);
                                    var closeStarListWhenSetTargetPlanetFlag = CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag;
                                    if (closeStarListWhenSetTargetPlanetFlag)
                                    {
                                        Show[wIdx] = false;
                                    }
                                }
                                else
                                {
                                    if (planetData != null)
                                    {
                                        if (CruiseAssistPlugin.Bookmark.Contains(planetData.id))
                                        {
                                            CruiseAssistPlugin.Bookmark.Remove(planetData.id);
                                        }
                                        else
                                        {
                                            if (CruiseAssistPlugin.Bookmark.Count <= 128)
                                            {
                                                CruiseAssistPlugin.Bookmark.Add(planetData.id);
                                                CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
                                            }
                                        }
                                    }
                                }
                            }
                            GUILayout.EndHorizontal();
                        });
                    }
                    else
                    {
                        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                        nameLabelStyle.normal.textColor = Color.white;
                        nRangeLabelStyle.normal.textColor = Color.white;
                        if (CruiseAssistPlugin.SelectTargetStar != null && star.id == CruiseAssistPlugin.SelectTargetStar.id)
                        {
                            nameLabelStyle.normal.textColor = Color.cyan;
                            nRangeLabelStyle.normal.textColor = Color.cyan;
                        }
                        var name = starName;
                        if (CruiseAssistPlugin.Conf.MarkVisitedFlag)
                        {
                            name = (star.planets.Count(p => p.factory != null) > 0 ? "● " : "") + name;
                        }
                        GUILayout.Label(name, nameLabelStyle, Array.Empty<GUILayoutOption>());
                        var height = nameLabelStyle.CalcHeight(new GUIContent(name), nameLabelStyle.fixedWidth);
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(CruiseAssistMainUI.RangeToString(range), height < 30f ? nRangeLabelStyle : hRangeLabelStyle, Array.Empty<GUILayoutOption>());
                        if (GUILayout.Button(actionSelected[ListSelected] == 0 ? "SET" : "-", height < 30f ? nActionButtonStyle : hActionButtonStyle, Array.Empty<GUILayoutOption>()))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            if (actionSelected[ListSelected] == 0)
                            {
                                SelectStar(star, null);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                });
                break;
            case 1 or 2:
            {
                var highlighted = false;
                var enumBookmark = ListSelected != 1 ? CruiseAssistPlugin.Bookmark.ToList() : Enumerable.Reverse(CruiseAssistPlugin.History);
                if (ListSelected == 1 && actionSelected[ListSelected] != 2 && CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag)
                {
                    enumBookmark = enumBookmark.Distinct();
                }
                var listIndex = -1;
                enumBookmark.Do(delegate(int id)
                {
                    listIndex += 1;
                    var planetData = GameMain.galaxy.PlanetById(id);
                    if (planetData == null) return;
                    var magnitude = (planetData.uPosition - GameMain.mainPlayer.uPosition).magnitude;
                    nameLabelStyle.normal.textColor = Color.white;
                    nRangeLabelStyle.normal.textColor = Color.white;
                    if (!highlighted && CruiseAssistPlugin.SelectTargetPlanet != null && planetData.id == CruiseAssistPlugin.SelectTargetPlanet.id)
                    {
                        nameLabelStyle.normal.textColor = Color.cyan;
                        nRangeLabelStyle.normal.textColor = Color.cyan;
                        highlighted = true;
                    }
                    GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                    var name = CruiseAssistPlugin.GetStarName(planetData.star) + " - " + CruiseAssistPlugin.GetPlanetName(planetData);
                    if (CruiseAssistPlugin.Conf.MarkVisitedFlag)
                    {
                        name = (planetData.factory != null ? "● " : "") + name;
                    }
                    GUILayout.Label(name, nameLabelStyle, Array.Empty<GUILayoutOption>());
                    var height = nameLabelStyle.CalcHeight(new GUIContent(name), nameLabelStyle.fixedWidth);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(CruiseAssistMainUI.RangeToString(magnitude), height < 30f ? nRangeLabelStyle : hRangeLabelStyle, Array.Empty<GUILayoutOption>());
                    if (ListSelected == 2 && actionSelected[ListSelected] == 1)
                    {
                        var index = CruiseAssistPlugin.Bookmark.IndexOf(id);
                        var first = index == 0;
                        var last = index == CruiseAssistPlugin.Bookmark.Count - 1;
                        if (GUILayout.Button(last ? "-" : "↓", height < 30f ? nSortButtonStyle : hSortButtonStyle, Array.Empty<GUILayoutOption>()))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            if (!last)
                            {
                                CruiseAssistPlugin.Bookmark.RemoveAt(index);
                                CruiseAssistPlugin.Bookmark.Insert(index + 1, id);
                            }
                        }

                        if (GUILayout.Button(first ? "-" : "↑", height < 30f ? nSortButtonStyle : hSortButtonStyle, Array.Empty<GUILayoutOption>()))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            if (!first)
                            {
                                CruiseAssistPlugin.Bookmark.RemoveAt(index);
                                CruiseAssistPlugin.Bookmark.Insert(index - 1, id);
                            }
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(actionSelected[ListSelected] == 0 ? "SET" : actionSelected[ListSelected] == 2 ? ListSelected == 1 && listIndex == 0 ? "-" : "DEL" : CruiseAssistPlugin.Bookmark.Contains(id) ? "DEL" : "ADD", height < 30f ? nActionButtonStyle : hActionButtonStyle, Array.Empty<GUILayoutOption>()))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            switch (actionSelected[ListSelected])
                            {
                                case 0:
                                {
                                    SelectStar(planetData.star, planetData);
                                    if (CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag)
                                    {
                                        Show[wIdx] = false;
                                    }

                                    break;
                                }
                                case 1:
                                {
                                    if (ListSelected == 1)
                                    {
                                        if (CruiseAssistPlugin.Bookmark.Contains(id))
                                        {
                                            CruiseAssistPlugin.Bookmark.Remove(id);
                                        }
                                        else if (CruiseAssistPlugin.Bookmark.Count <= 128)
                                        {
                                            CruiseAssistPlugin.Bookmark.Add(id);
                                            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
                                        }
                                    }

                                    break;
                                }
                                default:
                                {
                                    if (actionSelected[ListSelected] == 2)
                                    {
                                        switch (ListSelected)
                                        {
                                            case 1:
                                            {
                                                if (listIndex != 0)
                                                {
                                                    CruiseAssistPlugin.History.RemoveAt(CruiseAssistPlugin.History.Count - 1 - listIndex);
                                                    CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
                                                }

                                                break;
                                            }
                                            case 2:
                                                CruiseAssistPlugin.Bookmark.Remove(planetData.id);
                                                CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
                                                break;
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                });
                break;
            }
        }
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        var buttonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 80f,
            fixedHeight = 20f,
            fontSize = 12
        };
        var buttons = new[]
        {
            new[] { "Target", "Bookmark" },
            new[] { "Target", "Bookmark", "Delete" },
            new[] { "Target", "Sort", "Delete" }
        };
        if (GUILayout.Button(buttons[ListSelected][actionSelected[ListSelected]], buttonStyle, Array.Empty<GUILayoutOption>()))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            actionSelected[ListSelected]++;
            actionSelected[ListSelected] %= buttons[ListSelected].Length;
        }
        GUILayout.FlexibleSpace();
        if (!CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag && GUILayout.Button("Close", buttonStyle, Array.Empty<GUILayoutOption>()))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            Show[wIdx] = false;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        if (GUI.Button(new Rect(Rect[wIdx].width - 16f, 1f, 16f, 16f), "", CruiseAssistMainUI.CloseButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            Show[wIdx] = false;
        }
        GUI.DragWindow();
    }

    public static void SelectStar(StarData star, PlanetData planet)
    {
        CruiseAssistPlugin.SelectTargetStar = star;
        CruiseAssistPlugin.SelectTargetPlanet = planet;
        var uiGame = UIRoot.instance.uiGame;
        if (CruiseAssistPlugin.Conf.SelectFocusFlag && uiGame.starmap.active)
        {
            if (star != null)
            {
                var uistarmapStar = uiGame.starmap.starUIs.Where(s => s.star.id == star.id).FirstOrDefault();
                if (uistarmapStar != null)
                {
                    UIStarmap_OnStarClick(uiGame.starmap, uistarmapStar);
                    uiGame.starmap.OnCursorFunction2Click(0);
                }
            }

            if (planet != null)
            {
                var uistarmapPlanet = uiGame.starmap.planetUIs.Where(p => p.planet.id == planet.id).FirstOrDefault();
                if (uistarmapPlanet != null)
                {
                    UIStarmap_OnPlanetClick(uiGame.starmap, uistarmapPlanet);
                    uiGame.starmap.OnCursorFunction2Click(0);
                }
            }
        }

        if (planet != null)
        {
            GameMain.mainPlayer.navigation.indicatorAstroId = planet.id;
        }
        else
        {
            if (star != null)
            {
                GameMain.mainPlayer.navigation.indicatorAstroId = star.id * 100;
            }
            else
            {
                GameMain.mainPlayer.navigation.indicatorAstroId = 0;
            }
        }
        CruiseAssistPlugin.SelectTargetAstroId = GameMain.mainPlayer.navigation.indicatorAstroId;
        CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
        {
            extension.SetTargetAstroId(CruiseAssistPlugin.SelectTargetAstroId);
        });
    }

    private static void UIStarmap_OnStarClick(UIStarmap starmap, UIStarmapStar star)
    {
        var traverse = Traverse.Create(starmap);
        if (starmap.focusStar != star)
        {
            if (starmap.viewPlanet != null || (starmap.viewStar != null && star.star != starmap.viewStar))
            {
                starmap.screenCameraController.DisablePositionLock();
            }
            starmap.focusPlanet = null;
            starmap.focusStar = star;
            traverse.Field("_lastClickTime").SetValue(0.0);
        }
        traverse.Field("forceUpdateCursorView").SetValue(true);
    }

    private static void UIStarmap_OnPlanetClick(UIStarmap starmap, UIStarmapPlanet planet)
    {
        var traverse = Traverse.Create(starmap);
        if (starmap.focusPlanet != planet)
        {
            if ((starmap.viewPlanet != null && planet.planet != starmap.viewPlanet) || starmap.viewStar != null)
            {
                starmap.screenCameraController.DisablePositionLock();
            }
            starmap.focusPlanet = planet;
            starmap.focusStar = null;
            traverse.Field("_lastClickTime").SetValue(0.0);
        }
        traverse.Field("forceUpdateCursorView").SetValue(true);
    }

    private static int wIdx;

    public const float WindowWidth = 400f;

    public const float WindowHeight = 480f;

    public static readonly bool[] Show = new bool[2];

    public static readonly Rect[] Rect = {
        new Rect(0f, 0f, 400f, 480f),
        new Rect(0f, 0f, 400f, 480f)
    };

    public static int ListSelected;

    public static readonly int[] actionSelected = new int[3];

    private static readonly float[] lastCheckWindowLeft = { float.MinValue, float.MinValue };

    private static readonly float[] lastCheckWindowTop = { float.MinValue, float.MinValue };

    private static readonly Vector2[] scrollPos = {
        Vector2.zero,
        Vector2.zero,
        Vector2.zero
    };

    private const string VisitedMark = "● ";

    private const string NonVisitMark = "";
}