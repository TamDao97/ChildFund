package reportward.nts.reportward;


import android.Manifest;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;

import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.location.LocationManager;
import android.net.Uri;
import android.os.Bundle;
import android.provider.Settings;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.LocalBroadcastManager;
import android.support.v7.app.ActionBar;
import android.view.View;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.StringRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
//import com.google.firebase.database.DataSnapshot;
//import com.google.firebase.database.DatabaseError;
//import com.google.firebase.database.DatabaseReference;
//import com.google.firebase.database.FirebaseDatabase;
//import com.google.firebase.database.ValueEventListener;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.fragment.FagmentDetailCatuvan;
import reportward.nts.reportward.fragment.ReportCaseCreateFragment;
import reportward.nts.reportward.fragment.FragmentMenu;
import reportward.nts.reportward.fragment.ListReportFragment;
import reportward.nts.reportward.fragment.ListSearchReportFragment;
import reportward.nts.reportward.fragment.NotificationFragment;
import reportward.nts.reportward.model.InfoNotification;
import reportward.nts.reportward.model.LoginProfileModel;

import static com.androidnetworking.internal.ANRequestQueue.initialize;

public class MainActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    private ActionBar actionBar;
    //private NavigationView navigationView;
    private boolean mBound = false;
    private static TextView txtTotalNotify;
    private Toolbar toolbar;
    private static Activity activity;
    LocationManager locationManager;
    private static LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;
    private static SharedPreferences prefsNotify;

    /**
     * permissions request code
     */
    private final static int REQUEST_CODE_ASK_PERMISSIONS = 1;

    /**
     * Permissions that need to be explicitly requested from end user.
     */
    private static final String[] REQUIRED_SDK_PERMISSIONS = new String[]{
            Manifest.permission.WRITE_EXTERNAL_STORAGE,
            Manifest.permission.READ_EXTERNAL_STORAGE, Manifest.permission.CAMERA};

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        activity = this;

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.setDrawerLockMode(DrawerLayout.LOCK_MODE_LOCKED_CLOSED);

        actionBar = getSupportActionBar();

        txtTotalNotify = (TextView) toolbar.findViewById(R.id.txtTotalNotify);
        Button btnNotify = toolbar.findViewById(R.id.btnNotify);
        btnNotify.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Fragment fragmentNotify = new NotificationFragment();
                Utils.ChangeFragment(activity, fragmentNotify, null);
            }
        });

        //Get thông tin login
        prefsLogin = getSharedPreferences(Constants.Key_Info_Login, MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());

            if (loginProfileModel == null || (Utils.isEmpty(loginProfileModel.tenDangNhap) || Utils.isEmpty(loginProfileModel.accessToken))) {
                Intent loginActivity = new Intent(getApplicationContext(), LoginActivity.class);
                startActivity(loginActivity);
                finish();
            }
        }

        Bundle bundle = getIntent().getExtras();
        String maSoHoSo = "";
        if (bundle != null) {
            maSoHoSo = bundle.getString("maSoHoSo");
        }

        if (bundle != null && !Utils.isEmpty(maSoHoSo)) {
            FagmentDetailCatuvan fragment = new FagmentDetailCatuvan();
            fragment.soHoSo = maSoHoSo;
            Utils.ChangeFragment(this, fragment, null);

            actionBar.setTitle("Chi tiết ca tư vấn");
            actionBar = getSupportActionBar();
            actionBar.setDisplayHomeAsUpEnabled(true);
            actionBar.setHomeButtonEnabled(true);
        } else {
            // Khởi tạo Fragment đầu tiên cho main activity
            Fragment fragment = new FragmentMenu();
            Utils.ChangeFragment(this, fragment, null);
            actionBar = getSupportActionBar();
            actionBar.setDisplayHomeAsUpEnabled(true);
            actionBar.setHomeButtonEnabled(true);
        }

        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });

        prefsNotify = activity.getSharedPreferences(Constants.Key_Notify + loginProfileModel.tenDangNhap, MODE_PRIVATE);
        setTotalNotify();

        checkPermissions();
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
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            FragmentManager fragmentManager = ((FragmentActivity) activity).getSupportFragmentManager();
            Fragment f = fragmentManager.findFragmentById(R.id.frame_content);
            FragmentMenu fragmentMenu = null;
            if (fragmentManager.findFragmentByTag("xxx") instanceof FragmentMenu) {
                AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity.this);
                builder.setTitle("Đăng xuất!");
                builder.setMessage("Bạn có chắc muốn đăng xuất?");
                builder.setPositiveButton("Đăng xuất", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.logout) + loginProfileModel.accessToken)
                                .addHeaders("Authorization", loginProfileModel.accessToken)
                                .setPriority(Priority.MEDIUM)
                                .build()
                                .getAsString(new StringRequestListener() {
                                    @Override
                                    public void onResponse(String response) {
                                        prefsLogin.edit().remove(Constants.LoginProfile).commit();
                                    }

                                    @Override
                                    public void onError(ANError anError) {
                                        //progressDialog.dismiss();
                                        Utils.showErrorMessage(getApplication(), anError);
                                    }
                                });

                        finish();
                    }
                });
                builder.setNegativeButton("Hủy", null);
                builder.show();
            } else if (fragmentManager.getBackStackEntryCount() == 1 && f instanceof FagmentDetailCatuvan) {
                // Khởi tạo Fragment đầu tiên cho main activity
                Fragment fragment = new FragmentMenu();
                Utils.ChangeFragment(this, fragment, null);
                actionBar = getSupportActionBar();
                actionBar.setDisplayHomeAsUpEnabled(true);
                actionBar.setHomeButtonEnabled(true);
            } else {
                super.onBackPressed();
            }
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        //getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        item.setChecked(true);
        actionBar.setTitle(item.getTitle());
        int id = item.getItemId();
        Fragment fragment = null;
        if (id == R.id.nav_camera) {
            fragment = new ListReportFragment();
        } else if (id == R.id.nav_create_profile) {
            fragment = new ReportCaseCreateFragment();
        } else if (id == R.id.nav_logout) {
            prefsLogin.edit().remove(Constants.LoginProfile).commit();
            Intent loginActivity = new Intent(getApplicationContext(), LoginActivity.class);
            startActivity(loginActivity);
        } else if (id == R.id.nav_timeline) {
            fragment = new ListSearchReportFragment();
        }

        if (fragment != null) {
            Utils.ChangeFragment(this, fragment, null);
        }

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

    @Override
    protected void onStop() {
        // Unbind from the service
        if (mBound) {
            mBound = false;
        }
        super.onStop();
        LocalBroadcastManager.getInstance(this).unregisterReceiver(mMessageReceiver);
    }

    @Override
    protected void onStart() {
        //Unbind from the service
        if (!mBound) {
            mBound = true;
        }
        super.onStart();
        LocalBroadcastManager.getInstance(this).registerReceiver((mMessageReceiver),
                new IntentFilter("ChangeNotify")
        );
    }

    private BroadcastReceiver mMessageReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            setTotalNotify();
        }
    };

    @Override
    public void onResume() {
        super.onResume();

        setTotalNotify();
    }

    /**
     * Hiển thị số lượng notify
     */
    public static void setTotalNotify() {
        prefsNotify = activity.getSharedPreferences(Constants.Key_Notify + loginProfileModel.tenDangNhap, MODE_PRIVATE);
        int total = prefsNotify.getInt(Constants.NumOfNotify, 0);
        txtTotalNotify.setText(String.valueOf(total));
    }
}
