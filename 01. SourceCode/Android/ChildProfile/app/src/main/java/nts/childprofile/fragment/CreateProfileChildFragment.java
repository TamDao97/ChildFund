package nts.childprofile.fragment;

import android.Manifest;
import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.Color;
import android.graphics.PorterDuff;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.FileProvider;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.AppCompatTextView;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
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

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.ANRequest;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONArrayRequestListener;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.androidnetworking.interfaces.StringRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.squareup.picasso.Picasso;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import nts.childprofile.R;
import nts.childprofile.Sql.DataBaseHelper;
import nts.childprofile.adapter.CheckInputListAdapter;
import nts.childprofile.adapter.CheckListAdapter;
import nts.childprofile.adapter.FamilyListAdapter;
import nts.childprofile.adapter.TextInputListAdapter;
import nts.childprofile.common.Constants;
import nts.childprofile.common.DateUtils;
import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.model.ObjectInputModel;
import nts.childprofile.model.ChildProfileModel;
import nts.childprofile.model.ComboboxResult;
import nts.childprofile.model.FamilyMemberModel;
import nts.childprofile.model.LoginProfileModel;
import nts.childprofile.widget.DividerItemDecoration;

import static android.Manifest.permission.READ_EXTERNAL_STORAGE;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * create an instance of this fragment.
 */
public class CreateProfileChildFragment extends Fragment {
    private View view;
    private RelativeLayout progressDialog;
    private static final int maxStep = 5;
    private int currentStep = 1;
    private LinearLayout step1, step2, step3, step4, step5;
    private List<LinearLayout> listStep = new ArrayList<>();
    private ImageView imageCaptureEngine, imageAvata, imageChoosePhoto, imageAddFamilyInfo, imageSignature, imageSignatureChoose, imageSignatureCapture;
    private ChildProfileModel childProfileModel = new ChildProfileModel();
    private String imageLink = "", imageSignatureLink = "";
    private SharedPreferences sharedPreferencesDataFix;
    private List<ComboboxResult> listEmployeeTitle, listProvince, listDistrict, listWard, listVillage = new ArrayList<>(), listGeligion, listNation, listRelationship, listJob, listSchoolName;
    private ArrayList<String> arrayRelationship;
    private GlobalVariable global;
    private LoginProfileModel loginProfileModel;
    private Dialog dialogFamilyInfo;
    private FamilyMemberModel familyInfoModel;
    private TextView txtNext;
    private boolean isSetCheck = false;
    //Component step 1
    private EditText txtEmployeeName, txtChildCode, txtSchool, txtSchoolOtherName;
    private LinearLayout lySchoolOtherName;
    private EditText spnInfoDate, spnEmployeeTitle, spnNation, spnReligion, spnProvince, spnDistrict, spnWard, spnProgramCode, spnVillage;
    //Component step 2
    private EditText txtNameChild, txtNickName, txtDateOfBirth;
    private EditText spnPrimarySchoolClass, spnSecondarySchoolClass;
    private RadioGroup rgGender;
    private CheckBox cbChildhood, cbOutSchool, cbHandicap, cbPreschool, cbPrimarySchool, cbSecondarySchool , cbHighSchool;
    private RecyclerView recyclerViewSubject, recyclerLeaningCapacity, recyclerHouseWork, recyclerHealth,
            recyclerCharacteristic, recyclerHobbie, recyclerDream;
    private CheckListAdapter checkListFavouriteSubjectAdapter, checkListCapacityAdapter, checkListHouseWorkAdapter,
            checkListCharacteristicAdapter, checkListHobbieAdapter, checkListDreamAdapter;
    private CheckInputListAdapter checkListHealthAdapter, checkListHarvestOutput, checkListNumberPetAdapter;
    private EditText txtDiffrentSubject, txtBestSubject, txtAchievement, txtWorkOther, txtHealthOther, txtPersonalityOther, txtHobbieOther, txtDreamOther;
    //Component step 3
    private RecyclerView recyclerFamily, recyclerLiveParent, recyclerNotLiveParent, recyclerLiveWho;
    private FamilyListAdapter checkListFamilyAdapter;
    private CheckListAdapter checkListLiveParentAdapter, checkListLiveWhoAdapter;
    private CheckInputListAdapter checkListNotLiveParentAdapter;
    private EditText txtNotLiveParentOther, txtLiveWhoOther;
    private AppCompatTextView txtTotalSisters, txtTotalBrothers;
    //Component step 4
    private RecyclerView recyclerTypeHousing, recyclerRoofMaterial, recyclerWallMaterials, recyclerFloorMaterials, recyclerIsElectricity, recyclerSchoolMet,
            recyclerClinicsMet, recyclerWaterSourceMet, recyclerWaterFrom, recyclerRoadConditions, recyclerHarvestOutput, recyclerNumberPet, recyclerIncomeOther;
    private CheckListAdapter checkListTypeHousingAdapter, checkListRoofMaterialAdapter, checkListWallMaterialsAdapter, checkListFloorMaterialsAdapter,
            checkListIsElectricityAdapter, checkListSchoolMetAdapter, checkListClinicsMetAdapter, checkListWaterSourceMetAdapter, checkListWaterFromAdapter,
            checkListRoadConditionsAdapter;
    private CheckInputListAdapter checkListIncomeOtherAdapter;
    private TextInputListAdapter textListHarvestOutputAdapter, textListNumberPetAdapter;
    private EditText txtTypeHousingOther, txtRoofMaterialOther, txtWallMaterialsOther, txtFloorMaterialsOther, txtWaterOther, txtRoadConditionsOther;
    private EditText spnFamilyType;
    //Component step 5
    private EditText txtConsentName, spnConsentRelationship, txtConsentVillage, txtConsentWard;
    private RecyclerView recyclerSiblingsJoiningChildFund;
    private CheckInputListAdapter checkListSiblingsJoiningChildFundAdapter;

    private boolean isParentDead = false;
    private boolean isFatherDead = false;
    private boolean isMotherDead = false;
    private TextView txtClassName;


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_create_profile_child, container, false);

        Toolbar mToolbar = (Toolbar) getActivity().findViewById(R.id.toolbar);
        TextView toolbarTitle = mToolbar.findViewById(R.id.toolbar_title);
        TextView txtOffline = mToolbar.findViewById(R.id.txtOffline);
        toolbarTitle.setText("Tạo mới hồ sơ trẻ/Enrol new children");
        Toolbar.LayoutParams layoutParams = (Toolbar.LayoutParams) toolbarTitle.getLayoutParams();
        layoutParams.gravity = Gravity.LEFT;
        toolbarTitle.setLayoutParams(layoutParams);


        sharedPreferencesDataFix = getActivity().getSharedPreferences(Constants.Childprofile_Data_Fix, Context.MODE_PRIVATE);
        global = (GlobalVariable) getActivity().getApplication();
        loginProfileModel = global.getLoginProfile();

        //Chạy online mới load dữ liệu nền
        if (!global.isStatusRunApp()) {
            Utils.getBackgroundData(getActivity(), loginProfileModel, true);
        }
        if (global.isStatusRunApp()) {
            txtOffline.setVisibility(View.VISIBLE);
        }
        initComponent();

        viewDataSpinners();

        final String dataProfileDraft = sharedPreferencesDataFix.getString(Constants.Key_Data_Profile_Draft, null);
        if (dataProfileDraft != null) {
            new AlertDialog.Builder(getActivity())
                    .setMessage("Bạn có một hồ sơ trẻ lưu nháp. \n Chọn Đồng ý/Ok để tiếp tục với hồ sơ trẻ lưu nháp. \n Chọn Hủy/Cancel bản lưu nháp tự động xóa và bắt đầu một hồ sơ mới?")
                    .setCancelable(false)
                    .setPositiveButton("Đồng ý/Ok", new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int id) {
                            childProfileModel = new Gson().fromJson(dataProfileDraft, ChildProfileModel.class);
                            setDataProfileDraft();
                        }
                    })
                    .setNegativeButton("Hủy/Cancel", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialogInterface, int i) {
                            SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                            editor.remove(Constants.Key_Data_Profile_Draft);
                            editor.apply();
                        }
                    })
                    .show();
        }
        // Inflate the layout for this fragment
        return view;
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        try {
            super.onActivityResult(requestCode, resultCode, data);
            if (resultCode != Activity.RESULT_OK && data == null) {
                return;
            }
            if (requestCode == Constants.REQUEST_IMAGE_CAPTURE) {
                //Xoay ảnh
                Utils.AutoRotateImage(imageLink);

                //Resize ảnh
                //File file = Utils.ResizeImages(getContext(), imageLink);

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

                //Xoay ảnh
                Utils.AutoRotateImage(imageLink);

                //Resize ảnh
                //File file = Utils.ResizeImages(getContext(), imageLink);

                Utils.SetPicture(imageAvata, imageLink);
            } else if (requestCode == Constants.REQUEST_IMAGE_CAPTURE_Signature) {
                //Xoay ảnh
                Utils.AutoRotateImage(imageSignatureLink);

                //Resize ảnh
                //File file = Utils.ResizeImages(getContext(), imageSignatureLink);

                Utils.SetPicture(imageSignature, imageSignatureLink);
            } else if (requestCode == Constants.REQUEST_CHOOSE_IMAGE_Signature) {
                Uri uri = data.getData();
                Cursor cursor = getActivity().getContentResolver().query(uri, null, null, null, null);
                try {
                    if (cursor != null && cursor.moveToFirst()) {
                        String[] filePathColumn = {MediaStore.Images.Media.DATA};
                        int columnIndex = cursor.getColumnIndex(filePathColumn[0]);
                        imageSignatureLink = cursor.getString(columnIndex);
                    }
                } finally {
                    cursor.close();
                }

                //Xoay ảnh
                Utils.AutoRotateImage(imageSignatureLink);

                //Resize ảnh
                //File file = Utils.ResizeImages(getContext(), imageSignatureLink);

                Utils.SetPicture(imageSignature, imageSignatureLink);
            }
            super.onActivityResult(requestCode, resultCode, data);
        } catch (Exception ex) {
        }
    }

    private void initComponent() {
        progressDialog = (RelativeLayout) view.findViewById(R.id.progressDialog);
        step1 = (LinearLayout) view.findViewById(R.id.step1);
        listStep.add(step1);
        step2 = (LinearLayout) view.findViewById(R.id.step2);
        listStep.add(step2);
        step3 = (LinearLayout) view.findViewById(R.id.step3);
        listStep.add(step3);
        step4 = (LinearLayout) view.findViewById(R.id.step4);
        listStep.add(step4);
        step5 = (LinearLayout) view.findViewById(R.id.step5);
        listStep.add(step5);

        includeLayoutStep(currentStep);

        txtNext = view.findViewById(R.id.txtNext);
        ((LinearLayout) view.findViewById(R.id.lyt_back)).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                backStep(currentStep);
                bottomProgressDots(currentStep);
            }
        });

        ((LinearLayout) view.findViewById(R.id.lyt_next)).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                nextStep(currentStep);
                bottomProgressDots(currentStep);
            }
        });

        bottomProgressDots(currentStep);

        imageChoosePhoto = (ImageView) view.findViewById(R.id.imageChoosePhoto);
        imageAvata = (ImageView) view.findViewById(R.id.imageAvata);
        imageCaptureEngine = (ImageView) view.findViewById(R.id.imageCaptureEngine);

        /***
         * Chụp ảnh đại diện
         */
        imageCaptureEngine.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
                    if (ContextCompat.checkSelfPermission(getContext(), Manifest.permission.CAMERA)
                            != PackageManager.PERMISSION_GRANTED) {
                        requestPermissions("camera");
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
                if (ContextCompat.checkSelfPermission(getContext(), READ_EXTERNAL_STORAGE)
                        != PackageManager.PERMISSION_GRANTED) {
                    requestPermissions("đọc bộ nhớ");
                    return;
                }

                Intent intent = new Intent(Intent.ACTION_PICK);
                intent.setType("image/*");
                startActivityForResult(intent, Constants.REQUEST_CHOOSE_IMAGE);
            }
        });

        /***
         * Component step 1
         */

        txtEmployeeName = (EditText) step1.findViewById(R.id.txtEmployeeName);
        spnEmployeeTitle = (EditText) step1.findViewById(R.id.spnEmployeeTitle);
        spnProgramCode = (EditText) step1.findViewById(R.id.spnProgramCode);
        txtChildCode = (EditText) step1.findViewById(R.id.txtChildCode);
        txtSchool = (EditText) step1.findViewById(R.id.spnSchool);
        lySchoolOtherName = (LinearLayout) step1.findViewById(R.id.lySchoolOtherName);
        txtSchoolOtherName = (EditText) step1.findViewById(R.id.txtSchoolOtherName);

        spnInfoDate = (EditText) step1.findViewById(R.id.spnInfoDate);
        spnNation = (EditText) step1.findViewById(R.id.spnNation);
        spnReligion = (EditText) step1.findViewById(R.id.spnReligion);
        spnProvince = (EditText) step1.findViewById(R.id.spnProvince);
        spnDistrict = (EditText) step1.findViewById(R.id.spnDistrict);
        spnWard = (EditText) step1.findViewById(R.id.spnWard);
        spnVillage = (EditText) step1.findViewById(R.id.spnVillage);
        rgGender = (RadioGroup) step2.findViewById(R.id.rgGender);
        txtClassName = step2.findViewById(R.id.txtClassName);
        //Hiển thị thông tin lên giao diện

        spnInfoDate.setText(DateUtils.CurrentDate(DateUtils.DATE_FORMAT_DD_MM_YYYY));
        spnInfoDate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker((EditText) v, getActivity());
            }
        });

        spnVillage.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(final View v) {
                if (spnWard.getTag() == null || Utils.isEmpty(spnWard.getText().toString())) {
                    Toast.makeText(getActivity(), "Hãy chọn Xã/ Phường trước!", Toast.LENGTH_SHORT).show();
                    return;
                }

                if (listVillage != null && listVillage.size() > 0) {
                    final ArrayList<String> arrayVillageName = new ArrayList<>();
                    for (ComboboxResult comboboxResult : listVillage) {
                        arrayVillageName.add(comboboxResult.Name);
                    }
                    showChoiceDialog((EditText) v, arrayVillageName.toArray(new String[0]), listVillage, "Thôn/ Xóm/ Village");
                }
            }
        });

        /***
         * Component step 2
         */
        txtNameChild = (EditText) step2.findViewById(R.id.txtNameChild);
        txtNickName = (EditText) step2.findViewById(R.id.txtNickName);
        txtDateOfBirth = (EditText) step2.findViewById(R.id.txtDateOfBirth);
        spnPrimarySchoolClass = (EditText) step2.findViewById(R.id.spnPrimarySchoolClass);
        spnSecondarySchoolClass = (EditText) step2.findViewById(R.id.spnSecondarySchoolClass);


        cbChildhood = (CheckBox) step2.findViewById(R.id.cbChildhood);
        cbOutSchool = (CheckBox) step2.findViewById(R.id.cbOutSchool);
        cbHandicap = (CheckBox) step2.findViewById(R.id.cbHandicap);
        cbPreschool = (CheckBox) step2.findViewById(R.id.cbPreschool);
        cbPrimarySchool = (CheckBox) step2.findViewById(R.id.cbPrimarySchool);
        cbSecondarySchool = (CheckBox) step2.findViewById(R.id.cbSecondarySchool);
        cbHighSchool = step2.findViewById(R.id.cbHighSchool);

        txtDiffrentSubject = (EditText) step2.findViewById(R.id.txtDiffrentSubject);
        txtBestSubject = (EditText) step2.findViewById(R.id.txtBestSubject);
        txtAchievement = (EditText) step2.findViewById(R.id.txtAchievement);
        txtWorkOther = (EditText) step2.findViewById(R.id.txtWorkOther);
        txtHealthOther = (EditText) step2.findViewById(R.id.txtHealthOther);
        txtPersonalityOther = (EditText) step2.findViewById(R.id.txtPersonalityOther);
        txtHobbieOther = (EditText) step2.findViewById(R.id.txtHobbieOther);
        txtDreamOther = (EditText) step2.findViewById(R.id.txtDreamOther);

        rgGender.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                RadioButton checkedRadioButton = (RadioButton) group.findViewById(checkedId);
                childProfileModel.Gender = Integer.parseInt(checkedRadioButton.getTag().toString());
            }
        });

        txtDateOfBirth.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker((EditText) v, getActivity());
            }
        });

        cbChildhood.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    childProfileModel.LeaningStatus = "11";
                    cbOutSchool.setChecked(false);
                    cbPreschool.setChecked(false);
                    cbPrimarySchool.setChecked(false);
                    cbSecondarySchool.setChecked(false);
                    cbHighSchool.setChecked(false);
                    spnSecondarySchoolClass.setText("");
                    spnPrimarySchoolClass.setText("");
                    spnSecondarySchoolClass.setClickable(false);
                    spnPrimarySchoolClass.setClickable(false);
                }else  if (childProfileModel.LeaningStatus.equals("11")){
                    childProfileModel.LeaningStatus = "";
                }
            }
        });

        cbOutSchool.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    childProfileModel.LeaningStatus = "12";
                    cbChildhood.setChecked(false);
                    cbHighSchool.setChecked(false);
                    cbPreschool.setChecked(false);
                    cbPrimarySchool.setChecked(false);
                    cbSecondarySchool.setChecked(false);
                    spnSecondarySchoolClass.setText("");
                    spnPrimarySchoolClass.setText("");
                    spnSecondarySchoolClass.setClickable(false);
                    spnPrimarySchoolClass.setClickable(false);
                }else  if (childProfileModel.LeaningStatus.equals("12")){
                    childProfileModel.LeaningStatus = "";
                }
            }
        });

        cbHandicap.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
