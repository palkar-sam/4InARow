using UnityEngine;

public class RayCastHitDetector : MonoBehaviour, IRayCastHitDetector
{
    public GameObject GetHit(Vector3 position, Vector2 direction)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(position, direction);

        if (hit2D.collider != null)
        {
            return hit2D.collider.gameObject;
        }

        return null;
    }
}