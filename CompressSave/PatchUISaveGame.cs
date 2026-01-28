using CompressSave.UI;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace CompressSave;

static class PatchUISaveGame
{
    public static void OnDestroy()
    {
        if (_context.ButtonCompress)
            Object.Destroy(_context.ButtonCompress.gameObject);
        if (_context.Window)
        {
            _context.SaveButton.onClick -= WrapClick;
            _context.SaveButton.onClick += _context.Window.OnSaveClick;
        }
        _OnDestroy();
    }

    [HarmonyPatch(typeof(UISaveGameWindow), nameof(UISaveGameWindow.OnSelectedChange)), HarmonyPostfix]
    private static void OnSelectedChange(UISaveGameWindow __instance)
    {
        var selected = __instance.selected;
        var compressedType = SaveUtil.SaveGetCompressType(selected == null ? null : selected._saveName);
        var prop3Text = __instance.prop3Text;
        prop3Text.text = compressedType switch
        {
            CompressionType.LZ4 => "(LZ4)" + prop3Text.text,
            CompressionType.Zstd => "(ZSTD)" + prop3Text.text,
            _ => "(N)" + prop3Text.text
        };
    }

    [HarmonyPatch(typeof(UISaveGameWindow), nameof(UISaveGameWindow._OnDestroy)), HarmonyPostfix]
    private static void _OnDestroy()
    {
        //Console.WriteLine("OnCreate");
        _context = new UIContext();
    }

    [HarmonyPatch(typeof(UISaveGameWindow), nameof(UISaveGameWindow.OnSaveClick)), HarmonyReversePatch]
    private static void OSaveGameAs(this UISaveGameWindow ui, int data) { }

    [HarmonyPatch(typeof(UISaveGameWindow), nameof(UISaveGameWindow.CheckAndSetSaveButtonEnable)), HarmonyPostfix]
    private static void CheckAndSetSaveButtonEnable(UISaveGameWindow __instance)
    {
        _OnOpen(__instance);
        if (_context.SaveButton)
            _context.ButtonCompress.button.interactable = _context.SaveButton.button.interactable;
    }

    private class UIContext
    {
        public UIButton ButtonCompress;
        public UIButton SaveButton;
        public GameObject ManualSaveTypeComboBox;
        public GameObject AutoSaveTypeComboBox;
        public Text ButtonCompressText;
        public Text SaveButtonText;
        public UISaveGameWindow Window;
    }

    [HarmonyPatch(typeof(UISaveGameWindow), nameof(UISaveGameWindow.OnSaveClick)), HarmonyPrefix]
    private static void OnSaveClick()
    {
        PatchSave.UseCompressSave = true;
        PatchSave.UseCommonSaveCompressionType();
    }

    private static UIContext _context = new();

