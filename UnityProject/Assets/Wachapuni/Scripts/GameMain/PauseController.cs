using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    //ポーズボタン、リトライ＆EXIT用のテクスチャーを用意します
    //0 『Retryボタン』通常時
    //1 『Retryボタン』押されたとき
    //2 『Exitボタン』通常時
    //3 『Exitボタン』押されたとき
    public Sprite[] pauseSprite;

    //ポーズボタン、リトライ＆EXIT用のオブジェクトを取得します
    //0 『Retryボタン』
    //1 『Exitボタン』
    [SerializeField] GameObject[] pauseButton;
    //SEControllerを参照します
    [SerializeField] private SEController seController;

    private int selectPauseButtonNum = 0;
    private int selectPauseButtonNumMax = 2;

    /// <summary>
    /// セレクトシーンに遷移します
    /// その際に、ポーズタイムを解除します
    /// </summary>
    public void OnClickExitButton()
    {
        SceneManager.LoadScene("Select");
        Time.timeScale = 1f;
        seController.playSE(8);
    }
    /// <summary>
    /// リトライします
    /// その際に、ポーズタイムを解除します
    /// </summary>
    public void OnClickRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     // リトライ時のステージ名を取得します。
        Time.timeScale = 1f;
        seController.playSE(8);

    }

    void Start () {

    }

    private void Update()
    {
        //↑キー
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectPauseButtonNum--;
            if (selectPauseButtonNum <= 0) selectPauseButtonNum = 0;
            SerectPauseButton(selectPauseButtonNum);
            seController.playSE(8);
        }
        //↓キー
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectPauseButtonNum++;
            if (selectPauseButtonNum > selectPauseButtonNumMax) selectPauseButtonNum = 1;
            SerectPauseButton(selectPauseButtonNum);
            seController.playSE(8);
        }
        //選択したボタンによってリトライorセレクト画面に遷移します
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (selectPauseButtonNum)
            {
                case 0:
                    OnClickRetryButton();
                    break;
                case 1:
                    OnClickExitButton();
                    break;
            }
            seController.playSE(8);
        }
    }

    /// <summary>
    /// ポーズボタンのセレクトを行います
    /// </summary>
    /// <param name="buttonNum">選択したボタン番号を取得します</param>
    private void SerectPauseButton(int buttonNum)
    {
        switch (buttonNum)
        {
            case 0:
                pauseButton[0].GetComponent<Image>().sprite = pauseSprite[1];
                pauseButton[1].GetComponent<Image>().sprite = pauseSprite[2];
                break;
            case 1 :
                pauseButton[0].GetComponent<Image>().sprite = pauseSprite[0];
                pauseButton[1].GetComponent<Image>().sprite = pauseSprite[3];
                break;
        }
    }
}
