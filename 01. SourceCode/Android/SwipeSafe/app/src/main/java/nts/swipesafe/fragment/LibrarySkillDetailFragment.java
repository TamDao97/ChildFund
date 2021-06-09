package nts.swipesafe.fragment;

import android.app.Dialog;
import android.content.Intent;
import android.content.res.AssetManager;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.webkit.WebView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.google.gson.Gson;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;

import nts.swipesafe.R;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.SkillModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class LibrarySkillDetailFragment extends Fragment {
    private View view;
    private ImageView imgBack;
    private TextView txtTitle;
    private WebView webContent;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_library_skill_details, container, false);

        Bundle bundle = this.getArguments();

        initComponent();

        if (bundle != null) {
            String skillModelJson = bundle.getString("SkillModel");
            SkillModel skillModel = new Gson().fromJson(skillModelJson, SkillModel.class);
            setContent(skillModel);
        }

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new LibrarySkillFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        txtTitle = view.findViewById(R.id.txtTitle);
        webContent = view.findViewById(R.id.webContent);

        imgBack = view.findViewById(R.id.imgBack);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backFragment();
            }
        });
    }

    private void setContent(SkillModel skillModel) {
        if (skillModel != null) {
            AssetManager mgr = getContext().getAssets();
            txtTitle.setText(skillModel.Title);

            try {
                InputStream in = mgr.open(skillModel.FileHTML, AssetManager.ACCESS_BUFFER);
                String htmlContentInStringFormat = Utils.StreamToString(in);
                in.close();
                webContent.loadDataWithBaseURL(null, htmlContentInStringFormat, "text/html", "utf-8", null);

            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }
}
