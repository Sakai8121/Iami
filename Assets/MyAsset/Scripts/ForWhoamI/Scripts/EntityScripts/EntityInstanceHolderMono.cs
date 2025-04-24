#nullable enable
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using VContainer;

public class EntityInstanceHolderMono: MonoBehaviour
{
    [SerializeField] Material burningMaterial;

    [Inject]
    public void Construtc(TruthCheckExecutor executor)
    {
        executor.EndGameAction += DestroyFalseEntities;
        burningMaterial.SetFloat("_Threshold", 0);
    }


    List<IEntity> _entityInstanceList;
    IEntity _truthEntity;

    float _burningThreshold;

    public void SetList(List<IEntity> entityInstanceList,IEntity truthEntity)
    {
        _entityInstanceList = entityInstanceList;
        _truthEntity = truthEntity;    
    }

    public void VisibleEntities()
    {
        foreach (var entityInstance in _entityInstanceList)
        {
            entityInstance.Visible();
        }
    }

    public void DestroyFalseEntities()
    {
        foreach (var entityInstance in _entityInstanceList)
        {
            if(entityInstance != _truthEntity)
                entityInstance.DisEnableEntity();
        }

        // DOTween‚Å1•bŠÔ‚©‚¯‚Äthreshold‚ð1.1‚Ü‚Åã‚°‚é
        DOTween.To(() => _burningThreshold, x => {
            _burningThreshold = x;
            burningMaterial.SetFloat("_Threshold", _burningThreshold);
        }, 1.1f, TimeInfoStatic.burningTime);
    }
}