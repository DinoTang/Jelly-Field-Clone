using UnityEngine;


public class JellyPieceSizeConfig : BaseBehaviour
{
   [SerializeField] private float quarterWidth = 18f;
   [SerializeField] private float quarterHeight = 18f;
   [SerializeField] private float depth = 35f;

   public Vector3 Quarter => new Vector3(quarterWidth, quarterHeight, depth);
   public Vector3 HalfHorizontal => new Vector3(quarterWidth * 2f, quarterHeight, depth);
   public Vector3 HalfVertical => new Vector3(quarterWidth, quarterHeight * 2f, depth);
   public Vector3 Full => new Vector3(quarterWidth * 2f, quarterHeight * 2f, depth);
}