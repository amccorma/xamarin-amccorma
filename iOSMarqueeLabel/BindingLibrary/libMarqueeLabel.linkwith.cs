using System;
using ObjCRuntime;

[assembly: LinkWith ("libMarqueeLabel.a", LinkTarget.Simulator | LinkTarget.ArmV7, SmartLink = true, ForceLoad = true)]
