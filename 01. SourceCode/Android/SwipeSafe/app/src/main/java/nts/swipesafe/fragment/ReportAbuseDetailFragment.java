package nts.swipesafe.fragment;

import android.app.Dialog;
import android.content.DialogInterface;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.ANRequest;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.StringRequestListener;
import com.google.gson.Gson;

import java.io.File;
import java.util.ArrayList;

import nts.swipesafe.R;
import nts.swipesafe.adapter.ChildDetailListAdapter;
import nts.swipesafe.adapter.ImageReportAdapter;
import nts.swipesafe.adapter.PrisonerDetailListAdapter;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ChildModel;
import nts.swipesafe.model.GalleryModel;
import nts.swipesafe.model.PrisonerModel;
import nts.swipesafe.model.ReportModel;
import nts.swipesafe.model.ReporterModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseDetailFragment extends Fragment {
    private View view;
    private LinearLayout lyNext;
    private LinearLayout lyBack;
    private GlobalVariable global;
    private ReportModel reportModel;
    private RelativeLayout progressDialog;
    private RecyclerView rvChild, rvPrisoner;
    private TextView txtName, txtGender, txtPhone, txtEmail, txtRelationship, txtAddress, txtDescription, txtIncognito;
    private LinearLayout lyReporter, lyName, lyGender, lyPhone, lyEmail, lyRelationship, lyAddress, lyDescription, lyGallery;
    private TextView txtEditReporter, txtEditDescription;
    private ImageReportAdapter imageReportAdapter;
    private ArrayList<GalleryModel> imageUrlsReport = new ArrayList<>();

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_report_abuse_detail, container, false);

        global = (GlobalVariable) getActivity().getApplication();
        reportModel = global.getReport();

        initComponent();

        viewInfoReport();

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new ReportAbuseStepFourFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        progressDialog = view.findViewById(R.id.progressDialog);
        lyNext = view.findViewById(R.id.lyNext);

        rvChild = view.findViewById(R.id.rvChild);
        rvChild.setLayoutManager(new GridLayoutManager(getActivity(), 1));

        rvPrisoner = view.findViewById(R.id.rvPrisoner);
        rvPrisoner.setLayoutManager(new GridLayoutManager(getActivity(), 1));

        lyReporter = view.findViewById(R.id.lyReporter);
        lyName = view.findViewById(R.id.lyName);
        lyGender = view.findViewById(R.id.lyGender);
        lyPhone = view.findViewById(R.id.lyPhone);
        lyEmail = view.findViewById(R.id.lyEmail);
        lyAddress = view.findViewById(R.id.lyAddress);
        lyRelationship = view.findViewById(R.id.lyRelationship);
        lyDescription = view.findViewById(R.id.lyDescription);
        lyGallery = view.findViewById(R.id.lyGallery);

        txtName = view.findViewById(R.id.txtName);
        txtGender = view.findViewById(R.id.txtGender);
        txtPhone = view.findViewById(R.id.txtPhone);
        txtEmail = view.findViewById(R.id.txtEmail);
        txtRelationship = view.findViewById(R.id.txtRelationship);
        txtAddress = view.findViewById(R.id.txtAddress);
        txtDescription = view.findViewById(R.id.txtDescription);
        txtIncognito = view.findViewById(R.id.txtIncognito);

        txtEditReporter = view.findViewById(R.id.txtEditReporter);
        txtEditDescription = view.findViewById(R.id.txtEditDescription);

        lyNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                saveReportChild();
            }
        });

        lyBack = view.findViewById(R.id.lyBack);
        lyBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                backFragment();
            }
        });

        txtEditReporter.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Bundle bundle = new Bundle();
                bundle.putBoolean("edit", true);
                Fragment fragment = new ReportAbuseStepFourFragment();
                fragment.setArguments(bundle);
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        txtEditDescription.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Bundle bundle = new Bundle();
                bundle.putBoolean("edit", true);
                Fragment fragment = new ReportAbuseStepThreeFragment();
                fragment.setArguments(bundle);
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });
    }

    /***
     * Hiển thị thông tin báo cáo
     */
    private void viewInfoReport() {
        if (global.reportModel.ListChild != null && global.reportModel.ListChild.size() > 0) {
            ChildDetailListAdapter childListAdapter = new ChildDetailListAdapter(getActivity(), global.reportModel.ListChild);
            rvChild.setAdapter(childListAdapter);
            childListAdapter.SetOnItemClickListener(new ChildDetailListAdapter.OnItemClickListener() {
                @Override
                public void onEditClick(View view, int position, ChildModel obj) {
                    Bundle bundle = new Bundle();
                    bundle.putInt("indexEdit", position);
                    Fragment fragment = new ReportAbuseStepOneFragment();
                    fragment.setArguments(bundle);
                    Utils.ChangeFragment(getActivity(), fragment, null);
                }
            });
        }

        if (global.reportModel.ListPrisoner != null && global.reportModel.ListPrisoner.size() > 0) {
            PrisonerDetailListAdapter prisonerDetailListAdapter = new PrisonerDetailListAdapter(getActivity(), global.reportModel.ListPrisoner);
            rvPrisoner.setAdapter(prisonerDetailListAdapter);
            prisonerDetailListAdapter.SetOnItemClickListener(new PrisonerDetailListAdapter.OnItemClickListener() {
                @Override
                public void onEditClick(View view, int position, PrisonerModel obj) {
                    Bundle bundle = new Bundle();
                    bundle.putInt("indexEdit", position);
                    Fragment fragment = new ReportAbuseStepTwoFragment();
                    fragment.setArguments(bundle);
                    Utils.ChangeFragment(getActivity(), fragment, null);
                }
            });
        }

        ReporterModel reporterModel = global.getReporter();
        if (reporterModel.Type.equals("1")) {
            txtIncognito.setVisibility(View.VISIBLE);
            lyReporter.setVisibility(View.GONE);
        } else {
            txtIncognito.setVisibility(View.GONE);
            lyReporter.setVisibility(View.VISIBLE);
            if (!Utils.isEmpty(reporterModel.Name)) {
                lyName.setVisibility(View.VISIBLE);
                txtName.setText(reporterModel.Name);
            } else {
                lyName.setVisibility(View.GONE);
            }

            if (!Utils.isEmpty(reporterModel.GenderName)) {
                lyGender.setVisibility(View.VISIBLE);
                txtGender.setText(reporterModel.GenderName);
            } else {
                lyGender.setVisibility(View.GONE);
            }

            if (!Utils.isEmpty(reporterModel.Phone)) {
                lyPhone.setVisibility(View.VISIBLE);
                txtPhone.setText(reporterModel.Phone);
            } else {
                lyPhone.setVisibility(View.GONE);
            }

            if (!Utils.isEmpty(reporterModel.Email)) {
                lyEmail.setVisibility(View.VISIBLE);
                txtEmail.setText(reporterModel.Email);
            } else {
                lyEmail.setVisibility(View.GONE);
            }

            if (!Utils.isEmpty(reporterModel.RelationshipName)) {
                lyRelationship.setVisibility(View.VISIBLE);
                txtRelationship.setText(reporterModel.RelationshipName);
            } else {
                lyRelationship.setVisibility(View.GONE);
            }

            if (!Utils.isEmpty(reporterModel.FullAddress)) {
                lyAddress.setVisibility(View.VISIBLE);
                txtAddress.setText(reporterModel.FullAddress);
            } else {
                lyAddress.setVisibility(View.GONE);
            }
        }

        String description = global.getDescription();
        if (!Utils.isEmpty(description)) {
            lyDescription.setVisibility(View.VISIBLE);
            txtDescription.setText(description);
        } else {
            lyDescription.setVisibility(View.GONE);
        }

        if (global.fileReport != null && global.fileReport.size() > 0) {
            viewImageReport(global.fileReport);
            lyGallery.setVisibility(View.VISIBLE);
        } else {
            lyGallery.setVisibility(View.GONE);
        }
    }

    private void viewImageReport(final ArrayList<GalleryModel> imageUrls) {
        imageReportAdapter = new ImageReportAdapter(getContext(), imageUrls, false);

        RecyclerView.LayoutManager layoutManager = new GridLayoutManager(getContext(), 5);
        RecyclerView recyclerView = (RecyclerView) view.findViewById(R.id.rvGalleryChoose);
        recyclerView.setLayoutManager(layoutManager);
        recyclerView.setItemAnimator(new DefaultItemAnimator());
        recyclerView.addItemDecoration(new ItemOffsetDecoration(getContext(), R.dimen.item_offset));
        recyclerView.setAdapter(imageReportAdapter);

        imageReportAdapter.SetOnItemClickListener(new ImageReportAdapter.OnItemClickListener() {
            @Override
            public void onRemoveClick(View view, int position, GalleryModel imageUrl) {
            }
        });
    }

    ///Lưu thông tin báo cáo
    private void saveReportChild() {
//        String jsonModel = new Gson().toJson(reportModel);
//        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/Report/AddReport"));
//
//        if (global.fileReport != null && global.fileReport.size() > 0) {
//            for (int i = 0; i < global.fileReport.size(); i++) {
//                File file = new File(global.fileReport.get(i).FileUrl);
//                anRequest.addMultipartFile("file" + i, file);
//            }
//        }
//
//        progressDialog.setVisibility(View.VISIBLE);
//        anRequest.addMultipartParameter("Model", jsonModel)
//                .setPriority(Priority.MEDIUM)
//                .build()
//                .getAsString(new StringRequestListener() {
//                    @Override
//                    public void onResponse(String response) {
//                        progressDialog.setVisibility(View.GONE);
//                        global.clearReport();
//                        ConfirmDialog();
//
////                        new AlertDialog.Builder(getActivity())
////                                .setTitle("Thông báo")
////                                .setIcon(R.drawable.ic_success)
////                                .setMessage("Gửi báo cáo thành công. Bạn có muốn tiếp tục một báo cáo mới?")
////                                .setCancelable(false)
////                                .setPositiveButton("Có", new DialogInterface.OnClickListener() {
////                                    public void onClick(DialogInterface dialog, int id) {
////                                        Fragment fragment = new ReportAbuseStepOneFragment();
////                                        Utils.ChangeFragment(getActivity(), fragment, null);
////                                    }
////                                })
////                                .setNegativeButton("Không", new DialogInterface.OnClickListener() {
////                                    @Override
////                                    public void onClick(DialogInterface dialog, int which) {
////                                        Fragment fragment = new ReportMainFragment();
////                                        Utils.ChangeFragment(getActivity(), fragment, null);
////                                    }
////                                })
////                                .show();
//                    }
//
//                    @Override
//                    public void onError(ANError anError) {
//                        progressDialog.setVisibility(View.GONE);
//                        Utils.showErrorMessage(getActivity().getApplication(), anError);
//                    }
//                });
    }

    public void ConfirmDialog() {
        final Dialog dialog = new Dialog(getContext());
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialog.setContentView(R.layout.popup_screen);
        LinearLayout btnBack = dialog.findViewById(R.id.lyBack);
        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new ReportMainFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
                dialog.dismiss();
            }
        });

        LinearLayout btnNext = dialog.findViewById(R.id.lyNext);
        btnNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new ReportAbuseStepOneFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
                dialog.dismiss();
            }
        });
        dialog.show();
    }
}
