package nts.childprofile.fragment;

import android.Manifest;
import android.app.Activity;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Build;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.annotation.NonNull;
import android.support.design.widget.Snackbar;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.os.Bundle;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.FileProvider;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.ANRequest;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.StringRequestListener;
import com.google.gson.Gson;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import nts.childprofile.R;
import nts.childprofile.adapter.ImageAdapter;
import nts.childprofile.adapter.ImageReportAdapter;
import nts.childprofile.common.Constants;
import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.model.GalleryModel;
import nts.childprofile.model.LoginProfileModel;
import nts.childprofile.model.UserUploadImageModel;

import static android.Manifest.permission.READ_EXTERNAL_STORAGE;
import static android.app.Activity.RESULT_OK;

public class UploadImage extends Fragment {
    private View view;
    private LinearLayout lyNext, lyBack;
    private EditText txtContent;
    private ImageReportAdapter imageReportAdapter;
    private ImageAdapter imageAdapter;
    private static final int REQUEST_FOR_STORAGE_PERMISSION = 123;
    private Dialog dialogGallery;
    private ImageView imageCaptureEngine, imageChoosePhoto;
    private String imageLink;
    private ArrayList<GalleryModel> fileUpload = new ArrayList<>();
    private RelativeLayout progressDialog;
    private GlobalVariable global;
    private LoginProfileModel loginProfileModel;
    private ArrayList<GalleryModel> gallery;
    private long fileSize = 0;
    private TextView txtFileSize;
    RecyclerView recyclerView;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_upload_image, container, false);

        Toolbar mToolbar = (Toolbar) getActivity().findViewById(R.id.toolbar);
        TextView toolbarTitle = mToolbar.findViewById(R.id.toolbar_title);
//        toolbarTitle.setText("Ảnh hoạt động cộng đồng/Community photos");
        toolbarTitle.setText("Ảnh cộng đồng/Community photos");
        Toolbar.LayoutParams layoutParams = (Toolbar.LayoutParams) toolbarTitle.getLayoutParams();
        layoutParams.gravity = Gravity.LEFT;
        toolbarTitle.setLayoutParams(layoutParams);

        global = (GlobalVariable) getActivity().getApplication();
        loginProfileModel = global.getLoginProfile();
        recyclerView = (RecyclerView) view.findViewById(R.id.rvGalleryChoose);

        initComponent();

        viewImageReport(fileUpload);
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
                //imageUrlsReport.add(imageLink);
                GalleryModel galleryModel = new GalleryModel();
                galleryModel.FileUrl = imageLink;
                galleryModel.Type = 1;
                fileUpload.add(galleryModel);
                viewImageReport(fileUpload);
            }
            super.onActivityResult(requestCode, resultCode, data);
        } catch (Exception ex) {
        }
    }

    /***
     * Khỏi tạo thành phần trên giao diện
     */
    private void initComponent() {
        lyNext = view.findViewById(R.id.lyNext);
        progressDialog = view.findViewById(R.id.progressDialog);

        imageChoosePhoto = (ImageView) view.findViewById(R.id.imageChoosePhoto);
        imageCaptureEngine = (ImageView) view.findViewById(R.id.imageCaptureEngine);

        txtContent = view.findViewById(R.id.txtContent);
        txtFileSize =  view.findViewById(R.id.txtFileSize);

        lyNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                UploadImage();
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

                dialogGallery.show();
            }
        });
    }


    private void populateImagesFromGallery() {
        if (!mayRequestGalleryImages()) {
            return;
        }

        gallery = loadPhotosFromNativeGallery();
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
                MediaStore.Files.FileColumns.MEDIA_TYPE,
                MediaStore.Files.FileColumns.SIZE,
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

        RecyclerView.LayoutManager layoutManager = new GridLayoutManager(getContext(), 3);

        recyclerView.setLayoutManager(layoutManager);
        recyclerView.setItemAnimator(new DefaultItemAnimator());
        recyclerView.addItemDecoration(new ItemOffsetDecoration(getContext(), R.dimen.item_offset));
        recyclerView.setAdapter(imageReportAdapter);

        imageReportAdapter.SetOnItemClickListener(new ImageReportAdapter.OnItemClickListener() {
            @Override
            public void onRemoveClick(View view, int position, GalleryModel galleryModel) {
                fileUpload.remove(galleryModel);

                imageReportAdapter.notifyItemRemoved(position);
                calculateSize();
                imageAdapter.notifyDataSetChanged();
                if (fileUpload.size() == 0)
                {
                    recyclerView.setVisibility(View.INVISIBLE);
                }
            }

            @Override
            public void showFullImage(View view, int position, GalleryModel imageUrl) {

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
                    recyclerView.setVisibility(View.VISIBLE);
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
        } catch (Exception ex) {
            String temp = "";
        }
    }

    public void UploadImage() {
        if (!validateFrom()) {
            return;
        }

        UserUploadImageModel userUploadImageModel = new UserUploadImageModel(loginProfileModel.Name + "_" + txtContent.getText().toString(), loginProfileModel.Id);

        String jsonModel = new Gson().toJson(userUploadImageModel);
        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.GetUrlApi("api/ImageLibrary/UploadImage"));

        for (int i = 0; i < fileUpload.size(); i++) {
            File file = new File(fileUpload.get(i).FileUrl);
            anRequest.addMultipartFile("file" + i, file);
        }

        progressDialog.setVisibility(View.VISIBLE);
        anRequest.addMultipartParameter("Model", jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsString(new StringRequestListener() {
                    @Override
                    public void onResponse(String response) {
                        Toast.makeText(getActivity(), "Chia sẻ ảnh/ video thành công", Toast.LENGTH_SHORT).show();
                        progressDialog.setVisibility(View.GONE);
                        try {
                            txtContent.setText("");
                            fileUpload.clear();
                            viewImageReport(fileUpload);
                            initDialogGallery(gallery);
                        } catch (Exception ex) {
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Toast.makeText(getActivity(), anError.getErrorBody(), Toast.LENGTH_SHORT).show();
                        progressDialog.setVisibility(View.GONE);
                    }
                });

    }

    /***
     * Check validate from
     * @return
     */
    private boolean validateFrom() {
        boolean isValidate = true;

        if (Utils.isEmpty(txtContent.getText().toString())) {
            Toast.makeText(getActivity(), "Nội dung chia sẻ không được để trống.", Toast.LENGTH_SHORT).show();
            txtContent.requestFocus();
            return false;
        }

        if (txtContent.getText().length() < 100) {
            Toast.makeText(getActivity(), "Nội dung ít nhất phải có 100 ký tự. Số ký tự hiện tại là " + txtContent.getText().length(), Toast.LENGTH_SHORT).show();
            txtContent.requestFocus();
            return false;
        }

        if (fileUpload == null || fileUpload.size() == 0) {
            Toast.makeText(getActivity(), "Chưa chọn ảnh/ video để chia sẻ", Toast.LENGTH_SHORT).show();
            return false;
        }

        if (fileSize > 25) {
            Toast.makeText(getActivity(), "Tổng độ lớn tối đa của các file gửi kèm là 25 MB/The total maximum size of attachments is 25 MB", Toast.LENGTH_SHORT).show();
            return false;
        }

        return isValidate;
    }

    /***
     * Tính độ lớn file upload
     */
    private void calculateSize() {
        fileSize = 0;
        if (fileUpload != null && fileUpload.size() > 0) {
            for (int i = 0; i < fileUpload.size(); i++) {
                fileSize += fileUpload.get(i).Size;
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
