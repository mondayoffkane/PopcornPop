using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TweenMove : MonoBehaviour
{

    public enum State
    {
        Up_down_move,
        Left_Right_move
    }
    public State _state;

    public float Move_x, Move_y;
    public float Move_time;
    public float Rot_time;



    void Start()
    {

        switch (_state)
        {
            case State.Up_down_move:
                transform.DOMoveY( Move_y, Move_time)
           .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
                break;

            case State.Left_Right_move:
                transform.DOMoveX( Move_x, Move_time)
        .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
                break;

            default:
                break;
        }


    }


}
