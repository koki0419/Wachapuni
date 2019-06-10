using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBox_01 : MonoBehaviour
{

    //------------Unityコンポーネント--------------
    //子供オブジェクトを取得
    GameObject[] boxes;

    //SEコントローラを参照します
    [SerializeField]
    private SEController seController;

    new private Rigidbody rigidbody;

    //-----------クラスの定義--------------------


    //----------数値用変数-----------------------
    //炎消去時間（カウンター）
    private float destroyTime;
    //炎消去時間
    [SerializeField]
    private const float destroyMaxTime = 2.0f;
    //[SerializeField]
    private float startTime = 3.0f;
    //------------フラグ用変数---------------------
    //スライムがヒットしたかどうか
    private bool hitFlag;

    bool start;

    // Use this for initialization
    void Start()
    {
        start = true;
        //スライムがヒットしたかどうか
        hitFlag = false;
        //子供オブジェクトを取得
        boxes = new GameObject[transform.childCount];

        for (int i = 0; transform.childCount > i; i++)
        {
            boxes[i] = transform.GetChild(i).gameObject;
        }
        //炎パーティクルを非表示
        boxes[0].SetActive(false);
        //煙パーティクルを非表示
        boxes[1].SetActive(false);


        rigidbody = gameObject.GetComponent<Rigidbody>();
    }



    // Update is called once per frame
    void Update()
    {

        //スタートしてからカウントUp
        startTime -= Time.deltaTime;
        //スタートして3秒間物理演算切る
        if (start)
        {
            if (startTime > 0)
            {
                rigidbody.isKinematic = false;
            }
            else if (startTime <= 0)
            {
                rigidbody.isKinematic = true;
                start = false;
            }
        }
        //赤スライムがヒットしたとき
        if (hitFlag == true)
        {
            destroyTime -= Time.deltaTime;
            //炎パーティクルをOn
            boxes[0].SetActive(true);


            //炎消去時間が0以下の時
            if (destroyTime <= 0)
            {
                //木箱を消す
                foreach (Transform child in gameObject.transform)
                {
                    Destroy(child.gameObject);
                }
                Destroy(this.gameObject);
                seController.StopSE();
            }
        }
        else
        {
            //炎パーティクルをoff
            boxes[0].SetActive(false);
        }

    }


    //------------関数の定義---------------
    //煙のコルーチン
    public IEnumerator OnSmoke()
    {
        //0.5秒待つ
        //煙表示
        boxes[1].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        seController.StopSE();
        //煙非表示
        boxes[1].SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        //赤スライムが当たった時の場合
        if (other.gameObject.tag == "Children_Red" && other.gameObject.GetComponent<MurepuniBulletController>().isUsedFlag == true)
        {
            if (hitFlag == false)
            {
                //燃えている音を鳴らす
                seController.playSE(1);
                destroyTime = destroyMaxTime;
                hitFlag = true;
            }
            Destroy(other.gameObject);
        }
        //青スライムが当たった時の場合
        if (other.gameObject.tag == "Children_Blue")
        {
            if (hitFlag == true)
            {
                seController.StopSE();
                //消火音の再生
                seController.playSE(3);
                StartCoroutine(OnSmoke());
                hitFlag = false;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Children_Green")
        {
            Destroy(other.gameObject);
        }
    }
    /// <summary>
    /// 誰もこのオブジェに接していないとき
    /// 物理演算をのもどします
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Children")
        {
            rigidbody.isKinematic = true;
        }
    }

    /// <summary>
    /// 誰もこのオブジェに接していないとき
    /// 物理演算をのもどします
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Children")
        {
            rigidbody.isKinematic = false;
        }
    }
}
