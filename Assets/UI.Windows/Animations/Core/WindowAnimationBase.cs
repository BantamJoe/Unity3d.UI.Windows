﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI.Windows.Animations {

	public class WindowAnimationBase : ScriptableObject {

		[System.Serializable]
		public class Transition {

			public TransitionBase transition;

			public void ApplyInputParameters(WindowComponentBase layoutElement) {

				var component = layoutElement.gameObject.AddComponent(System.Type.GetType("UnityEngine.UI.Windows.Animations." + this.transition.GetType().Name + "Parameters")) as TransitionInputParameters;
				component.SetDefaultParameters(this.GetDefaultInputParameters());
				layoutElement.animationInputParams.Add(component);

			}

			public TransitionBase.ParametersBase GetDefaultInputParameters() {

				return this.transition.GetDefaultInputParameters();

			}

			public void Play(WindowBase window, TransitionInputParameters parameters, WindowComponentBase root, bool forward, System.Action callback) {

				this.transition.Play(window, parameters, root, forward, callback);
				
			}
			
			public float GetDuration(TransitionInputParameters parameters, bool forward) {
				
				return this.transition.GetDuration(parameters, forward);

			}
			
			public void SetInState(TransitionInputParameters parameters, WindowBase window, WindowComponentBase root) {
				
				this.transition.SetInState(parameters, window, root);

			}
			
			public void SetOutState(TransitionInputParameters parameters, WindowBase window, WindowComponentBase root) {
				
				this.transition.SetOutState(parameters, window, root);

			}
			
			public void SetResetState(TransitionInputParameters parameters, WindowBase window, WindowComponentBase root) {

				this.transition.SetResetState(parameters, window, root);

			}

		}

		public List<Transition> transitions = new List<Transition>();

		public void ApplyInputParameters(WindowComponentBase layoutElement) {

			layoutElement.animationInputParams.Clear();
			foreach (var transition in this.transitions) {

				transition.ApplyInputParameters(layoutElement);

			}

		}
		
		public void SetInState(List<TransitionInputParameters> parameters, WindowBase window, WindowComponentBase root) {
			
			var i = 0;
			foreach (var transition in this.transitions) {
				
				transition.SetInState(parameters[i++], window, root);
				
			}

		}
		
		public void SetOutState(List<TransitionInputParameters> parameters, WindowBase window, WindowComponentBase root) {
			
			var i = 0;
			foreach (var transition in this.transitions) {
				
				transition.SetOutState(parameters[i++], window, root);
				
			}

		}
		
		public void SetResetState(List<TransitionInputParameters> parameters, WindowBase window, WindowComponentBase root) {
			
			var i = 0;
			foreach (var transition in this.transitions) {
				
				transition.SetResetState(parameters[i++], window, root);
				
			}

		}

		public void Play(WindowBase window, List<TransitionInputParameters> parameters, WindowComponentBase root, bool forward, System.Action callback) {

			Transition callbacker = null;
			var maxDuration = 0f;
			var i = 0;
			foreach (var transition in this.transitions) {

				var d = transition.GetDuration(parameters[i++], forward);
				if (d >= maxDuration) {

					maxDuration = d;
					callbacker = transition;

				}

			}

			i = 0;
			foreach (var transition in this.transitions) {

				transition.Play(window, parameters[i++], root, forward, callbacker == transition ? callback : null);

			}

			if (callbacker == null && callback != null) callback();

		}

		public float GetDuration(List<TransitionInputParameters> parameters, bool forward) {

			var maxDuration = 0f;
			var i = 0;
			foreach (var transition in this.transitions) {

				var d = transition.GetDuration(parameters[i++], forward);
				if (d > maxDuration) maxDuration = d;

			}

			return maxDuration;

		}
		
		#if UNITY_EDITOR
		[UnityEditor.MenuItem("Assets/Create/UI Windows/Animations/Default")]
		public static void CreateInstance() {
			
			ME.EditorUtilities.CreateAsset<WindowAnimationBase>();
			
		}
		#endif

	}

}