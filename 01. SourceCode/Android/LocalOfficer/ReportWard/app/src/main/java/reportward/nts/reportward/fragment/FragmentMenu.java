package reportward.nts.reportward.fragment;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.animation.AlphaAnimation;
import android.view.animation.Animation;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import reportward.nts.reportward.LoginActivity;
import reportward.nts.reportward.R;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.LoginModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;

import static android.content.Context.MODE_PRIVATE;

public class FragmentMenu extends Fragment {
    private View view;
    private TextView total;
    private Animation in;
    private LinearLayout lyTuoiGTDT, lyDiaBan, lyKetQuaHoTro, lyLoaiCaMoiTruong,
            lyBaoLuc, lyMuaBan, lyTinhDuc, lyXamHai;
    private ActionBar actionBar;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_menu, container, false);

        actionBar = ((AppCompatActivity) getActivity()).getSupportActionBar();
        actionBar.setTitle("Báo cáo BVTE");

        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }

        total = view.findViewById(R.id.txtTotalNotify);


        TextView tv_Name = view.findViewById(R.id.tv_Name);
        tv_Name.setText(loginProfileModel.tenDangNhap);

        final LinearLayout lyLogout = view.findViewById(R.id.lyLogout);
        lyLogout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Logout();
            }
        });

        LinearLayout search = view.findViewById(R.id.cardView_Search);
        search.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
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

                actionBar.setTitle("Tra cứu ca");
                Fragment fragment = new ListSearchReportFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });


        LinearLayout report = view.findViewById(R.id.cardView_Report);
        report.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
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

                actionBar.setTitle("Báo cáo ca");
                removeFragmentViewPager();
                Fragment fragment = new ReportCaseCreateFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        in = new AlphaAnimation(0.0f, 1.0f);
        in.setDuration(500);

        final LinearLayout change_Password = view.findViewById(R.id.lyChangePassword);
        change_Password.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DialogChangePassword();
            }
        });

        lyTuoiGTDT = view.findViewById(R.id.lyTuoiGTDT);
        lyDiaBan = view.findViewById(R.id.lyDiaBan);
        lyKetQuaHoTro = view.findViewById(R.id.lyKetQuaHoTro);
        lyLoaiCaMoiTruong = view.findViewById(R.id.lyLoaiCaMoiTruong);
        lyBaoLuc = view.findViewById(R.id.lyBaoLuc);
        lyMuaBan = view.findViewById(R.id.lyMuaBan);
        lyTinhDuc = view.findViewById(R.id.lyTinhDuc);
        lyXamHai = view.findViewById(R.id.lyXamHai);

        lyTuoiGTDT.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
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

                actionBar.setTitle("Thống kê báo cáo");
                removeFragmentViewPager();
                Fragment fragment = new BaoCaoTuoiGTDTFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyDiaBan.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
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

                actionBar.setTitle("Thống kê báo cáo");
                Fragment fragment = new BaoCaoDiabanFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyKetQuaHoTro.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
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

                actionBar.setTitle("Thống kê báo cáo");
                Fragment fragment = new fragment_ketqua_hotro();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyLoaiCaMoiTruong.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
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

                actionBar.setTitle("Thống kê báo cáo");
                Fragment fragment = new Fragment_LoaiCa_MoiTruong();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyBaoLuc.setOnClickListener(new View.OnClickListener() {
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

                actionBar.setTitle("Thống kê báo cáo");
                removeFragmentViewPager();
                Fragment fragment = new BaoCaoThuPhamFragment();
                Bundle bundle = new Bundle();
                bundle.putString("urlApi", LinkApi.thuphambaoluctreem);
                bundle.putString("title", "Báo cáo thủ phạm bạo lực trẻ em");
                fragment.setArguments(bundle);
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyMuaBan.setOnClickListener(new View.OnClickListener() {
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

                actionBar.setTitle("Thống kê báo cáo");
                removeFragmentViewPager();
                Fragment fragment = new BaoCaoThuPhamFragment();
                Bundle bundle = new Bundle();
                bundle.putString("urlApi", LinkApi.thuphammuabantreem);
                bundle.putString("title", "Báo cáo thủ phạm mua bán trẻ em");
                fragment.setArguments(bundle);
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyTinhDuc.setOnClickListener(new View.OnClickListener() {
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

                actionBar.setTitle("Thống kê báo cáo");
                removeFragmentViewPager();
                Fragment fragment = new BaoCaoThuPhamFragment();
                Bundle bundle = new Bundle();
                bundle.putString("urlApi", LinkApi.thuphamxamhaitinhductreem);
                bundle.putString("title", "Báo cáo thủ phạm xâm hại tình dục trẻ em");
                fragment.setArguments(bundle);
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });

        lyXamHai.setOnClickListener(new View.OnClickListener() {
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

                actionBar.setTitle("Thống kê báo cáo");
                removeFragmentViewPager();
                Fragment fragment = new BaoCaoThuPhamFragment();
                Bundle bundle = new Bundle();
                bundle.putString("urlApi", LinkApi.thuphamxamhaitreem);
                bundle.putString("title", "Báo cáo thủ phạm xâm hại trẻ em");
                fragment.setArguments(bundle);
                Utils.ChangeFragment(getActivity(), fragment, null);
            }
        });
        return view;
    }

    private void Logout() {
        AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle("Đăng xuất!");
        builder.setMessage("Bạn có chắc muốn thoát tài khoản này?");
        builder.setPositiveButton("Đăng xuất", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                try {
                    SharedPreferences.Editor editor = prefsLogin.edit();
                    editor.remove(Constants.LoginProfile);
                    editor.apply();
                    AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.logout) + loginProfileModel.accessToken)
                            .addHeaders("Authorization", loginProfileModel.accessToken)
                            .setPriority(Priority.MEDIUM)
                            .build()
                            .getAsJSONObject(new JSONObjectRequestListener() {
                                @Override
                                public void onResponse(JSONObject response) {
                                    try {
                                        ResultModel<String> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<String>>() {
                                        }.getType());

                                    } catch (Exception ex) {

                                    }
                                }

                                @Override
                                public void onError(ANError anError) {
                                    Intent loginActivity = new Intent(getActivity(), LoginActivity.class);
                                    startActivity(loginActivity);
                                    getActivity().finish();
                                }
                            });

                    Intent loginActivity = new Intent(getActivity(), LoginActivity.class);
                    startActivity(loginActivity);
                    getActivity().finish();
                } catch (Exception ex) {
                    Intent loginActivity = new Intent(getActivity(), LoginActivity.class);
                    startActivity(loginActivity);
                    getActivity().finish();
                }
            }
        });
        builder.setNegativeButton("Hủy", null);
        builder.show();
    }

    private class ChangePasswordModel {
        public String matKhauCu;
        public String matKhau;
    }

    private void DialogChangePassword() {
        final Dialog dialog = new Dialog(getContext());
        dialog.setContentView(R.layout.popup_change_password);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;

        final EditText txtOldPassword = dialog.findViewById(R.id.txtOldPassword);
        final EditText txtNewPassword = dialog.findViewById(R.id.txtNewPassword);
        final EditText txtConfirmPassword = dialog.findViewById(R.id.txtConfirmPassword);
        //Button btnChangePassword = dialog.findViewById(R.id.btnChangePassword);
        final ChangePasswordModel changePasswordModel = new ChangePasswordModel();
        final TextView txtExit = dialog.findViewById(R.id.txtExit);
        txtExit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                txtExit.startAnimation(in);
                dialog.dismiss();
            }
        });

        final TextView txtChangePassword = dialog.findViewById(R.id.txtChangePassword);
        txtChangePassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                txtChangePassword.startAnimation(in);
                if (txtOldPassword.getText().toString().isEmpty() || txtOldPassword.getText().toString() == null) {
                    txtOldPassword.requestFocus();
                    Toast.makeText(getActivity(), "Nhập mật khẩu cũ", Toast.LENGTH_LONG).show();
                } else {
                    if (txtNewPassword.getText().toString().isEmpty() || txtNewPassword.getText().toString() == null) {
                        txtNewPassword.requestFocus();
                        Toast.makeText(getActivity(), "Nhập mật khẩu mới", Toast.LENGTH_LONG).show();
                    } else {
                        if (txtConfirmPassword.getText().toString().isEmpty() || txtConfirmPassword.getText().toString() == null) {
                            txtConfirmPassword.requestFocus();
                            Toast.makeText(getActivity(), "Nhập xác nhận mật khẩu mới", Toast.LENGTH_LONG).show();
                        } else {
                            if (!txtNewPassword.getText().toString().equals(txtConfirmPassword.getText().toString())) {
                                Toast.makeText(getActivity(), "Xác nhận mật khẩu mới không đúng", Toast.LENGTH_LONG).show();
                            } else {
                                if (txtNewPassword.getText().toString().length() <= 5) {
                                    Toast.makeText(getActivity(), "Mật khẩu mới phải lớn hơn 5 kí tự", Toast.LENGTH_LONG).show();
                                } else {
                                    changePasswordModel.matKhau = txtNewPassword.getText().toString();
                                    changePasswordModel.matKhauCu = txtOldPassword.getText().toString();
                                    JSONObject jsonModel = new JSONObject();
                                    try {
                                        jsonModel = new JSONObject(new Gson().toJson(changePasswordModel));
                                    } catch (JSONException e) {
                                        // MessageUtils.Show(getActivity(), e.getMessage());
                                    }

                                    AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.changePass))
                                            .addHeaders("Authorization", loginProfileModel.accessToken)
                                            .addJSONObjectBody(jsonModel)
                                            .setPriority(Priority.MEDIUM)
                                            .build()
                                            .getAsJSONObject(new JSONObjectRequestListener() {
                                                @Override
                                                public void onResponse(JSONObject response) {
                                                    try {
                                                        ResultModel<String> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<String>>() {
                                                        }.getType());
                                                        if (resultModel.status.equals(Constants.StatusSuccess)) {
                                                            dialog.dismiss();
                                                            LoginModel loginModel = new LoginModel();
                                                            loginModel.TenDangNhap = loginProfileModel.tenDangNhap;
                                                            loginModel.MatKhau = changePasswordModel.matKhau;
                                                            JSONObject jsonLoginModel = new JSONObject();
                                                            try {
                                                                jsonLoginModel = new JSONObject(new Gson().toJson(loginModel));
                                                            } catch (JSONException e) {
                                                                // MessageUtils.Show(getActivity(), e.getMessage());
                                                            }
                                                            AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.login))
                                                                    .addJSONObjectBody(jsonLoginModel)
                                                                    .setPriority(Priority.MEDIUM)
                                                                    .build()
                                                                    .getAsJSONObject(new JSONObjectRequestListener() {
                                                                        @Override
                                                                        public void onResponse(JSONObject response) {
                                                                            try {
                                                                                ResultModel<LoginProfileModel> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<LoginProfileModel>>() {
                                                                                }.getType());
                                                                                loginProfileModel = resultModel.data;

                                                                            } catch (Exception ex) {

                                                                            }
                                                                        }

                                                                        @Override
                                                                        public void onError(ANError anError) {

                                                                        }
                                                                    });
                                                            Toast.makeText(getActivity(), resultModel.message, Toast.LENGTH_LONG).show();
                                                        } else {
                                                            Toast.makeText(getActivity(), resultModel.message, Toast.LENGTH_LONG).show();
                                                        }
                                                    } catch (Exception ex) {

                                                    }

                                                }

                                                @Override
                                                public void onError(ANError anError) {
                                                }
                                            });
                                }
                            }
                        }
                    }
                }
            }
        });

        dialog.show();
    }

    private void removeFragmentViewPager() {
        for (Fragment fragment : getActivity().getSupportFragmentManager().getFragments()) {
            try {
                if (fragment instanceof BaoCaoThuPhamGioiTinhFragment || fragment instanceof BaoCaoThuPhamDoTuoiFragment
                        || fragment instanceof BaoCaoDoTuoiFragment || fragment instanceof BaoCaoGioiTinhFragment || fragment instanceof BaoCaoDanTocFragment
                        || fragment instanceof ReportAbuseInfoFragment || fragment instanceof ReportAbuseChildFragment
                        || fragment instanceof ReportAbuseObjectFragment || fragment instanceof ReportAbuseContactFragment) {
                    getActivity().getSupportFragmentManager().beginTransaction().remove(fragment).commit();
                }
            } catch (NullPointerException e) {

            }
        }
    }
}
