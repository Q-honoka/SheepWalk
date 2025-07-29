using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeAnimationSceneManager : SingletonMonoBehaviour<FadeAnimationSceneManager>
{
    private GameObject FadeAnimation;
    private Image _i;

    public bool FadeIn;
    public float interval;
    [Space]
    public bool FadeOut;

    private bool f;
    private string s;
    private float i;

    private float t;
    private bool animating_out;
    private bool animating_in;
    private void Start()
    {
        FadeAnimation = GameObject.Find("FadeAnimation");
        _i = FadeAnimation.GetComponent<Image>();
        t = 0;
        animating_in = true;
        if (FadeIn == true)
            _i.color = new Color32(0, 0, 0, 255);

        this.gameObject.transform.localScale = Vector3.zero;
    }
    private void Update()
    {
        FadeInAnimation();

        if (f == true && animating_in == false)
            FadeOutAnimation(s, i);
    }

    void FadeInAnimation()
    {
        if (FadeIn == true && animating_in == true)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            t += Time.deltaTime;
            byte a = (byte)(255 - 255 / (interval / t));
            a = (byte)a < 0 ? (byte)0 : a;
            _i.color = new Color32(0, 0, 0, a);

            if (interval < t)
            {
                gameObject.transform.localScale = new Vector3(0, 0, 0);
                animating_in = false;
                t = 0;
            }
        }
    }
    void FadeOutAnimation(string Scene_name, float Interval)
    {
        FadeAnimation = GameObject.Find("FadeAnimation");
        _i = FadeAnimation.GetComponent<Image>();
        //フェイドアウト時の処理
        if (FadeOut == true)
        {
            FadeAnimation.gameObject.transform.localScale = new Vector3(1, 1, 1);
            t += Time.deltaTime;
            //インターバルとtの割合でα色を計算して出す
            byte a = (byte)(255 / (Interval / t));
            a = (byte)255 < a ? (byte)255 : a;
            _i.color = new Color32(0, 0, 0, a);

            //アニメーションをしているかどうか
            if (t <= Interval) animating_out = true;
            else
            {
                animating_out = false;
                t = 0;
            }
        }

        //アニメーションが終わったらシーンを切り替える
        if (animating_out == false)
        {
            SceneManager.LoadScene(Scene_name);
        }
    }
    public void LoadScene(string Scene_name, float Interval)
    {
        //この名前のシーンがあるか調べる
        if (SceneUtility.GetBuildIndexByScenePath(Scene_name) == -1)
        {
            //エラーコード
            Debug.LogError(Scene_name + "は存在しないシーンです。");
        }
        else
        {
            s = Scene_name;
            i = Interval;
            f = true;
        }
    }
}