using System.Collections;
using TMPro;
using UnityEngine;

public class WinPanelUI : BaseUI
{
    [SerializeField] private WinContainerPanelUI winContainerPanelUI;
    [SerializeField] private BlackImageUI blackImageUI;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float revealDelay = 1f;

    private Coroutine revealCoroutine;

    protected override void Start()
    {
        base.Start();

        this.blackImageUI.Hide();
        this.winContainerPanelUI.Hide();
        if (GameManager.Instance != null)
            GameManager.Instance.GameStateChanged += this.HandleGameStateChanged;
    }


    protected override void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GameStateChanged -= this.HandleGameStateChanged;

        if (this.revealCoroutine != null)
        {
            StopCoroutine(this.revealCoroutine);
            this.revealCoroutine = null;
        }

        base.OnDisable();
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadWinContainerPanelUI();
        this.LoadBlackImageUI();
        this.LoadTextMeshPro();
    }

    protected void LoadWinContainerPanelUI()
    {
        if (this.winContainerPanelUI != null) return;

        this.winContainerPanelUI = GetComponentInChildren<WinContainerPanelUI>();
    }

    protected void LoadBlackImageUI()
    {
        if (this.blackImageUI != null) return;

        this.blackImageUI = GetComponentInChildren<BlackImageUI>();
    }

    protected void LoadTextMeshPro()
    {
        if (this.textMeshPro != null) return;

        this.textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Won)
            this.BeginReveal();

        else if (state == GameState.Playing)
            this.ResetPanel();
    }

    private void BeginReveal()
    {
        if (this.revealCoroutine != null) return;

        this.revealCoroutine = StartCoroutine(this.RevealWinPanel());
    }

    private IEnumerator RevealWinPanel()
    {
        this.blackImageUI.Show();

        yield return new WaitForSeconds(this.revealDelay);

        this.textMeshPro.text = GameManager.Instance.CoinReward.ToString();
        Debug.Log(this.textMeshPro.text);

        this.winContainerPanelUI.Show();

        this.revealCoroutine = null;
    }
    private void ResetPanel()
    {
        if (this.revealCoroutine != null)
        {
            this.StopCoroutine(this.revealCoroutine);
            this.revealCoroutine = null;
        }

        this.blackImageUI.Hide();
        this.winContainerPanelUI.Hide();
    }
}