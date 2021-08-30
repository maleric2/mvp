using maleric.MVP.Service;
using maleric.MVP.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace maleric.MVP.Common
{
	/// <summary>
	/// Creates Initial router and destroy itself
	/// </summary>
	public class Kickstarter : MonoBehaviour
	{
		public ARouter RouterPrefab;

		public void Awake()
		{
			var router = Instantiate<ARouter>(RouterPrefab);
			PostInitializeRouter(router);

			Destroy(this);
		}

		/// <summary>
		/// Override this method to change state or call other stuff from router
		/// </summary>
		/// <param name="router"></param>
		protected virtual void PostInitializeRouter(ARouter router) { }

	}
}