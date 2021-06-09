package nts.childprofile.fragment;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import nts.childprofile.R;
import nts.childprofile.common.Utils;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class LinkWebsiteFragment extends Fragment {
    private View view;
    private LinearLayout btnChildProfile, btnChildFund, btnFacebook;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_web, container, false);

        initComponent();

        Toolbar mToolbar = (Toolbar) getActivity().findViewById(R.id.toolbar);
        TextView toolbarTitle = mToolbar.findViewById(R.id.toolbar_title);
        toolbarTitle.setText("Hướng dẫn hữu ích/Useful guide");
        Toolbar.LayoutParams layoutParams = (Toolbar.LayoutParams) toolbarTitle.getLayoutParams();
        layoutParams.gravity = Gravity.LEFT;
        toolbarTitle.setLayoutParams(layoutParams);

        return view;
    }

    public void backFragment() {
        Fragment fragment = new MenuFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        btnChildProfile = view.findViewById(R.id.btnChildProfile);
        btnChildProfile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://13.76.190.85:8080/"));
                startActivity(browserIntent);
            }
        });

        btnChildFund = view.findViewById(R.id.btnChildFund);
        btnChildFund.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://www.childfund.org.vn/"));
                startActivity(browserIntent);
            }
        });

        btnFacebook = view.findViewById(R.id.btnFacebook);
        btnFacebook.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://www.facebook.com/ChildFund.Vietnam/"));
                startActivity(browserIntent);
            }
        });
    }
}
