using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBox : MonoBehaviour {
    //------------Unityコンポーネント--------------
    //子供オブジェクト取得用変数
    private GameObject[] boxes;

    private GameObject player;
    // 0 通常時
    // 1 加熱時
    [SerializeField] Material[] material;

    private Renderer ironBox;

    private Rigidbody boxRigid;

    //-----------クラスの定義--------------------
    //SEコントローラを参照します
    [SerializeField] private SEController seController;

    private TutorialController tutorialController;

    //----------数値用変数-----------------------
    //炎消去時間（カウンター）
    private float destroyTime;
    //炎消去時間
    private const float destroyMaxTime = 1.5f;
    //鉄箱の準備時間
    [SerializeField] private float preparationStartTime = 5.0f;
    //------------フラグ用変数---------------------
    //スライムがヒットしたかどうか
    private bool isHitFlag;
    //??????何?????　isPlaying作成したほうがいい？？
    bool start;

    //鉄箱が熱せられたかどうかの判定
    [SerializeField] private bool isFire = false;

    // Use this for initialization
    void Start () {
        start = true;
        //スライムがヒットしたかどうか
        isHitFlag = false;
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

        if (isFire)
        {
            //炎パーティクルをoff
            boxes[0].SetActive(false);
            //マテリアルをHOT状態に変更
            boxes[2].GetComponent<Renderer>().material = material[1];
        }

        boxRigid = gameObject.GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            //スタートしてからカウントUp
            preparationStartTime -= Time.deltaTime;
            //スタートして3秒間物理演算切る
            if (preparationStartTime > 0)
            {
                boxRigid.isKinematic = true;
            }
            else if (preparationStartTime < 0)
            {
                boxRigid.isKinematic = false;
                start = false;
            }
        }
        //赤スライムがヒットしたとき
        if (isHitFlag == true)
        {
            destroyTime -= Time.deltaTime;
            //炎パーティクルをOn
            boxes[0].SetActive(true);

            //炎消去時間が0以下の時
            if (destroyTime <= 0)
            {
                //hitFlag = false;
                isFire = true;
                //炎パーティクルをoff
                boxes[0].SetActive(false);
                //マテリアルをHOT状態に変更
                boxes[2].GetComponent<Renderer>().material = material[1];


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
        //煙表示
        boxes[1].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        //SE音を止める
        seController.StopSE();
        //煙非表示
        boxes[1].SetActive(false);
        //マテリアルを通常状態に変更
        boxes[2].GetComponent<Renderer>().material = material[0];
    }
    //プレイヤーが当たった時の処理
    private void OnCollisionEnter(Collision other)
    {
        if (isFire && other.gameObject.tag == "Player")
        {
            seController.playSE(5);
            player = other.gameObject;
            player.GetComponent<PlayerController>().isHotIronHit = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //赤スライムが当たった時の場合
        if (other.gameObject.tag == "Children_Red")
        {
            if (isHitFlag == false)
            {
                //燃えている音を再生
                seController.playSE(2);
                destroyTime = destroyMaxTime;
                isHitFlag = true;
            }
            Destroy(other.gameObject);
        }
        //青スライムが当たった時の場合
        else if (other.gameObject.tag == "Children_Blue")
        {
            if (isHitFlag == true)
            {
                //SE音を止める
                seController.StopSE();
                //消火音の再生
                seController.playSE(3);
                StartCoroutine(OnSmoke());
                isHitFlag = false;
                isFire = false;
            }

                Destroy(other.gameObject);
        }else if(other.gameObject.tag == "Children_Green")
        {
            Destroy(other.gameObject);
        }

    }
    /// <summary>
    /// プレイヤーか群れプニが鉄箱に触れていた場合
    /// 鉄箱の物理演算をきります
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Children")
        {
            boxRigid.isKinematic = true;
        }
    }
    /// <summary>
    /// プレイヤーと群れプニが鉄箱に当たっていなかった場合
    /// 鉄箱の物理演算を再開します
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Children")
        {
            boxRigid.isKinematic = false;
        }
    }
}
