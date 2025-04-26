#nullable enable

using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

using UnityEngine;
using VContainer;

public class EntityFactoryMono : MonoBehaviour
{
    [Header("Entity Prefabs")]
    [SerializeField] GameObject circlePrefab = null!;
    [SerializeField] GameObject starPrefab = null!;
    [SerializeField] GameObject gearPrefab = null!;
    [SerializeField] GameObject heartPrefab = null!;
    [SerializeField] GameObject squarePrefab = null!;

    int circleCount = 18;
    int starCount = 18;
    int gearCount = 12;
    int heartCount = 12;
    int squareCount = 6;

    [SerializeField] Transform spawnParent = null!;

    EntityActionExecutor _entityActionExecutor;
    EntityInstanceHolderMono _entityInstanceHolderMono;

    [Inject]
    public void Construct(
        EntityActionExecutor entityActionExecutor,EntityInstanceHolderMono entityInstanceHolderMono)
    {
        _entityActionExecutor = entityActionExecutor;
        _entityInstanceHolderMono = entityInstanceHolderMono;
    }

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
                actionInterval = 2;
                break;
            case EntityKind.Star:
                prefab = starPrefab;
                count = starCount;
                moveSpeed = 4f;
                actionInterval = 2;
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
                actionInterval = 2;
                break;
            case EntityKind.Square:
                prefab = squarePrefab;
                count = squareCount;
                moveSpeed = 0f; // 動かない
                actionInterval = 3;
                break;
        }

        List<IEntity> ientities = new ();
        IEntity? truthEntity = null;
        int truthIndex = Random.Range(0, count);
        for (int i = 0; i < count; i++)
        {
            bool isTruth = false;
            if (i == truthIndex)
                isTruth = true;

            var entityObj = Instantiate(prefab, GetAlignedPosition(i,count), Quaternion.identity, spawnParent);
            var entity = entityObj.GetComponent<IEntity>();

            if (entity != null)
            {
                entity.Init(moveSpeed, actionInterval, isTruth);

                if (isTruth)
                {
                    _entityActionExecutor.EntityAction += entity.Action;
                    truthEntity = entity;
                }

                ientities.Add(entity);
            }
        }

        _entityInstanceHolderMono.SetList(ientities, truthEntity);
    }

    Vector2 GetAlignedPosition(int index, int count)
    {
        int columns = 6;            // 横に並べる数（偶数推奨）
        float spacing = 3.0f;       // 各オブジェクト間の距離

        int rows = Mathf.CeilToInt((float)count / columns);
        int row = index / columns;
        int col = index % columns;

        float xOffset = col * spacing;

        // 全体の横幅から中央基準で揃える
        float totalWidth = columns * spacing;
        float originX = -totalWidth / 2f + spacing / 2f;

        // 縦方向の揃え（中央から上下に並べたい場合の調整）
        float totalHeight = rows * spacing;
        float originY = totalHeight / 2f - spacing / 2f;

        Vector2 position = new Vector2(originX + xOffset, originY - row * spacing);
        return position;
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