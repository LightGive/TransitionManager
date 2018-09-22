#region License
//Copyright(c) 2017 Akase Matsuura
//https://github.com/LightGive/TransitionManager

//Permission is hereby granted, free of charge, to any person obtaining a
//copy of this software and associated documentation files (the
//"Software"), to deal in the Software without restriction, including
//without limitation the rights to use, copy, modify, merge, publish, 
//distribute, sublicense, and/or sell copies of the Software, and to
//permit persons to whom the Software is furnished to do so, subject to
//the following conditions:

//The above copyright notice and this permission notice shall be
//included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
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
	public const string TransitionShaderName = "LightGive/Unlit/TransitionShader";
	public const string ShaderParamTextureGradation = "_Gradation";
	public const string ShaderParamFloatInvert = "_Invert";
	public const string ShaderParamFloatCutoff = "_Cutoff";
	public const string ShaderParamColor = "_Color";
	private const float DefaultTransitionTime = 1.0f;

	[SerializeField]
	private TransitionType m_transitionType;
	[SerializeField]
	private float m_duration = DefaultTransitionTime;
	[SerializeField]
	private Texture m_ruleTex;
	[SerializeField]
	private Color m_texColor = Color.black;
	[SerializeField]
	private AnimationCurve m_animCurve = AnimationCurve.Linear(0, 0, 1, 1);
	[SerializeField]
	private Shader m_transShader;
	[SerializeField]
	private bool m_isInvert = false;

	private int m_texCount = 0;
	private bool m_isTransition = false;
	private Sprite m_transitionSprite;
	private Image m_transImage;
	private RawImage m_transRawImage;
	private CanvasScaler m_baseCanvasScaler;
	private Canvas m_baseCanvas;


	public bool isRawImage { get { return (m_transitionType == TransitionType.Custom); } }

	/// <summary>
	/// 遷移方法の種類
	/// </summary>
	public enum TransitionType
	{
		Fade = 0,

		Horizontal_Right = 1,
		Horizontal_Left = 2,

		Vertical_Top = 3,
		Vertical_Bottom = 4,

		Radial90_TopRight = 5,
		Radial90_TopLeft = 6,
		Radial90_BottomRight = 7,
		Radial90_BottomLeft = 8,

		Radial180_Right = 9,
		Radial180_Left = 10,
		Radial180_Top = 11,
		Radial180_Bottom = 12,

		Radial360_Right = 13,
		Radial360_Left = 14,
		Radial360_Top = 15,
		Radial360_Bottom = 16,

		Custom = 17,
	}

	protected override void Awake()
	{
		base.isDontDestroy = true;
		base.Awake();
		Init();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	void Init()
	{
		if (m_duration <= 0.0f)
			m_duration = DefaultTransitionTime;

		//Canvasを生成
		CreateCanvas();

		//RawImageを生成する
		CreateRawImage();

		//Imageを生成
		CreateImage();

		//真っ白なテクスチャを作成
		Texture2D plainTex = CreateTexture2D();

		//スプライト作成
		m_transitionSprite = Sprite.Create(plainTex, new Rect(0, 0, 32, 32), Vector2.zero);

		//スプライト設定
		m_transitionSprite.name = "TransitionSpeite";
		m_transImage.sprite = m_transitionSprite;
		m_transImage.type = Image.Type.Filled;
		m_transImage.fillAmount = 1.0f;

		switch (m_transitionType)
		{
			case TransitionType.Fade:
				m_transImage.fillAmount = 1.0f;
				break;

			case TransitionType.Custom:
				Material mat = new Material(m_transShader);
				m_transRawImage.material = mat;
				m_transRawImage.material.SetTexture(ShaderParamTextureGradation, m_ruleTex);
				m_transRawImage.material.SetFloat(ShaderParamFloatInvert, m_isInvert ? 1.0f : 0.0f);
				break;

			case TransitionType.Horizontal_Right:
				m_transImage.fillMethod = Image.FillMethod.Horizontal;
				m_transImage.fillOrigin = (int)Image.OriginHorizontal.Right;
				break;
			case TransitionType.Horizontal_Left:
				m_transImage.fillMethod = Image.FillMethod.Horizontal;
				m_transImage.fillOrigin = (int)Image.OriginHorizontal.Left;
				break;
			case TransitionType.Vertical_Top:
				m_transImage.fillMethod = Image.FillMethod.Vertical;
				m_transImage.fillOrigin = (int)Image.OriginVertical.Top;
				break;
			case TransitionType.Vertical_Bottom:
				m_transImage.fillMethod = Image.FillMethod.Vertical;
				m_transImage.fillOrigin = (int)Image.OriginVertical.Bottom;
				break;
			case TransitionType.Radial90_TopRight:
				m_transImage.fillMethod = Image.FillMethod.Radial90;
				m_transImage.fillOrigin = (int)Image.Origin90.TopRight;
				break;
			case TransitionType.Radial90_TopLeft:
				m_transImage.fillMethod = Image.FillMethod.Radial90;
				m_transImage.fillOrigin = (int)Image.Origin90.TopLeft;
				break;
			case TransitionType.Radial90_BottomRight:
				m_transImage.fillMethod = Image.FillMethod.Radial90;
				m_transImage.fillOrigin = (int)Image.Origin90.BottomRight;
				break;
			case TransitionType.Radial90_BottomLeft:
				m_transImage.fillMethod = Image.FillMethod.Radial90;
				m_transImage.fillOrigin = (int)Image.Origin90.BottomLeft;
				break;
			case TransitionType.Radial180_Right:
				m_transImage.fillMethod = Image.FillMethod.Radial180;
				m_transImage.fillOrigin = (int)Image.Origin180.Right;
				break;
			case TransitionType.Radial180_Left:
				m_transImage.fillMethod = Image.FillMethod.Radial180;
				m_transImage.fillOrigin = (int)Image.Origin180.Left;
				break;
			case TransitionType.Radial180_Top:
				m_transImage.fillMethod = Image.FillMethod.Radial180;
				m_transImage.fillOrigin = (int)Image.Origin180.Top;
				break;
			case TransitionType.Radial180_Bottom:
				m_transImage.fillMethod = Image.FillMethod.Radial180;
				m_transImage.fillOrigin = (int)Image.Origin180.Bottom;
				break;
			case TransitionType.Radial360_Right:
				m_transImage.fillMethod = Image.FillMethod.Radial360;
				m_transImage.fillOrigin = (int)Image.Origin360.Right;
				break;
			case TransitionType.Radial360_Left:
				m_transImage.fillMethod = Image.FillMethod.Radial360;
				m_transImage.fillOrigin = (int)Image.Origin360.Left;
				break;
			case TransitionType.Radial360_Top:
				m_transImage.fillMethod = Image.FillMethod.Radial360;
				m_transImage.fillOrigin = (int)Image.Origin360.Top;
				break;
			case TransitionType.Radial360_Bottom:
				m_transImage.fillMethod = Image.FillMethod.Radial360;
				m_transImage.fillOrigin = (int)Image.Origin360.Bottom;
				break;
		}
		m_transRawImage.gameObject.SetActive(false);
		m_transImage.gameObject.SetActive(false);
	}


	/// <summary>
	/// シーンを遷移する
	/// </summary>
	/// <param name="_sceneName">遷移先シーン名</param>
	public void LoadScene(string _sceneName)
	{
		StartCoroutine(TransitionAction(() => SceneManager.LoadScene(_sceneName), m_duration));
	}

	/// <summary>
	/// シーンを遷移する
	/// </summary>
	/// <param name="_sceneName">遷移先シーン名</param>
	/// <param name="_duration">遷移時間</param>
	public void LoadScene(string _sceneName, float _duration)
	{
		StartCoroutine(TransitionAction(() => SceneManager.LoadScene(_sceneName), _duration));
	}

	/// <summary>
	/// シーン再読み込み
	/// </summary>
	/// <param name="_duration">遷移時間</param>
	public void ReLoadScene(float _duration)
	{
		StartCoroutine(TransitionAction(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), _duration));
	}

	/// <summary>
	/// シーン再読み込み
	/// </summary>
	public void ReLoadScene()
	{
		StartCoroutine(TransitionAction(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), m_duration));
	}

	/// <summary>
	/// アプリを終了する
	/// </summary>
	/// <param name="_transtime">遷移時間</param>
	public void ApplicationQuit(float _transtime)
	{
		StartCoroutine(TransitionAction(() => Application.Quit(), _transtime));
	}

	/// <summary>
	/// アプリを終了する
	/// </summary>
	public void ApplicationQuit()
	{
		StartCoroutine(TransitionAction(() => Application.Quit(), m_duration));
	}

	public void StartTransitonEffect(float _transtime, UnityAction _act)
	{
		StartCoroutine(TransitionAction(_act, _transtime));
	}

	/// <summary>
	/// Canvasを生成する
	/// </summary>
	void CreateCanvas()
	{
		if (this.gameObject.GetComponent<Canvas>() != null)
			m_baseCanvas = this.gameObject.GetComponent<Canvas>();
		else
			m_baseCanvas = this.gameObject.AddComponent<Canvas>();

		m_baseCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		m_baseCanvas.sortingOrder = 999;
		m_baseCanvas.pixelPerfect = false;

		if (this.gameObject.GetComponent<CanvasScaler>() != null)
			m_baseCanvasScaler = this.gameObject.GetComponent<CanvasScaler>();
		else
			m_baseCanvasScaler = this.gameObject.AddComponent<CanvasScaler>();

		m_baseCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		m_baseCanvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
		m_baseCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
		m_baseCanvasScaler.referencePixelsPerUnit = 100.0f;

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
		m_transImage = transImageObj.AddComponent<Image>();
		m_transImage.color = m_texColor;
		m_transImage.sprite = null;
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
		m_transRawImage = transRawImageObj.AddComponent<RawImage>();
		m_transRawImage.color = m_texColor;
		m_transRawImage.texture = null;
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
	private IEnumerator TransitionAction(UnityAction _act, float _transtime)
	{
		//コルーチンが動いている時は中断
		if (m_isTransition)
			yield break;

		m_isTransition = true;
		SceneTransitionInit();

		float t = Time.time;
		float lp = 0.0f;

		while (Time.time - t < _transtime)
		{
			lp = m_animCurve.Evaluate((Time.time - t) / _transtime);
			SceneTransitionDirection(lp);
			yield return null;
		}

		//シーン遷移
		_act.Invoke();


		t = Time.time;
		lp = 0.0f;

		m_animCurve = FlipCurve();

		while (Time.time - t < _transtime)
		{
			lp = m_animCurve.Evaluate((Time.time - t) / _transtime);
			SceneTransitionDirection(lp);
			yield return null;
		}
		m_animCurve = FlipCurve();

		m_isTransition = false;

		if (isRawImage)
		{
			SceneTransitionDirection(0.0f);
			m_transRawImage.gameObject.SetActive(false);
		}
		else
		{
			m_transImage.fillAmount = 0.0f;
			m_transImage.gameObject.SetActive(false);
		}
	}


	/// <summary>
	/// アニメーションカーブを反転させる
	/// </summary>
	/// <returns>反転させたアニメーションカーブ</returns>
	private AnimationCurve FlipCurve()
	{
		AnimationCurve newCurve = new AnimationCurve();
		for (int i = 0; i < m_animCurve.length; i++)
		{
			Keyframe key = m_animCurve[i];
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
		switch (m_transitionType)
		{
			case TransitionType.Fade:
				m_transImage.color = new Color(m_texColor.r, m_texColor.g, m_texColor.b, _lerp);
				break;

			case TransitionType.Custom:
				m_transRawImage.material.SetFloat(ShaderParamFloatCutoff, _lerp);
				break;

			default:
				m_transImage.fillAmount = _lerp;
				break;
		}
	}

	/// <summary>
	/// シーン遷移前の演出ごとの初期化
	/// </summary>
	void SceneTransitionInit()
	{
		switch (m_transitionType)
		{
			case TransitionType.Fade:
				m_transImage.fillAmount = 1.0f;
				break;
			default:
				m_transImage.fillAmount = 0.0f;
				break;
		}

		if (isRawImage)
		{
			m_transRawImage.gameObject.SetActive(true);
		}
		else
		{
			m_transImage.gameObject.SetActive(true);
		}

	}

	/// <summary>
	/// シーン遷移後の演出後の処理
	/// </summary>
	void SceneTransitionEnd()
	{
		switch (m_transitionType)
		{
			case TransitionType.Fade:
				m_transImage.fillAmount = 1.0f;
				break;
			default:
				m_transImage.fillAmount = 0.0f;
				break;
		}
	}

}