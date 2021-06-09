package nts.swipesafe.fragment;

import android.content.pm.ActivityInfo;
import android.os.Bundle;
import android.os.Message;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.android.youtube.player.YouTubeInitializationResult;
import com.google.android.youtube.player.YouTubePlayer;
import com.google.android.youtube.player.YouTubePlayerSupportFragment;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.MainActivity;
import nts.swipesafe.R;
import nts.swipesafe.adapter.YoutobeVideoAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.VideoModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class LibraryGenderFragment extends Fragment {
    private View view;
    private GlobalVariable global;
    private ImageView imgBack;
    private RecyclerView rvVideo;
    private YoutobeVideoAdapter youtobeVideoAdapter;
    private YouTubePlayer YPlayer;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_library_gender, container, false);
        global = (GlobalVariable) getActivity().getApplication();

        initComponent();

        try {
            if (global.ListGenderVideo == null || global.ListGenderVideo.size() == 0) {
                viewVideo();
            } else {
                youtobeVideoAdapter = new YoutobeVideoAdapter(getActivity(), global.ListGenderVideo);
                rvVideo.setAdapter(youtobeVideoAdapter);
                youtobeVideoAdapter.SetOnItemClickListener(new YoutobeVideoAdapter.OnItemClickListener() {
                    @Override
                    public void onItemClick(View view, int position, final VideoModel videoModel) {
                        YouTubePlayerSupportFragment youTubePlayerFragment = YouTubePlayerSupportFragment.newInstance();
                        Bundle bundle = new Bundle();
                        bundle.putString("Name", "Gender");
                        Utils.ChangeFragment(getActivity(), youTubePlayerFragment, bundle);

                        youTubePlayerFragment.initialize(Constants.KeyYoutube, new YouTubePlayer.OnInitializedListener() {

                            @Override
                            public void onInitializationSuccess(YouTubePlayer.Provider arg0, YouTubePlayer youTubePlayer, boolean b) {
                                if (!b) {
                                    YPlayer = youTubePlayer;
                                    YPlayer.setFullscreen(false);
                                    YPlayer.setShowFullscreenButton(false);
                                    YPlayer.loadVideo(videoModel.VideoId);
                                    YPlayer.play();
                                }
                            }

                            @Override
                            public void onInitializationFailure(YouTubePlayer.Provider arg0, YouTubeInitializationResult arg1) {
                                // TODO Auto-generated method stub

                            }
                        });
                    }
                });
            }
        } catch (Exception ex) {
        }

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new ChildKnowFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        rvVideo = view.findViewById(R.id.rvVideo);
        rvVideo.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        rvVideo.setHasFixedSize(true);

        imgBack = view.findViewById(R.id.imgBack);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backFragment();
            }
        });
    }

    private void viewVideo() {
        AndroidNetworking.get("https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&playlistId=" + Constants.KeyPlaylistId111 + "&key=" + Constants.KeyYoutube)
                .setPriority(Priority.LOW)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            JSONArray listVideo = response.getJSONArray("items");
                            List<VideoModel> listVideoResult = new ArrayList<>();
                            VideoModel videoModel;
                            for (int i = 0; i < listVideo.length(); i++) {
                                JSONObject itemVideo = listVideo.getJSONObject(i).getJSONObject("snippet");
                                videoModel = new VideoModel();
                                videoModel.Title = itemVideo.getString("title");
                                videoModel.ImageThumbnail = itemVideo.getJSONObject("thumbnails").getJSONObject("default").getString("url");
                                videoModel.DateTime = itemVideo.getString("publishedAt");
                                videoModel.VideoId = itemVideo.getJSONObject("resourceId").getString("videoId");
                                listVideoResult.add(videoModel);
                            }

                            youtobeVideoAdapter = new YoutobeVideoAdapter(getActivity(), listVideoResult);
                            rvVideo.setAdapter(youtobeVideoAdapter);
                            youtobeVideoAdapter.SetOnItemClickListener(new YoutobeVideoAdapter.OnItemClickListener() {
                                @Override
                                public void onItemClick(View view, int position, final VideoModel videoModel) {
                                    YouTubePlayerSupportFragment youTubePlayerFragment = YouTubePlayerSupportFragment.newInstance();
                                    Bundle bundle = new Bundle();
                                    bundle.putString("Name", "Gender");
                                    Utils.ChangeFragment(getActivity(), youTubePlayerFragment, bundle);

                                    youTubePlayerFragment.initialize(Constants.KeyYoutube, new YouTubePlayer.OnInitializedListener() {

                                        @Override
                                        public void onInitializationSuccess(YouTubePlayer.Provider arg0, YouTubePlayer youTubePlayer, boolean b) {
                                            if (!b) {
                                                YPlayer = youTubePlayer;
                                                YPlayer.setFullscreen(false);
                                                YPlayer.setShowFullscreenButton(false);
                                                YPlayer.loadVideo(videoModel.VideoId);
                                                YPlayer.play();
                                            }
                                        }

                                        @Override
                                        public void onInitializationFailure(YouTubePlayer.Provider arg0, YouTubeInitializationResult arg1) {
                                            // TODO Auto-generated method stub

                                        }
                                    });
                                }
                            });
                        } catch (Exception ex) {

                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        String eror = "";
                    }
                });
    }
}
