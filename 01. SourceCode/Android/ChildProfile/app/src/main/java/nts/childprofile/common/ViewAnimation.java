package nts.childprofile.common;

import android.view.View;

public class ViewAnimation {
    public static void fadeIn(final View v) {
        if (v != null) {
            v.setVisibility(View.VISIBLE);
        }
    }

    public static void fadeOut(final View v) {
        if (v != null) {
            v.setVisibility(View.GONE);
        }
    }
}
