/*
 * Copyright 2012 Google Inc. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package nts.swipesafe.fragment;

import com.google.android.youtube.player.YouTubeBaseActivity;
import com.google.android.youtube.player.YouTubeInitializationResult;
import com.google.android.youtube.player.YouTubePlayer;
import com.google.android.youtube.player.YouTubePlayerSupportFragment;
import com.google.android.youtube.player.YouTubePlayerView;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import nts.swipesafe.R;
import nts.swipesafe.common.Utils;

/**
 * A simple YouTube Android API demo application which shows how to create a simple application that
 * displays a YouTube Video in a {@link YouTubePlayerView}.
 * <p>
 * Note, to use a {@link YouTubePlayerView}, your activity must extend {@link YouTubeBaseActivity}.
 */
public class PlayerViewVideoFragment extends Fragment {
    private View view;
    private YouTubePlayer mPlayer;
    private String videoId;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_player_video, container, false);

        Bundle bundle = this.getArguments();
        if (bundle != null) {
            videoId = bundle.getString("VideoId");
        }

        YouTubePlayer youtubeView = (YouTubePlayer) view.findViewById(R.id.youtube_view);
        youtubeView.setFullscreen(false);
        youtubeView.loadVideo(videoId);
        youtubeView.play();

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        LibraryGenderFragment fragment = new LibraryGenderFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }
}
