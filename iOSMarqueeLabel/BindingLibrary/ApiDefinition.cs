using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

// @interface MarqueeLabel : UILabel
[BaseType (typeof(UILabel))]
interface MarqueeLabel
{
	// -(instancetype)initWithFrame:(CGRect)frame;
	[Export ("initWithFrame:")]
	IntPtr Constructor (CGRect frame);

	// -(instancetype)initWithFrame:(CGRect)frame rate:(CGFloat)pixelsPerSec andFadeLength:(CGFloat)fadeLength;
	[Export ("initWithFrame:rate:andFadeLength:")]
	IntPtr Constructor (CGRect frame, nfloat pixelsPerSec, nfloat fadeLength);

	// -(instancetype)initWithFrame:(CGRect)frame duration:(NSTimeInterval)scrollDuration andFadeLength:(CGFloat)fadeLength;
	[Export ("initWithFrame:duration:andFadeLength:")]
	IntPtr Constructor (CGRect frame, double scrollDuration, nfloat fadeLength);

	// -(void)minimizeLabelFrameWithMaximumSize:(CGSize)maxSize adjustHeight:(BOOL)adjustHeight;
	[Export ("minimizeLabelFrameWithMaximumSize:adjustHeight:")]
	void MinimizeLabelFrameWithMaximumSize (CGSize maxSize, bool adjustHeight);

	// @property (assign, nonatomic) UIViewAnimationOptions animationCurve;
	[Export ("animationCurve", ArgumentSemantic.Assign)]
	UIViewAnimationOptions AnimationCurve { get; set; }

	// @property (assign, nonatomic) BOOL labelize;
	[Export ("labelize")]
	bool Labelize { get; set; }

	// @property (assign, nonatomic) BOOL holdScrolling;
	[Export ("holdScrolling")]
	bool HoldScrolling { get; set; }

	// @property (assign, nonatomic) BOOL tapToScroll;
	[Export ("tapToScroll")]
	bool TapToScroll { get; set; }

	// @property (assign, nonatomic) MarqueeType marqueeType;
	[Export ("marqueeType", ArgumentSemantic.Assign)]
	MarqueeType MarqueeType { get; set; }

	// @property (assign, nonatomic) CGFloat scrollDuration;
	[Export ("scrollDuration", ArgumentSemantic.Assign)]
	nfloat ScrollDuration { get; set; }

	// @property (assign, nonatomic) CGFloat rate;
	[Export ("rate", ArgumentSemantic.Assign)]
	nfloat Rate { get; set; }

	// @property (assign, nonatomic) CGFloat leadingBuffer;
	[Export ("leadingBuffer", ArgumentSemantic.Assign)]
	nfloat LeadingBuffer { get; set; }

	// @property (assign, nonatomic) CGFloat trailingBuffer;
	[Export ("trailingBuffer", ArgumentSemantic.Assign)]
	nfloat TrailingBuffer { get; set; }

	// @property (assign, nonatomic) CGFloat continuousMarqueeExtraBuffer;
	[Export ("continuousMarqueeExtraBuffer", ArgumentSemantic.Assign)]
	nfloat ContinuousMarqueeExtraBuffer { get; set; }

	// @property (assign, nonatomic) CGFloat fadeLength;
	[Export ("fadeLength", ArgumentSemantic.Assign)]
	nfloat FadeLength { get; set; }

	// @property (assign, nonatomic) CGFloat animationDelay;
	[Export ("animationDelay", ArgumentSemantic.Assign)]
	nfloat AnimationDelay { get; set; }

	// -(void)restartLabel;
	[Export ("restartLabel")]
	void RestartLabel ();

	// -(void)resetLabel;
	[Export ("resetLabel")]
	void ResetLabel ();

	// -(void)pauseLabel;
	[Export ("pauseLabel")]
	void PauseLabel ();

	// -(void)unpauseLabel;
	[Export ("unpauseLabel")]
	void UnpauseLabel ();

	// -(void)triggerScrollStart;
	[Export ("triggerScrollStart")]
	void TriggerScrollStart ();

	// -(void)labelWillBeginScroll;
	[Export ("labelWillBeginScroll")]
	void LabelWillBeginScroll ();

	// -(void)labelReturnedToHome:(BOOL)finished;
	[Export ("labelReturnedToHome:")]
	void LabelReturnedToHome (bool finished);

	// @property (readonly, assign, nonatomic) BOOL isPaused;
	[Export ("isPaused")]
	bool IsPaused { get; }

	// @property (readonly, assign, nonatomic) BOOL awayFromHome;
	[Export ("awayFromHome")]
	bool AwayFromHome { get; }

	// +(void)restartLabelsOfController:(UIViewController *)controller;
	[Static]
	[Export ("restartLabelsOfController:")]
	void RestartLabelsOfController (UIViewController controller);

	// +(void)controllerViewDidAppear:(UIViewController *)controller;
	[Static]
	[Export ("controllerViewDidAppear:")]
	void ControllerViewDidAppear (UIViewController controller);

	// +(void)controllerViewWillAppear:(UIViewController *)controller;
	[Static]
	[Export ("controllerViewWillAppear:")]
	void ControllerViewWillAppear (UIViewController controller);

	// +(void)controllerViewAppearing:(UIViewController *)controller __attribute__((deprecated("Use restartLabelsOfController: method")));
	[Static]
	[Export ("controllerViewAppearing:")]
	void ControllerViewAppearing (UIViewController controller);

	// +(void)controllerLabelsShouldLabelize:(UIViewController *)controller;
	[Static]
	[Export ("controllerLabelsShouldLabelize:")]
	void ControllerLabelsShouldLabelize (UIViewController controller);

	// +(void)controllerLabelsShouldAnimate:(UIViewController *)controller;
	[Static]
	[Export ("controllerLabelsShouldAnimate:")]
	void ControllerLabelsShouldAnimate (UIViewController controller);
}
