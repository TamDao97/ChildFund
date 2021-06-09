package nts.swipesafe.fragment;

import android.app.Dialog;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.Toast;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

import nts.swipesafe.R;
import nts.swipesafe.common.Utils;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportMainFragment extends Fragment {
    private View view;
    private LinearLayout lyBegin, lyJoin, lyJoinParent;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_report_abuse_about, container, false);

        lyBegin = view.findViewById(R.id.lyBegin);
        lyBegin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new ReportAbuseStepInfoFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyJoin = view.findViewById(R.id.lyJoin);
        lyJoinParent = view.findViewById(R.id.lyJoinParent);

        try {
            Date today = Calendar.getInstance().getTime();
            SimpleDateFormat dateFormat = new SimpleDateFormat("dd/MM/yyyy");
            String expirationDate = "16/07/2020";
            Date selectedDate = dateFormat.parse(expirationDate);

            if (selectedDate.before(today)) {
                lyJoinParent.setVisibility(View.GONE);
            }
        } catch (Exception ex) {

        }
        lyJoin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new FragmentCompetition();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });
        // Inflate the layout for this fragment
        return view;
    }
}
