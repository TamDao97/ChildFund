package nts.childprofile.fragment;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.FloatingActionButton;
import android.support.v4.app.Fragment;
import android.support.v7.widget.Toolbar;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.animation.AlphaAnimation;
import android.view.animation.Animation;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.ANRequest;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.androidnetworking.interfaces.StringRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.squareup.picasso.Picasso;

import org.json.JSONObject;

import java.io.File;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;

import nts.childprofile.LoginActivity;
import nts.childprofile.R;
import nts.childprofile.Sql.DataBaseHelper;
import nts.childprofile.common.BorderImage;
import nts.childprofile.common.Constants;
import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.model.ChildProfileModel;
import nts.childprofile.model.GalleryModel;
import nts.childprofile.model.ImageChildByYearModel;
import nts.childprofile.model.LoginProfileModel;
import nts.childprofile.model.ProfileUserModel;
import nts.childprofile.model.ReportProfilesModel;

public class MenuFragment extends Fragment {
    private View view;
    public LoginProfileModel loginProfileModel;
    private Toolbar mToolbar;
    private ImageView imgLogo;
    private TextView toolbarTitle;
    private TextView txtOffline;
    private Toolbar.LayoutParams layoutParams;
    private GlobalVariable global;
    boolean anhien = false;
    private Animation in;
    private ProfileUserModel profileUserModel;
    private RelativeLayout syncProgress;
    private AtomicInteger countProgress = new AtomicInteger(0);

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_menu, null);
        global = (GlobalVariable) getActivity().getApplication();
        loginProfileModel = global.getLoginProfile();

        mToolbar = (Toolbar) getActivity().findViewById(R.id.toolbar);
        imgLogo = mToolbar.findViewById(R.id.imgLogo);
        toolbarTitle = mToolbar.findViewById(R.id.toolbar_title);
        txtOffline = mToolbar.findViewById(R.id.txtOffline);
        toolbarTitle.setText("Child Profile");
        layoutParams = (Toolbar.LayoutParams) toolbarTitle.getLayoutParams();
        layoutParams = (Toolbar.LayoutParams) txtOffline.getLayoutParams();
        layoutParams.gravity = Gravity.CENTER;
        toolbarTitle.setLayoutParams(layoutParams);
        txtOffline.setLayoutParams(layoutParams);
        imgLogo.setVisibility(View.GONE);

        if (global.isStatusRunApp()) {
            txtOffline.setVisibility(View.VISIBLE);
        }
        //viewProfileUser();
        TextView nameAcc = view.findViewById(R.id.nameAcc);
        nameAcc.setText(loginProfileModel.Name);

        syncProgress = view.findViewById(R.id.progressDialog);
        final LinearLayout menuEdit = view.findViewById(R.id.menuEdit);

        final LinearLayout btnThongTinCaNhan = view.findViewById(R.id.btnThongTinCaNhan);
        final LinearLayout btnDoiMatKhau = view.findViewById(R.id.btnDoiMatKhau);
        //final LinearLayout menuEdit = view.findViewById(R.id.menuEdit);
        FloatingActionButton btnMenu = view.findViewById(R.id.btnMenu);
        btnMenu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (anhien == false) {
                    menuEdit.setVisibility(View.VISIBLE);
                    menuEdit.startAnimation(in);
                    menuEdit.startAnimation(in);
                    anhien = true;
                } else {
                    menuEdit.setVisibility(View.GONE);
                    anhien = false;
                }

            }
        });

        final Bundle bundle = new Bundle();
        btnDoiMatKhau.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                anhien = false;
                Fragment fragment = new ChangePasswordFragment();
                Utils.ChangeFragment(getActivity(), fragment, bundle);
            }
        });

        btnThongTinCaNhan.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                anhien = false;
                Fragment fragment = new ProfileUserFragment();
                Utils.ChangeFragment(getActivity(), fragment, bundle);
            }
        });

        in = new AlphaAnimation(0.0f, 1.0f);
        in.setDuration(500);
        FloatingActionButton fabLogout = view.findViewById(R.id.fabLogout);
        fabLogout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
                builder.setTitle("Đăng xuất");
                builder.setMessage("Bạn muốn đăng xuất khỏi ứng dụng/You want to log out of the application?");
                builder.setPositiveButton("Đồng ý/Ok", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        global.removeLoginProfile();
//                        Intent mainActivity = new Intent(getActivity().getApplicationContext(), LoginActivity.class);
//                        startActivity(mainActivity);
                        getActivity().finish();
                    }
                });
                builder.setNegativeButton("Huỷ/Cancel", null);
                builder.show();
            }
        });


        BorderImage imageView = view.findViewById(R.id.imageAva);
        try {
            if (loginProfileModel.ImagePath != null && !loginProfileModel.ImagePath.isEmpty()) {
                Picasso.with(getActivity()).load(loginProfileModel.ImagePath).resize(208, 208)
                        .centerInside().into(imageView);
            } else {
                Picasso.with(getActivity()).load(R.drawable.ic_people).resize(300, 300)
                        .centerInside().into(imageView);
            }
        } catch (Exception ex) {
        }

        LinearLayout btnTimKimHoSoTre = view.findViewById(R.id.btnTimKimHoSoTre);
        btnTimKimHoSoTre.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new ProfileListFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
                imgLogo.setVisibility(View.VISIBLE);
            }
        });

        LinearLayout btnTaoHoSo = view.findViewById(R.id.btnTaoHoSo);
        btnTaoHoSo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new CreateProfileChildFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
                imgLogo.setVisibility(View.VISIBLE);
            }
        });

        LinearLayout btnDangAnh = view.findViewById(R.id.btnDangAnh);
        btnDangAnh.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new UploadImage();
                Utils.ChangeFragment(getActivity(), fragment, null);
                imgLogo.setVisibility(View.VISIBLE);
            }
        });

        LinearLayout btnLink = view.findViewById(R.id.btnLink);
        btnLink.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new LinkWebsiteFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
                imgLogo.setVisibility(View.VISIBLE);
            }
        });

        if (!global.isStatusRunApp()) {
            syncChildProfile();
        }
        return view;
    }

    private void DialogMenu() {
        final Dialog dialog = new Dialog(getContext());
        dialog.setContentView(R.layout.popup_edit);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.show();
    }

    private void viewProfileUser() {
        AndroidNetworking.get(Utils.GetUrlApi("api/Authorize/GetProfileUser?id=" + loginProfileModel.Id))
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        profileUserModel = new Gson().fromJson(response.toString(), new TypeToken<ProfileUserModel>() {
                        }.getType());
                    }

                    @Override
                    public void onError(ANError anError) {
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    private int sizeListChildProfile = 0;

    public void syncChildProfile() {
        try {
            DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
            List<ChildProfileModel> listChildProfile = new ArrayList<>();
            listChildProfile = dataBaseHelper.getListChildProfileToSync();
            List<ReportProfilesModel> listReport = new ArrayList<>();
            listReport = dataBaseHelper.getReportChildProfile();
            List<ImageChildByYearModel> listImageCPR = new ArrayList<>();
            listImageCPR = dataBaseHelper.getListImageCPR();

            sizeListChildProfile = listChildProfile.size() + listReport.size() + listImageCPR.size();
            if (sizeListChildProfile > 0) {
                syncProgress.setVisibility(View.VISIBLE);
            }
            if (listReport.size() > 0) {
                for (ReportProfilesModel reportProfilesModel : listReport) {
                    reportChildProfile(reportProfilesModel);
                }
            }

            if (listImageCPR.size() > 0) {
                for (ImageChildByYearModel imageChildByYearModel : listImageCPR) {
                    updateImageCPR(imageChildByYearModel);
                }
            }
            if (listChildProfile.size() > 0) {
                for (ChildProfileModel childProfileModel : listChildProfile) {
                    childProfileModel.UserLever = loginProfileModel.UserLever;
                    if (childProfileModel.TypeChildProfile.equals(Constants.TYPE_CHILDPROFILE_SQLITE_CREATED)) {
                        createChildProfile(childProfileModel);
                    } else if (childProfileModel.TypeChildProfile.equals(Constants.TYPE_CHILDPROFILE_SQLITE_UPDATED)) {
                        updatẹChildProfile(childProfileModel);
                    }
                }
            }
        } catch (Exception ex) {
            Toast.makeText(getActivity(), "Đồng bộ hồ sơ trẻ không thành công", Toast.LENGTH_LONG).show();
        }
    }

    private void check() {
        if (countProgress.get() == sizeListChildProfile) {
            syncProgress.setVisibility(View.GONE);
        }
    }

    private void createChildProfile(final ChildProfileModel childProfileModel) {
        childProfileModel.ProcessStatus = Constants.ProfilesNew;
        childProfileModel.CreateBy = loginProfileModel.Id;
        childProfileModel.UpdateBy = loginProfileModel.Id;
        String jsonModel = new Gson().toJson(childProfileModel);
        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/ChildProfiles/AddChildProfile"));
        if (!Utils.isEmpty(childProfileModel.ImagePath) && !childProfileModel.ImagePath.contains("https")) {
            File file = new File(childProfileModel.ImagePath);
            anRequest.addMultipartFile("Avatar", file);
        }
        if (!Utils.isEmpty(childProfileModel.ImageSignaturePath) && !childProfileModel.ImageSignaturePath.contains("https")) {
            File file = new File(childProfileModel.ImageSignaturePath);
            anRequest.addMultipartFile("ImageSignature", file);
        }
        anRequest.addMultipartParameter("Model", jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String response) {
//                        progressDialog.setVisibility(View.GONE);
//                        Toast.makeText(getActivity(), "Thêm mới hồ sơ trẻ thành công!", Toast.LENGTH_SHORT).show();

                        countProgress.addAndGet(1);
                        DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
                        dataBaseHelper.delete(Constants.DATABASE_TABLE_ChildProfile, childProfileModel.Id);

                        if (!Utils.isEmpty(childProfileModel.ImagePath) && !childProfileModel.ImagePath.contains("https")) {
                            File image = new File(childProfileModel.ImagePath);
                            image.delete();
                        }
                        if (!Utils.isEmpty(childProfileModel.ImageSignaturePath) && !childProfileModel.ImageSignaturePath.contains("https")) {
                            File signature = new File(childProfileModel.ImageSignaturePath);
                            signature.delete();
                        }
                        check();
                    }

                    @Override
                    public void onError(ANError anError) {
//                        progressDialog.setVisibility(View.GONE);
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                        countProgress.addAndGet(1);
                        check();
                    }
                });
    }

    private void updatẹChildProfile(final ChildProfileModel childProfileModel) {
        childProfileModel.UpdateBy = loginProfileModel.Id;
        String jsonModel = new Gson().toJson(childProfileModel);
        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/ChildProfiles/UpdateChildProfile"));
        if (!Utils.isEmpty(childProfileModel.ImagePath) && !childProfileModel.ImagePath.contains("https")) {
            File file = new File(childProfileModel.ImagePath);
            anRequest.addMultipartFile("Avatar", file);
        }
        if (!Utils.isEmpty(childProfileModel.ImageSignaturePath) && !childProfileModel.ImageSignaturePath.contains("https")) {
            File file = new File(childProfileModel.ImageSignaturePath);
            anRequest.addMultipartFile("ImageSignature", file);
        }
//        progressDialog.setVisibility(View.VISIBLE);
        anRequest.addMultipartParameter("Model", jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String response) {
//                        progressDialog.setVisibility(View.GONE);
                        countProgress.addAndGet(1);
                        DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
                        dataBaseHelper.delete(Constants.DATABASE_TABLE_ChildProfile, childProfileModel.Id);

                        if (!Utils.isEmpty(childProfileModel.ImagePath) && !childProfileModel.ImagePath.contains("https")) {
                            File image = new File(childProfileModel.ImagePath);
                            image.delete();
                        }
                        if (!Utils.isEmpty(childProfileModel.ImageSignaturePath) && !childProfileModel.ImageSignaturePath.contains("https")) {
                            File signature = new File(childProfileModel.ImageSignaturePath);
                            signature.delete();
                        }
                        check();
                    }

                    @Override
                    public void onError(ANError anError) {
                        countProgress.addAndGet(1);
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                        check();
                    }
                });
    }

    private void reportChildProfile(final ReportProfilesModel reportProfilesModel) {
        String[] listFileUrl = new String[0];
        if (!Utils.isEmpty(reportProfilesModel.Url)) {
            listFileUrl = reportProfilesModel.Url.split(";");
        }
        String jsonModel = new Gson().toJson(reportProfilesModel);
        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/ChildProfiles/AddReportProfile"));
        if (listFileUrl.length > 0) {
            for (int i = 1; i < listFileUrl.length; i++) {
                if (!Utils.isEmpty(listFileUrl[i])) {
                    File file = new File(listFileUrl[i]);
                    anRequest.addMultipartFile("file" + i, file);
                }
            }
        }

        anRequest.addMultipartParameter("Model", jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String response) {
                        countProgress.addAndGet(1);
//                        Toast.makeText(getActivity(), "Báo cáo thay đổi tình trạng của trẻ thành công.", Toast.LENGTH_SHORT).show();
                        DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
                        dataBaseHelper.deleteReport(reportProfilesModel.ChildProfileId);
                        check();
                    }

                    @Override
                    public void onError(ANError anError) {
//                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                        countProgress.addAndGet(1);
                        check();
                    }
                });
    }

    private void updateImageCPR(final ImageChildByYearModel imageChildByYearModel) {
        String jsonModel = new Gson().toJson(imageChildByYearModel);
        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/ChildProfiles/AddImageChilByYear"));
        File file = new File(imageChildByYearModel.ImageUrl);
        anRequest.addMultipartFile("Avatar", file);

        anRequest.addMultipartParameter("Model", jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String response) {
                        DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
                        dataBaseHelper.deleteImageCPR(imageChildByYearModel.ChildProfileId);
                        countProgress.addAndGet(1);
                        check();
                    }

                    @Override
                    public void onError(ANError anError) {
                        countProgress.addAndGet(1);
                        check();
                    }
                });
    }
}
