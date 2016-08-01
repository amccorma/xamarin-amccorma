using System;
using ObjCRuntime;

[assembly: LinkWith ("libMarqueeLabel.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.Simulator64 | LinkTarget.Arm64, SmartLink = true, ForceLoad = true)]
