package nts.swipesafe.fragment;

import android.app.DownloadManager;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.adapter.RightAdapter;
import nts.swipesafe.adapter.SkillAdapter;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.RightModel;
import nts.swipesafe.model.SkillModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class LibraryRightFragment extends Fragment {
    private View view;
    private ImageView imgBack;
    private RecyclerView rvRight;
    private RightAdapter rightAdapter;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_library_right, container, false);

        initComponent();

        viewRight();
        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new LibraryFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {

        imgBack = view.findViewById(R.id.imgBack);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backFragment();
            }
        });

        rvRight = view.findViewById(R.id.rvRight);
        rvRight.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        rvRight.setHasFixedSize(true);
    }

    private void viewRight() {
        String webJson = Utils.ReadJSONFromAsset(getContext(), "right.json");
        List<RightModel> listRight = new Gson().fromJson(webJson, new TypeToken<ArrayList<RightModel>>() {
        }.getType());
        rightAdapter = new RightAdapter(getActivity(), listRight);
        rvRight.setAdapter(rightAdapter);
        rightAdapter.SetOnItemClickListener(new RightAdapter.OnItemClickListener() {
            @Override
            public void onItemClick(View view, int position, RightModel rightModel) {
                DowloadFile(rightModel.FileDownload);
            }
        });
    }

    private void DowloadFile(String fileDownload) {
        String fileName = fileDownload.substring(fileDownload.lastIndexOf('/') + 1);
        DownloadManager.Request request = new DownloadManager.Request(Uri.parse(fileDownload));
        request.setDescription(fileName);
        request.setTitle("SwipeSafe");
// in order for this if to run, you must use the android 3.2 to compile your app
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) {
            request.allowScanningByMediaScanner();
            request.setNotificationVisibility(DownloadManager.Request.VISIBILITY_VISIBLE_NOTIFY_COMPLETED);
        }
        request.setDestinationInExternalPublicDir(Environment.DIRECTORY_DOWNLOADS, fileName);

// get download service and enqueue file
        DownloadManager manager = (DownloadManager) getActivity().getSystemService(Context.DOWNLOAD_SERVICE);
        manager.enqueue(request);
    }
}
