using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
   public Vector3 GetMouseWorldPosition(PointerEventData eventData)
   {
      Ray ray = Camera.main.ScreenPointToRay(eventData.position);

      Plane plane = new Plane(Vector3.forward, transform.position);

      if (plane.Raycast(ray, out float distance)) return ray.GetPoint(distance);

      return transform.position;
   }
}