//                if (isChecked) {
                childProfileModel.Handicap = isChecked;
//                    cbChildhood.setChecked(false);
//                    cbOutSchool.setChecked(false);
//                    cbPreschool.setChecked(false);
//                    cbPrimarySchool.setChecked(false);
//                    cbSecondarySchool.setChecked(false);
//                    spnSecondarySchoolClass.setText("");
//                    spnPrimarySchoolClass.setText("");
//                    spnSecondarySchoolClass.setClickable(false);
//                    spnPrimarySchoolClass.setClickable(false);

            }
        });

        cbPreschool.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    childProfileModel.LeaningStatus = "14";
                    cbChildhood.setChecked(false);
                    cbOutSchool.setChecked(false);
                    cbHighSchool.setChecked(false);
                    cbPrimarySchool.setChecked(false);
                    cbSecondarySchool.setChecked(false);
                    spnSecondarySchoolClass.setText("");
                    spnPrimarySchoolClass.setText("");
                    spnSecondarySchoolClass.setClickable(false);
                    spnPrimarySchoolClass.setClickable(false);
                }else  if (childProfileModel.LeaningStatus.equals("14")){
                    childProfileModel.LeaningStatus = "";
                }
            }
        });

        cbPrimarySchool.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    childProfileModel.LeaningStatus = "15";
                    cbChildhood.setChecked(false);
                    cbOutSchool.setChecked(false);
                    cbHighSchool.setChecked(false);
                    cbPreschool.setChecked(false);
                    cbSecondarySchool.setChecked(false);
                    spnSecondarySchoolClass.setText("");
                    spnSecondarySchoolClass.setClickable(false);
                    spnPrimarySchoolClass.setClickable(true);
                    if (!isSetCheck) {
//                        spnPrimarySchoolClass.callOnClick();
                    }
                    isSetCheck = false;
                }else  if (childProfileModel.LeaningStatus.equals("15")){
                    childProfileModel.LeaningStatus = "";
                }
            }
        });

        cbSecondarySchool.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    childProfileModel.LeaningStatus = "16";
                    cbChildhood.setChecked(false);
                    cbOutSchool.setChecked(false);
                    cbHighSchool.setChecked(false);
                    cbPreschool.setChecked(false);
                    cbPrimarySchool.setChecked(false);
                    spnPrimarySchoolClass.setText("");
                    spnPrimarySchoolClass.setClickable(false);
                    spnSecondarySchoolClass.setClickable(true);
                    if (!isSetCheck) {
//                        spnSecondarySchoolClass.callOnClick();
                    }
                    isSetCheck = false;
                }else  if (childProfileModel.LeaningStatus.equals("16")){
                    childProfileModel.LeaningStatus = "";
                }
            }
        });

        cbHighSchool.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    childProfileModel.LeaningStatus = "17";
                    cbChildhood.setChecked(false);
                    cbOutSchool.setChecked(false);
                    cbPreschool.setChecked(false);
                    cbPrimarySchool.setChecked(false);
                    cbSecondarySchool.setChecked(false);
                    spnPrimarySchoolClass.setText("");
                    spnPrimarySchoolClass.setClickable(false);
                    spnSecondarySchoolClass.setClickable(false);
                    cbHighSchool.setClickable(true);
                    if (!isSetCheck) {
//                        spnSecondarySchoolClass.callOnClick();
                    }
                    isSetCheck = false;
                }else  if (childProfileModel.LeaningStatus.equals("17")){
                    childProfileModel.LeaningStatus = "";
                }
            }
        });

        recyclerViewSubject = (RecyclerView) step2.findViewById(R.id.recyclerViewSubject);
        recyclerViewSubject.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerViewSubject.setHasFixedSize(true);
        recyclerViewSubject.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerLeaningCapacity = (RecyclerView) step2.findViewById(R.id.recyclerLeaningCapacity);
        recyclerLeaningCapacity.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerLeaningCapacity.setHasFixedSize(true);
        recyclerLeaningCapacity.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerHouseWork = (RecyclerView) step2.findViewById(R.id.recyclerHouseWork);
        recyclerHouseWork.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerHouseWork.setHasFixedSize(true);
        recyclerHouseWork.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerHealth = (RecyclerView) step2.findViewById(R.id.recyclerHealth);
        recyclerHealth.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerHealth.setHasFixedSize(true);
        recyclerHealth.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerCharacteristic = (RecyclerView) step2.findViewById(R.id.recyclerPersonality);
        recyclerCharacteristic.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerCharacteristic.setHasFixedSize(true);
        recyclerCharacteristic.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerHobbie = (RecyclerView) step2.findViewById(R.id.recyclerHobbie);
        recyclerHobbie.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerHobbie.setHasFixedSize(true);
        recyclerHobbie.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerDream = (RecyclerView) step2.findViewById(R.id.recyclerDream);
        recyclerDream.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerDream.setHasFixedSize(true);
        recyclerDream.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        /***
         * Component step 3
         */

        imageAddFamilyInfo = (ImageView) step3.findViewById(R.id.imageAddFamilyInfo);
        imageAddFamilyInfo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                initDialogFamilyInfo(false);
                familyInfoModel = new FamilyMemberModel();
                dialogFamilyInfo.show();
                Window window = dialogFamilyInfo.getWindow();
                window.setLayout(WindowManager.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
            }
        });

        recyclerFamily = (RecyclerView) step3.findViewById(R.id.recyclerFamily);
        recyclerFamily.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerFamily.setHasFixedSize(true);
        recyclerFamily.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerLiveParent = (RecyclerView) step3.findViewById(R.id.recyclerLiveParent);
        recyclerLiveParent.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerLiveParent.setHasFixedSize(true);
        recyclerLiveParent.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));


        recyclerNotLiveParent = (RecyclerView) step3.findViewById(R.id.recyclerNotLiveParent);
        recyclerNotLiveParent.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerNotLiveParent.setHasFixedSize(true);
        recyclerNotLiveParent.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerLiveWho = (RecyclerView) step3.findViewById(R.id.recyclerLiveWho);
        recyclerLiveWho.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerLiveWho.setHasFixedSize(true);
        recyclerLiveWho.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        txtNotLiveParentOther = (EditText) step3.findViewById(R.id.txtNotLiveParentOther);
        txtLiveWhoOther = (EditText) step3.findViewById(R.id.txtLiveWhoOther);
        txtTotalSisters = (AppCompatTextView) step3.findViewById(R.id.txtTotalSisters);
        txtTotalBrothers = (AppCompatTextView) step3.findViewById(R.id.txtTotalBrothers);

        /***
         * Component step 4
         */
        recyclerTypeHousing = (RecyclerView) step4.findViewById(R.id.recyclerTypeHousing);
        recyclerTypeHousing.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerTypeHousing.setHasFixedSize(true);
        recyclerTypeHousing.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerRoofMaterial = (RecyclerView) step4.findViewById(R.id.recyclerRoofMaterial);
        recyclerRoofMaterial.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerRoofMaterial.setHasFixedSize(true);
        recyclerRoofMaterial.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerWallMaterials = (RecyclerView) step4.findViewById(R.id.recyclerWallMaterials);
        recyclerWallMaterials.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerWallMaterials.setHasFixedSize(true);
        recyclerWallMaterials.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));


        recyclerFloorMaterials = (RecyclerView) step4.findViewById(R.id.recyclerFloorMaterials);
        recyclerFloorMaterials.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerFloorMaterials.setHasFixedSize(true);
        recyclerFloorMaterials.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerIsElectricity = (RecyclerView) step4.findViewById(R.id.recyclerIsElectricity);
        recyclerIsElectricity.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerIsElectricity.setHasFixedSize(true);
        recyclerIsElectricity.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerSchoolMet = (RecyclerView) step4.findViewById(R.id.recyclerSchoolMet);
        recyclerSchoolMet.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerSchoolMet.setHasFixedSize(true);
        recyclerSchoolMet.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerClinicsMet = (RecyclerView) step4.findViewById(R.id.recyclerClinicsMet);
        recyclerClinicsMet.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerClinicsMet.setHasFixedSize(true);
        recyclerClinicsMet.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerWaterSourceMet = (RecyclerView) step4.findViewById(R.id.recyclerWaterSourceMet);
        recyclerWaterSourceMet.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerWaterSourceMet.setHasFixedSize(true);
        recyclerWaterSourceMet.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerWaterFrom = (RecyclerView) step4.findViewById(R.id.recyclerWaterFrom);
        recyclerWaterFrom.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerWaterFrom.setHasFixedSize(true);
        recyclerWaterFrom.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerRoadConditions = (RecyclerView) step4.findViewById(R.id.recyclerRoadConditions);
        recyclerRoadConditions.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerRoadConditions.setHasFixedSize(true);
        recyclerRoadConditions.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

