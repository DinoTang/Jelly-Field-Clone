using UnityEngine;

public class PlayBtn : BaseBtn
{
    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();

        SceneLoader.Instance.LoadScene("GamePlay");

    }
}