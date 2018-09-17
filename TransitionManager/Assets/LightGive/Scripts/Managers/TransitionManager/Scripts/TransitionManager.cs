using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using LightGive;

/// <summary>
/// UnityのUIを使ったシーン遷移
/// フェードもあり。
/// </summary>
public class TransitionManager : LightGive.SingletonMonoBehaviour<TransitionManager>
{
	private const float DefaultTransitionTime = 1.0f;
	private const string ShaderParamFloatGradation = "_Gradation";
	private const string ShaderParamFloatInvert = "_Invert";
	private const string ShaderParamFloatCutoff = "_Cutoff";

	[SerializeField]
	private Texture m_transTex;
	[SerializeField]
	private Shader m_transShader;
	[SerializeField]
	private Color m_transColor = Color.black;
	[SerializeField]
	private float m_transDuration = DefaultTransitionTime;
	[SerializeField]
	private AnimationCurve m_transCurve = AnimationCurve.Linear(0, 0, 1, 1);
	[SerializeField]
	private float transAlphaCutOff = 0.0f;
	[SerializeField]
	private bool transIsInvert = false;

	public Texture TransTex
	{
		get { return m_transTex; }
		set { m_transTex = value; }
	}

	public Shader TransShader
	{
		get { return m_transShader; }
		set { m_transShader = value; }
	}

	public float TransAlphaCutOff
	{
		get { return transAlphaCutOff; }
		set { transAlphaCutOff = value; }
	}

	public AnimationCurve TransCurve
	{
		get { return m_transCurve; }
		set { m_transCurve = value; }
	}

	public bool TransIsInvert
	{
		get { return transIsInvert; }
		set { transIsInvert = value; }
	}

	private int imageCnt = 0;
	private bool isTransition = false;
	private Sprite transSprite;
	private Image transImage;
	private RawImage transRawImage;
	private CanvasScaler fadeCancasScaler;
	private Canvas fadeCanvas;

	public TransitionType transType;

	//遷移方法の種類をまとめたもの
	public enum TransitionType
	{
		Fade,

		Custom,

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

	protected override void Awake()
	{
		base.Awake();
		Init();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	void Init()
	{
		if (m_transDuration <= 0.0f)
			m_transDuration = DefaultTransitionTime;

		//Canvasを生成
		CreateCanvas();

		//RawImageを生成する
		CreateRawImage();

		//Imageを生成
		CreateImage();

		//真っ白なテクスチャを作成
		Texture2D plainTex = CreateTexture2D();

		//スプライト作成
		transSprite = Sprite.Create(plainTex, new Rect(0, 0, 32, 32), Vector2.zero);

		//スプライト設定
		transSprite.name = "TransitionSpeite";
		transImage.sprite = transSprite;
		transImage.type = Image.Type.Filled;
		transImage.fillAmount = 1.0f;

		m_transShader = Shader.Find("LightGive/Unlit/TransitionShader");
		if (m_transShader == null)
		{
			Debug.Log("I could not find a shader.");
			transType = TransitionType.Fade;
		}

		switch (transType)
		{
			case TransitionType.Fade:
				transImage.fillAmount = 1.0f;
				break;

			case TransitionType.Custom:
				Material mat = new Material(m_transShader);
				transRawImage.material = mat;
				transRawImage.material.SetTexture(ShaderParamFloatGradation, m_transTex);
				transRawImage.material.SetFloat(ShaderParamFloatInvert, transIsInvert ? 1.0f : 0.0f);
				break;

			case TransitionType.Horizontal_Right:
				transImage.fillMethod = Image.FillMethod.Horizontal;
				transImage.fillOrigin = (int)Image.OriginHorizontal.Right;
				break;
			case TransitionType.Horizontal_Left:
				transImage.fillMethod = Image.FillMethod.Horizontal;
				transImage.fillOrigin = (int)Image.OriginHorizontal.Left;
				break;
			case TransitionType.Vertical_Top:
				transImage.fillMethod = Image.FillMethod.Vertical;
				transImage.fillOrigin = (int)Image.OriginVertical.Top;
				break;
			case TransitionType.Vertical_Bottom:
				transImage.fillMethod = Image.FillMethod.Vertical;
				transImage.fillOrigin = (int)Image.OriginVertical.Bottom;
				break;
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

		transRawImage.gameObject.SetActive(false);
		transImage.gameObject.SetActive(false);

	}


	/// <summary>
	/// シーンを遷移する
	/// </summary>
	/// <param name="_scenename">遷移先シーン名</param>
	public void LoadLevel(string _scenename)
	{
		StartCoroutine(StartTransitionEffect(() => SceneManager.LoadScene(_scenename), m_transDuration));
	}

	/// <summary>
	/// シーンを遷移する
	/// </summary>
	/// <param name="_scenename">遷移先シーン名</param>
	/// <param name="_transtime">遷移時間</param>
	public void LoadLevel(string _scenename, float _transtime)
	{
		StartCoroutine(StartTransitionEffect(() => SceneManager.LoadScene(_scenename), _transtime));
	}

	/// <summary>
	/// シーン再読み込み
	/// </summary>
	/// <param name="_transtime">遷移時間</param>
	public void ReLoadLevel(float _transtime)
	{
		StartCoroutine(StartTransitionEffect(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), _transtime));
	}

	/// <summary>
	/// シーン再読み込み
	/// </summary>
	public void ReLoadLevel()
	{
		StartCoroutine(StartTransitionEffect(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), m_transDuration));
	}

