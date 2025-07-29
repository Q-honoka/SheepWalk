using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TimerText;  // タイマーを表示するテキスト

    [Header("制限時間")]
    [SerializeField] float limit = 15.0f;

    [Header("操作説明の有無")]
    [SerializeField] bool manualDisp = true;

    [Header("操作説明を表示するゲームオブジェクト")]
    [SerializeField] GameObject ManualObj;

    [Header("操作説明に使う画像")]
    [SerializeField] Sprite[] manualSprite;

    [Header("ゲームで使うカメラ")]
    [SerializeField] GameObject Camera;

    [Header("歩くボタンを押したときのカメラの場所")]
    [SerializeField] GameObject Screen;

    public bool GameStart;      // ゲームスタートフラグ

    private Image ManualImage;
    private int currentPage;          // 現在の表示画像番号
    private bool finishManual;

    private bool TimerStart;    // タイマーを開始するフラグ
    private float Timer;    // 経過時間
    private Transform camTrans;
    private Vector3 camPos;
    private quaternion camRot;
    private bool camInit;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        TimerText.enabled = false;
        TimerStart = false;
        Timer = limit;  // 制限時間を代入
        currentPage = 0;    // 現在の枚数
        ManualImage = ManualObj.GetComponent<Image>();
        GameStart = false;

        finishManual = false;
        ManualObj.SetActive(true);
        ManualImage.sprite = manualSprite[currentPage];
        camTrans = Camera.transform;
        camPos = camTrans.position;
        camRot = camTrans.rotation;
        camInit = false;

        // マニュアルを表示しないならスキップ
        if(manualDisp == false)
        {
            finishManual = true;
            ManualObj.SetActive(false);
            TimerStart = true;
            GameStart = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(TimerStart == true)
        {
            TimerText.enabled = true;

            // 時間進行
            Timer -= Time.deltaTime;

            // タイマーが０以下になったらリザルトシーンへ遷移する
            if (Timer <= 0)
            {
                Timer = 0;
                // GameManagerからリザルトの情報をタイムアップに変更
                GameManager.instance.SetCurrentResultState(ResultState.TIMESUP);
            }

            // タイマーの表示
            TimerText.text = Timer.ToString("F0");
        }
        else
        {
            Manual();
        }

        // カメラの位置と回転の初期化
        if (GameStart == true && camInit == false)
        {
            InitCamera();
            camInit = true;
        }
    }

    private void InitCamera()
    {
        Camera.transform.position = camPos;
        Camera.transform.rotation = camRot;
    }

    /// <summary>
    /// タイマーを止める関数
    /// </summary>
    public void StopTimer()
    {
        TimerStart = false;
        Debug.Log(TimerStart);
    }

    /// <summary>
    /// 表示する画像を変える関数
    /// </summary>
    private void Manual()
    {
        // タイマーがスタートしているかマニュアルの表示が終わっていたらスキップ
        if(TimerStart != false || finishManual == true)
        {
            return;
        }

        // 現在のページが操作説明の枚数より大きいなら
        if (currentPage > manualSprite.Length - 1)
        {
            // マニュアル表示終了
            finishManual = true;
            Invoke("ManualClose", 0.4f);
            // タイマーをスタートする
            Timer = limit;
            TimerStart = true;
            GameStart = true;
        }
        else
        {
            ManualImage.sprite = manualSprite[currentPage];
        }
    }

    /// <summary>
    /// ページを１枚めくる
    /// </summary>
    public void PageUp()
    {
        currentPage++;
    }

    private void ManualClose()
    {
        // 操作説明を非表示にする
        ManualObj.SetActive(false);
    }

    public void MoveCameraPos()
    {
        Camera.transform.parent = Screen.transform;
        Camera.transform.localPosition = Vector3.zero;
        Camera.transform.localRotation = Quaternion.identity;
        Camera.GetComponent<CameraController>().StopMove();
    }
}
