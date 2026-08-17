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

    /// <summary>
    /// 歩くボタンの処理
    /// </summary>
    public void SheepWalk()
    {
        if(walk == false)
        {
            // タイマーを止める
            gameController.StopTimer();
            gameController.MoveCameraPos();
            walk = true;

            Invoke("Walk", 0.3f);

            // ジャンプの状態に応じてジャンプボタンの表示を切り替える
            if (JumpBool == true)
            {
                Button.GetComponent<Image>().sprite = JumpButton;
            }
            else
            {
                Button.GetComponent<Button>().interactable = false;
            }
        }
        // 羊をジャンプさせる
        else if(JumpBool == true)
        {
            playerController.SetJumpStart();
        }

    }

    /// <summary>
    /// 羊を歩かせるボタンの処理
    /// </summary>
    private void Walk()
    {
        playerController.SetMoveStart(Direction.Up);
    }

    /// <summary>
    /// 操作説明を次のページにする
    /// </summary>
    public void NextPage()
    {
        gameController.PageUp();
    }

    /// <summary>
    /// ステージ１に遷移
    /// </summary>
    public void ToStage1Scene()
    {
        gameManager.SetCurrentStageState(StageState.STAGE1);
    }

    /// <summary>
    /// ステージ２に遷移
    /// </summary>
    public void ToStage2Scene()
    {
        gameManager.SetCurrentStageState(StageState.STAGE2);
    }

    /// <summary>
    /// ステージ３に遷移
    /// </summary>
    public void ToStage3Scene()
    {
        gameManager.SetCurrentStageState(StageState.STAGE3);
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void ToEnd()
    {
        Invoke("End", 0.4f);
    }

    /// <summary>
    /// タイトルシーンに遷移
    /// </summary>
    public void ToTitleScene()
    {
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
