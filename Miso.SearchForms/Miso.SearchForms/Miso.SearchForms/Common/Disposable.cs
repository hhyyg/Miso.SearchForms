using System;
using System.Diagnostics;

namespace Miso.SearchForms.Common
{
	/// <summary>
	/// Rx に含まれてるクラスの模倣
	/// </summary>
	public static class Disposable
	{
		public struct ActionDisposable : IDisposable
		{
			public ActionDisposable(Action dispose)
			{
				Debug.Assert(dispose != null);

				_dispose = dispose;
			}
			private Action _dispose;
			public void Dispose() => _dispose();
		}

		public static ActionDisposable Create(Action dispose)
		{
			if (dispose == null)
				throw new ArgumentNullException(nameof(dispose));
			return new ActionDisposable(dispose);
		}
	}
}
