using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//各ステージの情報


public class SelectSceneController : MonoBehaviour
{

    //セレクト画面のシーン状態
    public enum SelectSceneType
    {
        None,
        Normal,
        ReleaseStage,
        Exit,
    }
    SelectSceneType selectSceneType = SelectSceneType.None;


    //-----------------Unity コンポーネント関係------------------------
    //EXIT用のダイアログを表示します
    [SerializeField] private GameObject eXitUI;
    //
    [SerializeField] private GameObject exitButton;

    [SerializeField] private GameObject yesButton;

    [SerializeField] private GameObject noButton;

    [SerializeField] private GameObject titleButton;

    [SerializeField] private GameObject[] stageButton;
    //背景オブジェクト
    [SerializeField] private GameObject selectBG;
    //ステージの背景画像
    //0 ステージ1解放時
    //1 ステージ2解放時
    //2 ステージ3解放時
    //3 ステージ4解放時
    //4 ステージ5解放時
    [SerializeField] private Sprite[] stageBGSprite;
    //各ステージの画像を選択します
    //偶数　押される前
    //奇数　押されたとき
    //0,1  チュートリアル
    //2,3  ステージ1
    //4,5  ステージ2
    //6,7  ステージ3
    //7,8  ステージ4
    //9,10 ステージ5
    [SerializeField] private Sprite[] StageSprite;
    //EXITの画像を選択します
    //0　押される前
    //1　押されたとき
    [SerializeField] private Sprite[] exitSprite;
    //Yesの画像を選択します
    //0　押される前
    //1　押されたとき
    [SerializeField] private Sprite[] yesSprite;
    //Noの画像を選択します
    //0　押される前
    //1　押されたとき
    [SerializeField] private Sprite[] noSprite;
    //Titleの画像を選択します
    //0　押される前
    //1　押されたとき
    [SerializeField] private Sprite[] titleSprite;
    //光るUI
    [SerializeField] private GameObject luminescenceUI;
    //光るImageを取得
    private Image luminescenceImage;
    // 自分のアニメーションコンポーネント
    [SerializeField] private Animator animatorComponent;
    // スライムのアニメーションコンポーネント
    [SerializeField] private Animator slimeAnimatorComponent;
    //ステージが解放したときのメッセージUI
    [SerializeField] private GameObject releaseMessageUI;
    //ランクUIオブジェクト
    [SerializeField] private GameObject runkUI;
    //ランク画像
    //0 ランクA
    //1 ランクB
    //2 ランクC
    //3 Clearしていない
    [SerializeField] private Sprite[] runkSprite;
    //スタートボタン
    [SerializeField] private GameObject startButton;
    //ステージ番号UI
    [SerializeField] private GameObject stageNoUI;
    //選択したステージの番号を取得します
    [SerializeField] private Sprite[] stageNumSprite;
    //------------------クラスの定義---------------------------
    [SerializeField] private SEController seController;



    //-------------------数値用変数----------------------------

    //クリアステージ数
    private int stageNum = 0;
    //スタートSEを再生します
    [SerializeField] int buttonSENo;
    private float stageClearTime;
    //ステージスタートNo
    private int startStageNo;
    private int cliarStageNo;

    //一つ前にいたところ
    private int previousStageNo;

    //セレクトジョイスティック時間
    [SerializeField]　private float joystickTime;
    private float joystickTimeMax = 0.1f;

    //ステージセレクト用
    private int[,] buttonNum;
    private int leftrightButtonNumMax = 3;
    private int updownNumButtonNumMax = 2;
    static public int updownNum = 0;
    static public int leftrightNum = 0;

