using TMPro;
using UnityEngine;

public class CoinBarUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI text;

    protected override void Start()
    {
        base.Start();

        if (CoinManager.Instance == null) return;

        CoinManager.Instance.CoinsChanged += this.Refresh;

        this.Refresh(CoinManager.Instance.Coins);
    }

    protected override void OnDisable()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.CoinsChanged -= this.Refresh;

        base.OnDisable();
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadTextMeshProUGUI();
    }

    protected void LoadTextMeshProUGUI()
    {
        if (this.text != null) return;

        this.text = GetComponentInChildren<TextMeshProUGUI>();

        Debug.Log(transform.name + ": LoadTextMeshProUGUI");
    }

    private void Refresh(int coins)
    {
        this.text.text = coins.ToString();
    }
}