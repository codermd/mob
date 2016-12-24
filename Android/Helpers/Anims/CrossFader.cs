using System;
using Android.Content.Res;
using Android.Views;
using Android.Animation;

namespace Mxp.Droid.Helpers
{
	public class CrossFader
	{
		private View mView1, mView2;
		private int mShortAnimationDuration;

		/**
		 * Instantiate a new CrossFader object.
		 * @param view1 the view to fade out
		 * @param view2 the view to fade in
		 * @param fadeDuration the duration in milliseconds for each fade to last
		 **/
		public CrossFader (View view1, View view2, int fadeDuration) {
			this.mView1 = view1;
			this.mView2 = view2;				
			this.mShortAnimationDuration = fadeDuration;
		}

		/**
		 * Start the cross-fade animation.
		 **/
		public void Start () {
			// Set the content view to 0% opacity but visible, so that it is visible
			// (but fully transparent) during the animation.
			this.mView2.Alpha = 0f;
			this.mView2.Visibility = ViewStates.Visible;

			// Animate the content view to 100% opacity, and clear any animation
			// listener set on the view.
			this.mView2.Animate ()
				.Alpha (1f)
				.SetDuration (this.mShortAnimationDuration)
				.SetListener (null);

			// Animate the loading view to 0% opacity.
			this.mView1.Animate ()
				.Alpha (0f)
				.SetDuration (this.mShortAnimationDuration)
				.SetListener (new CustomAnimatorListenerAdapter (this));
		}

		public class CustomAnimatorListenerAdapter : AnimatorListenerAdapter
		{
			private CrossFader mParent;

			public CustomAnimatorListenerAdapter (CrossFader parent) {
				this.mParent = parent;
			}

			// After the animation ends, set its visibility to GONE
			// as an optimization step (it won't participate in layout passes, etc.)
			public override void OnAnimationEnd (Animator animation) {
				this.mParent.mView1.Visibility = ViewStates.Gone;
			}
		}
	}
}