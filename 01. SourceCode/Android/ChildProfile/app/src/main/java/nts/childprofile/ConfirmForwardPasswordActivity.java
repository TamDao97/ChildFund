package nts.childprofile;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.TextInputEditText;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.Fragment;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.androidnetworking.interfaces.StringRequestListener;
import com.google.gson.Gson;

import org.json.JSONException;
import org.json.JSONObject;

import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.model.ForwardPasswordModel;
import nts.childprofile.model.LoginProfileModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * create an instance of this fragment.
 */
public class ConfirmForwardPasswordActivity extends AppCompatActivity {
    private TextInputEditText txtConfirmKey, txtPasswordNew, txtConfirmPasswordNew;
    private TextInputLayout tilConfirmKey, tilPasswordNew, tilConfirmPasswordNew;
    private ForwardPasswordModel forwardPasswordModel = new ForwardPasswordModel();
    private Button btnNext, btnClose;
    private GlobalVariable global;
    private LoginProfileModel loginProfileModel;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_confirm_forward_password);

        global = (GlobalVariable) getApplication();
        loginProfileModel = global.getLoginProfile();

        initLayout();
    }

    ///Khởi tạo thuộc tính trên giao diện
    private void initLayout() {
        txtConfirmKey = (TextInputEditText) findViewById(R.id.txtConfirmKey);
        txtPasswordNew = (TextInputEditText) findViewById(R.id.txtPasswordNew);
        txtConfirmPasswordNew = (TextInputEditText) findViewById(R.id.txtConfirmPasswordNew);

        tilConfirmKey = (TextInputLayout) findViewById(R.id.tilConfirmKey);
        tilPasswordNew = (TextInputLayout) findViewById(R.id.tilPasswordNew);
        tilConfirmPasswordNew = (TextInputLayout) findViewById(R.id.tilConfirmPasswordNew);

        btnNext = (Button) findViewById(R.id.btnNext);
        btnClose = (Button) findViewById(R.id.btnClose);

        /***
         * Chuyển bước
         */
        btnNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                nextStep();
            }
        });

        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent loginActivity = new Intent(getApplicationContext(), LoginActivity.class);
                startActivity(loginActivity);
            }
        });
    }

    /***
     * Chuyển bước
     */
    private void nextStep() {
        getInfoInFrom();
        forwardPasswordModel.Id = loginProfileModel.Id;

        if (!validateFrom()) {
            return;
        }

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(forwardPasswordModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.GetUrlApi("api/Authorize/ConfirmForwardPassword"))
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String  response) {
                        Intent loginActivity = new Intent(getApplicationContext(), LoginActivity.class);
                        startActivity(loginActivity);
                    }

                    @Override
                    public void onError(ANError anError) {
                        Utils.showErrorMessage(getApplication(),anError);
                    }
                });
    }

    /***
     * Lấy thông tin trên giao diện
     */
    private void getInfoInFrom() {
        try {
            forwardPasswordModel.ConfirmKey = txtConfirmKey.getText().toString();
            forwardPasswordModel.PasswordNew = txtPasswordNew.getText().toString();
            forwardPasswordModel.ConfirmPasswordNew = txtConfirmPasswordNew.getText().toString();
        } catch (Exception ex) {
        }
    }

    /***
     * Check dữ liệu
     * @return
     */
    private boolean validateFrom() {
        if (forwardPasswordModel.ConfirmKey.isEmpty()) {
            tilConfirmKey.setError("Không được để trống.");
            txtConfirmKey.requestFocus();
            return false;
        } else {
            tilConfirmKey.setErrorEnabled(false);
        }

        if (forwardPasswordModel.PasswordNew.isEmpty()) {
            tilPasswordNew.setError("Không được để trống.");
            txtPasswordNew.requestFocus();
            return false;
        } else {
            tilPasswordNew.setErrorEnabled(false);
        }

        if (forwardPasswordModel.ConfirmPasswordNew.isEmpty()) {
            tilConfirmPasswordNew.setError("Không được để trống.");
            txtConfirmPasswordNew.requestFocus();
            return false;
        } else {
            tilConfirmPasswordNew.setErrorEnabled(false);
        }

        if (!forwardPasswordModel.PasswordNew.equals(forwardPasswordModel.ConfirmPasswordNew)) {
            tilConfirmPasswordNew.setError("Hai mật khẩu mới không khớp nhau.");
            txtConfirmPasswordNew.requestFocus();
            return false;
        } else {
            tilConfirmPasswordNew.setErrorEnabled(false);
        }
        return true;
    }
}