//        recyclerIncomeFamily = (RecyclerView) step4.findViewById(R.id.recyclerIncomeFamily);
//        recyclerIncomeFamily.setLayoutManager(new GridLayoutManager(getActivity(), 1));
//        recyclerIncomeFamily.setHasFixedSize(true);
//        recyclerIncomeFamily.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerHarvestOutput = (RecyclerView) step4.findViewById(R.id.recyclerHarvestOutput);
        recyclerHarvestOutput.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerHarvestOutput.setHasFixedSize(true);
        recyclerHarvestOutput.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerNumberPet = (RecyclerView) step4.findViewById(R.id.recyclerNumberPet);
        recyclerNumberPet.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerNumberPet.setHasFixedSize(true);
        recyclerNumberPet.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        recyclerIncomeOther = (RecyclerView) step4.findViewById(R.id.recyclerIncomeOther);
        recyclerIncomeOther.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerIncomeOther.setHasFixedSize(true);
        recyclerIncomeOther.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        txtTypeHousingOther = (EditText) step4.findViewById(R.id.txtTypeHousingOther);
        txtRoofMaterialOther = (EditText) step4.findViewById(R.id.txtRoofMaterialOther);
        txtWallMaterialsOther = (EditText) step4.findViewById(R.id.txtWallMaterialsOther);
        txtFloorMaterialsOther = (EditText) step4.findViewById(R.id.txtFloorMaterialsOther);
        txtWaterOther = (EditText) step4.findViewById(R.id.txtWaterOther);
        txtRoadConditionsOther = (EditText) step4.findViewById(R.id.txtRoadConditionsOther);

        spnFamilyType = step4.findViewById(R.id.spnFamilyType);
        // txtIncomeSources = step4.findViewById(R.id.txtIncomeSources);

        //Component step 5
        imageSignature = (ImageView) view.findViewById(R.id.imageSignature);
        imageSignatureCapture = (ImageView) view.findViewById(R.id.imageSignatureCapture);
        imageSignatureChoose = (ImageView) view.findViewById(R.id.imageSignatureChoose);
        txtConsentName = step5.findViewById(R.id.txtConsentName);
        spnConsentRelationship = step5.findViewById(R.id.spnConsentRelationship);
        txtConsentVillage = step5.findViewById(R.id.txtConsentVillage);
        txtConsentWard = step5.findViewById(R.id.txtConsentWard);
        recyclerSiblingsJoiningChildFund = step5.findViewById(R.id.recyclerSiblingsJoiningChildFund);
        recyclerSiblingsJoiningChildFund.setLayoutManager(new GridLayoutManager(getActivity(), 2));
        recyclerSiblingsJoiningChildFund.setHasFixedSize(true);
        recyclerSiblingsJoiningChildFund.addItemDecoration(new DividerItemDecoration(getActivity(), DividerItemDecoration.VERTICAL_LIST));

        /***
         * Chụp ảnh đại diện
         */
        imageSignatureCapture.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
                    if (ContextCompat.checkSelfPermission(getContext(), Manifest.permission.CAMERA)
                            != PackageManager.PERMISSION_GRANTED) {
                        requestPermissions("camera");
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
                        imageSignatureLink = photoFile.getAbsolutePath();
                        Uri photoUri = FileProvider.getUriForFile(getActivity(), getActivity().getPackageName() + ".nts.childprofile.provider", photoFile);
                        intent.putExtra(MediaStore.EXTRA_OUTPUT, photoUri);
                        startActivityForResult(intent, Constants.REQUEST_IMAGE_CAPTURE_Signature);
                    }
                } catch (Exception ex) {
                }
            }
        });

        /***
         * Chọn ảnh đại diện
         */
        imageSignatureChoose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (ContextCompat.checkSelfPermission(getContext(), READ_EXTERNAL_STORAGE)
                        != PackageManager.PERMISSION_GRANTED) {
                    requestPermissions("đọc bộ nhớ");
                    return;
                }

                Intent intent = new Intent(Intent.ACTION_PICK);
                intent.setType("image/*");
                startActivityForResult(intent, Constants.REQUEST_CHOOSE_IMAGE_Signature);
            }
        });

        if (global.isStatusRunApp()) {
            childProfileModel = new ChildProfileModel();
            childProfileModel.convertObjectJsonToModel(getContext());
            setInfoProfileChild();
        } else {
            viewProfileChild();
        }
    }

    /***
     * Show Dialog chọn item
     * @param bt
     * @param arrayName
     */
    private void showChoiceDialog(final EditText bt, final String[] arrayName, String title) {
        final AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, Utils.getIndexByName(arrayName, Utils.getText(bt)), new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                bt.setTextColor(Color.BLACK);
                bt.setText(arrayName[which]);
            }
        });
        builder.show();
    }

    /***
     * Show Dialog chọn item
     * @param bt
     * @param arrayName
     */
    private void showChoiceRelationshipDialog(final EditText bt, final RadioGroup rgRelationshipGender, final String[] arrayName, final List<ComboboxResult> arraySource, String title) {
        final AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, Utils.getIndexByName(arrayName, Utils.getText(bt)), new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                bt.setTextColor(Color.BLACK);
                bt.setText(arrayName[which]);
                bt.setTag(arraySource.get(which).Id);

                String gender = arraySource.get(which).PId;
                if (gender != null) {
                    if (gender.equals(String.valueOf(Constants.Male))) {
                        RadioButton radioMale = rgRelationshipGender.findViewById(R.id.radioMale);
                        radioMale.setChecked(true);
                    } else {
                        RadioButton radioFemale = rgRelationshipGender.findViewById(R.id.radioFemale);
                        radioFemale.setChecked(true);
                    }
                }
            }
        });
        builder.show();
    }

    /***
     * Show Dialog chọn item
     * @param bt
     * @param arrayName
     */
    private void showChoiceDialog(final EditText bt, final String[] arrayName, final List<ComboboxResult> arraySource, final String title) {
        final AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, Utils.getIndexByName(arrayName, Utils.getText(bt)), new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                bt.setTextColor(Color.BLACK);
                bt.setText(arrayName[which]);
                bt.setTag(arraySource.get(which).Id);

                if (title.equals("Xã/ Commune")) {
                    txtConsentWard.setText(arrayName[which]);
                    spnVillage.setTag("");
                    spnVillage.setText("");

                    getDataVillage(arraySource.get(which).Id);
                    getDataSchool(arraySource.get(which).Id);
                } else if (title.equals("Thôn/ Xóm/ Village")) {
                    txtConsentVillage.setText(arrayName[which]);
                } else if (title.equals("Nghề nghiệp/ Occupation")) {
                    LinearLayout lyJobOtherName = dialogFamilyInfo.findViewById(R.id.lyJobOtherName);
                    if (arrayName[which].equals("Khác/Other")) {
                        lyJobOtherName.setVisibility(View.VISIBLE);
                    } else {
                        lyJobOtherName.setVisibility(View.GONE);
                    }
                } else if (title.equals("Trường-lớp/ School-grade")) {
                    if (arraySource.get(which).Id.equals("Other")) {
                        lySchoolOtherName.setVisibility(View.VISIBLE);
                        txtSchoolOtherName.requestFocus();
                    } else {
                        lySchoolOtherName.setVisibility(View.GONE);
                    }
                }
            }
        });
        builder.show();
    }

    // Lưu thông tin hồ sơ offline
    private void saveProfileChildOffline() {
        progressDialog.setVisibility(View.VISIBLE);
        childProfileModel.School = txtSchool.getText().toString();
        if (!imageLink.isEmpty()) {
            childProfileModel.ImagePath = imageLink;
            childProfileModel.ImageThumbnailPath = imageLink;
        }
        if (!imageSignatureLink.isEmpty()) {
            childProfileModel.ImageSignaturePath = imageSignatureLink;
            childProfileModel.ImageSignatureThumbnailPath = imageSignatureLink;
        }
        try {
            DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
            dataBaseHelper.updateChildProfile(childProfileModel);
            progressDialog.setVisibility(View.GONE);
            Toast.makeText(getActivity(), "Lưu hồ sơ trẻ thành công!", Toast.LENGTH_SHORT).show();
            resetForm();
            SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
            editor.remove(Constants.Key_Data_Profile_Draft);
            editor.apply();
            progressDialog.setVisibility(View.GONE);
        } catch (Exception ex) {
            Toast.makeText(getActivity(), "Lưu hồ sơ trẻ không thành công!", Toast.LENGTH_SHORT).show();
        }

    }

    //Next step
    private void nextStep(int progress) {
        if (!ValidateFrom()) {
            return;
        }

        if (progress == maxStep) {
            if (global.isStatusRunApp()) {
                saveProfileChildOffline();
            } else {
                saveProfileChild();
            }
        }

        if (progress < maxStep) {
            progress++;
            currentStep = progress;
            saveDraftChildProfile();
            includeLayoutStep(currentStep);
            txtNext.setText("Tiếp theo/ NEXT");
            if (currentStep == maxStep) {
                txtNext.setText("LƯU/ Save");
            }
        }
    }

    public void backListProfile() {
        new AlertDialog.Builder(getActivity())
                .setMessage("Bạn có muốn thoát khỏi chức năng Thêm mới hồ sơ này/Exit?")
                .setCancelable(false)
                .setPositiveButton("Đồng ý/Ok", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        Fragment fragment = new MenuFragment();
                        Utils.ChangeFragment(getActivity(), fragment, null);
                    }
                })
                .setNegativeButton("Đóng/Close", null)
                .setNeutralButton("Lưu nháp/Save draft", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        saveDraftChildProfile();
                        Fragment fragment = new MenuFragment();
                        Utils.ChangeFragment(getActivity(), fragment, null);
                    }
                })
                .show();
    }

    private void saveDraftChildProfile() {
//        if (!ValidateFrom()) {
//            return;
//        }
//        GetInfoStep1();
//        GetInfoStep2();
//        GetInfoStep3();
//        GetInfoStep4();
//        GetInfoStep5();
        if (!imageLink.isEmpty()) {
            childProfileModel.ImagePath = imageLink;
            childProfileModel.ImageThumbnailPath = imageLink;
        }
        if (!imageSignatureLink.isEmpty()) {
            childProfileModel.ImageSignaturePath = imageSignatureLink;
            childProfileModel.ImageSignatureThumbnailPath = imageSignatureLink;
        }

        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
        editor.putString(Constants.Key_Data_Profile_Draft, new Gson().toJson(childProfileModel));
        editor.apply();
    }

    //Back step
    private void backStep(int progress) {
        if (progress > 1) {
            progress--;
            currentStep = progress;
            includeLayoutStep(currentStep);
        } else {
            backListProfile();
        }
        txtNext.setText("Tiếp theo/ NEXT");
    }

    private void bottomProgressDots(int current_index) {
        current_index--;
        LinearLayout dotsLayout = (LinearLayout) view.findViewById(R.id.layoutDots);
        ImageView[] dots = new ImageView[maxStep];

        dotsLayout.removeAllViews();
        for (int i = 0; i < dots.length; i++) {
            dots[i] = new ImageView(getActivity());
            int width_height = 15;
            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(new ViewGroup.LayoutParams(width_height, width_height));
            params.setMargins(10, 10, 10, 10);
            dots[i].setLayoutParams(params);
            dots[i].setImageResource(R.drawable.shape_circle);
            dots[i].setColorFilter(getResources().getColor(R.color.colorPrimaryDark), PorterDuff.Mode.SRC_IN);
            dotsLayout.addView(dots[i]);
        }

        if (dots.length > 0) {
            int width_height = 30;
            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(new ViewGroup.LayoutParams(width_height, width_height));
            dots[current_index].setLayoutParams(params);
            dots[current_index].setImageResource(R.drawable.shape_circle);
            dots[current_index].setColorFilter(getResources().getColor(R.color.colorRed), PorterDuff.Mode.SRC_IN);
        }
    }

    //Ẩn hiện step
    private void includeLayoutStep(int current_index) {
        for (int index = 0; index < maxStep; index++) {
            if ((current_index - 1) == index)
                listStep.get(index).setVisibility(View.VISIBLE);
            else
                listStep.get(index).setVisibility(View.GONE);
        }
    }

    ///Lưu thông tin hồ sơ
    private void saveProfileChild() {
        String jsonModel = new Gson().toJson(childProfileModel);
        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/ChildProfiles/AddChildProfile"));

        if (!imageLink.isEmpty()) {
            File file = new File(imageLink);
            anRequest.addMultipartFile("Avatar", file);
        }

        if (!imageSignatureLink.isEmpty()) {
            File file = new File(imageSignatureLink);
            anRequest.addMultipartFile("ImageSignature", file);
        }

        progressDialog.setVisibility(View.VISIBLE);
        anRequest.addMultipartParameter("Model", jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String response) {
                        progressDialog.setVisibility(View.GONE);
                        Toast.makeText(getActivity(), "Thêm mới hồ sơ trẻ thành công!", Toast.LENGTH_SHORT).show();
                        resetForm();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.remove(Constants.Key_Data_Profile_Draft);
                        editor.apply();
                    }

                    @Override
                    public void onError(ANError anError) {
                        progressDialog.setVisibility(View.GONE);
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    ///////
    /// Check validate from
    //////
    private boolean ValidateFrom() {
        GetInfoStep();
        boolean isValidate = true;
        switch (currentStep) {
            case 1: {
                isValidate = ValidateStep1();
                break;
            }
            case 2: {
                isValidate = ValidateStep2();
                break;
            }
            case 3: {
                break;
            }
            case 4: {
                break;
            }
            case 5: {
                isValidate = ValidateStep5();
                break;
            }
        }
        return isValidate;
    }

    /***
     * Validate fragment set 1
     */
    private boolean ValidateStep1() {
        if (Utils.isEmpty(childProfileModel.EmployeeName)) {
            Toast.makeText(getActivity(), "Tên cán bộ phụ trách không được để trống.", Toast.LENGTH_SHORT).show();
            txtEmployeeName.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childProfileModel.InfoDate)) {
            Toast.makeText(getActivity(), "Ngày thu thập không được để trống.", Toast.LENGTH_SHORT).show();
            spnInfoDate.requestFocus();
            return false;
        }

        if (!Utils.isEmpty(childProfileModel.InfoDate)) {
            SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd");
            try {
                childProfileModel.InfoDate = DateUtils.ConvertDMYToYMD(childProfileModel.InfoDate);
                Date date = format.parse(childProfileModel.InfoDate);
                spnInfoDate.requestFocus();
            } catch (Exception ex) {
                Toast.makeText(getActivity(), "Ngày thu thập lỗi định dạng. Định dạng đúng 'dd/MM/yyyy'", Toast.LENGTH_SHORT).show();
                return false;
            }
        }

        if (Utils.isEmpty(childProfileModel.ProgramCode)) {
            Toast.makeText(getActivity(), "Mã chương trình không được để trống.", Toast.LENGTH_SHORT).show();
            spnProgramCode.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childProfileModel.ChildCode)) {
            Toast.makeText(getActivity(), "Mã số trẻ không được để trống.", Toast.LENGTH_SHORT).show();
            txtChildCode.requestFocus();
            return false;
        }

//        if (childProfileModel.School == null) {
//            Toast.makeText(getActivity(), "Số thứ tự trẻ không được để trống.", Toast.LENGTH_SHORT).show();
//            txtSchool.requestFocus();
//            return false;
//        }

        if (Utils.isEmpty(childProfileModel.EthnicId)) {
            Toast.makeText(getActivity(), "Dân tộc không được để trống.", Toast.LENGTH_SHORT).show();
            spnNation.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childProfileModel.ProvinceId)) {
            Toast.makeText(getActivity(), "Tỉnh/Thành không được để trống.", Toast.LENGTH_SHORT).show();
            spnProvince.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childProfileModel.DistrictId)) {
            Toast.makeText(getActivity(), "Quận/Huyện không được để trống.", Toast.LENGTH_SHORT).show();
            spnDistrict.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childProfileModel.WardId)) {
            Toast.makeText(getActivity(), "Xã/Phường không được để trống.", Toast.LENGTH_SHORT).show();
            spnWard.requestFocus();
            return false;
        }
        return true;
    }

    /***
     * Validate fragment set 2
     */
    private boolean ValidateStep2() {
        if (Utils.isEmpty(childProfileModel.Name)) {
            Toast.makeText(getActivity(), "Họ và tên không được để trống.", Toast.LENGTH_SHORT).show();
            txtNameChild.requestFocus();
            return false;
        }

        if (Utils.isEmpty(childProfileModel.DateOfBirth)) {
            Toast.makeText(getActivity(), "Ngày sinh không được để trống.", Toast.LENGTH_SHORT).show();
            txtDateOfBirth.requestFocus();
            return false;
        }

        if (!Utils.isEmpty(childProfileModel.DateOfBirth)) {
            SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd");
            try {
                childProfileModel.DateOfBirth = DateUtils.ConvertDMYToYMD(childProfileModel.DateOfBirth);
                Date date = format.parse(childProfileModel.DateOfBirth);
            } catch (Exception ex) {
                Toast.makeText(getActivity(), "Ngày sinh lỗi định dạng. Định dạng đúng 'dd/MM/yyyy'.", Toast.LENGTH_SHORT).show();
                txtDateOfBirth.requestFocus();
                return false;
            }
        }

        if (Utils.isEmpty(childProfileModel.LeaningStatus)) {
            Toast.makeText(getActivity(), "Hãy xác định xem trẻ có đi học không.", Toast.LENGTH_SHORT).show();
            cbChildhood.requestFocus();
            return false;
        }
        if (childProfileModel.LeaningStatus.equals("15") || childProfileModel.LeaningStatus.equals("16") || childProfileModel.LeaningStatus.equals("17")) {
            if (Utils.isEmpty(childProfileModel.ClassInfo)) {
                Toast.makeText(getActivity(), "Hãy xác định xem trẻ học lớp nào.", Toast.LENGTH_SHORT).show();
                txtClassName.requestFocus();
                return false;
            } else {
                if (childProfileModel.LeaningStatus.equals("15")) {
                    String checkClass = childProfileModel.ClassInfo.substring(0, 1);
                    try {
                        if (Integer.valueOf(checkClass) > 5) {
                            Toast.makeText(getActivity(), "Lớp học không phải cấp 1.", Toast.LENGTH_SHORT).show();
                            return false;
                        }
                    } catch (Exception e) {
                        Toast.makeText(getActivity(), "Lớp học không phải cấp 1.", Toast.LENGTH_SHORT).show();
                        return false;
                    }
                } else if (childProfileModel.LeaningStatus.equals("16")) {
                    String checkClass = childProfileModel.ClassInfo.substring(0, 1);
                    try {
                        if (Integer.valueOf(checkClass) < 6 || Integer.valueOf(checkClass) > 9) {
                            Toast.makeText(getActivity(), "Lớp học không phải cấp 2.", Toast.LENGTH_SHORT).show();
                            return false;
                        }
                    } catch (Exception e) {
                        Toast.makeText(getActivity(), "Lớp học không phải cấp 2.", Toast.LENGTH_SHORT).show();
                        return false;
                    }
                } else if (childProfileModel.LeaningStatus.equals("17")) {
                    String checkClass = childProfileModel.ClassInfo.substring(0, 2);
                    try {
                        if (Integer.valueOf(checkClass) > 12 || Integer.valueOf(checkClass) < 10) {
                            Toast.makeText(getActivity(), "Lớp học không phải cấp 3.", Toast.LENGTH_SHORT).show();
                            return false;
                        }
                    } catch (Exception e) {
                        Toast.makeText(getActivity(), "Lớp học không phải cấp 3.", Toast.LENGTH_SHORT).show();
                        return false;
                    }
                }
            }
        }
        return true;
    }

    /***
     * Validate fragment set 5
     */
    private boolean ValidateStep5() {
        if (Utils.isEmpty(imageSignatureLink)) {
            Toast.makeText(getActivity(), "Chưa chụp ảnh chữ ký của gia đình trẻ đồng thuận tham gia chương trình ChildFund.", Toast.LENGTH_SHORT).show();
            imageSignature.requestFocus();
            return false;
        }
        return true;
    }

    ///////
    /// Get info step
    //////
    private void GetInfoStep() {
        try {
            switch (currentStep) {
                case 1: {
                    GetInfoStep1();
                    break;
                }
                case 2: {
                    GetInfoStep2();
                    break;
                }
                case 3: {
                    GetInfoStep3();
                    break;
                }
                case 4: {
                    GetInfoStep4();
                    break;
                }
                case 5: {
                    GetInfoStep5();
                    break;
                }
            }
        } catch (Exception ex) {
        }
    }

    /***
     * Get thông tin chung về trẻ và chương trình
     */
    private void GetInfoStep1() {
        childProfileModel.UserLever = loginProfileModel.UserLever;
        childProfileModel.ProcessStatus = Constants.ProfilesNew;
        childProfileModel.CreateBy = loginProfileModel.Id;
        childProfileModel.UpdateBy = loginProfileModel.Id;

        childProfileModel.EmployeeName = txtEmployeeName.getText().toString();
        childProfileModel.EmployeeTitle = spnEmployeeTitle.getTag() != null ? spnEmployeeTitle.getTag().toString() : "";
        childProfileModel.InfoDate = spnInfoDate.getText().toString();
        childProfileModel.ProgramCode = spnProgramCode.getText().toString();
        childProfileModel.ChildCode = txtChildCode.getText().toString();
        childProfileModel.SchoolId = txtSchool.getTag() != null ? txtSchool.getTag().toString() : "";
        childProfileModel.SchoolOtherName = "";
        if (childProfileModel.SchoolId.equals("Other")) {
            childProfileModel.SchoolOtherName = txtSchoolOtherName.getText().toString();
        }
        childProfileModel.EthnicId = spnNation.getTag() != null ? spnNation.getTag().toString() : "";
        childProfileModel.ReligionId = spnReligion.getTag() != null ? spnReligion.getTag().toString() : "";
        childProfileModel.ProvinceId = spnProvince.getTag() != null ? spnProvince.getTag().toString() : "";
        childProfileModel.DistrictId = spnDistrict.getTag() != null ? spnDistrict.getTag().toString() : "";
        childProfileModel.WardId = spnWard.getTag() != null ? spnWard.getTag().toString() : "";
        childProfileModel.Address = spnVillage.getTag() != null ? spnVillage.getTag().toString() : "";
        childProfileModel.FullAddress = (!Utils.isEmpty(spnVillage.getText().toString()) ? (spnVillage.getText().toString() + " - ") : "") + spnWard.getText().toString()
                + " - " + spnDistrict.getText().toString() + " - " + spnProvince.getText().toString();
    }

    /***
     * Get thông tin Thông tin cụ thể về trẻ
     */
    private void GetInfoStep2() {
        childProfileModel.Name = txtNameChild.getText().toString();
        childProfileModel.NickName = txtNickName.getText().toString();
        int radioButtonID = rgGender.getCheckedRadioButtonId();
        RadioButton radioButton = (RadioButton) rgGender.findViewById(radioButtonID);
        if (radioButton != null) {
            childProfileModel.Gender = Integer.parseInt(radioButton.getTag().toString());
        }
        childProfileModel.DateOfBirth = txtDateOfBirth.getText().toString();
//        if (childProfileModel.LeaningStatus != null && childProfileModel.LeaningStatus.equals("15")) {
//            childProfileModel.ClassInfo = spnPrimarySchoolClass.getText().toString();
//        } else if (childProfileModel.LeaningStatus != null && childProfileModel.LeaningStatus.equals("16")) {
//            childProfileModel.ClassInfo = spnSecondarySchoolClass.getText().toString();
//        }
        childProfileModel.ClassInfo = txtClassName.getText().toString();
        childProfileModel.FavouriteSubjectModel.OtherValue = txtDiffrentSubject.getText().toString();
        childProfileModel.FavouriteSubjectModel.OtherValue2 = txtBestSubject.getText().toString();
        childProfileModel.LearningCapacityModel.OtherValue = txtAchievement.getText().toString();
        childProfileModel.HouseworkModel.OtherValue = txtWorkOther.getText().toString();
        childProfileModel.HealthModel.OtherValue = txtHealthOther.getText().toString();
        childProfileModel.PersonalityModel.OtherValue = txtPersonalityOther.getText().toString();
        childProfileModel.HobbyModel.OtherValue = txtHobbieOther.getText().toString();
        childProfileModel.DreamModel.OtherValue = txtDreamOther.getText().toString();
    }

    /***
     * Get Thông tin về các thành viên trong gia đình trẻ
     */
    private void GetInfoStep3() {
        childProfileModel.NotLivingWithParentModel.OtherValue = txtNotLiveParentOther.getText().toString();
        childProfileModel.LivingWithOtherModel.OtherValue = txtLiveWhoOther.getText().toString();
    }

    /***
     * Get Thông tin về điều kiện sống của gia đình
     */
    private void GetInfoStep4() {
        childProfileModel.HouseTypeModel.OtherValue = txtTypeHousingOther.getText().toString();
        childProfileModel.HouseRoofModel.OtherValue = txtRoofMaterialOther.getText().toString();
        childProfileModel.HouseWallModel.OtherValue = txtWallMaterialsOther.getText().toString();
        childProfileModel.HouseFloorModel.OtherValue = txtFloorMaterialsOther.getText().toString();


        childProfileModel.WaterSourceUseModel.OtherValue = txtWaterOther.getText().toString();
        childProfileModel.RoadConditionModel.OtherValue = txtRoadConditionsOther.getText().toString();
        childProfileModel.FamilyType = spnFamilyType.getText().toString();
        //childProfileModel.IncomeSources = txtIncomeSources.getText().toString();
    }

    /***
     * Get Thông tin cam kết gia đình
     */
    private void GetInfoStep5() {
        childProfileModel.ConsentName = txtConsentName.getText().toString();
        childProfileModel.ConsentRelationship = spnConsentRelationship.getText().toString();
        childProfileModel.ConsentVillage = txtConsentVillage.getText().toString();
        childProfileModel.ConsentWard = txtConsentWard.getText().toString();
    }

    /***
     *Hiển thị thông tin lên giao diện
     */
    private void viewProfileChild() {
        progressDialog.setVisibility(View.VISIBLE);
        AndroidNetworking.get(Utils.GetUrlApi("api/ChildProfiles/GetInfoChildProfile?id="))
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        progressDialog.setVisibility(View.GONE);
                        childProfileModel = new Gson().fromJson(response.toString(), new TypeToken<ChildProfileModel>() {
                        }.getType());
                        setInfoProfileChild();
                    }

                    @Override
                    public void onError(ANError anError) {
                        progressDialog.setVisibility(View.GONE);
                    }
                });
    }

    /***
     * Set thông tin lên giao diện
     */
    private void setInfoProfileChild() {
        try {
            //Step 1
            ((RadioButton) rgGender.getChildAt(0)).setChecked(true);

            //Step 2
            spnSecondarySchoolClass.setClickable(false);
            spnPrimarySchoolClass.setClickable(false);

            checkListFavouriteSubjectAdapter = new CheckListAdapter(getActivity(), childProfileModel.FavouriteSubjectModel.ListObject);
            recyclerViewSubject.setAdapter(checkListFavouriteSubjectAdapter);
            checkListFavouriteSubjectAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.FavouriteSubjectModel.ListObject.get(position).Check = checkBox.isChecked();
                }
            });

            checkListCapacityAdapter = new CheckListAdapter(getActivity(), childProfileModel.LearningCapacityModel.ListObject);
            recyclerLeaningCapacity.setAdapter(checkListCapacityAdapter);
            checkListCapacityAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.LearningCapacityModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.LearningCapacityModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListCapacityAdapter.notifyDataSetChanged();
                }
            });

            checkListHouseWorkAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseworkModel.ListObject);
            recyclerHouseWork.setAdapter(checkListHouseWorkAdapter);
            checkListHouseWorkAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.HouseworkModel.ListObject.get(position).Check = checkBox.isChecked();
                }
            });

            checkListHealthAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.HealthModel.ListObject, false);
            recyclerHealth.setAdapter(checkListHealthAdapter);
            checkListHealthAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel comboboxResult : childProfileModel.HealthModel.ListObject) {
                        comboboxResult.Check = false;
                    }
                    childProfileModel.HealthModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListHealthAdapter.notifyDataSetChanged();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.HealthModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                    if (obj.getTag().equals("Yes")) {
                        childProfileModel.HealthModel.ListObject.get(position).YesValue = true;
                        childProfileModel.HealthModel.ListObject.get(position).NoValue = false;
                    } else {
                        childProfileModel.HealthModel.ListObject.get(position).YesValue = false;
                        childProfileModel.HealthModel.ListObject.get(position).NoValue = true;
                    }
                }
            });

            checkListCharacteristicAdapter = new CheckListAdapter(getActivity(), childProfileModel.PersonalityModel.ListObject);
            recyclerCharacteristic.setAdapter(checkListCharacteristicAdapter);
            checkListCharacteristicAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.PersonalityModel.ListObject.get(position).Check = checkBox.isChecked();
                }
            });

            checkListHobbieAdapter = new CheckListAdapter(getActivity(), childProfileModel.HobbyModel.ListObject);
            recyclerHobbie.setAdapter(checkListHobbieAdapter);
            checkListHobbieAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.HobbyModel.ListObject.get(position).Check = checkBox.isChecked();
                }
            });

            checkListDreamAdapter = new CheckListAdapter(getActivity(), childProfileModel.DreamModel.ListObject);
            recyclerDream.setAdapter(checkListDreamAdapter);
            checkListDreamAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.DreamModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.DreamModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListDreamAdapter.notifyDataSetChanged();
                }
            });

            //Step 3
            childProfileModel.ListFamilyMember = new ArrayList<>();
            loadFamilyInfoAdapter();

            checkListLiveParentAdapter = new CheckListAdapter(getActivity(), childProfileModel.LivingWithParentModel.ListObject);
            recyclerLiveParent.setAdapter(checkListLiveParentAdapter);
            checkListLiveParentAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.LivingWithParentModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    checkListLiveParentAdapter.notifyDataSetChanged();
                    //Nếu check có sống cùng cả bố và mẹ
                    if (obj.Id.equals("01") && checkBox.isChecked()) {
                        boolean isOk = true;
                        for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_FATHER) && item.LiveWithChild == 0) {
//                                Toast.makeText(getActivity(), "Trong danh sách gia đình Bố/Father không sống cùng trẻ", Toast.LENGTH_LONG).show();
                                isOk = false;
                            }
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_MOTHER) && item.LiveWithChild == 0) {
//                                Toast.makeText(getActivity(), "Trong danh sách gia đình Mẹ/Mother không sống cùng trẻ", Toast.LENGTH_LONG).show();
                                isOk = false;
                            }
                        }
                        if (isOk) {
                            childProfileModel.LivingWithParentModel.ListObject.get(position).Check = checkBox.isChecked();
                            checkListLiveParentAdapter.notifyDataSetChanged();
                            if (checkBox.isChecked()) {
                                for (ObjectInputModel objectInputModel : childProfileModel.NotLivingWithParentModel.ListObject) {
                                    objectInputModel.Check = false;
                                }
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                            }
                        } else {
                            checkBox.setChecked(false);
                        }
                    } else if (obj.Id.equals("04") && checkBox.isChecked()) {
                        int isOk = 0;
                        for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_FATHER) && item.LiveWithChild == 1) {
                                Toast.makeText(getActivity(), "Trong danh sách gia đình Bố/Father sống cùng trẻ", Toast.LENGTH_LONG).show();
                                isOk++;
                            }
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_MOTHER) && item.LiveWithChild == 1) {
                                Toast.makeText(getActivity(), "Trong danh sách gia đình Mẹ/Mother sống cùng trẻ", Toast.LENGTH_LONG).show();
                                isOk++;
                            }
                        }
                        if (isOk != 2) {
                            childProfileModel.LivingWithParentModel.ListObject.get(position).Check = checkBox.isChecked();
                            checkListLiveParentAdapter.notifyDataSetChanged();
                            if (checkBox.isChecked()) {
                                for (ObjectInputModel objectInputModel : childProfileModel.NotLivingWithParentModel.ListObject) {
                                    objectInputModel.Check = false;
                                }
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                            }
                        } else {
                            checkBox.setChecked(false);
                        }
                    }

                    for (ObjectInputModel objectInputModel : childProfileModel.NotLivingWithParentModel.ListObject) {
                        objectInputModel.Check = false;
                        objectInputModel.Enabled = false;
                    }
                    checkListNotLiveParentAdapter.notifyDataSetChanged();
                    isParentDead = false;
                    isFatherDead = false;
                    isMotherDead = false;
                }
            });

            checkListNotLiveParentAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.NotLivingWithParentModel.ListObject, true);
            for (ObjectInputModel item : childProfileModel.NotLivingWithParentModel.ListObject) {
                if (item.Check) {
                    setValueEnabled(childProfileModel.NotLivingWithParentModel.ListObject.indexOf(item));
                }
            }
            recyclerNotLiveParent.setAdapter(checkListNotLiveParentAdapter);
            checkListNotLiveParentAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    try {
                        for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_FATHER) && item.LiveWithChild == 1) {
                                if (position == 0 || position == 1 || position == 3 || position == 8){
                                    checkListNotLiveParentAdapter.notifyDataSetChanged();
                                    return;
                                }
                            }
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_MOTHER) && item.LiveWithChild == 1) {
                                if (position == 0 || position == 2 || position == 4 || position == 9){
                                    checkListNotLiveParentAdapter.notifyDataSetChanged();
                                    return;
                                }
                            }
                        }
                        //Trường hợp chọn sống cùng với bố mẹ đẻ thì không cho chọn mục này nuwax
                        if (childProfileModel.LivingWithParentModel.ListObject.get(0).Check) {
                            checkListNotLiveParentAdapter.notifyDataSetChanged();
                            return;
                        } else {
                            CheckBox checkBox = (CheckBox) view;

                            //Check ko cho click
                            if (childProfileModel.NotLivingWithParentModel.ListObject.get(0).Check && checkBox.isChecked()) {
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                                setValueEnabled(position);
                                return;
                            } else if (childProfileModel.NotLivingWithParentModel.ListObject.get(1).Check && checkBox.isChecked()
                                    && (position == 0 || position == 3 || position == 5 || position == 6 || position == 7 || position == 8)) {
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                                setValueEnabled(position);
                                return;
                            } else if (childProfileModel.NotLivingWithParentModel.ListObject.get(2).Check && checkBox.isChecked()
                                    && (position == 0 || position == 4 || position == 5 || position == 6 || position == 7 || position == 9)) {
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                                setValueEnabled(position);
                                return;
                            } else if (((childProfileModel.NotLivingWithParentModel.ListObject.get(3).Check || childProfileModel.NotLivingWithParentModel.ListObject.get(8).Check) && checkBox.isChecked())
                                    && (position == 0 || position == 1)) {
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                                setValueEnabled(position);
                                return;
                            } else if (((childProfileModel.NotLivingWithParentModel.ListObject.get(4).Check || childProfileModel.NotLivingWithParentModel.ListObject.get(9).Check) && checkBox.isChecked())
                                    && (position == 0 || position == 2)) {
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                                setValueEnabled(position);
                                return;
                            } else if (((childProfileModel.NotLivingWithParentModel.ListObject.get(5).Check || childProfileModel.NotLivingWithParentModel.ListObject.get(6).Check || childProfileModel.NotLivingWithParentModel.ListObject.get(7).Check) && checkBox.isChecked())
                                    && (position == 0 || position == 1 || position == 2)) {
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                                setValueEnabled(position);
                                return;
                            }

                            childProfileModel.NotLivingWithParentModel.ListObject.get(position).Check = checkBox.isChecked();
                            checkListNotLiveParentAdapter.notifyDataSetChanged();
                            setValueEnabled(position);
                        }
                    } catch (Exception ex) {
                    }
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.NotLivingWithParentModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                }
            });

            checkListLiveWhoAdapter = new CheckListAdapter(getActivity(), childProfileModel.LivingWithOtherModel.ListObject);
            recyclerLiveWho.setAdapter(checkListLiveWhoAdapter);
            checkListLiveWhoAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.LivingWithOtherModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.LivingWithOtherModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListLiveWhoAdapter.notifyDataSetChanged();
                }
            });

            //Step 4
            checkListTypeHousingAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseTypeModel.ListObject);
            recyclerTypeHousing.setAdapter(checkListTypeHousingAdapter);
            checkListTypeHousingAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.HouseTypeModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.HouseTypeModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListTypeHousingAdapter.notifyDataSetChanged();
                }
            });

            checkListRoofMaterialAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseRoofModel.ListObject);
            recyclerRoofMaterial.setAdapter(checkListRoofMaterialAdapter);
            checkListRoofMaterialAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.HouseRoofModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.HouseRoofModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListRoofMaterialAdapter.notifyDataSetChanged();
                }
            });

            checkListWallMaterialsAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseWallModel.ListObject);
            recyclerWallMaterials.setAdapter(checkListWallMaterialsAdapter);
            checkListWallMaterialsAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.HouseWallModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.HouseWallModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListWallMaterialsAdapter.notifyDataSetChanged();
                }
            });

            checkListFloorMaterialsAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseFloorModel.ListObject);
            recyclerFloorMaterials.setAdapter(checkListFloorMaterialsAdapter);
            checkListFloorMaterialsAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.HouseFloorModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.HouseFloorModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListFloorMaterialsAdapter.notifyDataSetChanged();
                }
            });

            checkListIsElectricityAdapter = new CheckListAdapter(getActivity(), childProfileModel.UseElectricityModel.ListObject);
            recyclerIsElectricity.setAdapter(checkListIsElectricityAdapter);
            checkListIsElectricityAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.UseElectricityModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.UseElectricityModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListIsElectricityAdapter.notifyDataSetChanged();
                }
            });

            checkListSchoolMetAdapter = new CheckListAdapter(getActivity(), childProfileModel.SchoolDistanceModel.ListObject);
            recyclerSchoolMet.setAdapter(checkListSchoolMetAdapter);
            checkListSchoolMetAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.SchoolDistanceModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.SchoolDistanceModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListSchoolMetAdapter.notifyDataSetChanged();
                }
            });

            checkListClinicsMetAdapter = new CheckListAdapter(getActivity(), childProfileModel.ClinicDistanceModel.ListObject);
            recyclerClinicsMet.setAdapter(checkListClinicsMetAdapter);
            checkListClinicsMetAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.ClinicDistanceModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.ClinicDistanceModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListClinicsMetAdapter.notifyDataSetChanged();
                }
            });

            checkListWaterSourceMetAdapter = new CheckListAdapter(getActivity(), childProfileModel.WaterSourceDistanceModel.ListObject);
            recyclerWaterSourceMet.setAdapter(checkListWaterSourceMetAdapter);
            checkListWaterSourceMetAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.WaterSourceDistanceModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.WaterSourceDistanceModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListWaterSourceMetAdapter.notifyDataSetChanged();
                }
            });

            checkListWaterFromAdapter = new CheckListAdapter(getActivity(), childProfileModel.WaterSourceUseModel.ListObject);
            recyclerWaterFrom.setAdapter(checkListWaterFromAdapter);
            checkListWaterFromAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.WaterSourceUseModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.WaterSourceUseModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListWaterFromAdapter.notifyDataSetChanged();
                }
            });

            checkListRoadConditionsAdapter = new CheckListAdapter(getActivity(), childProfileModel.RoadConditionModel.ListObject);
            recyclerRoadConditions.setAdapter(checkListRoadConditionsAdapter);
            checkListRoadConditionsAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.RoadConditionModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.RoadConditionModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListRoadConditionsAdapter.notifyDataSetChanged();
                }
            });

