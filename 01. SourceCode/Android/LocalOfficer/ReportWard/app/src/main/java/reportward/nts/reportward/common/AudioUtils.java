package reportward.nts.reportward.common;

import android.content.Context;
import android.media.Ringtone;
import android.media.RingtoneManager;
import android.net.Uri;

/**
 * Created by NTS-VANVV on 09/08/2018.
 */

public class AudioUtils {
    private static Uri notification = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);

    public static void audioBeep(Context app) {
        //set up MediaPlayer
        try {

            Ringtone r = RingtoneManager.getRingtone(app, notification);
            r.play();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
