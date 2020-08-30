using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    private float nextFlashTime;
    [SerializeField] float flashDelayTime;
    [SerializeField] GameObject flashObject;

    private void Start() {
        nextFlashTime = Time.time + flashDelayTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFlashTime) {
            flashObject.SetActive(!flashObject.activeSelf);
            nextFlashTime = Time.time + flashDelayTime;
        }
    }
}
