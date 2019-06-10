using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{

    //セレクト画面のシーン状態
    public enum TitleSceneType
    {
        None,
        Normal,
        Exit,
    }
    TitleSceneType titleSceneType = TitleSceneType.None;
    //---------------Unityコンポーネント関係-------------------
    [SerializeField] private GameObject eXitUI;

    [SerializeField] private GameObject yesButton;

    [SerializeField] private GameObject noButton;


    //Yesの画像を選択します
    //0　押される前
    //1　押されたとき
    [SerializeField] private Sprite[] yesSprite;

    //Noの画像を選択します
    //0　押される前
    //1　押されたとき
    [SerializeField] private Sprite[] noSprite;

    //Exit用
    //Exit選択ボタン番号
    private int exitButtonNum = 0;
    //Exit選択ボタン最大数
    private int exitLeftRightButtonNumMax = 2;
    //選択するときに使用する変数
    private int exitLeftRightNum = 0;
    //キー入力させていないときキーをもう一度使えるようにします
    //GetAxisで入力させるので連続処理されないようにboolを使用しています
    private bool canExitSelect;



    // Use this for initialization
    void Start()
    {
        eXitUI.SetActive(false);
        titleSceneType = TitleSceneType.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        switch (titleSceneType)
        {
            case TitleSceneType.Normal:
                //セレクト画面遷移
                //キーボード『Enter』キーもしくはゲームパッド『A』ボタン
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    FadeManager.Instance.LoadScene("Select", 2.0f);
                }
                //初期化画面遷移
                //キーボード『A + D』キーもしくはゲームパッド『Start + R』ボタン
                if ((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) ||
                    (Input.GetKey(KeyCode.Joystick1Button7) && Input.GetKey(KeyCode.Joystick1Button5)))
                {
                    SceneManager.LoadScene("Configuration");
                }
                //ExitUI表示
                //キーボード『Esc』キーもしくはゲームパッド『R』ボタン
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
                {
                    eXitUI.SetActive(!eXitUI.activeSelf);
                    titleSceneType = TitleSceneType.Exit;
                }
                break;
            //終了ボタンが押されたときの処理です
            case TitleSceneType.Exit:
                //ExitUIの『Yes、No』ボタンを選択するときに使用します
                float h = Input.GetAxis("Horizontal");
                //『Yes、No』ボタンを選択します
                if (canExitSelect)
                {
                    //右
                    if (h > 0)
                    {
                        if (exitLeftRightNum == 0) exitLeftRightNum++;
                        exitButtonNum = exitLeftRightNum;
                        canExitSelect = false;
                        SelectExitButton(exitButtonNum);
                    }
                    //左
                    else if (h < 0)
                    {
                        if (exitLeftRightNum == 1) exitLeftRightNum--;
                        exitButtonNum = exitLeftRightNum;
                        canExitSelect = false;
                        SelectExitButton(exitButtonNum);
                    }
                }
                //キー入力させていないときキーをもう一度使えるようにします
                //GetAxisで入力させるので連続処理されないようにboolを使用しています
                if (h == 0)
                {
                    canExitSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switch (exitButtonNum)
                    {
                        case 0://yesButton
                            ExitYesButton();
                            break;
                        case 1://noButton
                            ExitNoButton();
                            break;
                    }
                }
                break;
        }
    }


    //------------------関数の定義---------------------------
    /// <summary>
    /// データ初期化画面に遷移
    /// </summary>
    public void OnClickConfiButton()
    {
        SceneManager.LoadScene("Configuration");
    }


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
        titleSceneType = TitleSceneType.Normal;
    }
    /// <summary>
    /// Exitボタンで選択した画像を選択画像に切り替えます
    /// </summary>
    /// <param name="selectNum">選択したステージ番号</param>
    private void SelectExitButton(int selectNum)
    {
        switch (selectNum)
        {
            case 0://Yesボタン
                yesButton.GetComponent<Image>().sprite = yesSprite[1];
                noButton.GetComponent<Image>().sprite = noSprite[0];
                break;
            case 1://Noボタン
                yesButton.GetComponent<Image>().sprite = yesSprite[0];
                noButton.GetComponent<Image>().sprite = noSprite[1];
                break;
        }
    }

}
