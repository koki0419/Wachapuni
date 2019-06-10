using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeansTree : MonoBehaviour {
    //ツタの状態です
    public enum TreeType
    {
        None,
        Normal,   //通常
        FireTree, //燃えている
    }
    [SerializeField]
    private TreeType treeType = TreeType.None;

    //-------------Unityコンポーネント---------------------
    //自身の子オブジェのツタを参照します
    [SerializeField] private GameObject fireTreeParticle;
    //子供オブジェクトを取得
    private GameObject[] boxes;
    [SerializeField] private Fill fill;

    //------------クラスの宣言---------------------------
    private SEController seController;

    //-----------数値用変数---------------------------
    //ツタが燃え尽きるまでのタイムカウンター
    private float destroyTime;
    //ツタが燃え尽きるまでのタイム
    private float destroyTimeMax = 5.0f;

    //--------------フラグ用変数------------------------




    // Use this for initialization
    void Start () {
       treeType = TreeType.Normal;

        //SEコントローラを参照します
        seController = GameObject.Find("SEManager").GetComponent<SEController>();

        //子供オブジェクトを取得
        boxes = new GameObject[transform.childCount];

        for (int i = 0; transform.childCount > i; i++)
        {
            boxes[i] = transform.GetChild(i).gameObject;
        }
        //炎パーティクルを非表示
        boxes[0].SetActive(false);
        //煙パーティクルを非表示
        boxes[1].SetActive(false);
    }

    //se4を流す
	
	// Update is called once per frame
	void Update () {
        switch (treeType)
        {
            case TreeType.Normal:
                boxes[0].SetActive(false);
                destroyTime = destroyTimeMax;
                break;
            case TreeType.FireTree:
                boxes[0].SetActive(true);
                //ツタが燃えたら、タイムカウント開始
                destroyTime -= Time.deltaTime;

                if(destroyTime <= 0)
                {
                    this.gameObject.SetActive(false);
                    fill.IsUsed = false;
                    treeType = TreeType.Normal;
                    destroyTime = 0;
                    seController.StopSE();
                }

                break;
        }
	}



    //---------------------関数定義-----------------------------
    /// <summary>
    /// ツタに各群れプニが当たった時の処理
    /// 赤　→　燃やす
    /// 青　→　鎮火
    /// 緑　→　何も起こらない
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //赤群れプニ
        if(collision.gameObject.tag== "Children_Red")
        {
            seController.playSE(1);
            treeType = TreeType.FireTree;
            Destroy(collision.gameObject);
        }
        //青群れプニ
        else if (collision.gameObject.tag == "Children_Blue")
        {
            seController.StopSE();
            seController.playSE(3);
            treeType = TreeType.Normal;
            Destroy(collision.gameObject);
        }
        //緑群れプニ
        else if(collision.gameObject.tag == "Children_Green")
        {
            Destroy(collision.gameObject);
        }
    }


    //煙のコルーチン
    public IEnumerator OnSmoke()
    {
        //煙表示
        boxes[1].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        seController.StopSE();
        //煙非表示
        boxes[1].SetActive(false);
    }
}
