using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{

    public void onClickButton()
    {
        FadeManager.Instance.LoadScene("Second", 2.0f);
    }

}
