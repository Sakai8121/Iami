using UnityEngine;

public class GoalControllerMono : MonoBehaviour
{
    [SerializeField] RectTransform goalTransform = null!;
    public RectTransform GoalRect => goalTransform;
    
    
    public Vector2 GoalPosition()
    {
        return goalTransform.position;
    }
}
