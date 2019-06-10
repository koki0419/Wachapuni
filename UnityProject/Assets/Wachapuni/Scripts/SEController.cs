using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SEを管理します
public class SEController : MonoBehaviour
{


    // 自分のサウンドソースコンポーネント
    AudioSource audioSource;
    // オーディオクリップデータ
    // ゲーム画面
    // 0 積む音                      SE_04
    // 1 木箱が燃える音              SE_05
    // 2 鉄箱が燃える音              SE_06
    // 3 消化音                      SE_07
    // 4 ツタを燃やす音              SE_08
    // 5 ノックバック音              SE_09
    // 6 ゴール地点に着いたときの音  SE_10
    // 7 ゴール1秒後に流すジングル   SE_11

    //ステージセレクト
    // 0 ステージ選択音               SE_02
    // 1 ステージスタート音           SE_03

    public AudioClip[] audioClip;

    public int buttonSENo;

    // Use this for initialization
    void Start()
    {
        // サウンドソースコンポーネント取得
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip[0];
    }

    public void StopSE()
    {
        audioSource.Stop();
    }
    public void SeButton()
    {
        //スタートSEを再生します
        audioSource.PlayOneShot(audioClip[buttonSENo]);
    }
    public void playSE(int playSENo)
    {
        //スタートSEを再生します
        audioSource.PlayOneShot(audioClip[playSENo]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
