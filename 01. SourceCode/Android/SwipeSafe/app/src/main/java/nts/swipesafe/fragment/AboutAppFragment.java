package nts.swipesafe.fragment;

import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.adapter.AbuseListAdapter;
import nts.swipesafe.adapter.ChildListAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.LocationGpsListener;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ChildAbuseModel;
import nts.swipesafe.model.ChildModel;
import nts.swipesafe.model.ComboboxResult;
import nts.swipesafe.model.LocationModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class AboutAppFragment extends Fragment {
    private View view;
    private TextView tvVersionName;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_about_app, container, false);

        initComponent();

        // Inflate the layout for this fragment
        return view;
    }

    private void initComponent() {
        try {
            PackageInfo pinfo = getActivity().getPackageManager().getPackageInfo(getActivity().getPackageName(), 0);
            String versionName = pinfo.versionName;

            tvVersionName = view.findViewById(R.id.tvVersionName);
            tvVersionName.setText(versionName);
        } catch (Exception ex) {
        }
    }
}
