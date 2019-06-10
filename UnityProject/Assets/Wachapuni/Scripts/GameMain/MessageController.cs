using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour {

    [SerializeField] GameObject errorMessageUI;
    public GameObject ErrorMessageUI
    {
        set { errorMessageUI = value; }
        get { return errorMessageUI; }
    }
    Text errorText;

    // Use this for initialization
    void Start()
    {
        errorText = errorMessageUI.GetComponentInChildren<Text>();
        errorMessageUI.SetActive(false);
    }

    /// <summary>
    /// エラーメッセージです
    /// エラーナンバーを取得することでエラーメッセージを表示します
    /// </summary>
    /// <param name="errorNo">エラーナンバー</param>
    public void GameError(int errorNo)
    {
        switch (errorNo)
        {
            case 0: errorText.text = "残弾がありません";
                break;
            case 1:
                errorText.text = "スライムを装填してください";
                break;
        }
    } 


}
