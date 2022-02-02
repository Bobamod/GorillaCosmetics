﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics.UI
{
	public class ViewButton : GorillaPressableButton
	{
		ISelectionManager.SelectionView view;

		public float buttonFadeTime = 0.25f;

		WardrobeFunctionButton wardrobeFunctionButton;

		bool sendPress = false;

		public void Awake()
		{
			wardrobeFunctionButton = GetComponent<WardrobeFunctionButton>();
			wardrobeFunctionButton.enabled = false;

			pressedMaterial = wardrobeFunctionButton.pressedMaterial;
			unpressedMaterial = wardrobeFunctionButton.unpressedMaterial;
			buttonRenderer = wardrobeFunctionButton.buttonRenderer;
			debounceTime = wardrobeFunctionButton.debounceTime;
			myText = wardrobeFunctionButton.myText;
		}

		public void OnDestroy()
		{
			wardrobeFunctionButton.enabled = true;
		}

		public void Update()
		{

			if (sendPress)
			{
				sendPress = false;
				Plugin.SelectionManager.SetView(view);
			}
		}

		public void SetView(ISelectionManager.SelectionView view)
		{
			this.view = view;
			myText.text = view.ToString().ToUpper();
		}

		public override void ButtonActivation()
		{
			base.ButtonActivation();
			// Breaks when run from wierd stacks (e.g. physics tick)
			sendPress = true;
			StartCoroutine(ButtonColorUpdate());
		}

		public override void UpdateColor() { }

		private IEnumerator ButtonColorUpdate()
		{
			// Calling it here so DestroyImmediate works
			try
			{

			} catch (Exception e)
			{
				Debug.LogException(e);
			}
			buttonRenderer.material = pressedMaterial;
			yield return new WaitForSeconds(buttonFadeTime);
			buttonRenderer.material = unpressedMaterial;
		}
	}
}
