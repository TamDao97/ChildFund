package reportward.nts.reportward.fragment;

import android.app.Dialog;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.PorterDuff;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.view.ViewPager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.ANRequest;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.SectionsPagerAdapter;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.GalleryModel;
import reportward.nts.reportward.model.HoSoCaTuVanModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;
import reportward.nts.reportward.model.UploadFileResultModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportCaseCreateFragment extends Fragment {
    private View view;
    private SectionsPagerAdapter adapter;
    private ViewPager reportViewPager;
    private TabLayout reportTabLayout;
    private LinearLayout lyBack, lyNext;
    private HoSoCaTuVanModel hoSoCaTuVanModel;
    private String maHoSo;
    private Dialog dialogConfirm;
    private RelativeLayout progressDialog;
    private long fileSize = 0;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_report_case, container, false);

        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }

        initComponent();

        ConfirmDialog();

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        Fragment fragment = new FragmentMenu();
        Utils.ChangeFragment(getActivity(), fragment, null);
    }

    private void initComponent() {
        progressDialog = view.findViewById(R.id.progressDialog);
        SimpleDateFormat format = new SimpleDateFormat("dd-MM-yyyy-hhmmssSSS");
        maHoSo = format.format(new Date());

        lyBack = view.findViewById(R.id.lyBack);
        lyNext = view.findViewById(R.id.lyNext);

        reportViewPager = (ViewPager) view.findViewById(R.id.reportViewPager);
        setupViewPager(reportViewPager);

        reportTabLayout = (TabLayout) view.findViewById(R.id.reportTabLayout);
        reportTabLayout.setupWithViewPager(reportViewPager);

        reportTabLayout.getTabAt(0).setIcon(R.drawable.ic_thong_tin);
        reportTabLayout.getTabAt(1).setIcon(R.drawable.ic_tre);
        reportTabLayout.getTabAt(2).setIcon(R.drawable.ic_doi_tuong);
        reportTabLayout.getTabAt(3).setIcon(R.drawable.ic_lien_he);

        reportTabLayout.addOnTabSelectedListener(new TabLayout.OnTabSelectedListener() {
            @Override
            public void onTabSelected(TabLayout.Tab tab) {
                tab.getIcon().setColorFilter(getResources().getColor(R.color.indigo_900), PorterDuff.Mode.SRC_IN);
            }

            @Override
            public void onTabUnselected(TabLayout.Tab tab) {
                if (tab.getPosition() == 0)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).GetInfoModel();
                else if (tab.getPosition() == 3)
                    ((ReportAbuseContactFragment) adapter.getItem(3)).GetInfoModel();

                tab.getIcon().setColorFilter(getResources().getColor(R.color.indigo_500), PorterDuff.Mode.SRC_IN);
            }

            @Override
            public void onTabReselected(TabLayout.Tab tab) {

            }
        });

        lyNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (!Utils.checkConnectedNetwork(getActivity())) {
                    new android.support.v7.app.AlertDialog.Builder(getActivity())
                            .setTitle("Thông báo")
                            .setIcon(R.drawable.ic_warning)
                            .setMessage("Vui lòng kiểm tra kết nối Internet/3G/Wifi để tiếp tục.")
                            .setCancelable(false)
                            .setNegativeButton("Đóng", null)
                            .show();
                    return;
                }

                try {
                    hoSoCaTuVanModel = ((ReportAbuseInfoFragment) adapter.getItem(0)).GetInfoModel();
                    hoSoCaTuVanModel.CaTuVan = ((ReportAbuseContactFragment) adapter.getItem(3)).GetInfoModel();
                    fileSize = ((ReportAbuseContactFragment) adapter.getItem(3)).fileSize;
                    ArrayList<GalleryModel> fileReport = ((ReportAbuseContactFragment) adapter.getItem(3)).fileReport;

                    if (!validateInput()) {
                        return;
                    }

                    if (fileReport != null && fileReport.size() > 0) {
                        progressDialog.setVisibility(View.VISIBLE);
                        hoSoCaTuVanModel.CaTuVan.ListFiles = new ArrayList<>();
                        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.upload));
                        int index = 0;
                        for (GalleryModel galleryModel : fileReport) {
                            File file = new File(galleryModel.FileUrl);
                            anRequest.addMultipartFile("file" + index, file);
                            index++;
                        }
                        anRequest.addHeaders("Authorization", loginProfileModel.accessToken)
                                .setPriority(Priority.MEDIUM)
                                .build()
                                .getAsJSONObject(new JSONObjectRequestListener() {
                                    @Override
                                    public void onResponse(JSONObject response) {
                                        try {
                                            ResultModel<List<UploadFileResultModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<UploadFileResultModel>>>() {
                                            }.getType());

                                            for (UploadFileResultModel modelFile : resultObject.data) {
                                                hoSoCaTuVanModel.CaTuVan.ListFiles.add(modelFile.mappingName);
                                            }
                                        } catch (Exception ex) {
                                        }
                                        saveReportContent();
                                    }

                                    @Override
                                    public void onError(ANError anError) {
                                        saveReportContent();
                                    }
                                });
                    } else {
                        saveReportContent();
                    }
                } catch (Exception ex) {
                    progressDialog.setVisibility(View.GONE);
                    Toast.makeText(getActivity(), "Lỗi phát sinh trong quá trình xử lý.", Toast.LENGTH_SHORT).show();
                }
            }
        });

        lyBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Fragment fragment = new FragmentMenu();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });
    }

    private void saveReportContent() {
        if (!Utils.checkConnectedNetwork(getActivity())) {
            new android.support.v7.app.AlertDialog.Builder(getActivity())
                    .setTitle("Thông báo")
                    .setIcon(R.drawable.ic_warning)
                    .setMessage("Vui lòng kiểm tra kết nối Internet/3G/Wifi để tiếp tục.")
                    .setCancelable(false)
                    .setNegativeButton("Đóng", null)
                    .show();
            return;
        }

        progressDialog.setVisibility(View.VISIBLE);
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(hoSoCaTuVanModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.hoso))
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        String tem = "";
                        ResultModel<String> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<String>>() {
                        }.getType());
                        if (resultObject.status.equals(Constants.StatusSuccess)) {
                            dialogConfirm.show();
                        } else {
                            Toast.makeText(getActivity(), "Có lỗi phát sinh thêm mới hồ sơ!", Toast.LENGTH_SHORT).show();
                        }
                        progressDialog.setVisibility(View.GONE);
                    }

                    @Override
                    public void onError(ANError anError) {
                        progressDialog.setVisibility(View.GONE);
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    private void setupViewPager(ViewPager viewPager) {
        if (adapter != null && adapter.mFragmentList != null) {
            adapter.mFragmentList.clear();
            adapter.notifyDataSetChanged();
        }
        adapter = new SectionsPagerAdapter(getActivity().getSupportFragmentManager());
        adapter.addFragment(ReportAbuseInfoFragment.newInstance(maHoSo), "");
        adapter.addFragment(ReportAbuseChildFragment.newInstance(maHoSo), "");
        adapter.addFragment(ReportAbuseObjectFragment.newInstance(maHoSo), "");
        adapter.addFragment(ReportAbuseContactFragment.newInstance(), "");
        viewPager.setAdapter(adapter);
    }

    private boolean validateInput() {
        try {
            if (Utils.isEmpty(hoSoCaTuVanModel.CacVanDe)) {
                reportTabLayout.getTabAt(0).select();
                Toast.makeText(getActivity(), "Chưa lựa chọn các vấn đề!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseInfoFragment) adapter.getItem(0)).spnCacVanDe != null)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).spnCacVanDe.requestFocus();
                return false;
            }
            if (Utils.isEmpty(hoSoCaTuVanModel.NguonThongTin)) {
                reportTabLayout.getTabAt(0).select();
                Toast.makeText(getActivity(), "Chưa lựa chọn nguồn thông tin!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseInfoFragment) adapter.getItem(0)).spnNguonThongTin != null)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).spnNguonThongTin.requestFocus();
                return false;
            }
            if (Utils.isEmpty(hoSoCaTuVanModel.MaDoiTuong)) {
                reportTabLayout.getTabAt(0).select();
                Toast.makeText(getActivity(), "Chưa lựa chọn đối tượng cung cấp!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseInfoFragment) adapter.getItem(0)).spnDoiTuong != null)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).spnDoiTuong.requestFocus();
                return false;
            }
            if (Utils.isEmpty(hoSoCaTuVanModel.GioiTinh)) {
                reportTabLayout.getTabAt(0).select();
                Toast.makeText(getActivity(), "Chưa lựa chọn giới tính!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseInfoFragment) adapter.getItem(0)).rgGioiTinh != null)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).rgGioiTinh.requestFocus();
                return false;
            }
            if (Utils.isEmpty(hoSoCaTuVanModel.NamSinh)) {
                reportTabLayout.getTabAt(0).select();
                Toast.makeText(getActivity(), "Chưa nhập năm sinh!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseInfoFragment) adapter.getItem(0)).spnNamSinh != null)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).spnNamSinh.requestFocus();
                return false;
            }

            if (Utils.isEmpty(hoSoCaTuVanModel.MaDanToc)) {
                reportTabLayout.getTabAt(0).select();
                Toast.makeText(getActivity(), "Chưa lựa chọn dân tộc!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseInfoFragment) adapter.getItem(0)).spnDanToc != null)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).spnDanToc.requestFocus();
                return false;
            }

            if (!Utils.isEmpty(hoSoCaTuVanModel.Email)) {
                if (!Utils.phoneNumberValidator(hoSoCaTuVanModel.Email)) {
                    reportTabLayout.getTabAt(0).select();
                    Toast.makeText(getActivity(), "Email nhập không hợp lệ!", Toast.LENGTH_SHORT).show();
                    if (((ReportAbuseInfoFragment) adapter.getItem(0)).txtEmail != null)
                        ((ReportAbuseInfoFragment) adapter.getItem(0)).txtEmail.requestFocus();
                    return false;
                }
            }

            if (!Utils.isEmpty(hoSoCaTuVanModel.SoDienThoai)) {
                if (hoSoCaTuVanModel.SoDienThoai.length() != 10 || !Utils.phoneNumberValidator(hoSoCaTuVanModel.SoDienThoai)) {
                    reportTabLayout.getTabAt(0).select();
                    Toast.makeText(getActivity(), "Số điện thoại nhập không hợp lệ!", Toast.LENGTH_SHORT).show();
                    if (((ReportAbuseInfoFragment) adapter.getItem(0)).txtSoDienThoai != null)
                        ((ReportAbuseInfoFragment) adapter.getItem(0)).txtSoDienThoai.requestFocus();
                    return false;
                }
            }

            if (hoSoCaTuVanModel.LoaiCa.equals("2")) {
                if (Utils.isEmpty(hoSoCaTuVanModel.MoiTruongXamHai)) {
                    reportTabLayout.getTabAt(0).select();
                    Toast.makeText(getActivity(), "Chưa lựa môi trường xâm hại!", Toast.LENGTH_SHORT).show();
                    if (((ReportAbuseInfoFragment) adapter.getItem(0)).spnMoiTruongXamHai != null)
                        ((ReportAbuseInfoFragment) adapter.getItem(0)).spnMoiTruongXamHai.requestFocus();
                    return false;
                }
            }

            if (Utils.isEmpty(hoSoCaTuVanModel.MaTinhNguoiGoi)) {
                reportTabLayout.getTabAt(0).select();
                Toast.makeText(getActivity(), "Chưa lựa địa điểm Tỉnh/ Thành!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseInfoFragment) adapter.getItem(0)).spnTinhNguoiGoi != null)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).spnTinhNguoiGoi.requestFocus();
                return false;
            }

            if (Utils.isEmpty(hoSoCaTuVanModel.MaHuyenNguoiGoi)) {
                reportTabLayout.getTabAt(0).select();
                Toast.makeText(getActivity(), "Chưa lựa địa điểm Quận/ Huyện!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseInfoFragment) adapter.getItem(0)).spnHuyenNguoiGoi != null)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).spnHuyenNguoiGoi.requestFocus();
                return false;
            }

            if (Utils.isEmpty(hoSoCaTuVanModel.MaXaNguoiGoi)) {
                reportTabLayout.getTabAt(0).select();
                Toast.makeText(getActivity(), "Chưa lựa địa điểm Xã/ Phường!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseInfoFragment) adapter.getItem(0)).spnXaNguoiGoi != null)
                    ((ReportAbuseInfoFragment) adapter.getItem(0)).spnXaNguoiGoi.requestFocus();
                return false;
            }

            if (!Utils.isEmpty(hoSoCaTuVanModel.CaTuVan.Email)) {
                if (!Utils.emailValidator(hoSoCaTuVanModel.CaTuVan.Email)) {
                    reportTabLayout.getTabAt(3).select();
                    Toast.makeText(getActivity(), "Email nhập không hợp lệ!", Toast.LENGTH_SHORT).show();
                    if (((ReportAbuseContactFragment) adapter.getItem(3)).txtEmail != null)
                        ((ReportAbuseContactFragment) adapter.getItem(3)).txtEmail.requestFocus();
                    return false;
                }
            }

            if (!Utils.isEmpty(hoSoCaTuVanModel.CaTuVan.SoDienThoai)) {
                if (hoSoCaTuVanModel.CaTuVan.SoDienThoai.length() != 10 || !Utils.phoneNumberValidator(hoSoCaTuVanModel.CaTuVan.SoDienThoai)) {
                    reportTabLayout.getTabAt(3).select();
                    Toast.makeText(getActivity(), "Số điện thoại nhập không hợp lệ!", Toast.LENGTH_SHORT).show();
                    if (((ReportAbuseContactFragment) adapter.getItem(3)).txtSoDienThoai != null)
                        ((ReportAbuseContactFragment) adapter.getItem(3)).txtSoDienThoai.requestFocus();
                    return false;
                }
            }

            if (Utils.isEmpty(hoSoCaTuVanModel.CaTuVan.NoiDungTuVan)) {
                reportTabLayout.getTabAt(3).select();
                Toast.makeText(getActivity(), "Chưa nhập nội dung!", Toast.LENGTH_SHORT).show();
                if (((ReportAbuseContactFragment) adapter.getItem(3)).txtNoiDungTuVan != null)
                    ((ReportAbuseContactFragment) adapter.getItem(3)).txtNoiDungTuVan.requestFocus();
                return false;
            }

            if (fileSize > 100) {
                Toast.makeText(getActivity(), "Các hình ảnh/ video gửi kèm vượt quá 100 MB.", Toast.LENGTH_SHORT).show();
                reportTabLayout.getTabAt(3).select();
                return false;
            }

            return true;
        } catch (Exception ex) {
        }
        return false;
    }

    private boolean isDismiss = false;
    public void ConfirmDialog() {
        isDismiss = true;
        dialogConfirm = new Dialog(getContext());
        dialogConfirm.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialogConfirm.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogConfirm.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogConfirm.setContentView(R.layout.popup_confirm_success);
        LinearLayout btnBack = dialogConfirm.findViewById(R.id.lyBack);
        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                isDismiss = false;
                Fragment fragment = new FragmentMenu();
                Utils.ChangeFragment(getActivity(), fragment, null);
                dialogConfirm.dismiss();
            }
        });

        LinearLayout btnNext = dialogConfirm.findViewById(R.id.lyNext);
        btnNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                isDismiss = false;
                FragmentManager fm = getActivity().getSupportFragmentManager();
                for (int i = 0; i < fm.getBackStackEntryCount(); ++i) {
                    fm.popBackStack();
                }
                Fragment fragment = new ReportCaseCreateFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
                dialogConfirm.dismiss();
            }
        });

        dialogConfirm.setOnDismissListener(new DialogInterface.OnDismissListener() {
            @Override
            public void onDismiss(DialogInterface dialog) {
                if (isDismiss) {
                    Fragment fragment = new FragmentMenu();
                    Utils.ChangeFragment(getActivity(), fragment, null);
                    dialogConfirm.dismiss();
                }
            }
        });
    }
}
