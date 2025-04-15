#nullable enable

using System;
using UnityEngine;
using VContainer.Unity;

public class PlayerInputController : ITickable
{
    public event Action StartStretchAction = () => { };
    public event Action EndStretchAction = () => { };
    public event Action SwitchHandAction = () => { };
    public event Action RotateRightAction = () => { };
    public event Action RotateLeftAction = () => { };


    public void Tick()
    {
        //体を伸ばし始めた時のアクション
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartStretchAction();
        }

        //体を伸ばすのをやめた時のアクション
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            EndStretchAction();
        }

        //手の位置を切り替えるアクション
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchHandAction();
        }

        //右側に回転するアクション
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateRightAction();
        }

        //左側に回転するアクション
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateLeftAction();
        }
    }
}