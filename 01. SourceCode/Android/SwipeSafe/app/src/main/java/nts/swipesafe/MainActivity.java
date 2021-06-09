package nts.swipesafe;

import android.Manifest;
import android.app.Activity;
import android.app.Dialog;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.view.Gravity;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.google.android.youtube.player.YouTubePlayerSupportFragment;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import nts.swipesafe.common.Utils;
import nts.swipesafe.fragment.AboutAppFragment;
import nts.swipesafe.fragment.LibraryFragment;
import nts.swipesafe.fragment.LibraryGenderFragment;
import nts.swipesafe.fragment.LibraryInternetFragment;
import nts.swipesafe.fragment.LibraryInternetImageDetailFragment;
import nts.swipesafe.fragment.LibraryRightFragment;
import nts.swipesafe.fragment.LibrarySkillDetailFragment;
import nts.swipesafe.fragment.LibrarySkillFragment;
import nts.swipesafe.fragment.LinkWebsiteFragment;
import nts.swipesafe.fragment.PhoneCategoryFragment;
import nts.swipesafe.fragment.ReportAbuseStepInfoFragment;
import nts.swipesafe.fragment.ReportMainFragment;
import nts.swipesafe.services.SendOfflineService;

import static com.androidnetworking.internal.ANRequestQueue.initialize;

public class MainActivity extends AppCompatActivity {
    private Dialog dialogMenu;
    private LinearLayout lyFeedback, lyCall, lyLibrary, lyWeb, lyAboutApp;
    private Activity mActivity;
    /**
     * permissions request code
     */
    private final static int REQUEST_CODE_ASK_PERMISSIONS = 1;

    /**
     * Permissions that need to be explicitly requested from end user.
     */
    private static final String[] REQUIRED_SDK_PERMISSIONS = new String[]{
            Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission.WRITE_EXTERNAL_STORAGE,
            Manifest.permission.READ_EXTERNAL_STORAGE, Manifest.permission.CAMERA, Manifest.permission.READ_PHONE_STATE};

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        mActivity = this;
        checkPermissions();

        if (!Utils.checkConnectedNetwork(getApplicationContext())) {
            new android.support.v7.app.AlertDialog.Builder(mActivity)
                    .setTitle("Thông báo")
                    .setIcon(R.drawable.ic_warning)
                    .setMessage("Vui lòng kiểm tra kết nối Internet/3G/Wifi để tiếp tục.")
                    .setCancelable(false)
                    .setNegativeButton("Đóng", null)
                    .show();
        }
        // Khởi tạo Fragment đầu tiên cho main activity
        Fragment fragment = new ReportMainFragment();
        Utils.ChangeFragment(this, fragment, null);

        initDialogMenu();

