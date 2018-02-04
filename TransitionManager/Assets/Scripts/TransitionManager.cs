using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// UnityのUIを使ったシーン遷移
/// フェードもあり。
/// </summary>
public class TransitionManager : LightGive.SingletonMonoBehaviour<TransitionManager>
{
    public enum TransitionType
    {
        Fade,

        Horizontal_Right,
        Horizontal_Left,

        Vertical_Top,
        Vertical_Bottom,

        Radial90_TopRight,
        Radial90_TopLeft,
        Radial90_BottomRight,
        Radial90_BottomLeft,

        Radial180_Right,
        Radial180_Left,
        Radial180_Top,
        Radial180_Bottom,

        Radial360_Right,
        Radial360_Left,
        Radial360_Top,
        Radial360_Bottom,
    }


    [SerializeField]
    private float transitionTime = 1.0f;
    [SerializeField]
    private Color transitionColor = Color.black;
    [SerializeField]
    private TransitionType transitionType = TransitionType.Fade;
    [SerializeField]
    private AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private float transTimeCnt = 0.0f;
    private bool isTransition = false;
    private RectTransform transImageRectTransfrm;
    private GameObject transImageObj;
    private Texture2D transTexture;
    private Sprite transSprite;
    private Image transImage;
    private CanvasScaler fadeCancasScaler;
    private Canvas fadeCanvas;


    protected override void Awake()
    {
        base.Awake();
        Init();
    }


    void Init()
    {
        if (transitionTime <= 0.0f)
            transitionTime = 0.0f;

        if (this.gameObject.GetComponent<Canvas>() != null)
            fadeCanvas = this.gameObject.GetComponent<Canvas>();
        else
            fadeCanvas = this.gameObject.AddComponent<Canvas>();

        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 999;
        fadeCanvas.pixelPerfect = false;

        if (this.gameObject.GetComponent<CanvasScaler>() != null)
            fadeCancasScaler = this.gameObject.GetComponent<CanvasScaler>();
        else
            fadeCancasScaler = this.gameObject.AddComponent<CanvasScaler>();

        fadeCancasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        fadeCancasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        fadeCancasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
        fadeCancasScaler.referencePixelsPerUnit = 100.0f;

        transImageObj = new GameObject("Transition Image");
        transImageObj.transform.SetParent(this.gameObject.transform);
        transImageObj.transform.position = Vector3.zero;
        transImageObj.transform.localPosition = Vector3.zero;
        transImageObj.transform.localScale = Vector3.one;
        transImageObj.transform.rotation = Quaternion.identity;

        transImage = transImageObj.AddComponent<Image>();
        transImage.color = transitionColor;
        transImage.sprite = null;

        transImageRectTransfrm = transImageObj.GetComponent<RectTransform>();
        transImageRectTransfrm.anchorMin = Vector2.zero;
        transImageRectTransfrm.anchorMax = Vector2.one;
        transImageRectTransfrm.pivot = new Vector2(0.5f, 0.5f);
        transImageRectTransfrm.localPosition = Vector3.zero;
        transImageRectTransfrm.sizeDelta = Vector3.zero;

        //CreateTexture
        transTexture = new Texture2D(32, 32, TextureFormat.RGB24, false);

        for (int i = 0; i < transTexture.width; i++)
        {
            for (int ii = 0; ii < transTexture.height; ii++)
            {
                transTexture.SetPixel(i, ii, Color.white);
            }
        }


        //CreateSprite
        transSprite = Sprite.Create(transTexture, new Rect(0, 0, 32, 32), Vector2.zero);
        transSprite.name = "TranstionName";

        //ImageSetting
        transImage.sprite = transSprite;
        transImage.type = Image.Type.Filled;
        transImage.fillAmount = 1.0f;

        SettingTransitionType(transitionType);
        transImageObj.SetActive(false);
        SceneTransitionInit();
    }

    /// <summary>
    /// SceneTransiton
    /// </summary>
    /// <param name="_sceneName">Scenename.</param>
    /// <param name="_transitionTime">Transtime.</param>
	public void LoadLevel(string _sceneName, float _transitionTime)
    {
        LoadLevel(_sceneName, _transitionTime, transitionType, transitionColor);
    }

    /// <summary>
    /// SceneTransition
    /// </summary>
    /// <param name="_sceneName">Scene name.</param>
    /// <param name="_transitionTime">Transition time.</param>
    /// <param name="_transitionType">Transition type.</param>
    /// <param name="_transitionColor">Transition color.</param>
    public void LoadLevel(string _sceneName, float _transitionTime, TransitionType _transitionType, Color _transitionColor)
    {
        StartCoroutine(SceneChange(_sceneName, _transitionTime, _transitionType, _transitionColor));
    }


