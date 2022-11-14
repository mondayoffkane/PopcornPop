using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Popcorn"))
        {
            //other.GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color;

            other.GetComponent<MeshRenderer>().material.SetColor("_MainColor", GetComponent<MeshRenderer>().material.GetColor("_MainColor"));

            Popcorn _corn = other.GetComponent<Popcorn>();
            GameManager.instance.ManagerAddMoney();
            //GameManager.instance.ManagerAddMoney(_corn.Price, _corn.Price_Index);


            if (GameManager.instance.Floating_Waiting_Pool.childCount <= 0)
            {
                GameManager.instance.Add_Floating_Pool(GameManager.instance.Add_Pool_Size);
            }

            Transform _floating = GameManager.instance.Floating_Waiting_Pool.GetChild(0);
            _floating.SetParent(GameManager.instance.Floating_Using_Pool);
            _floating.transform.position = other.transform.position + Vector3.up * 1f;
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



        }
    }
}
