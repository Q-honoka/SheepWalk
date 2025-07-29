using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sounds : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip audioClipWalk;
    private AudioClip audioClipJump;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 基本的クリック
    /// </summary>
    public void ClickSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

}
