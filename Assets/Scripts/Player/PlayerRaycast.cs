using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{

    public Transform ray_point;
    public float rayDistance = 100f;
    public void Puntatore()
    {
        //Raycast verso centro schermo
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        Vector3 direction = (ray.origin + ray.direction * rayDistance) - ray_point.position;
        if (Physics.Raycast(ray_point.position, direction.normalized, out RaycastHit hit, rayDistance))
        {
            Debug.Log("Sparo Raycast");
            Debug.DrawLine(ray_point.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawRay(ray_point.position, direction.normalized * rayDistance, Color.green);
        }
    }
}