        View toolBar = findViewById(R.id.toolBar);
        //Show menu
        ImageView mMenu = toolBar.findViewById(R.id.imgMenu);
        mMenu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogMenu.show();
            }
        });

        //Show gọi tổng đài 111
        LinearLayout lyCall = toolBar.findViewById(R.id.lyCall);
        lyCall.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivity(new Intent(Intent.ACTION_DIAL, Uri.fromParts("tel", "111", null)));
            }
        });
    }

    @Override
    public void onResume() {
        super.onResume();
        startService();
    }

    @Override
    public void onStop() {
        super.onStop();
        stopService();
    }

    /**
     * Checks the dynamically-controlled permissions and requests missing permissions from end user.
     */
    protected void checkPermissions() {
        final List<String> missingPermissions = new ArrayList<String>();
        // check all required dynamic permissions
        for (final String permission : REQUIRED_SDK_PERMISSIONS) {
            final int result = ContextCompat.checkSelfPermission(this, permission);
            if (result != PackageManager.PERMISSION_GRANTED) {
                missingPermissions.add(permission);
            }
        }
        if (!missingPermissions.isEmpty() && (missingPermissions.size() == REQUIRED_SDK_PERMISSIONS.length)) {
            // request all missing permissions
            final String[] permissions = missingPermissions
                    .toArray(new String[missingPermissions.size()]);
            ActivityCompat.requestPermissions(this, permissions, REQUEST_CODE_ASK_PERMISSIONS);
        } else {
            final int[] grantResults = new int[REQUIRED_SDK_PERMISSIONS.length];
            Arrays.fill(grantResults, PackageManager.PERMISSION_GRANTED);
            onRequestPermissionsResult(REQUEST_CODE_ASK_PERMISSIONS, REQUIRED_SDK_PERMISSIONS,
                    grantResults);
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String permissions[],
                                           @NonNull int[] grantResults) {
        switch (requestCode) {
            case REQUEST_CODE_ASK_PERMISSIONS:
                for (int index = permissions.length - 1; index >= 0; --index) {
                    if (grantResults[index] != PackageManager.PERMISSION_GRANTED) {
                        // exit the app if one permission is not granted
                        Toast.makeText(this, "Required permission '" + permissions[index]
                                + "' not granted, exiting", Toast.LENGTH_LONG).show();
                        finish();
                        return;
                    }
                }
                // all permissions were granted
                initialize();
                break;
        }
    }

    @Override
    public void onBackPressed() {
        Fragment f =
                getSupportFragmentManager().findFragmentById(R.id.frame_content);
        if (f != null) {
            if (f instanceof ReportAbuseStepInfoFragment) {
                ((ReportAbuseStepInfoFragment) f).backFragment();
                return;
            } else if (f instanceof LibraryFragment) {
                ((LibraryFragment) f).backFragment();
                return;
            } else if (f instanceof LibraryRightFragment) {
                ((LibraryRightFragment) f).backFragment();
                return;
            } else if (f instanceof LibrarySkillFragment) {
                ((LibrarySkillFragment) f).backFragment();
                return;
            } else if (f instanceof LibrarySkillDetailFragment) {
                ((LibrarySkillDetailFragment) f).backFragment();
                return;
            } else if (f instanceof YouTubePlayerSupportFragment) {
                Bundle bundle =((YouTubePlayerSupportFragment) f).getArguments();
                if (bundle != null) {
                    String fragmentName = bundle.getString("Name");
                    if(fragmentName.equals("Internet"))
                    {
                        Fragment fragment = new LibraryInternetFragment();
                        Utils.ChangeFragment(this, fragment, null);
                    }else {
                        Fragment fragment = new LibraryGenderFragment();
                        Utils.ChangeFragment(this, fragment, null);
                    }
                }
                return;
            } else if (f instanceof ReportMainFragment) {
            }else if (f instanceof LibraryInternetImageDetailFragment) {
                ((LibraryInternetImageDetailFragment) f).backFragment();
                return;
            }
            else {
                Fragment fragment = new ReportMainFragment();
                Utils.ChangeFragment(this, fragment, null);
                return;
            }
        }
        super.onBackPressed();
    }

    public void initDialogMenu() {
        dialogMenu = new Dialog(this);
        dialogMenu.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialogMenu.setContentView(R.layout.popup_menu);
        dialogMenu.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogMenu.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogMenu.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.WRAP_CONTENT);

        WindowManager.LayoutParams wmlp = dialogMenu.getWindow().getAttributes();
        wmlp.gravity = Gravity.TOP | Gravity.LEFT;
        wmlp.x = 0;   //x position
        wmlp.y = 0;   //y position
        dialogMenu.getWindow().setGravity(Gravity.LEFT | Gravity.TOP);

        lyFeedback = dialogMenu.findViewById(R.id.lyFeedback);
        lyFeedback.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new ReportAbuseStepInfoFragment();
                Utils.ChangeFragment(mActivity, fragment, null);
                dialogMenu.hide();
            }
        });

        lyCall = dialogMenu.findViewById(R.id.lyCall);
        lyCall.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new PhoneCategoryFragment();
                Utils.ChangeFragment(mActivity, fragment, null);
                dialogMenu.hide();
            }
        });


        lyLibrary = dialogMenu.findViewById(R.id.lyLibrary);
        lyLibrary.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new LibraryFragment();
                Utils.ChangeFragment(mActivity, fragment, null);
                dialogMenu.hide();
            }
        });

        lyWeb = dialogMenu.findViewById(R.id.lyWeb);
        lyWeb.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new LinkWebsiteFragment();
                Utils.ChangeFragment(mActivity, fragment, null);
                dialogMenu.hide();
            }
        });

        lyAboutApp = dialogMenu.findViewById(R.id.lyAboutApp);
        lyAboutApp.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragment = new AboutAppFragment();
                Utils.ChangeFragment(mActivity, fragment, null);
                dialogMenu.hide();
            }
        });
    }

    public void startService() {
        Intent intent = new Intent(this, SendOfflineService.class);
        startService(intent);
    }

    public void stopService() {
        Intent intent = new Intent(this, SendOfflineService.class);
        stopService(intent);
    }
}
