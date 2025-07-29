using UnityEngine;

public enum Direction
{
    Left,
    Up,
    Right,
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
    private Vector3 dir, position;
    private float distance = 0.15f;
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

        // 羊のy座標が地面より下になったら
        if(transform.position.y < 0)
        {
            // 移動をやめる
            MoveStart = false;
            // リザルト状態を変更してシーン遷移
            gameManager.SetCurrentResultState(ResultState.RIVER);

        }
    }

    private void SheepMove()
    {
        // 移動フラグがfalseなら処理しない
        if(MoveStart != true)
        {
            return;
        }

        transform.Translate(MoveSpeed * Time.deltaTime, 0.0f, 0.0f);
    }

    private void SheepJump()
    {
        if(JumpStart == false)
        {
            return;
        }

        if(CheckGround() == true)
        {
            Debug.Log("ジャンプ");
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

    private bool CheckGround()
    {
        // Rayの方向。足下なのでdown
        dir = Vector3.down;
        // Rayの始点。原点 + offset
        position = transform.position + offset;
        Ray ray = new Ray(position, dir);
        // RayをGizmoで確認するためのDrawRay
        Debug.DrawRay(position, dir * distance, Color.red);

        return Physics.Raycast(ray, distance, groundLayer);
    }

    // 羊が当たったら
    private void OnCollisionEnter(Collision collision)
    {
        // 羊とゴールが衝突したら
        if (collision.gameObject.CompareTag("goal"))
        {
            // 移動をやめる
            MoveStart = false;
            // リザルト状態を変更してシーン遷移
            gameManager.SetCurrentResultState(ResultState.CLEAR);
        }
        // 羊とフェンスが衝突したら
        else if (collision.gameObject.CompareTag("fence"))
        {
            // 移動をやめる
            MoveStart = false;
            // リザルト状態を変更してシーン遷移
            gameManager.SetCurrentResultState(ResultState.COLLISION);
        }
        // 羊と左に曲がるフェンスが衝突したら
        else if(collision.gameObject.CompareTag("left"))
        {
            if (direction == Direction.Up)
            {
                direction = Direction.Left;
                // 向きを変える
                transform.rotation *= Quaternion.Euler(0, -90, 0);
            }
            else if (direction == Direction.Right)
            {
                direction = Direction.Up;
                // 向きを変える
                transform.rotation *= Quaternion.Euler(0, -90, 0);
            }
        }
        // 羊と右に曲がるフェンスが衝突したら
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
        // 池と接触したら
        if(other.gameObject.CompareTag("puddle"))
        {
            MoveSpeed = 1f;
            // リザルト状態を変更してシーン遷移
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

    public bool GetJumpStart()
    {
        return JumpStart;
    }
}
