using TMPro;
using UnityEngine;

public class GoalBarUI : BaseUI
{
    [Header("References")]
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform goalContainer;
    [SerializeField] private TextMeshProUGUI text;
    [Header("Background Padding")]
    [SerializeField] private float horizontalPadding = 30f;
    [SerializeField] private float verticalPadding = 0f;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBackgroundRectTransform();
        this.LoadGoalContainerRectTransform();
        this.LoadTextMeshProUGUI();
    }
    protected void LoadBackgroundRectTransform()
    {
        if (this.background != null) return;

        this.background = transform.Find("Background").GetComponent<RectTransform>();

        Debug.Log(transform.name + ": LoadBackgroundRectTransform");
    }
    protected void LoadGoalContainerRectTransform()
    {
        if (this.goalContainer != null) return;

        this.goalContainer = transform.Find("GoalContainer").GetComponent<RectTransform>();

        Debug.Log(transform.name + ": LoadGoalContainerRectTransform");
    }
    protected void LoadTextMeshProUGUI()
    {
        if (this.text != null) return;

        this.text = GetComponentInChildren<TextMeshProUGUI>();

        Debug.Log(transform.name + ": LoadTextMeshProUGUI");
    }

    public void Refresh()
    {
        Canvas.ForceUpdateCanvases();

        float width = goalContainer.rect.width + horizontalPadding * 2f;
        float height = goalContainer.rect.height + verticalPadding * 2f;

        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    protected override void Start()
    {
        base.Start();

        this.Refresh();
        this.RefreshLevelText();

        if (GameManager.Instance != null)
            GameManager.Instance.LevelRestarted += this.RefreshLevelText;
    }

    protected override void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LevelRestarted -= this.RefreshLevelText;

        base.OnDisable();
    }

    private void RefreshLevelText()
    {
        if (this.text == null) return;

        this.text.text = $"LEVEL {GameManager.Instance.CurrentLevelIndex + 1}";
    }
}