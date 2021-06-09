package reportward.nts.reportward;

import android.annotation.SuppressLint;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.Snackbar;
import android.support.design.widget.TextInputLayout;
import android.support.v7.app.AppCompatActivity;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.MotionEvent;
import android.view.View;
import android.view.WindowManager;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.iid.FirebaseInstanceId;
import com.google.firebase.iid.InstanceIdResult;
import com.google.gson.Gson;
import com.google.gson.annotations.Until;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.Globals;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.ComboboxItem;
import reportward.nts.reportward.model.ComboboxResult;
import reportward.nts.reportward.model.LoginModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;

import static java.security.AccessController.getContext;

/**
 * A login screen that offers login via email/password.
 */
public class LoginActivity extends AppCompatActivity {
    private EditText txtUserName, txtPassword, spnProvince;
    private TextInputLayout tilUserName, tilPassword, tilEmail, tilProvinces;
    private ProgressBar progressBar;
    private Button btnLogin;
    private View parent_view;
    private TextView txtforwardPassword;
    private String tokenFirebase, area;
    private String[] arrayProvince;
    private List<ComboboxItem> listProvince;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        if (!Utils.checkConnectedNetwork(this)) {
            new android.support.v7.app.AlertDialog.Builder(this)
                    .setTitle("Thông báo")
                    .setIcon(R.drawable.ic_warning)
                    .setMessage("Vui lòng kiểm tra kết nối Internet/3G/Wifi để tiếp tục.")
                    .setCancelable(false)
                    .setNegativeButton("Đóng", null)
                    .show();
            return;
        }

        try {
            FirebaseInstanceId.getInstance().getInstanceId()
                    .addOnCompleteListener(new OnCompleteListener<InstanceIdResult>() {
                        @Override
                        public void onComplete(@NonNull Task<InstanceIdResult> task) {
                            try {
                                if (!task.isSuccessful()) {
                                    return;
                                }

                                tokenFirebase = task.getResult().getToken();
                            } catch (Exception ex) {
                            }
                        }
                    });
        } catch (Exception ex) {
        }

        parent_view = findViewById(android.R.id.content);

        this.getDataFix();

