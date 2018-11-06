using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example1 : MonoBehaviour
{
	//LoadScene
	[SerializeField]
	private Dropdown m_dropDownLoadSceneColor;
	[SerializeField]
	private Dropdown m_dropDownEffectType;
	[SerializeField]
	private InputField m_inputLoadSceneDuration;

	//Flash
	[SerializeField]
	private Dropdown m_dropDownFlashColor;
	[SerializeField]
	private InputField m_inputFlashFadeDuration;
	[SerializeField]
	private InputField m_inputFlashWhiteDuration;
	[SerializeField]
	private InputField m_inputFlashCount;

	private Color[] m_colors = new Color[]
	{
		Color.white,
		Color.black,
		Color.red,
		Color.green,
		Color.blue
	};

	private void Start()
	{
		//Set dropdown enum value
		m_dropDownEffectType.ClearOptions();
		string[] enumNames = System.Enum.GetNames(typeof(TransitionManager.EffectType));
		List<string> names = new List<string>(enumNames);
		m_dropDownEffectType.AddOptions(names);
	}

	public void OnButtonDownReloadScene()
	{
		var effectType = TransitionManager.Instance.defaultEffectType;
		var transDuration = TransitionManager.Instance.defaultTransDuration;
		var effectColor = TransitionManager.Instance.defaultEffectColor;

		if (m_dropDownEffectType.interactable)
			effectType = (TransitionManager.EffectType)m_dropDownEffectType.value;
		if (m_dropDownLoadSceneColor.interactable)
			effectColor = m_colors[m_dropDownLoadSceneColor.value];
		if (m_inputLoadSceneDuration.interactable)
			float.TryParse(m_inputLoadSceneDuration.text, out transDuration);

		//LoadSceneTransition
		TransitionManager.Instance.ReLoadScene(transDuration, effectType, effectColor);
	}

	public void OnButtonDownFlash()
	{
		var flashCount = 1;
		var fadeDuration = TransitionManager.Instance.defaultFlashDuration;
		var whiteDuration = TransitionManager.Instance.defaultFlashWhiteDuration;
		var flashColor = TransitionManager.Instance.defaultFlashColor;

		if (m_inputFlashFadeDuration.interactable)
			float.TryParse(m_inputFlashFadeDuration.text, out fadeDuration);
		if (m_inputFlashWhiteDuration.interactable)
			float.TryParse(m_inputFlashWhiteDuration.text, out whiteDuration);
		if (m_inputFlashCount.interactable)
			int.TryParse(m_inputFlashCount.text, out flashCount);
		if (m_dropDownFlashColor.interactable)
			flashColor = m_colors[m_dropDownFlashColor.value];

		//Flash
		TransitionManager.Instance.Flash(flashCount, fadeDuration, whiteDuration, flashColor);
	}
}
