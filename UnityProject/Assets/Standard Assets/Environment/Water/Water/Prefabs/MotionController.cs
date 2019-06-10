using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour {

    // 自分のアニメーションコンポーネント
    Animator animatorComponent;

	// Use this for initialization
	void Start () {
        // アニメーションコンポーネント取得
        animatorComponent = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //アニメーションの設定します
        //ジャンプボタン
        if (Input.GetKeyDown(KeyCode.A))
        {
            animatorComponent.SetBool("JumpFlag", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //animatorComponent.SetBool("JumpFlag", true);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            animatorComponent.SetBool("JumpFlag", false);
        }
    }
}
