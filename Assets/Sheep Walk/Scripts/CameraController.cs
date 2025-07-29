using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("回転速度")]
    [SerializeField] float rotateSpeed = 0.5f;
    
    [Header("カメラを向ける速度")]
    [SerializeField] float speed = 0.1f;

    [Header("矢印ボタン")]
    [SerializeField] GameObject ArrowButton;

    [SerializeField]
    private GameObject clickedGameObject;       //クリックされたゲームオブジェクトを保存
    private GameObject clickedGameObject_old;   // 前にクリックされたゲームオブジェクトを保存
    [SerializeField]
    private GameObject target;                  // 回転の中心になるオブジェクト

    [Header("横向きバームクーヘン")]
    [SerializeField] GameObject[] landscapeBaum;

    private float horizontalInput;  // 水平方向の入力
    private float verticalInput;    // 垂直方向の入力
    private Vector3 relativePos;    // ターゲットの方向のベクトル
    private Quaternion rotation;    // 回転
    private bool IsStart;           // カメラの移動制御

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        clickedGameObject_old = clickedGameObject;
        clickedGameObject.GetComponent<Outline>().enabled = true;
        IsStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStart)
        {
            return;
        }
        // カメラの移動
        CameraMove();

        // レイをとばしてオブジェクトを取得
        InputMouseButton();
    }

    /// <summary>
    /// クリックされたバームクーヘンを返す関数
    /// </summary>
    /// <returns></returns>
    public GameObject GetClickedGameObject()
    {
        // クリックされたゲームオブジェクトがなければ null を返す
        if(clickedGameObject == null)
        {
            return null;
        }

        // デバッグ用
        Debug.Log(clickedGameObject.name + "を返す");
        return clickedGameObject;
    }

    /// <summary>
    /// クリックでレイをとばす処理
    /// </summary>
    private void InputMouseButton()
    {
        // 左クリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }

            // クリックされた先にレイをとばす
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                // ヒットしたコライダーのタグがpuddleだったら
                if (hit.collider.gameObject.CompareTag("puddle") ||
                    hit.collider.gameObject.CompareTag("fence") ||
                    hit.collider.gameObject.CompareTag("right") ||
                    hit.collider.gameObject.CompareTag("left"))
                {
                    // ヒットしたコライダーの親オブジェクトをセット
                    clickedGameObject = hit.collider.gameObject.transform.parent.gameObject;

                    // 前と違う Baum がクリックされたら入れ替える
                    if (clickedGameObject != clickedGameObject_old)
                    {
                        clickedGameObject_old.GetComponent<Outline>().enabled = false;
                        clickedGameObject_old = clickedGameObject;
                    }

                    // クリックされたゲームオブジェクトのアウトラインを表示
                    clickedGameObject.GetComponent<Outline>().enabled = true;

                }
                // ヒットしたオブジェクトのタグが Baum だったら
                else if (hit.collider.gameObject.CompareTag("Baum"))
                {
                    // クリックされたゲームオブジェクトをセットする
                    clickedGameObject = hit.collider.gameObject;

                    // 前と違う Baum がクリックされたら入れ替える
                    if (clickedGameObject != clickedGameObject_old)
                    {
                        clickedGameObject_old.GetComponent<Outline>().enabled = false;
                        clickedGameObject_old = clickedGameObject;
                    }

                    // クリックされたゲームオブジェクトのアウトラインを表示
                    clickedGameObject.GetComponent<Outline>().enabled = true;
                }

            }

        }

    }

    /// <summary>
    /// カメラの移動処理
    /// </summary>
    private void CameraMove()
    {
        // カメラの移動処理
        // 入力
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        // 水平方向の回転
        transform.RotateAround(target.transform.position, new Vector3(0, 1, 0), rotateSpeed * -horizontalInput);
        // 垂直方向の回転
        transform.RotateAround(target.transform.position, new Vector3(1, 0, 0), rotateSpeed * verticalInput);

        // 回転制御


        // バームクーヘンの方を向く
        // ターゲット方向のベクトルを取得
        relativePos = clickedGameObject.transform.position - this.transform.position;

        // 方向を、回転情報に変換
        rotation = Quaternion.LookRotation(relativePos);

        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);

        // カメラと一緒に矢印ボタンも回転する
        ArrowButton.transform.rotation = Quaternion.Euler(0, 0, Mathf.Round(transform.eulerAngles.y / 90) * 90 - 90);

        // 横向きバームクーヘンがなければこのあとの処理はしない
        if(landscapeBaum.Length == 0)
        {
            return;
        }

        // 横向きバームクーヘンだったらさらに-90度回転させる
        for(int i = 0; i < landscapeBaum.Length; i++)
        {
            GameObject obj = landscapeBaum[i];

            // クリックされたオブジェクトが横向きバームクーヘンとして登録されていたら
            if (clickedGameObject == obj)
            {
                float adjustedRotation = Mathf.Round(transform.eulerAngles.y / 90) * 90;

                if (i == 3 || i == 4 || i == 5)
                {
                    // 横向きで進行方向が右から左のバームクーヘン
                    ArrowButton.transform.rotation = Quaternion.Euler(0, 0, adjustedRotation);
                }
                else
                {
                    // 横向きで進行方向が左から右のバームクーヘン
                    ArrowButton.transform.rotation = Quaternion.Euler(0, 0, adjustedRotation - 180);
                }
            }

        }
    }

    public void StopMove()
    {
        IsStart = false;
    }
}
