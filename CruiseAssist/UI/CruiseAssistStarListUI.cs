﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using JetBrains.Annotations;

namespace CruiseAssist.UI;

public static class CruiseAssistStarListUI
{
    private static GUIStyle _toolbarStyle;
    private static GUIStyle _nameLabelStyle;
    private static GUIStyle _nRangeLabelStyle;
    private static GUIStyle _hRangeLabelStyle;
    private static GUIStyle _nActionButtonStyle;
    private static GUIStyle _hActionButtonStyle;
    private static GUIStyle _nSortButtonStyle;
    private static GUIStyle _hSortButtonStyle;
    private static GUIStyle _verticalScrollbarStyle;
    private static GUIStyle _commonButtonStyle;
    private static readonly string[] Tabs = ["Normal", "History", "Bookmark"];
    private static readonly string[][] ButtonTexts =
    [
        ["Target", "Bookmark"],
        ["Target", "Bookmark", "Delete"],
        ["Target", "Sort", "Delete"]
    ];

    public static void OnInit()
    {
        _toolbarStyle = new GUIStyle(CruiseAssistMainUI.BaseToolbarButtonStyle)
        {
            fixedWidth = 80f,
            fixedHeight = 20f,
            fontSize = 12
        };
        _nameLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 240f,
            stretchHeight = true,
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft
        };
        _nRangeLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 60f,
            fixedHeight = 20f,
            fontSize = 14,
            alignment = TextAnchor.MiddleRight
        };
        _hRangeLabelStyle = new GUIStyle(_nRangeLabelStyle)
        {
            fixedHeight = 40f
        };
        _nActionButtonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 40f,
            fixedHeight = 18f,
            margin =
            {
                top = 6
            },
            fontSize = 12
        };
        _hActionButtonStyle = new GUIStyle(_nActionButtonStyle)
        {
            margin =
            {
                top = 16
            }
        };
        _nSortButtonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 20f,
            fixedHeight = 18f,
            margin =
            {
                top = 6
            },
            fontSize = 12
        };
        _hSortButtonStyle = new GUIStyle(_nSortButtonStyle)
        {
            margin =
            {
                top = 16
            }
        };
        _verticalScrollbarStyle = new GUIStyle(CruiseAssistMainUI.BaseVerticalScrollBarStyle);
        _commonButtonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 80f,
            fixedHeight = 20f,
            fontSize = 12
        };
    }

    public static void OnGUI()
    {
        _wIdx = CruiseAssistMainUI.WIdx;
        Rect[_wIdx] = GUILayout.Window(99030292, Rect[_wIdx], WindowFunction, "CruiseAssist - StarList", CruiseAssistMainUI.WindowStyle);
        var scale = CruiseAssistMainUI.Scale / 100f;
        if (Screen.width / scale < Rect[_wIdx].xMax)
        {
            Rect[_wIdx].x = Screen.width / scale - Rect[_wIdx].width;
        }

        if (Rect[_wIdx].x < 0f)
        {
            Rect[_wIdx].x = 0f;
        }

        if (Screen.height / scale < Rect[_wIdx].yMax)
        {
            Rect[_wIdx].y = Screen.height / scale - Rect[_wIdx].height;
        }

        if (Rect[_wIdx].y < 0f)
        {
            Rect[_wIdx].y = 0f;
        }

        if (LastCheckWindowLeft[_wIdx] != float.MinValue)
        {
            if (Rect[_wIdx].x != LastCheckWindowLeft[_wIdx] || Rect[_wIdx].y != LastCheckWindowTop[_wIdx])
            {
                LastCheckWindowLeft[_wIdx] = Rect[_wIdx].x;
                LastCheckWindowTop[_wIdx] = Rect[_wIdx].y;
                CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        else
        {
            LastCheckWindowLeft[_wIdx] = Rect[_wIdx].x;
            LastCheckWindowTop[_wIdx] = Rect[_wIdx].y;
        }
    }

    private static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUI.changed = false;
        var selectedIndex = GUILayout.Toolbar(ListSelected, Tabs, _toolbarStyle);
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
        ScrollPos[ListSelected] = GUILayout.BeginScrollView(ScrollPos[ListSelected], GUIStyle.none, _verticalScrollbarStyle);
        var selectTargetPlanet = CruiseAssistPlugin.SelectTargetPlanet;
        switch (ListSelected)
        {
            case 0:
                UpdateStarPlanetList();
                var localStar = GameMain.localStar;
                var selectTargetStar = CruiseAssistPlugin.SelectTargetStar;
                var targetStarId = selectTargetStar == null ? -1 : selectTargetStar.id;
                GameMain.galaxy.stars.Select(star => new Commons.Tuple<StarData, double>(star, (star.uPosition - GameMain.mainPlayer.uPosition).magnitude)).OrderBy(tuple => tuple.Item2).Do(delegate(Commons.Tuple<StarData, double> tuple)
                {
                    var star = tuple.Item1;
                    var range = tuple.Item2;
                    var starName = CruiseAssistPlugin.GetStarName(star);
                    var ok = false;
                    if (localStar != null && star.id == localStar.id)
                    {
                        ok = true;
                    }
                    else
                    {
                        if (star.id == targetStarId && GameMain.history.universeObserveLevel >= (range >= 14400000.0 ? 4 : 3))
                        {
                            ok = true;
                        }
                    }

                    if (ok)
                    {
                        star.planets.Select(planet => new Commons.Tuple<PlanetData, double>(planet, (planet.uPosition - GameMain.mainPlayer.uPosition).magnitude)).AddItem(new Commons.Tuple<PlanetData, double>(null, (star.uPosition - GameMain.mainPlayer.uPosition).magnitude)).OrderBy(tuple2 => tuple2.Item2).Do(delegate(Commons.Tuple<PlanetData, double> tuple2)
                        {
                            GUILayout.BeginHorizontal();
                            var planetData = tuple2.Item1;
                            var distance = tuple2.Item2;
                            _nameLabelStyle.normal.textColor = Color.white;
                            _nRangeLabelStyle.normal.textColor = Color.white;
                            float height;
                            if (planetData == null)
                            {
                                if (selectTargetPlanet == null && selectTargetStar != null && star.id == selectTargetStar.id)
                                {
                                    _nameLabelStyle.normal.textColor = Color.cyan;
                                    _nRangeLabelStyle.normal.textColor = Color.cyan;
                                }
                                var name = starName;
                                if (CruiseAssistPlugin.Conf.MarkVisitedFlag)
                                {
                                    name = (star.planets.Any(p => p.factory != null) ? VisitedMark : NonVisitMark) + name;
                                }
                                GUILayout.Label(name, _nameLabelStyle);
                                height = _nameLabelStyle.CalcHeight(new GUIContent(name), _nameLabelStyle.fixedWidth);
                            }
                            else
                            {
                                if (selectTargetPlanet != null && planetData.id == selectTargetPlanet.id)
                                {
                                    _nameLabelStyle.normal.textColor = Color.cyan;
                                    _nRangeLabelStyle.normal.textColor = Color.cyan;
                                }
                                var name = starName + " - " + CruiseAssistPlugin.GetPlanetName(planetData);
                                if (CruiseAssistPlugin.Conf.MarkVisitedFlag)
                                {
                                    name = (planetData.factory != null ? VisitedMark : NonVisitMark) + name;
                                }
                                GUILayout.Label(name, _nameLabelStyle);
                                height = _nameLabelStyle.CalcHeight(new GUIContent(name), _nameLabelStyle.fixedWidth);
                            }
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(CruiseAssistMainUI.RangeToString(planetData == null ? range : distance), height < 30f ? _nRangeLabelStyle : _hRangeLabelStyle);
                            if (GUILayout.Button(ActionSelected[ListSelected] == 0 ? "SET" : planetData == null ? "-" : CruiseAssistPlugin.Bookmark.Contains(planetData.id) ? "DEL" : "ADD", height < 30f ? _nActionButtonStyle : _hActionButtonStyle))
                            {
                                VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                                if (ActionSelected[ListSelected] == 0)
                                {
                                    SelectStar(star, planetData);
                                    var closeStarListWhenSetTargetPlanetFlag = CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag;
                                    if (closeStarListWhenSetTargetPlanetFlag)
                                    {
                                        Show[_wIdx] = false;
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
                        GUILayout.BeginHorizontal();
                        _nameLabelStyle.normal.textColor = Color.white;
                        _nRangeLabelStyle.normal.textColor = Color.white;
                        if (selectTargetStar != null && star.id == selectTargetStar.id)
                        {
                            _nameLabelStyle.normal.textColor = Color.cyan;
                            _nRangeLabelStyle.normal.textColor = Color.cyan;
                        }
                        var name = starName;
                        if (CruiseAssistPlugin.Conf.MarkVisitedFlag)
                        {
                            name = (star.planets.Count(p => p.factory != null) > 0 ? VisitedMark : NonVisitMark) + name;
                        }
                        GUILayout.Label(name, _nameLabelStyle);
                        var height = _nameLabelStyle.CalcHeight(new GUIContent(name), _nameLabelStyle.fixedWidth);
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(CruiseAssistMainUI.RangeToString(range), height < 30f ? _nRangeLabelStyle : _hRangeLabelStyle);
                        if (GUILayout.Button(ActionSelected[ListSelected] == 0 ? "SET" : "-", height < 30f ? _nActionButtonStyle : _hActionButtonStyle))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            if (ActionSelected[ListSelected] == 0)
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
                if (ListSelected == 1 && ActionSelected[ListSelected] != 2 && CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag)
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
                    _nameLabelStyle.normal.textColor = Color.white;
                    _nRangeLabelStyle.normal.textColor = Color.white;
                    if (!highlighted && selectTargetPlanet != null && planetData.id == selectTargetPlanet.id)
                    {
                        _nameLabelStyle.normal.textColor = Color.cyan;
                        _nRangeLabelStyle.normal.textColor = Color.cyan;
                        highlighted = true;
                    }
                    GUILayout.BeginHorizontal();
                    var name = CruiseAssistPlugin.GetStarName(planetData.star) + " - " + CruiseAssistPlugin.GetPlanetName(planetData);
                    if (CruiseAssistPlugin.Conf.MarkVisitedFlag)
                    {
                        name = (planetData.factory != null ? VisitedMark : NonVisitMark) + name;
                    }
                    GUILayout.Label(name, _nameLabelStyle);
                    var height = _nameLabelStyle.CalcHeight(new GUIContent(name), _nameLabelStyle.fixedWidth);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(CruiseAssistMainUI.RangeToString(magnitude), height < 30f ? _nRangeLabelStyle : _hRangeLabelStyle);
                    if (ListSelected == 2 && ActionSelected[ListSelected] == 1)
                    {
                        var index = CruiseAssistPlugin.Bookmark.IndexOf(id);
                        var first = index == 0;
                        var last = index == CruiseAssistPlugin.Bookmark.Count - 1;
                        if (GUILayout.Button(last ? "-" : "↓", height < 30f ? _nSortButtonStyle : _hSortButtonStyle))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            if (!last)
                            {
                                CruiseAssistPlugin.Bookmark.RemoveAt(index);
                                CruiseAssistPlugin.Bookmark.Insert(index + 1, id);
                            }
                        }

                        if (GUILayout.Button(first ? "-" : "↑", height < 30f ? _nSortButtonStyle : _hSortButtonStyle))
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
                        if (GUILayout.Button(ActionSelected[ListSelected] == 0 ? "SET" : ActionSelected[ListSelected] == 2 ? ListSelected == 1 && listIndex == 0 ? "-" : "DEL" : CruiseAssistPlugin.Bookmark.Contains(id) ? "DEL" : "ADD", height < 30f ? _nActionButtonStyle : _hActionButtonStyle))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            switch (ActionSelected[ListSelected])
                            {
                                case 0:
                                {
                                    SelectStar(planetData.star, planetData);
                                    if (CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag)
                                    {
                                        Show[_wIdx] = false;
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
                                    if (ActionSelected[ListSelected] == 2)
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
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(ButtonTexts[ListSelected][ActionSelected[ListSelected]], _commonButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            ActionSelected[ListSelected]++;
            ActionSelected[ListSelected] %= ButtonTexts[ListSelected].Length;
        }
        GUILayout.FlexibleSpace();
        if (!CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag && GUILayout.Button("Close", _commonButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            Show[_wIdx] = false;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        if (GUI.Button(new Rect(Rect[_wIdx].width - 16f, 1f, 16f, 16f), "", CruiseAssistMainUI.CloseButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            Show[_wIdx] = false;
        }
        GUI.DragWindow();
    }

    public static void SelectStar(StarData star, PlanetData planet)
    {
        if (star == CruiseAssistPlugin.SelectTargetStar && planet == CruiseAssistPlugin.SelectTargetPlanet) return;
        _celestialBodies = null;
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
        if (starmap.focusStar != star)
        {
            if (starmap.viewPlanet != null || (starmap.viewStar != null && star.star != starmap.viewStar))
            {
                starmap.screenCameraController.DisablePositionLock();
            }
            starmap.focusPlanet = null;
            starmap.focusStar = star;
            starmap._lastClickTime = 0.0;
        }
        starmap.forceUpdateCursorView = true;
    }

    private static void UIStarmap_OnPlanetClick(UIStarmap starmap, UIStarmapPlanet planet)
    {
        if (starmap.focusPlanet != planet)
        {
            if ((starmap.viewPlanet != null && planet.planet != starmap.viewPlanet) || starmap.viewStar != null)
            {
                starmap.screenCameraController.DisablePositionLock();
            }
            starmap.focusPlanet = planet;
            starmap.focusStar = null;
            starmap._lastClickTime= 0.0;
        }
        starmap.forceUpdateCursorView = true;
    }

    public static void OnReset()
    {
        _celestialBodies = null;
        _stars = null;
        _nextCheckTick = 0;
        _lastLocalStar = null;
        _lastLocalPlanet = null;
        _lastPlayerPos = VectorLF3.zero;
        GameMain.history.onFunctionUnlocked += OnObserveUpgradeUnlocked;
    }

    private static void OnObserveUpgradeUnlocked(int func, double value, int _)
    {
        var num = (int)(value > 0.0 ? value + 0.5 : value - 0.5);
        if (func == 23 && num > 2)
        {
            _celestialBodies = null;
        }
    }

    private static void UpdateStarPlanetList()
    {
        var gameMain = GameMain.instance;
        if (gameMain == null) return;
        var gameData = GameMain.data;
        if (gameData == null) return;
        if (_stars == null) LoadAllStars();
        if (_celestialBodies == null) LoadCurrentStarPlanets();
        var localPlanet = gameData.localPlanet;
        var mainPlayer = gameData.mainPlayer;
        if (localPlanet != null && localPlanet == _lastLocalPlanet)
        {
            if (gameMain.timei < _nextCheckTick) return;
            _nextCheckTick = gameMain.timei + 300;
            return;
        }
        _lastLocalPlanet = localPlanet;
        _lastPlayerPos = mainPlayer.uPosition;
        _nextCheckTick = gameMain.timei + 300;
        var localStar = gameData.localStar;
        if (localStar == _lastLocalStar)
        {
            if ((mainPlayer.uPosition - _lastPlayerPos).sqrMagnitude < 2000.0 * 2000.0)
                return;
        }
        else
        {
            _lastLocalStar = localStar;
            if (localStar != null)
                LoadCurrentStarPlanets();
        }
        SortStars();
    }

    private static void LoadAllStars()
    {
        _stars = [];
        var markVisitedFlag = CruiseAssistPlugin.Conf.MarkVisitedFlag;
        foreach (var star in GameMain.galaxy.stars)
        {
            if (star == null) continue;
            var visted = markVisitedFlag && star.planets.Any(p => p.factory != null);
            _stars.Add(new CelestialBody
            {
                StarData = star,
                Name = (visted ? VisitedMark : NonVisitMark) + CruiseAssistPlugin.GetStarName(star),
                Pos = star.uPosition,
                Visited = visted,
                IsPlanet = false
            });
        }
    }

    private static void LoadCurrentStarPlanets()
    {
        _celestialBodies = [.._stars];
        var localStar = GameMain.localStar;
        var markVisitedFlag = CruiseAssistPlugin.Conf.MarkVisitedFlag;
        if (localStar != null)
        {
            foreach (var planet in localStar.planets)
            {
                if (planet == null) continue;
                var visted = markVisitedFlag && planet.factory != null;
                _celestialBodies.Add(new CelestialBody
                {
                    PlanetData = planet,
                    Name = (visted ? VisitedMark : NonVisitMark) + CruiseAssistPlugin.GetPlanetName(planet),
                    Pos = planet.uPosition,
                    Visited = visted,
                    IsPlanet = true
                });
            }
        }

        var selectedStar = CruiseAssistPlugin.SelectTargetStar;
        if (selectedStar == null || selectedStar == localStar) return;
        var range = (selectedStar.uPosition - GameMain.mainPlayer.uPosition).magnitude;
        if (GameMain.history.universeObserveLevel < (range >= 14400000.0 ? 4 : 3)) return;
        foreach (var planet in selectedStar.planets)
        {
            if (planet == null) continue;
            var visted = markVisitedFlag && planet.factory != null;
            _celestialBodies.Add(new CelestialBody
            {
                PlanetData = planet,
                Name = (visted ? VisitedMark : NonVisitMark) + CruiseAssistPlugin.GetPlanetName(planet),
                Pos = planet.uPosition,
                Visited = visted,
                IsPlanet = true
            });
        }
    }

    private static void SortStars()
    {
        var uPos = GameMain.mainPlayer.uPosition;
        foreach (var body in _celestialBodies)
        {
            body.Range = (body.Pos - uPos).magnitude;
        }
        _celestialBodies.Sort((s1, s2) => s1.Range.CompareTo(s2.Range));
    }

    private class CelestialBody
    {
        public StarData StarData;
        public PlanetData PlanetData;
        public string Name;
        public VectorLF3 Pos;
        public double Range;
        public bool Visited;
        public bool IsPlanet;
    }
    private static List<CelestialBody> _celestialBodies;
    private static List<CelestialBody> _stars;

    private static long _nextCheckTick;
    private static StarData _lastLocalStar;
    private static PlanetData _lastLocalPlanet;
    private static VectorLF3 _lastPlayerPos = VectorLF3.zero;
    private static int _wIdx;
    public const float WindowWidth = 400f;
    public const float WindowHeight = 480f;
    public static readonly bool[] Show = new bool[2];
    public static readonly Rect[] Rect =
    [
        new Rect(0f, 0f, 400f, 480f),
        new Rect(0f, 0f, 400f, 480f)
    ];
    public static int ListSelected;
    private static readonly int[] ActionSelected = new int[3];
    private static readonly float[] LastCheckWindowLeft = [float.MinValue, float.MinValue];
    private static readonly float[] LastCheckWindowTop = [float.MinValue, float.MinValue];
    private static readonly Vector2[] ScrollPos =
    [
        Vector2.zero,
        Vector2.zero,
        Vector2.zero
    ];
    private const string VisitedMark = "● ";
    private const string NonVisitMark = "";
}