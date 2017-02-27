using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miso.SearchForms.Controls.SegmentedControlEvents
{
	/// <summary>
	/// SemgentControlOptionのTextが変更されたときのイベントのArgs
	/// 
	/// 変更後のIndexを取得したく、デフォルトのXamarin.Forms.TextChangedEventArgsではなく、このArgsを定義しました
	/// </summary>
	public class TextChangedEventArgs
	{
		public string OldText { get; set; }
		public string NewText { get; set; }
		/// <summary>
		/// 変更されたSegmentControlOptionの要素のIndex
		/// </summary>
		public int Index { get; set; }
	}
}
