using UnityEngine;

public class RestartBtn : BaseBtn
{
    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();

        GameManager.Instance.RestartLevel();
    }
}