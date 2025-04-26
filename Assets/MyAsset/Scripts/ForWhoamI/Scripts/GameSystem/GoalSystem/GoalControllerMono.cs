using UnityEngine;
using UnityEngine.UI;

public class GoalControllerMono : MonoBehaviour
{
    [SerializeField] Transform goalTransform = null!;
    [SerializeField] SpriteRenderer goalMaskImage = null!;
    public Transform GoalRect => goalTransform;
    public Vector2 GoalPosition()
    {
        return goalTransform.position;
    }

    public void SetGoalImage(Sprite sprite)
    {
        goalMaskImage.sprite = sprite;
    }
}
