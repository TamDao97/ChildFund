package nts.swipesafe.fragment;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.annotation.NonNull;
import android.support.design.widget.Snackbar;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.FileProvider;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.telephony.TelephonyManager;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.CheckBox;
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
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import nts.swipesafe.MainActivity;
import nts.swipesafe.R;
import nts.swipesafe.adapter.ChildInfoListAdapter;
import nts.swipesafe.adapter.ImageAdapter;
import nts.swipesafe.adapter.ImageReportAdapter;
import nts.swipesafe.adapter.PrisonerInfoListAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.LinkApi;
import nts.swipesafe.common.LocationGpsListener;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ChildInfoModel;
import nts.swipesafe.model.ComboboxItem;
import nts.swipesafe.model.GalleryModel;
import nts.swipesafe.model.LocationModel;
import nts.swipesafe.model.PrisonerInfoModel;
import nts.swipesafe.model.ReportInfoModel;
import nts.swipesafe.model.ReportMobileModel;
import nts.swipesafe.model.ReportOfflineModel;
import nts.swipesafe.model.ResultModel;
import nts.swipesafe.model.UploadFileResultModel;
import nts.swipesafe.services.SendOfflineService;

import static android.Manifest.permission.READ_EXTERNAL_STORAGE;
import static android.content.Context.MODE_PRIVATE;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseStepInfoFragment extends Fragment {
    private View view;
    private GlobalVariable global;
    private LinearLayout lyNext, lyBack, lyAddChild, lyAddPrisoner, lilConfirm;
    private EditText txtName, txtPhone, txtAddress, txtDescription;
    private AutoCompleteTextView spnProvince, spnDistrict, spnWard;
    private String[] arrayProvince, arrayDistrict, arrayWard, arrayRelationship;
    private List<ComboboxItem> listProvince, listDistrict, listWard, listRelationship;
    private TextView lblLocation, lblConfirm;
    private LocationGpsListener locationGpsListener;
    private RelativeLayout progressDialog;
    private Dialog dialogAddChild, dialogAddObject;
    private Toast toast;
    private RecyclerView rvChild, rvPrisoner, rvGalleryChoose;
    private ChildInfoListAdapter childListAdapter;
    private PrisonerInfoListAdapter prisonerListAdapter;
    private ReportInfoModel reportInfoModel = new ReportInfoModel();
    private List<ChildInfoModel> listChild = new ArrayList<>();
    private List<PrisonerInfoModel> listPrisoner = new ArrayList<>();
    private CheckBox ckConfirm;

    //Upload ảnh
    private ImageReportAdapter imageReportAdapter;
    private ImageAdapter imageAdapter;
    private static final int REQUEST_FOR_STORAGE_PERMISSION = 123;
    private Dialog dialogGallery;
    private ImageView imageCaptureEngine, imageChoosePhoto;
    private String imageLink;
    private ArrayList<GalleryModel> fileReport = new ArrayList<>();
    private long fileSize = 0;
    private TextView txtFileSize;
    //Vùng tổng đài
    private String area = "";
    private boolean isDismiss = true;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_report_abuse_step_info, container, false);

        locationGpsListener = new LocationGpsListener(getActivity().getApplicationContext());
        global = (GlobalVariable) getActivity().getApplication();

        initComponent();

        initDialogAddChild();
        initDialogAddObject();

        getDataFix();

        confirmSecurity();

        viewImageReport(fileReport);

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
                Utils.SavePicture(imageLink);
                File file = new File(imageLink);
                GalleryModel galleryModel = new GalleryModel();
                galleryModel.FileUrl = imageLink;
                galleryModel.Type = 1;
                galleryModel.Size = file.length();
                fileReport.add(galleryModel);
                viewImageReport(fileReport);
            }
            super.onActivityResult(requestCode, resultCode, data);
        } catch (Exception ex) {
        }
    }

    public void backFragment() {
        new AlertDialog.Builder(getActivity())
                .setTitle("Cảnh báo")
                .setIcon(R.drawable.ic_warning)
                .setMessage("Báo cáo chưa được gửi đi. Bạn có muốn thoát khỏi chương trình không?")
                .setCancelable(false)
                .setPositiveButton("Đồng ý", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        Fragment fragment = new ReportMainFragment();
                        Utils.ChangeFragment(getActivity(), fragment, null);
                    }
                })
                .setNegativeButton("Hủy", null)
                .show();
    }

    /***
     * Khỏi tạo thành phần trên giao diện
     */
    private void initComponent() {
        progressDialog = view.findViewById(R.id.progressDialog);

        rvChild = view.findViewById(R.id.rvChild);
        rvChild.setLayoutManager(new GridLayoutManager(getActivity(), 1));

        rvPrisoner = view.findViewById(R.id.rvPrisoner);
        rvPrisoner.setLayoutManager(new GridLayoutManager(getActivity(), 1));

        lyBack = view.findViewById(R.id.lyBack);
        lyNext = view.findViewById(R.id.lyNext);
        lyAddChild = view.findViewById(R.id.lyAddChild);
        lyAddPrisoner = view.findViewById(R.id.lyAddPrisoner);
        lilConfirm = view.findViewById(R.id.lilConfirm);

        txtName = view.findViewById(R.id.txtName);
        txtPhone = view.findViewById(R.id.txtPhone);
        spnProvince = view.findViewById(R.id.spnProvince);
        spnProvince.setSelectAllOnFocus(true);
        spnDistrict = view.findViewById(R.id.spnDistrict);
        spnWard = view.findViewById(R.id.spnWard);
        txtAddress = view.findViewById(R.id.txtAddress);
        txtDescription = view.findViewById(R.id.txtDescription);

        lblLocation = view.findViewById(R.id.lblLocation);

        imageChoosePhoto = (ImageView) view.findViewById(R.id.imageChoosePhoto);
        imageCaptureEngine = (ImageView) view.findViewById(R.id.imageCaptureEngine);
        txtFileSize = view.findViewById(R.id.txtFileSize);
        ckConfirm = view.findViewById(R.id.ckConfirm);
        lblConfirm = view.findViewById(R.id.lblConfirm);


        lyNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                sendReport();
            }
        });

        lyBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                backFragment();
            }
        });

        if (ContextCompat.checkSelfPermission(getContext(), Manifest.permission.READ_PHONE_STATE)
                == PackageManager.PERMISSION_GRANTED) {
            TelephonyManager tMgr = (TelephonyManager) getContext().getSystemService(Context.TELEPHONY_SERVICE);
            String phoneNumber = tMgr.getLine1Number();
            if (!Utils.isEmpty(phoneNumber)) {
                txtPhone.setText(phoneNumber.replace("+84", "0"));
            }
        }

        spnProvince.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                int index = Utils.getIndexByName(arrayProvince, adapterView.getItemAtPosition(i).toString());
                area = listProvince.get(index).area;
                getDataDistrict(listProvince.get(index).code);
                spnDistrict.setTag("");
                spnDistrict.setText("");
                spnWard.setTag("");
                spnWard.setText("");
                txtAddress.setText("");
            }
        });

        spnProvince.setOnTouchListener(new View.OnTouchListener() {

            @SuppressLint("ClickableViewAccessibility")
            @Override
            public boolean onTouch(View paramView, MotionEvent paramMotionEvent) {
                if (arrayProvince != null && arrayProvince.length > 0) {
                    // show all suggestions
                    if (spnProvince.getText().toString().equals(""))
                        spnProvince.showDropDown();
                }
                return false;
            }
        });

        spnDistrict.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                int index = Utils.getIndexByName(arrayDistrict, adapterView.getItemAtPosition(i).toString());
                getDataWrap(listDistrict.get(index).code);
                spnWard.setTag("");
                spnWard.setText("");
                txtAddress.setText("");
            }
        });

        spnDistrict.setOnTouchListener(new View.OnTouchListener() {

            @SuppressLint("ClickableViewAccessibility")
            @Override
            public boolean onTouch(View paramView, MotionEvent paramMotionEvent) {
                if (arrayDistrict != null && arrayDistrict.length > 0) {
                    // show all suggestions
                    if (spnDistrict.getText().toString().equals(""))
                        spnDistrict.showDropDown();
                }
                return false;
            }
        });


        spnWard.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                txtAddress.setText("");
            }
        });

        spnWard.setOnTouchListener(new View.OnTouchListener() {

            @SuppressLint("ClickableViewAccessibility")
            @Override
            public boolean onTouch(View paramView, MotionEvent paramMotionEvent) {
                if (arrayWard != null && arrayWard.length > 0) {
                    // show all suggestions
                    if (spnWard.getText().toString().equals(""))
                        spnWard.showDropDown();
                }
                return false;
            }
        });

        lyAddChild.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialogAddChild.show();
            }
        });

        lyAddPrisoner.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialogAddObject.show();
            }
        });

        final int sdk = android.os.Build.VERSION.SDK_INT;
        lilConfirm.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                ckConfirm.setChecked(!ckConfirm.isChecked());
                if (ckConfirm.isChecked()) {
                    lblConfirm.setTextColor(getResources().getColor(R.color.blue_600));
                    if (sdk < android.os.Build.VERSION_CODES.JELLY_BEAN) {
                        lyNext.setBackgroundDrawable(ContextCompat.getDrawable(getContext(), R.drawable.btn_rounded_green));
                    } else {
                        lyNext.setBackground(ContextCompat.getDrawable(getContext(), R.drawable.btn_rounded_green));
                    }
                } else {
                    lblConfirm.setTextColor(getResources().getColor(R.color.grey_700));
                    if (sdk < android.os.Build.VERSION_CODES.JELLY_BEAN) {
                        lyNext.setBackgroundDrawable(ContextCompat.getDrawable(getContext(), R.drawable.btn_rounded_grey));
                    } else {
                        lyNext.setBackground(ContextCompat.getDrawable(getContext(), R.drawable.btn_rounded_grey));
                    }
                }
            }
        });

        childListAdapter = new ChildInfoListAdapter(getActivity(), listChild);
        rvChild.setAdapter(childListAdapter);
        childListAdapter.SetOnItemClickListener(new ChildInfoListAdapter.OnItemClickListener() {
            @Override
            public void onDeleteClick(View view, int position, ChildInfoModel obj) {
                listChild.remove(position);
                childListAdapter.notifyDataSetChanged();
            }
        });

        prisonerListAdapter = new PrisonerInfoListAdapter(getActivity(), listPrisoner);
        rvPrisoner.setAdapter(prisonerListAdapter);
        prisonerListAdapter.SetOnItemClickListener(new PrisonerInfoListAdapter.OnItemClickListener() {
            @Override
            public void onDeleteClick(View view, int position, PrisonerInfoModel obj) {
                listPrisoner.remove(position);
                prisonerListAdapter.notifyDataSetChanged();
            }
        });

        lblLocation.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (ContextCompat.checkSelfPermission(getContext(), Manifest.permission.ACCESS_FINE_LOCATION)
                        != PackageManager.PERMISSION_GRANTED) {
                    requestPermissions("vị trí");
                    return;
                }

                progressDialog.setVisibility(View.VISIBLE);
                try {
                    locationGpsListener = new LocationGpsListener(getActivity());

                    if (locationGpsListener.isPermissionGPS) {
                        //Kiểm tra xem thiết bị đã bật định vị chưa
                        if (!locationGpsListener.isGPSEnabled) {
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
                            String provinceId = Utils.getCodeByName(listProvince, locationModel.ProvinceName);
                            if (!Utils.isEmpty(provinceId)) {
                                spnProvince.setText(locationModel.ProvinceName);
                                spnProvince.setTag(provinceId);

                                getDataDistrict(provinceId);
                                String districtId = Utils.getCodeByName(listDistrict, locationModel.DistrictName);
                                if (!Utils.isEmpty(districtId)) {
                                    spnDistrict.setText(locationModel.DistrictName);
                                    spnDistrict.setTag(districtId);

                                    getDataWrap(districtId);
                                    String wardId = Utils.getCodeByName(listWard, locationModel.WardName);
                                    if (!Utils.isEmpty(wardId)) {
                                        spnWard.setText(locationModel.WardName);
                                        spnWard.setTag(wardId);

                                        txtAddress.setText(locationModel.Address);
                                    } else {
                                        setLocation(locationModel);
                                    }
                                } else {
                                    setLocation(locationModel);
                                }
                            } else {
                                setLocation(locationModel);
                            }
                        }
                    } else {
                        // ((MainActivity) getActivity()).checkLocationPermission();
                    }
                } catch (Exception ex) {

                }
                progressDialog.setVisibility(View.GONE);
            }
        });

        /***
         * Chụp ảnh
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
                        Uri photoUri = FileProvider.getUriForFile(getActivity(), getActivity().getPackageName() + ".nts.swipesafe.provider", photoFile);
                        intent.putExtra(MediaStore.EXTRA_OUTPUT, photoUri);
                        startActivityForResult(intent, Constants.REQUEST_IMAGE_CAPTURE);
                    }
                } catch (Exception ex) {
                }
            }
        });

        /***
         * Chọn ảnh
         */
        imageChoosePhoto.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (ContextCompat.checkSelfPermission(getContext(), READ_EXTERNAL_STORAGE)
                        != PackageManager.PERMISSION_GRANTED) {
                    requestPermissions("đọc bộ nhớ");
                    return;
                }

                if (dialogGallery == null) {
                    populateImagesFromGallery();
                }

                dialogGallery.show();
            }
        });
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

    private void setLocation(LocationModel locationModel) {
        spnProvince.setText(Utils.isEmpty(locationModel.ProvinceName) ? "" : locationModel.ProvinceName);
        spnDistrict.setText(Utils.isEmpty(locationModel.DistrictName) ? "" : locationModel.DistrictName);
        spnWard.setText(Utils.isEmpty(locationModel.WardName) ? "" : locationModel.WardName);
        txtAddress.setText(Utils.isEmpty(locationModel.Address) ? "" : locationModel.Address);
    }

    private void getDataFix() {
        if (global.ListProvince == null || global.ListProvince.size() == 0) {
            String tinhJson = Utils.ReadJSONFromAsset(getContext(), "tinh_tp.json");
            listProvince = new Gson().fromJson(tinhJson, new TypeToken<ArrayList<ComboboxItem>>() {
            }.getType());
        }else {
            listProvince = new ArrayList<>();
            listProvince.addAll(global.ListProvince);
        }

        if (listProvince != null && listProvince.size() > 0) {
            arrayProvince = new String[listProvince.size()];
            int index = 0;
            for (ComboboxItem item : listProvince) {
                arrayProvince[index] = item.name;
                index++;
            }
        }
        ArrayAdapter adapterProvince = new ArrayAdapter(getContext(), R.layout.item_filter, arrayProvince);
        spnProvince.setAdapter(adapterProvince);

        if (global.ListRelationship == null || global.ListRelationship.size() == 0) {
            String qhJson = Utils.ReadJSONFromAsset(getContext(), "moi_quan_he.json");
            listRelationship = new Gson().fromJson(qhJson, new TypeToken<ArrayList<ComboboxItem>>() {
            }.getType());
        }else {
            listRelationship = new ArrayList<>();
            listRelationship.addAll(global.ListRelationship);
        }

        if (listRelationship != null && listRelationship.size() > 0) {
            arrayRelationship = new String[listRelationship.size()];
            int index = 0;
            for (ComboboxItem item : listRelationship) {
                arrayRelationship[index] = item.name;
                index++;
            }
        }
    }

    private void getDataDistrict(String tinhId) {
        String huyenJson = Utils.ReadJSONFromAsset(getContext(), "quan-huyen/" + tinhId + ".json");
        listDistrict = new Gson().fromJson(huyenJson, new TypeToken<ArrayList<ComboboxItem>>() {
        }.getType());

        if (listDistrict != null && listDistrict.size() > 0) {
            arrayDistrict = new String[listDistrict.size()];
            int index = 0;
            for (ComboboxItem item : listDistrict) {
                arrayDistrict[index] = item.name;
                index++;
            }
        }
        ArrayAdapter adapterDistrict = new ArrayAdapter(getContext(), R.layout.item_filter, arrayDistrict);
        spnDistrict.setAdapter(adapterDistrict);
    }

    private void getDataWrap(String huyenId) {
        String xaJson = Utils.ReadJSONFromAsset(getContext(), "xa-phuong/" + huyenId + ".json");
        listWard = new Gson().fromJson(xaJson, new TypeToken<ArrayList<ComboboxItem>>() {
        }.getType());

        if (listWard != null && listWard.size() > 0) {
            arrayWard = new String[listWard.size()];
            int index = 0;
            for (ComboboxItem item : listWard) {
                arrayWard[index] = item.name;
                index++;
            }
        }
        ArrayAdapter adapterWard = new ArrayAdapter(getContext(), R.layout.item_filter, arrayWard);
        spnWard.setAdapter(adapterWard);
    }

    /***
     * Fill dữ liệu lên view
     * @param model
     */
    private void fillInfoChild(ReportInfoModel model) {
        try {
            txtName.setText(model.Name);
            txtPhone.setText(model.Phone);
            txtAddress.setText(model.Address);
            spnProvince.setText(model.ProvinceName);
            spnProvince.setTag(model.ProvinceId);
            spnDistrict.setText(model.DistrictName);
            spnDistrict.setTag(model.DistrictId);
            spnWard.setText(model.WardName);
            spnWard.setTag(model.WardId);
            txtDescription.setText(model.DistrictName);
        } catch (Exception ex) {
        }
    }

    /***
     * Gửi thông tin báo cáo
     */

    private void sendReport() {
        try {
            //Get thông tin
            getInfoModel();

            //Check validate
            if (!validateFrom()) {
                return;
            }

            final ReportMobileModel reportMobileModel = new ReportMobileModel();
            reportMobileModel.HoTen = reportInfoModel.Name;
            reportMobileModel.SoDienThoai = reportInfoModel.Phone;
            reportMobileModel.DiaChi = (!Utils.isEmpty(reportInfoModel.Address) ? (reportInfoModel.Address + " - ") : "") + reportInfoModel.WardName
                    + " - " + reportInfoModel.DistrictName + " - " + reportInfoModel.ProvinceName;
            reportMobileModel.NoiDung = reportInfoModel.Description;

            reportMobileModel.TreTMP = new Gson().toJson(listChild);
            reportMobileModel.DoiTuongTMP = new Gson().toJson(listPrisoner);

            if (!Utils.checkConnectedNetwork(getContext())) {
                new android.support.v7.app.AlertDialog.Builder(getActivity())
                        .setTitle("Thông báo")
                        .setIcon(R.drawable.ic_warning)
                        .setMessage("Vui lòng kiểm tra kết nối Internet/3G/Wifi để tiếp tục. Hoặc chọn 'GỬI SAU' khi có kết nối Internet/3G/Wifi trở lại.")
                        .setCancelable(false)
                        .setNegativeButton("Đóng", null)
                        .setNeutralButton("Gửi sau", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialogInterface, int i) {
                                try {
                                    SharedPreferences sharedPreferences = getActivity().getSharedPreferences(Constants.DataReportOffline, MODE_PRIVATE);
                                    SharedPreferences.Editor editor = sharedPreferences.edit();

                                    String reportOffline = sharedPreferences.getString(Constants.KeyReportOffline, "");
                                    List<ReportOfflineModel> listReportOffline = null;
                                    if (!Utils.isEmpty(reportOffline)) {
                                        listReportOffline = new Gson().fromJson(reportOffline, new TypeToken<List<ReportOfflineModel>>() {
                                        }.getType());
                                    }

                                    if (listReportOffline == null || listReportOffline.size() == 0)
                                        listReportOffline = new ArrayList<>();

                                    ReportOfflineModel reportOfflineModel = new ReportOfflineModel();
                                    reportOfflineModel.ReportData = reportMobileModel;
                                    reportOfflineModel.ListFile = fileReport;
                                    reportOfflineModel.Area = area;

                                    listReportOffline.add(reportOfflineModel);

                                    String json = new Gson().toJson(listReportOffline);
                                    editor.putString(Constants.KeyReportOffline, json);
                                    editor.apply();
                                    ((MainActivity) getActivity()).startService();
                                    confirmDialog();
                                } catch (Exception ex) {
                                }
                            }
                        })
                        .show();
                return;
            }

            if (fileReport != null && fileReport.size() > 0) {
                progressDialog.setVisibility(View.VISIBLE);
                reportMobileModel.ListFiles = new ArrayList<>();
                ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.getUrlAPIByArea(area, LinkApi.upload));
                int index = 0;
                for (GalleryModel galleryModel : fileReport) {
                    File file = new File(galleryModel.FileUrl);
                    anRequest.addMultipartFile("file" + index, file);
                    index++;
                }
                anRequest.setPriority(Priority.MEDIUM)
                        .build()
                        .getAsJSONObject(new JSONObjectRequestListener() {
                            @Override
                            public void onResponse(JSONObject response) {
                                try {
                                    ResultModel<List<UploadFileResultModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<UploadFileResultModel>>>() {
                                    }.getType());

                                    for (UploadFileResultModel modelFile : resultObject.data) {
                                        reportMobileModel.ListFiles.add(modelFile.mappingName);
                                    }
                                } catch (Exception ex) {
                                }
                                sendReportContent(reportMobileModel);
                            }

                            @Override
                            public void onError(ANError anError) {
                                sendReportContent(reportMobileModel);
                            }
                        });
            } else {
                sendReportContent(reportMobileModel);
            }
        } catch (Exception ex) {
            progressDialog.setVisibility(View.GONE);
            Toast.makeText(getActivity(), "Lỗi phát sinh trong quá trình xử lý.", Toast.LENGTH_SHORT).show();
        }
    }

    /***
     *Đây nộji dung báo caó
     * @param reportMobileModel
     */
    private void sendReportContent(final ReportMobileModel reportMobileModel) {
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(reportMobileModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        progressDialog.setVisibility(View.VISIBLE);
        AndroidNetworking.post(Utils.getUrlAPIByArea(area, LinkApi.taomoicatuvan))
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        progressDialog.setVisibility(View.GONE);
                        confirmDialog();
                    }

                    @Override
                    public void onError(ANError anError) {
                        progressDialog.setVisibility(View.GONE);
                        if (anError.getErrorDetail().equals("connectionError")) {
                            new android.support.v7.app.AlertDialog.Builder(getActivity())
                                    .setTitle("Thông báo")
                                    .setIcon(R.drawable.ic_warning)
                                    .setMessage("Không kết nối được với hệ thống Tổng Đài Quốc Gia Bảo Vệ Trẻ Em 111. Vui lòng chọn 'GỬI SAU' khi có kết nối trở lại.")
                                    .setCancelable(false)
                                    .setNegativeButton("Đóng", null)
                                    .setNeutralButton("Gửi sau", new DialogInterface.OnClickListener() {
                                        @Override
                                        public void onClick(DialogInterface dialogInterface, int i) {
                                            try {
                                                SharedPreferences sharedPreferences = getActivity().getSharedPreferences(Constants.DataReportOffline, MODE_PRIVATE);
                                                SharedPreferences.Editor editor = sharedPreferences.edit();

                                                String reportOffline = sharedPreferences.getString(Constants.KeyReportOffline, "");
                                                List<ReportOfflineModel> listReportOffline = null;
                                                if (!Utils.isEmpty(reportOffline)) {
                                                    listReportOffline = new Gson().fromJson(reportOffline, new TypeToken<List<ReportOfflineModel>>() {
                                                    }.getType());
                                                }

                                                if (listReportOffline == null || listReportOffline.size() == 0)
                                                    listReportOffline = new ArrayList<>();

                                                ReportOfflineModel reportOfflineModel = new ReportOfflineModel();
                                                reportOfflineModel.ReportData = reportMobileModel;
                                                reportOfflineModel.ListFile = fileReport;
                                                reportOfflineModel.Area = area;

                                                listReportOffline.add(reportOfflineModel);

                                                String json = new Gson().toJson(listReportOffline);
                                                editor.putString(Constants.KeyReportOffline, json);
                                                editor.apply();
                                                ((MainActivity) getActivity()).startService();
                                                confirmDialog();
                                            } catch (Exception ex) {
                                            }
                                        }
                                    })
                                    .show();
                            return;
                        }
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    /***
     * Check validate from
     * @return
     */
    private boolean validateFrom() {
        boolean isValidate = true;
        if (Utils.isEmpty(txtPhone.getText().toString())) {
            Toast.makeText(getActivity(), "Số điện thoại liên hệ không được để trống.", Toast.LENGTH_SHORT).show();
            txtPhone.requestFocus();
            return false;
        }

        if (txtPhone.getText().toString().length() != 10 || !Utils.phoneNumberValidator(txtPhone.getText().toString())) {
            Toast.makeText(getActivity(), "Số điện thoại nhập không hợp lệ!", Toast.LENGTH_SHORT).show();
            txtPhone.requestFocus();
            return false;
        }

        if (Utils.isEmpty(spnProvince.getText().toString())) {
            Toast.makeText(getActivity(), "Chưa chọn Tỉnh/Thành nơi xảy ra.", Toast.LENGTH_SHORT).show();
            spnProvince.requestFocus();
            return false;
        }

        if (Utils.isEmpty(spnDistrict.getText().toString())) {
            Toast.makeText(getActivity(), "Chưa chọn Quận/Huyện nơi xảy ra.", Toast.LENGTH_SHORT).show();
            spnDistrict.requestFocus();
            return false;
        }

        if (Utils.isEmpty(spnWard.getText().toString())) {
            Toast.makeText(getActivity(), "Chưa chọn Xã/Phường nơi xảy ra.", Toast.LENGTH_SHORT).show();
            spnWard.requestFocus();
            return false;
        }

        if (Utils.isEmpty(txtDescription.getText().toString())) {
            Toast.makeText(getActivity(), "Nội dung báo cáo không được để trống.", Toast.LENGTH_SHORT).show();
            txtDescription.requestFocus();
            return false;
        }

        if (fileSize > 100) {
            Toast.makeText(getActivity(), "Các hình ảnh/ video gửi kèm vượt quá 100 MB.", Toast.LENGTH_SHORT).show();
            rvGalleryChoose.requestFocus();
            return false;
        }

        if (!ckConfirm.isChecked()) {
            Toast.makeText(getActivity(), "Hãy tích xác nhận trách nhiệm về toàn bộ thông tin đã khai báo ở trên trước khi gửi báo cáo.", Toast.LENGTH_SHORT).show();
            lilConfirm.requestFocus();
            return false;
        }
        return isValidate;
    }

    /***
     * Get thông tin model
     */
    private void getInfoModel() {
        reportInfoModel = new ReportInfoModel();
        reportInfoModel.Name = txtName.getText().toString();
        reportInfoModel.Phone = txtPhone.getText().toString();
        reportInfoModel.Description = txtDescription.getText().toString();
        reportInfoModel.ProvinceId = spnProvince.getTag() != null ? spnProvince.getTag().toString() : "";
        reportInfoModel.ProvinceName = spnProvince.getText().toString();
        reportInfoModel.DistrictId = spnDistrict.getTag() != null ? spnDistrict.getTag().toString() : "";
        reportInfoModel.DistrictName = spnDistrict.getText().toString();
        reportInfoModel.WardId = spnWard.getTag() != null ? spnWard.getTag().toString() : "";
        reportInfoModel.WardName = spnWard.getText().toString();
        reportInfoModel.Address = txtAddress.getText().toString();
    }

    private void clearInfoChild() {
        reportInfoModel = new ReportInfoModel();
        fillInfoChild(reportInfoModel);
        txtName.requestFocus();
    }

    /***
     * Show Dialog chọn
     * @param editText
     * @param arrayName
     * @param title
     * @param funtion
     */
    private void showChooseDialog(final EditText editText, final String[] arrayName, final List<ComboboxItem> listSource, final String title, final Runnable funtion) {
        final android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, Utils.getIndexByName(arrayName, Utils.getText(editText)), new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                editText.setText(arrayName[which]);

                String id = listSource != null ? listSource.get(which).code : "";
                editText.setTag(id);

                if (funtion != null) {
                    try {
                        funtion.run();
                    } catch (Exception ex) {
                    }
                }
            }
        });
        builder.show();
    }

    private void initDialogAddChild() {
        dialogAddChild = new Dialog(getContext());
        dialogAddChild.setContentView(R.layout.popup_add_child);
        dialogAddChild.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogAddChild.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogAddChild.setCanceledOnTouchOutside(false);

        LinearLayout lyBackChild = dialogAddChild.findViewById(R.id.lyBack);
        final LinearLayout lyNextChild = dialogAddChild.findViewById(R.id.lyNext);

        final EditText txtNameChild = dialogAddChild.findViewById(R.id.txtName);
        final EditText txtAgeChild = dialogAddChild.findViewById(R.id.txtAge);
        final EditText txtAddressChild = dialogAddChild.findViewById(R.id.txtAddress);

        TextView lblLocationChild = dialogAddChild.findViewById(R.id.lblLocation);

        final RadioGroup rgGenderChild = dialogAddChild.findViewById(R.id.rgGender);

        final EditText spnBirthdayChild = dialogAddChild.findViewById(R.id.spnBirthday);

        spnBirthdayChild.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        lyNextChild.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                lyNextChild.setEnabled(false);
                try {
                    if (Utils.isEmpty(txtNameChild.getText().toString())) {
                        toast = Toast.makeText(getActivity(), "Chưa nhập Họ và tên trẻ.", Toast.LENGTH_SHORT);
                        toast.setGravity(Gravity.CENTER, 0, 0);
                        toast.show();
                        lyNextChild.setEnabled(true);
                        return;
                    }

                    ChildInfoModel childInfoModel = new ChildInfoModel();
                    childInfoModel.id = "";
                    childInfoModel.name = txtNameChild.getText().toString();
                    int rgGenderID = rgGenderChild.getCheckedRadioButtonId();
                    RadioButton radioButton = rgGenderChild.findViewById(rgGenderID);
                    if (radioButton != null) {
                        childInfoModel.gender = radioButton.getText().toString();
                    }
                    childInfoModel.age = txtAgeChild.getText().toString();
                    childInfoModel.birthday = spnBirthdayChild.getText().toString();
                    childInfoModel.fullAddress = txtAddressChild.getText().toString();
                    listChild.add(childInfoModel);
                    childListAdapter.notifyDataSetChanged();

                    ((RadioButton) rgGenderChild.getChildAt(0)).setChecked(true);
                    txtNameChild.setText("");
                    txtAgeChild.setText("");
                    spnBirthdayChild.setText("");
                    txtAddressChild.setText("");
                    toast = Toast.makeText(getActivity(), "Thêm mới trẻ thành công!", Toast.LENGTH_SHORT);
                    toast.setGravity(Gravity.CENTER, 0, 0);
                    toast.show();
                    lyNextChild.setEnabled(true);
                } catch (Exception ex) {
                    lyNextChild.setEnabled(true);
                }
            }
        });

        lyBackChild.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialogAddChild.hide();
            }
        });

        spnBirthdayChild.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String birthday = spnBirthdayChild.getText().toString();
                SimpleDateFormat dateFormat = new SimpleDateFormat(DateUtils.DATE_FORMAT_DD_MM_YYYY);
                try {
                    if (!birthday.isEmpty() && dateFormat.parse(birthday).compareTo(dateFormat.parse(DateUtils.CurrentDate(DateUtils.DATE_FORMAT_DD_MM_YYYY))) >= 0) {
                        spnBirthdayChild.setText("");
                        toast = Toast.makeText(getActivity(), "Ngày sinh không đúng.", Toast.LENGTH_SHORT);
                        toast.setGravity(Gravity.CENTER, 0, 0);
                        toast.show();
                    }

                    if (!birthday.isEmpty()) {
                        Date dateBirthday = dateFormat.parse(birthday);
                        Calendar calendar = Calendar.getInstance();
                        calendar.setTime(dateBirthday);
                        int age = DateUtils.CurrentYear() - calendar.get(Calendar.YEAR);
                        txtAgeChild.setText(String.valueOf(age));
                        txtAgeChild.setEnabled(false);
                    }

                } catch (Exception ex) {
                    spnBirthdayChild.setText("");
                    toast = Toast.makeText(getActivity(), "Ngày sinh không đúng.", Toast.LENGTH_SHORT);
                    toast.setGravity(Gravity.CENTER, 0, 0);
                    toast.show();
                }

                birthday = spnBirthdayChild.getText().toString();
                if (birthday.isEmpty()) {
                    txtAgeChild.setText("");
                    txtAgeChild.setEnabled(true);
                }
            }
        });

        txtAgeChild.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String age = txtAgeChild.getText().toString();

                if ((!age.isEmpty() && Integer.parseInt(age) > 18) || (!age.isEmpty() && Integer.parseInt(age) < 0)) {
                    toast = Toast.makeText(getActivity(), "Tuổi ước lượng phải lớn 0 và nhỏ hơn 18 tuổi.", Toast.LENGTH_SHORT);
                    toast.setGravity(Gravity.CENTER, 0, 0);
                    toast.show();
                    txtAgeChild.setEnabled(true);
                    txtAgeChild.setText("");
                    spnBirthdayChild.setText("");
                }
            }
        });

        lblLocationChild.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                progressDialog.setVisibility(View.VISIBLE);
                try {
                    locationGpsListener = new LocationGpsListener(getActivity());
                    if (locationGpsListener.isPermissionGPS) {
                        //Kiểm tra xem thiết bị đã bật định vị chưa
                        if (!locationGpsListener.isGPSEnabled) {
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
                        if (locationModel != null && !Utils.isEmpty(locationModel.FullAddress)) {
                            txtAddressChild.setText(locationModel.FullAddress);
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

    private void initDialogAddObject() {
        dialogAddObject = new Dialog(getContext());
        dialogAddObject.setContentView(R.layout.popup_add_object);
        dialogAddObject.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogAddObject.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogAddObject.setCanceledOnTouchOutside(false);

        LinearLayout lyBackObject = dialogAddObject.findViewById(R.id.lyBack);
        final LinearLayout lyNextObject = dialogAddObject.findViewById(R.id.lyNext);

        final EditText txtNameObject = dialogAddObject.findViewById(R.id.txtName);
        final EditText txtAgeObject = dialogAddObject.findViewById(R.id.txtAge);
        final EditText txtAddressObject = dialogAddObject.findViewById(R.id.txtAddress);

        TextView lblLocationObject = dialogAddObject.findViewById(R.id.lblLocation);

        final RadioGroup rgGenderObject = dialogAddObject.findViewById(R.id.rgGender);

        final EditText spnBirthdayObject = dialogAddObject.findViewById(R.id.spnBirthday);
        final EditText spnRelationshipObject = dialogAddObject.findViewById(R.id.spnRelationship);

        spnBirthdayObject.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        spnBirthdayObject.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String birthday = spnBirthdayObject.getText().toString();
                SimpleDateFormat dateFormat = new SimpleDateFormat(DateUtils.DATE_FORMAT_DD_MM_YYYY);
                try {
                    if (!birthday.isEmpty() && dateFormat.parse(birthday).compareTo(dateFormat.parse(DateUtils.CurrentDate(DateUtils.DATE_FORMAT_DD_MM_YYYY))) >= 0) {
                        spnBirthdayObject.setText("");
                        toast = Toast.makeText(getActivity(), "Ngày sinh không đúng.", Toast.LENGTH_SHORT);
                        toast.setGravity(Gravity.CENTER, 0, 0);
                        toast.show();
                    }

                    birthday = spnBirthdayObject.getText().toString();
                    if (!birthday.isEmpty()) {
                        Date dateBirthday = dateFormat.parse(birthday);
                        Calendar calendar = Calendar.getInstance();
                        calendar.setTime(dateBirthday);
                        int age = DateUtils.CurrentYear() - calendar.get(Calendar.YEAR);
                        txtAgeObject.setText(String.valueOf(age));
                        txtAgeObject.setEnabled(false);
                    }

                } catch (Exception ex) {
                    spnBirthdayObject.setText("");
                    toast = Toast.makeText(getActivity(), "Ngày sinh không đúng.", Toast.LENGTH_SHORT);
                    toast.setGravity(Gravity.CENTER, 0, 0);
                    toast.show();
                }

                birthday = spnBirthdayObject.getText().toString();
                if (birthday.isEmpty()) {
                    txtAgeObject.setText("");
                    txtAgeObject.setEnabled(true);
                }
            }
        });

        txtAgeObject.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String age = txtAgeObject.getText().toString();

                if ((!age.isEmpty() && Integer.parseInt(age) >= 150) || (!age.isEmpty() && Integer.parseInt(age) < 0)) {
                    toast = Toast.makeText(getActivity(), "Tuổi ước lượng phải lớn 0 và nhỏ hơn 150 tuổi.", Toast.LENGTH_SHORT);
                    toast.setGravity(Gravity.CENTER, 0, 0);
                    toast.show();
                    txtAgeObject.setEnabled(true);
                    txtAgeObject.setText("");
                    spnBirthdayObject.setText("");
                }
            }
        });

        lyNextObject.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                lyNextObject.setEnabled(false);
                try {
                    if (Utils.isEmpty(txtNameObject.getText().toString())) {
                        toast = Toast.makeText(getActivity(), "Chưa nhập Họ và tên đối tượng.", Toast.LENGTH_SHORT);
                        toast.setGravity(Gravity.CENTER, 0, 0);
                        toast.show();
                        lyNextObject.setEnabled(true);
                        return;
                    }

                    PrisonerInfoModel prisonerInfoModel = new PrisonerInfoModel();
                    prisonerInfoModel.id = "";
                    prisonerInfoModel.name = txtNameObject.getText().toString();
                    int rgGenderID = rgGenderObject.getCheckedRadioButtonId();
                    RadioButton radioButton = rgGenderObject.findViewById(rgGenderID);
                    if (radioButton != null) {
                        prisonerInfoModel.gender = radioButton.getText().toString();
                    }
                    prisonerInfoModel.age = txtAgeObject.getText().toString();
                    prisonerInfoModel.birthday = spnBirthdayObject.getText().toString();
                    prisonerInfoModel.relationship = spnRelationshipObject.getText().toString();
                    prisonerInfoModel.fullAddress = txtAddressObject.getText().toString();

                    listPrisoner.add(prisonerInfoModel);
                    prisonerListAdapter.notifyDataSetChanged();

                    ((RadioButton) rgGenderObject.getChildAt(0)).setChecked(true);
                    txtNameObject.setText("");
                    txtAgeObject.setText("");
                    spnBirthdayObject.setText("");
                    spnRelationshipObject.setText("");
                    txtAddressObject.setText("");
                    toast = Toast.makeText(getActivity(), "Thêm mới đối tượng thành công!", Toast.LENGTH_SHORT);
                    toast.setGravity(Gravity.CENTER, 0, 0);
                    toast.show();
                    lyNextObject.setEnabled(true);
                } catch (Exception ex) {
                    lyNextObject.setEnabled(true);
                }
            }
        });

        lyBackObject.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialogAddObject.hide();
            }
        });

        spnRelationshipObject.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog((EditText) v, arrayRelationship, listRelationship, "Chọn mối quan hệ với trẻ", null);
            }
        });

        lblLocationObject.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                progressDialog.setVisibility(View.VISIBLE);
                try {
                    locationGpsListener = new LocationGpsListener(getActivity());
                    if (locationGpsListener.isPermissionGPS) {
                        //Kiểm tra xem thiết bị đã bật định vị chưa
                        if (!locationGpsListener.isGPSEnabled) {
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
                        if (locationModel != null && !Utils.isEmpty(locationModel.FullAddress)) {
                            txtAddressObject.setText(locationModel.FullAddress);
                        }
                    } else {
                        // ((MainActivity) getActivity()).checkLocationPermission();
                    }
                } catch (Exception ex) {

                }
                progressDialog.setVisibility(View.GONE);
            }
        });
    }

    public void confirmDialog() {
        isDismiss = true;
        final Dialog dialog = new Dialog(getContext());
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialog.setContentView(R.layout.popup_screen);
        dialog.setCancelable(true);
        LinearLayout btnBack = dialog.findViewById(R.id.lyBack);
        dialog.setOnDismissListener(new DialogInterface.OnDismissListener() {
            @Override
            public void onDismiss(DialogInterface dialogInterface) {
                if (isDismiss) {
                    Fragment fragment = new ReportMainFragment();
                    Utils.ChangeFragment(getActivity(), fragment, null);
                }
            }
        });

        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                isDismiss = false;
                Fragment fragment = new ReportMainFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
                dialog.dismiss();
            }
        });

        LinearLayout btnNext = dialog.findViewById(R.id.lyNext);
        btnNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                isDismiss = false;
                Fragment fragment = new ReportAbuseStepInfoFragment();
                Utils.ChangeFragment(getActivity(), fragment, null);
                dialog.dismiss();
            }
        });
        dialog.show();
    }

    public void confirmSecurity() {
        final Dialog dialog = new Dialog(getContext());
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialog.setContentView(R.layout.popup_info_security);
        dialog.show();

        final Handler handler = new Handler();
        handler.postDelayed(new Runnable() {
            @Override
            public void run() {
                dialog.hide();
            }
        }, 15000);
    }

    private void populateImagesFromGallery() {

        if (!mayRequestGalleryImages()) {
            return;
        }

        progressDialog.setVisibility(View.VISIBLE);
        try {
            ArrayList<GalleryModel> gallery = loadPhotosFromNativeGallery();
            initDialogGallery(gallery);
        } catch (Exception ex) {

        }
        progressDialog.setVisibility(View.GONE);
    }

    private boolean mayRequestGalleryImages() {

        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.M) {
            return true;
        }

        if (getActivity().checkSelfPermission(READ_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED) {
            return true;
        }

        if (shouldShowRequestPermissionRationale(READ_EXTERNAL_STORAGE)) {
            //promptStoragePermission();
            showPermissionRationaleSnackBar();
        } else {
            requestPermissions(new String[]{READ_EXTERNAL_STORAGE}, REQUEST_FOR_STORAGE_PERMISSION);
        }

        return false;
    }

    /**
     * Callback received when a permissions request has been completed.
     */
    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions,
                                           @NonNull int[] grantResults) {

        switch (requestCode) {

            case REQUEST_FOR_STORAGE_PERMISSION: {

                if (grantResults.length > 0) {
                    if (grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                        populateImagesFromGallery();
                    } else {
                        if (ActivityCompat.shouldShowRequestPermissionRationale(getActivity(), READ_EXTERNAL_STORAGE)) {
                            showPermissionRationaleSnackBar();
                        } else {
                            Toast.makeText(getContext(), "Go to settings and enable permission", Toast.LENGTH_LONG).show();
                        }
                    }
                }

                break;
            }
        }
    }

    /***
     * Get all thư viện
     * @return
     */
    private ArrayList<GalleryModel> loadPhotosFromNativeGallery() {
        final String[] columnsmns = {MediaStore.Images.Media.DATA, MediaStore.Images.Media._ID, MediaStore.Images.Media.DATE_TAKEN};
        String[] columns = {MediaStore.Files.FileColumns._ID,
                MediaStore.Files.FileColumns.DATA,
                MediaStore.Files.FileColumns.DATE_ADDED,
                MediaStore.Files.FileColumns.MEDIA_TYPE,
                MediaStore.Files.FileColumns.SIZE
        };

        String selection = MediaStore.Files.FileColumns.MEDIA_TYPE + "="
                + MediaStore.Files.FileColumns.MEDIA_TYPE_IMAGE
                + " OR "
                + MediaStore.Files.FileColumns.MEDIA_TYPE + "="
                + MediaStore.Files.FileColumns.MEDIA_TYPE_VIDEO;

        final String orderBy = MediaStore.Images.Media.DATE_ADDED;
        Uri queryUri = MediaStore.Files.getContentUri("external");
        Cursor imagecursor = getActivity().managedQuery(
                queryUri, columns, selection,
                null, orderBy + " DESC");

        ArrayList<GalleryModel> imageUrls = new ArrayList<GalleryModel>();
        int image_column_index = imagecursor.getColumnIndex(MediaStore.Files.FileColumns._ID);
        BitmapFactory.Options bmOptions = new BitmapFactory.Options();
        bmOptions.inSampleSize = 3;
        bmOptions.inPurgeable = true;
        GalleryModel galleryModel;
        for (int i = 0; i < imagecursor.getCount(); i++) {
            galleryModel = new GalleryModel();
            imagecursor.moveToPosition(i);
            int dataColumnIndex = imagecursor.getColumnIndex(MediaStore.Images.Media.DATA);
            galleryModel.FileUrl = imagecursor.getString(dataColumnIndex);

            int type = imagecursor.getColumnIndex(MediaStore.Files.FileColumns.MEDIA_TYPE);

            int id = imagecursor.getInt(image_column_index);
            galleryModel.Type = imagecursor.getInt(type);

            if (galleryModel.Type == 3)
                galleryModel.ThumbnailVideo = MediaStore.Video.Thumbnails.getThumbnail(
                        getActivity().getContentResolver(), id,
                        MediaStore.Video.Thumbnails.MINI_KIND, bmOptions);

            int dataColumnSize = imagecursor.getColumnIndex(MediaStore.Images.Media.SIZE);
            galleryModel.Size = imagecursor.getLong(dataColumnSize);

            imageUrls.add(galleryModel);
        }

        return imageUrls;
    }

    private void viewImageReport(final ArrayList<GalleryModel> imageUrls) {
        imageReportAdapter = new ImageReportAdapter(getContext(), imageUrls, true);

        RecyclerView.LayoutManager layoutManager = new GridLayoutManager(getContext(), 5);
        rvGalleryChoose = (RecyclerView) view.findViewById(R.id.rvGalleryChoose);
        rvGalleryChoose.setLayoutManager(layoutManager);
        rvGalleryChoose.setItemAnimator(new DefaultItemAnimator());
        rvGalleryChoose.addItemDecoration(new ItemOffsetDecoration(getContext(), R.dimen.item_offset));
        rvGalleryChoose.setAdapter(imageReportAdapter);
        calculateSize();

        imageReportAdapter.SetOnItemClickListener(new ImageReportAdapter.OnItemClickListener() {
            @Override
            public void onRemoveClick(View view, int position, GalleryModel galleryModel) {
                fileReport.remove(position);
                imageAdapter.notifyDataSetChanged();
                calculateSize();
                imageReportAdapter.notifyDataSetChanged();
            }
        });
    }

    private void showPermissionRationaleSnackBar() {
        Snackbar.make(view.findViewById(R.id.lyNext), "Cần có quyền lưu trữ để tìm nạp hình ảnh từ Thư viện.",
                Snackbar.LENGTH_INDEFINITE).setAction("OK", new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                // Request the permission
                ActivityCompat.requestPermissions(getActivity(),
                        new String[]{READ_EXTERNAL_STORAGE},
                        REQUEST_FOR_STORAGE_PERMISSION);
            }
        }).show();

    }


    /***
     * Khởi tạo modal thư viện ảnh
     */
    private void initDialogGallery(ArrayList<GalleryModel> gallery) {
        dialogGallery = new Dialog(getContext(), android.R.style.Theme_Black_NoTitleBar_Fullscreen);
        dialogGallery.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialogGallery.setContentView(R.layout.popup_gallery);

        imageAdapter = new ImageAdapter(getContext(), gallery, fileReport);

        final RecyclerView rvGallery = dialogGallery.findViewById(R.id.rvGallery);
        RecyclerView.LayoutManager layoutManager = new GridLayoutManager(getContext(), 5);
        rvGallery.setLayoutManager(layoutManager);
        rvGallery.setItemAnimator(new DefaultItemAnimator());
        rvGallery.addItemDecoration(new ItemOffsetDecoration(getContext(), R.dimen.item_offset));
        rvGallery.setAdapter(imageAdapter);

        imageAdapter.SetOnItemClickListener(new ImageAdapter.OnItemClickListener() {
            @Override
            public void onItemClick(View view, int position, boolean isCheck, GalleryModel galleryModel) {
                if (isCheck) {
                    fileReport.add(galleryModel);
                } else {
                    fileReport.remove(galleryModel);
                }
            }
        });

        final TextView txtOk = dialogGallery.findViewById(R.id.txtOk);
        txtOk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                imageReportAdapter.notifyDataSetChanged();
                dialogGallery.hide();
                calculateSize();
            }
        });

        final ImageView imBack = dialogGallery.findViewById(R.id.imBack);
        imBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialogGallery.hide();
                populateImagesFromGallery();
            }
        });
    }


    /***
     * Tính độ lớn file upload
     */
    private void calculateSize() {
        fileSize = 0;
        if (fileReport != null && fileReport.size() > 0) {
            for (int i = 0; i < fileReport.size(); i++) {
                fileSize += fileReport.get(i).Size;
            }
        }

        if (fileSize > 0 && fileSize < 1024) {
            txtFileSize.setText(" (" + String.valueOf(fileSize) + " Bytes)");
        } else if (fileSize > 0 && fileSize < 1024 * 1024) {
            txtFileSize.setText(" (" + String.valueOf(fileSize / 1024) + " KB)");
        } else {
            txtFileSize.setText(" (" + String.valueOf((fileSize / 1024) / 1024) + " MB)");
        }
        fileSize = (fileSize / 1024) / 1024;
    }

    public void hideSoftKeyboard() {
        getActivity().getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
    }
}
