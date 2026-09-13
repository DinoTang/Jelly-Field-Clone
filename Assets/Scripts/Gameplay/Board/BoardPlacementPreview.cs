using UnityEngine;

public class BoardPlacementPreview : BaseBehaviour
{
    public void Show(Vector3 position)
    {
        transform.position = position;
        transform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }
}