//            textListIncomeFamilyAdapter = new TextInputListAdapter(getActivity(), childProfileModel.IncomeFamilyModel.ListObject);
//            recyclerIncomeFamily.setAdapter(textListIncomeFamilyAdapter);
//            textListIncomeFamilyAdapter.SetOnItemClickListener(new TextInputListAdapter.OnItemClickListener() {
//                @Override
//                public void onTextChange(int position, String content) {
//                    childProfileModel.IncomeFamilyModel.ListObject.get(position).Value = content;
//                }
//            });

            checkListIncomeOtherAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.IncomeOtherModel.ListObject, false);
            recyclerIncomeOther.setAdapter(checkListIncomeOtherAdapter);
            checkListIncomeOtherAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.IncomeOtherModel.ListObject.get(position).Check = checkBox.isChecked();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.IncomeOtherModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                }
            });

//            textListHarvestOutputAdapter = new TextInputListAdapter(getActivity(), childProfileModel.HarvestOutputModel.ListObject);
//            recyclerHarvestOutput.setAdapter(textListHarvestOutputAdapter);
//            textListHarvestOutputAdapter.SetOnItemClickListener(new TextInputListAdapter.OnItemClickListener() {
//                @Override
//                public void onTextChange(int position, String content) {
//                    childProfileModel.HarvestOutputModel.ListObject.get(position).Value = content;
//                }
//            });

            checkListHarvestOutput = new CheckInputListAdapter(getActivity(), childProfileModel.HarvestOutputModel.ListObject, false);
            recyclerHarvestOutput.setAdapter(checkListHarvestOutput);
            checkListHarvestOutput.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel comboboxResult : childProfileModel.HarvestOutputModel.ListObject) {
                        comboboxResult.Check = false;
                    }
                    childProfileModel.HarvestOutputModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListHealthAdapter.notifyDataSetChanged();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.HarvestOutputModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                    if (obj.getTag().equals("Yes")) {
                        childProfileModel.HarvestOutputModel.ListObject.get(position).YesValue = true;
                        childProfileModel.HarvestOutputModel.ListObject.get(position).NoValue = false;
                    } else {
                        childProfileModel.HarvestOutputModel.ListObject.get(position).YesValue = false;
                        childProfileModel.HarvestOutputModel.ListObject.get(position).NoValue = true;
                    }
                }
            });

            checkListNumberPetAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.NumberPetModel.ListObject, false);
            recyclerNumberPet.setAdapter(checkListNumberPetAdapter);
            checkListNumberPetAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel comboboxResult : childProfileModel.NumberPetModel.ListObject) {
                        comboboxResult.Check = false;
                    }
                    childProfileModel.NumberPetModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListHealthAdapter.notifyDataSetChanged();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.NumberPetModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                    if (obj.getTag().equals("Yes")) {
                        childProfileModel.NumberPetModel.ListObject.get(position).YesValue = true;
                        childProfileModel.NumberPetModel.ListObject.get(position).NoValue = false;
                    } else {
                        childProfileModel.NumberPetModel.ListObject.get(position).YesValue = false;
                        childProfileModel.NumberPetModel.ListObject.get(position).NoValue = true;
                    }
                }
            });