    //Exit用
    //Exit選択ボタン番号
    private int[,] exitButtonNum;
    //Exit選択ボタン横の数最大数
    private int exitLeftRightButtonNumMax = 2;
    //Exit選択ボタン縦の数最大数
    private int exitUpDownButtonNumMax = 2;
    private int exitUpDownNum = 0;
    private int exitLeftRightNum = 0;
    //--------------------フラグ用の変数----------------------------
    //光るかどうか
    private bool isLuminescenceFlag;
    //scoreデータの表示用
    private bool scoreDelete;
    //ステージをスタートします
    private bool oNStartFrag;
    //ステージ選択の連続処理を起こさないようにするフラグ
    private bool oNSelectFrag;
    //ステージ選択のアニメーションをOnにするフラグ
    private bool isAnimation;
    //終了ボタンを押したかどうか
    private bool canExitSelect;


    void Start()
    {
        //ボタン配列の初期化
        buttonNum = new int[updownNumButtonNumMax, leftrightButtonNumMax];
        buttonNum[0, 0] = 1;
        buttonNum[0, 1] = 2;
        buttonNum[0, 2] = 3;
        buttonNum[1, 0] = -1;
        buttonNum[1, 1] = 5;
        buttonNum[1, 2] = 4;

        //セレクトボタン配列の初期化
        exitButtonNum = new int[exitUpDownButtonNumMax, exitLeftRightButtonNumMax];
        exitButtonNum[0, 0] = 0;
        exitButtonNum[0, 1] = 1;
        exitButtonNum[1, 0] = 2;
        exitButtonNum[1, 1] = -1;

        // クリアステージ数
        if (PlayerPrefs.HasKey("StageCliarNum"))
        {
            cliarStageNo = PlayerPrefs.GetInt("StageCliarNum");
        }
        else
        {
            cliarStageNo = 0;
        }

        Debug.Log("cliarStageNo" + cliarStageNo);
        Debug.Log("stageNo" + GameSystem.stageNo);
        //解放ステージの確認をします
        if (GameSystem.stageNo > stageNum)
        {
            PlayerPrefs.SetInt("StageCliarNum", GameSystem.stageNo);
            PlayerPrefs.Save();
            selectSceneType = SelectSceneType.ReleaseStage;
        }
        else
        {
            selectSceneType = SelectSceneType.Normal;
            releaseMessageUI.SetActive(false);
        }

        stageNum = cliarStageNo;

        StartCoroutine(OnStageStart(stageNum));

        //『EXIT』ボタンの通常状態の画像を設定します
        exitButton.GetComponent<Image>().sprite = exitSprite[0];

        //ステージの背景を選択します
        //1ステージもクリアしていなかった場合はstageBGSprite[0]を設定します
        if (stageNum != 0)
        {
            //クリアステージが5以下ならそのまま設定
            //5以上ならstageBGSprite[4]を設定します
            if (stageNum < 5)
            {
                selectBG.GetComponent<Image>().sprite = stageBGSprite[stageNum];
            }
            else
            {
                selectBG.GetComponent<Image>().sprite = stageBGSprite[4];
            }
        }
        else if (stageNum == 0)
        {
            selectBG.GetComponent<Image>().sprite = stageBGSprite[0];
        }

        //ボタンの状態を確認します
        //クリアステージ分のみボタン押せる状態に設定します
        for (int loop0 = 0; loop0 < stageButton.Length; loop0++)
        {
            if (loop0 <= (stageNum + 1))
            {
                stageButton[loop0].GetComponent<ButtonController>().releaseStageFlag = true;
            }
        }
        //雲のアニメーションを設定します
        if (stageNum >= 3 && stageNum < 5)
        {
            animatorComponent.SetBool("Release01", true);
        }
        else if (stageNum >= 5)
        {
            animatorComponent.SetBool("Release01", true);
            animatorComponent.SetBool("Release02", true);
        }

        oNStartFrag = true;
        eXitUI.SetActive(false);
        //初期状態ステージ１のデータを表示
        startStageNo = 1;
        DysplaySaveDate(startStageNo);
    }



    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        switch (selectSceneType)
        {
            case SelectSceneType.Normal:
                //キーボード『Enter』キー + 選択したステージが1以上の時もしくは
                //ゲームパッド『A』ボタン + 選択したステージが1以上の時
                //各選択したステージに遷移します
                if (Input.GetKeyDown(KeyCode.Return) && buttonNum[updownNum, leftrightNum] != 0 ||
                    Input.GetKeyDown(KeyCode.Joystick1Button0) && buttonNum[updownNum, leftrightNum] != 0)
                {
                    if (oNStartFrag)
                    {
                        oNStartFrag = false;
                        GameStart();
                    }
                }
                //ExitUIを表示します
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    eXitUI.SetActive(!eXitUI.activeSelf);
                    //ボタンSEを再生します
                    seController.playSE(0);
                    selectSceneType = SelectSceneType.Exit;
                }

