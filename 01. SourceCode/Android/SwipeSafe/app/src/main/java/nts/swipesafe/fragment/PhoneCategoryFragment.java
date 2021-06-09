package nts.swipesafe.fragment;

import android.content.Intent;
import android.net.Uri;
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
public class PhoneCategoryFragment extends Fragment {
    private View view;
    private LinearLayout ly113, ly115, ly111;
    private ImageView imgBack;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_phone_category, container, false);

        initComponent();

        // Inflate the layout for this fragment
        return view;
    }

    public void  backFragment()
    {
        Fragment fragment = new ReportMainFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        ly113 = view.findViewById(R.id.ly113);
        ly113.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivity(new Intent(Intent.ACTION_DIAL, Uri.fromParts("tel", "113", null)));
            }
        });

        ly111 = view.findViewById(R.id.ly111);
        ly111.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivity(new Intent(Intent.ACTION_DIAL, Uri.fromParts("tel", "111", null)));
            }
        });

        ly115 = view.findViewById(R.id.ly115);
        ly115.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivity(new Intent(Intent.ACTION_DIAL, Uri.fromParts("tel", "115", null)));
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