	/// <summary>
	/// アプリを終了する
	/// </summary>
	/// <param name="_transtime">遷移時間</param>
	public void Quit(float _transtime)
	{
		StartCoroutine(StartTransitionEffect(() => Application.Quit(), _transtime));
	}

	/// <summary>
	/// アプリを終了する
	/// </summary>
	public void Quit()
	{
		StartCoroutine(StartTransitionEffect(() => Application.Quit(), m_transDuration));
	}

	public void StartTransitonEffect(float _transtime, UnityAction _act)
	{
		StartCoroutine(StartTransitionEffect(_act, _transtime));

	}

	/// <summary>
	/// Canvasを生成する
	/// </summary>
	void CreateCanvas()
	{
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

	}

	/// <summary>
	/// Imageを生成
	/// </summary>
	void CreateImage()
	{
		GameObject transImageObj;
		transImageObj = new GameObject("Transition Image");
		transImageObj.transform.SetParent(this.gameObject.transform);
		transImageObj.transform.position = Vector3.zero;
		transImageObj.transform.localPosition = Vector3.zero;
		transImageObj.transform.localScale = Vector3.one;
		transImageObj.transform.rotation = Quaternion.identity;
		transImage = transImageObj.AddComponent<Image>();
		transImage.color = m_transColor;
		transImage.sprite = null;
		RectTransform transImageRectTransfrm;
		transImageRectTransfrm = transImageObj.GetComponent<RectTransform>();
		transImageRectTransfrm.anchorMin = Vector2.zero;
		transImageRectTransfrm.anchorMax = Vector2.one;
		transImageRectTransfrm.pivot = new Vector2(0.5f, 0.5f);
		transImageRectTransfrm.localPosition = Vector3.zero;
		transImageRectTransfrm.sizeDelta = Vector3.zero;
	}


	/// <summary>
	/// RawImageを生成する
	/// </summary>
	void CreateRawImage()
	{
		GameObject transRawImageObj;
		transRawImageObj = new GameObject("Transition Raw Image");
		transRawImageObj.transform.SetParent(this.gameObject.transform);
		transRawImageObj.transform.position = Vector3.zero;
		transRawImageObj.transform.localPosition = Vector3.zero;
		transRawImageObj.transform.localScale = Vector3.one;
		transRawImageObj.transform.rotation = Quaternion.identity;
		transRawImage = transRawImageObj.AddComponent<RawImage>();
		transRawImage.color = m_transColor;
		transRawImage.texture = null;
		RectTransform transRawImageRectTransfrm;
		transRawImageRectTransfrm = transRawImageObj.GetComponent<RectTransform>();
		transRawImageRectTransfrm.anchorMin = Vector2.zero;
		transRawImageRectTransfrm.anchorMax = Vector2.one;
		transRawImageRectTransfrm.pivot = new Vector2(0.5f, 0.5f);
		transRawImageRectTransfrm.localPosition = Vector3.zero;
		transRawImageRectTransfrm.sizeDelta = Vector3.zero;

	}


