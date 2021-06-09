package nts.childprofile.fragment;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.TextInputEditText;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.Fragment;
import android.support.v7.app.ActionBar;
import android.support.v7.widget.Toolbar;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.androidnetworking.interfaces.StringRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import nts.childprofile.LoginActivity;
import nts.childprofile.MainActivity;
import nts.childprofile.R;
import nts.childprofile.adapter.ProfileListAdapter;
import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.model.ChangePasswordUserModel;
import nts.childprofile.model.ChildProfileSearchResult;
import nts.childprofile.model.LoginProfileModel;
import nts.childprofile.model.SearchResultObject;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * create an instance of this fragment.
 */
public class ChangePasswordFragment extends Fragment {
    private View view;
    private TextInputEditText txtPasswordOld, txtPasswordNew, txtConfirmPasswordNew;
    private TextInputLayout tilPasswordOld, tilPasswordNew, tilConfirmPasswordNew;
    private ChangePasswordUserModel changePasswordModel = new ChangePasswordUserModel();
    private Button btnSave;
    private GlobalVariable global;
    private LoginProfileModel loginProfileModel;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_change_password, container, false);

        global = (GlobalVariable) getActivity().getApplication();
        loginProfileModel = global.getLoginProfile();

        Toolbar mToolbar = (Toolbar) getActivity().findViewById(R.id.toolbar);
        TextView toolbarTitle = mToolbar.findViewById(R.id.toolbar_title);
        toolbarTitle.setText("Đổi mật khẩu/Change password");
        Toolbar.LayoutParams layoutParams = (Toolbar.LayoutParams) toolbarTitle.getLayoutParams();
        layoutParams.gravity = Gravity.LEFT;
        toolbarTitle.setLayoutParams(layoutParams);

        ImageView imgLogo = mToolbar.findViewById(R.id.imgLogo);
        imgLogo.setVisibility(View.VISIBLE);

        initLayout();
        // Inflate the layout for this fragment
        return view;
    }

    ///Khởi tạo thuộc tính trên giao diện
    private void initLayout() {
        txtPasswordOld = (TextInputEditText) view.findViewById(R.id.txtPasswordOld);
        txtPasswordNew = (TextInputEditText) view.findViewById(R.id.txtPasswordNew);
        txtConfirmPasswordNew = (TextInputEditText) view.findViewById(R.id.txtConfirmPasswordNew);

        tilPasswordOld = (TextInputLayout) view.findViewById(R.id.tilPasswordOld);
        tilPasswordNew = (TextInputLayout) view.findViewById(R.id.tilPasswordNew);
        tilConfirmPasswordNew = (TextInputLayout) view.findViewById(R.id.tilConfirmPasswordNew);

        btnSave = (Button) view.findViewById(R.id.btnSave);

        /***
         * Lưu thông tin
         */
        btnSave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                savePassword();
            }
        });
    }

    /***
     * Lưu mật khẩu
     */
    private void savePassword() {
        getInfoInFrom();
        changePasswordModel.Id = loginProfileModel.Id;

        if (!validateFrom()) {
            return;
        }

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(changePasswordModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.GetUrlApi("api/Authorize/ChangePasswordUser"))
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String response) {
                        Toast.makeText(getActivity(), "Thay đổi mật khẩu thành công!", Toast.LENGTH_SHORT).show();
                        Intent loginActivity = new Intent(getActivity().getApplicationContext(), LoginActivity.class);
                        startActivity(loginActivity);
                    }

                    @Override
                    public void onError(ANError anError) {
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    /***
     * Lấy thông tin trên giao diện
     */
    private void getInfoInFrom() {
        try {
            changePasswordModel.PasswordOld = txtPasswordOld.getText().toString();
            changePasswordModel.PasswordNew = txtPasswordNew.getText().toString();
            changePasswordModel.ConfirmPasswordNew = txtConfirmPasswordNew.getText().toString();
        } catch (Exception ex) {
        }
    }

    /***
     * Check dữ liệu
     * @return
     */
    private boolean validateFrom() {
        if (changePasswordModel.PasswordOld.isEmpty()) {
            tilPasswordOld.setError("Không được để trống.");
            txtPasswordOld.requestFocus();
            return false;
        } else {
            tilPasswordOld.setErrorEnabled(false);
        }

        if (changePasswordModel.PasswordNew.isEmpty()) {
            tilPasswordNew.setError("Không được để trống.");
            txtPasswordNew.requestFocus();
            return false;
        } else {
            tilPasswordNew.setErrorEnabled(false);
        }

        if (changePasswordModel.ConfirmPasswordNew.isEmpty()) {
            tilConfirmPasswordNew.setError("Không được để trống.");
            txtConfirmPasswordNew.requestFocus();
            return false;
        } else {
            tilConfirmPasswordNew.setErrorEnabled(false);
        }

        if (!changePasswordModel.ConfirmPasswordNew.equals(changePasswordModel.ConfirmPasswordNew)) {
            tilConfirmPasswordNew.setError("Hai mật khẩu mới không khớp nhau.");
            txtConfirmPasswordNew.requestFocus();
            return false;
        } else {
            tilConfirmPasswordNew.setErrorEnabled(false);
        }
        return true;
    }
}
