package nts.swipesafe.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;

import nts.swipesafe.R;
import nts.swipesafe.common.Utils;

public class ChildKnowFragment extends Fragment {
    private View view;
    private ImageView imgBack;
    private LinearLayout lyLibSkill, lyLibGender;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_library_child_know, container, false);
        imgBack = view.findViewById(R.id.imgBack);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backFragment();
            }
        });


        lyLibGender = view.findViewById(R.id.lyLibGender);
        lyLibGender.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new LibraryGenderFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyLibSkill = view.findViewById(R.id.lyLibSkill);
        lyLibSkill.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new LibrarySkillFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);

            }
        });
        return view;
    }

    public void backFragment() {
        Fragment fragment = new LibraryFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }
}
