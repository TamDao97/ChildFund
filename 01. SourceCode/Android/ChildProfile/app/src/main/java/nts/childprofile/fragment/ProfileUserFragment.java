package nts.childprofile.fragment;

import android.Manifest;
import android.app.Activity;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.media.Image;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.FileProvider;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.Toolbar;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.RadioButton;
import android.widget.RadioGroup;
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
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

import nts.childprofile.MainActivity;
import nts.childprofile.R;
import nts.childprofile.adapter.ProfileListAdapter;
import nts.childprofile.common.Constants;
import nts.childprofile.common.DateUtils;
import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.model.ChildProfileSearchResult;
import nts.childprofile.model.LoginProfileModel;
import nts.childprofile.model.ProfileUserModel;
import nts.childprofile.model.SearchResultObject;

import android.support.v4.app.Fragment;

import static android.Manifest.permission.READ_EXTERNAL_STORAGE;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * create an instance of this fragment.
 */
public class ProfileUserFragment extends Fragment {
    private View view;
    private ProfileUserModel profileUserModel;
    private ImageView imageCaptureEngine, imageAvata, imageChoosePhoto;
    private GlobalVariable global;
    private LoginProfileModel loginProfileModel;
    private EditText txtName, txtDateOfBirth, txtPhoneNumber, txtEmail, txtAddress;
    private RadioButton radioMale, radioFemale;
    private String imageLink = "";
    private Button btnSave;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_profile_user, container, false);

        global = (GlobalVariable) getActivity().getApplication();
        loginProfileModel = global.getLoginProfile();

        Toolbar mToolbar = (Toolbar) getActivity().findViewById(R.id.toolbar);
        TextView toolbarTitle = mToolbar.findViewById(R.id.toolbar_title);
        toolbarTitle.setText("Thông tin cá nhân/Personal");
        Toolbar.LayoutParams layoutParams = (Toolbar.LayoutParams) toolbarTitle.getLayoutParams();
        layoutParams.gravity = Gravity.LEFT;
        toolbarTitle.setLayoutParams(layoutParams);

        ImageView imgLogo = mToolbar.findViewById(R.id.imgLogo);
        imgLogo.setVisibility(View.VISIBLE);

        //Khởi tạo thuộc tính trên giao diện
        initLayout();

        //Hiển thị thông tin lên giao diện
        viewProfileUser();

        // Inflate the layout for this fragment
        return view;
    }

    ///Khởi tạo thuộc tính trên giao diện
    private void initLayout() {

        txtName = (EditText) view.findViewById(R.id.txtName);
        txtDateOfBirth = (EditText) view.findViewById(R.id.txtDateOfBirth);
        txtPhoneNumber = (EditText) view.findViewById(R.id.txtPhoneNumber);
        txtEmail = (EditText) view.findViewById(R.id.txtEmail);
        txtAddress = (EditText) view.findViewById(R.id.txtAddress);
        radioMale = (RadioButton) view.findViewById(R.id.radioMale);
        radioFemale = (RadioButton) view.findViewById(R.id.radioFemale);

        imageChoosePhoto = (ImageView) view.findViewById(R.id.imageChoosePhoto);
        imageAvata = (ImageView) view.findViewById(R.id.imageAvata);
        imageCaptureEngine = (ImageView) view.findViewById(R.id.imageCaptureEngine);
        RadioGroup rgGender = (RadioGroup) view.findViewById(R.id.rgGender);
        btnSave = (Button) view.findViewById(R.id.btnSave);

        rgGender.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                RadioButton checkedRadioButton = (RadioButton) group.findViewById(checkedId);
                profileUserModel.Gender = Integer.parseInt(checkedRadioButton.getTag().toString());
            }
        });

        viewProfileUser();

        /***
         * Chụp ảnh đại diện
         */
        imageCaptureEngine.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
                    if (ActivityCompat.checkSelfPermission(getContext(),
                            Manifest.permission.CAMERA) != PackageManager.PERMISSION_GRANTED) {
                        // Get the package name
                        final String packageName = getActivity().getApplicationContext().getPackageName();
                        new AlertDialog.Builder(getActivity())
                                .setTitle("Cấp quyền sử dụng máy ảnh")
                                .setIcon(R.drawable.ic_warning)
                                .setMessage("Vui lòng cấp quyền sử máy ảnh để chụp hình từ ứng dụng.")
                                .setCancelable(false)
                                .setPositiveButton("Bật", new DialogInterface.OnClickListener() {
                                    public void onClick(DialogInterface dialog, int id) {
                                        startActivity(new Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS, Uri.parse("package:" + packageName)));
                                    }
                                })
                                .setNegativeButton("Không", null)
                                .show();
                        return;
                    }

                    if (!isPermission()) {
                        return;
                    }

                    Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
                    // Create the File where the photo should go
                    File photoFile = null;
                    try {
                        photoFile = Utils.CreateImageFile(getActivity());
                    } catch (IOException ex) {
                    }
                    // Continue only if the File was successfully created
                    if (photoFile != null) {
                        final int position = v.getId();
                        imageLink = photoFile.getAbsolutePath();
                        Uri photoUri = FileProvider.getUriForFile(getActivity(), getActivity().getPackageName() + ".nts.childprofile.provider", photoFile);
                        intent.putExtra(MediaStore.EXTRA_OUTPUT, photoUri);
                        startActivityForResult(intent, Constants.REQUEST_IMAGE_CAPTURE);
                    }
                } catch (Exception ex) {
                }
            }
        });

        /***
         * Chọn ảnh đại diện
         */
        imageChoosePhoto.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (!isPermission()) {
                    return;
                }

                Intent intent = new Intent(Intent.ACTION_PICK);
                intent.setType("image/*");
                startActivityForResult(intent, Constants.REQUEST_CHOOSE_IMAGE);
            }
        });

        txtDateOfBirth.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DateUtils.dialogDatePicker(txtDateOfBirth, getContext());
            }
        });

        /***
         * Lưu thông tin
         */
        btnSave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                saveProfileUser();
            }
        });
    }

    private static final int REQUEST_FOR_STORAGE_PERMISSION = 123;

    public boolean isPermission() {
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.M) {
            return true;
        }

        if (getActivity().checkSelfPermission(READ_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED) {
            return true;
        }

        if (shouldShowRequestPermissionRationale(READ_EXTERNAL_STORAGE)) {
            // Get the package name
            final String packageName = getActivity().getApplicationContext().getPackageName();
            new AlertDialog.Builder(getActivity())
                    .setTitle("Cấp quyền sử dụng bộ nhớ")
                    .setIcon(R.drawable.ic_warning)
                    .setMessage("Vui lòng cấp quyền sử dụng bộ nhớ để tìm nạp hình ảnh từ thư viện.")
                    .setCancelable(false)
                    .setPositiveButton("Bật", new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int id) {
                            startActivity(new Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS, Uri.parse("package:" + packageName)));
                        }
                    })
                    .setNegativeButton("Không", null)
                    .show();
        } else {
            requestPermissions(new String[]{READ_EXTERNAL_STORAGE}, REQUEST_FOR_STORAGE_PERMISSION);
        }

        return false;
    }

    /***
     *  Hiển thị thông tin người dùng
     */
    private void viewProfileUser() {
        AndroidNetworking.get(Utils.GetUrlApi("api/Authorize/GetProfileUser?id=" + loginProfileModel.Id))
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        profileUserModel = new Gson().fromJson(response.toString(), new TypeToken<ProfileUserModel>() {
                        }.getType());
                        setInfoProfileUser(profileUserModel);
                    }

                    @Override
                    public void onError(ANError anError) {
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    /***
     * Lưu thông tin hồ sơ
     */
    private void saveProfileUser() {
        getInfoProfileUser();

        if (!validateFrom()) {
            return;
        }

        String jsonModel = new Gson().toJson(profileUserModel);
        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/Authorize/UpdateProfileUser"));

        if (!imageLink.isEmpty()) {
            File file = new File(imageLink);
            anRequest.addMultipartFile("file", file);
        }

        anRequest.addMultipartParameter("Model", jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        profileUserModel = new Gson().fromJson(response.toString(), new TypeToken<ProfileUserModel>() {
                        }.getType());
                        try {
                            Picasso.with(getActivity()).load(loginProfileModel.ImagePath).into(imageAvata);
                        } catch (Exception ex) {
                        }
                        Toast.makeText(getActivity(), "Cập nhật thông tin cá nhân thành công!", Toast.LENGTH_SHORT).show();
                    }

                    @Override
                    public void onError(ANError anError) {
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    /***
     * Set thông tin lên giao diện
     */
    private void setInfoProfileUser(ProfileUserModel profileUserModel) {
        try {
            txtName.setText(profileUserModel.Name != null ? profileUserModel.Name : "");

            if (profileUserModel.Gender == Constants.FeMale) {
                radioFemale.setChecked(true);
            } else if (profileUserModel.Gender == Constants.Male) {
                radioMale.setChecked(true);
            }

            txtDateOfBirth.setText(profileUserModel.DateOfBirth != null ? DateUtils.ConvertYMDServerToDMY(profileUserModel.DateOfBirth) : "");
            txtPhoneNumber.setText(profileUserModel.PhoneNumber != null ? profileUserModel.PhoneNumber : "");
            txtEmail.setText(profileUserModel.Email != null ? profileUserModel.Email : "");
            txtAddress.setText(profileUserModel.Address != null ? profileUserModel.Address : "");
            try {
                if (loginProfileModel.ImagePath != null && !loginProfileModel.ImagePath.isEmpty()) {
                    Picasso.with(getActivity()).load(loginProfileModel.ImagePath).into(imageAvata);
                } else {
                    Picasso.with(this.getContext()).load(R.drawable.ic_people).into(imageAvata);
                }
            } catch (Exception ex) {
            }
        } catch (Exception ex) {
        }
    }

    /***
     * Lấy thông tin trên giao diện
     */
    private void getInfoProfileUser() {
        try {
            profileUserModel.Name = txtName.getText().toString();
            profileUserModel.DateOfBirth = txtDateOfBirth.getText().toString();
            profileUserModel.PhoneNumber = txtPhoneNumber.getText().toString();
            profileUserModel.Email = txtEmail.getText().toString();
            profileUserModel.Address = txtAddress.getText().toString();
        } catch (Exception ex) {
        }
    }

    /***
     * Check dữ liệu
     * @return
     */
    private boolean validateFrom() {
        if (profileUserModel.Name.isEmpty()) {
            Toast.makeText(getActivity(), "Họ và tên không được để trống.", Toast.LENGTH_SHORT).show();
            txtName.requestFocus();
            return false;
        }

        if (profileUserModel.Email.isEmpty()) {
            Toast.makeText(getActivity(), "Email không được để trống.", Toast.LENGTH_SHORT).show();
            txtEmail.requestFocus();
            return false;
        }

        if (!Utils.emailValidator(profileUserModel.Email)) {
            Toast.makeText(getActivity(), "Email không đúng định dạng.", Toast.LENGTH_SHORT).show();
            txtEmail.requestFocus();
            return false;
        }

        if (!profileUserModel.DateOfBirth.isEmpty()) {
            SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd");
            try {
                profileUserModel.DateOfBirth = DateUtils.ConvertDMYToYMD(profileUserModel.DateOfBirth);
                Date date = format.parse(profileUserModel.DateOfBirth);
            } catch (Exception ex) {
                Toast.makeText(getActivity(), "Ngày sinh lỗi định dạng. Định dạng đúng 'dd/MM/yyyy'", Toast.LENGTH_SHORT).show();
                txtDateOfBirth.requestFocus();
                return false;
            }
        }
        return true;
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode != Activity.RESULT_OK && data == null) {
            return;
        }
        if (requestCode == Constants.REQUEST_IMAGE_CAPTURE) {
            Utils.SetPicture(imageAvata, imageLink);
        } else if (requestCode == Constants.REQUEST_CHOOSE_IMAGE) {
            Uri uri = data.getData();
            Cursor cursor = getActivity().getContentResolver().query(uri, null, null, null, null);
            try {
                if (cursor != null && cursor.moveToFirst()) {
                    String[] filePathColumn = {MediaStore.Images.Media.DATA};
                    int columnIndex = cursor.getColumnIndex(filePathColumn[0]);
                    imageLink = cursor.getString(columnIndex);
                }
            } finally {
                cursor.close();
            }
            Utils.SetPicture(imageAvata, imageLink);
        }
        super.onActivityResult(requestCode, resultCode, data);
    }
}
