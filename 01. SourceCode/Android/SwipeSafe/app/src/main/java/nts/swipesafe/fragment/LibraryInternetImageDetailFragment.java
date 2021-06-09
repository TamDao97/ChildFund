package nts.swipesafe.fragment;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

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
import nts.swipesafe.adapter.ImageInternetAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.InternetModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class LibraryInternetImageDetailFragment extends Fragment {
    private View view;
    private ImageView imgBack;
    private RecyclerView rvInternet;
    private ImageInternetAdapter internetAdapter;
    private TextView txtTitle;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_library_internet_image_detail, container, false);

        initComponent();

        Bundle bundle = this.getArguments();
        if (bundle != null) {
            String title = bundle.getString("Title");
            txtTitle.setText(title);

            String listImage = bundle.getString("ListImage");
            if (!Utils.isEmpty(listImage))
                viewInternetImage(listImage.split(","));
        }

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new LibraryInternetFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        txtTitle = view.findViewById(R.id.txtTitle);
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

    private void viewInternetImage(String[] listImage) {
        internetAdapter = new ImageInternetAdapter(getActivity(), listImage);
        rvInternet.setAdapter(internetAdapter);
    }
}