//            textListNumberPetAdapter = new TextInputListAdapter(getActivity(), childProfileModel.NumberPetModel.ListObject);
//            recyclerNumberPet.setAdapter(textListNumberPetAdapter);
//            textListNumberPetAdapter.SetOnItemClickListener(new TextInputListAdapter.OnItemClickListener() {
//                @Override
//                public void onTextChange(int position, String content) {
//                    childProfileModel.NumberPetModel.ListObject.get(position).Value = content;
//                }
//            });

            //Step 5
            checkListSiblingsJoiningChildFundAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.SiblingsJoiningChildFundModel.ListObject, false);
            recyclerSiblingsJoiningChildFund.setAdapter(checkListSiblingsJoiningChildFundAdapter);
            checkListSiblingsJoiningChildFundAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.SiblingsJoiningChildFundModel.ListObject.get(position).Check = checkBox.isChecked();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.SiblingsJoiningChildFundModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                }
            });

        } catch (Exception ex) {
            String a = "";
        }
    }

    private void setValueEnabled(int position) {

        if (position == 0) {
            if (!(this.isFatherDead || this.isMotherDead)) {
                this.isParentDead = !this.isParentDead;
                this.isFatherDead = false;
                this.isMotherDead = false;
            }
        }

        if (position == 1 && !this.isParentDead) {
            this.isFatherDead = !this.isFatherDead;
        }

        if (position == 2 && !this.isParentDead) {
            this.isMotherDead = !this.isMotherDead;
        }

        if (this.isParentDead) {
            for (int i = 1; i < childProfileModel.NotLivingWithParentModel.ListObject.size(); i++) {
                childProfileModel.NotLivingWithParentModel.ListObject.get(i).Enabled = true;
            }
        } else {
            for (ObjectInputModel objectInputModel : childProfileModel.NotLivingWithParentModel.ListObject) {
                objectInputModel.Enabled = false;
            }
        }

        if (this.isFatherDead) {
            int index = 0;
            for (ObjectInputModel objectInputModel : childProfileModel.NotLivingWithParentModel.ListObject) {
                if ((index == 0 || index == 3 || index == 5 || index == 6 || index == 7 || index == 8)) {
                    objectInputModel.Enabled = true;
                }
                index++;
            }
        }

        if (this.isMotherDead) {
            int index = 0;
            for (ObjectInputModel objectInputModel : childProfileModel.NotLivingWithParentModel.ListObject) {
                if ((index == 0 || index == 4 || index == 5 || index == 6 || index == 7 || index == 9)) {
                    objectInputModel.Enabled = true;
                }
                index++;
            }
        }
        checkListNotLiveParentAdapter.notifyDataSetChanged();
    }

    /***
     * Set thông tin nhấp lên giao diện
     */
    private void setDataProfileDraft() {
        //Step 1
        try {
            if (childProfileModel.ImagePath != null && !childProfileModel.ImagePath.isEmpty()) {
                if (childProfileModel.ImagePath.contains("http")) {
                    Picasso.with(getContext()).load(childProfileModel.ImagePath).into(imageAvata);
                } else {
                    File file = new File(childProfileModel.ImagePath);
                    Uri uri = Uri.fromFile(file);
                    imageAvata.setImageURI(uri);
                }

            } else {
                Picasso.with(this.getContext()).load(R.drawable.ic_people).resize(300, 300)
                        .centerInside().into(imageAvata);
            }
        } catch (Exception ex) {
        }

        try {
            txtEmployeeName.setText(childProfileModel.EmployeeName);
            spnEmployeeTitle.setTag(childProfileModel.EmployeeTitle);
            spnEmployeeTitle.setText(Utils.getNameById(listEmployeeTitle, childProfileModel.EmployeeTitle));
//            spnInfoDate.setText(childProfileModel.InfoDate);
            spnInfoDate.setText(DateUtils.ConvertYMDServerToDMY(childProfileModel.InfoDate));
            spnProgramCode.setText(childProfileModel.ProgramCode);
            txtChildCode.setText(childProfileModel.ChildCode);
            setDataSchool(childProfileModel.WardId, childProfileModel.SchoolId);
            if (childProfileModel.SchoolId.equals("Other")) {
                lySchoolOtherName.setVisibility(View.VISIBLE);
            } else {
                lySchoolOtherName.setVisibility(View.GONE);
            }
            txtSchoolOtherName.setText(childProfileModel.SchoolOtherName);
            spnNation.setTag(childProfileModel.EthnicId);
            spnNation.setText(Utils.getNameById(listNation, childProfileModel.EthnicId));
            spnReligion.setTag(childProfileModel.ReligionId);
            spnReligion.setText(Utils.getNameById(listGeligion, childProfileModel.ReligionId));
            spnVillage.setTag(childProfileModel.Address);
            setDataProvince(childProfileModel.ProvinceId);
            setDataDistrict(childProfileModel.DistrictId);
            setDataWard(childProfileModel.WardId);
            setDataVillage(childProfileModel.WardId, childProfileModel.Address);
            if (childProfileModel.Gender == Constants.Male) {
                ((RadioButton) rgGender.getChildAt(0)).setChecked(true);
            } else if (childProfileModel.Gender == Constants.FeMale) {
                ((RadioButton) rgGender.getChildAt(1)).setChecked(true);
            }

            cbHandicap.setChecked(childProfileModel.Handicap);
            switch (childProfileModel.LeaningStatus) {
                case "11": {
                    cbChildhood.setChecked(true);
                    spnPrimarySchoolClass.setText("");
                    spnSecondarySchoolClass.setText("");
                    spnSecondarySchoolClass.setClickable(false);
                    spnPrimarySchoolClass.setClickable(false);
                    break;
                }
                case "12": {
                    cbOutSchool.setChecked(true);
                    spnPrimarySchoolClass.setText("");
                    spnSecondarySchoolClass.setText("");
                    spnSecondarySchoolClass.setClickable(false);
                    spnPrimarySchoolClass.setClickable(false);
                    break;
                }
//                case "13": {
//                    cbHandicap.setChecked(true);
//                    spnPrimarySchoolClass.setText("");
//                    spnSecondarySchoolClass.setText("");
//                    spnSecondarySchoolClass.setClickable(false);
//                    spnPrimarySchoolClass.setClickable(false);
//                    break;
//                }
                case "14": {
                    cbPreschool.setChecked(true);
                    spnPrimarySchoolClass.setText("");
                    spnSecondarySchoolClass.setText("");
                    spnSecondarySchoolClass.setClickable(false);
                    spnPrimarySchoolClass.setClickable(false);
                    break;
                }
                case "15": {
                    isSetCheck = true;
                    cbPrimarySchool.setChecked(true);
                    spnSecondarySchoolClass.setClickable(false);
                    spnPrimarySchoolClass.setClickable(true);
                    txtClassName.setText(childProfileModel.ClassInfo);
                    break;
                }
                case "16": {
                    isSetCheck = true;
                    cbSecondarySchool.setChecked(true);
                    spnSecondarySchoolClass.setClickable(true);
                    spnPrimarySchoolClass.setClickable(false);
                    txtClassName.setText(childProfileModel.ClassInfo);

                    break;
                }
                case "17": {
                    isSetCheck = true;
                    spnSecondarySchoolClass.setClickable(false);
                    spnPrimarySchoolClass.setClickable(false);
                    cbHighSchool.setChecked(true);
                    txtClassName.setText(childProfileModel.ClassInfo);
                    break;
                }
            }

            //Step 2
            txtNameChild.setText(childProfileModel.Name);
            txtNickName.setText(childProfileModel.NickName);
//            txtDateOfBirth.setText(childProfileModel.DateOfBirth);
            txtDateOfBirth.setText(DateUtils.ConvertYMDServerToDMY(childProfileModel.DateOfBirth));
            txtDiffrentSubject.setText(childProfileModel.FavouriteSubjectModel.OtherValue);
            txtBestSubject.setText(childProfileModel.FavouriteSubjectModel.OtherValue2);
            txtAchievement.setText(childProfileModel.LearningCapacityModel.OtherValue);
            txtWorkOther.setText(childProfileModel.HouseworkModel.OtherValue);
            txtHealthOther.setText(childProfileModel.HealthModel.OtherValue);
            txtPersonalityOther.setText(childProfileModel.PersonalityModel.OtherValue);
            txtHobbieOther.setText(childProfileModel.HobbyModel.OtherValue);
            txtDreamOther.setText(childProfileModel.DreamModel.OtherValue);

            checkListFavouriteSubjectAdapter = new CheckListAdapter(getActivity(), childProfileModel.FavouriteSubjectModel.ListObject);
            recyclerViewSubject.setAdapter(checkListFavouriteSubjectAdapter);
            checkListFavouriteSubjectAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.FavouriteSubjectModel.ListObject.get(position).Check = checkBox.isChecked();
                }
            });

            checkListCapacityAdapter = new CheckListAdapter(getActivity(), childProfileModel.LearningCapacityModel.ListObject);
            recyclerLeaningCapacity.setAdapter(checkListCapacityAdapter);
            checkListCapacityAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.LearningCapacityModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.LearningCapacityModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListCapacityAdapter.notifyDataSetChanged();
                }
            });

            checkListHouseWorkAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseworkModel.ListObject);
            recyclerHouseWork.setAdapter(checkListHouseWorkAdapter);
            checkListHouseWorkAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.HouseworkModel.ListObject.get(position).Check = checkBox.isChecked();
                }
            });

            checkListHealthAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.HealthModel.ListObject, false);
            recyclerHealth.setAdapter(checkListHealthAdapter);
            checkListHealthAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel comboboxResult : childProfileModel.HealthModel.ListObject) {
                        comboboxResult.Check = false;
                    }
                    childProfileModel.HealthModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListHealthAdapter.notifyDataSetChanged();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.HealthModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                }
            });

            checkListCharacteristicAdapter = new CheckListAdapter(getActivity(), childProfileModel.PersonalityModel.ListObject);
            recyclerCharacteristic.setAdapter(checkListCharacteristicAdapter);
            checkListCharacteristicAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.PersonalityModel.ListObject.get(position).Check = checkBox.isChecked();
                }
            });

            checkListHobbieAdapter = new CheckListAdapter(getActivity(), childProfileModel.HobbyModel.ListObject);
            recyclerHobbie.setAdapter(checkListHobbieAdapter);
            checkListHobbieAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.HobbyModel.ListObject.get(position).Check = checkBox.isChecked();
                }
            });

            checkListDreamAdapter = new CheckListAdapter(getActivity(), childProfileModel.DreamModel.ListObject);
            recyclerDream.setAdapter(checkListDreamAdapter);
            checkListDreamAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.DreamModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.DreamModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListDreamAdapter.notifyDataSetChanged();
                }
            });

            //Step 3
            if (childProfileModel.ListFamilyMember == null) {
                childProfileModel.ListFamilyMember = new ArrayList<>();
            } else {
                for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                    if (item.DateOfBirth != null && !item.DateOfBirth.isEmpty()) {
                        item.DateOfBirthView = DateUtils.ConvertYMDServerToDMY(item.DateOfBirth);
                    }
                    item.RelationshipName = Utils.getNameById(listRelationship, item.RelationshipId);
                    item.JobName = Utils.getNameById(listJob, item.Job);
                }
                countSisterAndBrother();
            }
            loadFamilyInfoAdapter();

            txtNotLiveParentOther.setText(childProfileModel.NotLivingWithParentModel.OtherValue);
            txtLiveWhoOther.setText(childProfileModel.LivingWithOtherModel.OtherValue);

            checkListLiveParentAdapter = new CheckListAdapter(getActivity(), childProfileModel.LivingWithParentModel.ListObject);
            recyclerLiveParent.setAdapter(checkListLiveParentAdapter);
            checkListLiveParentAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.LivingWithParentModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    checkListLiveParentAdapter.notifyDataSetChanged();
                    //Nếu check có sống cùng cả bố và mẹ
                    if (obj.Id.equals("01") && checkBox.isChecked()) {
                        boolean isOk = true;
                        for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_FATHER) && item.LiveWithChild == 0) {
//                                Toast.makeText(getActivity(), "Trong danh sách gia đình Bố/Father không sống cùng trẻ", Toast.LENGTH_LONG).show();
                                isOk = false;
                            }
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_MOTHER) && item.LiveWithChild == 0) {
//                                Toast.makeText(getActivity(), "Trong danh sách gia đình Mẹ/Mother không sống cùng trẻ", Toast.LENGTH_LONG).show();
                                isOk = false;
                            }
                        }
                        if (isOk) {
                            childProfileModel.LivingWithParentModel.ListObject.get(position).Check = checkBox.isChecked();
                            checkListLiveParentAdapter.notifyDataSetChanged();
                            if (checkBox.isChecked()) {
                                for (ObjectInputModel objectInputModel : childProfileModel.NotLivingWithParentModel.ListObject) {
                                    objectInputModel.Check = false;
                                }
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                            }
                        } else {
                            checkBox.setChecked(false);
                        }
                    } else if (obj.Id.equals("04") && checkBox.isChecked()) {
                        int isOk = 0;
                        for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_FATHER) && item.LiveWithChild == 1) {
                                Toast.makeText(getActivity(), "Trong danh sách gia đình Bố/Father sống cùng trẻ", Toast.LENGTH_LONG).show();
                                isOk++;
                            }
                            if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_MOTHER) && item.LiveWithChild == 1) {
                                Toast.makeText(getActivity(), "Trong danh sách gia đình Mẹ/Mother sống cùng trẻ", Toast.LENGTH_LONG).show();
                                isOk++;
                            }
                        }
                        if (isOk != 2) {
                            childProfileModel.LivingWithParentModel.ListObject.get(position).Check = checkBox.isChecked();
                            checkListLiveParentAdapter.notifyDataSetChanged();
                            if (checkBox.isChecked()) {
                                for (ObjectInputModel objectInputModel : childProfileModel.NotLivingWithParentModel.ListObject) {
                                    objectInputModel.Check = false;
                                }
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                            }
                        } else {
                            checkBox.setChecked(false);
                        }
                    }
                }
            });

            checkListNotLiveParentAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.NotLivingWithParentModel.ListObject, true);
            recyclerNotLiveParent.setAdapter(checkListNotLiveParentAdapter);
            checkListNotLiveParentAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                        if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_FATHER) && item.LiveWithChild == 1) {
                            if (position == 0 || position == 1 || position == 3 || position == 8){
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                                return;
                            }
                        }
                        if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_MOTHER) && item.LiveWithChild == 1) {
                            if (position == 0 || position == 2 || position == 4 || position == 9){
                                checkListNotLiveParentAdapter.notifyDataSetChanged();
                                return;
                            }
                        }
                    }
                    if (childProfileModel.LivingWithParentModel.ListObject.get(0).Check) {
                        checkListNotLiveParentAdapter.notifyDataSetChanged();
                        return;
                    } else {
                        CheckBox checkBox = (CheckBox) view;
                        //Check ko cho click
                        if (childProfileModel.NotLivingWithParentModel.ListObject.get(0).Check && checkBox.isChecked()) {
                            checkListNotLiveParentAdapter.notifyDataSetChanged();
                            setValueEnabled(position);
                            return;
                        } else if (childProfileModel.NotLivingWithParentModel.ListObject.get(1).Check && checkBox.isChecked()
                                && (position == 0 || position == 3 || position == 5 || position == 6 || position == 7 || position == 8)) {
                            checkListNotLiveParentAdapter.notifyDataSetChanged();
                            setValueEnabled(position);
                            return;
                        } else if (childProfileModel.NotLivingWithParentModel.ListObject.get(2).Check && checkBox.isChecked()
                                && (position == 0 || position == 4 || position == 5 || position == 6 || position == 7 || position == 9)) {
                            checkListNotLiveParentAdapter.notifyDataSetChanged();
                            setValueEnabled(position);
                            return;
                        } else if (((childProfileModel.NotLivingWithParentModel.ListObject.get(3).Check || childProfileModel.NotLivingWithParentModel.ListObject.get(8).Check) && checkBox.isChecked())
                                && (position == 0 || position == 1)) {
                            checkListNotLiveParentAdapter.notifyDataSetChanged();
                            setValueEnabled(position);
                            return;
                        } else if (((childProfileModel.NotLivingWithParentModel.ListObject.get(4).Check || childProfileModel.NotLivingWithParentModel.ListObject.get(9).Check) && checkBox.isChecked())
                                && (position == 0 || position == 2)) {
                            checkListNotLiveParentAdapter.notifyDataSetChanged();
                            setValueEnabled(position);
                            return;
                        } else if (((childProfileModel.NotLivingWithParentModel.ListObject.get(5).Check || childProfileModel.NotLivingWithParentModel.ListObject.get(6).Check || childProfileModel.NotLivingWithParentModel.ListObject.get(7).Check) && checkBox.isChecked())
                                && (position == 0 || position == 1 || position == 2)) {
                            checkListNotLiveParentAdapter.notifyDataSetChanged();
                            setValueEnabled(position);
                            return;
                        }

                        childProfileModel.NotLivingWithParentModel.ListObject.get(position).Check = checkBox.isChecked();
                        checkListNotLiveParentAdapter.notifyDataSetChanged();
                        setValueEnabled(position);

                    }
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.NotLivingWithParentModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                }
            });

            checkListLiveWhoAdapter = new CheckListAdapter(getActivity(), childProfileModel.LivingWithOtherModel.ListObject);
            recyclerLiveWho.setAdapter(checkListLiveWhoAdapter);
            checkListLiveWhoAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.LivingWithOtherModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.LivingWithOtherModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListLiveWhoAdapter.notifyDataSetChanged();
                }
            });

            //Step 4
            txtTypeHousingOther.setText(childProfileModel.HouseTypeModel.OtherValue);
            txtRoofMaterialOther.setText(childProfileModel.HouseRoofModel.OtherValue);
            txtWallMaterialsOther.setText(childProfileModel.HouseWallModel.OtherValue);
            txtFloorMaterialsOther.setText(childProfileModel.HouseFloorModel.OtherValue);

            txtWaterOther.setText(childProfileModel.WaterSourceUseModel.OtherValue);
            txtRoadConditionsOther.setText(childProfileModel.RoadConditionModel.OtherValue);
            spnFamilyType.setText(childProfileModel.FamilyType);
            //txtIncomeSources.setText(childProfileModel.IncomeSources);

            checkListTypeHousingAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseTypeModel.ListObject);
            recyclerTypeHousing.setAdapter(checkListTypeHousingAdapter);
            checkListTypeHousingAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.HouseTypeModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.HouseTypeModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListTypeHousingAdapter.notifyDataSetChanged();
                }
            });

            checkListRoofMaterialAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseRoofModel.ListObject);
            recyclerRoofMaterial.setAdapter(checkListRoofMaterialAdapter);
            checkListRoofMaterialAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.HouseRoofModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.HouseRoofModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListRoofMaterialAdapter.notifyDataSetChanged();
                }
            });

            checkListWallMaterialsAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseWallModel.ListObject);
            recyclerWallMaterials.setAdapter(checkListWallMaterialsAdapter);
            checkListWallMaterialsAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.HouseWallModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.HouseWallModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListWallMaterialsAdapter.notifyDataSetChanged();
                }
            });

            checkListFloorMaterialsAdapter = new CheckListAdapter(getActivity(), childProfileModel.HouseFloorModel.ListObject);
            recyclerFloorMaterials.setAdapter(checkListFloorMaterialsAdapter);
            checkListFloorMaterialsAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.HouseFloorModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.HouseFloorModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListFloorMaterialsAdapter.notifyDataSetChanged();
                }
            });

            checkListIsElectricityAdapter = new CheckListAdapter(getActivity(), childProfileModel.UseElectricityModel.ListObject);
            recyclerIsElectricity.setAdapter(checkListIsElectricityAdapter);
            checkListIsElectricityAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.UseElectricityModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.UseElectricityModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListIsElectricityAdapter.notifyDataSetChanged();
                }
            });

            checkListSchoolMetAdapter = new CheckListAdapter(getActivity(), childProfileModel.SchoolDistanceModel.ListObject);
            recyclerSchoolMet.setAdapter(checkListSchoolMetAdapter);
            checkListSchoolMetAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.SchoolDistanceModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.SchoolDistanceModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListSchoolMetAdapter.notifyDataSetChanged();
                }
            });

            checkListClinicsMetAdapter = new CheckListAdapter(getActivity(), childProfileModel.ClinicDistanceModel.ListObject);
            recyclerClinicsMet.setAdapter(checkListClinicsMetAdapter);
            checkListClinicsMetAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.ClinicDistanceModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.ClinicDistanceModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListClinicsMetAdapter.notifyDataSetChanged();
                }
            });

            checkListWaterSourceMetAdapter = new CheckListAdapter(getActivity(), childProfileModel.WaterSourceDistanceModel.ListObject);
            recyclerWaterSourceMet.setAdapter(checkListWaterSourceMetAdapter);
            checkListWaterSourceMetAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.WaterSourceDistanceModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.WaterSourceDistanceModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListWaterSourceMetAdapter.notifyDataSetChanged();
                }
            });

            checkListWaterFromAdapter = new CheckListAdapter(getActivity(), childProfileModel.WaterSourceUseModel.ListObject);
            recyclerWaterFrom.setAdapter(checkListWaterFromAdapter);
            checkListWaterFromAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.WaterSourceUseModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.WaterSourceUseModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListWaterFromAdapter.notifyDataSetChanged();
                }
            });

            checkListRoadConditionsAdapter = new CheckListAdapter(getActivity(), childProfileModel.RoadConditionModel.ListObject);
            recyclerRoadConditions.setAdapter(checkListRoadConditionsAdapter);
            checkListRoadConditionsAdapter.SetOnItemClickListener(new CheckListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.RoadConditionModel.ListObject) {
                        objectInputModel.Check = false;
                    }
                    childProfileModel.RoadConditionModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListRoadConditionsAdapter.notifyDataSetChanged();
                }
            });

