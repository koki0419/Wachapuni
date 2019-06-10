using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    //------Unityコンポーネント宣言--------------
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SceneController sceneController;

    //赤数字のテクスチャー
    //0～9を取得します
    [SerializeField] private Sprite[] redNumSprites;
    //緑数字のテクスチャー
    //0～9を取得します
    [SerializeField] private Sprite[] greenNumSprites;
    //青数字のテクスチャー
    //0～9を取得します
    [SerializeField] private Sprite[] blueNumSprites;
    //赤数字表示用オブジェクト
    [SerializeField] private GameObject[] redScoreObj;
    //緑数字表示用オブジェクト
    [SerializeField] private GameObject[] greenScoreObj;
    //青数字表示用オブジェクト
    [SerializeField] private GameObject[] blueScoreObj;
    //クリア時表示用UI
    [SerializeField] private GameObject[] clearTimeUI;



    //スコアをテクスチャーに置き換えます
    void ScoreDisplay(int redScore, int greenScore, int blueScore)
    {
        //初期化
        //赤数字表示用オブジェクトを非表示にします
        for (int i = 0; i < redScoreObj.Length; i++)
        {
            redScoreObj[i].SetActive(false);
        }
        //緑数字表示用オブジェクトを非表示にします
        for (int i1 = 0; i1 < greenScoreObj.Length; i1++)
        {
            greenScoreObj[i1].SetActive(false);
        }
        //青数字表示用オブジェクトを非表示にします
        for (int i2 = 0; i2 < blueScoreObj.Length; i2++)
        {
            blueScoreObj[i2].SetActive(false);
        }

        //（あか）
        if (redScore < 10)
        {
            //1/1
            redScoreObj[0].GetComponent<Image>().sprite = redNumSprites[redScore % 10];
            redScoreObj[0].SetActive(true);
        }
        else if (redScore < 100)
        {
            //1/1
            redScoreObj[0].GetComponent<Image>().sprite = redNumSprites[redScore % 10];
            //1/10
            redScoreObj[1].GetComponent<Image>().sprite = redNumSprites[(redScore / 10) % 10];
            redScoreObj[0].SetActive(true);
            redScoreObj[1].SetActive(true);
        }
        else if (redScore < 1000)
        {
            //1/1
            redScoreObj[0].GetComponent<Image>().sprite = redNumSprites[redScore % 10];
            //1/10
            redScoreObj[1].GetComponent<Image>().sprite = redNumSprites[(redScore / 10) % 10];
            //1/100
            redScoreObj[2].GetComponent<Image>().sprite = redNumSprites[(redScore / 100) % 10];
            redScoreObj[0].SetActive(true);
            redScoreObj[1].SetActive(true);
            redScoreObj[2].SetActive(true);
        }
        else if (redScore > 1000)
        {
            //1/1
            redScoreObj[0].GetComponent<Image>().sprite = redNumSprites[redScore % 10];
            //1/10
            redScoreObj[1].GetComponent<Image>().sprite = redNumSprites[(redScore / 10) % 10];
            //1/100
            redScoreObj[2].GetComponent<Image>().sprite = redNumSprites[(redScore / 100) % 10];
            //1/1000
            redScoreObj[3].GetComponent<Image>().sprite = redNumSprites[(redScore / 1000) % 10];

            redScoreObj[0].SetActive(true);
            redScoreObj[1].SetActive(true);
            redScoreObj[2].SetActive(true);
            redScoreObj[3].SetActive(true);
        }

        //（みどり）
        if (greenScore < 10)
        {
            //1/1
            greenScoreObj[0].GetComponent<Image>().sprite = greenNumSprites[greenScore % 10];
            greenScoreObj[0].SetActive(true);
        }
        else if (greenScore < 100)
        {
            //1/1
            greenScoreObj[0].GetComponent<Image>().sprite = greenNumSprites[greenScore % 10];
            //1/10
            greenScoreObj[1].GetComponent<Image>().sprite = greenNumSprites[(greenScore / 10) % 10];
            greenScoreObj[0].SetActive(true);
            greenScoreObj[1].SetActive(true);
        }
        else if (greenScore < 1000)
        {
            //1/1
            greenScoreObj[0].GetComponent<Image>().sprite = greenNumSprites[greenScore % 10];
            //1/10
            greenScoreObj[1].GetComponent<Image>().sprite = greenNumSprites[(greenScore / 10) % 10];
            //1/100
            greenScoreObj[2].GetComponent<Image>().sprite = greenNumSprites[(greenScore / 100) % 10];
            greenScoreObj[0].SetActive(true);
            greenScoreObj[1].SetActive(true);
            greenScoreObj[2].SetActive(true);
        }
        else if (greenScore > 1000)
        {
            //1/1
            greenScoreObj[0].GetComponent<Image>().sprite = greenNumSprites[greenScore % 10];
            //1/10
            greenScoreObj[1].GetComponent<Image>().sprite = greenNumSprites[(greenScore / 10) % 10];
            //1/100
            greenScoreObj[2].GetComponent<Image>().sprite = greenNumSprites[(greenScore / 100) % 10];
            //1/1000
            greenScoreObj[3].GetComponent<Image>().sprite = greenNumSprites[(greenScore / 1000) % 10];

            greenScoreObj[0].SetActive(true);
            greenScoreObj[1].SetActive(true);
            greenScoreObj[2].SetActive(true);
            greenScoreObj[3].SetActive(true);
        }
        //（あお）
        if (blueScore < 10)
        {
            //1/1
            blueScoreObj[0].GetComponent<Image>().sprite = blueNumSprites[blueScore % 10];
            blueScoreObj[0].SetActive(true);
        }
        else if (blueScore < 100)
        {
            //1/1
            blueScoreObj[0].GetComponent<Image>().sprite = blueNumSprites[blueScore % 10];
            //1/10
            blueScoreObj[1].GetComponent<Image>().sprite = blueNumSprites[(blueScore / 10) % 10];
            blueScoreObj[0].SetActive(true);
            blueScoreObj[1].SetActive(true);
        }
        else if (blueScore < 1000)
        {
            //1/1
            blueScoreObj[0].GetComponent<Image>().sprite = blueNumSprites[blueScore % 10];
            //1/10
            blueScoreObj[1].GetComponent<Image>().sprite = blueNumSprites[(blueScore / 10) % 10];
            //1/100
            blueScoreObj[2].GetComponent<Image>().sprite = blueNumSprites[(blueScore / 100) % 10];
            blueScoreObj[0].SetActive(true);
            blueScoreObj[1].SetActive(true);
            blueScoreObj[2].SetActive(true);
        }
        else if (blueScore > 1000)
        {
            //1/1
            blueScoreObj[0].GetComponent<Image>().sprite = blueNumSprites[blueScore % 10];
            //1/10
            blueScoreObj[1].GetComponent<Image>().sprite = blueNumSprites[(blueScore / 10) % 10];
            //1/100
            blueScoreObj[2].GetComponent<Image>().sprite = blueNumSprites[(blueScore / 100) % 10];
            //1/1000
            blueScoreObj[3].GetComponent<Image>().sprite = blueNumSprites[(blueScore / 1000) % 10];

            blueScoreObj[0].SetActive(true);
            blueScoreObj[1].SetActive(true);
            blueScoreObj[2].SetActive(true);
            blueScoreObj[3].SetActive(true);
        }
    }

    void ScoreDisplay(float clearTime)
    {
        //初期化
        //赤数字表示用オブジェクトを非表示にします
        for (int i = 0; i < clearTimeUI.Length; i++)
        {
            clearTimeUI[i].SetActive(false);
        }
        float time_0 = clearTime * 10;
        float time_1 = clearTime * 100;

        //（クリアタイム）
        if (clearTime < 10)
        {
            //10/1
            clearTimeUI[0].GetComponent<Image>().sprite = redNumSprites[(int)time_0 % 10];
            clearTimeUI[0].SetActive(true);
            //100/1
            clearTimeUI[1].GetComponent<Image>().sprite = redNumSprites[(int)time_1 % 10];
            clearTimeUI[1].SetActive(true);
            //1/1
            clearTimeUI[2].GetComponent<Image>().sprite = redNumSprites[(int)clearTime % 10];
            clearTimeUI[2].SetActive(true);
        }
        else if (clearTime < 100)
        {
            //10/1
            clearTimeUI[0].GetComponent<Image>().sprite = redNumSprites[(int)time_0 % 10];
            //100/1
            clearTimeUI[1].GetComponent<Image>().sprite = redNumSprites[(int)time_1 % 10];
            //1/1
            clearTimeUI[2].GetComponent<Image>().sprite = redNumSprites[(int)clearTime % 10];
            //1/10
            clearTimeUI[3].GetComponent<Image>().sprite = redNumSprites[(int)(clearTime / 10) % 10];
            clearTimeUI[0].SetActive(true);
            clearTimeUI[1].SetActive(true);
            clearTimeUI[2].SetActive(true);
            clearTimeUI[3].SetActive(true);
        }
        else if (clearTime < 1000)
        {
            //10/1
            clearTimeUI[0].GetComponent<Image>().sprite = redNumSprites[(int)time_0 % 10];
            //100/1
            clearTimeUI[1].GetComponent<Image>().sprite = redNumSprites[(int)time_1 % 10];
            //1/1
            clearTimeUI[2].GetComponent<Image>().sprite = redNumSprites[(int)clearTime % 10];
            //1/10
            clearTimeUI[3].GetComponent<Image>().sprite = redNumSprites[(int)(clearTime / 10) % 10];
            //1/100
            clearTimeUI[4].GetComponent<Image>().sprite = redNumSprites[(int)(clearTime / 100) % 10];
            clearTimeUI[0].SetActive(true);
            clearTimeUI[1].SetActive(true);
            clearTimeUI[2].SetActive(true);
            clearTimeUI[3].SetActive(true);
            clearTimeUI[4].SetActive(true);
        }
        else if (clearTime > 1000)
        {
            //10/1
            clearTimeUI[0].GetComponent<Image>().sprite = redNumSprites[(int)time_0 % 10];
            //100/1
            clearTimeUI[1].GetComponent<Image>().sprite = redNumSprites[(int)time_1 % 10];
            //1/1
            clearTimeUI[2].GetComponent<Image>().sprite = redNumSprites[(int)clearTime % 10];
            //1/10
            clearTimeUI[3].GetComponent<Image>().sprite = redNumSprites[(int)(clearTime / 10) % 10];
            //1/100
            clearTimeUI[4].GetComponent<Image>().sprite = redNumSprites[(int)(clearTime / 100) % 10];
            //1/1000
            clearTimeUI[5].GetComponent<Image>().sprite = redNumSprites[(int)(clearTime / 1000) % 10];

            clearTimeUI[0].SetActive(true);
            clearTimeUI[1].SetActive(true);
            clearTimeUI[2].SetActive(true);
            clearTimeUI[3].SetActive(true);
            clearTimeUI[4].SetActive(true);
            clearTimeUI[5].SetActive(true);
        }
    }

        // Use this for initialization
        void Start()
    {

        //赤数字表示用オブジェクトを非表示にします
        for (int i = 0; i < redScoreObj.Length; i++)
        {
            redScoreObj[i].SetActive(false);
        }
        //緑数字表示用オブジェクトを非表示にします
        for (int i1 = 0; i1 < greenScoreObj.Length; i1++)
        {
            greenScoreObj[i1].SetActive(false);
        }
        //青数字表示用オブジェクトを非表示にします
        for (int i2 = 0; i2 < blueScoreObj.Length; i2++)
        {
            blueScoreObj[i2].SetActive(false);
        }


    }

    // Update is called once per frame
    void Update()
    {
        ScoreDisplay(playerController.MurepuniSlimeObj_Red.GetComponent<Slime_Children_R>().slimeState.bp,
        playerController.MurepuniSlimeObj_Green.GetComponent<Slime_Children_G>().slimeState.bp,
                     playerController.MurepuniSlimeObj_Blue.GetComponent<Slime_Children_B>().slimeState.bp);

        ScoreDisplay(sceneController.GameTime);
    }
}
