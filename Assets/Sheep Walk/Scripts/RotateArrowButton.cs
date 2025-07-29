using UnityEngine;
using SmoothigTransform;
using UnityEngine.UI;

public class RotateArrowButton : MonoBehaviour
{
    // カメラ
    [Header("レイをとばすカメラ")]
    [SerializeField] GameObject Camera;
    CameraController cameraController;

    [Header("プレイヤー")]
    [SerializeField] GameObject player;
    PlayerController playerController;

    // 回転方向を定数で定義
    public enum Dir
    {
        Left = 1,
        Right = -1,
    }

    [Header("回転方向")]
    [SerializeField] Dir dir;

    // 回転対象のバームクーヘン
    private GameObject ClickBaum;
    private RotateBaum rotateBaum;

    private void Start()
    {
        // CameraController の取得
        cameraController = Camera.GetComponent<CameraController>();
        playerController = player.GetComponent<PlayerController>();
        this.gameObject.SetActive(true);
    }

    public void Update()
    {
        // プレイヤーが歩き出したらボタンを非アクティブにする
        if(playerController.GetMoveStart() == true)
        {
            this.GetComponent<Button>().interactable = false;
            this.gameObject.SetActive(false);
        }
    }

    public void OnArrowButton()
    {
        // ゲームオブジェクトを代入
        ClickBaum = cameraController.GetClickedGameObject();

        // クリックされたゲームオブジェクトが空でなければ
        if (ClickBaum != null || playerController.GetMoveStart() == false)
        {
            // クリックされたオブジェクトのスクリプトを取得する
            rotateBaum = ClickBaum.GetComponent<RotateBaum>();

            // バームクーヘンに回転指示を出す
            rotateBaum.SetTargetRotation((int)dir);
        }
    }
}