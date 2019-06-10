using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Slime_ChildrenController : MonoBehaviour {


    //スライムのステータスです
    public struct SlimeState
    {
        public int bp;          //攻撃回数
    }
    public SlimeState slimeState = new SlimeState();

    //------Unityコンポーネント宣言--------------

    [SerializeField] private GameObject playerObj;
    public GameObject PlayerObj
    {
        get { return playerObj; }
        set { playerObj = value; }
    }
    new Rigidbody rigidbody;

    // 自分のアニメーションコンポーネント
    private Animator animatorComponent;

    //--------------数値変数の宣言------------------
    //追尾スピード
    [SerializeField]private float defSpeed = 0.1f;
    public float DefSpeed
    {
        get { return defSpeed; }
    }
    //--------フラグ変数の宣言------------------
    //地面にいるかどうか
    public bool isGround
    {
        get;set;
    }
    //サーチ状態かどうか
    public bool isSearch
    {
        get;set;
    }

    //このスライムを使用したかどうかのフラグ
    //true  使用した
    //false 使用していない
    public bool isUsed
    {
        get;set;
    }
  

    // Use this for initialization
    void Start () {
        isSearch = false;
        isUsed = false;

        // アニメーションコンポーネント取得
        animatorComponent = gameObject.GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (isUsed == true)
        {
            animatorComponent.SetBool("upright", true);
        }
        if (isUsed == false)
        {
            animatorComponent.SetBool("upright", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isSearch = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }
}
