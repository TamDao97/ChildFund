package nts.swipesafe.fragment;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
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

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.adapter.LinkWebsiteAdapter;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.WebsiteModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class LinkWebsiteFragment extends Fragment {
    private View view;
    private ImageView imgBack;
    private RecyclerView rvWebsite;
    private LinkWebsiteAdapter linkWebsiteAdapter;
    private LinearLayout btnCucTreEm, btnChildFund, btnVarc, btnUnicef, btnPlanInternational, btnWorldVision, btnSaveChildren, btnHagar, btnGoodNeighbors;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_web, container, false);

        initComponent();

        //viewWebsite();

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new ReportMainFragment();
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

        btnCucTreEm = view.findViewById(R.id.btnCucTreEm);
        btnCucTreEm.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://treem.gov.vn/"));
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

        btnVarc = view.findViewById(R.id.btnVarc);
        btnVarc.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://treemviet.vn/"));
                startActivity(browserIntent);
            }
        });

        btnUnicef = view.findViewById(R.id.btnUnicef);
        btnUnicef.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://www.unicef.org/vietnam/vi"));
                startActivity(browserIntent);
            }
        });

        btnPlanInternational = view.findViewById(R.id.btnPlanInternational);
        btnPlanInternational.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://plan-international.org/vietnam"));
                startActivity(browserIntent);
            }
        });

        btnWorldVision = view.findViewById(R.id.btnWorldVision);
        btnWorldVision.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://www.wvi.org/vi/vi%E1%BB%87t-nam"));
                startActivity(browserIntent);
            }
        });

        btnSaveChildren = view.findViewById(R.id.btnSaveChildren);
        btnSaveChildren.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://vietnam.savethechildren.net/"));
                startActivity(browserIntent);
            }
        });

        btnHagar = view.findViewById(R.id.btnHagar);
        btnHagar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://hagarinternational.org/vietnam/"));
                startActivity(browserIntent);
            }
        });

        btnGoodNeighbors = view.findViewById(R.id.btnGoodNeighbors);
        btnGoodNeighbors.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://goodneighbors.vn/"));
                startActivity(browserIntent);
            }
        });


    }

//    private void viewWebsite()
//    {
//        String webJson = Utils.ReadJSONFromAsset(getContext(),"organize.json");
//        List<WebsiteModel> listWebsite = new Gson().fromJson(webJson, new TypeToken<ArrayList<WebsiteModel>>() {
//        }.getType());
//        linkWebsiteAdapter = new LinkWebsiteAdapter(getActivity(), listWebsite);
//        rvWebsite.setAdapter(linkWebsiteAdapter);
//        linkWebsiteAdapter.SetOnItemClickListener(new LinkWebsiteAdapter.OnItemClickListener() {
//            @Override
//            public void onItemClick(View view, int position, WebsiteModel websiteModel) {
//                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(websiteModel.Website));
//                startActivity(browserIntent);
//            }
//        });
//    }
}
