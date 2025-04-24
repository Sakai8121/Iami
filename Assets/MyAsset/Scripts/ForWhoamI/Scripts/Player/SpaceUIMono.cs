using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SpaceUIMono : MonoBehaviour
{
    [SerializeField] Image spaceUIImage;

    [Inject]
    public void Costruct(EntityActionExecutor executor)
    {
        executor.ActiveSpaceUIAction += ActiveSpaceUI;
        executor.DeactivateSpaceUIAction += DeactivateSpaceUI;
        executor.ChangeUIFillAmount += ChangeFillAmount;

        DeactivateSpaceUI();
    }

    public void ActiveSpaceUI()
    {
        spaceUIImage.color = Color.red;
        spaceUIImage.fillAmount = 1;
    }

    public void DeactivateSpaceUI()
    {
        spaceUIImage.fillAmount = 0;
        spaceUIImage.color = Color.white;
    }

    public void ChangeFillAmount(float value)
    {
        spaceUIImage.fillAmount = value;
    }
}
