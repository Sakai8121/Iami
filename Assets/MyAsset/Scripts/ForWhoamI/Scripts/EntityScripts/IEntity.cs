
public interface IEntity
{
    void Init(float moveSpeed,float actionInterval);
    void EnableEntity();
    void DisEnableEntity();
    void Action();
    void Caught();
    void Move();
    void Destroy();
}