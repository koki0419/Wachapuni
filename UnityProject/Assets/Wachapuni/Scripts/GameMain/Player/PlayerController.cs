using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //プレイヤーの状態です
    public enum SlimeType
    {
        None,
        GameStart,       //ゲーム開始
        Stand_ByMode,   //待機モード
        Normal,         //通常
        HotBoxHit,      //熱鉄箱に当たった時
        GameClear,      //クリア時モード
    }
    public SlimeType slimeType = SlimeType.None;

    //------Unityコンポーネント宣言--------------
    //Rigidbodyを取得します
    private Rigidbody charaRigidbody;
    // 自分のアニメーションコンポーネント
    private Animator animatorComponent;
    public Animator AnimatorComponent
    {
        get { return animatorComponent; }
    }
    //群れぷに（赤）を取得します
    [SerializeField] private GameObject murepuniSlimeObj_Red;
    public GameObject MurepuniSlimeObj_Red
    {
        get { return murepuniSlimeObj_Red; }
    }
    //群れぷに（緑）を取得します
    [SerializeField] private GameObject murepuniSlimeObj_Green;
    public GameObject MurepuniSlimeObj_Green
    {
        get { return murepuniSlimeObj_Green; }
    }
    //群れぷに（青）を取得します
    [SerializeField] private GameObject murepuniSlimeObj_Blue;
    public GameObject MurepuniSlimeObj_Blue
    {
        get { return murepuniSlimeObj_Blue; }
    }
    //群れぷに_バレット（赤）を取得します
    [SerializeField] private GameObject murepuniSlimeObj_RedBullet;
    //群れぷに_バレット（緑）を取得します
    [SerializeField] private GameObject murepuniSlimeObj_GreenBullet;
    //群れぷに_バレット（青）を取得します
    [SerializeField] private GameObject murepuniSlimeObj_BlueBullet;
    //子供オブジェクトを取得
    [SerializeField] private GameObject armObj;
    // 座標設定用変数
    private Vector3 pos;
    private float x;
    private float z;
    //弾の加速度
    private Vector3 shotMov;
    //使用したスライムのポジションを取得
    //使用しなかった時よう
    private Vector3 slimeOriginPos;
    //使用するバレットぷにを格納するオブジェクト
    private GameObject bulletObj;
    //地面にいるかどうか
    private bool isGround;
    //--------クラスの宣言----------------------
    //シーンコントローラーを取得します
    [SerializeField] private SceneController sceneController;
    private Slime_ChildrenController redSlime_ChildrenController;
    private Slime_ChildrenController greenSlime_ChildrenController;
    private Slime_ChildrenController blueSlime_ChildrenController;
    [SerializeField] private SEController seController;

    //--------------数値変数の宣言------------------
    //移動用変数
    [Header("ゲームパッド用移動スピード")]
    [SerializeField] private float gamepadmoveSpeed;
    [Header("キーボード用移動スピード")]
    [SerializeField] private float keypatmoveSpeed;
    // デフォルト値
    const float DefShotPos = 1.0f;
    const float DefShotSpeed = 3.5f;
    // 回転量
    private float rot = 90.0f;
    //アニメーション再生時間
    private float animationPlayTime;
    //アニメーション再生時間
    [SerializeField] private float animationPlayTimeMax;
    //積み上げ時の使用したスライム番号
    private int[] usedSlime = new int[3];
    //積み上げた回数
    private int usedSlimeCount = 0;
    //積み上げ時の最大回数
    private int pileUpCountMax = 3;
    //使用するスライムの番号
    private int slimeNo;

    //--------フラグ変数の宣言------------------
    //動いたかどうかのフラグ
    private bool isMoveFlag;
    //群れぷに積み上げているかフラグ
    private bool isStackUpFlag;
    //ロード状態フラグ
    private bool isMurepuniLoadFlag;
    //熱せられた鉄箱に当たったかどうか
    public bool isHotIronHit
    {
        get;
        set;
    }


    //初期化します
    void Start()
    {
        //自身のRigidbodyを取得
        charaRigidbody = gameObject.GetComponent<Rigidbody>();
        //各ムレスラのクラスを取得
        redSlime_ChildrenController = murepuniSlimeObj_Red.GetComponent<Slime_ChildrenController>();
        greenSlime_ChildrenController = murepuniSlimeObj_Green.GetComponent<Slime_ChildrenController>();
        blueSlime_ChildrenController = murepuniSlimeObj_Blue.GetComponent<Slime_ChildrenController>();
        // アニメーションコンポーネント取得
        animatorComponent = gameObject.GetComponentInChildren<Animator>();
        //移動確認フラグをoffに設定します
        isMoveFlag = false;
        animationPlayTime = 0;
        slimeType = SlimeType.GameStart;
        for (int loop0 = 0; loop0 < pileUpCountMax; loop0++)
            usedSlime[loop0] = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //タイプ別の処理
        switch (slimeType)
        {
            case SlimeType.GameStart:
                break;
            case SlimeType.Stand_ByMode:
                break;
            case SlimeType.Normal:
                {
                    var deltaTime = Time.deltaTime;
                    Move(deltaTime);
                    if (isMoveFlag == true)
                    {
                        //持っていたら離す
                        Separate();
                        animatorComponent.SetBool("HaveFlag", false);
                        animatorComponent.SetBool("WalkFlag", true);
                    }
                    else
                    {
                        animatorComponent.SetBool("WalkFlag", false);
                    }
                    //赤スライム生成（装填）
                    if (Input.GetKeyDown(KeyCode.Joystick1Button1) && isMoveFlag == false || Input.GetKeyDown(KeyCode.Alpha1) && isMoveFlag == false)
                    {
                        Load_Bullet(murepuniSlimeObj_Red, redSlime_ChildrenController, murepuniSlimeObj_RedBullet, 0);
                    }
                    //緑スライム生成（装填）
                    if (Input.GetKeyDown(KeyCode.Joystick1Button0) && isMoveFlag == false || Input.GetKeyDown(KeyCode.Alpha2) && isMoveFlag == false)
                    {
                        Load_Bullet(murepuniSlimeObj_Green, greenSlime_ChildrenController, murepuniSlimeObj_GreenBullet, 1);
                    }
                    //青スライム生成（装填）
                    if (Input.GetKeyDown(KeyCode.Joystick1Button2) && isMoveFlag == false || Input.GetKeyDown(KeyCode.Alpha3) && isMoveFlag == false)
                    {
                        Load_Bullet(murepuniSlimeObj_Blue, blueSlime_ChildrenController, murepuniSlimeObj_BlueBullet, 2);
                    }
                    //足場スライム生成（装填）
                    if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.S))
                    {
                        Scaffold();
                    }
                    float TrigerInput = Input.GetAxis("Triger");
                    if (TrigerInput > 0.0f && isStackUpFlag == true)
                    {
                        isStackUpFlag = false;
                        Scaffold();
                    }else if (TrigerInput == 0)
                    {
                        isStackUpFlag = true;
                    }
                    //熱せられている鉄箱に当たった時
                    if (isHotIronHit)
                    {
                        isHotIronHit = false;
                        slimeType = SlimeType.HotBoxHit;
                    }
                }
                break;
            case SlimeType.HotBoxHit:
                StartCoroutine(DamageIEnumerator());
                break;
            case SlimeType.GameClear:
                gameObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
                animatorComponent.SetBool("GoalFlag", true);
                break;
        }
    }


    //------------------関数の宣言-----------------------
    private IEnumerator DamageIEnumerator()
    {
        animatorComponent.SetBool("DamageFlag", true);
        yield return new WaitForSeconds(1.0f);
        animatorComponent.SetBool("DamageFlag", false);
        slimeType = SlimeType.Normal;
    }
    //*************当たり判定******************
    /// <summary>
    /// キャラクターの当たり判定です
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "IronBox" || collision.gameObject.tag == "WoodBox")
        {
            //足場を作成するときの処理
            //使用制限でプレイヤーが地面に接触した際にカウントがリセットされる
            if (isMoveFlag == true)
            {
                usedSlimeCount = 0;
                //群れスラをサーチ状態にします
                MurepuniSearchStateResume(redSlime_ChildrenController, murepuniSlimeObj_Red);
                MurepuniSearchStateResume(greenSlime_ChildrenController, murepuniSlimeObj_Green);
                MurepuniSearchStateResume(blueSlime_ChildrenController, murepuniSlimeObj_Blue);
            }
        }
    }
    /// <summary>
    /// 群れスラのサーチ状態にします
    /// </summary>
    /// <param name="slime_ChildrenController">使用する群れスラのslime_ChildrenController</param>
    /// <param name="murepuniObj">使用する群れスラのオブジェ</param>
    void MurepuniSearchStateResume(Slime_ChildrenController slime_ChildrenController,GameObject murepuniObj)
    {
        slime_ChildrenController.isSearch = true;
        var slime_rigid = murepuniObj.GetComponent<Rigidbody>();
        slime_rigid.isKinematic = false;
        slime_ChildrenController.isUsed = false;
    }
    /// <summary>
    /// 地面との当たり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        isGround = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }
    //*************移動用関数*********************
    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="deltaTime">地面と接していないときに重力加速度を付けるときに使用します</param>
    void Move(float deltaTime)
    {
        float h = Input.GetAxis("Horizontal");
        //右方向移動
        if (h > 0)
        {
            isMoveFlag = true;
            gameObject.transform.transform.localRotation = Quaternion.Euler(0, rot, 0);
            var trans = gameObject.transform.localPosition;
            trans += new Vector3(h / 10, 0, 0);
            gameObject.transform.localPosition = trans;
        }
        //左方向移動
        else if (h < 0)
        {
            isMoveFlag = true;
            gameObject.transform.localRotation = Quaternion.Euler(0, -rot, 0);
            var trans = gameObject.transform.localPosition;
            trans += new Vector3(h / 10, 0, 0);
            gameObject.transform.localPosition = trans;
        }
        //止まる
        else if (h == 0)
        {
            isMoveFlag = false;
        }
        //地面に接していないとき重力加速度を付けて自然に落ちていくようにする
        if (!isGround)
        {
            var charaVelo = charaRigidbody.velocity;
            charaVelo.y += Physics.gravity.y * deltaTime;
            charaRigidbody.velocity = charaVelo;
        }
    }

    //**************足場作成******************
    /// <summary>
    /// ランダムで使用する群れスラを決めてキャラクターポジション下に移動させます。
    /// 3体積めるが、投てき状態になっていた場合は2体までしか積めない
    /// </summary>
    void Scaffold()
    {
        // キャラクターの向きを確認します
        CheckDirection();
        //3体分足場作成
        if (usedSlimeCount < pileUpCountMax)
        {
            //足場SEを再生します
            seController.playSE(0);
            var scaffold_slime = 0;
            do
            {
                scaffold_slime = Random.Range(0, 3);
            } while (usedSlime[0] == scaffold_slime || usedSlime[1] == scaffold_slime);
            //条件を達成したので代入します
            usedSlime[usedSlimeCount] = scaffold_slime;
            //足場の数をカウントUPさせます
            usedSlimeCount++;
            //各群れスラの足場を作成します
            switch (scaffold_slime)
            {
                case 0: ScaffoldCreat(redSlime_ChildrenController, murepuniSlimeObj_Red);
                    break;
                case 1: ScaffoldCreat(greenSlime_ChildrenController, murepuniSlimeObj_Green);
                    break;
                case 2: ScaffoldCreat(blueSlime_ChildrenController, murepuniSlimeObj_Blue);
                    break;
            }
        }
    }
    /// <summary>
    /// 足場を作成する中身です
    /// </summary>
    /// <param name="slime_ChildrenController">使用する群れスラのslime_ChildrenController</param>
    /// <param name="muresuraObj">使用する群れスラオブジェ</param>
    void ScaffoldCreat(Slime_ChildrenController slime_ChildrenController, GameObject muresuraObj)
    {
        slime_ChildrenController.isUsed = true;
        //足場になるスライムのポジションを獲得
        Vector3 slime_Pos = muresuraObj.transform.localPosition;
        //群れスラをキャラクターポジションに設定します
        slime_Pos = gameObject.transform.position;
        //キャラクターのポジションを足場スライムの上に設定
        gameObject.transform.position += new Vector3(0, 0.5f, 0);
        //スライムを足場ポジションに移動
        muresuraObj.transform.localPosition = slime_Pos;
        //サーチ状態をoffにします
        slime_ChildrenController.isSearch = false;
    }

    /// <summary>
    /// キャラクターの向きを確認します
    /// </summary>
    void CheckDirection()
    {
        // キャラクタ管理のデータ取得
        // 回転量
        Vector3 objRot = gameObject.transform.localEulerAngles;
        // 座標
        Vector3 objPos = gameObject.transform.localPosition;
        // キャラクタの向いている方向ベクトル計算
        x = Mathf.Sin(objRot.y * Mathf.Deg2Rad);
        z = Mathf.Cos(objRot.y * Mathf.Deg2Rad);
    }

    /// <summary>
    /// 各スライムの装填を行います
    /// 使用するスライムを設定します　→　slimeNo
    /// 各群れぷに（赤、青、緑）を非表示にする　→　murepuniSlimeObj_〇〇〇.SetActive(false);
    /// 
    /// 装填されていない状態の時は弾が装填され
    /// 装填されているときは、Throw()関数に遷移する
    /// </summary>

    void Load_Bullet(GameObject murepuniObj, Slime_ChildrenController slime_ChildrenController, GameObject bulletPrefab, int slimeNum)
    {
        //装填していない状態の時
        if (isMurepuniLoadFlag == false)
        {
            //使用するスライム使用可能数が残っているか
            if (slime_ChildrenController.slimeState.bp > 0)
            {
                murepuniObj.SetActive(false);
                slimeNo = slimeNum;
                //打てる状態にする
                isMurepuniLoadFlag = true;
                // 移動するスライムのtransformを取得します
                Vector3 slime_Pos = murepuniObj.transform.localPosition;
                slimeOriginPos = slime_Pos;
                slime_Pos = pos;
                murepuniObj.transform.localPosition = slime_Pos;
                //プレイヤーオブジェクトにアタッチ
                murepuniObj.transform.parent = gameObject.transform;
                //使用する群れぷにのsearchエリアをfalseに設定
                slime_ChildrenController.isSearch = false;
                //使用する群れぷに（赤）のBPを1Downさせます
                slime_ChildrenController.slimeState.bp--;
                //BPが0以下のとき0に設定します
                if (slime_ChildrenController.slimeState.bp <= 0)
                {
                    slime_ChildrenController.slimeState.bp = 0;
                }
                // 実体化
                bulletObj =
                    Instantiate(bulletPrefab, armObj.transform.localPosition, transform.rotation);
                //bulletObjの（非物理演算）をtrueに設定
                bulletObj.GetComponent<Rigidbody>().isKinematic = true;
                //弾スライムの状態を使用していない状態に設定します
                bulletObj.GetComponent<MurepuniBulletController>().isUsedFlag = false;
                //アームオブジェクトにアタッチ
                bulletObj.transform.parent = armObj.transform;
                bulletObj.transform.localPosition = new Vector3(-40, 30, -35);
                //アニメーション
                animatorComponent.SetBool("HaveFlag", true);
            }
            else
            {
                sceneController.StartCoroutine(sceneController.ErrorMessage(0));
            }
        }
        //既に装填されている状態の時
        //投てきを行います
        else if (isMurepuniLoadFlag == true)
        {
            murepuniObj.SetActive(true);
            Throw();
        }
    }


    //************スライム投てき*****************
    /// <summary>
    /// 使用スライムごとに群れスラを1体に加速度を渡して投げます
    /// </summary>
    void Throw()
    {
        //アニメーション再生
        animatorComponent.SetBool("HaveFlag", false);
        animatorComponent.SetTrigger("ThrowFlag");
        //バレット発射（弾発射）
        switch (slimeNo)
        {
            //ロードフラグがONの時の処理
            case 0:
                //赤スライムの時
                PutBulletObjIntoTheState(murepuniSlimeObj_Red, bulletObj);
                break;
            case 1:
                //緑スライムの時
                PutBulletObjIntoTheState(murepuniSlimeObj_Green, bulletObj);
                break;
            case 2:
                //青スライムの時
                PutBulletObjIntoTheState(murepuniSlimeObj_Blue, bulletObj);
                break;
        }
        //ロードフラグをoffにする
        isMurepuniLoadFlag = false;
    }
    /// <summary>
    /// 投てきの中身の処理　→　三種類で同じ処理なので作りました
    /// 弾のFreezeRotationを設定　動かなくする
    /// 弾の加速度を設定して弾を飛ばす
    /// 
    /// 残弾がないときはエラーメッセージが表示されます
    /// </summary>
    /// <param name="murepuniSlime">現在使用しているスライム</param>
    /// <param name="bulletObj">弾オブジェ</param>
    void PutBulletObjIntoTheState(GameObject murepuniSlime, GameObject bulletObj)
    {
        if (isMurepuniLoadFlag == true)
        {
            CheckDirection();
            //キャラクターの向きを判断
            if (x > 0) z = 1;
            if (x < 0) z = -1;
            // ショットの移動ベクトル（加速度）
            var shotMov = new Vector3(x * DefShotSpeed, 3.0f, z);
            //弾を動かなくします
            var rigidbody = bulletObj.GetComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            //弾に加速度を渡します
            bulletObj.GetComponent<Rigidbody>().velocity = shotMov;
            bulletObj.transform.parent = null;
            //物理演算を使用します
            rigidbody.isKinematic = false;
            bulletObj.GetComponent<MurepuniBulletController>().isUsedFlag = true;
            //親子関係を解除する
            murepuniSlime.transform.parent = null;
            //使用した群れぷにを元の座標に戻す
            murepuniSlime.transform.localPosition = slimeOriginPos;
        }
        //ロードフラグがoffの時の処理
        else
        {
            //エラーメッセージを表示
            sceneController.StartCoroutine(sceneController.ErrorMessage(1));
        }
    }

    //**************スライム装填解除***************
    /// <summary>
    /// キャラクターが動いたとき装填を解除します
    /// </summary>
    void Separate()
    {
        if (isMurepuniLoadFlag == true)
        {
            switch (slimeNo)
            {
                case 0: ReleaseThrowing(murepuniSlimeObj_Red, redSlime_ChildrenController);
                    break;
                case 1: ReleaseThrowing(murepuniSlimeObj_Green, greenSlime_ChildrenController);
                    break;
                case 2: ReleaseThrowing(murepuniSlimeObj_Blue, blueSlime_ChildrenController);
                    break;
            }
            isMurepuniLoadFlag = false;
            //生成したbulletObjを消去
            Destroy(bulletObj);
        }
    }
    /// <summary>
    /// 投てき後と投てき途中終了時に呼び出されます
    /// プレイヤーオブジェクトから解放
    /// スライムをもとの位置に戻す
    /// 群れスラの状態を初期状態に戻します
    /// slimeState.bpカウントを1戻す
    /// 群れスラ再表示
    /// </summary>
    void ReleaseThrowing(GameObject nurepuniObj, Slime_ChildrenController slime_ChildrenController)
    {
        nurepuniObj.transform.parent = null;
        nurepuniObj.transform.localPosition = slimeOriginPos;
        slime_ChildrenController.isSearch = true;
        slime_ChildrenController.isUsed = false;
        slime_ChildrenController.slimeState.bp++;
        nurepuniObj.SetActive(true);
    }
}
