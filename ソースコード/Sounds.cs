using UnityEngine;

public class Sounds : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 基本的なクリック音を鳴らす
    /// </summary>
    public void ClickSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

}
