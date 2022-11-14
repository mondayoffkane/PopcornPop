using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Goal : MonoBehaviour
{
    [SerializeField]
    //Transform _floating;
    public Transform _waiting_Pool;

    public float add_size = 0.2f;
    public float Interval = 0.2f;

    public int Max_Count = 50;
    public int Current_Count = 0;
    public bool isFull = false;

    [SerializeField] public float Col_Radius = 3f;
    [SerializeField] Goal Front_Goal, Back_Goal;

    private void Start()
    {
        transform.DOScale(Vector3.one * add_size, Interval)
                .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Popcorn"))
        {


            Popcorn _corn = other.GetComponent<Popcorn>();
            GameManager.instance.ManagerAddMoney();

            Current_Count++;

            if (GameManager.instance.Floating_Waiting_Pool.childCount <= 0)
            {
                GameManager.instance.Add_Floating_Pool(GameManager.instance.Add_Pool_Size);
            }

            Transform _floating = GameManager.instance.Floating_Waiting_Pool.GetChild(0);
            _floating.SetParent(GameManager.instance.Floating_Using_Pool);
            _floating.transform.position = other.transform.position + Vector3.up * 2f;
            _floating.localScale = Vector3.one * 0.01f;
            _floating.gameObject.SetActive(true);
            _floating.GetChild(0).GetComponent<Text>().text = GameManager.ToCurrencyString(GameManager.instance.Current_Up_Income[GameManager.instance.Current_Income_Level]);

            Color _color = _floating.GetChild(0).GetComponent<Text>().color;

            _floating.GetChild(0).GetComponent<Text>().color = new Vector4(_color.r, _color.g, _color.b, 1);




            DOTween.Sequence().Append(_floating.transform.DOMove(_floating.transform.position + Vector3.up * 3.5f, 1.5f))
                 //.Join(_floating.GetChild(0).GetComponent<Text>().DOColor(new Vector4(_color.r, _color.g, _color.b, 0.5f), 0.5f))
                 .Join(_floating.DOScale(Vector3.zero, 1.5f)).SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    _floating.SetParent(GameManager.instance.Floating_Waiting_Pool);
                    _floating.gameObject.SetActive(false);
                });



            //

            if (isFull == false)
            {
                other.gameObject.SetActive(false);
                other.transform.SetParent(_waiting_Pool);
                //other.GetComponent<MeshRenderer>().material.color = Color.white;
                other.GetComponent<MeshRenderer>().material.SetColor("_MainColor", Color.white);
                //if (Current_Count >= Max_Count)
                //{
                //    isFull = true;
                //    Full();
                //}
            }
            else
            {
                other.transform.SetParent(transform);
            }

        }
    }
    public void Set()
    {
        transform.position = new Vector3(15f, transform.position.y, transform.position.z);

        DOTween.Sequence().Append(transform.DOMoveX(0f, 1f))
            .Join(transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear)
            .SetRelative(true))
            .OnComplete(() =>
            {
                transform.DOScale(Vector3.one * add_size, Interval)
                 .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
            });
    }

    public void Full()
    {
        DOTween.Sequence().AppendInterval(2f)
            .AppendCallback(() =>
            {
                Collider[] _cols = Physics.OverlapSphere(transform.position + Vector3.up * 1.5f, Col_Radius);
                foreach (Collider _col in _cols)
                {
                    if (_col.CompareTag("Popcorn"))
                    {
                        _col.transform.SetParent(transform);
                    }
                }
                Back_Goal.Set();
            })
            .Append(transform.DOMoveX(-15f, 1f))
            .Join(transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear)
            .SetRelative(true))
            .OnComplete(() =>
            {
                Collider[] _cols = Physics.OverlapSphere(transform.position + Vector3.up * 1.5f, Col_Radius);
                foreach (Collider _col in _cols)
                {
                    if (_col.CompareTag("Popcorn"))
                    {
                        _col.gameObject.SetActive(false);
                        _col.transform.SetParent(_waiting_Pool);
                    }
                }
                DOTween.Kill(transform);

            });
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Vector4(1f, 1f, 1f, 0.3f);
    //    Gizmos.DrawSphere(transform.position + Vector3.up * 1.5f, Col_Radius);

    //}

}