//            textListIncomeFamilyAdapter = new TextInputListAdapter(getActivity(), childProfileModel.IncomeFamilyModel.ListObject);
//            recyclerIncomeFamily.setAdapter(textListIncomeFamilyAdapter);
//            textListIncomeFamilyAdapter.SetOnItemClickListener(new TextInputListAdapter.OnItemClickListener() {
//                @Override
//                public void onTextChange(int position, String content) {
//                    childProfileModel.IncomeFamilyModel.ListObject.get(position).Value = content;
//                }
//            });

            checkListIncomeOtherAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.IncomeOtherModel.ListObject, false);
            recyclerIncomeOther.setAdapter(checkListIncomeOtherAdapter);
            checkListIncomeOtherAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    childProfileModel.IncomeOtherModel.ListObject.get(position).Check = checkBox.isChecked();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.IncomeOtherModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                }
            });

//            textListHarvestOutputAdapter = new TextInputListAdapter(getActivity(), childProfileModel.HarvestOutputModel.ListObject);
//            recyclerHarvestOutput.setAdapter(textListHarvestOutputAdapter);
//            textListHarvestOutputAdapter.SetOnItemClickListener(new TextInputListAdapter.OnItemClickListener() {
//                @Override
//                public void onTextChange(int position, String content) {
//                    childProfileModel.HarvestOutputModel.ListObject.get(position).Value = content;
//                }
//            });
            checkListHarvestOutput = new CheckInputListAdapter(getActivity(), childProfileModel.HarvestOutputModel.ListObject, false);
            recyclerHarvestOutput.setAdapter(checkListHarvestOutput);
            checkListHarvestOutput.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel comboboxResult : childProfileModel.HarvestOutputModel.ListObject) {
                        comboboxResult.Check = false;
                    }
                    childProfileModel.HarvestOutputModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListHealthAdapter.notifyDataSetChanged();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.HarvestOutputModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                }
            });

