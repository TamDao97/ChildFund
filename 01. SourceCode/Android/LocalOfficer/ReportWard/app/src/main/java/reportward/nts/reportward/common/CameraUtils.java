package reportward.nts.reportward.common;

import android.Manifest;
import android.content.Context;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Matrix;
import android.media.ExifInterface;
import android.os.Environment;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.ImageView;

import com.androidnetworking.AndroidNetworking;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by NTS-VANVV on 04/01/2019.
 */

public class CameraUtils {
    /**
     * Yêu cầu quyền đọc ghi dữ liệu
     */
    public static final int REQUEST_ID_READ_WRITE_PERMISSION = 99;

    /**
     * Yêu cầu chụp ảnh
     */
    public static final int REQUEST_ID_IMAGE_CAPTURE = 100;
    /***
     * Chụp ảnh số khung
     */
    public static final int FRAME_NO_IMAGE_CAPTURE = 9001;
    /***
     * Chụp ảnh số máy
     */
    public static final int ENGINE_NO_IMAGE_CAPTURE = 9002;

    /**
     * Yêu cầu quyền cấp bộ nhớ
     *
     * @param context
     */
    public static void RequirePermission(AppCompatActivity context) {
        if (android.os.Build.VERSION.SDK_INT >= 23) {
            // Kiểm tra quyền đọcadmin  /ghi dữ liệu vào thiết bị lưu trữ ngoài.
            int readPermission = ActivityCompat.checkSelfPermission(context, Manifest.permission.READ_EXTERNAL_STORAGE);
            int writePermission = ActivityCompat.checkSelfPermission(context, Manifest.permission.WRITE_EXTERNAL_STORAGE);

            if (writePermission != PackageManager.PERMISSION_GRANTED || readPermission != PackageManager.PERMISSION_GRANTED) {
                // Nếu không có quyền, cần nhắc người dùng cho phép.
                context.requestPermissions(new String[]{Manifest.permission.WRITE_EXTERNAL_STORAGE, Manifest.permission.READ_EXTERNAL_STORAGE}, REQUEST_ID_READ_WRITE_PERMISSION);
                return;
            }
        }
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
        int scaleFactor = Math.min(photoW / targetW, photoH / targetH);

        // Decode the image file into a Bitmap sized to fill the View
        bmOptions.inJustDecodeBounds = false;
        bmOptions.inSampleSize = scaleFactor;
        bmOptions.inPurgeable = true;

        Bitmap bitmap = BitmapFactory.decodeFile(imagePath, bmOptions);

        try {
            ExifInterface ei = new ExifInterface(imagePath);
            int orientation = ei.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_UNDEFINED);
            switch (orientation) {

                case ExifInterface.ORIENTATION_ROTATE_90:
                    imgView.setImageBitmap(RotateImage(bitmap, 90));
                    break;

                case ExifInterface.ORIENTATION_ROTATE_180:
                    imgView.setImageBitmap(RotateImage(bitmap, 180));
                    break;

                case ExifInterface.ORIENTATION_ROTATE_270:
                    imgView.setImageBitmap(RotateImage(bitmap, 270));
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
     * Hiện thị hình ảnh từ server
     *
     * @param imageUrl
     */
    public static void ShowPictureFromUrl(final ImageView imgView, String imageUrl) {
//        AndroidNetworking.get(Constants.APIURL + imageUrl)
//                .setTag("imageRequestTag")
//                .setPriority(Priority.MEDIUM)
//                .setBitmapMaxHeight(300)
//                .setBitmapMaxWidth(300)
//                .setBitmapConfig(Bitmap.Config.ARGB_8888)
//                .build()
//                .getAsBitmap(new BitmapRequestListener() {
//                    @Override
//                    public void onResponse(Bitmap bitmap) {
//                        imgView.setImageBitmap(bitmap);
//                    }
//
//                    @Override
//                    public void onError(ANError error) {
//
//                    }
//                });
    }

    /**
     * Tạo file
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
}
