using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfiSceneController : MonoBehaviour
{


    //----------------Unityコンポーネント関係----------------
    //2回目のDeleteダイアログ
    [SerializeField] private GameObject ollagainConfiUI;
    //消去時表示するTextUI
    [SerializeField] private GameObject messageUI;
    private Text messageText;

    //1回目の『Yes,No』ボタン
    [SerializeField] private GameObject fastYesButton;
    [SerializeField] private GameObject fastNoButton;
    //2回目の『Yes,No』ボタン
    [SerializeField] private GameObject secondYesButton;
    [SerializeField] private GameObject secondNoButton;

    //----------------クラスの定義---------------------------




    //----------------数値用変数------------------------------
    private int cliarStageNo;

    //初回のボタン選択用
    private int fastButtonNum = 0;
    private int fastLeftRightButtonNumMax = 2;
    private int fastLeftRightNum = 0;

    //2回目のボタン選択用
    private int secondButtonNum = 0;
    private int secondLeftRightButtonNumMax = 2;
    private int secondLeftRightNum = 0;
    //----------------フラグ用の変数--------------------------
    private bool fastSelect;
    private bool secondSelect;

    private bool isDelete = false;

    // Use this for initialization
    void Start()
    {
        ollagainConfiUI.SetActive(false);
        messageUI.SetActive(false);
        messageText = messageUI.GetComponentInChildren<Text>();
        fastSelect = true;
        secondSelect = false;
    }

    void Update()
    {
        //ExitUIの『Yes、No』ボタンを選択するときに使用します
        float h = Input.GetAxis("Horizontal");
        if (!isDelete)
        {
            //『Yes、No』ボタンを選択します
            if (fastSelect)
            {
                //右
                if (h > 0)
                {
                    if (fastLeftRightNum == 0) fastLeftRightNum++;
                    fastButtonNum = fastLeftRightNum;
                    fastSelect = false;
                    FastSelectButton(fastButtonNum);
                }
                //左
                else if (h < 0)
                {
                    if (fastLeftRightNum == 1) fastLeftRightNum--;
                    fastButtonNum = fastLeftRightNum;
                    fastSelect = false;
                    FastSelectButton(fastButtonNum);
                }
            }
            //キー入力させていないときキーをもう一度使えるようにします
            //GetAxisで入力させるので連続処理されないようにboolを使用しています
            if (h == 0)
            {
                fastSelect = true;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (fastButtonNum)
                {
                    case 0://yesButton
                        OnClickOllYesButton();
                        break;
                    case 1://noButton
                        OnClickNoButton();
                        break;
                }
            }
        }
        else
        {
            //『Yes、No』ボタンを選択します
            if (secondSelect)
            {
                //右
                if (h > 0)
                {
                    if (secondLeftRightNum == 0) secondLeftRightNum++;
                    secondButtonNum = secondLeftRightNum;
                    secondSelect = false;
                    SecondSelectButton(secondButtonNum);
                }
                //左
                else if (h < 0)
                {
                    if (secondLeftRightNum == 1) secondLeftRightNum--;
                    secondButtonNum = secondLeftRightNum;
                    secondSelect = false;
                    SecondSelectButton(secondButtonNum);
                }
            }
            //キー入力させていないときキーをもう一度使えるようにします
            //GetAxisで入力させるので連続処理されないようにboolを使用しています
            if (h == 0)
            {
                secondSelect = true;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (secondButtonNum)
                {
                    case 0://yesButton
                        OnClickOllAgainYesButton();
                        break;
                    case 1://noButton
                        OnClickOllAgainNoButton();
                        break;
                }
            }
        }
    }

    //----------------関数の定義-------------------------------
    /// <summary>
    /// １回目のデリートボタンの選択したボタンを選択画像に切り替えます
    /// </summary>
    /// <param name="selectNum">選択したステージ番号</param>
    private void FastSelectButton(int selectNum)
    {
        switch (selectNum)
        {
            case 0://Yesボタン
                fastYesButton.GetComponent<Image>().color = new Color(0, 0 ,0);
                fastNoButton.GetComponent<Image>().color = new Color(255, 255, 255);
                break;
            case 1://Noボタン
                fastYesButton.GetComponent<Image>().color = new Color(255, 255, 255);
                fastNoButton.GetComponent<Image>().color = new Color(0, 0, 0);
                break;
        }
    }
    /// <summary>
    /// １回目のデリートボタンの選択したボタンを選択画像に切り替えます
    /// </summary>
    /// <param name="selectNum">選択したステージ番号</param>
    private void SecondSelectButton(int selectNum)
    {
        switch (selectNum)
        {
            case 0://Yesボタン
                secondYesButton.GetComponent<Image>().color = new Color(0, 0, 0);
                secondNoButton.GetComponent<Image>().color = new Color(255, 255, 255);
                break;
            case 1://Noボタン
                secondYesButton.GetComponent<Image>().color = new Color(255, 255, 255);
                secondNoButton.GetComponent<Image>().color = new Color(0, 0, 0);
                break;
        }
    }
    //*************最初の全てYESボタン**********************
    public void OnClickOllYesButton()
    {
        ollagainConfiUI.SetActive(true);
        isDelete = true;
    }
    //*************最初のNoボタン***********************
    public void OnClickNoButton()
    {
        SceneManager.LoadScene("Title");
    }
    //**************2回目の全てYESボタン********************
    public void OnClickOllAgainYesButton()
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(OnSaveDateDestroy());
        isDelete = false;
    }
    //**************2回目の全てNoボタン**********************
    public void OnClickOllAgainNoButton()
    {
        ollagainConfiUI.SetActive(false);
        isDelete = false;
    }
    /// <summary>
    /// データを消去した際にTextを表示します
    /// </summary>
    IEnumerator OnSaveDateDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        messageText.text = "全てを初期化しました";
        messageUI.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        messageUI.SetActive(false);
        messageText.text = "タイトルに戻ります";
        messageUI.SetActive(true);
        SceneManager.LoadScene("Title");
    }
}
