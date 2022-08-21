using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour {

    public void OnRestartBtnPress() {
        UIManager.Instance.ButtonRestart();
        Debug.Log("Test");
    }

    public void OnMainMenuBtnPress() {
        UIManager.Instance.ButtonMainMenu();
    }
}