//            textListNumberPetAdapter = new TextInputListAdapter(getActivity(), childProfileModel.NumberPetModel.ListObject);
//            recyclerNumberPet.setAdapter(textListNumberPetAdapter);
//            textListNumberPetAdapter.SetOnItemClickListener(new TextInputListAdapter.OnItemClickListener() {
//                @Override
//                public void onTextChange(int position, String content) {
//                    childProfileModel.NumberPetModel.ListObject.get(position).Value = content;
//                }
//            });

            checkListNumberPetAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.NumberPetModel.ListObject, false);
            recyclerHarvestOutput.setAdapter(checkListNumberPetAdapter);
            checkListNumberPetAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel comboboxResult : childProfileModel.NumberPetModel.ListObject) {
                        comboboxResult.Check = false;
                    }
                    childProfileModel.NumberPetModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListHealthAdapter.notifyDataSetChanged();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.NumberPetModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                }
            });

            //Step 5
            txtConsentName.setText(childProfileModel.ConsentName);
            spnConsentRelationship.setText(childProfileModel.ConsentRelationship);
            txtConsentVillage.setText(childProfileModel.ConsentVillage);
            txtConsentWard.setText(childProfileModel.ConsentWard);

            checkListSiblingsJoiningChildFundAdapter = new CheckInputListAdapter(getActivity(), childProfileModel.SiblingsJoiningChildFundModel.ListObject, false);
            recyclerSiblingsJoiningChildFund.setAdapter(checkListSiblingsJoiningChildFundAdapter);
            checkListSiblingsJoiningChildFundAdapter.SetOnItemClickListener(new CheckInputListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, ObjectInputModel obj) {
                    CheckBox checkBox = (CheckBox) view;
                    for (ObjectInputModel objectInputModel : childProfileModel.SiblingsJoiningChildFundModel.ListObject) {
                        objectInputModel.Check = false;
                        objectInputModel.OtherValue = "";
                    }
                    childProfileModel.SiblingsJoiningChildFundModel.ListObject.get(position).Check = checkBox.isChecked();
                    checkListSiblingsJoiningChildFundAdapter.notifyDataSetChanged();
                }

                @Override
                public void onTextChange(int position, String obj) {
                    childProfileModel.SiblingsJoiningChildFundModel.ListObject.get(position).OtherValue = obj;
                }

                @Override
                public void onRadioCheck(int position, RadioButton obj) {
                }
            });
        } catch (Exception ex) {
        }
    }

    /***
     *Sét data cho slect chọn Tỉnh
     */
    private void setDataProvince(String provinceId) {
        try {
            String dataFixProvince = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Province_Area, null);
            if (dataFixProvince != null) {
                listProvince = new Gson().fromJson(dataFixProvince, new TypeToken<List<ComboboxResult>>() {
                }.getType());

                spnProvince.setText(Utils.getNameById(listProvince, provinceId));
                spnProvince.setTag(provinceId);
            }
        } catch (Exception ex) {
        }
    }

    /***
     * Sét data cho slect chọn huyện
     * @param districtId
     */
    private void setDataDistrict(String districtId) {
        try {
            String dataFixDistrict = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_District_Area, null);
            if (dataFixDistrict != null) {
                listDistrict = new Gson().fromJson(dataFixDistrict, new TypeToken<List<ComboboxResult>>() {
                }.getType());

                for (ComboboxResult model : listDistrict) {
                    if (model.Id.equals(districtId)) {
                        spnDistrict.setText(model.Name);
                        spnDistrict.setTag(model.Id);
                    }
                }
            }
        } catch (Exception ex) {
        }
    }

    /***
     *Sét data cho slect chọn xã
     * @param wardId
     */
    private void setDataWard(String wardId) {
        try {
            String dataFixWard = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Ward_Area, null);
            if (dataFixWard != null) {
                listWard = new Gson().fromJson(dataFixWard, new TypeToken<List<ComboboxResult>>() {
                }.getType());
                final ArrayList<String> arrayWardName = new ArrayList<>();
                for (ComboboxResult model : listWard) {
                    if (model.Id.equals(wardId)) {
                        spnWard.setText(model.Name);
                        spnWard.setTag(model.Id);
                    }
                    arrayWardName.add(model.Name);
                }
                spnWard.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        showChoiceDialog((EditText) v, arrayWardName.toArray(new String[0]), listWard, "Xã/Phường");
                    }
                });
            }
        } catch (Exception ex) {
        }
    }

    /***
     *Sét data cho slect chọn thôn xóm
     * @param wardId
     */
    private void setDataVillage(String wardId, final String villageId) {
        try {
            progressDialog.setVisibility(View.VISIBLE);
            if (global.isStatusRunApp()) {
                listVillage = new Gson().fromJson(sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Village + loginProfileModel.WardId, null), new TypeToken<List<ComboboxResult>>() {
                }.getType());
            } else {
                AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetVillageByWrad?id=" + wardId))
                        .setPriority(Priority.MEDIUM)
                        .build()
                        .getAsJSONArray(new JSONArrayRequestListener() {
                            @Override
                            public void onResponse(JSONArray response) {
                                progressDialog.setVisibility(View.GONE);
                                listVillage = new Gson().fromJson(response.toString(), new TypeToken<List<ComboboxResult>>() {
                                }.getType());
                            }

                            @Override
                            public void onError(ANError anError) {
                                progressDialog.setVisibility(View.GONE);
                                Utils.showErrorMessage(getActivity().getApplication(), anError);
                            }
                        });
            }
            for (ComboboxResult model : listVillage) {
                if (model.Id.equals(villageId)) {
                    spnVillage.setText(model.Name);
                    spnVillage.setTag(model.Id);
                    break;
                }
            }
            progressDialog.setVisibility(View.VISIBLE);
        } catch (Exception ex) {
        }
    }

    /***
     *Sét data cho slect chọn trường học theo xã
     * @param wardId
     */
    private void setDataSchool(String wardId, String schoolId) {
        try {
            String dataFixSchool = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_School, null);
            if (dataFixSchool != null) {
                List<ComboboxResult> listFixSchool = new Gson().fromJson(dataFixSchool, new TypeToken<List<ComboboxResult>>() {
                }.getType());
                final ArrayList<String> arraySchoolName = new ArrayList<>();
                listSchoolName = new ArrayList<>();
                for (ComboboxResult model : listFixSchool) {
                    if (model.PId.equals(wardId)) {
                        listSchoolName.add(model);
                        arraySchoolName.add(model.Name);
                    }

                    if (schoolId.equals(model.Id)) {
                        txtSchool.setText(model.Name);
                        txtSchool.setTag(model.Id);
                    }
                }
                ComboboxResult comboboxResult = new ComboboxResult();
                comboboxResult.Id = "Other";
                comboboxResult.Name = "Khác/Other";
                listSchoolName.add(comboboxResult);
                arraySchoolName.add("Khác/Other");

                if (schoolId.equals("Other")) {
                    txtSchool.setText("Khác/Other");
                    txtSchool.setTag("Other");
                }

                txtSchool.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        showChoiceDialog((EditText) v, arraySchoolName.toArray(new String[0]), listSchoolName, "Trường-lớp/ School-grade");
                    }
                });
            }
        } catch (Exception ex) {
        }
    }

    /***
     * Hiển thị thông tin lên Spinners
     */
    private void viewDataSpinners() {
        try {
            final ArrayList<String> arrayPrimarySchoolClass = new ArrayList<>();
            arrayPrimarySchoolClass.add("1");
            arrayPrimarySchoolClass.add("2");
            arrayPrimarySchoolClass.add("3");
            arrayPrimarySchoolClass.add("4");
            arrayPrimarySchoolClass.add("5");
            spnPrimarySchoolClass.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChoiceDialog(spnPrimarySchoolClass, arrayPrimarySchoolClass.toArray(new String[0]), "Học lớp");
                }
            });

            final ArrayList<String> arraySecondarySchoolClass = new ArrayList<>();
            arraySecondarySchoolClass.add("6");
            arraySecondarySchoolClass.add("7");
            arraySecondarySchoolClass.add("8");
            arraySecondarySchoolClass.add("9");
            spnSecondarySchoolClass.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChoiceDialog(spnSecondarySchoolClass, arraySecondarySchoolClass.toArray(new String[0]), "Học lớp/Grade");
                }
            });

            final ArrayList<String> arrayEmployeeTitle = new ArrayList<>();
            arrayEmployeeTitle.add("Giáo viên/Teacher");
            arrayEmployeeTitle.add("Điều phối viên cộng đồng/Community Facilitator");
            arrayEmployeeTitle.add("Cán bộ ChildFund/ChildFund staff");
            listEmployeeTitle = new ArrayList<>();
            ComboboxResult comboboxResult = new ComboboxResult();
            comboboxResult.Id = "1";
            comboboxResult.Name = "Giáo viên/Teacher";
            listEmployeeTitle.add(comboboxResult);
            comboboxResult = new ComboboxResult();
            comboboxResult.Id = "2";
            comboboxResult.Name = "Điều phối viên cộng đồng/Community Facilitator";
            listEmployeeTitle.add(comboboxResult);
            comboboxResult = new ComboboxResult();
            comboboxResult.Id = "3";
            comboboxResult.Name = "Cán bộ ChildFund/ChildFund staff";
            listEmployeeTitle.add(comboboxResult);

            spnEmployeeTitle.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChoiceDialog((EditText) v, arrayEmployeeTitle.toArray(new String[0]), listEmployeeTitle, "Chức vụ/Title");
                }
            });

            final ArrayList<String> arrayProgramCode = new ArrayList<>();
            arrayProgramCode.add("199");
            arrayProgramCode.add("213");
            spnProgramCode.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChoiceDialog((EditText) v, arrayProgramCode.toArray(new String[0]), "Mã số chương trình/ Program Code");
                }
            });

            final ArrayList<String> arrayFamilyType = new ArrayList<>();
            arrayFamilyType.add("Nghèo/Poor");
            arrayFamilyType.add("Cận nghèo/Almost poor");
            arrayFamilyType.add("Trung bình/Average");
            arrayFamilyType.add("Khá/Good");
            spnFamilyType.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChoiceDialog((EditText) v, arrayFamilyType.toArray(new String[0]), "Thuộc loại hộ nào?/ Household type");
                }
            });

            final ArrayList<String> arrayRelationship5 = new ArrayList<>();
            arrayRelationship5.add("Bố/Father");
            arrayRelationship5.add("Mẹ/Mother");
            arrayRelationship5.add("Ông/Grandfather");
            arrayRelationship5.add("Bà/Grandmother");
            arrayRelationship5.add("Cô,dì,bác/Aunt");
            arrayRelationship5.add("Chú,cậu,bác/Uncle");
            spnConsentRelationship.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChoiceDialog((EditText) v, arrayRelationship5.toArray(new String[0]), "Mối quan hệ/ Relationship");
                }
            });

            String dataFixGeligion = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Geligion, null);
            if (dataFixGeligion != null) {
                listGeligion = new Gson().fromJson(dataFixGeligion, new TypeToken<List<ComboboxResult>>() {
                }.getType());
                final ArrayList<String> arrayGeligion = new ArrayList<>();
                for (ComboboxResult model : listGeligion) {
                    arrayGeligion.add(model.Name);
                }
                spnReligion.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        showChoiceDialog((EditText) v, arrayGeligion.toArray(new String[0]), listGeligion, "Trẻ theo tôn giáo gì?/ Religion");
                    }
                });
            }

            String dataFixNation = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Nation, null);
            if (dataFixNation != null) {
                listNation = new Gson().fromJson(dataFixNation, new TypeToken<List<ComboboxResult>>() {
                }.getType());
                final ArrayList<String> arrayNation = new ArrayList<>();
                for (ComboboxResult model : listNation) {
                    arrayNation.add(model.Name);
                }
                spnNation.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        showChoiceDialog((EditText) v, arrayNation.toArray(new String[0]), listNation, "Trẻ thuộc dân tộc nào?/ Ethnic minority group");
                    }
                });
            }

            String dataFixProvince = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Province_Area, null);
            if (dataFixProvince != null) {
                listProvince = new Gson().fromJson(dataFixProvince, new TypeToken<List<ComboboxResult>>() {
                }.getType());
                if (listProvince.size() > 0) {
                    spnProvince.setText(listProvince.get(0).Name);
                    spnProvince.setTag(listProvince.get(0).Id);
                }
            }

            String dataFixDistrict = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_District_Area, null);
            if (dataFixDistrict != null) {
                listDistrict = new Gson().fromJson(dataFixDistrict, new TypeToken<List<ComboboxResult>>() {
                }.getType());
                if (listDistrict.size() > 0) {
                    spnDistrict.setText(listDistrict.get(0).Name);
                    spnDistrict.setTag(listDistrict.get(0).Id);
                }
            }

            String dataFixWard = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Ward_Area, null);
            if (dataFixWard != null) {
                listWard = new Gson().fromJson(dataFixWard, new TypeToken<List<ComboboxResult>>() {
                }.getType());

                if (listWard.size() > 0) {
                    txtConsentWard.setText(listWard.get(0).Name);
                    spnWard.setText(listWard.get(0).Name);
                    spnWard.setTag(listWard.get(0).Id);
                    getDataVillage(listWard.get(0).Id);
                    getDataSchool(listWard.get(0).Id);
                }
//                final ArrayList<String> arrayWardName = new ArrayList<>();
//                for (ComboboxResult model : listWard) {
//                    arrayWardName.add(model.Name);
//                }
//                spnWard.setOnClickListener(new View.OnClickListener() {
//                    @Override
//                    public void onClick(View v) {
//                        showChoiceDialog((EditText) v, arrayWardName.toArray(new String[0]), listWard, "Xã/ Commune");
//                    }
//                });
            }

            String dataFixRelationship = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Relationship, null);
            if (dataFixRelationship != null) {
                listRelationship = new Gson().fromJson(dataFixRelationship, new TypeToken<List<ComboboxResult>>() {
                }.getType());
                arrayRelationship = new ArrayList<>();
                for (ComboboxResult model : listRelationship) {
                    arrayRelationship.add(model.Name);
                }
            }
        } catch (Exception ex) {
        }
    }

    private void getDataSchool(String wradId) {
        String dataFixSchool = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_School, null);
        if (dataFixSchool != null) {
            List<ComboboxResult> listFixSchool = new Gson().fromJson(dataFixSchool, new TypeToken<List<ComboboxResult>>() {
            }.getType());
            final ArrayList<String> arraySchoolName = new ArrayList<>();
            listSchoolName = new ArrayList<>();
            for (ComboboxResult model : listFixSchool) {
                if (model.PId.equals(wradId)) {
                    listSchoolName.add(model);
                    arraySchoolName.add(model.Name);
                }
            }
            ComboboxResult comboboxResult = new ComboboxResult();
            comboboxResult.Id = "Other";
            comboboxResult.Name = "Khác/Other";
            listSchoolName.add(comboboxResult);
            arraySchoolName.add("Khác/Other");
            txtSchool.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChoiceDialog((EditText) v, arraySchoolName.toArray(new String[0]), listSchoolName, "Trường-lớp/ School-grade");
                }
            });
        }
    }

    private void getDataVillage(String wardId) {
        try {
            if (global.isStatusRunApp()) {
                listVillage = new Gson().fromJson(sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Village + wardId, null), new TypeToken<List<ComboboxResult>>() {
                }.getType());
            } else {
                AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetVillageByWrad?id=" + (spnWard.getTag() != null ? spnWard.getTag().toString() : "")))
                        .setPriority(Priority.MEDIUM)
                        .build()
                        .getAsJSONArray(new JSONArrayRequestListener() {
                            @Override
                            public void onResponse(JSONArray response) {
                                listVillage = new Gson().fromJson(response.toString(), new TypeToken<List<ComboboxResult>>() {
                                }.getType());
                            }

                            @Override
                            public void onError(ANError anError) {
                                Utils.showErrorMessage(getActivity().getApplication(), anError);
                            }
                        });
            }
        } catch (Exception ex) {

        }
    }

    /***
     * Khởi tạo modal thêm mới thành viên trong gia đình
     */
    private void initDialogFamilyInfo(final boolean isUpdate) {
        familyInfoModel = new FamilyMemberModel();
        familyInfoModel.LiveWithChild = 1;

        dialogFamilyInfo = new Dialog(getContext());
        dialogFamilyInfo.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialogFamilyInfo.setContentView(R.layout.popup_family_info);
        dialogFamilyInfo.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));

        final TextInputLayout tilName = dialogFamilyInfo.findViewById(R.id.tilName);
        final TextInputLayout tilDateOfBirth = dialogFamilyInfo.findViewById(R.id.tilDateOfBirth);
        final TextInputLayout tilRelationship = dialogFamilyInfo.findViewById(R.id.tilRelationship);

        final EditText txtName = dialogFamilyInfo.findViewById(R.id.txtName);
        final EditText txtDateOfBirth = dialogFamilyInfo.findViewById(R.id.txtDateOfBirth);
        final EditText spnRelationship = dialogFamilyInfo.findViewById(R.id.spnRelationship);
        final RadioGroup rgGender = dialogFamilyInfo.findViewById(R.id.rgGender);
        final EditText spnJob = dialogFamilyInfo.findViewById(R.id.spnJob);
        final EditText txtJobOtherName = dialogFamilyInfo.findViewById(R.id.txtJobOtherName);
        final RadioGroup rgLiveTogether = dialogFamilyInfo.findViewById(R.id.rgLiveTogether);
        Button btnAdd = dialogFamilyInfo.findViewById(R.id.btnAdd);
        Button btnClose = dialogFamilyInfo.findViewById(R.id.btnClose);

        btnAdd.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Lấy giá trị vào model
                familyInfoModel.Name = txtName.getText().toString();
                familyInfoModel.DateOfBirthView = "01/01/" + txtDateOfBirth.getText().toString();
                familyInfoModel.DateOfBirth = "01/01/" + txtDateOfBirth.getText().toString();

                familyInfoModel.Job = spnJob.getTag() != null ? spnJob.getTag().toString() : "";
                familyInfoModel.JobName = Utils.getNameById(listJob, familyInfoModel.Job);
                familyInfoModel.JobOtherName = "";
                if (familyInfoModel.JobName.equals("Khác/Other")) {
                    familyInfoModel.JobOtherName = txtJobOtherName.getText().toString();
                }

                int radioButtonID = rgGender.getCheckedRadioButtonId();
                RadioButton radioButton = (RadioButton) rgGender.findViewById(radioButtonID);
                if (radioButton != null) {
                    familyInfoModel.Gender = Integer.parseInt(radioButton.getTag().toString());
                }

                radioButtonID = rgLiveTogether.getCheckedRadioButtonId();
                radioButton = (RadioButton) rgLiveTogether.findViewById(radioButtonID);
                if (radioButton != null) {
                    familyInfoModel.LiveWithChild = Integer.parseInt(radioButton.getTag().toString());
                }

                //Check validate
                if (Utils.isEmpty(familyInfoModel.Name)) {
                    tilName.setError("Không được để trống.");
                    txtName.requestFocus();
                    return;
                } else {
                    tilName.setErrorEnabled(false);
                }


                if (!Utils.isEmpty(familyInfoModel.DateOfBirth)) {
                    SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd");
                    try {
                        familyInfoModel.DateOfBirth = DateUtils.ConvertDMYToYMD(familyInfoModel.DateOfBirthView);
                        Date date = format.parse(familyInfoModel.DateOfBirth);
                    } catch (Exception ex) {
                        tilDateOfBirth.setError("Lỗi định dạng.");
                        txtDateOfBirth.requestFocus();
                        return;
                    }
                    tilDateOfBirth.setErrorEnabled(false);
                }

                if (Utils.isEmpty(spnRelationship.getText().toString())) {
                    tilRelationship.setError("Không được để trống.");
                    spnRelationship.requestFocus();
                    return;
                } else {
                    tilRelationship.setErrorEnabled(false);
                }

                if (childProfileModel.ListFamilyMember.size() > 0) {
                    for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                        item.Position = childProfileModel.ListFamilyMember.indexOf(item);
                    }
                }

                boolean isHasFather = false;
                int positionFather = -1;
                if (spnRelationship.getTag().toString().equals(Constants.FAMILY_RELATIONSHIP_FATHER)) {
                    for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                        if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_FATHER)) {
                            isHasFather = true;
                            positionFather = item.Position;
                        }
                    }
                }

                if (isHasFather ) {
                    if (isUpdate) {
                        if (positionFather != familyInfoModel.Position) {
                            tilRelationship.setError("Danh sách gia đình trẻ đã có Bố/Father");
                            Toast.makeText(getActivity(), "Danh sách gia đình trẻ đã có Bố/Father", Toast.LENGTH_SHORT).show();
                            spnRelationship.requestFocus();
                            return;
                        } else {
                            tilRelationship.setErrorEnabled(false);
                        }
                    } else {
                        tilRelationship.setError("Danh sách gia đình trẻ đã có Bố/Father");
                        Toast.makeText(getActivity(), "Danh sách gia đình trẻ đã có Bố/Father", Toast.LENGTH_SHORT).show();
                        spnRelationship.requestFocus();
                        return;
                    }
                } else {
                    tilRelationship.setErrorEnabled(false);
                }

                boolean isHasMother = false;
                int positionMother = -1;
                if (spnRelationship.getTag().toString().equals(Constants.FAMILY_RELATIONSHIP_MOTHER)) {
                    for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                        if (item.RelationshipId.equals(Constants.FAMILY_RELATIONSHIP_MOTHER)) {
                            isHasMother = true;
                            positionMother = item.Position;
                        }
                    }
                }

                if (isHasMother) {
                    if (isUpdate) {
                        if (positionMother != familyInfoModel.Position) {
                            tilRelationship.setError("Danh sách gia đình trẻ đã có Mẹ/Mother");
                            Toast.makeText(getActivity(), "Danh sách gia đình trẻ đã có Mẹ/Mother", Toast.LENGTH_SHORT).show();
                            spnRelationship.requestFocus();
                            return;
                        } else {
                            tilRelationship.setErrorEnabled(false);
                        }
                    } else {
                        tilRelationship.setError("Danh sách gia đình trẻ đã có Mẹ/Mother");
                        Toast.makeText(getActivity(), "Danh sách gia đình trẻ đã có Mẹ/Mother", Toast.LENGTH_SHORT).show();
                        spnRelationship.requestFocus();
                        return;
                    }
                } else {
                    tilRelationship.setErrorEnabled(false);
                }

                familyInfoModel.RelationshipId = spnRelationship.getTag() != null ? spnRelationship.getTag().toString() : "";
                familyInfoModel.RelationshipName = Utils.getNameById(listRelationship, familyInfoModel.RelationshipId);

                if (familyInfoModel.Position != null) {
                    childProfileModel.ListFamilyMember.get(familyInfoModel.Position).Name = familyInfoModel.Name;
                    childProfileModel.ListFamilyMember.get(familyInfoModel.Position).UpdateBy = familyInfoModel.UpdateBy = loginProfileModel.Id;
                    childProfileModel.ListFamilyMember.get(familyInfoModel.Position).DateOfBirth = familyInfoModel.DateOfBirth;
                    childProfileModel.ListFamilyMember.get(familyInfoModel.Position).LiveWithChild = familyInfoModel.LiveWithChild;
                    childProfileModel.ListFamilyMember.get(familyInfoModel.Position).Gender = familyInfoModel.Gender;
                    childProfileModel.ListFamilyMember.get(familyInfoModel.Position).Job = familyInfoModel.Job;
                    checkListFamilyAdapter.notifyDataSetChanged();
                    countSisterAndBrother();
                    dialogFamilyInfo.dismiss();
                    Toast.makeText(getActivity(), "Cập nhật thành viên thành công!", Toast.LENGTH_SHORT).show();
                } else {
                    familyInfoModel.CreateBy = loginProfileModel.Id;
                    familyInfoModel.UpdateBy = loginProfileModel.Id;
                    childProfileModel.ListFamilyMember.add(familyInfoModel);
                    familyInfoModel = new FamilyMemberModel();
                    countSisterAndBrother();
                    Toast.makeText(getActivity(), "Thêm mới thành viên thành công!", Toast.LENGTH_SHORT).show();
                }
                txtName.setText("");
                txtDateOfBirth.setText("");
                spnRelationship.setTag("");
                spnRelationship.setText("");
                spnJob.setTag("");
                spnJob.setText("");
                txtName.requestFocus();
                checkListFamilyAdapter.notifyDataSetChanged();
            }
        });

        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                txtName.setText("");
                txtDateOfBirth.setText("");
                spnRelationship.setTag("");
                spnRelationship.setText("");
                spnJob.setTag("");
                spnJob.setText("");
                dialogFamilyInfo.dismiss();
            }
        });

        rgGender.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                RadioButton checkedRadioButton = (RadioButton) group.findViewById(checkedId);
                familyInfoModel.Gender = Integer.parseInt(checkedRadioButton.getTag().toString());
            }
        });

        rgLiveTogether.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                RadioButton checkedRadioButton = (RadioButton) group.findViewById(checkedId);
                familyInfoModel.LiveWithChild = Integer.parseInt(checkedRadioButton.getTag().toString());
            }
        });

        spnRelationship.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChoiceRelationshipDialog((EditText) v, rgGender, arrayRelationship.toArray(new String[0]), listRelationship, "Mối quan hệ với trẻ được tuyển/ Relationship with child");
            }
        });

        String dataFixJob = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Job, null);
        if (dataFixJob != null) {
            listJob = new Gson().fromJson(dataFixJob, new TypeToken<List<ComboboxResult>>() {
            }.getType());
            final ArrayList<String> arrayJob = new ArrayList<>();
            for (ComboboxResult model : listJob) {
                arrayJob.add(model.Name);
            }
            spnJob.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    showChoiceDialog((EditText) v, arrayJob.toArray(new String[0]), listJob, "Nghề nghiệp/ Occupation");
                }
            });
        }
    }

    /***
     * Show giao diện sửa
     * @param position
     */
    private void showUpdateFamilyInfo(Integer position) {
        if (childProfileModel.ListFamilyMember.size() > 0) {
            for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
                item.Position = childProfileModel.ListFamilyMember.indexOf(item);
            }
        }
        EditText txtName = dialogFamilyInfo.findViewById(R.id.txtName);
        EditText txtDateOfBirth = dialogFamilyInfo.findViewById(R.id.txtDateOfBirth);
        EditText spnRelationship = dialogFamilyInfo.findViewById(R.id.spnRelationship);
        RadioGroup rgGender = dialogFamilyInfo.findViewById(R.id.rgGender);
        EditText spnJob = dialogFamilyInfo.findViewById(R.id.spnJob);
        RadioGroup rgLiveTogether = dialogFamilyInfo.findViewById(R.id.rgLiveTogether);
        EditText txtJobOtherName = dialogFamilyInfo.findViewById(R.id.txtJobOtherName);
        LinearLayout lyJobOtherName = dialogFamilyInfo.findViewById(R.id.lyJobOtherName);

        if (position != null) {
            familyInfoModel = childProfileModel.ListFamilyMember.get(position);
            familyInfoModel.Position = position;
            txtName.setText(familyInfoModel.Name);
            String dateOfBirth = DateUtils.ConvertYMDServerToDMY(familyInfoModel.DateOfBirth);
            if (dateOfBirth.length() >= 10)
                txtDateOfBirth.setText(dateOfBirth.substring(6, 10));
            spnJob.setText(Utils.getNameById(listJob, familyInfoModel.Job));
            spnJob.setTag(familyInfoModel.Job);
            spnRelationship.setText(Utils.getNameById(listRelationship, familyInfoModel.RelationshipId));
            spnRelationship.setTag(familyInfoModel.RelationshipId);
            if (familyInfoModel.Gender != null) {
                if (familyInfoModel.Gender == Constants.Male) {
                    RadioButton radioMale = rgGender.findViewById(R.id.radioMale);
                    radioMale.setChecked(true);
                } else {
                    RadioButton radioFemale = rgGender.findViewById(R.id.radioFemale);
                    radioFemale.setChecked(true);
                }
            }
            if (familyInfoModel.LiveWithChild != null) {
                if (familyInfoModel.LiveWithChild == 1) {
                    RadioButton radioYes = rgLiveTogether.findViewById(R.id.radioYes);
                    radioYes.setChecked(true);
                } else {
                    RadioButton radioNo = rgLiveTogether.findViewById(R.id.radioNo);
                    radioNo.setChecked(true);
                }
            }
            if (spnJob.getText().toString().equals("Khác/Other")) {
                lyJobOtherName.setVisibility(View.VISIBLE);
                txtJobOtherName.setText(familyInfoModel.JobOtherName);
            } else {
                lyJobOtherName.setVisibility(View.GONE);
            }
            dialogFamilyInfo.show();
        }
    }

    /***
     * Load thông tin thành viên gia đình
     */
    private void loadFamilyInfoAdapter() {
        checkListFamilyAdapter = new FamilyListAdapter(getActivity(), childProfileModel.ListFamilyMember);
        recyclerFamily.setAdapter(checkListFamilyAdapter);
        checkListFamilyAdapter.SetOnItemClickListener(new FamilyListAdapter.OnItemClickListener() {
            @Override
            public void onButtonEditClick(View view, int position, FamilyMemberModel obj) {
                initDialogFamilyInfo(true);
                showUpdateFamilyInfo(position);
            }

            @Override
            public void onButtonDeleteClick(View view, final int position, FamilyMemberModel obj) {
                new AlertDialog.Builder(getActivity())
                        .setMessage("Bạn có muốn xóa thành viên này không?")
                        .setCancelable(false)
                        .setPositiveButton("Đồng ý", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                childProfileModel.ListFamilyMember.remove(position);
                                checkListFamilyAdapter.notifyDataSetChanged();
                                countSisterAndBrother();
                                for (ObjectInputModel item :childProfileModel.LivingWithParentModel.ListObject) {
                                    item.Check=false;
                                }
                                checkListLiveParentAdapter.notifyDataSetChanged();
                            }
                        })
                        .setNegativeButton("Đóng", null)
                        .show();
            }
        });
    }

    /***
     * Dếm số lượng anh chị em ruột
     */
    private void countSisterAndBrother() {
        int countSister = 0, countBrother = 0;
        for (FamilyMemberModel item : childProfileModel.ListFamilyMember) {
            if (item.RelationshipId.equals(Constants.RelationshipOlderSister) || item.RelationshipId.equals(Constants.RelationshipYoungerSister)) {
                countSister++;
            } else if (item.RelationshipId.equals(Constants.RelationshipOlderBrother) || item.RelationshipId.equals(Constants.RelationshipYoungerBrother)) {
                countBrother++;
            }
        }
        txtTotalSisters.setText(String.valueOf(countSister));
        txtTotalBrothers.setText(String.valueOf(countBrother));
    }

    private void resetForm() {
        Fragment fragment = new CreateProfileChildFragment();
        FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        fragmentTransaction.replace(R.id.frame_content, fragment);
        fragmentTransaction.commit();
    }

    private void requestPermissions(String permissionsName) {
        new AlertDialog.Builder(getActivity())
                .setTitle("Thiết bị chưa cho phép ứng dụng sử dụng " + permissionsName)
                .setIcon(R.drawable.ic_warning)
                .setMessage("Vui lòng cho phép ứng dụng quyền " + permissionsName + " để sử dụng tính năng này.")
                .setCancelable(false)
                .setPositiveButton("Bật", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        Intent intent = new Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS,
                                Uri.parse("package:" + getActivity().getPackageName()));
                        intent.addCategory(Intent.CATEGORY_DEFAULT);
                        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                        getActivity().startActivity(intent);
                    }
                })
                .setNegativeButton("Không", null)
                .show();
    }
}
