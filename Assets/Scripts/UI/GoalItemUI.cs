using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalItemUI : BaseUI
{
    [SerializeField] private Image img;
    [SerializeField] private Image tickIcon;
    [SerializeField] private TextMeshProUGUI text;

    public JellyGoalData Progress { get; private set; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadImage();
        this.LoadTickIcon();
        this.LoadTextMeshPro();
    }

    protected void LoadImage()
    {
        if (this.img != null) return;

        this.img = GetComponentInChildren<Image>();

        Debug.Log(transform.name + ": LoadImage");
    }
    protected void LoadTickIcon()
    {
        if (this.tickIcon != null) return;

        this.tickIcon = transform.Find("TickIcon").GetComponent<Image>();

        Debug.Log(transform.name + ": LoadTickIcon");
    }
    protected void LoadTextMeshPro()
    {
        if (this.text != null) return;

        this.text = GetComponentInChildren<TextMeshProUGUI>();

        Debug.Log(transform.name + ": LoadTextMeshPro");
    }

    public void SetProgress(JellyGoalData progress)
    {
        this.Progress = progress;
        this.Refresh();
    }

    public void Refresh()
    {
        if (this.Progress == null) return;

        this.text.text = this.Progress.TargetAmount.ToString();

        if (this.Progress.Goal != null)
            this.img.sprite = this.Progress.Goal.Icon;

        if (this.Progress.TargetAmount <= 0)
        {
            this.text.gameObject.SetActive(false);
            this.tickIcon.gameObject.SetActive(true);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioDataSO.goalReached);
        }

    }
}