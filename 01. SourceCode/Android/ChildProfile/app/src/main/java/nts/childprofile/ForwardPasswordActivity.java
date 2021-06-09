package nts.childprofile;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.TextInputEditText;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.Fragment;
import android.support.v7.app.AppCompatActivity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.androidnetworking.interfaces.StringRequestListener;
import com.google.gson.Gson;

import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONStringer;

import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.model.ForwardPasswordModel;
import nts.childprofile.model.LoginProfileModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * create an instance of this fragment.
 */
public class ForwardPasswordActivity extends AppCompatActivity {
    private EditText txtUserName, txtEmail;
    private TextInputLayout tilUserName, tilEmail;
    private ForwardPasswordModel forwardPasswordModel = new ForwardPasswordModel();
    private Button btnNext,btnClose;
    private GlobalVariable global;
    private LoginProfileModel loginProfileModel;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_forward_password);

        global = (GlobalVariable) getApplication();
        loginProfileModel = global.getLoginProfile();

        initLayout();
    }

    ///Khởi tạo thuộc tính trên giao diện
    private void initLayout() {
        txtUserName = (EditText) findViewById(R.id.txtUserName);
        txtEmail = (EditText) findViewById(R.id.txtEmail);

        tilUserName = (TextInputLayout) findViewById(R.id.tilUserName);
        tilEmail = (TextInputLayout) findViewById(R.id.tilEmail);

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

        if (!validateFrom()) {
            return;
        }

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(forwardPasswordModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.GetUrlApi("api/Authorize/ForwardPassword"))
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String response) {
                        loginProfileModel.Id = response.replace('"',' ').trim();
                        Intent confirmForwardPasswordActivity = new Intent(getApplicationContext(), ConfirmForwardPasswordActivity.class);
                        startActivity(confirmForwardPasswordActivity);
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
            forwardPasswordModel.UserName = txtUserName.getText().toString();
            forwardPasswordModel.Email = txtEmail.getText().toString();
        } catch (Exception ex) {
        }
    }

    /***
     * Check dữ liệu
     * @return
     */
    private boolean validateFrom() {
        if (forwardPasswordModel.UserName.isEmpty()) {
            tilUserName.setError("Không được để trống.");
            txtUserName.requestFocus();
            return false;
        } else {
            tilUserName.setErrorEnabled(false);
        }

        if (forwardPasswordModel.Email.isEmpty()) {
            tilEmail.setError("Không được để trống.");
            txtEmail.requestFocus();
            return false;
        } else {
            tilEmail.setErrorEnabled(false);
        }

        if (!Utils.emailValidator(forwardPasswordModel.Email)) {
            tilEmail.setError("Không đúng định dạng.");
            txtEmail.requestFocus();
            return false;
        } else {
            tilEmail.setErrorEnabled(false);
        }
        return true;
    }
}
