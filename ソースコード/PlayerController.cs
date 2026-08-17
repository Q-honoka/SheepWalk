using UnityEngine;

// プレイヤーの向き
public enum Direction
{
    Left,   // 左
    Up,     // 正面
    Right,  // 右
    All,
}

public class PlayerController : MonoBehaviour
{
    [Header("地面のレイヤー")]
    [SerializeField] LayerMask groundLayer;

    [Header("ジャンプ力")]
    [SerializeField] float JumpPower;

    [Header("移動スピード")]
    [SerializeField] float MoveSpeed;

    public AudioClip audioClipWalk;
    public AudioClip audioClipJump;

    private Direction direction;    // 移動方向
    private bool MoveStart;     // 移動フラグ
    private bool JumpStart;     // ジャンプフラグ
    private Vector3 dir, position;  // 方向と位置
    private float distance = 0.15f; // 地面との接地を調べる距離
    private Vector3 offset = new Vector3(0, 0.1f, 0f);
    Rigidbody rb;   // 羊のRigidbody

    private GameManager gameManager;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        MoveStart = false;
        JumpStart = false;
        rb = GetComponent<Rigidbody>();
        gameManager = GameManager.instance;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SheepMove();

        SheepJump();

        // 羊のy座標が地面より下になったら、ゲームオーバーとしてリザルトへ遷移する
        if(transform.position.y < 0)
        {
            MoveStart = false;
            gameManager.SetCurrentResultState(ResultState.RIVER);

        }
    }

    /// <summary>
    /// 羊の移動処理
    /// </summary>
    private void SheepMove()
    {
        // 移動フラグがfalseなら処理しない
        if(MoveStart != true)
        {
            return;
        }

        transform.Translate(MoveSpeed * Time.deltaTime, 0.0f, 0.0f);
    }

    /// <summary>
    /// 羊のジャンプ処理
    /// </summary>
    private void SheepJump()
    {
        if(JumpStart == false)
        {
            return;
        }

        // 地面と接地していたらジャンプする
        if(CheckGround() == true)
        {
            audioSource.PlayOneShot(audioClipJump);
            JumpStart = false;
            rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
        }
        else
        {
            JumpStart = true;
            return;
        }

    }

    /// <summary>
    /// 地面と接地しているか調べる
    /// </summary>
    /// <returns>接地しているか（true：接地している, false：接地していない）</returns>
    private bool CheckGround()
    {
        dir = Vector3.down;
        position = transform.position + offset;
        // 現在地から下方向へRayをとばす
        Ray ray = new Ray(position, dir);

        return Physics.Raycast(ray, distance, groundLayer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 羊とゴールが衝突したら、ゲームクリア
        if (collision.gameObject.CompareTag("goal"))
        {
            MoveStart = false;
            gameManager.SetCurrentResultState(ResultState.CLEAR);
        }
        // 羊とフェンスが衝突したら、ゲームオーバー
        else if (collision.gameObject.CompareTag("fence"))
        {
            MoveStart = false;
            gameManager.SetCurrentResultState(ResultState.COLLISION);
        }
        // 羊と左に曲がるフェンスが衝突したら、方向を変える
        else if(collision.gameObject.CompareTag("left"))
        {
            if (direction == Direction.Up)
            {
                direction = Direction.Left;
                transform.rotation *= Quaternion.Euler(0, -90, 0);
            }
            else if (direction == Direction.Right)
            {
                direction = Direction.Up;
                transform.rotation *= Quaternion.Euler(0, -90, 0);
            }
        }
        // 羊と右に曲がるフェンスが衝突したら、方向を変える
        else if (collision.gameObject.CompareTag("right"))
        {
            if (direction == Direction.Up)
            {
                direction = Direction.Right;
                transform.rotation *= Quaternion.Euler(0, 90, 0);
            }
            else if (direction == Direction.Left)
            {
                direction = Direction.Up;
                transform.rotation *= Quaternion.Euler(0, 90, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 池と接触したら、ゲームオーバー
        if(other.gameObject.CompareTag("puddle"))
        {
            MoveSpeed = 1f;
            gameManager.SetCurrentResultState(ResultState.WETTING);
        }
    }

    /// <summary>
    /// 羊の移動フラグをtrueにする
    /// </summary>
    public void SetMoveStart(Direction dir)
    {
        // 方向転換
        direction = dir;

        // すでに歩いているなら処理をとばす
        if (MoveStart == true)
        {
            return;
        }

        audioSource.PlayOneShot(audioClipWalk);
        MoveStart = true;
    }

    /// <summary>
    /// ジャンプフラグをtrueにする
    /// </summary>
    public void SetJumpStart()
    {
        JumpStart = true;
    }

    /// <summary>
    /// 移動フラグを返す関数
    /// </summary>
    /// <returns></returns>
    public bool GetMoveStart()
    {
        return MoveStart;
    }
}
