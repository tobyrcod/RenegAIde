using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    bool pressed;

    private void Start() {
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pressed) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.P)) {
                pressed = true;
                SoundManager.PlaySound(SoundManager.Sound.pressStart);
                StartCoroutine(StartGame(1f));
            }
        }
    }

    private IEnumerator StartGame(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        GameManager.instance.ChangeScene("Settings");
    }
}
