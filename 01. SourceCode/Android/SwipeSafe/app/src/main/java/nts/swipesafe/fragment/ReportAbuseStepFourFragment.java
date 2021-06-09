package nts.swipesafe.fragment;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.provider.Settings;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RelativeLayout;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.MainActivity;
import nts.swipesafe.R;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.LocationGpsListener;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ComboboxResult;
import nts.swipesafe.model.LocationModel;
import nts.swipesafe.model.PrisonerModel;
import nts.swipesafe.model.ReportModel;
import nts.swipesafe.model.ReporterModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseStepFourFragment extends Fragment {
    private View view;
    private SharedPreferences sharedPreferencesDataFix;
    private LinearLayout lyNext, lyBack, lyForm;
    private List<ComboboxResult> listProvince, listDistrict, listWard, listRelationship;
    private CheckBox cbInvisible;
    private RadioGroup rgGender;
    private EditText txtName, txtPhone, txtEmail, txtAddress;
    private EditText spnRelationship, spnProvince, spnDistrict, spnWard;
    private GlobalVariable global;
    private ReporterModel reporterModel;
    private Bundle bundle = null;
    private TextView lblLocation;
    private LocationGpsListener locationGpsListener;
    private RelativeLayout progressDialog;


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_report_abuse_step_four, container, false);

        sharedPreferencesDataFix = getActivity().getSharedPreferences(Constants.Swipe_Safe_Data_Fix, Context.MODE_PRIVATE);
        global = (GlobalVariable) getActivity().getApplication();
        bundle = getArguments();

        locationGpsListener = new LocationGpsListener(getActivity().getApplicationContext());

        initComponent();

        getDataFix();

        viewData();

        if (bundle != null) {
            TextView btnText = ((TextView) lyNext.getChildAt(0));
            btnText.setText("Lưu");
            ImageView btnIcon = ((ImageView) lyNext.getChildAt(1));
            btnIcon.setImageResource( R.drawable.ic_save);
        }

        // Inflate the layout for this fragment
        return view;
    }

    public void  backFragment()
    {
        if (bundle == null) {
            saveInfo(false);
            Fragment fragment = new ReportAbuseStepThreeFragment();
            Utils.ChangeFragment(getActivity(), fragment, null);
        } else {
            Fragment fragment = new ReportAbuseDetailFragment();
            Utils.ChangeFragment(getActivity(), fragment, null);
        }
    }

    private void initComponent() {
        progressDialog = view.findViewById(R.id.progressDialog);

        lyBack = view.findViewById(R.id.lyBack);
        lyNext = view.findViewById(R.id.lyNext);
        lyForm = view.findViewById(R.id.lyForm);


        cbInvisible = view.findViewById(R.id.cbInvisible);
        txtName = view.findViewById(R.id.txtName);
        rgGender = view.findViewById(R.id.rgGenderReporter);
        spnRelationship = view.findViewById(R.id.spnRelationship);
        spnProvince = view.findViewById(R.id.spnProvince);
        spnDistrict = view.findViewById(R.id.spnDistrict);
        spnWard = view.findViewById(R.id.spnWard);
        txtPhone = view.findViewById(R.id.txtPhone);
        txtEmail = view.findViewById(R.id.txtEmail);
        txtAddress = view.findViewById(R.id.txtAddress);

        lblLocation = view.findViewById(R.id.lblLocation);

        lyNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (saveInfo(true)) {
                    Fragment fragment = new ReportAbuseDetailFragment();
                    Utils.ChangeFragment(getActivity(), fragment, null);
                }
            }
        });

        lyBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                backFragment();
            }
        });

        cbInvisible.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                if (b) {
                    lyForm.setVisibility(View.GONE);
                } else {
                    lyForm.setVisibility(View.VISIBLE);
                }
                reporterModel = new ReportModel();
                reporterModel.Type = b ? "1" : "0";
                fillInfoReporter(reporterModel, false);
            }
        });

        lblLocation.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                progressDialog.setVisibility(View.VISIBLE);
                try {
                    locationGpsListener = new LocationGpsListener(getActivity());
                    if (locationGpsListener.isPermissionGPS) {
                        //Kiểm tra xem thiết bị đã bật định vị chưa
                        if(!locationGpsListener.isGPSEnabled)
                        {
                            new AlertDialog.Builder(getActivity())
                                    .setTitle("Thiết bị chưa bật định vị")
                                    .setIcon(R.drawable.ic_warning)
                                    .setMessage("Vui lòng bật các dịch vụ định vị để sử dụng tính năng này.")
                                    .setCancelable(false)
                                    .setPositiveButton("Bật", new DialogInterface.OnClickListener() {
                                        public void onClick(DialogInterface dialog, int id) {
                                            startActivity(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS));
                                        }
                                    })
                                    .setNegativeButton("Không", null)
                                    .show();
                            progressDialog.setVisibility(View.GONE);
                            return;
                        }

                        LocationModel locationModel = locationGpsListener.getCurrentAddress();
                        if (locationModel != null) {
                            String provinceId = Utils.getIdByName(listProvince, locationModel.ProvinceName);
                            if (!Utils.isEmpty(provinceId)) {
                                spnProvince.setText(locationModel.ProvinceName);
                                spnProvince.setTag(provinceId);

                                setDataDistrict(provinceId);
                                String districtId = Utils.getIdByName(listDistrict, locationModel.DistrictName);
                                if (!Utils.isEmpty(districtId)) {
                                    spnDistrict.setText(locationModel.DistrictName);
                                    spnDistrict.setTag(districtId);

                                    setDataWard(districtId);
                                    String wardId = Utils.getIdByName(listWard, locationModel.WardName);
                                    if (!Utils.isEmpty(wardId)) {
                                        spnWard.setText(locationModel.WardName);
                                        spnWard.setTag(wardId);

                                        txtAddress.setText(locationModel.Address);
                                    }
                                }
                            }
                        }
                    } else {
                        //((MainActivity) getActivity()).checkLocationPermission();
                    }
                } catch (Exception ex) {

                }
                progressDialog.setVisibility(View.GONE);
            }
        });
    }

    /***
     * Hiển thị thông tin trẻ lê giao diện
     */
    private void viewData() {
        reporterModel = global.getReporter();
        fillInfoReporter(reporterModel, true);
    }

    /***
     * Fill dữ liệu lên view
     * @param model
     */
    private void fillInfoReporter(ReporterModel model, boolean isView) {
        txtName.setText(model.Name);
        View viewDefault = rgGender.getChildAt(0);
        RadioButton radioGenderDefault = ((RadioButton) viewDefault);
        radioGenderDefault.setChecked(true);
        int countGender = rgGender.getChildCount();
        for (int i = 0; i < countGender; i++) {
            View view = rgGender.getChildAt(i);
            if (view instanceof RadioButton && !Utils.isEmpty(model.Gender)) {
                RadioButton radioGender = ((RadioButton) view);
                if (model.Gender != null && radioGender.getTag().toString().equals(model.Gender)) {
                    radioGender.setChecked(true);
                    break;
                }
            }
        }

        if (isView) {
            if (!Utils.isEmpty(model.Type) && model.Type.equals("1")) {
                cbInvisible.setChecked(true);
            } else {
                cbInvisible.setChecked(false);
            }
        }

        txtAddress.setText(model.Address);
        txtEmail.setText(model.Email);
        txtPhone.setText(model.Phone);
        spnProvince.setText(model.ProvinceName);
        spnProvince.setTag(model.ProvinceId);
        setDataDistrict(model.ProvinceId);
        spnDistrict.setText(model.DistrictName);
        spnDistrict.setTag(model.DistrictId);
        setDataWard(model.DistrictId);
        spnWard.setTag(model.WardId);
        spnWard.setText(model.WardName);
        spnRelationship.setTag(model.Relationship);
        spnRelationship.setText(model.RelationshipName);
    }

    private void getDataFix() {
        String dataFixRelationship = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Relationship, null);
        if (!Utils.isEmpty(dataFixRelationship)) {
            listRelationship = new Gson().fromJson(dataFixRelationship, new TypeToken<List<ComboboxResult>>() {
            }.getType());
            final ArrayList<String> arrayRelationshipName = new ArrayList<>();
            for (ComboboxResult model : listRelationship) {
                arrayRelationshipName.add(model.text);
            }
            spnRelationship.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChooseDialog((EditText) v, arrayRelationshipName.toArray(new String[0]), listRelationship, "Chọn mối quan hệ với trẻ");
                }
            });
        }

        String dataFixProvince = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Province, null);
        if (!Utils.isEmpty(dataFixProvince)) {
            listProvince = new Gson().fromJson(dataFixProvince, new TypeToken<List<ComboboxResult>>() {
            }.getType());
            final ArrayList<String> arrayProvinceName = new ArrayList<>();
            for (ComboboxResult model : listProvince) {
                arrayProvinceName.add(model.text);
            }
            spnProvince.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChooseDialog((EditText) v, arrayProvinceName.toArray(new String[0]), listProvince, "Chọn Tỉnh/Thành");
                }
            });
        }
    }

    private void getReporter() {
        reporterModel = new ReporterModel();
        reporterModel.Name = txtName.getText().toString();
        reporterModel.Address = txtAddress.getText().toString();
        reporterModel.Phone = txtPhone.getText().toString();
        reporterModel.Email = txtEmail.getText().toString();
        reporterModel.Relationship = spnRelationship.getTag() != null ? spnRelationship.getTag().toString() : "";
        reporterModel.RelationshipName = spnRelationship.getText().toString();
        reporterModel.ProvinceId = spnProvince.getTag() != null ? spnProvince.getTag().toString() : "";
        reporterModel.ProvinceName = spnProvince.getText().toString();
        reporterModel.DistrictId = spnDistrict.getTag() != null ? spnDistrict.getTag().toString() : "";
        reporterModel.DistrictName = spnDistrict.getText().toString();
        reporterModel.WardId = spnWard.getTag() != null ? spnWard.getTag().toString() : "";
        reporterModel.WardName = spnWard.getText().toString();
        reporterModel.Type = cbInvisible.isChecked() ? "1" : "0";
        reporterModel.FullAddress = (!Utils.isEmpty(reporterModel.Address) ? (txtAddress.getText().toString() + " - ") : "")
                + (!Utils.isEmpty(reporterModel.WardName) ? (reporterModel.WardName + " - ") : "")
                + (!Utils.isEmpty(reporterModel.DistrictName) ? (reporterModel.DistrictName + " - ") : "")
                + reporterModel.ProvinceName;

        int rgGenderID = rgGender.getCheckedRadioButtonId();
        RadioButton radioButton = rgGender.findViewById(rgGenderID);
        if (radioButton != null) {
            reporterModel.Gender = radioButton.getTag().toString();
            reporterModel.GenderName = radioButton.getText().toString();
        }
    }

    /***
     * Show Dialog chọn
     * @param editText
     * @param arrayName
     */
    private void showChooseDialog(final EditText editText, final String[] arrayName, final List<ComboboxResult> listSource, final String title) {
        final AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, Utils.getIndexByName(arrayName, Utils.getText(editText)), new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                editText.setText(arrayName[which]);

                String id = listSource != null ? listSource.get(which).id : "";
                editText.setTag(id);

                if (title.equals("Chọn Tỉnh/Thành")) {
                    setDataDistrict(id);
                    spnDistrict.setText("");
                    spnDistrict.setTag("");
                    spnWard.setText("");
                    spnWard.setTag("");
                    txtAddress.setText("");
                } else if (title.equals("Chọn Quận/Huyện")) {
                    setDataWard(id);
                    spnWard.setText("");
                    spnWard.setTag("");
                    txtAddress.setText("");
                }else if (title.equals("Chọn Xã/Phường")) {
                    txtAddress.setText("");
                }

            }
        });
        builder.show();
    }

    private boolean saveInfo(boolean isCheck) {
        try {
            //Get thông tin
            getReporter();

            //Check validate
            if (isCheck && !validateFrom()) {
                return false;
            }

            global.setReporter(reporterModel);
            return true;
        } catch (Exception ex) {
            Toast.makeText(getActivity(), "Lỗi phát sinh trong quá trình xử lý.", Toast.LENGTH_SHORT).show();
        }
        return false;
    }

    private boolean validateFrom() {
        boolean isValidate = true;

//        if (!cbInvisible.isChecked() && Utils.isEmpty(reporterModel.Name)) {
//            Toast.makeText(getActivity(), "Họ và tên không được để trống.", Toast.LENGTH_SHORT).show();
//            txtName.requestFocus();
//            return false;
//        }
//
//        if (!cbInvisible.isChecked() && !Utils.isEmpty(reporterModel.Email) && !Utils.emailValidator(reporterModel.Email)) {
//            Toast.makeText(getActivity(), "Địa chỉ emial không hợp lệ.", Toast.LENGTH_SHORT).show();
//            txtEmail.requestFocus();
//            return false;
//        }
//
//        if (!cbInvisible.isChecked() && Utils.isEmpty(reporterModel.ProvinceId)) {
//            Toast.makeText(getActivity(), "Chưa chọn Tỉnh/Thành nơi xảy ra.", Toast.LENGTH_SHORT).show();
//            spnProvince.requestFocus();
//            return false;
//        }
//
//        if (!cbInvisible.isChecked() && Utils.isEmpty(reporterModel.DistrictId))
//
//        {
//            Toast.makeText(getActivity(), "Chưa chọn Quận/Huyện nơi xảy ra.", Toast.LENGTH_SHORT).show();
//            spnDistrict.requestFocus();
//            return false;
//        }
//
//        if (!cbInvisible.isChecked() && Utils.isEmpty(reporterModel.WardId))
//
//        {
//            Toast.makeText(getActivity(), "Chưa chọn Xã/Phường nơi xảy ra.", Toast.LENGTH_SHORT).show();
//            spnWard.requestFocus();
//            return false;
//        }

        return isValidate;
    }

    /***
     * Set data Quận/Huyện
     * @param provinceId
     */
    private void setDataDistrict(String provinceId) {
        String dataFixDistrict = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_District, null);
        if (!Utils.isEmpty(dataFixDistrict)) {
            List<ComboboxResult> listDistrictTemp = new Gson().fromJson(dataFixDistrict, new TypeToken<List<ComboboxResult>>() {
            }.getType());
            listDistrict = new ArrayList<>();
            final ArrayList<String> arrayDistrictName = new ArrayList<>();
            for (ComboboxResult item : listDistrictTemp) {
                if (item.ParentId.equals(provinceId)) {
                    listDistrict.add(item);
                    arrayDistrictName.add(item.text);
                }
            }
            spnDistrict.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChooseDialog((EditText) v, arrayDistrictName.toArray(new String[0]), listDistrict, "Chọn Quận/Huyện");
                }
            });
        }
    }

    /***
     * Set data Quận/Huyện
     * @param districtId
     */
    private void setDataWard(String districtId) {
        String dataFixWard = sharedPreferencesDataFix.getString(districtId, null);
        if (!Utils.isEmpty(dataFixWard)) {
            listWard = new Gson().fromJson(dataFixWard, new TypeToken<List<ComboboxResult>>() {
            }.getType());
            final ArrayList<String> arrayWardName = new ArrayList<>();
            for (ComboboxResult item : listWard) {
                arrayWardName.add(item.text);
            }
            spnWard.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChooseDialog((EditText) v, arrayWardName.toArray(new String[0]), listWard, "Chọn Xã/Phường");
                }
            });
        }
    }
}
