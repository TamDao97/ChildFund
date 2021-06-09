package nts.swipesafe.common;

import android.app.Activity;
import android.app.Application;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.Matrix;
import android.media.ExifInterface;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.os.Environment;
import android.os.Handler;
import android.support.constraint.ConstraintLayout;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.content.ContextCompat;
import android.support.v4.graphics.drawable.DrawableCompat;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.ANRequest;
import com.androidnetworking.common.ANResponse;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.AnalyticsListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.io.StringWriter;
import java.io.Writer;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import nts.swipesafe.R;
import nts.swipesafe.fragment.ReportMainFragment;
import nts.swipesafe.model.ChildAbuseModel;
import nts.swipesafe.model.ComboboxItem;
import nts.swipesafe.model.ComboboxResult;

/**
 * Created by NTS-VANVV on 26/12/2018.
 */

public class Utils {
    /**
     * Lưu ảnh
     *
     * @param bitmap
     * @param imagePath
     */
    public static void SaveBitmapToImage(Bitmap bitmap, String imagePath) {
        try {
            File file = new File(imagePath);
            FileOutputStream fOut = new FileOutputStream(file);
            bitmap.compress(Bitmap.CompressFormat.JPEG, 85, fOut);
        } catch (Exception ex) {
        }
    }

    /**
     * Quay ảnh
     *
     * @param source
     * @param angle
     * @return
     */
    public static Bitmap RotateImage(Bitmap source, float angle) {
        Matrix matrix = new Matrix();
        matrix.postRotate(angle);
        return Bitmap.createBitmap(source, 0, 0, source.getWidth(), source.getHeight(),
                matrix, true);
    }

