using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [Header("クリア時に表示するオブジェクト")]
    [SerializeField] GameObject clear;

    [Header("タイムアップ時に表示するオブジェクト")]
    [SerializeField] GameObject timesUp;

    [Header("衝突時に表示するオブジェクト")]
    [SerializeField] GameObject collision;

    [Header("池に落ちた時に表示するオブジェクト")]
    [SerializeField] GameObject wetting;

    [Header("池に落ちた時に表示するオブジェクト")]
    [SerializeField] GameObject river;

    [Header("ゲームクリア音楽")]
    [SerializeField] AudioClip audioClipClear;

    [Header("ゲームオーバー音楽")]
    [SerializeField] AudioClip audioClipOver;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        clear.SetActive(false);
        timesUp.SetActive(false);
        collision.SetActive(false);
        wetting.SetActive(false);
        river.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        // ResultStateに応じて表示を変える
        switch (GameManager.instance.GetResultState())
        {
            case ResultState.CLEAR:
                clear.SetActive(true);
                audioSource.PlayOneShot(audioClipClear,0.5f);
                break;

            case ResultState.TIMESUP:
                timesUp.SetActive(true);
                audioSource.PlayOneShot(audioClipOver);
                break;

            case ResultState.COLLISION:
                collision.SetActive(true);
                audioSource.PlayOneShot(audioClipOver);
                break;

            case ResultState.WETTING:
                wetting.SetActive(true);
                audioSource.PlayOneShot(audioClipOver);
                break;

            case ResultState.RIVER:
                river.SetActive (true);
                audioSource.PlayOneShot(audioClipOver);
                break;

            default:
                Debug.Log("ResultStateに対応する処理がありません");
                break;

        }
    }
}
