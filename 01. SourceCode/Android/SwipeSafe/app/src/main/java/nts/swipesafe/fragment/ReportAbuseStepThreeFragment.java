package nts.swipesafe.fragment;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
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
import android.support.annotation.NonNull;
import android.support.design.widget.Snackbar;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.content.CursorLoader;
import android.support.v4.content.FileProvider;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.MainActivity;
import nts.swipesafe.R;
import nts.swipesafe.adapter.ImageAdapter;
import nts.swipesafe.adapter.ImageReportAdapter;
import nts.swipesafe.adapter.PrisonerListAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ComboboxResult;
import nts.swipesafe.model.GalleryModel;
import nts.swipesafe.model.PrisonerModel;

import static android.Manifest.permission.READ_EXTERNAL_STORAGE;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseStepThreeFragment extends Fragment {
    private View view;
    private SharedPreferences sharedPreferencesDataFix;
    private GlobalVariable global;
    private LinearLayout lyNext, lyBack;
    private EditText txtDescription;
    private Bundle bundle = null;
    private ImageReportAdapter imageReportAdapter;
    private ImageAdapter imageAdapter;
    private static final int REQUEST_FOR_STORAGE_PERMISSION = 123;
    private Dialog dialogGallery;
    private ImageView imageCaptureEngine, imageChoosePhoto;
    private String imageLink;
    private ArrayList<GalleryModel> fileReport = new ArrayList<>();
    private long fileSize = 0;
    private TextView txtFileSize;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_report_abuse_step_three, container, false);

        sharedPreferencesDataFix = getActivity().getSharedPreferences(Constants.Swipe_Safe_Data_Fix, Context.MODE_PRIVATE);
        global = (GlobalVariable) getActivity().getApplication();
        bundle = getArguments();

        initComponent();

        viewData();

        if (bundle != null) {
            TextView btnText = ((TextView) lyNext.getChildAt(0));
            btnText.setText("Lưu");
            ImageView btnIcon = ((ImageView) lyNext.getChildAt(1));
            btnIcon.setImageResource( R.drawable.ic_save);
        }

        populateImagesFromGallery();

        viewImageReport(fileReport);
        // Inflate the layout for this fragment
        return view;
    }

    public void backFragment() {
        if (bundle == null) {
            saveInfo(false);
            Fragment fragment = new ReportAbuseStepTwoFragment();
            Utils.ChangeFragment(getActivity(), fragment, null);
        } else {
            Fragment fragment = new ReportAbuseDetailFragment();
            Utils.ChangeFragment(getActivity(), fragment, null);
        }
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
        lyBack = view.findViewById(R.id.lyBack);
        lyNext = view.findViewById(R.id.lyNext);

        imageChoosePhoto = (ImageView) view.findViewById(R.id.imageChoosePhoto);
        imageCaptureEngine = (ImageView) view.findViewById(R.id.imageCaptureEngine);

        txtDescription = view.findViewById(R.id.txtDescription);
        txtFileSize = view.findViewById(R.id.txtFileSize);

        lyNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (saveInfo(true)) {
                    if (bundle == null) {
                        Fragment fragment = new ReportAbuseStepFourFragment();
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

        /***
         * Chụp ảnh
         */
        imageCaptureEngine.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
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
                dialogGallery.show();
            }
        });
    }

    /***
     * Hiển thị thông tin trẻ lê giao diện
     */
    private void viewData() {
        if (global.reportModel != null) {
            txtDescription.setText(global.getDescription());
            fileReport = global.fileReport;
            viewImageReport(fileReport);
        } else {
            txtDescription.setText("");
        }
        txtDescription.requestFocus();
        calculateSize();
    }

    /***
     * Lưu thông tin
     */
    private boolean saveInfo(boolean isCheck) {
        try {
            if (isCheck && Utils.isEmpty(txtDescription.getText().toString())) {
                Toast.makeText(getActivity(), "Bạn chưa nhập mô tả hành vi chi tiết của trẻ bị xâm hại.", Toast.LENGTH_SHORT).show();
                txtDescription.requestFocus();
                return false;
            }

            calculateSize();
            if (fileSize > 100) {
                Toast.makeText(getActivity(), "Tổng độ lớn các file gửi kèm lớn hơn 100 MB.", Toast.LENGTH_SHORT).show();
                return false;
            }

            //global.imageUrlReport = imageUrlsReport;
            global.setDescription(txtDescription.getText().toString());
            return true;
        } catch (Exception ex) {
            Toast.makeText(getActivity(), "Lỗi phát sinh trong quá trình xử lý.", Toast.LENGTH_SHORT).show();
        }
        return false;
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
            for (int i = 0; i < global.fileReport.size(); i++) {
                fileSize += global.fileReport.get(i).Size;
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
}
