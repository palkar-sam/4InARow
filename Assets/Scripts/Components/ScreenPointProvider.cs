using UnityEngine;

public class ScreenPointProvider : MonoBehaviour, IScreenPointProvider
{
    public Vector3 ScreenToWorldPoint(Vector3 position)
    {
        return Camera.main.ScreenToWorldPoint(position);
    }
}
