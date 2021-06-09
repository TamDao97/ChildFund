package reportward.nts.reportward.common;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.media.RingtoneManager;
import android.net.Uri;
import android.support.v4.app.NotificationCompat;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.LocalBroadcastManager;

import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import me.leolin.shortcutbadger.ShortcutBadger;
import reportward.nts.reportward.MainActivity;
import reportward.nts.reportward.R;
import reportward.nts.reportward.model.InfoNotification;
import reportward.nts.reportward.model.LoginProfileModel;

import static android.content.Intent.FLAG_ACTIVITY_CLEAR_TOP;
import static android.content.Intent.FLAG_ACTIVITY_NEW_TASK;

public class MyFirebaseMessagingService extends FirebaseMessagingService {
    private LocalBroadcastManager broadcaster;

    @Override
    public void onCreate() {
        broadcaster = LocalBroadcastManager.getInstance(this);
    }

    @Override
    public void onMessageReceived(RemoteMessage message) {
        super.onMessageReceived(message);

        if (message.getNotification() != null) {
            try {
                Context c = getApplicationContext();

                NotificationCompat.Builder mBuilder = null;
                NotificationManager notificationManager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);

                Uri defaultSoundUri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);

                // The id of the channel.
                final String CHANNEL_ID = "report_channel_id";
                // The user-visible name of the channel.
                final String CHANNEL_NAME = "Report Ward Channel";
                if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.O) {
                    NotificationChannel notificationChannel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationManager.IMPORTANCE_HIGH);
                    //Disabling vibration!
                    notificationChannel.enableVibration(false);
                    notificationManager.createNotificationChannel(notificationChannel);
                    mBuilder = new NotificationCompat.Builder(this, CHANNEL_ID);
                } else {
                    mBuilder = new NotificationCompat.Builder(this);
                    mBuilder.setVibrate(new long[]{0L});
                }

                Map<String, String> params = message.getData();
                JSONObject object = new JSONObject(params);
                String maSoHoSo = object != null ? object.getString("maSoHoSo") : "";

                Intent intent = new Intent(this, MainActivity.class);
                intent.putExtra("maSoHoSo", maSoHoSo);
                intent.setFlags(FLAG_ACTIVITY_NEW_TASK | FLAG_ACTIVITY_CLEAR_TOP);
                PendingIntent pendingIntent = PendingIntent.getActivity(c, 0, intent, PendingIntent.FLAG_UPDATE_CURRENT);



                mBuilder.setSmallIcon(R.drawable.ic_launcher)
                        .setColor(ContextCompat.getColor(c, R.color.colorPrimary))
                        .setContentTitle("Thông báo từ vấn từ tổng đài quốc gia bảo vệ trẻ em")
                        .setContentText(message.getNotification().getBody())
                        .setWhen(System.currentTimeMillis())
                        .setSound(defaultSoundUri)
                        .setDefaults(Notification.DEFAULT_SOUND | Notification.DEFAULT_VIBRATE)
                        .setLights(ContextCompat.getColor(c, R.color.colorPrimary), 5000, 5000)
                        .setAutoCancel(true)
                        .setPriority(NotificationCompat.PRIORITY_MAX)
                        .setContentIntent(pendingIntent);

                Notification notification = mBuilder.build();

                notificationManager.notify("reportward.nts.reportward", 0, notification);

                SharedPreferences prefsLogin = getSharedPreferences(Constants.Key_Info_Login, MODE_PRIVATE);
                String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
                LoginProfileModel loginProfileModel = null;
                if (loginProfile != null) {
                    loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
                    }.getType());
                }

                if (loginProfileModel != null && !Utils.isEmpty(loginProfileModel.tenDangNhap)) {
                    //Lưu bản ghi thông báo lại trên máy
                    SharedPreferences prefsNotify = getSharedPreferences(Constants.Key_Notify + loginProfileModel.tenDangNhap, MODE_PRIVATE);
                    InfoNotification infoNotification = new InfoNotification();
                    infoNotification.maSoCaTuVan = maSoHoSo;
                    infoNotification.date = DateUtils.CurrentDate(DateUtils.DATE_FORMAT_DD_MM_YYYY_HH_MM);
                    infoNotification.massenger = message.getNotification().getBody() + " " + maSoHoSo;
                    infoNotification.viewStatus = false;

                    String current = prefsNotify.getString(Constants.ListNotify, "");
                    List<InfoNotification> listNotify = new Gson().fromJson(current, new TypeToken<List<InfoNotification>>() {
                    }.getType());

                    if (listNotify == null) {
                        listNotify = new ArrayList<>();
                    }

                    if (listNotify.size() > 1000) {
                        listNotify.remove(1000);
                    }
                    listNotify.add(0, infoNotification);

                    prefsNotify.edit().putString(Constants.ListNotify, new Gson().toJson(listNotify)).commit();

                    //Đêm số lượng notify
                    int count = prefsNotify.getInt(Constants.NumOfNotify, 0);
                    count = count + 1;

                    // Then apply it
                    ShortcutBadger.applyCount(getBaseContext(), count);

                    // After that, you need to save the value again for another badge count
                    SharedPreferences.Editor editor = prefsNotify.edit();
                    editor.putInt("NumOfNotify", count);
                    editor.commit();
                    ShortcutBadger.applyCount(getApplicationContext(), count);

                    Intent intentChange = new Intent("ChangeNotify");
                    broadcaster.sendBroadcast(intentChange);
                }
            } catch (Exception ex) {
                String temp = ex.getMessage();
            }
        }
    }
}
