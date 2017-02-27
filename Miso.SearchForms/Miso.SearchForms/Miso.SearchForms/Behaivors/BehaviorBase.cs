using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Miso.SearchForms.Behaivors
{
	//
	//https://github.com/PrismLibrary/Prism/tree/master/Source/Xamarin/Prism.Forms/Behaviors

	/// <summary>
	/// Base class that extends on Xamarin Forms Behaviors.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BehaviorBase<T> : Behavior<T> where T : BindableObject
	{
		/// <summary>
		/// The Object associated with the Behavior
		/// </summary>
		public T AssociatedObject { get; private set; }

		/// <inheritDoc />
		protected override void OnAttachedTo(T bindable)
		{
			base.OnAttachedTo(bindable);
			AssociatedObject = bindable;

			if (bindable.BindingContext != null)
			{
				BindingContext = bindable.BindingContext;
			}

			bindable.BindingContextChanged += OnBindingContextChanged;
		}

		/// <inheritDoc />
		protected override void OnDetachingFrom(T bindable)
		{
			base.OnDetachingFrom(bindable);
			bindable.BindingContextChanged -= OnBindingContextChanged;
			AssociatedObject = null;
		}

		void OnBindingContextChanged(object sender, EventArgs e)
		{
			OnBindingContextChanged();
		}

		/// <inheritDoc />
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			BindingContext = AssociatedObject.BindingContext;
		}
	}
}
