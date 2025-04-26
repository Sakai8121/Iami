using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

public class ClickUIMono : MonoBehaviour
{
    [SerializeField] Image clickUIImage;

    [Inject]
    public void Costruct(CatchEntitySystem catchEntitySystem)
    {
        catchEntitySystem.ActiveClickUIAction += ActiveClickUI;
        catchEntitySystem.DeactivateClickUIAction += DeactivateClickUI;
        catchEntitySystem.ChangeUIFillAmount += ChangeFillAmount;

        DeactivateClickUI();
    }

    public void ActiveClickUI()
    {
        clickUIImage.color = Color.red;
        clickUIImage.fillAmount = 1;
    }

    public void DeactivateClickUI()
    {
        clickUIImage.fillAmount = 0;
        clickUIImage.color = Color.white;
    }

    public void ChangeFillAmount(float value)
    {
        clickUIImage.fillAmount = value;
    }
}
