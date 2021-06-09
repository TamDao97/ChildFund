package nts.swipesafe.fragment;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;

import nts.swipesafe.R;
import nts.swipesafe.common.Utils;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class LibraryFragment extends Fragment {
    private View view;
    private LinearLayout lyLibSkill, lyLibRight,lyLibInternet;
    private ImageView imgBack;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_library, container, false);

        initComponent();

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new ReportMainFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        lyLibSkill = view.findViewById(R.id.lyLibSkill);
        lyLibSkill.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new ChildKnowFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyLibRight = view.findViewById(R.id.lyLibRight);
        lyLibRight.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new LibraryRightFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyLibInternet = view.findViewById(R.id.lyLibInternet);
        lyLibInternet.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new LibraryInternetFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        imgBack = view.findViewById(R.id.imgBack);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backFragment();
            }
        });
    }
}
