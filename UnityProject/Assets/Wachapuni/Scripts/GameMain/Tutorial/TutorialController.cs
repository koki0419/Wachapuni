using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    [SerializeField] private UIManager uiManager;
    //チュートリアル看板用の番号です
    [SerializeField] private int tutorialNum;


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            uiManager.playerUI[0].SetActive(false);
            uiManager.playerUI[1].SetActive(false);
            other.GetComponent<PlayerController>().isHotIronHit = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerController>().isHotIronHit == false)
            {
                uiManager.PlayerUIDysplay(tutorialNum);
            }
            else if (other.GetComponent<PlayerController>().isHotIronHit == true)
            {
                uiManager.PlayerUIDysplay(1);
            }
            uiManager.playerUI[0].SetActive(true);
            if (tutorialNum < 5)
            {
                uiManager.playerUI[1].SetActive(true);
            }
        }
    }

}
