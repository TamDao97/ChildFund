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
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.text.Editable;
import android.text.TextWatcher;
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
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import nts.swipesafe.MainActivity;
import nts.swipesafe.R;
import nts.swipesafe.adapter.PrisonerListAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.LocationGpsListener;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ChildModel;
import nts.swipesafe.model.ComboboxResult;
import nts.swipesafe.model.LocationModel;
import nts.swipesafe.model.PrisonerModel;


/*
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseStepTwoFragment extends Fragment {
    private View view;
    private SharedPreferences sharedPreferencesDataFix;
    private GlobalVariable global;
    private LinearLayout lyNext, lyBack, lyAddPrisoner, lyPrisoner;
    private EditText txtName, txtAge, txtAddress;
    private EditText spnBirthday, spnProvince, spnDistrict, spnWard, spnRelationship;
    private List<ComboboxResult> listProvince, listDistrict, listWard, listRelationship;
    private RadioGroup rgGender;
    private RecyclerView rvPrisoner;
    private PrisonerListAdapter prisonerListAdapter;
    private PrisonerModel prisonerModel;
    private int indexPrisoner = -1;
    private Bundle bundle = null;
    private TextView lblLocation, lblJointly;
    private LocationGpsListener locationGpsListener;
    private RelativeLayout progressDialog;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_report_abuse_step_two, container, false);

        sharedPreferencesDataFix = getActivity().getSharedPreferences(Constants.Swipe_Safe_Data_Fix, Context.MODE_PRIVATE);
        global = (GlobalVariable) getActivity().getApplication();
        bundle = getArguments();

        locationGpsListener = new LocationGpsListener(getActivity().getApplicationContext());

        initComponent();

        getDataFix();

        if (bundle == null) {
            viewData();
        } else {
            indexPrisoner = bundle.getInt("indexEdit");
            prisonerModel = global.reportModel.ListPrisoner.get(indexPrisoner);
            fillInfoPrisoner(prisonerModel);

            rvPrisoner.setVisibility(View.GONE);
            lyAddPrisoner.setVisibility(View.GONE);
            TextView btnText = ((TextView) lyNext.getChildAt(0));
            btnText.setText("Lưu");
            ImageView btnIcon = ((ImageView) lyNext.getChildAt(1));
            btnIcon.setImageResource( R.drawable.ic_save);
        }

        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        if (bundle == null) {
            saveInfo(false);
            Fragment fragment = new ReportAbuseStepOneFragment();
            Utils.ChangeFragment(getActivity(), fragment, null);
        } else {
            Fragment fragment = new ReportAbuseDetailFragment();
            Utils.ChangeFragment(getActivity(), fragment, null);
        }
    }

    /***
     * Khỏi tạo thành phần trên giao diện
     */
    private void initComponent() {
        progressDialog = view.findViewById(R.id.progressDialog);

        lyBack = view.findViewById(R.id.lyBack);
        lyNext = view.findViewById(R.id.lyNext);
        lyAddPrisoner = view.findViewById(R.id.lyAddPrisoner);
        lyPrisoner = view.findViewById(R.id.lyPrisoner);

        txtName = view.findViewById(R.id.txtName);
        txtAge = view.findViewById(R.id.txtAge);
        txtAddress = view.findViewById(R.id.txtAddress);

        lblLocation = view.findViewById(R.id.lblLocation);

        lblJointly = view.findViewById(R.id.lblJointly);

        rgGender = view.findViewById(R.id.rgGender);

        spnProvince = view.findViewById(R.id.spnProvince);
        spnDistrict = view.findViewById(R.id.spnDistrict);
        spnWard = view.findViewById(R.id.spnWard);
        spnBirthday = view.findViewById(R.id.spnBirthday);
        spnRelationship = view.findViewById(R.id.spnRelationship);

        rvPrisoner = view.findViewById(R.id.rvPrisoner);
        rvPrisoner.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        rvPrisoner.setHasFixedSize(true);

        spnBirthday.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        spnBirthday.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String birthday = spnBirthday.getText().toString();
                SimpleDateFormat dateFormat = new SimpleDateFormat(DateUtils.DATE_FORMAT_DD_MM_YYYY);
                try {
                    if (!birthday.isEmpty() && dateFormat.parse(birthday).compareTo(dateFormat.parse(DateUtils.CurrentDate(DateUtils.DATE_FORMAT_DD_MM_YYYY))) >= 0) {
                        spnBirthday.setText("");
                        Toast.makeText(getActivity(), "Ngày sinh không đúng.", Toast.LENGTH_SHORT).show();
                    }

                    birthday = spnBirthday.getText().toString();
                    if (!birthday.isEmpty()) {
                        Date dateBirthday = dateFormat.parse(birthday);
                        Calendar calendar = Calendar.getInstance();
                        calendar.setTime(dateBirthday);
                        int age = DateUtils.CurrentYear() - calendar.get(Calendar.YEAR);
                        txtAge.setText(String.valueOf(age));
                        txtAge.setEnabled(false);
                    }

                } catch (Exception ex) {
                    spnBirthday.setText("");
                    Toast.makeText(getActivity(), "Ngày sinh không đúng.", Toast.LENGTH_SHORT).show();
                }

                birthday = spnBirthday.getText().toString();
                if (birthday.isEmpty()) {
                    txtAge.setText("");
                    txtAge.setEnabled(true);
                }
            }
        });

        txtAge.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String age = txtAge.getText().toString();

                if ((!age.isEmpty() && Integer.parseInt(age) >= 150) || (!age.isEmpty() && Integer.parseInt(age) < 0)) {
                    Toast.makeText(getActivity(), "Tuổi ước lượng phải lớn 0 và nhỏ hơn 150 tuổi.", Toast.LENGTH_SHORT).show();
                    txtAge.setEnabled(true);
                    txtAge.setText("");
                    spnBirthday.setText("");
                }
            }
        });

        lyAddPrisoner.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                addPrisoner();
            }
        });

        lyNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (saveInfo(false)) {
                    if (bundle == null) {
                        Fragment fragment = new ReportAbuseStepThreeFragment();
                        Utils.ChangeFragment(getActivity(), fragment, null);
                    } else {
                        Fragment fragment = new ReportAbuseDetailFragment();
                        Utils.ChangeFragment(getActivity(), fragment, null);
                    }
                }
            }
        });

        lyBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                backFragment();
            }
        });

        lblJointly.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (global.getChildSize() <= 1) {
                    fillAddressByChild(0);
                } else {
                    final ArrayList<String> arrayChildName = new ArrayList<>();
                    for (ChildModel model : global.getListChild()) {
                        arrayChildName.add(model.Name);
                    }
                    showChooseAddressDialog(arrayChildName.toArray(new String[0]));
                }
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

    /***
     * Hiển thị thông tin trẻ lê giao diện
     */
    private void viewData() {
        lyPrisoner.setVisibility(View.GONE);
        if (global.reportModel != null && global.reportModel.ListPrisoner != null && global.reportModel.ListPrisoner.size() > 0) {
            prisonerModel = global.reportModel.ListPrisoner.get(0);
            indexPrisoner = 0;

            if (global.reportModel.ListPrisoner.size() > 1) {
                lyPrisoner.setVisibility(View.VISIBLE);
            }
        } else {
            prisonerModel = new PrisonerModel();
        }

        fillInfoPrisoner(prisonerModel);

        setDataPrisonerList();
    }

    /***
     * Fill dữ liệu lên view
     * @param model
     */
    private void fillInfoPrisoner(PrisonerModel model) {
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
        spnBirthday.setText(DateUtils.ConvertYMDServerToDMY(model.Birthday));
        if (model.Age != null) {
            txtAge.setText(String.valueOf(model.Age));
        } else {
            txtAge.setText("");
        }

        txtAddress.setText(model.Address);
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

    /***
     * Lưu thông tin
     */
    private boolean saveInfo(boolean isCheck) {
        try {
            //Get thông tin
            getInfoModel();

            if (isCheck) {
                //Check validate
                if (!validateFrom()) {
                    return false;
                }
            }

            global.addPrisoner(prisonerModel, indexPrisoner);
            return true;
        } catch (Exception ex) {
            Toast.makeText(getActivity(), "Lỗi phát sinh trong quá trình xử lý.", Toast.LENGTH_SHORT).show();
        }
        return false;
    }

    /***
     * Check validate from
     * @return
     */
    private boolean validateFrom() {
        boolean isValidate = true;

        if (Utils.isEmpty(prisonerModel.Name) && Utils.isEmpty(prisonerModel.Gender) && Utils.isEmpty(prisonerModel.Birthday)
                && Utils.isEmpty(prisonerModel.Phone) && Utils.isEmpty(prisonerModel.RelationshipName) && Utils.isEmpty(prisonerModel.ProvinceId)
                && Utils.isEmpty(prisonerModel.DistrictId) && Utils.isEmpty(prisonerModel.WardId) && Utils.isEmpty(prisonerModel.Address)) {
            Toast.makeText(getActivity(), "Hãy nhập thông tin để tiếp tục.", Toast.LENGTH_SHORT).show();
            txtName.requestFocus();
            return false;
        }
        return isValidate;
    }

    /***
     * Get thông tin model
     */
    private void getInfoModel() {
        prisonerModel = new PrisonerModel();
        prisonerModel.Name = txtName.getText().toString();
        int rgGenderID = rgGender.getCheckedRadioButtonId();
        RadioButton radioButton = rgGender.findViewById(rgGenderID);
        if (radioButton != null) {
            prisonerModel.Gender = radioButton.getTag().toString();
            prisonerModel.GenderName = radioButton.getText().toString();
        }
        prisonerModel.Birthday = DateUtils.ConvertDMYToYMD(spnBirthday.getText().toString());
        try {
            prisonerModel.Age = Integer.parseInt(txtAge.getText().toString());
        } catch (Exception ex) {
            prisonerModel.Age = null;
        }
        prisonerModel.Address = txtAddress.getText().toString();
        prisonerModel.ProvinceId = spnProvince.getTag() != null ? spnProvince.getTag().toString() : "";
        prisonerModel.ProvinceName = spnProvince.getText().toString();
        prisonerModel.DistrictId = spnDistrict.getTag() != null ? spnDistrict.getTag().toString() : "";
        prisonerModel.DistrictName = spnDistrict.getText().toString();
        prisonerModel.WardId = spnWard.getTag() != null ? spnWard.getTag().toString() : "";
        prisonerModel.WardName = spnWard.getText().toString();
        prisonerModel.Relationship = spnRelationship.getTag() != null ? spnRelationship.getTag().toString() : "";
        prisonerModel.RelationshipName = spnRelationship.getText().toString();

        prisonerModel.FullAddress = (!Utils.isEmpty(prisonerModel.Address) ? (prisonerModel.Address + " - ") : "")
                + (!Utils.isEmpty(prisonerModel.WardName) ? (prisonerModel.WardName + " - ") : "")
                + (!Utils.isEmpty(prisonerModel.DistrictName) ? (prisonerModel.DistrictName + " - ") : "")
                + prisonerModel.ProvinceName;
    }

    /***
     * Thêm mới trẻ bị hại
     */
    private void addPrisoner() {
        if (saveInfo(true)) {
            clearInfoPrisoner();
            setDataPrisonerList();
            lyPrisoner.setVisibility(View.VISIBLE);
        }
    }

    private void clearInfoPrisoner() {
        indexPrisoner = -1;
        prisonerModel = new PrisonerModel();
        fillInfoPrisoner(prisonerModel);
        txtName.requestFocus();
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
                } else if (title.equals("Chọn Xã/Phường")) {
                    txtAddress.setText("");
                }

            }
        });
        builder.show();
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

    /***
     * Set dữ liệu list Prisoner
     */
    private void setDataPrisonerList() {
        if (global.reportModel.ListPrisoner != null && global.reportModel.ListPrisoner.size() > 0) {
            prisonerListAdapter = new PrisonerListAdapter(getActivity(), global.reportModel.ListPrisoner);
            rvPrisoner.setAdapter(prisonerListAdapter);
            prisonerListAdapter.SetOnItemClickListener(new PrisonerListAdapter.OnItemClickListener() {
                @Override
                public void onEditClick(View view, int position, PrisonerModel obj) {
                    if (indexPrisoner != -1) {
                        addPrisoner();
                    }
                    prisonerModel = obj;
                    indexPrisoner = position;
                    fillInfoPrisoner(prisonerModel);
                    prisonerListAdapter.notifyDataSetChanged();
                    if (global.reportModel.ListPrisoner.size() <= 1) {
                        lyPrisoner.setVisibility(View.GONE);
                    }
                }

                @Override
                public void onRemoveClick(View view, int position, PrisonerModel obj) {
                    global.removePrisoner(position);
                    prisonerListAdapter.notifyDataSetChanged();
                    clearInfoPrisoner();
                    if (global.reportModel.ListPrisoner.size() <= 1) {
                        lyPrisoner.setVisibility(View.GONE);
                    }
                }
            });
            rvPrisoner.requestFocus();
        }
    }

    /***
     * Hiển thị thông tin địa chỉ theo trẻ
     * @param index
     */
    private void fillAddressByChild(int index) {
        ChildModel childModel = global.getChild(index);
        prisonerModel.ProvinceName = childModel.ProvinceName;
        prisonerModel.ProvinceId = childModel.ProvinceId;
        prisonerModel.DistrictName = childModel.DistrictName;
        prisonerModel.DistrictId = childModel.DistrictId;
        prisonerModel.WardName = childModel.WardName;
        prisonerModel.WardId = childModel.WardId;
        prisonerModel.Address = childModel.Address;

        spnProvince.setText(prisonerModel.ProvinceName);
        spnProvince.setTag(prisonerModel.ProvinceId);
        setDataDistrict(prisonerModel.ProvinceId);
        spnDistrict.setText(prisonerModel.DistrictName);
        spnDistrict.setTag(prisonerModel.DistrictId);
        setDataWard(prisonerModel.DistrictId);
        spnWard.setTag(prisonerModel.WardId);
        spnWard.setText(prisonerModel.WardName);
        txtAddress.setText(prisonerModel.Address);
    }

    /***
     * Show Dialog chọn
     * @param arrayName
     */
    private void showChooseAddressDialog(final String[] arrayName) {
        final AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle("Cùng địa chỉ với trẻ bị xâm hại");
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, -1, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                fillAddressByChild(which);
            }
        });
        builder.show();
    }
}
