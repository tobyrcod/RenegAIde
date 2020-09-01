using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
    [SerializeField] Image[] frames;

    public void SetColor(Color color) {
        foreach (Image image in frames) {
            image.color = color;
        }
    }
}
