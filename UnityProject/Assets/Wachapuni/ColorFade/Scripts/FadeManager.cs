using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

// シーン遷移時のフェードイン・アウトを制御するためのクラス .
public class FadeManager : MonoBehaviour
{

    #region Singleton

    private static FadeManager instance;

    public static FadeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (FadeManager)FindObjectOfType(typeof(FadeManager));

                if (instance == null)
                {
                    Debug.LogError(typeof(FadeManager) + "is nothing");
                }
            }

            return instance;
        }
    }

    #endregion Singleton

    public bool DebugMode = true;       // デバッグモードフラグ(trueでデバッグ窓表示）

    private float fadeAlpha = 0;        // フェード中の透明度(0で透明)
    private bool isFading = false;      // フェード中かどうか(true:フェード中)
    public Color fadeColor = Color.black;   // フェード中の色

    // 起動後チェック
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // フェード表示をするための描画部分
    public void OnGUI()
    {

        // Fade .
        if (this.isFading)
        {
            //色と透明度を更新して白テクスチャを描画 .
            this.fadeColor.a = this.fadeAlpha;
            GUI.color = this.fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }

        //if (this.DebugMode)
        //{
        //    if (!this.isFading)
        //    {
        //        //Scene一覧を作成 .
        //        //(UnityEditor名前空間を使わないと自動取得できなかったので決めうちで作成) .
        //        List<string> scenes = new List<string>();
        //        scenes.Add("SampleScene");// 強制的にサンプルシーンにとぶ


        //        //Sceneが一つもない .
        //        if (scenes.Count == 0)
        //        {
        //            GUI.Box(new Rect(10, 10, 200, 50), "Fade Manager(Debug Mode)");
        //            GUI.Label(new Rect(20, 35, 180, 20), "Scene not found.");
        //            return;
        //        }


        //        GUI.Box(new Rect(10, 10, 300, 50 + scenes.Count * 25), "Fade Manager(Debug Mode)");
        //        GUI.Label(new Rect(20, 30, 280, 20), "Current Scene : " + SceneManager.GetActiveScene().name);

        //        int i = 0;
        //        foreach (string sceneName in scenes)
        //        {
        //            if (GUI.Button(new Rect(20, 55 + i * 25, 100, 20), "Load Level"))
        //            {
        //                LoadScene(sceneName, 1.0f);
        //            }
        //            GUI.Label(new Rect(125, 55 + i * 25, 1000, 20), sceneName);
        //            i++;
        //        }
        //    }
        //}
    }

    // シーン遷移関数　scene:シーン名　interval:暗転にかかる時間(秒)
    public void LoadScene(string scene, float interval)
    {
        StartCoroutine(TransScene(scene, interval));
    }

    // シーン遷移用コルーチン　scene:シーン名　interval:暗転にかかる時間(秒)
    private IEnumerator TransScene(string scene, float interval)
    {
        //だんだん暗く .
        this.isFading = true;   // フェード中フラグをtrue
        float time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        //シーン切替 .
        SceneManager.LoadScene(scene);

        //だんだん明るく .
        time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        this.isFading = false;   // フェード中フラグをfalse
    }
}