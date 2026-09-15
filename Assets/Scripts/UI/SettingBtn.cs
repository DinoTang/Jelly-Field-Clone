using UnityEngine;

public class SettingBtn : BaseBtn
{
    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();

        Debug.Log(transform.name + ": comming soon");
    }
}