    /// <summary>
    /// シーン遷移用コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SceneChange(string _scenename, float _transitionTime, TransitionType _transitionType, Color _transitionColor)
    {

        //コルーチンが動いている時、中断
        if (isTransition)
            yield break;

        isTransition = true;
        transImageObj.SetActive(true);
        SettingTransitionType(_transitionType);
        transImage.color = _transitionColor;
        SceneTransitionInit();

        //シーン遷移前
        float timeCnt = 0.0f;
        float lerp = 0.0f;
        while (timeCnt <= _transitionTime)
        {
            lerp = Mathf.Clamp(timeCnt / _transitionTime, 0.0f, 1.0f);
            SceneTransitionDirection(animationCurve.Evaluate(lerp));
            timeCnt += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //シーン遷移
        SceneManager.LoadScene(_scenename);

        timeCnt = 0.0f;
        while (timeCnt <= _transitionTime)
        {
            lerp = 1.0f - Mathf.Clamp(timeCnt / _transitionTime, 0.0f, 1.0f);
            SceneTransitionDirection(animationCurve.Evaluate(lerp));
            timeCnt += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isTransition = false;
        transImage.fillAmount = 0.0f;
        transImageObj.SetActive(false);
    }

    void SettingTransitionType(TransitionType _transitionType)
    {

        switch (_transitionType)
        {
            //Fade
            case TransitionType.Fade:
                transImage.fillAmount = 1.0f;
                break;

            //Horizontal
            case TransitionType.Horizontal_Right:
                transImage.fillMethod = Image.FillMethod.Horizontal;
                transImage.fillOrigin = (int)Image.OriginHorizontal.Right;
                break;
            case TransitionType.Horizontal_Left:
                transImage.fillMethod = Image.FillMethod.Horizontal;
                transImage.fillOrigin = (int)Image.OriginHorizontal.Left;
                break;

            //Vertical
            case TransitionType.Vertical_Top:
                transImage.fillMethod = Image.FillMethod.Vertical;
                transImage.fillOrigin = (int)Image.OriginVertical.Top;
                break;
            case TransitionType.Vertical_Bottom:
                transImage.fillMethod = Image.FillMethod.Vertical;
                transImage.fillOrigin = (int)Image.OriginVertical.Bottom;
                break;

            //Radial90
            case TransitionType.Radial90_TopRight:
                transImage.fillMethod = Image.FillMethod.Radial90;
                transImage.fillOrigin = (int)Image.Origin90.TopRight;
                break;
            case TransitionType.Radial90_TopLeft:
                transImage.fillMethod = Image.FillMethod.Radial90;
                transImage.fillOrigin = (int)Image.Origin90.TopLeft;
                break;
            case TransitionType.Radial90_BottomRight:
                transImage.fillMethod = Image.FillMethod.Radial90;
                transImage.fillOrigin = (int)Image.Origin90.BottomRight;
                break;
            case TransitionType.Radial90_BottomLeft:
                transImage.fillMethod = Image.FillMethod.Radial90;
                transImage.fillOrigin = (int)Image.Origin90.BottomLeft;
                break;

            //Radial180
            case TransitionType.Radial180_Right:
                transImage.fillMethod = Image.FillMethod.Radial180;
                transImage.fillOrigin = (int)Image.Origin180.Right;
                break;
            case TransitionType.Radial180_Left:
                transImage.fillMethod = Image.FillMethod.Radial180;
                transImage.fillOrigin = (int)Image.Origin180.Left;
                break;
            case TransitionType.Radial180_Top:
                transImage.fillMethod = Image.FillMethod.Radial180;
                transImage.fillOrigin = (int)Image.Origin180.Top;
                break;
            case TransitionType.Radial180_Bottom:
                transImage.fillMethod = Image.FillMethod.Radial180;
                transImage.fillOrigin = (int)Image.Origin180.Bottom;
                break;

            //Radial360
            case TransitionType.Radial360_Right:
                transImage.fillMethod = Image.FillMethod.Radial360;
                transImage.fillOrigin = (int)Image.Origin360.Right;
                break;
            case TransitionType.Radial360_Left:
                transImage.fillMethod = Image.FillMethod.Radial360;
                transImage.fillOrigin = (int)Image.Origin360.Left;
                break;
            case TransitionType.Radial360_Top:
                transImage.fillMethod = Image.FillMethod.Radial360;
                transImage.fillOrigin = (int)Image.Origin360.Top;
                break;
            case TransitionType.Radial360_Bottom:
                transImage.fillMethod = Image.FillMethod.Radial360;
                transImage.fillOrigin = (int)Image.Origin360.Bottom;
                break;

        }
    }

    /// <summary>
    /// シーン遷移中の演出
    /// </summary>
    /// <param name="_lerp"></param>
    void SceneTransitionDirection(float _lerp)
    {
        switch (transitionType)
        {
            case TransitionType.Fade:
                transImage.color = new Color(transitionColor.r, transitionColor.g, transitionColor.b, _lerp);
                break;

            default:
                transImage.fillAmount = _lerp;
                break;
        }
    }

    /// <summary>
    /// シーン遷移前の演出ごとの初期化
    /// </summary>
    void SceneTransitionInit()
    {
        switch (transitionType)
        {
            case TransitionType.Fade:
                transImage.fillAmount = 1.0f;
                break;
            default:
                transImage.fillAmount = 0.0f;
                break;
        }
    }

    /// <summary>
    /// シーン遷移後の演出後の処理
    /// </summary>
    void SceneTransitionEnd()
    {
        switch (transitionType)
        {
            case TransitionType.Fade:
                transImage.fillAmount = 1.0f;
                break;
            default:
                transImage.fillAmount = 0.0f;
                break;
        }
    }
}