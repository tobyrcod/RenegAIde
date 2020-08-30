using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Transform musicParent;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject soundGameObject = SoundManager.PlayBackgroundMusic(SoundManager.Sound.gameBackground);
        //soundGameObject.transform.parent = musicParent;
    }
}