	/// <summary>
	/// 真っ白なテクスチャを作成する
	/// </summary>
	/// <returns>真っ白なテクスチャ</returns>
	Texture2D CreateTexture2D()
	{
		var tex = new Texture2D(32, 32, TextureFormat.RGB24, false);

		for (int i = 0; i < tex.width; i++)
		{
			for (int ii = 0; ii < tex.height; ii++)
			{
				tex.SetPixel(i, ii, Color.white);
			}
		}

		return tex;
	}

	/// <summary>
	/// シーン遷移用コルーチン
	/// </summary>
	/// <returns></returns>
	private IEnumerator StartTransitionEffect(UnityAction _act, float _transtime)
	{
		//コルーチンが動いている時、中断
		if (isTransition)
			yield break;

		isTransition = true;
		SceneTransitionInit();

		float t = Time.time;
		float lp = 0.0f;

		while (Time.time - t < _transtime)
		{
			lp = m_transCurve.Evaluate((Time.time - t) / _transtime);
			SceneTransitionDirection(lp);
			yield return null;
		}

		//シーン遷移
		_act.Invoke();


		t = Time.time;
		lp = 0.0f;

		m_transCurve = FlipCurve();

		while (Time.time - t < _transtime)
		{
			lp = m_transCurve.Evaluate((Time.time - t) / _transtime);
			SceneTransitionDirection(lp);
			yield return null;
		}
		m_transCurve = FlipCurve();

		isTransition = false;

		if (IsRawImage())
		{
			SceneTransitionDirection(0.0f);
			transRawImage.gameObject.SetActive(false);
		}
		else
		{
			transImage.fillAmount = 0.0f;
			transImage.gameObject.SetActive(false);
		}
	}




	/// <summary>
	/// アニメーションカーブを反転させる
	/// </summary>
	/// <returns>反転させたアニメーションカーブ</returns>
	AnimationCurve FlipCurve()
	{
		AnimationCurve newCurve = new AnimationCurve();

		for (int i = 0; i < m_transCurve.length; i++)
		{
			Keyframe key = m_transCurve[i];
			key.time = 1f - key.time;
			key.inTangent = key.inTangent * -1f;
			key.outTangent = key.outTangent * -1f;
			newCurve.AddKey(key);
		}

		return newCurve;
	}

	/// <summary>
	/// シーン遷移中の演出
	/// </summary>
	/// <param name="_lerp"></param>
	void SceneTransitionDirection(float _lerp)
	{
		switch (transType)
		{
			case TransitionType.Fade:
				transImage.color = new Color(m_transColor.r, m_transColor.g, m_transColor.b, _lerp);
				break;

			case TransitionType.Custom:
				transRawImage.material.SetFloat(ShaderParamFloatCutoff, _lerp);
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
		switch (transType)
		{
			case TransitionType.Fade:
				transImage.fillAmount = 1.0f;
				break;
			default:
				transImage.fillAmount = 0.0f;
				break;
		}

		if (IsRawImage())
		{
			transRawImage.gameObject.SetActive(true);
		}
		else
		{
			transImage.gameObject.SetActive(true);
		}

	}

	/// <summary>
	/// シーン遷移後の演出後の処理
	/// </summary>
	void SceneTransitionEnd()
	{
		switch (transType)
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
	/// RawImageを使っているか
	/// </summary>
	/// <returns><c>true</c>, if raw image was ised, <c>false</c> otherwise.</returns>
	bool IsRawImage()
	{
		var isRawImage = false;
		switch (transType)
		{
			case TransitionType.Custom:
				isRawImage = true;
				break;
			default:
				isRawImage = false;
				break;
		}
		return isRawImage;
	}
}