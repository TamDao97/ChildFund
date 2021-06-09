package nts.swipesafe.fragment;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Resources;
import android.net.Uri;
import android.os.Bundle;
import android.provider.Settings;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
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
import nts.swipesafe.adapter.AbuseListAdapter;
import nts.swipesafe.adapter.ChildListAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.LocationGpsListener;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ChildAbuseModel;
import nts.swipesafe.model.ChildModel;
import nts.swipesafe.model.ComboboxResult;
import nts.swipesafe.model.LocationModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseStepOneFragment extends Fragment {
    private View view;
    private SharedPreferences sharedPreferencesDataFix;
    private GlobalVariable global;
    private LinearLayout lyNext, lyBack, lyAddChild, lyChild;
    private EditText txtName, txtAge, txtAddress;
    private EditText spnBirthday, spnProvince, spnDistrict, spnWard, spnTimeAction, spnDayAction;
    private List<ComboboxResult> listProvince, listDistrict, listWard;
    private RadioGroup rgGender, rgLevel;
    private RecyclerView rvChild, rvAbuse;
    private ChildListAdapter childListAdapter;
    private ChildModel childModel;
    private List<ChildAbuseModel> listAbuse;
    private AbuseListAdapter abuseListAdapter;
    private int indexChild = -1;
    private Bundle bundle = null;
    private TextView lblLocation;
    private LocationGpsListener locationGpsListener;
    private RelativeLayout progressDialog;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_report_abuse_step_one, container, false);
        getArguments();
        sharedPreferencesDataFix = getActivity().getSharedPreferences(Constants.Swipe_Safe_Data_Fix, Context.MODE_PRIVATE);
        global = (GlobalVariable) getActivity().getApplication();
        bundle = getArguments();

        locationGpsListener = new LocationGpsListener(getActivity().getApplicationContext());

        initComponent();

        getDataFix();

        if (bundle == null) {
            viewData();
        } else {
            indexChild = bundle.getInt("indexEdit");
            childModel = global.reportModel.ListChild.get(indexChild);
            fillInfoChild(childModel);

            rvChild.setVisibility(View.GONE);
            lyAddChild.setVisibility(View.GONE);
            TextView btnText = ((TextView) lyNext.getChildAt(0));
            btnText.setText("Lưu");
            ImageView btnIcon = ((ImageView) lyNext.getChildAt(1));
            btnIcon.setImageResource(R.drawable.ic_save);
        }
        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        if (bundle == null) {
            new AlertDialog.Builder(getActivity())
                    .setTitle("Cảnh báo")
                    .setIcon(R.drawable.ic_warning)
                    .setMessage("Báo cáo chưa được gửi đi. Bạn có muốn thoát khỏi chương trình không?")
                    .setCancelable(false)
                    .setPositiveButton("Đồng ý", new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int id) {
                            global.clearReport();
                            Fragment fragment = new ReportMainFragment();
                            Utils.ChangeFragment(getActivity(), fragment, null);
                        }
                    })
                    .setNegativeButton("Hủy", null)
                    .show();
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
        lyAddChild = view.findViewById(R.id.lyAddChild);
        lyChild = view.findViewById(R.id.lyChild);

        txtName = view.findViewById(R.id.txtName);
        txtAge = view.findViewById(R.id.txtAge);
        txtAddress = view.findViewById(R.id.txtAddress);

        lblLocation = view.findViewById(R.id.lblLocation);

        rgGender = view.findViewById(R.id.rgGender);
        rgLevel = view.findViewById(R.id.rgLevel);

        spnProvince = view.findViewById(R.id.spnProvince);
        spnDistrict = view.findViewById(R.id.spnDistrict);
        spnWard = view.findViewById(R.id.spnWard);
        spnBirthday = view.findViewById(R.id.spnBirthday);
        spnTimeAction = view.findViewById(R.id.spnTimeAction);
        spnDayAction = view.findViewById(R.id.spnDayAction);

        rvChild = view.findViewById(R.id.rvChild);
        rvChild.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        rvChild.setHasFixedSize(true);

        rvAbuse = view.findViewById(R.id.rvAbuse);
        rvAbuse.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        rvAbuse.setHasFixedSize(true);

        spnBirthday.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        spnTimeAction.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogTimePicker(getActivity(), (EditText) v);
            }
        });

        spnDayAction.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        lyAddChild.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                addChild();
            }
        });

        lyNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (saveInfo()) {
                    if (bundle == null) {
                        Fragment fragment = new ReportAbuseStepTwoFragment();
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
                    String dayAction = spnDayAction.getText().toString();

                    if (!birthday.isEmpty() && !dayAction.isEmpty() && dateFormat.parse(birthday).compareTo(dateFormat.parse(dayAction)) >= 0) {
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

                if ((!age.isEmpty() && Integer.parseInt(age) > 18) || (!age.isEmpty() && Integer.parseInt(age) < 0)) {
                    Toast.makeText(getActivity(), "Tuổi ước lượng phải lớn 0 và nhỏ hơn 18 tuổi.", Toast.LENGTH_SHORT).show();
                    txtAge.setEnabled(true);
                    txtAge.setText("");
                    spnBirthday.setText("");
                }
            }
        });

        spnDayAction.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                SimpleDateFormat dateFormat = new SimpleDateFormat(DateUtils.DATE_FORMAT_DD_MM_YYYY);
                try {
                    String birthday = spnBirthday.getText().toString();
                    String dayAction = spnDayAction.getText().toString();

                    if (!birthday.isEmpty() && !dayAction.isEmpty() && dateFormat.parse(birthday).compareTo(dateFormat.parse(dayAction)) >= 0) {
                        spnDayAction.setText("");
                        Toast.makeText(getActivity(), "Thời gian xảy ra không đúng.", Toast.LENGTH_SHORT).show();
                    } else if (!dayAction.isEmpty() && dateFormat.parse(dayAction).compareTo(dateFormat.parse(DateUtils.CurrentDate(DateUtils.DATE_FORMAT_DD_MM_YYYY))) > 0) {
                        spnDayAction.setText("");
                        Toast.makeText(getActivity(), "Thời gian xảy ra không đúng.", Toast.LENGTH_SHORT).show();
                    }
                } catch (Exception ex) {
                    spnDayAction.setText("");
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
        String dataFixAbuse = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Abuse, null);
        if (!Utils.isEmpty(dataFixAbuse)) {
            listAbuse = new Gson().fromJson(dataFixAbuse, new TypeToken<List<ChildAbuseModel>>() {
            }.getType());

            if (listAbuse != null && listAbuse.size() > 0) {
                abuseListAdapter = new AbuseListAdapter(getActivity(), listAbuse, true);
                rvAbuse.setAdapter(abuseListAdapter);
                abuseListAdapter.SetOnItemClickListener(new AbuseListAdapter.OnItemClickListener() {
                    @Override
                    public void onItemClick(View view, int position, boolean isCheck) {
                        listAbuse.get(position).IsCheck = isCheck;
                        abuseListAdapter.notifyDataSetChanged();

                        boolean check = false;
                        if (listAbuse != null && listAbuse.size() > 0) {
                            for (ChildAbuseModel item : listAbuse) {
                                if (item.IsCheck) {
                                    check = true;
                                    break;
                                }
                            }
                        }
                    }
                });
            }
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
        lyChild.setVisibility(View.GONE);
        if (global.reportModel != null && global.reportModel.ListChild != null && global.reportModel.ListChild.size() > 0) {
            childModel = global.reportModel.ListChild.get(0);
            indexChild = 0;

            if (global.reportModel.ListChild.size() > 1) {
                lyChild.setVisibility(View.VISIBLE);
            }
        } else {
            childModel = new ChildModel();
        }

        fillInfoChild(childModel);

        setDataChildList();
    }

    /***
     * Fill dữ liệu lên view
     * @param model
     */
    private void fillInfoChild(ChildModel model) {
        try {
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

            viewDefault = rgLevel.getChildAt(0);
            RadioButton radioLevelDefault = ((RadioButton) viewDefault);
            radioLevelDefault.setChecked(true);
            int countLevel = rgLevel.getChildCount();
            for (int i = 0; i < countLevel; i++) {
                View view = rgLevel.getChildAt(i);
                if (view instanceof RadioButton) {
                    RadioButton radioLevel = ((RadioButton) view);
                    if (!Utils.isEmpty(model.Level) && radioLevel.getTag().toString().equals(model.Level)) {
                        radioLevel.setChecked(true);
                    }
                }
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

            if (!Utils.isEmpty(model.DateAction)) {
                spnTimeAction.setText(model.DateAction.split(" ")[1]);
                spnDayAction.setText(DateUtils.ConvertYMDServerToDMY(model.DateAction.split(" ")[0]));
            } else {
                spnTimeAction.setText("");
                spnDayAction.setText("");
            }

            boolean isCheck = false;
            if (listAbuse != null && listAbuse.size() > 0) {
                for (ChildAbuseModel item : listAbuse) {
                    item.IsCheck = false;
                    if (model.ListAbuse != null) {
                        for (ChildAbuseModel itemChoose : model.ListAbuse) {
                            if (itemChoose.AbuseId.equals(item.AbuseId)) {
                                item.IsCheck = true;
                                isCheck = true;
                                break;
                            }
                        }
                    }
                }
            }
            abuseListAdapter.notifyDataSetChanged();
        }catch (Exception ex){}
    }

    /***
     * Lưu thông tin
     */

    private boolean saveInfo() {
        try {
            //Get thông tin
            getInfoModel();

            //Check validate
            if (!validateFrom()) {
                return false;
            }

            indexChild = global.addChild(childModel, indexChild);
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
        if (Utils.isEmpty(childModel.Level)) {
            Toast.makeText(getActivity(), "Hãy đánh giá mức độ nghiêm trọng trẻ bị xâm hại.", Toast.LENGTH_SHORT).show();
            rgLevel.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childModel.ProvinceId)) {
            Toast.makeText(getActivity(), "Chưa chọn Tỉnh/Thành nơi xảy ra.", Toast.LENGTH_SHORT).show();
            spnProvince.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childModel.DistrictId)) {
            Toast.makeText(getActivity(), "Chưa chọn Quận/Huyện nơi xảy ra.", Toast.LENGTH_SHORT).show();
            spnDistrict.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childModel.WardId)) {
            Toast.makeText(getActivity(), "Chưa chọn Xã/Phường nơi xảy ra.", Toast.LENGTH_SHORT).show();
            spnWard.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childModel.Address)) {
            Toast.makeText(getActivity(), "Địa điểm cụ thể không được để trống.", Toast.LENGTH_SHORT).show();
            spnWard.requestFocus();
            return false;
        }

        if (Utils.isEmpty(spnTimeAction.getText().toString()) || Utils.isEmpty(spnDayAction.getText().toString())) {
            Toast.makeText(getActivity(), "Thời gian xảy ra nhập đầy đủ cả thời gian và ngày.", Toast.LENGTH_SHORT).show();
            spnTimeAction.requestFocus();
            return false;
        }

        if (childModel.ListAbuse == null || childModel.ListAbuse.size() == 0) {
            Toast.makeText(getActivity(), "Chưa chọn hình thức xâm hại.", Toast.LENGTH_SHORT).show();
            rvAbuse.requestFocus();
            return false;
        }


        return isValidate;
    }

    /***
     * Get thông tin model
     */
    private void getInfoModel() {
        childModel = new ChildModel();
        childModel.Name = txtName.getText().toString();
        int rgGenderID = rgGender.getCheckedRadioButtonId();
        RadioButton radioButton = rgGender.findViewById(rgGenderID);
        if (radioButton != null) {
            childModel.Gender = radioButton.getTag().toString();
            childModel.GenderName = radioButton.getText().toString();
        }
        childModel.Birthday = DateUtils.ConvertDMYToYMD(spnBirthday.getText().toString());
        try {
            childModel.Age = Integer.parseInt(txtAge.getText().toString());
        } catch (Exception ex) {
            childModel.Age = null;
        }
        int rgLevelID = rgLevel.getCheckedRadioButtonId();
        radioButton = rgLevel.findViewById(rgLevelID);
        if (radioButton != null) {
            childModel.Level = radioButton.getTag().toString();
            childModel.LevelName = radioButton.getText().toString();
        }
        childModel.Address = txtAddress.getText().toString();
        childModel.ProvinceId = spnProvince.getTag() != null ? spnProvince.getTag().toString() : "";
        childModel.ProvinceName = spnProvince.getText().toString();
        childModel.DistrictId = spnDistrict.getTag() != null ? spnDistrict.getTag().toString() : "";
        childModel.DistrictName = spnDistrict.getText().toString();
        childModel.WardId = spnWard.getTag() != null ? spnWard.getTag().toString() : "";
        childModel.WardName = spnWard.getText().toString();
        childModel.FullAddress = (!Utils.isEmpty(childModel.Address) ? (txtAddress.getText().toString() + " - ") : "") + spnWard.getText().toString()
                + " - " + spnDistrict.getText().toString() + " - " + spnProvince.getText().toString();
        if (!Utils.isEmpty(spnDayAction.getText().toString()) && !Utils.isEmpty(spnTimeAction.getText().toString())) {
            childModel.DateAction = DateUtils.ConvertDMYToYMD(spnDayAction.getText().toString()) + " " + spnTimeAction.getText().toString();
        }
        childModel.ListAbuse = new ArrayList<>();
        if (listAbuse != null && listAbuse.size() > 0) {
            for (ChildAbuseModel item : listAbuse) {
                if (item.IsCheck) {
                    childModel.ListAbuse.add(item);
                }
            }
        }
    }

    /***
     * Thêm mới trẻ bị hại
     */
    private void addChild() {
        if (saveInfo()) {
            clearInfoChild();
            setDataChildList();
            lyChild.setVisibility(View.VISIBLE);
            for (ChildAbuseModel item : listAbuse) {
                item.IsCheck = false;
            }
            abuseListAdapter.notifyDataSetChanged();
        }
    }

    private void clearInfoChild() {
        indexChild = -1;
        childModel = new ChildModel();
        fillInfoChild(childModel);
        txtName.requestFocus();
    }

    /***
     * Show Dialog chọn Tỉnh/Thành
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
     * Set dữ liệu list child
     */
    private void setDataChildList() {
        if (global.reportModel.ListChild != null && global.reportModel.ListChild.size() > 0) {
            childListAdapter = new ChildListAdapter(getActivity(), global.reportModel.ListChild);
            rvChild.setAdapter(childListAdapter);
            childListAdapter.SetOnItemClickListener(new ChildListAdapter.OnItemClickListener() {
                @Override
                public void onEditClick(View view, int position, ChildModel obj) {
                    if (indexChild != -1) {
                        addChild();
                    } else {
                        getInfoModel();
                        if (childModel.IsChangeValue()) {
                            if (!saveInfo()) {
                                return;
                            }
                        }
                    }

                    childModel = obj;
                    indexChild = position;
                    fillInfoChild(childModel);
                    childListAdapter.notifyDataSetChanged();
                    if (global.reportModel.ListChild.size() <= 1) {
                        lyChild.setVisibility(View.GONE);
                    }
                }

                @Override
                public void onRemoveClick(View view, int position, ChildModel obj) {
                    global.removeChild(position);
                    childListAdapter.notifyDataSetChanged();
                    clearInfoChild();
                    if (global.reportModel.ListChild.size() < 1) {
                        lyChild.setVisibility(View.GONE);
                    }
                }
            });
            rvChild.requestFocus();
        }
    }

    /***
     * Set
     */
    private void setColorValidate(TextView tv, boolean isNull) {
        if (isNull) {
            tv.setTextColor(getResources().getColor(R.color.red_600));
        } else {
            tv.setTextColor(getResources().getColor(R.color.grey_700));
        }
    }
}
