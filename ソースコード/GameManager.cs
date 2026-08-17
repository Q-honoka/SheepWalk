using UnityEngine;
using UnityEngine.SceneManagement;

// ゲームの状態
public enum GameState
{
    TITLE,  // タイトルモード
    GAME,   // ゲームモード
    RESULT, // リザルトモード
}

// ステージの状態
public enum StageState
{
    STAGE1,     // ステージ１
    STAGE2,     // ステージ２
    STAGE3,     // ステージ３
}

// リザルトの状態
public enum ResultState
{
    CLEAR,      // クリア
    TIMESUP,    // タイムアップ
    COLLISION,  // 衝突
    WETTING,    // 池に落ちる
    RIVER,      // 川に落ちる
}


public class GameManager : MonoBehaviour
{
    // GameManagerのインスタンス
    public static GameManager instance;

    // 現在のゲームの状態
    private static GameState currentGameState;
    // 現在のステージの状態
    private static StageState currentStageState;
    // 現在のリザルトの状態
    private static ResultState currentResultState;

    void Awake()
    {
        instance = this;

        // 初期化処理
        if(SceneManager.GetActiveScene().name == "TitleScene")
        {
            SetCurrentGameState(GameState.TITLE);
        }
    }

    /// <summary>
    /// ゲームの状態を変更、状態に応じた処理をする
    /// </summary>
    /// <param name="state"></param>
    public void SetCurrentGameState(GameState state)
    {
        currentGameState = state;
        OnGameStateChanged(currentGameState);
    }

    /// <summary>
    /// ステージの状態を変更し、そのステージのシーンに遷移する
    /// </summary>
    /// <param name="state"></param>
    public void SetCurrentStageState(StageState state)
    {
        currentStageState = state;
        SetCurrentGameState(GameState.GAME);
    }

    /// <summary>
    /// リザルトの状態を変更し、リザルトシーンに遷移する
    /// </summary>
    /// <param name="state"></param>
    public void SetCurrentResultState(ResultState state)
    {
        currentResultState = state;
        SetCurrentGameState(GameState.RESULT);
    }

    /// <summary>
    /// 現在のリザルトの状態を取得する
    /// </summary>
    /// <returns></returns>
    public ResultState GetResultState()
    {
        return currentResultState;
    }

    /// <summary>
    /// 状態に応じて処理を分岐する
    /// </summary>
    /// <param name="state"></param>
    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            // タイトルの処理
            case GameState.TITLE:
                TitleAction();
                break;

            // ゲームの処理
            case GameState.GAME:
                GameAction();
                break;

            // リザルトの処理
            case GameState.RESULT:
                FadeAnimationSceneManager.Instance.LoadScene("ResultScene", 0.5f);  // シーン遷移
                break;

            // どれでもないときの処理
            default:
                Debug.Log("GameStateが設定されていない");
                break;
        }
    }

    /// <summary>
    /// TITLEのときの処理
    /// </summary>
    private void TitleAction()
    {
        // フレームレート
        Application.targetFrameRate = 60;
        // 垂直同期
        QualitySettings.vSyncCount = 0;
    }

    /// <summary>
    /// ステージの情報に応じてロードするシーンを変える
    /// </summary>
    /// <param name="stageState"></param>
    private void GameAction()
    {
        // ステージ番号で処理を分岐
        switch(currentStageState)
        {
            // ステージ１の処理を呼び出す
            case StageState.STAGE1:
                FadeAnimationSceneManager.Instance.LoadScene("Stage1Scene", 0.5f);  // シーン遷移
                break;

            // ステージ２の処理を呼び出す
            case StageState.STAGE2:
                FadeAnimationSceneManager.Instance.LoadScene("Stage3Scene", 0.5f);  // シーン遷移
                break;

            // ステージ３の処理を呼び出す
            case StageState.STAGE3:
                FadeAnimationSceneManager.Instance.LoadScene("Stage2Scene", 0.5f);  // シーン遷移
                break;

            // どれでもないときの処理
            default:
                Debug.Log("StageStateが設定されていない");
                break;
        }
    }
}
