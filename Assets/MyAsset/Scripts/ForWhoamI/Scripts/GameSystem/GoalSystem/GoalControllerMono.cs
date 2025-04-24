using UnityEngine;
using UnityEngine.UI;

public class GoalControllerMono : MonoBehaviour
{
    [SerializeField] RectTransform goalTransform = null!;
    [SerializeField] Image goalMaskImage = null!;
    public RectTransform GoalRect => goalTransform;
    public Vector2 GoalPosition()
    {
        return goalTransform.position;
    }

    public void SetGoalImage(Sprite sprite)
    {
        goalMaskImage.sprite = sprite;
    }
}