        tilUserName = (TextInputLayout) findViewById(R.id.tilUserName);
        tilPassword = (TextInputLayout) findViewById(R.id.tilPassword);
        txtUserName = (EditText) findViewById(R.id.txtUserName);
        txtPassword = (EditText) findViewById(R.id.txtPassword);
        btnLogin = (Button) findViewById(R.id.btnLogin);
        progressBar = (ProgressBar) findViewById(R.id.progressBar);
        txtforwardPassword = (TextView) findViewById(R.id.txtForwardPassword);
        spnProvince = (EditText) findViewById(R.id.spnProvince);
        spnProvince.setText(Globals.ProvinceName);
        spnProvince.setTag(Globals.ProvinceName);
        area = Globals.Area;

        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                submitLogin();
            }
        });

        TextView forgot_Password = parent_view.findViewById(R.id.txtForwardPassword);
        forgot_Password.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DialogForogotPassword();
            }
        });

        txtUserName.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                tilUserName.setErrorEnabled(false);
                tilPassword.setErrorEnabled(false);
            }
        });

        txtPassword.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                tilUserName.setErrorEnabled(false);
                tilPassword.setErrorEnabled(false);
            }
        });

        spnProvince.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnProvince, arrayProvince, listProvince, "Chọn Tỉnh/ Thành");

            }
        });
    }

    /***
     * Show Dialog chọn Tỉnh/Thành
     * @param editText
     * @param arrayName
     * @param listSource
     * @param title
     */
    private void showChooseDialog(final EditText editText, final String[] arrayName, final List<ComboboxItem> listSource, final String title) {
        final android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(this);
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, Utils.getIndexByName(arrayName, Utils.getText(editText)), new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                editText.setText(arrayName[which]);
                Globals.ProvinceName = arrayName[which];

                String id = listSource != null ? listSource.get(which).code : "";
                Globals.ProvinceId = id;
                editText.setTag(id);

                area = listSource.get(which).area;
                Globals.Area = area;
            }
        });
        builder.show();
    }

    private void getDataFix() {
        String tinhJson = Utils.ReadJSONFromAsset(this, "tinh_tp.json");
        listProvince = new Gson().fromJson(tinhJson, new TypeToken<ArrayList<ComboboxItem>>() {
        }.getType());

        if (listProvince != null && listProvince.size() > 0) {
            arrayProvince = new String[listProvince.size()];
            int index = 0;
            for (ComboboxItem item : listProvince) {
                arrayProvince[index] = item.name;
                index++;
            }
        }
    }

    private void DialogForogotPassword() {
        final Dialog dialog = new Dialog(this);
        dialog.setContentView(R.layout.popup_forgot_password);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        tilEmail = dialog.findViewById(R.id.tilEmail);

        final TextView txtEmail = dialog.findViewById(R.id.txtEmail);

        Button btnSendEmail = dialog.findViewById(R.id.btnSendEmail);
        btnSendEmail.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String email = txtEmail.getText().toString();

                if (email == null || email.isEmpty()) {
                    tilEmail.setError("Email không được để trống");
                } else {
                    if (!isEmailValid(email)) {
                        tilEmail.setError("Email không đúng định dạng");
                    } else {
                        AndroidNetworking.get(Utils.getUrlAPIByArea(area, LinkApi.resetpass) + email)
                                .setPriority(Priority.MEDIUM)
                                .build()
                                .getAsJSONObject(new JSONObjectRequestListener() {
                                    @Override
                                    public void onResponse(JSONObject response) {
                                        //progressDialog.dismiss();
                                        try {
                                            ResultModel<String> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<String>>() {
                                            }.getType());

                                            if (resultModel != null && resultModel.status.equals(Constants.StatusSuccess)) {
                                                dialog.dismiss();
                                                Snackbar.make(parent_view, resultModel.message, Snackbar.LENGTH_SHORT).show();
                                            } else {
                                                Snackbar.make(parent_view, resultModel.message, Snackbar.LENGTH_SHORT).show();
                                            }
                                        } catch (Exception ex) {
                                            Toast.makeText(getApplication(), "Có lỗi trong quá trình xử lý!", Toast.LENGTH_SHORT).show();
                                        }

                                    }

                                    @Override
                                    public void onError(ANError anError) {
                                        //progressDialog.dismiss();
                                        Utils.showErrorMessage(getApplication(), anError);
                                    }
                                });
                    }
                }


            }
        });

        Button btnClose = dialog.findViewById(R.id.btnClose);
        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialog.dismiss();
            }
        });
        dialog.show();
    }

    public static boolean isEmailValid(String email) {
        String expression = "^[\\w\\.-]+@([\\w\\-]+\\.)+[A-Z]{2,4}$";
        Pattern pattern = Pattern.compile(expression, Pattern.CASE_INSENSITIVE);
        Matcher matcher = pattern.matcher(email);
        return matcher.matches();
    }

    private void submitLogin() {
        if (Utils.isEmpty(tokenFirebase)) {
            try {
                FirebaseInstanceId.getInstance().getInstanceId()
                        .addOnCompleteListener(new OnCompleteListener<InstanceIdResult>() {
                            @Override
                            public void onComplete(@NonNull Task<InstanceIdResult> task) {
                                try {
                                    if (!task.isSuccessful()) {
                                        return;
                                    }

                                    tokenFirebase = task.getResult().getToken();
                                } catch (Exception ex) {
                                }
                            }
                        });
            } catch (Exception ex) {
            }

        }

        if (!validateUserName()) {
            return;
        }

        if (!validatePassword()) {
            return;
        }

        if (Utils.isEmpty(area)) {
            Snackbar.make(parent_view, "Hãy cho biết bạn thuộc tài khoản của tỉnh/thành nào?", Snackbar.LENGTH_SHORT).show();
            return;
        }

        LoginModel loginModel = new LoginModel();
        loginModel.TenDangNhap = txtUserName.getText().toString();
        loginModel.MatKhau = txtPassword.getText().toString();
        loginModel.deviceId = tokenFirebase;
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(loginModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        progressBar.setVisibility(View.VISIBLE);
        btnLogin.setVisibility(View.GONE);
        AndroidNetworking.post(Utils.getUrlAPIByArea(area, LinkApi.login))
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        progressBar.setVisibility(View.GONE);
                        btnLogin.setVisibility(View.VISIBLE);
                        try {
                            ResultModel<LoginProfileModel> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<LoginProfileModel>>() {
                            }.getType());
                            if (resultModel != null && resultModel.status.equals(Constants.StatusSuccess)) {
                                resultModel.data.area = area;
                                SharedPreferences.Editor editor = getSharedPreferences(Constants.Key_Info_Login, MODE_PRIVATE).edit();
                                Gson gson = new Gson();
                                String json = gson.toJson(resultModel.data);
                                editor.putString(Constants.LoginProfile, json);
                                editor.apply();

                                Snackbar.make(parent_view, resultModel.message, Snackbar.LENGTH_SHORT).show();
                                Intent mainActivity = new Intent(getApplicationContext(), MainActivity.class);
                                startActivity(mainActivity);
                                finish();
                            } else {
                                Snackbar.make(parent_view, resultModel.message, Snackbar.LENGTH_SHORT).show();
                            }
                        } catch (Exception ex) {
                            Snackbar.make(parent_view, "Lỗi trong quá trình đăng nhập!", Snackbar.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        progressBar.setVisibility(View.GONE);
                        btnLogin.setVisibility(View.VISIBLE);
                        Utils.showErrorMessage(getApplication(), anError);
                    }
                });
    }

    private boolean validateUserName() {
        String userName = txtUserName.getText().toString().trim();

        if (userName.isEmpty()) {
            tilUserName.setError("Tên đăng nhập không được để trống");
            requestFocus(txtUserName);
            return false;
        } else {
            tilUserName.setErrorEnabled(false);
        }

        return true;
    }

    private boolean validatePassword() {
        if (txtPassword.getText().toString().trim().isEmpty()) {
            tilPassword.setError("Mật khẩu không được để trống");
            requestFocus(txtPassword);
            return false;
        } else if (txtPassword.getText().length() < 5) {
            tilPassword.setError("Mật khẩu phải lớn hơn 5 kí tự");
            requestFocus(txtPassword);
            return false;
        } else {
            tilPassword.setErrorEnabled(false);
        }

        return true;
    }

    private void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }


}

