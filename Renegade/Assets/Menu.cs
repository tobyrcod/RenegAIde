using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SoundManager.PlaySound(SoundManager.Sound.pressStart);
            StartCoroutine(StartGame(1f));
        }        
    }

    private IEnumerator StartGame(float waitTime) {
        Debug.Log(waitTime);
        yield return new WaitForSeconds(waitTime);
        GameManager.instance.ChangeScene("Game");
    }
}
