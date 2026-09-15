using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseBtn : BaseBehaviour, IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        this.OnButtonClicked();
    }

    protected virtual void OnButtonClicked()
    {
        AudioClip btnClickClip = AudioManager.Instance.AudioDataSO.click;
        AudioManager.Instance.PlaySFX(btnClickClip);
    }
}