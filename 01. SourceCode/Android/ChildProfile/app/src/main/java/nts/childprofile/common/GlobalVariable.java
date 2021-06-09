package nts.childprofile.common;

import android.app.Application;

import nts.childprofile.model.LoginProfileModel;

/**
 * Created by NTS-VANVV on 29/12/2018.
 */

public class GlobalVariable extends Application {
    private LoginProfileModel loginProfileModel = new LoginProfileModel ();
    private boolean StatusRunApp;

    public void addLoginProfile(LoginProfileModel model) {
        loginProfileModel = model;
    }

    public void removeLoginProfile() {
        loginProfileModel = new LoginProfileModel ();
    }

    public LoginProfileModel getLoginProfile() {
        return loginProfileModel;
    }

    public boolean isStatusRunApp() {
        return StatusRunApp;
    }

    public void setStatusRunApp(boolean statusRunApp) {
        StatusRunApp = statusRunApp;
    }
}
