using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill : MonoBehaviour
{

    //ツタオブジェクトを取得します
    [SerializeField] private GameObject ivyObj;

    //ツタを起動させたか判断します
    [SerializeField] private bool isUsed;
    public bool IsUsed
    {
        set { isUsed = value; }
    }

    //カメラコントローラーを取得します
    [SerializeField] private GameObject panel;

    [SerializeField] private SceneController sceneController;

    // Use this for initialization
    void Start()
    {
        if (isUsed == true)
        {
            ivyObj.SetActive(true);
            panel.SetActive(false);
            GetComponent<BoxCollider>().isTrigger = true;
        }
        else
        {
            panel.SetActive(true);
            ivyObj.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        //緑色のスライムが盛土に入った判定
        //盛土に緑スライムが当たったらツタを生やす
        //その他の群れプニの場合はその群れプニを消します
        if (collision.tag == "Children_Green")
        {
            if (isUsed == false)
            {
                isUsed = true;
                sceneController.StandbyModeFlag = true;
                ivyObj.SetActive(true);
                panel.SetActive(false);
                //ツタが無いとき通れないようにトリガーを消しておいて
                //ツタが生えているときは通れるようにトリガーをONにする
                GetComponent<BoxCollider>().isTrigger = true;
            }
        }
        else if (collision.tag == "Children_Red" || collision.tag == "Children_Blue")
        {
            Destroy(collision);
        }
    }
}
