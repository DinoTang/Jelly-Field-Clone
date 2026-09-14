// using UnityEngine;

// public class JellyMatchSystem : BaseBehaviour
// {
//     private JellyMatchFinder matchFinder;

//     protected override void Awake()
//     {
//         base.Awake();

//         this.matchFinder = new JellyMatchFinder();
//     }

//     public void CheckMatch(BoardSlot slot)
//     {
//         // JellyMatchResult result = this.matchFinder.FindMatches(BoardManager.Instance.BoardBuilder, slot);

//         // if (!result.HasMatch)
//         //     return;

//         // this.HandleMatch(result);
//     }

//     private void HandleMatch(JellyMatchResult result)
//     {
//         Debug.Log($"Match Count: {result.MatchedPieces.Count}");

//         // Tạm thời presentation sẽ xử lý sau
//     }
// }