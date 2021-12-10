using maleric.MVP.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace maleric.MVP.States
{
	public interface ILoadState : IBaseState
	{
		public Task LoadScene(string sceneName);
		event Action OnTargetStateLoaded;
	}

	public abstract class ALoadState : ILoadState
	{
		public const int FAIL_SAFE_ACTIVATION_WAIT_MS = 500;
		public const float MIN_SCENE_LOAD_PROGRESS = 0.9f;

		public bool IsActive { get; private set; }

		public event Action OnTargetStateLoaded;

		private string _targetSceneName;

		private CancellationTokenSource _cts;

		private AsyncOperation _asyncOperation;

		public void Enter()
		{
			IsActive = true;
			OnStateEnter();
		}

		public void Exit()
		{
			OnStateExit();
			IsActive = false;
		}

		public Task PrepareToEnter()
		{
			return Task.CompletedTask;
		}

		protected abstract void OnStateEnter();

		protected abstract void OnStateExit();

		public async Task LoadScene(string sceneName)
		{
			if (_cts == null)
			{
				_cts = new CancellationTokenSource();
				try
				{
					await PerformSceneLoading(_cts.Token, sceneName);
				}
				catch (OperationCanceledException ex)
				{
					if (ex.CancellationToken == _cts.Token)
					{
						//Perform operation after cancelling
						Debug.Log("Task cancelled");
					}
				}
				finally
				{
					OnTargetStateLoaded?.Invoke();

					_cts.Cancel();
					_cts = null;
				}
			}
			else
			{
				//Cancel Previous token
				_cts.Cancel();
				_cts = null;
			}
		}

		private async Task PerformSceneLoading(CancellationToken token, string sceneName)
		{
			await Task.Yield();
			token.ThrowIfCancellationRequested();
			if (token.IsCancellationRequested)
				return;

			_asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
			_asyncOperation.allowSceneActivation = false;

			while (true)
			{
				OnProgressChanged(_asyncOperation.progress);

				token.ThrowIfCancellationRequested();
				if (token.IsCancellationRequested)
					return;

				if (_asyncOperation.progress >= MIN_SCENE_LOAD_PROGRESS)
					break;
			}

			_asyncOperation.allowSceneActivation = true;
			OnProgressChanged(1f);
			// Failsafe wait for everything to be ready
			await Task.Delay(FAIL_SAFE_ACTIVATION_WAIT_MS);

			_cts.Cancel();
			token.ThrowIfCancellationRequested();

			//added this as a failsafe unnecessary
			if (token.IsCancellationRequested)
				return;
		}

		protected virtual void OnProgressChanged(float progress)
		{
			Debug.LogError(((progress * 100).ToString("F0") + "%"));
		}
	}
}