    [HarmonyPatch(typeof(UISaveGameWindow), nameof(UISaveGameWindow._OnOpen)), HarmonyPostfix]
    private static void _OnOpen(UISaveGameWindow __instance)
    {
        if (_context.ButtonCompress)
        {
            var dist = __instance.cancelButton.transform.localPosition.x - __instance.saveButton.transform.localPosition.x - ((RectTransform)__instance.cancelButton.transform).sizeDelta.x;
            var cRectTrans = (RectTransform)_context.ButtonCompress.transform;
            var localPos = __instance.saveButton.transform.localPosition;
            cRectTrans.localPosition = new Vector3(localPos.x - dist - ((RectTransform)__instance.saveButton.transform).sizeDelta.x, localPos.y, localPos.z);
            return;
        }
        RectTransform rtrans;
        Vector3 pos;
        _context.SaveButton = __instance.saveButton;
        _context.SaveButtonText = __instance.saveButtonText;
        _context.Window = __instance;
        var gameObj = __instance.transform.Find("button-compress")?.gameObject;
        var isCreating = false;
        var theParent = __instance.saveButton.transform.parent;
        if (gameObj == null)
        {
            gameObj = Object.Instantiate(__instance.saveButton.gameObject, theParent);
            isCreating = true;
        }
        _context.ButtonCompress = gameObj.GetComponent<UIButton>();
        if (isCreating)
        {
            _context.ButtonCompress.gameObject.name = "button-compress";
            _context.ButtonCompress.button.image.color = new Color32(0xfc, 0x6f, 00, 0x77);
            var textTrans = _context.ButtonCompress.transform.Find("button-text");
            _context.ButtonCompressText = textTrans.GetComponent<Text>();
            _context.ButtonCompress.onClick += __instance.OnSaveClick;
            _context.SaveButton.onClick -= __instance.OnSaveClick;
            _context.SaveButton.onClick += WrapClick;
            _context.ButtonCompressText.text = "Save with Compression".Translate();
            var localizer = textTrans.GetComponent<Localizer>();
            if (localizer)
            {
                localizer.stringKey = "Save with Compression";
                localizer.translation = "Save with Compression".Translate();
            }
        }
        var distance = __instance.cancelButton.transform.localPosition.x - __instance.saveButton.transform.localPosition.x - ((RectTransform)__instance.cancelButton.transform).sizeDelta.x;
        rtrans = (RectTransform)_context.ButtonCompress.transform;
        pos = __instance.saveButton.transform.localPosition;
        rtrans.localPosition = new Vector3(pos.x - distance - ((RectTransform)__instance.saveButton.transform).sizeDelta.x, pos.y, pos.z);

        isCreating = false;
        gameObj = __instance.transform.Find("manual-save-type-combobox")?.gameObject;
        if (gameObj == null)
        {
            gameObj = UI.MyComboBox.CreateComboBox("manual-save-type-combobox");
            isCreating = true;
        }
        _context.ManualSaveTypeComboBox = gameObj;

        if (isCreating)
        {
            var btnCompressTrans = (RectTransform)_context.ButtonCompress.transform;
            var text = AddText("Compression for manual saves", 14, "manual-save-type-combobox-text");
            rtrans = text.rectTransform;
            rtrans.SetParent(theParent, false);
            pos = btnCompressTrans.localPosition;
            rtrans.anchorMin = btnCompressTrans.anchorMin;
            rtrans.anchorMax = btnCompressTrans.anchorMax;
            rtrans.pivot = btnCompressTrans.pivot;
            rtrans.localPosition = new Vector3(pos.x - 250f, pos.y + 45f, pos.z);

            rtrans = (RectTransform)gameObj.transform;
            rtrans.SetParent(theParent, false);
            rtrans.anchorMin = btnCompressTrans.anchorMin;
            rtrans.anchorMax = btnCompressTrans.anchorMax;
            rtrans.pivot = btnCompressTrans.pivot;
            text.UpdateGeometry();
            rtrans.localPosition = new Vector3(pos.x - 50f, pos.y + 45f, pos.z);
            var cb = rtrans.GetComponent<MyComboBox>();
            cb.WithItems("Store".Translate(), "LZ4", "Zstd").WithIndex((int)PatchSave.CompressionTypeForSaves).WithOnSelChanged((idx) =>
            {
                PatchSave.CompressionTypeForSaves = (CompressionType)idx;
                PatchSave.CompressionTypeForSavesConfig.Value = CompressSave.StringFromCompresstionType(PatchSave.CompressionTypeForSaves);
            });
        }

        isCreating = false;
        gameObj = __instance.transform.Find("auto-save-type-combobox")?.gameObject;
        if (gameObj == null)
        {
            gameObj = UI.MyComboBox.CreateComboBox("auto-save-type-combobox");
            isCreating = true;
        }
        _context.AutoSaveTypeComboBox = gameObj;
        if (isCreating)
        {
            var btnCompressTrans = (RectTransform)_context.ButtonCompress.transform;

            var text = AddText("Compression for auto saves", 14, "auto-save-type-combobox-text");
            rtrans = text.rectTransform;
            rtrans.SetParent(theParent, false);
            pos = btnCompressTrans.localPosition;
            rtrans.anchorMin = btnCompressTrans.anchorMin;
            rtrans.anchorMax = btnCompressTrans.anchorMax;
            rtrans.pivot = btnCompressTrans.pivot;
            rtrans.localPosition = new Vector3(pos.x + 160f, pos.y + 45f, pos.z);

            rtrans = (RectTransform)gameObj.transform;
            rtrans.SetParent(theParent, false);
            rtrans.anchorMin = btnCompressTrans.anchorMin;
            rtrans.anchorMax = btnCompressTrans.anchorMax;
            rtrans.pivot = btnCompressTrans.pivot;
            rtrans.localPosition = new Vector3(pos.x + 360f, pos.y + 45f, pos.z);
            var cb = rtrans.GetComponent<MyComboBox>();
            cb.WithItems(["已停用".Translate(), "Store".Translate(), "LZ4", "Zstd"]).WithIndex(PatchSave.EnableForAutoSaves.Value ? (int)PatchSave.CompressionTypeForAutoSaves + 1 : 0)
                .WithOnSelChanged((idx) =>
                {
                    if (idx == 0)
                    {
                        PatchSave.EnableForAutoSaves.Value = false;
                    }
                    else
                    {
                        PatchSave.EnableForAutoSaves.Value = true;
                        PatchSave.CompressionTypeForAutoSaves = (CompressionType)idx - 1;
                        PatchSave.CompressionTypeForAutoSavesConfig.Value = CompressSave.StringFromCompresstionType(PatchSave.CompressionTypeForAutoSaves);
                    }
                });
        }
    }

    private static void WrapClick(int data)
    {
        PatchSave.UseCompressSave = false;
        _context.Window.OSaveGameAs(data);
    }

    public static Text AddText(string label, int fontSize = 14, string objName = "label")
    {
        var txt = Object.Instantiate(UIRoot.instance.uiGame.assemblerWindow.stateText);
        txt.gameObject.name = objName;
        txt.text = label.Translate();
        txt.color = new Color(1f, 1f, 1f, 0.4f);
        txt.alignment = TextAnchor.MiddleLeft;
        txt.fontSize = fontSize;
        txt.rectTransform.sizeDelta = new Vector2(txt.preferredWidth + 8f, txt.preferredHeight + 8f);
        return txt;
    }
}
