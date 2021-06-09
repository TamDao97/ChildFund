package nts.childprofile;

import android.Manifest;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.res.Configuration;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.ActionBar;
import android.view.View;
import android.support.design.widget.NavigationView;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import nts.childprofile.Sql.DataBaseHelper;
import nts.childprofile.Sql.SyncCombobox;
import nts.childprofile.common.GlobalVariable;
import nts.childprofile.common.Utils;
import nts.childprofile.fragment.ChangePasswordFragment;
import nts.childprofile.fragment.CreateProfileChildFragment;
import nts.childprofile.fragment.MenuFragment;
import nts.childprofile.fragment.ProfileListFragment;
import nts.childprofile.fragment.ProfileUserFragment;
import nts.childprofile.fragment.UpdateProfileChildFragment;
import nts.childprofile.fragment.UploadImage;
import nts.childprofile.model.LoginProfileModel;

import static com.androidnetworking.internal.ANRequestQueue.initialize;

public class MainActivity extends AppCompatActivity {
    private DrawerLayout drawerLayout;
    private Toolbar mToolbar;
    private ActionBarDrawerToggle mDrawerToggle;
    private ActionBar actionBar;
    private Menu menu;
    private View parent_view;
    private NavigationView nav_view;
    private GlobalVariable global;
    private LoginProfileModel loginProfileModel;

    /**
     * permissions request code
     */
    private final static int REQUEST_CODE_ASK_PERMISSIONS = 1;

    /**
     * Permissions that need to be explicitly requested from end user.
     */
    private static final String[] REQUIRED_SDK_PERMISSIONS = new String[]{
            Manifest.permission.WRITE_EXTERNAL_STORAGE, Manifest.permission.READ_EXTERNAL_STORAGE,
            Manifest.permission.CAMERA};

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        parent_view = findViewById(R.id.main_content);

        checkPermissions();

        global = (GlobalVariable) getApplication();
        loginProfileModel = global.getLoginProfile();

        if (loginProfileModel == null || loginProfileModel.Id.isEmpty()) {
            Intent mainActivity = new Intent(getApplicationContext(), LoginActivity.class);
            startActivity(mainActivity);
        }

        initToolbar();

