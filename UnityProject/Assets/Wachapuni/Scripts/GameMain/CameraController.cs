using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //プレイヤーのTransformを取得
    private Transform player;

    // Use this for initialization
    void Start()
    {
        //プレイヤーのTransformを参照
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// プレイヤーを上下左右に追従します
    /// </summary>
    void Update()
    {
        if (player.GetComponent<PlayerController>().slimeType == PlayerController.SlimeType.Normal)
        {
            var position = transform.position;
            position.x = player.position.x;
            position.y = player.position.y + 2.1f;
            transform.position = position;
        }
    }
}
