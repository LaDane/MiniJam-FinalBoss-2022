using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMouseSensitivity : MonoBehaviour {

    [SerializeField] private Slider slider;

    private void Start() {
        slider.onValueChanged.AddListener(delegate { SetSensitivity(); });
    }

    public void SetSensitivity() {
        PlayerControls.Instance.controls.mouseSensitivity = slider.value;
    }
}
