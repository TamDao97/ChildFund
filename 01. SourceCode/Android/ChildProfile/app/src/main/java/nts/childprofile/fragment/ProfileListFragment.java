package nts.childprofile.fragment;

import android.Manifest;
import android.app.Activity;
import android.app.Dialog;
import android.app.DownloadManager;
import android.app.ProgressDialog;
import android.content.BroadcastReceiver;
import android.content.ContentValues;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.os.Handler;
import android.os.StatFs;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.FileProvider;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.AbsListView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
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
import java.io.IOException;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import nts.childprofile.R;
import nts.childprofile.ScanActivity;
import nts.childprofile.Sql.DataBaseHelper;
import nts.childprofile.adapter.ImageAdapter;
import nts.childprofile.adapter.ImageReportAdapter;
import nts.childprofile.adapter.ProfileListAdapter;
import nts.childprofile.common.Constants;
import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.model.ChildProfileModel;
import nts.childprofile.model.ChildProfileSearchCondition;
import nts.childprofile.model.ChildProfileSearchResult;
import nts.childprofile.model.ComboboxResult;
import nts.childprofile.model.DownloadSearchModel;
import nts.childprofile.model.DownloadSearchResultModel;
import nts.childprofile.model.GalleryModel;
import nts.childprofile.model.ImageChildByYearModel;
import nts.childprofile.model.LoginProfileModel;
import nts.childprofile.model.ReportProfilesModel;
import nts.childprofile.model.SearchResultObject;

import static android.Manifest.permission.READ_EXTERNAL_STORAGE;

/**
 * A fragment representing a list of Items.
 * interface.
 */
public class ProfileListFragment extends Fragment {
    private View view;
    private SharedPreferences sharedPreferencesDataFix;
    private ProgressDialog progressDialog;
    private ListView listView;
    private ProfileListAdapter profileListAdapter;
    private TextView item_total, price_total;
    private LinearLayout lyt_notfound, lySearch, lyScan, lyDownload, lyStorage, lySpaceStorage;
    private List<ChildProfileSearchResult> listProfile = new ArrayList<ChildProfileSearchResult>();
    private ChildProfileSearchCondition childProfileSearchCondition;
    Dialog dialog;
    private GlobalVariable global;
    private LoginProfileModel loginProfileModel;
    private ProfileListAdapter adapter = null;
    public int currentPage;
    boolean userScrolled = false;
    private ArrayList<String> arrayRepoetContent = new ArrayList<>();
    private static RelativeLayout bottomLayout;
    private static RelativeLayout downloadProgress;

    private RecyclerView rvGalleryChoose;
    private ImageReportAdapter imageReportAdapter;
    private ImageAdapter imageAdapter;
    private static final int REQUEST_FOR_STORAGE_PERMISSION = 123;
    private Dialog dialogGallery;
    private ImageView imageCaptureEngine, imageChoosePhoto;
    private String imageLink;
    private ArrayList<GalleryModel> fileUpload = new ArrayList<>();
    private ArrayList<GalleryModel> gallery;

