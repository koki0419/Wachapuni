using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Children_R : Slime_ChildrenController
{
    [SerializeField] private int bp = 100;         //攻撃回数

    // Use this for initialization
    void Start()
    {
        slimeState.bp = bp;
        isUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSearch)
        {
            Moving();
        }
    }
    /// <summary>
    /// 群れぷにの移動に関する関数
    /// プレイヤーと自分とのベクトルを生成して
    /// プレイヤーの向きを見て回転しながらプレイヤーを追いかけます
    /// </summary>
    public void Moving()
    {

        // プレイヤーのポジションを取得
        Vector3 playerpos = PlayerObj.transform.localPosition; // 都度プレイヤー位置更新
                                                               // 自分にプレイヤーポジションを入れる(ワープしてしまう)
                                                               // gameObject.transform.localPosition = playerpos;

        // 自分の座標とプレイヤーのベクトル作成(プレイヤー座標からEnemy座標を引く)
        Vector3 moveVec = playerpos - gameObject.transform.localPosition; // (オペレータ機能を利用している)
        // 単位ベクトルの作成（上記のベクトル）
        Vector3 V2 = moveVec.normalized;
        // 向きベクトルの作成（自分の向き）
        // 回転量
        Vector3 objRot = transform.eulerAngles;
        // キャラクタの向いている方向ベクトル計算
        float x = Mathf.Sin(objRot.y * Mathf.Deg2Rad);
        float z = Mathf.Cos(objRot.y * Mathf.Deg2Rad);
        Vector3 V1 = new Vector3(x, 0, z);
        // 内積計算（2つのベクトルを使用）
        float theta = V1.x * V2.x + V1.y * V2.y + V1.z * V2.z;
        // 計算誤差修正(内積結果が-1.0～1.0の間に収める)
        if (theta > 1.0f) theta = 1.0f;
        if (theta < -1.0f) theta = -1.0f;
        // 関数使用例
        theta = Mathf.Max(theta, -1.0f); // 大きいほうを採用
        theta = Mathf.Min(theta, 1.0f); // 小さいほうを採用
        // 角度を求める(acos)
        float rot = Mathf.Acos(theta);
        // 外積計算（回転向きを求める）
        float crosY = V1.x * V2.z - V1.z * V2.x;
        // 向き修正
        if (crosY > 0.0f) rot = -rot;
        transform.Rotate(new Vector3(0, rot * 2.5f, 0));
        moveVec = V2 * DefSpeed;
        // 自動追尾
        if (Mathf.Abs(playerpos.x - gameObject.transform.localPosition.x) >= 2.0f || Mathf.Abs(playerpos.z - gameObject.transform.localPosition.z) >= 2.0f)
        {
            gameObject.transform.localPosition += moveVec;
        }
        else
        {
            gameObject.transform.localPosition += new Vector3(0, 0, 0);
        }
        //空中にいるとき
        if (isGround == false)
        {
            var velocity = gameObject.GetComponent<Rigidbody>().velocity;
            velocity.y = -20.0f;
        }
        if (Mathf.Abs(playerpos.x - gameObject.transform.localPosition.x) >= 10.0f)
        {
            gameObject.transform.localPosition = new Vector3(playerpos.x, playerpos.y + 5.0f, playerpos.z);
        }
    }
}
