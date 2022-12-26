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

using SDKTemplate;
using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace VideoPlayback
{
    /// <summary>
    /// Demonstrates playing media with closed captions delivered out-of-band, specifically an MP4 video supplemented by external SRT files.
    /// </summary>
    public sealed partial class CaptionPositionRepro : Page
    {
        private MainPage rootPage;

        private Dictionary<TimedTextSource, Uri> ttsMap = new Dictionary<TimedTextSource, Uri>();

        public CaptionPositionRepro()
        {
            this.InitializeComponent();

            // Create the media source and supplement with external timed text sources
            var source = MediaSource.CreateFromUri(new Uri("https://personal-test-projects.s3.eu-west-2.amazonaws.com/videos/DurdleDoor.mp4"));

            var ttsSRTUri = new Uri("ms-appx:///Assets/TestCaption_srt.srt");
        
            var ttsSRT = TimedTextSource.CreateFromUri(ttsSRTUri);
            ttsMap[ttsSRT] = ttsSRTUri;

            var ttsVTTUri = new Uri("ms-appx:///Assets/TestCaption_vtt.vtt");
            var ttsVTT = TimedTextSource.CreateFromUri(ttsVTTUri);
            ttsMap[ttsVTT] = ttsVTTUri;


            ttsSRT.Resolved += Tts_Resolved;
            ttsVTT.Resolved += Tts_Resolved;

            source.ExternalTimedTextSources.Add(ttsVTT);
            source.ExternalTimedTextSources.Add(ttsSRT);
            

            // Create the playback item from the source
            var playbackItem = new MediaPlaybackItem(source);

            // Present the first track
            playbackItem.TimedMetadataTracksChanged += (sender, args) =>
            {
                playbackItem.TimedMetadataTracks.SetPresentationMode(0, TimedMetadataTrackPresentationMode.PlatformPresented);
            };
            
            // Set the source to start playback of the item
            this.mediaElement.SetPlaybackSource(playbackItem);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;
        }

        private void Tts_Resolved(TimedTextSource sender, TimedTextSourceResolveResultEventArgs args)
        {
            var ttsUri = ttsMap[sender];

            // Handle errors
            if(args.Error != null)
            {
                var ignoreAwaitWarning = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    rootPage.NotifyUser("Error resolving track " + ttsUri + " due to error " + args.Error.ErrorCode, NotifyType.ErrorMessage);
                });
                return;
            }

            // Update label manually since the VTT and SRT files do not contain them
            var ttsUriString = ttsUri.AbsoluteUri;
            if (ttsUriString.Contains("_srt"))
                args.Tracks[0].Label = "SRT";
            else if (ttsUriString.Contains("_vtt"))
                args.Tracks[0].Label = "WebVTT";
        }
    }
}
