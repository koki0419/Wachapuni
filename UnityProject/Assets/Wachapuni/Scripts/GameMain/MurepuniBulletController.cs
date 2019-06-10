using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurepuniBulletController : MonoBehaviour {

    //------------フラグ用変数---------------------
    public bool isUsedFlag
    {
        set; get;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
}
