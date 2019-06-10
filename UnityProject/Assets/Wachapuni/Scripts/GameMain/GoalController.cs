using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    //---------クラスの定義--------------
    //SceneControllerを参照します
    [SerializeField] private SceneController sceneController;
    //SEControllerを参照します
    [SerializeField] private SEController seController;
    //カメラについているBGM用のオーディオを参照します
    [SerializeField] AudioSource cameraAudioSource;

    //--------------関数の定義----------------

    //プレイヤーが当たった時の処理
    /// <summary>
    /// ゴールエリアに入っているときに↑キーを入力でゴール判定にします
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            float h = Input.GetAxis("Vertical");
            if (h > 0)
            {
                cameraAudioSource.Stop();
                sceneController.stageCliarFlag = true;
                seController.playSE(6);
                sceneController.StartCoroutine(sceneController.StageCliar());
               
                Destroy(this.gameObject);
            }
        }
    }

}
