#nullable enable
using System.Collections.Generic;
using UnityEngine;

public class EntitySpriteHolderMono : MonoBehaviour
{
    [SerializeField] List<EntitySpriteStruct> entitySpriteStructList = new();

    public Sprite? GetEntitySprite(EntityKind kind)
    {
        foreach (EntitySpriteStruct spriteStruct in entitySpriteStructList)
        {
            if (spriteStruct.Kind == kind)
                return spriteStruct.sprite;
        }

        return null;
    }
}

[System.Serializable]
public struct EntitySpriteStruct
{
    public EntityKind Kind;
    public Sprite sprite;
}
