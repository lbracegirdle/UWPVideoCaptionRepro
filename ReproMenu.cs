//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

// This project is based on the UWP Samples project and attributed with thanks
// to the original developers, in creating this repro of the bug.
// https://github.com/microsoft/Windows-universal-samples/tree/e13cf5dca497ad661706d150a154830666913be4

using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using VideoPlayback;

namespace SDKTemplate
{
    public partial class MainPage : Page
    {
        public const string FEATURE_NAME = "Video Playback";

        List<Repros> scenarios = new List<Repros>
        {
            new Repros() { Title= "Repro: SRT & VTT Caption Position", ClassType=typeof(CaptionPositionRepro)}
        };
    }

    public class Repros
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }
}
