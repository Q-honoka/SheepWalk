using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class OnButtonScript : MonoBehaviour
{
    // 変数宣言
    private GameManager gameManager;

    [Header("ジャンプの有無")]
    [SerializeField] bool JumpBool;

    [Header("ジャンプ画像")]
    [SerializeField] Sprite JumpButton;

    [Header("画像を差し替えるボタン")]
    [SerializeField] GameObject Button;

    public GameObject GC;
    GameController gameController;

    public GameObject Sheep;
    PlayerController playerController;

    private bool walk = false;

    private void Start()
    {
        // GameManagerの取得
        gameManager = GameManager.instance;

        if (SceneManager.GetActiveScene().name == "Stage1Scene" ||
            SceneManager.GetActiveScene().name == "Stage2Scene" ||
            SceneManager.GetActiveScene().name == "Stage3Scene")
        {
            gameController = GC.GetComponent<GameController>();
            playerController = Sheep.GetComponent<PlayerController>();
            Button.GetComponent<Button>().interactable = false;
        }
    }

    private void Update()
    {
        if(gameController != null)
        {
            if (gameController.GameStart == true)
            {
                Invoke("ActiveButton", 1.0f);
            }
        }
    }

    void ActiveButton()
    {
        Button.GetComponent<Button>().interactable = true;
    }

    // 羊が歩くボタン
    public void SheepWalk()
    {
        // 連打対策をする

        if(walk == false)
        {
            // タイマーを止める
            gameController.StopTimer();
            gameController.MoveCameraPos();
            // デバッグ用
            Debug.Log("Sheep Walk");
            walk = true;

            Invoke("Walk", 0.3f);

            if (JumpBool == true)
            {
                Button.GetComponent<Image>().sprite = JumpButton;
            }
            else
            {
                // JumpBoolがfalseなら非アクティブにする
                Button.GetComponent<Button>().interactable = false;
            }
        }
        else if(JumpBool == true)
        {
            Debug.Log("SheepJump");
            playerController.SetJumpStart();
        }

    }

    private void Walk()
    {
        playerController.SetMoveStart(Direction.Up);
    }

    // 操作説明を次のページにする
    public void NextPage()
    {
        Debug.Log("次のページ");
        gameController.PageUp();
    }

    // ステージ１に遷移
    public void ToStage1Scene()
    {
        // デバッグ用
        Debug.Log("To Stage1Scene");
        gameManager.SetCurrentStageState(StageState.STAGE1);
    }

    // ステージ２に遷移
    public void ToStage2Scene()
    {
        // デバッグ用
        Debug.Log("To Stage2Scene");
        gameManager.SetCurrentStageState(StageState.STAGE2);
    }

    // ステージ３に遷移
    public void ToStage3Scene()
    {
        // デバッグ用
        Debug.Log("To Stage3Scene");
        gameManager.SetCurrentStageState(StageState.STAGE3);
    }

    // ゲームを終了する
    public void ToEnd()
    {
        // デバッグ用
        Debug.Log("To End");
        Invoke("End", 0.4f);
    }

    // タイトルシーンに遷移
    public void ToTitleScene()
    {
        // デバッグ用
        Debug.Log("To TitleScene");
        // タイトルシーンに遷移
        gameManager.SetCurrentGameState(GameState.TITLE);
        FadeAnimationSceneManager.Instance.LoadScene("TitleScene", 0.5f);  // シーン遷移
    }

    private void End()
    {
        // ゲームを終了する
#if UNITY_EDITOR
        // Unityエディターでの動作
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 実際のゲーム終了処理
        Application.Quit();
#endif

    }
}
