using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    //-------------Unityコンポーネント---------------------
    //『シーン内メッセージ』表示用のUIを指定します
    [SerializeField] private GameObject messageUIBuckGround;
    [SerializeField] private GameObject messageUI;
    //リザルトUI
    [SerializeField] private GameObject gameClearmessageUI;
    //リザルトUI
    [SerializeField] private GameObject resultUINext;
    //リザルトスコアImage
    [SerializeField] private Image gameClearScoreImage;
    //リザルト評価用画像
    [SerializeField] private Sprite[] gameClearScoreSprite;
    //スタート時、ゴール時のテクスチャーです
    //0 1
    //1 2
    //2 3
    //3 スタートテクスチャー
    //4 ゴールテクスチャー
    [SerializeField] private Sprite[] rogoSprite;
    [SerializeField] private GameObject goalTutorialArea;
    [SerializeField] private GameObject playerCanpasUI;

    //-------------クラスの宣言-------------------------
    //MessageControllerを取得します
    [SerializeField] private MessageController messageController;
    //PlayerControllerを取得します
    [SerializeField] private PlayerController playerController;
    //uIManagerを取得します
    [SerializeField] private UIManager uIManager;
    [SerializeField] private SEController seController;

    //--------------数値用変数定義----------------------
    //このシーンが開始されてからの時間経過
    [SerializeField] private float gameTime = 0;
    public float GameTime
    {
        get { return gameTime; }
    }
    //クリアタイム
    private float cleatime;
    //今回のクリアタイム
    private float nowcleatime;
    //クリアしたステージ
    private int cliarStageNo;
    private int resultCount;

    //---------------フラグ用変数定義-------------------

    //プレイヤーのシーンを切り替えるフラグです
    [SerializeField] private bool standbyModeFlag = false;
    public bool StandbyModeFlag
    {
        set { standbyModeFlag = value; }
    }

    static public bool isPlaying;
    public bool stageCliarFlag;
    //クリア後遷移できるか判断
    public bool transitionFlag
    {
        get;private set;
    }


    // Use this for initialization
    void Start()
    {
        //『シーン開始演出』
        StartCoroutine(OnStartStage());
        gameClearmessageUI.SetActive(false);
        resultUINext.SetActive(false);
        isPlaying = false;
        stageCliarFlag = false;
        transitionFlag = false;
        resultCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //待機状態の時
        if (standbyModeFlag == true)
        {
            standbyModeFlag = false;
            StartCoroutine(Stand_ByMode());
        }

        //シーン時間を進める
        //deltaTimeは前回のフレームから今回のフレームまでの差分時間
        if (isPlaying == true)
        {
            gameTime += Time.deltaTime;

            if (stageCliarFlag == true)
            {
                isPlaying = false;
                nowcleatime = gameTime;
                //　クリアタイムを記憶する
                if (PlayerPrefs.HasKey(string.Format("Stage{0}Hiscore", GameSystem.stageNo)))
                {
                    cleatime = PlayerPrefs.GetFloat(string.Format("Stage{0}Hiscore", GameSystem.stageNo));
                }
                if (nowcleatime < cleatime)
                {
                    cleatime = nowcleatime;//タイムを判断する
                }
                if (cleatime == 0) cleatime = nowcleatime;

                PlayerPrefs.SetFloat(string.Format("Stage{0}Hiscore", GameSystem.stageNo), cleatime);
                PlayerPrefs.Save();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (transitionFlag == true)
            {
                resultUINext.SetActive(true);
                resultCount++;
                if (resultCount == 2)
                {
                    FadeManager.Instance.LoadScene("Select", 1.0f);
                }
            }
        }
    }




    //----------------関数の定義-------------------------
    //エラー時の『表示演出』を処理します
    public IEnumerator ErrorMessage(int errorNo)
    {
        messageController.GameError(errorNo);
        messageController.ErrorMessageUI.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        messageController.ErrorMessageUI.SetActive(false);
    }

    //待機状態時のコルーチン
    private IEnumerator Stand_ByMode()
    {
        playerController.slimeType = PlayerController.SlimeType.Stand_ByMode;
        seController.playSE(4);
        yield return new WaitForSeconds(2.0f);
        seController.StopSE();
        playerController.slimeType = PlayerController.SlimeType.Normal;

    }

    //スタートコルーチン『シーン開始演出』を処理します
    public IEnumerator OnStartStage()
    {
        messageUI.SetActive(false);
        messageUIBuckGround.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        messageUI.GetComponent<Image>().sprite = rogoSprite[2];
        messageUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        messageUI.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        messageUI.GetComponent<Image>().sprite = rogoSprite[1];
        messageUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        messageUI.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        messageUI.GetComponent<Image>().sprite = rogoSprite[0];
        messageUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        messageUI.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        messageUI.GetComponent<Image>().sprite = rogoSprite[3];
        messageUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        messageUI.SetActive(false);
        messageUIBuckGround.SetActive(false);
        //プレイヤーの走行開始
        playerController.AnimatorComponent.SetBool("GameStartFlag", true);
        playerController.slimeType = PlayerController.SlimeType.Normal;

        isPlaying = true;
    }


    //ゲームクリア時
    public IEnumerator StageCliar()
    {
        if (goalTutorialArea != null)
        {
            goalTutorialArea.SetActive(false);
        }
        if (playerCanpasUI != null)
        {
            playerCanpasUI.SetActive(false);
        }
        playerController.slimeType = PlayerController.SlimeType.GameClear;
        messageUI.GetComponent<Image>().sprite = rogoSprite[4];
        messageUI.transform.parent = GameObject.Find("UI").transform;
        messageUI.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        seController.StopSE();
        seController.playSE(7);
        yield return new WaitForSeconds(1.0f);
        messageUI.SetActive(false);
        yield return new WaitForSeconds(12);

        if (50 > nowcleatime)
        {
            //Aランク
            gameClearScoreImage.sprite = gameClearScoreSprite[0];
        }
        else if (100.0f > nowcleatime)
        {
            //Bランク
            gameClearScoreImage.sprite = gameClearScoreSprite[1];
        }
        else if (100.0f < nowcleatime)
        {
            //Cランク
            gameClearScoreImage.sprite = gameClearScoreSprite[2];
        }

        gameClearmessageUI.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        transitionFlag = true;
    }
}
