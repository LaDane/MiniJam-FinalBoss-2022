using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour {

    public void OnRestartBtnPress() {
        UIManager.Instance.ButtonRestart();
    }

    public void OnMainMenuBtnPress() {
        UIManager.Instance.ButtonMainMenu();
    }
}
