package nts.swipesafe.fragment;

import android.app.Dialog;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.ImageView;
import android.widget.LinearLayout;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.adapter.SkillAdapter;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.SkillModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class LibrarySkillFragment extends Fragment {
    private View view;
    private ImageView imgBack;
    private RecyclerView rvSkill;
    private SkillAdapter lySkillAdapter;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_library_skill, container, false);

        initComponent();

        viewSkill();

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new ChildKnowFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        rvSkill = view.findViewById(R.id.rvSkill);
        rvSkill.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        rvSkill.setHasFixedSize(true);

        imgBack = view.findViewById(R.id.imgBack);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backFragment();
            }
        });
    }

    private void viewSkill() {
        String webJson = Utils.ReadJSONFromAsset(getContext(), "skill.json");
        List<SkillModel> listSkill = new Gson().fromJson(webJson, new TypeToken<ArrayList<SkillModel>>() {
        }.getType());
        lySkillAdapter = new SkillAdapter(getActivity(), listSkill);
        rvSkill.setAdapter(lySkillAdapter);
        lySkillAdapter.SetOnItemClickListener(new SkillAdapter.OnItemClickListener() {
            @Override
            public void onItemClick(View view, int position, SkillModel skillModel) {
                Fragment fragment = new LibrarySkillDetailFragment();
                Bundle bundle = new Bundle();
                bundle.putString("SkillModel", new Gson().toJson(skillModel));
                Utils.ChangeFragment(getActivity(), fragment, bundle);
            }
        });
    }
}
