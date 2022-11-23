using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Compressor : MonoBehaviour
{
    public Mesh Change_Mesh;
    public Material _ChangeMat;

    [Title("Distance")]
    public float Max_Y = 26f, Min_Y = 20f;

    [Title("Interval")]
    public float Door_Interval = 0.5f;
    public float Down_Move_Interval = 0.3f;
    public float Up_Move_Interval = 1f;
    public float Waiting_Interval = 1f;

    public float Min_Force = 900f;
    public float Max_Force = 1000f;
    public Ease _ease;

    public Vector3 Door_Rot = new Vector3(0f, 90f, 0f);
    Vector3 Rot_Zero = Vector3.zero;
    Transform[] Door = new Transform[2];

    [SerializeField] Vector3 Coll_Size;
    [SerializeField] Collider[] _colls;

    GameManager _gamemanager;



    Coroutine _cor;
    void Awake()
    {
        Door[0] = transform.GetChild(0);
        Door[1] = transform.GetChild(1);
        //Coll_Size = GetComponent<BoxCollider>().size;

    }

    private void OnEnable()
    {
        if (_gamemanager == null)
            _gamemanager = GameManager.instance;
        if (_cor != null)
        {
            StopCoroutine(_cor);

            _cor = null;

        }
        _cor = StartCoroutine(Cor_Update());
    }



    IEnumerator Cor_Update()
    {
        yield return null;

        while (true)
        {
            yield return null;

            DOTween.Sequence().Append(Door[0].DOLocalRotate(Door_Rot, Door_Interval)) // 문 열림 
                .Join(Door[1].DOLocalRotate(Door_Rot, Door_Interval))
                .AppendInterval(Door_Interval)

                .Append(Door[0].DOLocalRotate(Rot_Zero, Door_Interval)) // 문 닫힘
                .Join(Door[1].DOLocalRotate(Rot_Zero, Door_Interval))
                //.AppendInterval(Door_Interval)

                //.Append(transform.DOLocalMoveY(transform.position.y + Move_Y, Move_Interval).SetEase(_ease)) //  하강
                .Append(transform.DOLocalMoveY(Min_Y, Down_Move_Interval).SetEase(_ease)) //  하강 

                //.Append(transform.DOShakeScale(1f))
                .AppendInterval(1f)
                .Append(transform.DOScale(Vector3.one * 1.2f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one * 1f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one * 1.2f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one * 1f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one * 1.2f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one * 1f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one * 1.2f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one * 1f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one * 1.2f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one * 1f, 0.2f)).SetEase(Ease.OutCubic)




               .Append(Door[0].DOLocalRotate(Door_Rot, Door_Interval)) // 문 열림 
                .Join(Door[1].DOLocalRotate(Door_Rot, Door_Interval))
                .AppendCallback(() =>
                {
                    _colls = Physics.OverlapBox(transform.position, Coll_Size * 0.5f);
                    for (int i = 0; i < _colls.Length; i++)
                    {
                        if (_colls[i].CompareTag("Popcorn"))
                        {
                            var _force = new Vector3(Random.Range(-1f, 1f), 0f, 0f);
                            _colls[i].GetComponent<Rigidbody>().AddForce(_force * Random.Range(Min_Force, Max_Force));
                            _colls[i].GetComponent<MeshFilter>().sharedMesh = Change_Mesh;
                            _colls[i].GetComponent<MeshRenderer>().material = _ChangeMat;

                        }
                    }
                    //Debug.Log("Shoot");

                })
                .AppendInterval(Waiting_Interval)
                  //.Append(transform.DOLocalMoveY(transform.position.y - Move_Y, Move_Interval).SetEase(_ease)); //  상승
                  .Append(transform.DOLocalMoveY(Max_Y, Up_Move_Interval).SetEase(_ease)) //  상승
                  .AppendInterval(Waiting_Interval);
            yield return new WaitForSeconds(Door_Interval * 4f + Down_Move_Interval + Up_Move_Interval + Waiting_Interval + 1.6f);





        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Popcorn"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Popcorn"))
        {
            //other.transform.SetParent(null);
            other.transform.SetParent(_gamemanager.Current_StageManager.Using_Pool);
            other.transform.DOScale(Vector3.one, 0.1f);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Vector4(0.5f, 0.5f, 0.5f, 0.4f);
        Gizmos.DrawCube(transform.position, Coll_Size);
    }


}





//1. 문이 열림 -> 보석 들어옴
//    2. 문 닫힘
//    3. 내려감
//    4. 문 열림 + 보석 폭발하듯이 나감
//    5. 다시 위로 올라ㅆ



