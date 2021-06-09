package nts.childprofile;

import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.support.design.widget.Snackbar;
import android.support.design.widget.TextInputLayout;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Switch;
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

import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.List;

import nts.childprofile.Sql.DataBaseHelper;
import nts.childprofile.common.Constants;
import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.model.ChildProfileModel;
import nts.childprofile.model.LoginModel;
import nts.childprofile.model.LoginProfileModel;

/**
 * A login screen that offers login via email/password.
 */
public class LoginActivity extends AppCompatActivity {
    private EditText txtUserName, txtPassword;
    private TextInputLayout tilUserName, tilPassword;
    private ProgressBar progressBar;
    private Button btnLogin;
    private View parent_view;
    private GlobalVariable global;
    private TextView txtforwardPassword;
    private Switch swMode;
    private SharedPreferences prefs;
    private LoginProfileModel loginProfileModel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        parent_view = findViewById(android.R.id.content);

        global = (GlobalVariable) getApplication();
        prefs = getSharedPreferences("UserData", MODE_PRIVATE);

        tilUserName = (TextInputLayout) findViewById(R.id.tilUserName);
        tilPassword = (TextInputLayout) findViewById(R.id.tilPassword);
        txtUserName = (EditText) findViewById(R.id.txtUserName);
        txtPassword = (EditText) findViewById(R.id.txtPassword);
        btnLogin = (Button) findViewById(R.id.btnLogin);
        progressBar = (ProgressBar) findViewById(R.id.progressBar);
        txtforwardPassword = (TextView) findViewById(R.id.txtForwardPassword);
        swMode = (Switch) findViewById(R.id.swMode);
        swMode.setTextOn("On");
        swMode.setTextOff("Off");
        swMode.setChecked(global.isStatusRunApp());
        swMode.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                global.setStatusRunApp(swMode.isChecked());
            }
        });

        swMode.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                global.setStatusRunApp(swMode.isChecked());
            }
        });
        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                submitLogin();
            }
        });

        txtforwardPassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent forwardPasswordActivity = new Intent(getApplicationContext(), ForwardPasswordActivity.class);
                startActivity(forwardPasswordActivity);
            }
        });

        PackageInfo pInfo = null;
        try {
            pInfo = getPackageManager().getPackageInfo(getPackageName(), 0);
            String version = pInfo.versionName;
            TextView txtVersion = findViewById(R.id.txtVersion);
            txtVersion.setText("Phiên bản/Version "+version);
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }

    }

    @Override
    protected void onResume() {
        super.onResume();
        global = (GlobalVariable) getApplication();
        prefs = getSharedPreferences("UserData", MODE_PRIVATE);
    }

    private void submitLogin() {
        if (!validateUserName()) {
            return;
        }

        if (!validatePassword()) {
            return;
        }

        final LoginModel loginModel = new LoginModel();
        loginModel.UserName = txtUserName.getText().toString();
        loginModel.Password = txtPassword.getText().toString();
        Boolean isStatus = global.isStatusRunApp();

        if (isStatus) {
            String passWord = PassWordHash(loginModel.Password);
            String localUserName = (prefs.getString("username", ""));
            String localPassword = (prefs.getString("password", ""));

            if (loginModel.UserName.equals(localUserName) && passWord.equals(localPassword)) {
                Snackbar.make(parent_view, "Đăng nhập thành công", Snackbar.LENGTH_SHORT).show();

                LoginProfileModel resultObject = new LoginProfileModel();
                resultObject.Id = (prefs.getString("Id", ""));
                resultObject.Name = (prefs.getString("Name", ""));
                resultObject.UserName = (prefs.getString("UserName", ""));
                resultObject.UserLever = (prefs.getString("UserLever", ""));
                resultObject.AreaUserId = (prefs.getString("AreaUserId", ""));
                resultObject.ProvinceId = (prefs.getString("ProvinceId", ""));
                resultObject.DistrictId = (prefs.getString("DistrictId", ""));
                resultObject.WardId = (prefs.getString("WardId", ""));
                resultObject.IsDisable = (prefs.getBoolean("IsDisable", false));
                resultObject.ImagePath = (prefs.getString("ImagePath", ""));
                resultObject.WardId = (prefs.getString("WardId", ""));

                global.addLoginProfile(resultObject);
                Intent mainActivity = new Intent(getApplicationContext(), MainActivity.class);
                startActivity(mainActivity);
            } else {
                Snackbar.make(parent_view, "Tài khoản hoặc mật khẩu không đúng", Snackbar.LENGTH_SHORT).show();
            }
        } else {
            JSONObject jsonModel = new JSONObject();
            try {
                jsonModel = new JSONObject(new Gson().toJson(loginModel));
            } catch (JSONException e) {
                // MessageUtils.Show(getActivity(), e.getMessage());
            }
            progressBar.setVisibility(View.VISIBLE);
            btnLogin.setVisibility(View.GONE);
            AndroidNetworking.post(Utils.GetUrlApi("api/Authorize/Login"))
                    .addJSONObjectBody(jsonModel)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            progressBar.setVisibility(View.GONE);
                            btnLogin.setVisibility(View.VISIBLE);
                            LoginProfileModel resultObject = new Gson().fromJson(response.toString(), new TypeToken<LoginProfileModel>() {
                            }.getType());
                            if (resultObject.UserLever.equals(Constants.LevelTeacher)) {

                                String passWordHash = PassWordHash(loginModel.Password);
                                SharedPreferences.Editor editor = prefs.edit();
                                editor.putString("username", loginModel.UserName);
                                editor.putString("password", passWordHash);
                                editor.putString("Id", resultObject.Id);
                                editor.putString("Name", resultObject.Name);
                                editor.putString("UserName", resultObject.UserName);
                                editor.putString("UserLever", resultObject.UserLever);
                                editor.putString("AreaUserId", resultObject.AreaUserId);
                                editor.putString("ProvinceId", resultObject.ProvinceId);
                                editor.putString("DistrictId", resultObject.DistrictId);
                                editor.putString("ImagePath", resultObject.ImagePath);
                                editor.putBoolean("IsDisable", resultObject.IsDisable);
                                editor.putString("WardId", resultObject.WardId);
                                editor.apply();
                                global.addLoginProfile(resultObject);
                                Utils.getBackgroundData(getApplication(), resultObject, false);
                                Snackbar.make(parent_view, "Đăng nhập thành công", Snackbar.LENGTH_SHORT).show();
                                Intent mainActivity = new Intent(getApplicationContext(), MainActivity.class);
                                startActivity(mainActivity);
//                                SyncChildProfile();

                            } else {
                                Snackbar.make(parent_view, "Chỉ có tài khoản giáo viên mới được sử dụng ứng dụng này.", Snackbar.LENGTH_SHORT).show();
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            progressBar.setVisibility(View.GONE);
                            btnLogin.setVisibility(View.VISIBLE);
//                            Utils.showErrorMessage(getApplication(), anError);
                            Toast.makeText(getApplication(), "Có lỗi trong quá trình xử lý", Toast.LENGTH_SHORT).show();
                        }
                    });
        }
    }

    public void SyncChildProfile() {
        loginProfileModel = global.getLoginProfile();
        new Thread(new Runnable() {
            @Override
            public void run() {
                DataBaseHelper dataBaseHelper = new DataBaseHelper(getApplicationContext(), Constants.DATABASE_NAME, null, 1);
                List<ChildProfileModel> listChildProfile = new ArrayList<>();
                listChildProfile = dataBaseHelper.getListChildProfileToSync();

                try {
                    if (listChildProfile.size() > 0) {
//                syncProgress.setVisibility(View.VISIBLE);
                        for (ChildProfileModel childProfileModel : listChildProfile) {
                            childProfileModel.UserLever = loginProfileModel.UserLever;
                            if (childProfileModel.TypeChildProfile.equals(Constants.TYPE_CHILDPROFILE_SQLITE_CREATED)) {
                                CreateChildProfile(childProfileModel);
                            } else if (childProfileModel.TypeChildProfile.equals(Constants.TYPE_CHILDPROFILE_SQLITE_UPDATED)) {
                                UpdatẹChildProfile(childProfileModel);
                            }
                        }
//                syncProgress.setVisibility(View.GONE);
//                        Toast.makeText(getApplication(), "Đồng bộ hồ sơ trẻ thành công", Toast.LENGTH_LONG).show();
                    }
                } catch (Exception ex) {
//                    Toast.makeText(getApplication(), "Đồng bộ hồ sơ trẻ không thành công", Toast.LENGTH_LONG).show();
                }
            }
        }).start();

    }

    private void CreateChildProfile(final ChildProfileModel childProfileModel) {
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
                        DataBaseHelper dataBaseHelper = new DataBaseHelper(getApplication(), Constants.DATABASE_NAME, null, 1);
                        dataBaseHelper.delete(Constants.DATABASE_TABLE_ChildProfile, childProfileModel.Id);

                        if (!Utils.isEmpty(childProfileModel.ImagePath) && !childProfileModel.ImagePath.contains("https")) {
                            File image = new File(childProfileModel.ImagePath);
                            image.delete();
                        }
                        if (!Utils.isEmpty(childProfileModel.ImageSignaturePath) && !childProfileModel.ImageSignaturePath.contains("https")) {
                            File signature = new File(childProfileModel.ImageSignaturePath);
                            signature.delete();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
//                        progressDialog.setVisibility(View.GONE);
//                        Utils.showErrorMessage(getApplication(), anError);
                    }
                });
    }

    private void UpdatẹChildProfile(final ChildProfileModel childProfileModel) {
        childProfileModel.UpdateBy = loginProfileModel.Id;
        String jsonModel = new Gson().toJson(childProfileModel);
        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/ChildProfiles/UpdateChildProfile"));
        if (!Utils.isEmpty(childProfileModel.ImagePath) && !childProfileModel.ImagePath.contains("https")) {
            File file = new File(childProfileModel.ImagePath);
            anRequest.addMultipartFile("Avatar", file);
        }
//        progressDialog.setVisibility(View.VISIBLE);
        anRequest.addMultipartParameter("Model", jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String response) {
//                        progressDialog.setVisibility(View.GONE);
                        DataBaseHelper dataBaseHelper = new DataBaseHelper(getApplication(), Constants.DATABASE_NAME, null, 1);
                        dataBaseHelper.delete(Constants.DATABASE_TABLE_ChildProfile, childProfileModel.Id);

                        if (!Utils.isEmpty(childProfileModel.ImagePath) && !childProfileModel.ImagePath.contains("https")) {
                            File image = new File(childProfileModel.ImagePath);
                            image.delete();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
//                        Utils.showErrorMessage(getApplication(), anError);
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

    private String PassWordHash(String password) {
        StringBuilder sb = new StringBuilder();
        try {
            MessageDigest digest = MessageDigest.getInstance("SHA-256");
            byte[] hash = digest.digest(password.getBytes("UTF-8"));

            for (int i = 0; i < hash.length; i++) {
                sb.append(Integer.toString((hash[i] & 0xff) + 0x100, 16).substring(1));
            }

        } catch (NoSuchAlgorithmException | UnsupportedEncodingException ex) {
        }
        return sb.toString();
    }
}