    private EditText spnContent, txtDescription, txtContentOther;
    private LinearLayout tilContentOther;
    ///Id hồ sơ trẻ dùng cho báo cáo chuyển biến
    private String childProfileId = "";
    private List<String> listCheckedId = new ArrayList<>();
    private TextView item_total_size, item_free_storage;
    private ImageButton btnDownload;
    private double totalSize;
    long megAvailable;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_profile_list, null);

        Toolbar mToolbar = (Toolbar) getActivity().findViewById(R.id.toolbar);
        TextView toolbarTitle = mToolbar.findViewById(R.id.toolbar_title);
        TextView txtOffline = mToolbar.findViewById(R.id.txtOffline);
        toolbarTitle.setText("Tìm kiếm trẻ/Search");
        Toolbar.LayoutParams layoutParams = (Toolbar.LayoutParams) toolbarTitle.getLayoutParams();
        layoutParams.gravity = Gravity.LEFT;
        toolbarTitle.setLayoutParams(layoutParams);

        global = (GlobalVariable) getActivity().getApplication();
        loginProfileModel = global.getLoginProfile();
        sharedPreferencesDataFix = getActivity().getSharedPreferences(Constants.Childprofile_Data_Fix, Context.MODE_PRIVATE);

        item_total = (TextView) view.findViewById(R.id.item_total);
        lyt_notfound = (LinearLayout) view.findViewById(R.id.lyt_notfound);
        listView = (ListView) view.findViewById(R.id.listView);
        lySearch = (LinearLayout) view.findViewById(R.id.lySearch);
        lyScan = (LinearLayout) view.findViewById(R.id.lyScan);

        progressDialog = new ProgressDialog(getActivity());
        bottomLayout = (RelativeLayout) view.findViewById(R.id.loadItemsLayout_listView);
        downloadProgress = (RelativeLayout) view.findViewById(R.id.downloadProgress);

        childProfileSearchCondition = new ChildProfileSearchCondition();
        childProfileSearchCondition.OrderType = false;
        childProfileSearchCondition.OrderBy = "CreateDate";
        childProfileSearchCondition.PageNumber = 1;
        childProfileSearchCondition.PageSize = 15;
        childProfileSearchCondition.UserId = loginProfileModel.Id;

        listProfile = new ArrayList<ChildProfileSearchResult>();

        item_total_size = view.findViewById(R.id.item_total_size);
        item_free_storage = view.findViewById(R.id.item_free_storage);
        lyStorage = view.findViewById(R.id.lyStorage);
        lySpaceStorage = view.findViewById(R.id.lySpaceStorage);
        StatFs stat = new StatFs(Environment.getExternalStorageDirectory().getPath());
        long bytesAvailable;
        bytesAvailable = stat.getBlockSizeLong() * stat.getAvailableBlocksLong();
        megAvailable = bytesAvailable / (1024 * 1024);
        item_free_storage.setText(String.valueOf(megAvailable) + " mb");
        profileListAdapter = new ProfileListAdapter(getActivity(), listProfile, global.isStatusRunApp());
        profileListAdapter.notifyDataSetChanged();
        listView.setAdapter(profileListAdapter);
        if (!global.isStatusRunApp()) {
            populateListView();
            implementScrollListener();
        } else {
            searchChildProfileOffline(childProfileSearchCondition);
            txtOffline.setVisibility(View.VISIBLE);
        }

        final String dataProfileDraft = sharedPreferencesDataFix.getString(Constants.Key_Data_Profile_Draft_Update, null);
        if (dataProfileDraft != null) {
            new AlertDialog.Builder(getActivity())
                    .setMessage("Bạn có một hồ sơ trẻ lưu nháp. \n Chọn Đồng ý/Ok để tiếp tục với hồ sơ trẻ lưu nháp. \n Chọn Hủy/Cancel bản lưu nháp tự động xóa và bắt đầu một hồ sơ mới?")
                    .setCancelable(false)
                    .setPositiveButton("Đồng ý/Ok", new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int id) {
//                            childProfileModel = new Gson().fromJson(dataProfileDraft, ChildProfileModel.class);
//                            setDataProfileDraft();
                            Fragment updateProfileChildFragment = new UpdateProfileChildFragment();
                            Bundle bundle = new Bundle();
                            bundle.putString("Id", Constants.DraftUpdate);
                            Utils.ChangeFragment(getActivity(), updateProfileChildFragment, bundle);
                        }
                    })
                    .setNegativeButton("Hủy/Cancel", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialogInterface, int i) {
                            SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                            editor.remove(Constants.Key_Data_Profile_Draft_Update);
                            editor.apply();
                        }
                    })
                    .show();
        }

        profileListAdapter.SetOnItemClickListener(new ProfileListAdapter.OnItemClickListener() {
            @Override
            public void onButtonEditClick(View view, int position, ChildProfileSearchResult obj) {
                Fragment updateProfileChildFragment = new UpdateProfileChildFragment();
                Bundle bundle = new Bundle();
                bundle.putString("Id", obj.Id);
                Utils.ChangeFragment(getActivity(), updateProfileChildFragment, bundle);
            }

            @Override
            public void onButtonReportClick(View view, final int position, ChildProfileSearchResult obj) {
                showReportProfiles(obj.Id);
            }

            @Override
            public void onSortClick(View view, String columnName) {
                childProfileSearchCondition.OrderType = !childProfileSearchCondition.OrderType;
                childProfileSearchCondition.OrderBy = columnName;
                if (global.isStatusRunApp()) {
                    searchChildProfileOffline(childProfileSearchCondition);
                } else {
                    SearchChildProfile(childProfileSearchCondition);
                }
            }

            @Override
            public void onCaptureClick(View view, String id) {
                try {
                    if (ContextCompat.checkSelfPermission(getContext(), Manifest.permission.CAMERA)
                            != PackageManager.PERMISSION_GRANTED) {
                        requestPermissions("camera");
                        return;
                    }
                    childProfileId = id;
                    Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
                    // Create the File where the photo should go
                    File photoFile = null;
                    try {
                        photoFile = Utils.CreateImageFile(getActivity());
                    } catch (IOException ex) {
                    }
                    // Continue only if the File was successfully created
                    if (photoFile != null) {
                        final int position = view.getId();
                        imageLink = photoFile.getAbsolutePath();
                        Uri photoUri = FileProvider.getUriForFile(getActivity(), getActivity().getPackageName() + ".nts.childprofile.provider", photoFile);
                        intent.putExtra(MediaStore.EXTRA_OUTPUT, photoUri);
                        startActivityForResult(intent, Constants.REQUEST_IMAGE_CAPTURE_CHANGE);
                    }
                } catch (Exception ex) {
                }
            }

            @Override
            public void onCheckbox(View view, ChildProfileSearchResult obj, boolean isCheck) {
                if (isCheck) {
                    listCheckedId.add(obj.Id);
                    totalSize = totalSize + obj.TotalSize;
                } else {
                    listCheckedId.remove(obj.Id);
                    totalSize = totalSize - obj.TotalSize;
                }
                DecimalFormat twoDForm = new DecimalFormat("#.##");
                item_total_size.setText(String.valueOf(twoDForm.format(totalSize)) + " mb");
                if (listCheckedId.size() > 0) {
                    lyDownload.setVisibility(View.VISIBLE);
                    lyScan.setVisibility(View.GONE);
                    lyStorage.setVisibility(View.VISIBLE);
                    lySpaceStorage.setVisibility(View.VISIBLE);
                } else {
                    lyDownload.setVisibility(View.GONE);
                    lyScan.setVisibility(View.VISIBLE);
                    lyStorage.setVisibility(View.GONE);
                    lySpaceStorage.setVisibility(View.GONE);
                }
            }
        });


        String dataFixRepoetContent = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Repoet_Content, null);
        if (dataFixRepoetContent != null) {
            List<ComboboxResult> listRepoetContent = new Gson().fromJson(dataFixRepoetContent, new TypeToken<List<ComboboxResult>>() {
            }.getType());
            for (ComboboxResult model : listRepoetContent) {
                arrayRepoetContent.add(model.Name);
            }
        }

        lySearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DialogSearch();
            }
        });

        lyScan.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (ContextCompat.checkSelfPermission(getContext(), Manifest.permission.CAMERA)
                        != PackageManager.PERMISSION_GRANTED) {
                    requestPermissions("camera");
                    return;
                }

                Intent mainActivity = new Intent(getActivity().getApplicationContext(), ScanActivity.class);
                startActivityForResult(mainActivity, 1);
            }
        });

        getListChildProfileSQLite();
