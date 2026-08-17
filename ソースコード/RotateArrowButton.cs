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

    /// <summary>
    /// 矢印ボタンを押したときの処理
    /// </summary>
    public void OnArrowButton()
    {
        ClickBaum = cameraController.GetClickedGameObject();

        // クリックされたゲームオブジェクトが空でなければ、回転処理の指示を出す
        if (ClickBaum != null || playerController.GetMoveStart() == false)
        {
            rotateBaum = ClickBaum.GetComponent<RotateBaum>();
            rotateBaum.SetTargetRotation((int)dir);
        }
    }
}