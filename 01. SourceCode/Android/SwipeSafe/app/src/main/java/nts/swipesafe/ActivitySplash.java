package nts.swipesafe;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.widget.ImageView;
import android.widget.LinearLayout;

import java.util.Timer;
import java.util.TimerTask;

import nts.swipesafe.services.ReadDataLocalService;

public class ActivitySplash extends AppCompatActivity {

    private ImageView bg_splash;
    private float valueRotation = 0;
    private float countRotation = 0;
    private Handler handler = new Handler();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash);

        bg_splash = this.findViewById(R.id.bg_splash);

        final TimerTask task = new TimerTask() {
            @Override
            public void run() {
                // go to the main activity
                Intent i = new Intent(getApplicationContext(), MainActivity.class);
                startActivity(i);
                // kill current activity
                finish();
            }
        };

        new Thread(new Runnable() {
            public void run() {
                while (countRotation < 1) {
                    valueRotation += 1;
                    // Update the progress bar and display the
                    //current value in the text view
                    handler.post(new Runnable() {
                        public void run() {
                            bg_splash.setRotation(valueRotation);
                            if (valueRotation >= 360) {
                                valueRotation = 0;
                                countRotation++;
                            }
                        }
                    });
                    try {
                        // Sleep for 200 milliseconds.
                        Thread.sleep(5);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }

                    if (countRotation == 1) {
                        Intent i = new Intent(getApplicationContext(), MainActivity.class);
                        startActivity(i);
                        // kill current activity
                        finish();
                    }
                }
            }
        }).start();

        startService();
    }

    public void startService() {
        Intent intent = new Intent(this, ReadDataLocalService.class);
        startService(intent);
    }

    public void stopService() {
        Intent intent = new Intent(this, ReadDataLocalService.class);
        stopService(intent);
    }
}
