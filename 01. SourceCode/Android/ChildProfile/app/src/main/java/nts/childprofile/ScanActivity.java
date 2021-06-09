package nts.childprofile;

import android.Manifest;
import android.app.Activity;
import android.app.Dialog;
import android.app.DialogFragment;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.Snackbar;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.AppCompatButton;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.bumptech.glide.util.Util;
import com.google.zxing.BarcodeFormat;
import com.google.zxing.Result;

import java.util.ArrayList;
import java.util.List;

import me.dm7.barcodescanner.zxing.ZXingScannerView;
import nts.childprofile.R;
import nts.childprofile.common.Utils;
import nts.childprofile.common.ViewAnimation;

public class ScanActivity extends AppCompatActivity implements ZXingScannerView.ResultHandler {
    private Activity context;
    private View parent_view;
    private LinearLayout lyProgress;

    //Init component dialog
    private Dialog dialogInputQrcode;
    private ImageButton btCloseDialog;
    private EditText txtCode;
    private AppCompatButton btnAction;

    private static final int MY_PERMISSION_REQUEST_CAMERA = 0;
    private ZXingScannerView mScannerView;
    private boolean mFlash;
    private boolean mAutoFocus = true;
    private ArrayList<Integer> mSelectedIndices;
    private int mCameraId = -1;

    private static final String FLASH_STATE = "FLASH_STATE";
    private static final String AUTO_FOCUS_STATE = "AUTO_FOCUS_STATE";
    private static final String SELECTED_FORMATS = "SELECTED_FORMATS";
    private static final String CAMERA_ID = "CAMERA_ID";
    private String barcode = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_scan);

        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.CAMERA)
                == PackageManager.PERMISSION_GRANTED) {
        } else {
            requestCameraPermission(parent_view);
        }

        context = this;

        initComponent();

        initInputQrcodeDialog();
    }

    private void initComponent() {
        lyProgress = (LinearLayout) this.findViewById(R.id.lyProgress);
        ViewAnimation.fadeOut(lyProgress);

        parent_view = this.findViewById(android.R.id.content);

        ViewGroup contentFrame = (ViewGroup) this.findViewById(R.id.content_frame);
        mScannerView = new ZXingScannerView(context);
        //setupFormats();
        contentFrame.addView(mScannerView);

        final ImageButton btFlash = this.findViewById(R.id.btFlash);
        btFlash.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                mFlash = !mFlash;
                mScannerView.setFlash(mFlash);
                if (mFlash) {
                    btFlash.setImageResource(R.drawable.ic_flash);
                } else {
                    btFlash.setImageResource(R.drawable.ic_flash_off_24dp);
                }
            }
        });

        ((ImageButton) this.findViewById(R.id.btInputCode)).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                mScannerView.stopCamera();
                dialogInputQrcode.show();
            }
        });

        ((ImageButton) this.findViewById(R.id.btClose)).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });
    }

    /***
     * Khởi tạo giao diện nhập qrcode
     */
    private void initInputQrcodeDialog() {
        dialogInputQrcode = new Dialog(context);
        dialogInputQrcode.requestWindowFeature(Window.FEATURE_NO_TITLE); // before
        dialogInputQrcode.setContentView(R.layout.popup_input_qrcode);
        dialogInputQrcode.setCanceledOnTouchOutside(false);

        WindowManager.LayoutParams lp = new WindowManager.LayoutParams();
        lp.copyFrom(dialogInputQrcode.getWindow().getAttributes());
        lp.width = WindowManager.LayoutParams.WRAP_CONTENT;
        lp.height = WindowManager.LayoutParams.WRAP_CONTENT;

        ImageButton btClose = (ImageButton) dialogInputQrcode.findViewById(R.id.btCloseDialog);
        txtCode = (EditText) dialogInputQrcode.findViewById(R.id.txtCode);
        btnAction = (AppCompatButton) dialogInputQrcode.findViewById(R.id.btnAction);

        btnAction.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent returnIntent = new Intent();
                returnIntent.putExtra("Barcode", txtCode.getText().toString());
                setResult(Activity.RESULT_OK, returnIntent);
                finish();
            }
        });

        btClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                mScannerView.startCamera();
                dialogInputQrcode.dismiss();
            }
        });

        dialogInputQrcode.getWindow().setAttributes(lp);
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions,
                                           @NonNull int[] grantResults) {
        if (requestCode != MY_PERMISSION_REQUEST_CAMERA) {
            return;
        }

        if (grantResults.length == 1 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
            Snackbar.make(parent_view, "Camera permission was granted.", Snackbar.LENGTH_SHORT).show();
        } else {
            Snackbar.make(parent_view, "Camera permission request was denied.", Snackbar.LENGTH_SHORT)
                    .show();
        }
    }

    private void requestCameraPermission(View parent_view) {
        if (ActivityCompat.shouldShowRequestPermissionRationale(this, Manifest.permission.CAMERA)) {
            Snackbar.make(parent_view, "Camera access is required to display the camera preview.",
                    Snackbar.LENGTH_INDEFINITE).setAction("OK", new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    ActivityCompat.requestPermissions(context, new String[]{
                            Manifest.permission.CAMERA
                    }, MY_PERMISSION_REQUEST_CAMERA);
                }
            }).show();
        } else {
            Snackbar.make(parent_view, "Permission is not available. Requesting camera permission.",
                    Snackbar.LENGTH_SHORT).show();
            ActivityCompat.requestPermissions(context, new String[]{
                    Manifest.permission.CAMERA
            }, MY_PERMISSION_REQUEST_CAMERA);
        }
    }

    public void setupFormats() {
        List<BarcodeFormat> formats = new ArrayList<BarcodeFormat>();
        if (mSelectedIndices == null || mSelectedIndices.isEmpty()) {
            mSelectedIndices = new ArrayList<Integer>();
            for (int i = 0; i < mScannerView.ALL_FORMATS.size(); i++) {
                mSelectedIndices.add(i);
            }
        }

        for (int index : mSelectedIndices) {
            formats.add(mScannerView.ALL_FORMATS.get(index));
        }
        if (mScannerView != null) {
            mScannerView.setFormats(formats);
        }
    }

    @Override
    public void onSaveInstanceState(Bundle outState) {
        super.onSaveInstanceState(outState);
        outState.putBoolean(FLASH_STATE, mFlash);
        outState.putBoolean(AUTO_FOCUS_STATE, mAutoFocus);
        outState.putIntegerArrayList(SELECTED_FORMATS, mSelectedIndices);
        outState.putInt(CAMERA_ID, mCameraId);
    }

    @Override
    public void onResume() {
        super.onResume();
        mScannerView.setResultHandler(this); // Register ourselves as a handler for scan results.
        mScannerView.startCamera();          // Start camera on resume
    }

    @Override
    public void onPause() {
        super.onPause();
        mScannerView.stopCamera();           // Stop camera on pause
    }

    @Override
    public void handleResult(Result rawResult) {
        if (!Utils.isEmpty(barcode)) {
            return;
        }

        barcode = rawResult.getText();
        if (!Utils.isEmpty(barcode)) {
            Intent returnIntent = new Intent();
            returnIntent.putExtra("Barcode", barcode);
            setResult(Activity.RESULT_OK, returnIntent);
            finish();
        }

        // If you would like to resume scanning, call this method below:
        mScannerView.resumeCameraPreview(this);
    }
}