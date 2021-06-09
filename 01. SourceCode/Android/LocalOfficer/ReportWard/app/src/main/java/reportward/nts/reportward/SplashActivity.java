package reportward.nts.reportward;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.LoginProfileModel;

/**
 * A login screen that offers login via email/password.
 */
public class SplashActivity extends Activity {
    /**
     * Duration of wait
     **/
    private final int SPLASH_DISPLAY_LENGTH = 5000;
    private  Activity activity;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash);

        activity = this;

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                SharedPreferences prefsLogin = getSharedPreferences(Constants.Key_Info_Login, MODE_PRIVATE);
                String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
                LoginProfileModel loginProfileModel = null;
                if (loginProfile != null) {
                    loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
                    }.getType());

                    if (loginProfileModel != null && !Utils.isEmpty(loginProfileModel.tenDangNhap) && !Utils.isEmpty(loginProfileModel.accessToken) && Utils.checkConnectedNetwork(activity)) {
                        Intent mainActivity = new Intent(getApplicationContext(), MainActivity.class);
                        startActivity(mainActivity);
                        finish();
                        return;
                    }
                }
                /* Create an Intent that will start the Menu-Activity. */
                Intent loginActivity = new Intent(getApplicationContext(), LoginActivity.class);
                startActivity(loginActivity);
                finish();
            }
        }, SPLASH_DISPLAY_LENGTH);
    }
}

