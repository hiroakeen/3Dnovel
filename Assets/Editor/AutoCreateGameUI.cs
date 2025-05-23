using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoCreateGameUI : MonoBehaviour
{
    [MenuItem("Tools/Auto Create Game UI (TMP)")]
    public static void CreateUI()
    {
        GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.layer = LayerMask.NameToLayer("UI");

        CreateUIPanel(canvasGO.transform, "CharacterSelectionPanel");
        CreateUIPanel(canvasGO.transform, "MemorySelectionPanel");
        CreateUIPanel(canvasGO.transform, "TargetSelectionPanel");
        CreateDialoguePanel(canvasGO.transform);
        CreateEndingPanel(canvasGO.transform);

        Debug.Log("Game UI Canvas (TextMeshPro) を自動生成しました。Canvas オブジェクトを UIManager にリンクしてください。");
    }

    private static GameObject CreateUIPanel(Transform parent, string name)
    {
        GameObject panel = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(parent);
        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        GameObject layout = new GameObject("CharacterButtonParent", typeof(RectTransform), typeof(VerticalLayoutGroup));
        layout.transform.SetParent(panel.transform);
        RectTransform layoutRt = layout.GetComponent<RectTransform>();
        layoutRt.anchorMin = new Vector2(0.5f, 0.5f);
        layoutRt.anchorMax = new Vector2(0.5f, 0.5f);
        layoutRt.sizeDelta = new Vector2(400, 500);
        layoutRt.anchoredPosition = Vector2.zero;

        return panel;
    }

    private static void CreateDialoguePanel(Transform parent)
    {
        GameObject panel = new GameObject("DialoguePanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(parent);
        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        GameObject textGO = new GameObject("DialogueText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textGO.transform.SetParent(panel.transform);
        RectTransform textRt = textGO.GetComponent<RectTransform>();
        textRt.anchorMin = new Vector2(0.5f, 0.5f);
        textRt.anchorMax = new Vector2(0.5f, 0.5f);
        textRt.sizeDelta = new Vector2(600, 150);
        textRt.anchoredPosition = Vector2.zero;

        TextMeshProUGUI text = textGO.GetComponent<TextMeshProUGUI>();
        text.text = "セリフがここに表示されます";
        text.fontSize = 24;
        text.alignment = TextAlignmentOptions.Center;
    }

    private static void CreateEndingPanel(Transform parent)
    {
        GameObject panel = new GameObject("EndingPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(parent);
        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        GameObject titleGO = new GameObject("EndingTitleText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        titleGO.transform.SetParent(panel.transform);
        RectTransform titleRt = titleGO.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0.5f, 0.7f);
        titleRt.anchorMax = new Vector2(0.5f, 0.7f);
        titleRt.sizeDelta = new Vector2(600, 80);
        titleRt.anchoredPosition = Vector2.zero;

        TextMeshProUGUI titleText = titleGO.GetComponent<TextMeshProUGUI>();
        titleText.text = "エンディングタイトル";
        titleText.fontSize = 30;
        titleText.alignment = TextAlignmentOptions.Center;

        GameObject descGO = new GameObject("EndingDescriptionText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        descGO.transform.SetParent(panel.transform);
        RectTransform descRt = descGO.GetComponent<RectTransform>();
        descRt.anchorMin = new Vector2(0.5f, 0.5f);
        descRt.anchorMax = new Vector2(0.5f, 0.5f);
        descRt.sizeDelta = new Vector2(600, 200);
        descRt.anchoredPosition = Vector2.zero;

        TextMeshProUGUI descText = descGO.GetComponent<TextMeshProUGUI>();
        descText.text = "エンディングの内容がここに表示されます";
        descText.fontSize = 24;
        descText.alignment = TextAlignmentOptions.Center;
    }
}