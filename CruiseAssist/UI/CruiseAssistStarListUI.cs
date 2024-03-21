using System.Collections.Generic;
using System.Linq;
using CruiseAssist.Commons;
using HarmonyLib;
using UnityEngine;

namespace CruiseAssist.UI;

public static class CruiseAssistStarListUI
{
    private static GUIStyle _toolbarStyle;
    private static GUIStyle _nameLabelStyle;
    private static GUIStyle _nameLabelHighlightStyle;
    private static GUIStyle _nRangeLabelStyle;
    private static GUIStyle _nRangeLabelHighlightStyle;
    private static GUIStyle _nActionButtonStyle;
    private static GUIStyle _nSortButtonStyle;
    private static GUIStyle _verticalScrollbarStyle;
    private static GUIStyle _commonButtonStyle;
    private static GUIStyle _searchLabelStyle;
    private static string[] _tabs = [
        Strings.Get(10),
        Strings.Get(11),
        Strings.Get(12)
    ];

    private static string[][] _buttonTexts =
    [
        [Strings.Get(13), Strings.Get(12)],
        [Strings.Get(13), Strings.Get(12), Strings.Get(15)],
        [Strings.Get(13), Strings.Get(16), Strings.Get(15)]
    ];

    public static void OnInit()
    {
        Strings.OnLanguageChanged += LangChanged;
        LangChanged();
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
            alignment = TextAnchor.MiddleLeft,
        };
        _nameLabelHighlightStyle = new GUIStyle(_nameLabelStyle)
        {
            normal =
            {
                textColor = Color.cyan
            }
        };
        _nRangeLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 60f,
            fixedHeight = 20f,
            fontSize = 14,
            alignment = TextAnchor.MiddleRight,
            normal = {textColor = Color.white}
        };
        _nRangeLabelHighlightStyle = new GUIStyle(_nRangeLabelStyle)
        {
            normal =
            {
                textColor = Color.cyan
            }
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
        _verticalScrollbarStyle = CruiseAssistMainUI.BaseVerticalScrollBarStyle;
        _commonButtonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 80f,
            fixedHeight = 20f,
            fontSize = 12
        };
        _searchLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 50f,
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.MiddleRight
        };
        return;

        void LangChanged()
        {
            _tabs = [Strings.Get(10), Strings.Get(11), Strings.Get(12)];
            _buttonTexts = [[Strings.Get(13), Strings.Get(14)], [Strings.Get(13), Strings.Get(14), Strings.Get(15)], [Strings.Get(13), Strings.Get(16), Strings.Get(15)]];
        }
    }

    public static void OnGUI()
    {
        _wIdx = CruiseAssistMainUI.WIdx;
        Rect[_wIdx] = GUILayout.Window(99030292, Rect[_wIdx], WindowFunction, "CruiseAssist - " + Strings.Get(8), CruiseAssistMainUI.WindowStyle);
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
        var selectedIndex = GUILayout.Toolbar(ListSelected, _tabs, _toolbarStyle);
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
                GUILayout.BeginHorizontal();
                GUILayout.Label(Strings.Get(32), _searchLabelStyle);
                GUI.changed = false;
                _searchString = GUILayout.TextField(_searchString, CruiseAssistMainUI.BaseTextFieldStyle);
                if (GUI.changed)
                {
                    _searchResults = string.IsNullOrEmpty(_searchString)
                        ? null
                        : _celestialBodies.Where(celestialBody => (celestialBody.IsPlanet ? celestialBody.PlanetData.name : celestialBody.StarData.name).Contains(_searchString)).ToList();
                }
                GUILayout.EndHorizontal();
                foreach (var celestialBody in _searchResults ?? _celestialBodies)
                {
                    var selected = celestialBody.Selected;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(celestialBody.Name, selected ? _nameLabelHighlightStyle : _nameLabelStyle);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(CruiseAssistMainUI.RangeToString(celestialBody.Range), selected ? _nRangeLabelHighlightStyle : _nRangeLabelStyle);
                    if (GUILayout.Button(ActionSelected[ListSelected] == 0 ? Strings.Get(17) :
                            celestialBody.IsPlanet ? celestialBody.InBookmark ? Strings.Get(19) : Strings.Get(18) : "-",
                            _nActionButtonStyle))
                    {
                        VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                        var planetData = celestialBody.PlanetData;
                        if (ActionSelected[ListSelected] == 0)
                        {
                            SelectStar(celestialBody.StarData, planetData);
                            if (CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag)
                            {
                                Show[_wIdx] = false;
                            }
                        }
                        else if (planetData != null)
                        {
                            if (RemoveBookmark(planetData))
                            {
                                celestialBody.InBookmark = false;
                                var bb = History.Find(b => b.PlanetData == planetData);
                                if (bb != null) bb.InBookmark = false;
                            }
                            else if (Bookmark.Count <= 128 && AddBookmark(planetData))
                            {
                                celestialBody.InBookmark = true;
                                var bb = History.Find(b => b.PlanetData == planetData);
                                if (bb != null) bb.InBookmark = true;
                                CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                break;
            case 1 or 2:
            {
                UpdateHistoryOrBookmarkList();
                var highlighted = false;
                var enumBookmark = ListSelected != 1 ? Bookmark : Enumerable.Reverse(History);
                if (ListSelected == 1 && ActionSelected[ListSelected] != 2 && CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag)
                {
                    enumBookmark = _historyDistinct;
                }

                var listIndex = 0;
                foreach (var body in enumBookmark)
                {
                    var planetData = body.PlanetData;
                    var id = planetData.id;
                    var selected = !highlighted && selectTargetPlanet != null && planetData.id == selectTargetPlanet.id;
                    if (selected)
                        highlighted = true;

                    GUILayout.BeginHorizontal();
                    var name = CruiseAssistPlugin.GetStarName(planetData.star) + " - " + CruiseAssistPlugin.GetPlanetName(planetData);
                    if (CruiseAssistPlugin.Conf.MarkVisitedFlag)
                    {
                        name = (planetData.factory != null ? VisitedMark : NonVisitMark) + name;
                    }

                    GUILayout.Label(name, selected ? _nameLabelHighlightStyle : _nameLabelStyle);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(CruiseAssistMainUI.RangeToString(body.Range), selected ? _nRangeLabelHighlightStyle : _nRangeLabelStyle);
                    if (ListSelected == 2 && ActionSelected[ListSelected] == 1)
                    {
                        var first = Bookmark[0] == body;
                        var last = Bookmark.Last() == body;
                        if (GUILayout.Button(last ? "-" : "↓", _nSortButtonStyle))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            if (!last)
                            {
                                var index = Bookmark.IndexOf(body);
                                Bookmark.RemoveAt(index);
                                Bookmark.Insert(index + 1, body);
                                goto outside;
                            }
                        }

                        if (GUILayout.Button(first ? "-" : "↑", _nSortButtonStyle))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            if (!first)
                            {
                                var index = Bookmark.IndexOf(body);
                                Bookmark.RemoveAt(index);
                                Bookmark.Insert(index - 1, body);
                                goto outside;
                            }
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(
                                ActionSelected[ListSelected] == 0 ? Strings.Get(17) :
                                ActionSelected[ListSelected] == 2 ? ListSelected == 1 && listIndex == 0 ? "-" : Strings.Get(19) :
                                body.InBookmark ? Strings.Get(19) : Strings.Get(18), _nActionButtonStyle))
                        {
                            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
                            switch (ActionSelected[ListSelected])
                            {
                                case 0:
                                    SelectStar(planetData.star, planetData);
                                    if (CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag)
                                    {
                                        Show[_wIdx] = false;
                                    }

                                    break;
                                case 1:
                                {
                                    if (ListSelected != 1) break;
                                    if (HasBookmark(id))
                                    {
                                        RemoveBookmark(planetData);
                                        var b = _celestialBodies.Find(b => b.IsPlanet && b.PlanetData == planetData);
                                        if (b != null) b.InBookmark = false;
                                        var bb = History.Find(bb => bb.PlanetData == planetData);
                                        if (bb != null) bb.InBookmark = false;
                                    }

                                    if (Bookmark.Count > 128 || !AddBookmark(planetData)) goto outside;
                                    var b2 = _celestialBodies.Find(b => b.IsPlanet && b.PlanetData == planetData);
                                    if (b2 != null) b2.InBookmark = true;
                                    var bb2 = History.Find(b => b.PlanetData == planetData);
                                    if (bb2 != null) bb2.InBookmark = true;
                                    CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
                                    goto outside;
                                }
                                case 2:
                                    switch (ListSelected)
                                    {
                                        case 1:
                                            if (listIndex != 0)
                                            {
                                                if (CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag)
                                                    RemoveHistoryDistinctAt(listIndex);
                                                else
                                                    RemoveHistoryAt(History.Count - 1 - listIndex);
                                                CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
                                                goto outside;
                                            }
                                            break;

                                        case 2:
                                            RemoveBookmark(planetData);
                                            var b = _celestialBodies.Find(b => b.IsPlanet && b.PlanetData == planetData);
                                            if (b != null) b.InBookmark = true;
                                            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                    listIndex++;
                }
                outside:
                break;
            }
        }

        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_buttonTexts[ListSelected][ActionSelected[ListSelected]], _commonButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            ActionSelected[ListSelected]++;
            ActionSelected[ListSelected] %= _buttonTexts[ListSelected].Length;
        }

        GUILayout.FlexibleSpace();
        if (!CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag && GUILayout.Button(Strings.Get(20), _commonButtonStyle))
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
        CruiseAssistPlugin.SelectTargetStar = star;
        CruiseAssistPlugin.SelectTargetPlanet = planet;
        var uiGame = UIRoot.instance.uiGame;
        if (CruiseAssistPlugin.Conf.SelectFocusFlag && uiGame.starmap.active)
        {
            if (star != null)
            {
                var uistarmapStar = uiGame.starmap.starUIs.FirstOrDefault(s => s.star.id == star.id);
                if (uistarmapStar != null)
                {
                    UIStarmap_OnStarClick(uiGame.starmap, uistarmapStar);
                    uiGame.starmap.OnCursorFunction2Click(0);
                }
            }

            if (planet != null)
            {
                var uistarmapPlanet = uiGame.starmap.planetUIs.FirstOrDefault(p => p.planet.id == planet.id);
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
        CruiseAssistPlugin.Extensions.ForEach(extension => extension.SetTargetAstroId(CruiseAssistPlugin.SelectTargetAstroId));
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
            starmap._lastClickTime = 0.0;
        }

        starmap.forceUpdateCursorView = true;
    }

    public static void OnReset()
    {
        _celestialBodies = null;
        _stars = null;
        _localStarPlanets = null;
        _selectedStarPlanets = null;
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
            _stars = null;
        }
    }

    private static void UpdateStarPlanetList()
    {
        if (_stars == null)
        {
            LoadAllStars();
            LoadLocalStarPlanets();
            LoadSelectedStarPlanets();
            SortCelestialBodies();
            return;
        }

        var reorder = LoadLocalStarPlanets();
        if (LoadSelectedStarPlanets() || reorder) SortCelestialBodies();
    }

    private static void UpdateHistoryOrBookmarkList()
    {
        if (_nextCheckTick2 > GameMain.instance.timei)
        {
            return;
        }
        _nextCheckTick2 = GameMain.instance.timei + (GameMain.data.localPlanet == null ? 30 : 300);
        foreach (var body in ListSelected == 1 ? _historyDistinct : Bookmark)
        {
            body.Range = (body.PlanetData.uPosition - GameMain.mainPlayer.uPosition).magnitude - body.PlanetData.realRadius;
        }
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
                IsPlanet = false
            });
        }
    }

    private static bool LoadLocalStarPlanets()
    {
        var localPlanet = GameMain.data.localPlanet;
        if (localPlanet != null && localPlanet == _lastLocalPlanet)
        {
            if (GameMain.instance.timei < _nextCheckTick) return false;
            _nextCheckTick = GameMain.instance.timei + 300;
            return true;
        }
        _lastLocalPlanet = GameMain.data.localPlanet;

        var localStar = GameMain.localStar;
        var mainPlayer = GameMain.data.mainPlayer;
        var currentPos = mainPlayer.uPosition;
        if (localStar == _lastLocalStar)
        {
            if (GameMain.instance.timei < _nextCheckTick) return false;
            if ((currentPos - _lastPlayerPos).sqrMagnitude < 2000.0 * 2000.0)
                return true;
        }

        _lastLocalStar = localStar;
        _lastPlayerPos = currentPos;
        _nextCheckTick = GameMain.instance.timei + 300;

        if (localStar == null)
        {
            if (_localStarPlanets is { Count: 0 }) return false;
            _localStarPlanets = [];
            return true;
        }

        _localStarPlanets = [];
        var markVisitedFlag = CruiseAssistPlugin.Conf.MarkVisitedFlag;
        foreach (var planet in localStar.planets)
        {
            if (planet == null) continue;
            var visted = markVisitedFlag && planet.factory != null;
            _localStarPlanets.Add(new CelestialBody
            {
                PlanetData = planet,
                Name = (visted ? VisitedMark : NonVisitMark) + CruiseAssistPlugin.GetPlanetName(planet),
                Pos = planet.uPosition,
                IsPlanet = true,
                InBookmark = HasBookmark(planet.id)
            });
        }

        return true;
    }

    private static bool LoadSelectedStarPlanets()
    {
        var selectedStar = CruiseAssistPlugin.SelectTargetStar;
        if (selectedStar == _lastSelectedStar)
        {
            if (_selectedStarPlanets != null) return false;
            _selectedStarPlanets = [];
            return true;
        }
        _lastSelectedStar = selectedStar;
        if (selectedStar == null || selectedStar == GameMain.localStar)
        {
            if (_selectedStarPlanets is { Count: 0 }) return false;
            _selectedStarPlanets = [];
            return true;
        }

        var range = (selectedStar.uPosition - GameMain.mainPlayer.uPosition).magnitude;
        if (GameMain.history.universeObserveLevel < (range >= 14400000.0 ? 4 : 3))
        {
            if (_selectedStarPlanets is { Count: 0 }) return false;
            _selectedStarPlanets = [];
            return true;
        }

        _selectedStarPlanets = [];
        var markVisitedFlag = CruiseAssistPlugin.Conf.MarkVisitedFlag;
        foreach (var planet in selectedStar.planets)
        {
            if (planet == null) continue;
            var visted = markVisitedFlag && planet.factory != null;
            _selectedStarPlanets.Add(new CelestialBody
            {
                PlanetData = planet,
                Name = (visted ? VisitedMark : NonVisitMark) + CruiseAssistPlugin.GetPlanetName(planet),
                Pos = planet.uPosition,
                IsPlanet = true,
                InBookmark = HasBookmark(planet.id)
            });
        }

        return true;
    }

    private static void SortCelestialBodies()
    {
        _celestialBodies = [.._stars, .._localStarPlanets, .._selectedStarPlanets];
        var uPos = GameMain.mainPlayer.uPosition;
        var selectedId = CruiseAssistPlugin.SelectTargetPlanet?.id ?? 0;
        var selectedStarId = CruiseAssistPlugin.SelectTargetStar?.id ?? 0;
        foreach (var body in _celestialBodies)
        {
            if (body.IsPlanet)
            {
                body.Pos = body.PlanetData.uPosition;
                body.Range = (body.Pos - uPos).magnitude - body.PlanetData.realRadius;
                body.Selected = body.PlanetData.id == selectedId || (body.PlanetData.star?.id ?? 0) == selectedStarId;
            }
            else
            {
                body.Pos = body.StarData.uPosition;
                body.Range = (body.Pos - uPos).magnitude - (body.StarData.viewRadius - 120f);
                body.Selected = body.StarData.id == selectedStarId;
            }
        }

        _celestialBodies.Sort((s1, s2) => s1.Range.CompareTo(s2.Range));
    }

    public static void AddHistory(PlanetData planet)
    {
        if (History.Count >= 128)
        {
            History.RemoveAt(0);
        }
        var b = History.Find(b => b.PlanetData == planet);
        if (b != null)
            History.Add(b);
        else
            History.Add(new BookmarkCelestialBody
            {
                PlanetData = planet,
                Range = (planet.uPosition - GameMain.mainPlayer.uPosition).magnitude - planet.realRadius,
                InBookmark = HasBookmark(planet.id)
            });
        _historyDistinct = Enumerable.Reverse(History).Distinct().ToList();
    }

    private static void RemoveHistoryAt(int index)
    {
        History.RemoveAt(index);
        _historyDistinct = Enumerable.Reverse(History).Distinct().ToList();
    }

    private static void RemoveHistoryDistinctAt(int index)
    {
        if (index < 0 || index >= _historyDistinct.Count) return;
        var body = _historyDistinct[index];
        History.RemoveAll(b => b == body);
        _historyDistinct.RemoveAt(index);
    }

    public static void ClearHistory()
    {
        History.Clear();
        _historyDistinct.Clear();
    }
    
    public static int LastHistoryId() => History.Count > 0 ? History.Last().PlanetData.id : 0;

    public static string HistoryToString() => History.Join(body => body.PlanetData.id.ToString(), ",");
    
    public static void HistoryFromString(string str)
    {
        Bookmark.Clear();
        foreach (var s in str.Split(','))
        {
            if (!int.TryParse(s, out var id)) continue;
            var planet = GameMain.galaxy.PlanetById(id);
            if (planet == null) continue;
            var b = History.Find(b => b.PlanetData == planet);
            if (b != null)
            {
                History.Add(b);
                continue;
            }
            History.Add(new BookmarkCelestialBody
            {
                PlanetData = planet,
                Range = (planet.uPosition - GameMain.mainPlayer.uPosition).magnitude - planet.realRadius,
                InBookmark = HasBookmark(planet.id)
            });
        }
        _historyDistinct = Enumerable.Reverse(History).Distinct().ToList();
    }

    private static bool HasBookmark(int id) => BookmarkSet.Contains(id);

    private static bool AddBookmark(PlanetData planet)
    {
        if (!BookmarkSet.Add(planet.id)) return false;
        Bookmark.Add(new BookmarkCelestialBody
        {
            PlanetData = planet,
            Range = (planet.uPosition - GameMain.mainPlayer.uPosition).magnitude - planet.realRadius,
            InBookmark = true
        });
        return true;
    }

    private static bool RemoveBookmark(PlanetData planet)
    {
        var id = planet.id;
        if (!BookmarkSet.Remove(id)) return false;
        Bookmark.RemoveAll(body => body.PlanetData.id == id);
        return true;
    }

    public static void RemoveBookmarkAt(int index)
    {
        var body = Bookmark[index];
        BookmarkSet.Remove(body.PlanetData.id);
        Bookmark.RemoveAt(index);
    }
    
    public static void ClearBookmark()
    {
        Bookmark.Clear();
        BookmarkSet.Clear();
    }

    public static string BookmarkToString() => Bookmark.Join(body => body.PlanetData.id.ToString(), ",");

    public static void BookmarkFromString(string str)
    {
        Bookmark.Clear();
        foreach (var s in str.Split(','))
        {
            if (!int.TryParse(s, out var id)) continue;
            var planet = GameMain.galaxy.PlanetById(id);
            if (planet == null) continue;
            AddBookmark(planet);
        }
    }

    private class CelestialBody
    {
        public StarData StarData;
        public PlanetData PlanetData;
        public string Name;
        public VectorLF3 Pos;
        public double Range;
        public bool IsPlanet;
        public bool Selected;
        public bool InBookmark;
    }

    private class BookmarkCelestialBody
    {
        public PlanetData PlanetData;
        public double Range;
        public bool InBookmark;
    }

    private static List<CelestialBody> _celestialBodies;
    private static List<CelestialBody> _stars;
    private static List<CelestialBody> _localStarPlanets;
    private static List<CelestialBody> _selectedStarPlanets;
    private static List<CelestialBody> _searchResults;
    private static string _searchString;

    private static readonly List<BookmarkCelestialBody> History = [];
    private static List<BookmarkCelestialBody> _historyDistinct = [];
    private static readonly List<BookmarkCelestialBody> Bookmark = [];
    private static readonly HashSet<int> BookmarkSet = [];

    private static long _nextCheckTick;
    private static long _nextCheckTick2;
    private static StarData _lastLocalStar;
    private static PlanetData _lastLocalPlanet;
    private static StarData _lastSelectedStar;
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