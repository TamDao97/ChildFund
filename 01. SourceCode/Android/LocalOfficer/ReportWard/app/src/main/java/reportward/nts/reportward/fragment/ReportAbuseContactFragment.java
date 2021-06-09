package reportward.nts.reportward.fragment;

import android.Manifest;
import android.app.Activity;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
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
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONObject;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.ImageAdapter;
import reportward.nts.reportward.adapter.ImageReportAdapter;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.CaTuVanModel;
import reportward.nts.reportward.model.ComboboxResult;
import reportward.nts.reportward.model.GalleryModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;

import static android.Manifest.permission.READ_EXTERNAL_STORAGE;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseContactFragment extends Fragment {
    private boolean isLoad = true;
    private View view;
    public EditText txtSoDienThoai, txtMangXaHoi, txtEmail, txtNoiDungTuVan;
    private EditText spnHinhThucLienHe;
    private String[] arrayHinhThucLienHe;
    private List<ComboboxResult> listHinhThucLienHe;
    private CaTuVanModel caTuVanModel;

    //Upload ảnh
    private ImageReportAdapter imageReportAdapter;
    private ImageAdapter imageAdapter;
    private static final int REQUEST_FOR_STORAGE_PERMISSION = 123;
    private Dialog dialogGallery;
    private ImageView imageCaptureEngine, imageChoosePhoto;
    private String imageLink;
    public ArrayList<GalleryModel> fileReport = new ArrayList<>();
    public long fileSize = 0;
    private TextView txtFileSize;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    public ReportAbuseContactFragment() {

    }

    public static ReportAbuseContactFragment newInstance() {
        ReportAbuseContactFragment fragment = new ReportAbuseContactFragment();
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        if (isLoad) {
            view = inflater.inflate(R.layout.fragment_report_abuse_contact, container, false);

            //Get thông tin login
            prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
            String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
            if (loginProfile != null) {
                loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
                }.getType());
            }

            initComponent();

            getDataCombobox(loginProfileModel.accessToken);

            populateImagesFromGallery();

            viewImageReport(fileReport);
        }
        isLoad = false;

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

    /***
     * Khỏi tạo thành phần trên giao diện
     */
    private void initComponent() {
        txtSoDienThoai = view.findViewById(R.id.txtSoDienThoai);
        txtMangXaHoi = view.findViewById(R.id.txtMangXaHoi);
        txtEmail = view.findViewById(R.id.txtEmail);
        txtNoiDungTuVan = view.findViewById(R.id.txtNoiDungTuVan);

        spnHinhThucLienHe = view.findViewById(R.id.spnHinhThucLienHe);

        imageChoosePhoto = (ImageView) view.findViewById(R.id.imageChoosePhoto1);
        imageCaptureEngine = (ImageView) view.findViewById(R.id.imageCaptureEngine1);
        txtFileSize = view.findViewById(R.id.txtFileSize1);

        spnHinhThucLienHe.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnHinhThucLienHe, arrayHinhThucLienHe, listHinhThucLienHe, "Chọn hình thức liên hệ", null);
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
                        Uri photoUri = FileProvider.getUriForFile(getActivity(), getActivity().getPackageName() + ".nts.reportward.provider", photoFile);
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

    private void viewData() {
        if (caTuVanModel != null) {
            txtNoiDungTuVan.setText(caTuVanModel.NoiDungTuVan);
            spnHinhThucLienHe.setTag(caTuVanModel.HinhThucLienHe);
            txtSoDienThoai.setText(caTuVanModel.SoDienThoai);
            txtMangXaHoi.setText(caTuVanModel.MangXaHoi);
            txtEmail.setText(caTuVanModel.Email);
        }
    }

    /***
     * Show Dialog chọn Tỉnh/Thành
     * @param editText
     * @param arrayName
     * @param listSource
     * @param title
     * @param funtion
     */
    private void showChooseDialog(final EditText editText, final String[] arrayName, final List<ComboboxResult> listSource, final String title, final Runnable funtion) {
        final android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, Utils.getIndexByName(arrayName, Utils.getText(editText)), new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                editText.setText(arrayName[which]);

                String id = listSource != null ? listSource.get(which).id : "";
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

    /***
     * Lấy dữ liệu cho combobox
     * @param authorizationKey
     */
    private void getDataCombobox(String authorizationKey) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dshinhthuclienhe))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listHinhThucLienHe = resultObject.data;

                        arrayHinhThucLienHe = new String[listHinhThucLienHe.size()];
                        int index = 0;
                        for (ComboboxResult item : listHinhThucLienHe) {
                            if (index == 0 && caTuVanModel == null) {
                                spnHinhThucLienHe.setText(item.text);
                                spnHinhThucLienHe.setTag(item.id);
                            }
                            arrayHinhThucLienHe[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

    /***
     * Gét thông tin trên fragment
     */
    public CaTuVanModel GetInfoModel() {
        caTuVanModel = new CaTuVanModel();
        try {
            caTuVanModel.NoiDungTuVan = txtNoiDungTuVan.getText().toString();
            caTuVanModel.HinhThucLienHe = spnHinhThucLienHe.getTag() != null ? spnHinhThucLienHe.getTag().toString() : "";
            caTuVanModel.ThoiGianTuVan = "";
            caTuVanModel.SoDienThoai = txtSoDienThoai.getText().toString();
            caTuVanModel.MangXaHoi = txtMangXaHoi.getText().toString();
            caTuVanModel.Email = txtEmail.getText().toString();
            caTuVanModel.NguoiDung = loginProfileModel.tenDangNhap;
            caTuVanModel.MaCapDo = "";
            caTuVanModel.HoTenNCT = "";
            caTuVanModel.DiaChiNCT = "";
            caTuVanModel.NoiCongTacNCT = "";
            caTuVanModel.SoDienThoaiNCT = "";
            caTuVanModel.CallId = "";
        } catch (Exception ex) {
        }
        return caTuVanModel;
    }

    private void populateImagesFromGallery() {
        if (!mayRequestGalleryImages()) {
            return;
        }

        ArrayList<GalleryModel> gallery = loadPhotosFromNativeGallery();
        initDialogGallery(gallery);
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
        RecyclerView recyclerView = (RecyclerView) view.findViewById(R.id.rvGalleryChoose);
        recyclerView.setLayoutManager(layoutManager);
        recyclerView.setItemAnimator(new DefaultItemAnimator());
        recyclerView.addItemDecoration(new ItemOffsetDecoration(getContext(), R.dimen.item_offset));
        recyclerView.setAdapter(imageReportAdapter);
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