    /**
     * Cài đặt thông số ảnh
     *
     * @param imgView
     * @param imagePath
     */
    public static void SetPicture(ImageView imgView, String imagePath) {
        // TODO config
        // Get the dimensions of the View
        int targetW = imgView.getWidth();
        int targetH = imgView.getHeight();

        // Get the dimensions of the bitmap
        BitmapFactory.Options bmOptions = new BitmapFactory.Options();
        bmOptions.inJustDecodeBounds = true;
        BitmapFactory.decodeFile(imagePath, bmOptions);
        int photoW = bmOptions.outWidth;
        int photoH = bmOptions.outHeight;

        // Determine how much to scale down the image
        //int scaleFactor = Math.min(photoW / targetW, photoH / targetH);

        // Decode the image file into a Bitmap sized to fill the View
        bmOptions.inJustDecodeBounds = false;
        //bmOptions.inSampleSize = scaleFactor;
        bmOptions.inPurgeable = true;

        Bitmap bitmap = BitmapFactory.decodeFile(imagePath, bmOptions);

        try {
            ExifInterface ei = new ExifInterface(imagePath);
            int orientation = ei.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_UNDEFINED);
            switch (orientation) {

                case ExifInterface.ORIENTATION_ROTATE_90:
                    bitmap = RotateImage(bitmap, 90);
                    imgView.setImageBitmap(bitmap);
                    SaveBitmapToImage(bitmap, imagePath);
                    break;

                case ExifInterface.ORIENTATION_ROTATE_180:
                    bitmap = RotateImage(bitmap, 180);
                    imgView.setImageBitmap(bitmap);
                    SaveBitmapToImage(bitmap, imagePath);
                    break;

                case ExifInterface.ORIENTATION_ROTATE_270:
                    bitmap = RotateImage(bitmap, 270);
                    imgView.setImageBitmap(bitmap);
                    SaveBitmapToImage(bitmap, imagePath);
                    break;

                case ExifInterface.ORIENTATION_NORMAL:
                    imgView.setImageBitmap(bitmap);
                    break;

                default:
                    imgView.setImageBitmap(bitmap);
                    break;
            }
        } catch (Exception e) {
            Log.e("CAMERA ROTATE", e.getMessage());
        }
    }

    /**
     * Lưu ảnh chup vào file image
     *
     * @param imagePath
     */
    public static void SavePicture(String imagePath) {
        // Get the dimensions of the bitmap
        BitmapFactory.Options bmOptions = new BitmapFactory.Options();
        bmOptions.inJustDecodeBounds = true;
        BitmapFactory.decodeFile(imagePath, bmOptions);
        int photoW = bmOptions.outWidth;
        int photoH = bmOptions.outHeight;

        // Determine how much to scale down the image
        //int scaleFactor = Math.min(photoW / targetW, photoH / targetH);

        // Decode the image file into a Bitmap sized to fill the View
        bmOptions.inJustDecodeBounds = false;
        //bmOptions.inSampleSize = scaleFactor;
        bmOptions.inPurgeable = true;

        Bitmap bitmap = BitmapFactory.decodeFile(imagePath, bmOptions);

        try {
            ExifInterface ei = new ExifInterface(imagePath);
            int orientation = ei.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_UNDEFINED);
            switch (orientation) {

                case ExifInterface.ORIENTATION_ROTATE_90:
                    bitmap = RotateImage(bitmap, 90);
                    SaveBitmapToImage(bitmap, imagePath);
                    break;

                case ExifInterface.ORIENTATION_ROTATE_180:
                    bitmap = RotateImage(bitmap, 180);
                    SaveBitmapToImage(bitmap, imagePath);
                    break;

                case ExifInterface.ORIENTATION_ROTATE_270:
                    bitmap = RotateImage(bitmap, 270);
                    SaveBitmapToImage(bitmap, imagePath);
                    break;
            }
        } catch (Exception e) {
            Log.e("CAMERA ROTATE", e.getMessage());
        }
    }

    /**
     * Tạo file
     *
     * @return
     * @throws IOException
     */
    public static File CreateImageFile(Context context) throws IOException {
        // Create an image file name
        String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").format(new Date());
        String imageFileName = "JPEG_" + timeStamp + "_";
        File storageDir = context.getExternalFilesDir(Environment.DIRECTORY_PICTURES);

        File image = File.createTempFile(
                imageFileName,  /* prefix */
                ".jpg",         /* suffix */
                storageDir      /* directory */
        );

        // Save a file: path for use with ACTION_VIEW intents
        //pathImageFile = image.getAbsolutePath();
        return image;
    }

    /***
     *
     * @param editText
     * @return
     */
    public static String getText(EditText editText) {
        return editText.getText() != null ? editText.getText().toString() : "";
    }

    /**
     * Get index by name
     *
     * @param array
     * @return
     */
    public static int getIndexByName(String[] array, String name) {
        int index = 0;
        try {

            for (; index < array.length; index++) {
                if (array[index].equals(name)) {
                    return index;
                }
            }
        } catch (Exception ex) {
        }
        return index;
    }

    /**
     * Get index by name
     *
     * @param array
     * @return
     */
    public static String getNameById(List<ComboboxResult> array, String id) {
        try {

            for (ComboboxResult item : array) {
                if (item.id.equals(id)) {
                    return item.text;
                }
            }
        } catch (Exception ex) {
        }
        return "";
    }

    /**
     * Get Id by name
     *
     * @param array
     * @return
     */
    public static String getIdByName(List<ComboboxResult> array, String name) {
        try {

            for (ComboboxResult item : array) {
                if (item.text.equals(name)) {
                    return item.id;
                }
            }
        } catch (Exception ex) {
        }
        return "";
    }

    /**
     * Get Id by name
     *
     * @param array
     * @return
     */
    public static String getCodeByName(List<ComboboxItem> array, String name) {
        try {

            for (ComboboxItem item : array) {
                if (item.name.replace("TP ", "").equals(name)) {
                    return item.code;
                }
            }
        } catch (Exception ex) {
        }
        return "";
    }

    /**
     * validate your email address format. Ex-akhi@mani.com
     */
    public static boolean emailValidator(String email) {
        Pattern pattern;
        Matcher matcher;
        final String EMAIL_PATTERN = "^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
        pattern = Pattern.compile(EMAIL_PATTERN);
        matcher = pattern.matcher(email);
        return matcher.matches();
    }


    /**
     * validate your phoneNumber format. Ex-akhi@mani.com
     */
    public static boolean phoneNumberValidator(String phoneNumber) {
        Pattern pattern;
        Matcher matcher;
        final String EMAIL_PATTERN = "(0)+([1-9]{1})+([0-9]{8})\\b";
        pattern = Pattern.compile(EMAIL_PATTERN);
        matcher = pattern.matcher(phoneNumber);
        return matcher.matches();
    }

    /**
     * validate your email address format. Ex-akhi@mani.com
     */
    public static boolean isEmpty(String text) {
        if (text == null || text.trim().isEmpty() || text.toLowerCase().equals("null")) {
            return true;
        }
        return false;
    }

    /***
     * Thay đổi fragment
     * @param activity
     * @param fragment
     * @param bundle
     */
    public static void ChangeFragment(final Activity activity, final android.support.v4.app.Fragment fragment, Bundle bundle) {
        if (fragment != null) {
            if (bundle != null) {
                fragment.setArguments(bundle);
            }
            FragmentManager fragmentManager = ((FragmentActivity) activity).getSupportFragmentManager();
            FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
            fragmentTransaction.replace(R.id.frame_content, fragment);
            fragmentTransaction.commit();

            final Handler handler = new Handler();
            handler.postDelayed(new Runnable() {
                @Override
                public void run() {
                    ConstraintLayout container = activity.findViewById(R.id.container);
                    View toolBar = activity.findViewById(R.id.toolBar);
                    ImageView imgMenu = toolBar.findViewById(R.id.imgMenu);
                    View toolbarLine = toolBar.findViewById(R.id.toolbarLine);
                    if (fragment instanceof ReportMainFragment) {
                        container.setBackgroundResource(R.drawable.background_gradient);
                        DrawableCompat.setTint(imgMenu.getDrawable(),
                                ContextCompat.getColor(activity, R.color.white));
                        toolbarLine.setBackgroundResource(R.color.white);
                    } else {
                        container.setBackgroundColor(Color.parseColor("#FFFFFF"));
                        DrawableCompat.setTint(imgMenu.getDrawable(),
                                ContextCompat.getColor(activity, R.color.blue));
                        toolbarLine.setBackgroundResource(R.color.blue);
                    }
                }
            }, 150);

        }
    }

    /***
     * Hiển thị trong tin lỗi
     * @param anError
     * @return
     */
    public static void showErrorMessage(Application activity, ANError anError) {
        String message = "Lỗi phát sinh trong hệ thống. Vui lòng thử lại.";
        if (anError.getErrorBody() != null) {
            message = anError.getErrorBody().toString();
        } else if (anError.getMessage() != null) {
            message = anError.getMessage().toString();
        }
        Toast.makeText(activity, message, Toast.LENGTH_SHORT).show();
    }

    /***
     * Đọc file json từ Asset
     * @param context
     * @param fileName
     * @return
     */
    public static String ReadJSONFromAsset(Context context, String fileName) {
        String json = null;
        try {
            InputStream is = context.getAssets().open(fileName);

            int size = is.available();

            byte[] buffer = new byte[size];

            is.read(buffer);

            is.close();

            json = new String(buffer, "UTF-8");
        } catch (IOException ex) {
            ex.printStackTrace();
            return null;
        }
        return json;

    }

    public static String StreamToString(InputStream in) throws IOException {
        if (in == null) {
            return "";
        }
        Writer writer = new StringWriter();
        char[] buffer = new char[1024];
        try {
            Reader reader = new BufferedReader(new InputStreamReader(in, "UTF-8"));
            int n;
            while ((n = reader.read(buffer)) != -1) {
                writer.write(buffer, 0, n);
            }
        } finally {
        }
        return writer.toString();
    }

    public static String getUrlAPIByArea(String area, String url) {
        switch (area) {
            case "B":
                return Constants.ApiUrlBac + url;
            case "T":
                return Constants.ApiUrlTrung + url;
            case "N":
                return Constants.ApiUrlNam + url;
            default:
                return Constants.ApiUrlBac + url;
        }
    }

    /***
     * Check kết nố mạng
     * @param context
     * @return
     */
    public static boolean checkConnectedNetwork(Context context)
    {
        boolean connected = false;
        ConnectivityManager connectivityManager = (ConnectivityManager)context.getSystemService(Context.CONNECTIVITY_SERVICE);
        if(connectivityManager.getNetworkInfo(ConnectivityManager.TYPE_MOBILE).getState() == NetworkInfo.State.CONNECTED ||
                connectivityManager.getNetworkInfo(ConnectivityManager.TYPE_WIFI).getState() == NetworkInfo.State.CONNECTED) {
            //we are connected to a network
            connected = true;
        }
        else
            connected = false;
        return  connected;
    }
}
