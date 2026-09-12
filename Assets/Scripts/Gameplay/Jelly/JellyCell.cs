using UnityEngine;

public class JellyCell : BaseBehaviour
{
    public JellyPiece[] jellyPieces = new JellyPiece[4];
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadJellyPieces();
        this.Arrange();
    }

    protected void LoadJellyPieces()
    {
        foreach (Transform child in transform)
        {
            JellyPiece jellyPiece = child.GetComponent<JellyPiece>();
            if (jellyPiece == null) continue;
            for (int i = 0; i < jellyPieces.Length; i++)
            {
                if (jellyPieces[i] != null) continue;
                jellyPieces[i] = jellyPiece;
                break;
            }
        }
    }

    public void Arrange()
    {
        float width = 0.95f;
        float height = 0.95f;
        Vector3 center = transform.position;

        Vector3 topLeft =
            center + new Vector3(-width / 4f, height / 4f, 0);

        Vector3 topRight =
            center + new Vector3(width / 4f, height / 4f, 0);

        Vector3 bottomLeft =
            center + new Vector3(-width / 4f, -height / 4f, 0);

        Vector3 bottomRight =
            center + new Vector3(width / 4f, -height / 4f, 0);


        jellyPieces[0].transform.position = topLeft;
        jellyPieces[1].transform.position = topRight;
        jellyPieces[2].transform.position = bottomLeft;
        jellyPieces[3].transform.position = bottomRight;
    }

}
