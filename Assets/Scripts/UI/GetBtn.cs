using UnityEngine;

public class GetBtn : BaseBtn
{
    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();

        int reward = GameManager.Instance.CoinReward;

        CoinManager.Instance.AddCoins(reward);
        // this.gameObject.SetActive(false);

        GameManager.Instance.LoadNextLevel();
    }
}