                //挑戦するステージを選択します
                if (oNSelectFrag && !isAnimation)
                {
                    //右
                    if (h > 0)
                    {
                        if (leftrightNum < (leftrightButtonNumMax - 1) && buttonNum[updownNum, leftrightNum] < (stageNum+1)) leftrightNum++;
                        oNSelectFrag = false;
                        isAnimation = true;
                        seController.playSE(0);
                        DysplaySaveDate(buttonNum[updownNum, leftrightNum]);
                        previousStageNo = buttonNum[updownNum, leftrightNum];
                    }
                    //左
                    else if (h < 0)
                    {
                        if (updownNum == 0 && leftrightNum > 0) leftrightNum--;
                        else if (updownNum == 1 && leftrightNum > 1 && buttonNum[updownNum, leftrightNum] < (stageNum + 1)) leftrightNum--;
                        oNSelectFrag = false;
                        isAnimation = true;
                        seController.playSE(0);
                        DysplaySaveDate(buttonNum[updownNum, leftrightNum]);
                        previousStageNo = buttonNum[updownNum, leftrightNum];
                    }
                    //上
                    if (v > 0)
                    {
                        if (buttonNum[updownNum, leftrightNum] == 4)
                        {
                            if (updownNum > 0) updownNum--;
                            oNSelectFrag = false;
                            isAnimation = true;
                            seController.playSE(0);
                            DysplaySaveDate(buttonNum[updownNum, leftrightNum]);
                            previousStageNo = buttonNum[updownNum, leftrightNum];
                        }
                    }
                    //下
                    else if (v < 0)
                    {
                        if (buttonNum[updownNum, leftrightNum] == 3)
                        {
                            if (updownNum < (updownNumButtonNumMax - 1) && buttonNum[updownNum, leftrightNum] < (stageNum+1)) updownNum++;
                            oNSelectFrag = false;
                            isAnimation = true;
                            seController.playSE(0);
                            DysplaySaveDate(buttonNum[updownNum, leftrightNum]);
                            previousStageNo = buttonNum[updownNum, leftrightNum];
                        }
                    }
                }
                //キー入力させていないときキーをもう一度使えるようにします
                //GetAxisで入力させるので連続処理されないようにboolを使用しています
                if (h == 0 && v == 0)
                {
                    oNSelectFrag = true;
                }
                break;
                //ステージスタート時に一回だけここを実行します
            case SelectSceneType.ReleaseStage:
                StartCoroutine(OnReleaseStage());
                break;
            //終了ボタンが押されたときの処理です
            case SelectSceneType.Exit:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    // 終了のアクティブ、非アクティブを切替
                    eXitUI.SetActive(!eXitUI.activeSelf);
                    //ボタンSEを再生します
                    seController.playSE(0);
                }
                //終了ボタンが押されたときの処理です
                if (canExitSelect)
                {
                    //右
                    if (h > 0)
                    {
                        if (exitUpDownNum == 0 && exitLeftRightNum < (exitLeftRightButtonNumMax - 1)) exitLeftRightNum++;
                        canExitSelect = false;
                        SelectExitButton(exitButtonNum[exitUpDownNum, exitLeftRightNum]);
                    }
                    //左
                    else if (h < 0)
                    {
                        if (exitLeftRightNum > 0) exitLeftRightNum--;
                        canExitSelect = false;
                        SelectExitButton(exitButtonNum[exitUpDownNum, exitLeftRightNum]);
                    }
                    //上
                    if (v > 0)
                    {
                        if (exitUpDownNum == 1) exitUpDownNum--;
                        canExitSelect = false;
                        SelectExitButton(exitButtonNum[exitUpDownNum, exitLeftRightNum]);
                    }

                    else if (v < 0)
                    {
                        if (exitUpDownNum == 0)
                        {
                            exitUpDownNum++;
                            exitLeftRightNum = 0;
                        }
                        canExitSelect = false;
                        SelectExitButton(exitButtonNum[exitUpDownNum, exitLeftRightNum]);

                    }
                }
                //キー入力させていないときキーをもう一度使えるようにします
                //GetAxisで入力させるので連続処理されないようにboolを使用しています
                if (h == 0 && v == 0)
                {
                    canExitSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switch (exitButtonNum[exitUpDownNum, exitLeftRightNum])
                    {
                        case 0://yesButton
                            ExitYesButton();
                            break;
                        case 1://noButton
                            ExitNoButton();
                            break;
                        case 2:
                            OnTitleButton();
                            break;
                    }
                }
                break;
        }
    }


    //----------------関数の定義---------------------
    //***************ExitボタンのYESボタン用*******************
    public void ExitYesButton()
    {
        yesButton.GetComponent<Image>().sprite = yesSprite[0];
        Application.Quit();
    }
    //***************ExitボタンのNoボタン用*********************
    public void ExitNoButton()
    {
        eXitUI.SetActive(false);
        noButton.GetComponent<Image>().sprite = noSprite[0];
        selectSceneType = SelectSceneType.Normal;
        seController.playSE(0);
    }
    //***************Exitボタン**********************************
    /// <summary>
    /// Exitボタンで選択した画像を選択画像に切り替えます
    /// </summary>
    /// <param name="selectNum">選択したステージ番号</param>
    private void SelectExitButton(int selectNum)
    {
        seController.playSE(0);
        switch (selectNum)
        {
            case 0://Yesボタン
                yesButton.GetComponent<Image>().sprite = yesSprite[1];
                noButton.GetComponent<Image>().sprite = noSprite[0];
                titleButton.GetComponent<Image>().sprite = titleSprite[0];
                break;
            case 1://Noボタン
                yesButton.GetComponent<Image>().sprite = yesSprite[0];
                noButton.GetComponent<Image>().sprite = noSprite[1];
                titleButton.GetComponent<Image>().sprite = titleSprite[0];
                break;
            case 2://タイトルボタン
                yesButton.GetComponent<Image>().sprite = yesSprite[0];
                noButton.GetComponent<Image>().sprite = noSprite[0];
                titleButton.GetComponent<Image>().sprite = titleSprite[1];
                break;
        }
    }
    //***************ステージボタンを押したとき*************
    IEnumerator OnSelectStart(int stageNo)
    {
        yield return new WaitForSeconds(0.5f);
        FadeManager.Instance.LoadScene(string.Format("Main_0{0}", stageNo), 2.0f);
    }

    //***************ステージが解放されたとき****************
    IEnumerator OnReleaseStage()
    {
        yield return new WaitForSeconds(0.01f);
        if (GameSystem.stageNo == 3)
        {
            animatorComponent.SetBool("Release01", true);
        }
        else if (GameSystem.stageNo == 6)
        {
            animatorComponent.SetBool("Release01", true);
            animatorComponent.SetBool("Release02", true);
        }
        yield return new WaitForSeconds(2.0f);
        selectSceneType = SelectSceneType.Normal;

    }
    //************光る**********************
    /// <summary>
    /// 新たなステージが解放されたときに「ピカーン！」と画面を光らせます
    /// </summary>
    public void OnRelease()
    {
        if (isLuminescenceFlag == true)
        {
            isLuminescenceFlag = false;
            this.luminescenceImage.color = new Color(255.0f, 255.0f, 255.0f, 2.0f);
        }
        else
        {
            this.luminescenceImage.color = Color.Lerp(this.luminescenceImage.color, Color.clear, Time.deltaTime);
        }
    }
    //*************スタートボタン*******************
    public void GameStart()
    {
        if (stageButton[startStageNo - 1].GetComponent<ButtonController>().releaseStageFlag == true)
        {
            GameSystem.stageNo = startStageNo;
            StartCoroutine(OnSelectStart(startStageNo));
            seController.playSE(1);
        }
    }
    //************過去のデータを参照します******************
    /// <summary>
    /// 選択したステージのデータの参照と表示とSlimeAnimationの再生を行います
    /// </summary>
    /// <param name="saveNo">選択したステージ番号</param>
    public void DysplaySaveDate(int saveNo)
    {
        stageNoUI.GetComponent<Image>().sprite = stageNumSprite[saveNo - 1];
        startStageNo = saveNo;

        if (PlayerPrefs.HasKey(string.Format("Stage{0}Hiscore", saveNo)))
        {
            stageClearTime = PlayerPrefs.GetFloat(string.Format("Stage{0}Hiscore", saveNo));
        }
        else
        {
            stageClearTime = 0;
        }

        StartCoroutine(OnSlimeAnimation(saveNo));
        DysplayResultSprite(stageClearTime);
    }
    //************評価画像を表示します*******************
    /// <summary>
    /// クリアタイムによってランクUIを表示します
    /// </summary>
    /// <param name="score">クリアタイム</param>
    public void DysplayResultSprite(float score)
    {
        if (0 >= score)
        {
            runkUI.GetComponent<Image>().sprite = runkSprite[3];
        }
        else if (50 > score)
        {
            //Aランク
            runkUI.GetComponent<Image>().sprite = runkSprite[0];
        }
        else if (100.0f > score)
        {
            //Bランク
            runkUI.GetComponent<Image>().sprite = runkSprite[1];
        }
        else if (100.0f < score)
        {
            //Cランク
            runkUI.GetComponent<Image>().sprite = runkSprite[2];
        }
    }
    //***************スライムのアニメーションとき****************
    /// <summary>
    /// ステージセレクト時のミニスライムのアニメーション
    /// 各ステージボタン位置まで移動したのち待機アニメーションに切替ます
    /// 短時間で複数のアニメーションを行うためコルーチンを使用しています
    /// </summary>
    /// <param name="stageNo">ステージ番号</param>
    /// <returns></returns>
    IEnumerator OnSlimeAnimation(int stageNo)
    {
        slimeAnimatorComponent.SetInteger("SlimeUI_MoveNum", stageNo);
        yield return new WaitForSeconds(0.5f);
        slimeAnimatorComponent.SetInteger("SlimeUI_MoveNum", 0);
        slimeAnimatorComponent.SetTrigger("SlimeUI_Stand");
        yield return new WaitForSeconds(2.5f);
        isAnimation = false;
    }
    //****************リリースUIを閉じるボタン*******************
    /// <summary>
    /// 新たなステージが出現した際に出てくるUIの非表示ボタン
    /// </summary>
    public void OnClickReleaseCloseButton()
    {
        seController.playSE(0);
        releaseMessageUI.SetActive(false);
        StartCoroutine(OnSlimeAnimation(0));
    }
    //****************タイトルに戻ります****************************
    public void OnTitleButton()
    {
        seController.playSE(0);
        SceneManager.LoadScene("Title");
    }
    //スタート演出
    /// <summary>
    /// セレクトステージ開始時に雲のアニメーションを再生させます
    /// ステージ1～3がクリアしている場合と
    /// ステージ4～5がクリアしている場合で再生が異なります
    /// </summary>
    /// <param name="stage">クリアステージ数</param>
    /// <returns></returns>
    IEnumerator OnStageStart(int stage)
    {
        if (stage >= 3)
        {
            animatorComponent.SetBool("Release01", true);
        }
        else if (stage >= 6)
        {
            animatorComponent.SetBool("Release01", true);
            animatorComponent.SetBool("Release02", true);
        }
        yield return new WaitForSeconds(0.5f);
    }
}
