using UnityEngine;

public interface IRayCastHitDetector
{
    public GameObject GetHit(Vector3 position, Vector2 direction);
}