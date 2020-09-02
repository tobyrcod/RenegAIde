using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglesController : MonoBehaviour
{
    [SerializeField] Sprite onImage;
    [SerializeField] Sprite offImage;

    [SerializeField] public Image AIToggle;

    [SerializeField] public Image WhiteToggle;
    [SerializeField] public Image BlackToggle;

    [SerializeField] public Image FirstToggle;
    [SerializeField] public Image SecondToggle;

    public void ToggleUI(Image image, bool on) {
        Sprite sprite = (on) ? onImage : offImage;
        image.sprite = sprite;
    }

}