        setupDrawerLayout();
        Bundle bundle = new Bundle();
        // display first page
        Fragment fragment = new MenuFragment();
        ((MenuFragment) fragment).loginProfileModel = loginProfileModel;
        Utils.ChangeFragment(this, fragment, bundle);
//        genDatabaseSQLite();
    }

    private void genDatabaseSQLite() {
        DataBaseHelper dataBaseHelper = new DataBaseHelper(this, "ChildFund.db", null, 1);
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Ethnic(Id VARCHAR(36) PRIMARY KEY NOT NULL, Name NVARCHAR(150) NOT NULL, OrderNumber INT)");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Job(Id VARCHAR(36) PRIMARY KEY NOT NULL, NameEn NVARCHAR(100), Name NVARCHAR(100), OrderNumber INT)");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Relationship(Id VARCHAR(36) PRIMARY KEY NOT NULL, Name NVARCHAR(100) NOT NULL, Gender INT, OrderNumber INT)");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Religion(Id VARCHAR(36) PRIMARY KEY NOT NULL, Name NVARCHAR(150) NOT NULL, OrderNumber INT)");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS School(Id VARCHAR(36) PRIMARY KEY NOT NULL, WardId VARCHAR(36), SchoolName NVARCHAR(150))");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS ReportProfile(Id VARCHAR(36) PRIMARY KEY NOT NULL, Content NVARCHAR(500), ChildProfileId VARCHAR(36) NOT NULL, ProcessStatus CHAR(1) NOT NULL, AreaApproverBy VARCHAR(36), AreaApproveDate DATETIME, OfficeApproveBy VARCHAR(36), OfficeApproveDate DATETIME, IsDelete INT NOT NULL, CreateBy VARCHAR(36),CreateDate DATETIME, UpdateBy VARCHAR(36), UpdateDate DATETIME, Description NVARCHAR(MAX),AreaApproverNotes NVARCHAR(MAX))");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Village(Id VARCHAR(36) PRIMARY KEY NOT NULL, WardId VARCHAR(36) NOT NULL, NameEN NVARCHAR(150), Name NVARCHAR(150) NOT NULL, Type NVARCHAR(50))");
        SyncCombobox syncCombobox=new SyncCombobox();
        syncCombobox.syncComboboxEthnic();
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
        if (!missingPermissions.isEmpty()) {
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

    private void initToolbar() {
        mToolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(mToolbar);
        actionBar = getSupportActionBar();
        actionBar.setDisplayHomeAsUpEnabled(true);
        actionBar.setHomeButtonEnabled(true);

    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    @Override
    protected void onPostCreate(Bundle savedInstanceState) {
        super.onPostCreate(savedInstanceState);
        mDrawerToggle.syncState();
        mDrawerToggle.setDrawerIndicatorEnabled(false);
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        mDrawerToggle.onConfigurationChanged(newConfig);
    }

    private void setupDrawerLayout() {
        drawerLayout = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawerLayout.setDrawerLockMode(DrawerLayout.LOCK_MODE_LOCKED_CLOSED);
        nav_view = (NavigationView) findViewById(R.id.navigation_view);
        View headerLayout = nav_view.getHeaderView(0);
        ImageView imageView = headerLayout.findViewById(R.id.imageView);
        try {
            if (loginProfileModel.ImagePath != null && !loginProfileModel.ImagePath.isEmpty()) {
                Picasso.with(this).load(loginProfileModel.ImagePath).resize(208, 208)
                        .centerInside().into(imageView);
            } else {
                Picasso.with(this).load(R.drawable.ic_people).resize(300, 300)
                        .centerInside().into(imageView);
            }
        } catch (Exception ex) {
        }

        TextView headertitle = headerLayout.findViewById(R.id.header_title);
        headertitle.setText(loginProfileModel.Name);
        mDrawerToggle = new ActionBarDrawerToggle(this, drawerLayout, mToolbar, R.string.drawer_open, R.string.drawer_close) {
            @Override
            public void onDrawerOpened(View drawerView) {
                super.onDrawerOpened(drawerView);
            }
        };

        mDrawerToggle.setToolbarNavigationClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });

        // Set the drawer toggle as the DrawerListener
        drawerLayout.setDrawerListener(mDrawerToggle);

        nav_view.setNavigationItemSelectedListener(new NavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(MenuItem menuItem) {
                menuItem.setChecked(true);
                drawerLayout.closeDrawers();
                actionBar.setTitle(menuItem.getTitle());
                displayView(menuItem.getItemId(), menuItem.getTitle().toString());
                return true;
            }
        });
    }

    private void displayView(int id, String title) {
        actionBar.setDisplayShowCustomEnabled(false);
        actionBar.setDisplayShowTitleEnabled(true);
        Fragment fragment = null;
        Bundle bundle = new Bundle();
        switch (id) {
            case R.id.nav_profile:
                fragment = new ProfileListFragment();
                break;
            case R.id.nav_create_profile:
                fragment = new CreateProfileChildFragment();
                break;
            case R.id.nav_manage:
                fragment = new UploadImage();
                break;
            case R.id.nav_profileuser:
                fragment = new ProfileUserFragment();
                break;
            case R.id.nav_changepassword:
                fragment = new ChangePasswordFragment();
                break;
            case R.id.nav_logout:
                global.removeLoginProfile();
                Intent mainActivity = new Intent(getApplicationContext(), LoginActivity.class);
                startActivity(mainActivity);
                break;
            default:
                break;
        }

        if (fragment != null) {
            Utils.ChangeFragment(this, fragment, bundle);
        }
    }

    private long exitTime = 0;

    public void doExitApp() {
        if ((System.currentTimeMillis() - exitTime) > 2000) {
            Toast.makeText(this, "Press again to exit", Toast.LENGTH_SHORT).show();
            exitTime = System.currentTimeMillis();
        } else {
            super.onBackPressed();
        }

    }

    @Override
    public void onBackPressed() {
        //doExitApp();
        FragmentManager fragmentManager = getSupportFragmentManager();
        Fragment f = getSupportFragmentManager().findFragmentById(R.id.frame_content);
        MenuFragment menuFragment = null;
        if (fragmentManager.findFragmentByTag("xxx") instanceof MenuFragment) {
            AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity.this);
            builder.setTitle("Đăng xuất");
            builder.setMessage("Bạn muốn thoát khỏi ứng dụng/You want to exit the application?");
            builder.setPositiveButton("Đồng ý/Ok", new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    finish();
                }
            });
            builder.setNegativeButton("Huỷ/Cancel", null);
            builder.show();
            return;
        } else if (fragmentManager.findFragmentByTag("xxx") instanceof CreateProfileChildFragment) {
            ((CreateProfileChildFragment) f).backListProfile();
            return;
        } else if (fragmentManager.findFragmentByTag("xxx") instanceof UpdateProfileChildFragment) {
            ((UpdateProfileChildFragment) f).backListProfile();
            return;
        } else if (fragmentManager.findFragmentByTag("xxx") instanceof ProfileListFragment) {
            ((ProfileListFragment) f).backMenu();
            return;
        }
        super.onBackPressed();
    }
}
