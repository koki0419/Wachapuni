using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //-------------Unityコンポーネント関係-------------------
    // 「Pause」UIを指定します。
    [SerializeField] private GameObject pauseUI;

    [SerializeField] private SceneController sceneController;

    [SerializeField] private GameObject messageUI;

    [SerializeField]
    private SEController seController;


    //プレイヤーのアナウンスUIを取得します
    //0 看板
    //1 ボタン
    public GameObject[] playerUI;


    //アナウンス用の看板テクスチャ
    //0 赤
    //1 緑
    //2 青
    [SerializeField] private Sprite[] kanbanSprite;

    //アナウンス用のボタンテクスチャ
    //0 赤
    //1 緑
    //2 青
    [SerializeField] private Sprite[] buttonSprite;

    // Use this for initialization
    void Start()
    {
        //各UIオブジェクト表示を非表示にします
        pauseUI.SetActive(false);
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            seController.playSE(8);
            OnClickPauseButton();
        }

        //プレイヤーUIをカメラと同じ向きに設定
        var playerUIrotate0 = playerUI[0].transform.rotation;
        playerUIrotate0 = Camera.main.transform.rotation;
        playerUI[0].transform.rotation = playerUIrotate0;

        var playerUIrotate1 = playerUI[1].transform.rotation;
        playerUIrotate1 = Camera.main.transform.rotation;
        playerUI[1].transform.rotation = playerUIrotate1;

        //ボタンテクスチャーを半ば強引に設定します
        if (playerUI[1].transform.localRotation.y == 1)
        {
            var playerUIpos1 = playerUI[1].transform.localPosition;
            playerUIpos1 = new Vector3(-0.53f, 0.64f, 0);
            playerUI[1].transform.localPosition = playerUIpos1;
        }
        else
        {
            var playerUIpos1 = playerUI[1].transform.localPosition;
            playerUIpos1 = new Vector3(0.53f, 0.64f, 0);
            playerUI[1].transform.localPosition = playerUIpos1;
        }

    }


    //---------------関数の定義----------------------
    /// <summary>
    /// ポーズボタン
    /// ポーズタイムをOnにします
    /// </summary>
    public void OnClickPauseButton()
    {
        // ポーズUIのアクティブ、非アクティブを切替
        pauseUI.SetActive(!pauseUI.activeSelf);
        // ポーズUIが表示されているときは停止
        if (pauseUI.activeSelf)
        {
            Time.timeScale = 0f;
            // ポーズUIが表示されていなければ通常通り進行
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    //************プレイヤーUI表示****************
    public void PlayerUIDysplay(int tutorialNum)
    {
        playerUI[0].GetComponent<Image>().sprite = kanbanSprite[tutorialNum];
        if (tutorialNum < 5)
        {
            playerUI[1].GetComponent<Image>().sprite = buttonSprite[tutorialNum];
        }
    }
}