//        File imageOriginal = new File("/storage/emulated/0/Download", "ChildProfileImageOriginal");
//        File thumbnailPath = new File("/storage/emulated/0/Download", "ChildProfileThumbnailPath");
//        if (!imageOriginal.exists()) {
//            imageOriginal.mkdirs();
//        }
//        if (!thumbnailPath.exists()) {
//            thumbnailPath.mkdirs();
//        }

        lyDownload = view.findViewById(R.id.lyDownload);
        lyDownload.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                downloadInfoChildProfile();
            }
        });

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
                if (resultCode == -1) {
                    Utils.AutoRotateImage(imageLink);

                    //Resize ảnh
                    //File file = Utils.ResizeImages(getContext(), imageLink);

                    GalleryModel galleryModel = new GalleryModel();
                    galleryModel.FileUrl = imageLink;
                    galleryModel.Type = 1;
                    fileUpload.add(galleryModel);
                    viewImageReport(fileUpload);
                }

            } else if (requestCode == Constants.REQUEST_IMAGE_CAPTURE_CHANGE) {
                ImageChildByYearModel model = new ImageChildByYearModel();
                model.ChildProfileId = childProfileId;
                String jsonModel = new Gson().toJson(model);
                ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/ChildProfiles/AddImageChilByYear"));

                if (!imageLink.isEmpty()) {

                    //Xoay ảnh
                    Utils.AutoRotateImage(imageLink);

                    //Resize ảnh
                    //File file = Utils.ResizeImages(getContext(), imageLink);

                    File file = new File(imageLink);
                    anRequest.addMultipartFile("Avatar", file);
                }

                progressDialog.setMessage("Đang tải ảnh lên...");
                progressDialog.show();

                if (global.isStatusRunApp()) {
                    DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
                    ContentValues values = new ContentValues();
                    values.put("ChildProfileId", childProfileId);
                    values.put("ImageUrl", imageLink);
                    dataBaseHelper.createImageCPR(values);
                    progressDialog.hide();
                    Toast.makeText(getActivity(), "Gửi ảnh báo cáo chuyển biến trẻ thành công!", Toast.LENGTH_SHORT).show();
                } else {
                    anRequest.addMultipartParameter("Model", jsonModel)
                            .setPriority(Priority.MEDIUM)
                            .build()
                            .getAsString(new StringRequestListener() {
                                @Override
                                public void onResponse(String response) {
                                    progressDialog.hide();
                                    Toast.makeText(getActivity(), "Gửi ảnh báo cáo chuyển biến trẻ thành công!", Toast.LENGTH_SHORT).show();
                                }

                                @Override
                                public void onError(ANError anError) {
                                    progressDialog.hide();
                                    Utils.showErrorMessage(getActivity().getApplication(), anError);
                                }
                            });
                }

            } else if (requestCode == 1) {
                if (resultCode == Activity.RESULT_OK) {
                    String childCode = data.getStringExtra("Barcode");
                    childProfileSearchCondition.ChildCode = childCode;
                    if (global.isStatusRunApp()) {
                        searchChildProfileOffline(childProfileSearchCondition);
                    } else {
                        populateListView();
                    }
                }
            }
            super.onActivityResult(requestCode, resultCode, data);
        } catch (Exception ex) {
        }
    }

    /***
     * Show Dialog chọn item
     * @param bt
     * @param arrayName
     */
    private void showChoiceDialog(final EditText bt, final String[] arrayName, final String title) {
        final AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, Utils.getIndexByName(arrayName, Utils.getText(bt)), new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                bt.setTextColor(Color.BLACK);
                bt.setText(arrayName[which]);

                if (title.equals("Nội dung báo cáo/Content")) {
                    if (bt.getText().toString().equals("Khác/Other")) {
                        tilContentOther.setVisibility(View.VISIBLE);
                        txtContentOther.requestFocus();
                    } else {
                        tilContentOther.setVisibility(View.GONE);
                    }
                }
            }
        });
        builder.show();
    }

    private EditText condition_Code;
    private EditText condition_Name;
    private EditText condition_Address;

    private void DialogSearch() {
        final Dialog dialog = new Dialog(getContext());
        condition_Code = dialog.findViewById(R.id.search_Code);
        condition_Name = dialog.findViewById(R.id.search_Name);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.setContentView(R.layout.popup_search_child);
        dialog.show();
        condition_Name = dialog.findViewById(R.id.search_Name);
        condition_Code = dialog.findViewById(R.id.search_Code);
        condition_Address = dialog.findViewById(R.id.search_Address);
        condition_Code.setText(childProfileSearchCondition.ChildCode);
        condition_Name.setText(childProfileSearchCondition.Name);
        condition_Address.setText(childProfileSearchCondition.Address);

        Button btn_Search = dialog.findViewById(R.id.btn_OkSearch);
        Button btn_Exit = dialog.findViewById(R.id.btn_Exit);
        btn_Exit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialog.dismiss();
            }
        });
        btn_Search.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                childProfileSearchCondition.ChildCode = condition_Code.getText().toString();
                childProfileSearchCondition.Name = condition_Name.getText().toString();
                childProfileSearchCondition.Address = condition_Address.getText().toString();
                if (global.isStatusRunApp()) {
                    searchChildProfileOffline(childProfileSearchCondition);
                } else {
                    populateListView();

                }
                dialog.dismiss();
            }
        });
    }

    private void SearchChildProfile(final ChildProfileSearchCondition childProfileSearchCondition) {

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(childProfileSearchCondition));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.GetUrlApi("api/ChildProfiles/SearchChilldProfile"))
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            SearchResultObject<ChildProfileSearchResult> resultObject = new Gson().fromJson(response.toString(), new TypeToken<SearchResultObject<ChildProfileSearchResult>>() {
                            }.getType());
                            item_total.setText(String.valueOf(resultObject.TotalItem));
                            //set data and list adapter

                            if (resultObject.ListResult.size() > 0) {
                                if (childProfileSearchCondition.PageNumber == 1) {
                                    listProfile.clear();
                                }

//                                if (listChildProfileIdSQLite.size() > 0) {
                                for (ChildProfileSearchResult item : resultObject.ListResult) {
                                    for (String id : listChildProfileIdSQLite) {
                                        if (item.Id.equals(id)) {
                                            item.isDownload = true;
                                        }
                                    }
                                    if (listCheckedId.size() > 0) {
                                        for (String checkedId : listCheckedId) {
                                            if (item.Id.equals(checkedId)) {
                                                item.isCheck = true;
                                            }
                                        }
                                    }
                                }
//                                }


                                listProfile.addAll(resultObject.ListResult);
                                profileListAdapter.notifyDataSetChanged();
                            } else if (resultObject.TotalItem == 0) {
                                if (listProfile.size() > 0) {
                                    listProfile.clear();
                                    profileListAdapter.notifyDataSetChanged();
                                }
                            }

                            if (listProfile.size() > 0) {
                                lyt_notfound.setVisibility(View.GONE);
                            } else {
                                lyt_notfound.setVisibility(View.VISIBLE);
                            }
                            bottomLayout.setVisibility(View.GONE);
                        } catch (Exception ex) {

                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

    private void populateListView() {
        this.childProfileSearchCondition.PageNumber = 1;
        SearchChildProfile(this.childProfileSearchCondition);

    }

    private void implementScrollListener() {
        listView.setOnScrollListener(new AbsListView.OnScrollListener() {

            @Override
            public void onScrollStateChanged(AbsListView arg0, int scrollState) {
                // If scroll state is touch scroll then set userScrolled
                // true
                if (scrollState == AbsListView.OnScrollListener.SCROLL_STATE_TOUCH_SCROLL) {
                    userScrolled = true;
                }
            }

            @Override
            public void onScroll(AbsListView view, int firstVisibleItem,
                                 int visibleItemCount, int totalItemCount) {
                // Now check if userScrolled is true and also check ifté
                // the item is end then update list view and set
                // userScrolled to false
                if (userScrolled) {
                    // thực hiện load thêm dữ liệu mới khi người dùng đang scroll ở dòng cuối cùng của listview
                    if (firstVisibleItem + visibleItemCount == totalItemCount) {
                        updateListView();
                        userScrolled = false;

                        // Trường hợp khi người dùng load tới dòng trên cùng, thì lấy lại dữ liệu mới
                    } else if (firstVisibleItem == 0 && totalItemCount > 15) {

                        populateListView();

                        userScrolled = false;
                    }
                }
            }
        });
    }

    private void updateListView() {
        // Show Progress Layout
        bottomLayout.setVisibility(View.VISIBLE);

        new Handler().postDelayed(new Runnable() {

            @Override
            public void run() {
                childProfileSearchCondition.PageNumber++;
                SearchChildProfile(childProfileSearchCondition);
                // After adding new data hide the view.
            }
        }, 100);
    }

    /***
     * Show giao diện sửa
     * @param id hồ sơ trẻ
     */
    private void showReportProfiles(final String id) {
        if (id == null || id.isEmpty()) {
            Toast.makeText(getActivity(), "Hãy chọn trẻ trước khi báo cáo!", Toast.LENGTH_SHORT);
            return;
        }

        final Dialog dialogReportProfiles = new Dialog(getContext());
        dialogReportProfiles.setContentView(R.layout.popup_report_profiles);
        dialogReportProfiles.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));

        spnContent = dialogReportProfiles.findViewById(R.id.spnContent);
        txtDescription = dialogReportProfiles.findViewById(R.id.txtDescription);
        txtContentOther = dialogReportProfiles.findViewById(R.id.txtContentOther);
        tilContentOther = dialogReportProfiles.findViewById(R.id.tilContentOther);

        final Button btnAdd = dialogReportProfiles.findViewById(R.id.btnAdd);
        Button btnClose = dialogReportProfiles.findViewById(R.id.btnClose);
        final ProgressBar progressBar = dialogReportProfiles.findViewById(R.id.progressBar);

        imageChoosePhoto = (ImageView) dialogReportProfiles.findViewById(R.id.imageChoosePhoto);
        imageCaptureEngine = (ImageView) dialogReportProfiles.findViewById(R.id.imageCaptureEngine);

        rvGalleryChoose = (RecyclerView) dialogReportProfiles.findViewById(R.id.rvGalleryChoose);
        fileUpload = new ArrayList<>();
        viewImageReport(fileUpload);

        spnContent.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showChoiceDialog((EditText) view, arrayRepoetContent.toArray(new String[0]), "Nội dung báo cáo/Content");
            }
        });

        DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
        ReportProfilesModel reportProfileOld = dataBaseHelper.getReportInfo(id);

        if (reportProfileOld != null) {
            spnContent.setText(reportProfileOld.Content);
            txtDescription.setText(reportProfileOld.Description);
            String[] listFileUrl = new String[0];
            if (!Utils.isEmpty(reportProfileOld.Url)) {
                listFileUrl = reportProfileOld.Url.split(";");
            }
            if (listFileUrl.length > 0) {
                for (int i = 1; i < listFileUrl.length; i++) {
                    if (!Utils.isEmpty(listFileUrl[i])) {
                        File file = new File(listFileUrl[i]);
                        GalleryModel galleryModel = new GalleryModel();
                        galleryModel.FileUrl = listFileUrl[i];
                        galleryModel.Type = 1;
                        fileUpload.add(galleryModel);
                    }
                }
                viewImageReport(fileUpload);
            }
        }

        btnAdd.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                progressBar.setVisibility(View.VISIBLE);
                btnAdd.setVisibility(View.GONE);

                ReportProfilesModel reportProfilesModel = new ReportProfilesModel();
                //Lấy giá trị vào model
                reportProfilesModel.Status = Constants.ReportProfilesNew;
                reportProfilesModel.IsDelete = Constants.ProfilesIsUse;
                reportProfilesModel.CreateBy = loginProfileModel.Id;
                reportProfilesModel.UpdateBy = loginProfileModel.Id;
                reportProfilesModel.Content = spnContent.getText().toString();
                if (spnContent.getText().toString().equals("Khác/Other")) {
                    reportProfilesModel.Content = txtContentOther.getText().toString();
                }
                reportProfilesModel.Description = txtDescription.getText().toString();
                reportProfilesModel.ChildProfileId = id;

                //Check validate
                if (reportProfilesModel.Content.isEmpty()) {
                    Toast.makeText(getActivity(), "Hãy lựa chọn nội dung báo cáo!", Toast.LENGTH_SHORT);
                    spnContent.requestFocus();
                    return;
                }

                if (global.isStatusRunApp()) {
                    String fileUrl = "";
                    ContentValues contentValues = new ContentValues();
                    contentValues.put("ChildProfileId", reportProfilesModel.ChildProfileId);
                    contentValues.put("ProcessStatus", Constants.ProfilesNew);
                    contentValues.put("Content", reportProfilesModel.Content);
                    contentValues.put("IsDelete", Constants.ProfilesIsUse);
                    contentValues.put("Description", reportProfilesModel.Description);
                    contentValues.put("UpdateBy", reportProfilesModel.UpdateBy);
                    contentValues.put("CreateBy", reportProfilesModel.CreateBy);

                    //lấy đường dẫn ảnh
                    for (int i = 0; i < fileUpload.size(); i++) {
                        fileUrl = fileUrl + ';' + fileUpload.get(i).FileUrl;
                    }
                    contentValues.put("Url", fileUrl);

                    DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
                    dataBaseHelper.insertReportChildProfile(Constants.DATABASE_TABLE_ReportProfile, contentValues);
                    dialogReportProfiles.dismiss();
                    Toast.makeText(getActivity(), "Báo cáo thay đổi tình trạng của trẻ thành công.", Toast.LENGTH_SHORT).show();
                } else {
                    String jsonModel = new Gson().toJson(reportProfilesModel);
                    ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/ChildProfiles/AddReportProfile"));
                    for (int i = 0; i < fileUpload.size(); i++) {
                        File file = new File(fileUpload.get(i).FileUrl);
                        anRequest.addMultipartFile("file" + i, file);
                    }
                    anRequest.addMultipartParameter("Model", jsonModel)
                            .setPriority(Priority.MEDIUM)
                            .build()
                            .getAsString(new StringRequestListener() {
                                @Override
                                public void onResponse(String response) {
                                    progressBar.setVisibility(View.GONE);
                                    btnAdd.setVisibility(View.VISIBLE);
                                    dialogReportProfiles.dismiss();
                                    Toast.makeText(getActivity(), "Báo cáo thay đổi tình trạng của trẻ thành công.", Toast.LENGTH_SHORT).show();
                                }

                                @Override
                                public void onError(ANError anError) {
                                    progressBar.setVisibility(View.GONE);
                                    btnAdd.setVisibility(View.VISIBLE);
                                    Utils.showErrorMessage(getActivity().getApplication(), anError);
                                }
                            });
                }
            }
        });

        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                spnContent.setText("");
                txtDescription.setText("");
                dialogReportProfiles.dismiss();
            }
        });
        dialogReportProfiles.show();

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

                    if (!mayRequestGalleryImages()) {
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

                initDialogGallery(gallery);

                dialogGallery.show();
            }
        });
    }

    private void populateImagesFromGallery() {
        if (!mayRequestGalleryImages()) {
            return;
        }

        gallery = loadPhotosFromNativeGallery();
    }

    private boolean mayRequestGalleryImages() {

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

    private ArrayList<GalleryModel> loadPhotosFromNativeGallery() {
        final String[] columnsmns = {MediaStore.Images.Media.DATA, MediaStore.Images.Media._ID, MediaStore.Images.Media.DATE_TAKEN};
        String[] columns = {MediaStore.Files.FileColumns._ID,
                MediaStore.Files.FileColumns.DATA,
                MediaStore.Files.FileColumns.DATE_ADDED,
                MediaStore.Files.FileColumns.MEDIA_TYPE
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

            imageUrls.add(galleryModel);
        }

        return imageUrls;
    }

    private void viewImageReport(final ArrayList<GalleryModel> imageUrls) {
        imageReportAdapter = new ImageReportAdapter(getContext(), imageUrls, true);

        RecyclerView.LayoutManager layoutManager = new GridLayoutManager(getContext(), 3);
        rvGalleryChoose.setLayoutManager(layoutManager);
        rvGalleryChoose.setItemAnimator(new DefaultItemAnimator());
        rvGalleryChoose.addItemDecoration(new ItemOffsetDecoration(getContext(), R.dimen.item_offset));
        rvGalleryChoose.setAdapter(imageReportAdapter);

        imageReportAdapter.SetOnItemClickListener(new ImageReportAdapter.OnItemClickListener() {
            @Override
            public void onRemoveClick(View view, int position, GalleryModel galleryModel) {
                fileUpload.remove(galleryModel);
                imageReportAdapter.notifyItemRemoved(position);
//                imageAdapter.notifyDataSetChanged();
            }

            @Override
            public void showFullImage(View view, int position, GalleryModel galleryModel) {
                Intent intent = new Intent(getActivity().getApplication(), ActivityShowFullImage.class);
                String urlImage = galleryModel.FileUrl;
                intent.putExtra("urlShowFullImage", urlImage);
                startActivity(intent);
            }
        });
    }

    /***
     * Khởi tạo modal thư viện ảnh
     */
    private void initDialogGallery(ArrayList<GalleryModel> gallery) {
        try {
            dialogGallery = new Dialog(getContext(), android.R.style.Theme_Black_NoTitleBar_Fullscreen);
            dialogGallery.requestWindowFeature(Window.FEATURE_NO_TITLE);
            dialogGallery.setContentView(R.layout.popup_gallery);

            imageAdapter = new ImageAdapter(getContext(), gallery, fileUpload);

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
                        fileUpload.add(galleryModel);
                    } else {
                        fileUpload.remove(galleryModel);
                    }
                }
            });

            final TextView txtOk = dialogGallery.findViewById(R.id.txtOk);
            txtOk.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    imageReportAdapter.notifyDataSetChanged();
                    dialogGallery.hide();
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
        } catch (Exception ex) {
            String temp = "";
        }
    }

    public void backMenu() {
        Fragment fragment = new MenuFragment();
        Utils.ChangeFragment(getActivity(), fragment, null);
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


    //    Lấy dữ liệu trẻ từ SQL Server
    private void downloadInfoChildProfile() {
        if (megAvailable <= totalSize) {
            Toast.makeText(getActivity(), "Không đủ dung lượng trống để tải hồ sơ", Toast.LENGTH_SHORT).show();
            return;
        }
        String request = "api/ChildProfiles/GetChildProfiles";
        DownloadSearchModel downloadSearchModel = new DownloadSearchModel();
        downloadSearchModel.ListChildProfileId = listCheckedId;
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(downloadSearchModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.GetUrlApi(request))
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            SearchResultObject<DownloadSearchResultModel> resultObject = new Gson().fromJson(response.toString(), new TypeToken<SearchResultObject<DownloadSearchResultModel>>() {
                            }.getType());
//                            item_total.setText(String.valueOf(resultObject.TotalItem));
                            //set data and list adapter

                            if (resultObject.ListResult.size() > 0) {
                                createChildProfileToSQLite(resultObject.ListResult);
                            } else if (resultObject.TotalItem == 0) {

                            }
                        } catch (Exception ex) {
                            String a = ";";
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

    //    Thêm trẻ trong danh sách tải vào SQLite
    private void createChildProfileToSQLite(List<DownloadSearchResultModel> listProfile) {
//        File imageOriginal = new File("/storage/emulated/0/Download", "ChildProfileImageOriginal");
//        File thumbnailPath = new File("/storage/emulated/0/Download", "ChildProfileThumbnailPath");
//        if (!imageOriginal.exists()) {
//            imageOriginal.mkdirs();
//        }
//        if (!thumbnailPath.exists()) {
//            thumbnailPath.mkdirs();
//        }

        for (DownloadSearchResultModel item : listProfile) {
            ContentValues values = new ContentValues();
            values.put("Id", item.Id);
            values.put("InfoDate", item.InfoDate);
            values.put("EmployeeName", item.EmployeeName);
            values.put("ProgramCode", item.ProgramCode);
            values.put("ProvinceId", item.ProvinceId);
            values.put("DistrictId", item.DistrictId);
            values.put("WardId", item.WardId);
            values.put("Address", item.Address);
            values.put("FullAddress", item.FullAddress);
            values.put("ChildCode", item.ChildCode);
            values.put("SchoolId", item.SchoolId);
            values.put("SchoolOtherName", item.SchoolOtherName);
            values.put("SchoolName", item.School);
            values.put("EthnicId", item.EthnicId);
            values.put("ReligionId", item.ReligionId);
            values.put("Name", item.Name);
            values.put("NickName", item.NickName);
            values.put("Gender", item.Gender);
            values.put("DateOfBirth", item.DateOfBirth);
            values.put("LeaningStatus", item.LeaningStatus);
            values.put("ClassInfo", item.ClassInfo);
            values.put("FavouriteSubject", item.FavouriteSubject);
            values.put("LearningCapacity", item.LearningCapacity);
            values.put("Housework", item.Housework);
            values.put("Health", item.Health);
            values.put("Personality", item.Personality);
            values.put("Hobby", item.Hobby);
            values.put("Dream", item.Dream);
            values.put("FamilyMember", item.FamilyMember);
            values.put("LivingWithParent", item.LivingWithParent);
            values.put("NotLivingWithParent", item.NotLivingWithParent);
            values.put("LivingWithOther", item.LivingWithOther);
            values.put("LetterWrite", item.LetterWrite);
            values.put("HouseType", item.HouseType);
            values.put("HouseRoof", item.HouseRoof);
            values.put("HouseWall", item.HouseWall);
            values.put("HouseFloor", item.HouseFloor);
            values.put("UseElectricity", item.UseElectricity);
            values.put("SchoolDistance", item.SchoolDistance);
            values.put("ClinicDistance", item.ClinicDistance);
            values.put("WaterSourceDistance", item.WaterSourceDistance);
            values.put("WaterSourceUse", item.WaterSourceUse);
            values.put("RoadCondition", item.RoadCondition);
            values.put("IncomeFamily", item.IncomeFamily);
            values.put("HarvestOutput", item.HarvestOutput);
            values.put("NumberPet", item.NumberPet);
            values.put("FamilyType", item.FamilyType);
            values.put("TotalIncome", item.TotalIncome);
            values.put("IncomeSources", item.IncomeSources);
            values.put("IncomeOther", item.IncomeOther);
            values.put("StoryContent", item.StoryContent);

            values.put("AreaApproverId", item.AreaApproverId);
            values.put("AreaApproverDate", item.AreaApproverDate);
            values.put("OfficeApproveBy", item.OfficeApproveBy);
            values.put("OfficeApproveDate", item.OfficeApproveDate);
            values.put("ProcessStatus", item.ProcessStatus);
            values.put("IsDelete", item.IsDelete);
            values.put("CreateBy", item.CreateBy);
            values.put("CreateDate", item.CreateDate);
            values.put("UpdateBy", item.UpdateBy);
            values.put("UpdateDate", item.UpdateDate);
            values.put("ConsentName", item.ConsentName);
            values.put("ConsentRelationship", item.ConsentRelationship);
            values.put("ConsentVillage", item.ConsentVillage);
            values.put("ConsentWard", item.ConsentWard);
            values.put("SiblingsJoiningChildFund", item.SiblingsJoiningChildFund);
            values.put("Malformation", item.Malformation);
            values.put("Orphan", item.Orphan);
            values.put("EmployeeTitle", item.EmployeeTitle);
//            values.put("ImageSignaturePath", item.ImageSignaturePath);
//            values.put("ImageSignatureThumbnailPath", item.ImageSignatureThumbnailPath);
            values.put("SaleforceId", item.SaleforceId);
            values.put("Handicap", item.Handicap);
            values.put("ImageSize", item.ImageSize);
            values.put("TypeChildProfile", Constants.TYPE_CHILDPROFILE_SQLITE_DOWNLOAD);
            DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
            try {
                if (dataBaseHelper.checkExistsData(Constants.DATABASE_TABLE_ChildProfile, item.Id).getCount() == 0) {
                    if (!Utils.isEmpty(item.ImagePath)) {
                        String[] arrUrlImagePath = item.ImagePath.split("/");
                        String imageName = arrUrlImagePath[arrUrlImagePath.length - 1];
                        values.put("ImagePath", downloadImage(item.ImagePath, imageName, Constants.TYPE_DOWNLOAD_IMAGE_ORIGINAL));
                    }
                    if (!Utils.isEmpty(item.ImageThumbnailPath)) {
                        String[] arrUrlThumbPath = item.ImageThumbnailPath.split("/");
                        String imageThumbName = arrUrlThumbPath[arrUrlThumbPath.length - 1];
                        values.put("ImageThumbnailPath", downloadImage(item.ImageThumbnailPath, imageThumbName, Constants.TYPE_DOWNLOAD_IMAGE_THUMB));
                    }
                    if (!Utils.isEmpty(item.ImageSignaturePath)) {
                        String[] arrUrlSignature = item.ImageSignaturePath.split("/");
                        String imageSignatureName = arrUrlSignature[arrUrlSignature.length - 1];
                        values.put("ImageSignaturePath", downloadImage(item.ImageSignaturePath, imageSignatureName, Constants.TYPE_DOWNLOAD_IMAGE_SIGNATURE));
                    }
                    if (!Utils.isEmpty(item.ImageSignatureThumbnailPath)) {
                        String[] arrUrlSignatureThumb = item.ImageSignatureThumbnailPath.split("/");
                        String imageSignatureThumbName = arrUrlSignatureThumb[arrUrlSignatureThumb.length - 1];
                        values.put("ImageSignatureThumbnailPath", downloadImage(item.ImageSignatureThumbnailPath, imageSignatureThumbName, Constants.TYPE_DOWNLOAD_IMAGE_SIGNATURE_THUMB));
                    }
                    dataBaseHelper.insert(Constants.DATABASE_TABLE_ChildProfile, values);
                } else {
                    Cursor cursor = dataBaseHelper.checkExistsData(Constants.DATABASE_TABLE_ChildProfile, item.Id);
                    String updateDate = "";
                    String imagePathLocal = "";
                    String thumbPathLocal = "";
                    if (cursor.moveToFirst()) {
                        while (!cursor.isAfterLast()) {
                            updateDate = cursor.getString(cursor.getColumnIndex("UpdateDate")).replace("T", " ");
                            imagePathLocal = cursor.getString(cursor.getColumnIndex("ImagePath"));
                            thumbPathLocal = cursor.getString(cursor.getColumnIndex("ImageThumbnailPath"));
                            cursor.moveToNext();
                        }
                    }

                    Date updateDateOld = new SimpleDateFormat("dd-MM-yyyy hh:mm:ss").parse(updateDate);
                    Date updateDateNew = new SimpleDateFormat("dd-MM-yyyy hh:mm:ss").parse(item.UpdateDate.replace("T", " "));
                    if (updateDateNew.after(updateDateOld)) {
                        dataBaseHelper.delete(Constants.DATABASE_TABLE_ChildProfile, item.Id);
                        if (!Utils.isEmpty(imagePathLocal)) {
                            File image = new File(imagePathLocal);
                            image.delete();
                        }
                        if (!Utils.isEmpty(thumbPathLocal)) {
                            File thumb = new File(thumbPathLocal);
                            thumb.delete();
                        }

                        if (!Utils.isEmpty(item.ImagePath)) {
                            String[] arrUrlImagePath = item.ImagePath.split("/");
                            String imageName = arrUrlImagePath[arrUrlImagePath.length - 1];
                            values.put("ImagePath", downloadImage(item.ImagePath, imageName, Constants.TYPE_DOWNLOAD_IMAGE_ORIGINAL));
                        }
                        if (!Utils.isEmpty(item.ImageThumbnailPath)) {
                            String[] arrUrlThumbPath = item.ImageThumbnailPath.split("/");
                            String imageThumbName = arrUrlThumbPath[arrUrlThumbPath.length - 1];
                            values.put("ImageThumbnailPath", downloadImage(item.ImagePath, imageThumbName, Constants.TYPE_DOWNLOAD_IMAGE_THUMB));
                        }
                        if (!Utils.isEmpty(item.ImageSignaturePath)) {
                            String[] arrUrlSignature = item.ImageSignaturePath.split("/");
                            String imageSignatureName = arrUrlSignature[arrUrlSignature.length - 1];
                            values.put("ImageSignaturePath", downloadImage(item.ImageSignaturePath, imageSignatureName, Constants.TYPE_DOWNLOAD_IMAGE_SIGNATURE));
                        }
                        if (!Utils.isEmpty(item.ImageSignatureThumbnailPath)) {
                            String[] arrUrlSignatureThumb = item.ImageSignatureThumbnailPath.split("/");
                            String imageSignatureThumbName = arrUrlSignatureThumb[arrUrlSignatureThumb.length - 1];
                            values.put("ImageSignatureThumbnailPath", downloadImage(item.ImageSignatureThumbnailPath, imageSignatureThumbName, Constants.TYPE_DOWNLOAD_IMAGE_SIGNATURE_THUMB));
                        }
                        dataBaseHelper.insert(Constants.DATABASE_TABLE_ChildProfile, values);
                    }
                }
                Toast.makeText(getContext(), "Tải hồ sơ trẻ thành công", Toast.LENGTH_SHORT).show();
            } catch (Exception ex) {
                Toast.makeText(getContext(), "Đã có lỗi trong quá trình tải hồ sơ trẻ", Toast.LENGTH_SHORT).show();
            }
        }
    }

    //Tải ảnh của trẻ
    private String downloadImage(String URL, String imageName, int typeImage) {
        final Long reference;
        DownloadManager downloadManager = (DownloadManager) getActivity().getSystemService(Context.DOWNLOAD_SERVICE);
        Uri uri = Uri.parse(URL);
        DownloadManager.Request request = new DownloadManager.Request(uri);
        String resultUrl = "";
        if (typeImage == Constants.TYPE_DOWNLOAD_IMAGE_ORIGINAL) {
            request.setDestinationInExternalPublicDir(Environment.DIRECTORY_PICTURES, "ChildProfileImageOriginal/" + imageName
            );
            resultUrl = Environment.getExternalStorageDirectory() + "/" + Environment.DIRECTORY_PICTURES + "/ChildProfileImageOriginal/" + imageName;
        } else if (typeImage == Constants.TYPE_DOWNLOAD_IMAGE_THUMB) {
            request.setDestinationInExternalPublicDir(Environment.DIRECTORY_PICTURES, "ChildProfileThumbnailPath/" + imageName
            );
            resultUrl = Environment.getExternalStorageDirectory() + "/" + Environment.DIRECTORY_PICTURES + "/ChildProfileThumbnailPath/" + imageName;
        } else if (typeImage == Constants.TYPE_DOWNLOAD_IMAGE_SIGNATURE) {
            request.setDestinationInExternalPublicDir(Environment.DIRECTORY_PICTURES, "ChildProfileSignature/" + imageName
            );
            resultUrl = Environment.getExternalStorageDirectory() + "/" + Environment.DIRECTORY_PICTURES + "/ChildProfileSignature/" + imageName;
        } else if (typeImage == Constants.TYPE_DOWNLOAD_IMAGE_SIGNATURE_THUMB) {
            request.setDestinationInExternalPublicDir(Environment.DIRECTORY_PICTURES, "ChildProfileSignatureThumbnail/" + imageName
            );
            resultUrl = Environment.getExternalStorageDirectory() + "/" + Environment.DIRECTORY_PICTURES + "/ChildProfileSignatureThumb/" + imageName;
        }

        request.setVisibleInDownloadsUi(true);
        request.setNotificationVisibility(DownloadManager.Request.VISIBILITY_VISIBLE_NOTIFY_COMPLETED);
        reference = downloadManager.enqueue(request);

        Log.d("download", "Image Download : " + reference);

        BroadcastReceiver onComplete = new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {
                try {
                    Toast.makeText(getActivity(), "Tải ảnh thành công", Toast.LENGTH_LONG);
                } catch (Exception e) {
                }
            }
        };
        getActivity().getApplicationContext().registerReceiver(onComplete, new IntentFilter(DownloadManager.ACTION_DOWNLOAD_COMPLETE));
//        String a = isThumb ? Environment.getExternalStorageDirectory() + "/" + Environment.DIRECTORY_PICTURES + "/ChildProfileThumbnailPath/" + imageName : Environment.getExternalStorageDirectory() + "/" + Environment.DIRECTORY_PICTURES + "/ChildProfileImageOriginal/" + imageName;
//        return isThumb ? Environment.DIRECTORY_PICTURES + "/Download/ChildProfileThumbnailPath/" + imageName : Environment.getExternalStorageDirectory() + "/Download/ChildProfileImageOriginal/" + imageName;
        return resultUrl;
    }

    private List<String> listChildProfileIdSQLite = new ArrayList<>();

    private void getListChildProfileSQLite() {
        DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
        Cursor cursor = dataBaseHelper.getAllChildProfileId();
        if (cursor.getCount() > 0) {
            if (cursor.moveToFirst()) {
                while (!cursor.isAfterLast()) {
                    String id = cursor.getString(cursor.getColumnIndex("Id"));
                    listChildProfileIdSQLite.add(id);
                    cursor.moveToNext();
                }
            }
        }
    }

    private void searchChildProfileOffline(final ChildProfileSearchCondition childProfileSearchCondition) {
        listProfile.clear();
        DataBaseHelper dataBaseHelper = new DataBaseHelper(getActivity(), Constants.DATABASE_NAME, null, 1);
        List<ChildProfileSearchResult> result = dataBaseHelper.searchChildProfile(childProfileSearchCondition);
        listProfile.addAll(result);
        profileListAdapter.notifyDataSetChanged();
        if (listProfile.size() > 0) {
            lyt_notfound.setVisibility(View.GONE);
        } else {
            lyt_notfound.setVisibility(View.VISIBLE);
        }
        item_total.setText(String.valueOf(listProfile.size()));
    }
}
