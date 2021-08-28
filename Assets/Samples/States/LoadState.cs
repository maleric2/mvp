﻿using maleric.Core.States;
using System;
using UnityEngine;

namespace maleric.Example
{
	public class LoadState : ALoadState
	{
		protected override void OnStateEnter()
		{
			Debug.LogError("Loading Enter");
		}

		protected override void OnStateExit()
		{
			Debug.LogError("Loading Exit");
		}
	}
}
