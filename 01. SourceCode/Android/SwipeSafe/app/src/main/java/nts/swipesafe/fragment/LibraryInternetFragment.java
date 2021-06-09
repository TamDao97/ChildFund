package nts.swipesafe.fragment;

import android.os.Bundle;
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

import nts.swipesafe.R;
import nts.swipesafe.adapter.InternetAdapter;
import nts.swipesafe.adapter.SkillAdapter;
import nts.swipesafe.adapter.YoutobeVideoAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.InternetModel;
import nts.swipesafe.model.SkillModel;
import nts.swipesafe.model.VideoModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class LibraryInternetFragment extends Fragment {
    private GlobalVariable global;
    private View view;
    private ImageView imgBack;
    private RecyclerView rvInternet;
    private InternetAdapter internetAdapter;
    private List<InternetModel> listInternet;
    private YouTubePlayer YPlayer;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_library_internet, container, false);
        global = (GlobalVariable) getActivity().getApplication();

        initComponent();

        listInternet = new ArrayList<>();

        viewInternetImage();

        if (global.ListInternetVideo == null || global.ListInternetVideo.size() == 0) {
            viewVideo();
        } else {
            listInternet.addAll(global.ListInternetVideo);
        }

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new LibraryFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        rvInternet = view.findViewById(R.id.rvInternet);
        rvInternet.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        rvInternet.setHasFixedSize(true);

        imgBack = view.findViewById(R.id.imgBack);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backFragment();
            }
        });
    }

    private void viewInternetImage() {
        List<InternetModel> listInternetImage = new ArrayList<>();
        if (global.ListInternetImage == null || global.ListInternetImage.size() == 0) {
            String webJson = Utils.ReadJSONFromAsset(getContext(), "internet.json");
            listInternetImage = new Gson().fromJson(webJson, new TypeToken<ArrayList<InternetModel>>() {
            }.getType());
        } else {
            listInternetImage.addAll(global.ListInternetImage);
        }

        listInternet.addAll(listInternetImage);
        internetAdapter = new InternetAdapter(getActivity(), listInternet);
        rvInternet.setAdapter(internetAdapter);
        internetAdapter.SetOnItemClickListener(new InternetAdapter.OnItemClickListener() {
            @Override
            public void onItemClick(View view, int position, final InternetModel model) {
                if (model.IsVideo) {
                    YouTubePlayerSupportFragment youTubePlayerFragment = YouTubePlayerSupportFragment.newInstance();
                    Bundle bundle = new Bundle();
                    bundle.putString("Name", "Internet");
                    Utils.ChangeFragment(getActivity(), youTubePlayerFragment, bundle);

                    youTubePlayerFragment.initialize(Constants.KeyYoutube, new YouTubePlayer.OnInitializedListener() {

                        @Override
                        public void onInitializationSuccess(YouTubePlayer.Provider arg0, YouTubePlayer youTubePlayer, boolean b) {
                            if (!b) {
                                YPlayer = youTubePlayer;
                                YPlayer.setFullscreen(false);
                                YPlayer.setShowFullscreenButton(false);
                                YPlayer.loadVideo(model.VideoId);
                                YPlayer.play();
                            }
                        }

                        @Override
                        public void onInitializationFailure(YouTubePlayer.Provider arg0, YouTubeInitializationResult arg1) {
                            // TODO Auto-generated method stub

                        }
                    });
                } else {
                    Fragment fragment = new LibraryInternetImageDetailFragment();
                    Bundle bundle = new Bundle();
                    bundle.putString("Title", model.Title);
                    bundle.putString("ListImage", model.ListImage);
                    Utils.ChangeFragment(getActivity(), fragment, bundle);
                }
            }
        });
    }

    private void viewVideo() {
        AndroidNetworking.get("https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&playlistId=" + Constants.KeyPlaylistIdATM + "&key=" + Constants.KeyYoutube)
                .setPriority(Priority.LOW)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            JSONArray listVideo = response.getJSONArray("items");
                            InternetModel internetModel;
                            for (int i = 0; i < listVideo.length(); i++) {
                                JSONObject itemVideo = listVideo.getJSONObject(i).getJSONObject("snippet");
                                internetModel = new InternetModel();
                                internetModel.Title = itemVideo.getString("title");
                                internetModel.ImageThumbnail = itemVideo.getJSONObject("thumbnails").getJSONObject("default").getString("url");
                                internetModel.DateTime = itemVideo.getString("publishedAt");
                                internetModel.VideoId = itemVideo.getJSONObject("resourceId").getString("videoId");
                                internetModel.IsVideo = true;
                                listInternet.add(internetModel);
                            }
                            internetAdapter.notifyDataSetChanged();
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
