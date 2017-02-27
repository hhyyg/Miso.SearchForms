using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miso.SearchForms.Common
{
	public static class TaskApplicationExtensions
	{
		/// <summary>
		/// await する必要のない投げっぱなしのタスクに対して、そのタスク内で起きた例外を拾うためのメソッド。
		/// </summary>
		/// <param name="task">投げっぱなしにしたいタスク。</param>
		public static async void FireAndForget(this Task task)
		{
			if (task == null) return;

			try
			{
				await task;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw ex;
			}
		}
	}
}
