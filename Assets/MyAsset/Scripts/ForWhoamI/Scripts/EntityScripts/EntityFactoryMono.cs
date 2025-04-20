#nullable enable

using UnityEngine;

using UnityEngine;

public class EntityFactoryMono : MonoBehaviour
{
    [Header("Entity Prefabs")]
    [SerializeField] GameObject circlePrefab = null!;
    [SerializeField] GameObject starPrefab = null!;
    [SerializeField] GameObject gearPrefab = null!;
    [SerializeField] GameObject heartPrefab = null!;
    [SerializeField] GameObject squarePrefab = null!;

    [Header("Spawn Settings")]
    [SerializeField] int circleCount = 30;
    [SerializeField] int starCount = 30;
    [SerializeField] int gearCount = 10;
    [SerializeField] int heartCount = 10;
    [SerializeField] int squareCount = 3;

    [SerializeField] Transform spawnParent = null!;

    public void GenerateEntities(EntityKind entityKind)
    {
        GameObject prefab = null!;
        int count = 0;
        float moveSpeed = 0f;
        float actionInterval = 2f;

        switch (entityKind)
        {
            case EntityKind.Circle:
                prefab = circlePrefab;
                count = circleCount;
                moveSpeed = 4f;
                actionInterval = 1;
                break;
            case EntityKind.Star:
                prefab = starPrefab;
                count = starCount;
                moveSpeed = 4f;
                actionInterval = 1;
                break;
            case EntityKind.Gear:
                prefab = gearPrefab;
                count = gearCount;
                moveSpeed = 1.5f;
                actionInterval = 2;
                break;
            case EntityKind.Heart:
                prefab = heartPrefab;
                count = heartCount;
                moveSpeed = 0f; // 通常は動かないが、アクション時に加速
                actionInterval = 1;
                break;
            case EntityKind.Square:
                prefab = squarePrefab;
                count = squareCount;
                moveSpeed = 0f; // 動かない
                actionInterval = 3;
                break;
        }

        for (int i = 0; i < count; i++)
        {
            var entityObj = Instantiate(prefab, GetRandomPosition(), Quaternion.identity, spawnParent);
            var entity = entityObj.GetComponent<IEntity>();
            entity?.Init(moveSpeed, actionInterval);
        }
    }

    Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    }
}

public enum EntityKind
{
    Square,//３つだけ、動かない、アクションで拡大
    Heart,//中くらい、動かない、アクションで加速移動
    Gear,//中くらい、ゆっくり動く、アクションで回転方向の切り替え
    Star,//いっぱい、早めに動く、アクションで進行方向の切り替え
    Circle//いっぱい、早めに動く、アクションで進行方向の切り替え
}