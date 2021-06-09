package nts.childprofile.common;

import android.app.Activity;
import android.app.Application;
import android.content.ContentValues;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Matrix;
import android.media.ExifInterface;
import android.os.Bundle;
import android.os.Environment;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.util.Log;
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

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import nts.childprofile.R;
import nts.childprofile.Sql.DataBaseHelper;
import nts.childprofile.Sql.SyncCombobox;
import nts.childprofile.model.ComboboxResult;
import nts.childprofile.model.LoginProfileModel;

/**
 * Created by NTS-VANVV on 26/12/2018.
 */

public class Utils {
    public static String GetUrlApi(String url) {
        return Constants.ApiUrl + url;
    }

    public static boolean isEmpty(String object) {
        if (object != null && !object.isEmpty() && !object.equals("null") && !object.equals("")) {
            return false;
        }
        return true;
    }

    public static String setNullToEmpty(String object) {
        if (object == null || object.equals("null")) {
            return "";
        }
        return object;
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
        Bitmap bitmapSave = null;
        try {
            ExifInterface ei = new ExifInterface(imagePath);
            int orientation = ei.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_UNDEFINED);
            switch (orientation) {

                case ExifInterface.ORIENTATION_ROTATE_90:
                    bitmapSave = RotateImage(bitmap, 90);
                    break;

                case ExifInterface.ORIENTATION_ROTATE_180:
                    bitmapSave = RotateImage(bitmap, 180);
                    break;

                case ExifInterface.ORIENTATION_ROTATE_270:
                    bitmapSave = RotateImage(bitmap, 270);
                    break;
            }
        } catch (Exception e) {
            Log.e("CAMERA ROTATE", e.getMessage());
        }

        if (bitmapSave != null) {
            SaveBitmapToImage(bitmapSave, imagePath);
        } else {
            SaveBitmapToImage(bitmap, imagePath);
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
        try {
            Matrix matrix = new Matrix();
            matrix.postRotate(angle);
            return Bitmap.createBitmap(source, 0, 0, source.getWidth(), source.getHeight(),
                    matrix, true);
        } catch (Exception ex) {
        }
        return null;
    }

    public static File ResizeImages12(Context context, String sPath) throws IOException {
        Bitmap photo = BitmapFactory.decodeFile(sPath);
        float factor = 720 / (float) photo.getWidth();

        photo = Bitmap.createScaledBitmap(photo, 720, (int) (photo.getHeight() * factor), false);
        ByteArrayOutputStream bytes = new ByteArrayOutputStream();
        photo.compress(Bitmap.CompressFormat.JPEG, Constants.IMAGE_COMPRESS_QUALITY, bytes);

        String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").format(new Date());
        String imageFileName = "JPEG_" + timeStamp + "_";
        File storageDir = context.getExternalFilesDir(Environment.DIRECTORY_PICTURES);

        File image = File.createTempFile(
                imageFileName,  /* prefix */
                ".jpeg",         /* suffix */
                storageDir      /* directory */
        );

        image.createNewFile();
        FileOutputStream fo = new FileOutputStream(image);
        fo.write(bytes.toByteArray());
        fo.close();
        File file = new File(sPath);
        file.delete();

        return image;
    }

    public static void SetPicture(ImageView imgView, String imagePath) {
        // Get the dimensions of the bitmap
        BitmapFactory.Options bmOptions = new BitmapFactory.Options();
        bmOptions.inJustDecodeBounds = true;
        BitmapFactory.decodeFile(imagePath, bmOptions);

        // Decode the image file into a Bitmap sized to fill the View
        bmOptions.inJustDecodeBounds = false;
        //bmOptions.inSampleSize = scaleFactor;
        bmOptions.inPurgeable = true;

        try {
            Bitmap bitmap = BitmapFactory.decodeFile(imagePath, bmOptions);
            imgView.setImageBitmap(bitmap);
        } catch (Exception e) {
            Log.e("CAMERA ROTATE", e.getMessage());
        }
    }

    /**
     * Tự động xoay ảnh
     *
     * @param imagePath
     */
    public static void AutoRotateImage(String imagePath) {

        // Get the dimensions of the bitmap
        BitmapFactory.Options bmOptions = new BitmapFactory.Options();
        bmOptions.inJustDecodeBounds = true;
        BitmapFactory.decodeFile(imagePath, bmOptions);

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
                    bitmap = rotateImage(bitmap, 90);
                    SaveBitmapToImage(bitmap, imagePath);
                    break;

                case ExifInterface.ORIENTATION_ROTATE_180:
                    bitmap = rotateImage(bitmap, 180);
                    SaveBitmapToImage(bitmap, imagePath);
                    break;

                case ExifInterface.ORIENTATION_ROTATE_270:
                    bitmap = rotateImage(bitmap, 270);
                    SaveBitmapToImage(bitmap, imagePath);
                    break;

                case ExifInterface.ORIENTATION_NORMAL:
                    break;
                default:
                    break;
            }
        } catch (Exception e) {
            Log.e("CAMERA ROTATE", e.getMessage());
        }
    }

    public static void SaveBitmapToImage(Bitmap bitmap, String imagePath) {
        try {
            File file = new File(imagePath);
            FileOutputStream fOut = new FileOutputStream(file);
            bitmap.compress(Bitmap.CompressFormat.JPEG, Constants.IMAGE_COMPRESS_QUALITY, fOut);
        } catch (Exception ex) {
        }
    }

    private static Bitmap rotateImage(Bitmap img, int degree) {
        Matrix matrix = new Matrix();
        matrix.postRotate(degree);
        Bitmap rotatedImg = Bitmap.createBitmap(img, 0, 0, img.getWidth(), img.getHeight(), matrix, true);
        img.recycle();
        return rotatedImg;
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
     * @param button
     * @return
     */
    public static String getText(EditText button) {
        return button.getText() != null ? button.getText().toString() : "";
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
                if (item.Id.equals(id)) {
                    return item.Name;
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

    /***
     * Thay đổi fragment
     * @param activity
     * @param fragment
     * @param bundle
     */
    public static void ChangeFragment(Activity activity, android.support.v4.app.Fragment fragment, Bundle bundle) {
        if (fragment != null) {
            if (bundle != null) {
                fragment.setArguments(bundle);
            }
            FragmentManager fragmentManager = ((FragmentActivity) activity).getSupportFragmentManager();
            FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
            fragmentTransaction.replace(R.id.frame_content, fragment, "xxx");
            fragmentTransaction.addToBackStack("xxx");
            fragmentTransaction.commit();
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
     * get dữ liệu fix sử dụng trong phần mềm
     */
    static DataBaseHelper dataBaseHelper;

    private static void genDatabaseSQLite(Context context) {
        dataBaseHelper = new DataBaseHelper(context, Constants.DATABASE_NAME, null, 1);
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Ethnic(Id VARCHAR(36) PRIMARY KEY NOT NULL, Name NVARCHAR(150) NOT NULL, OrderNumber INT)");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Job(Id VARCHAR(36) PRIMARY KEY NOT NULL, NameEn NVARCHAR(100), Name NVARCHAR(100), OrderNumber INT)");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Relationship(Id VARCHAR(36) PRIMARY KEY NOT NULL, Name NVARCHAR(100) NOT NULL, Gender INT, OrderNumber INT)");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Religion(Id VARCHAR(36) PRIMARY KEY NOT NULL, Name NVARCHAR(150) NOT NULL, OrderNumber INT)");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS School(Id VARCHAR(36) PRIMARY KEY NOT NULL, WardId VARCHAR(36), SchoolName NVARCHAR(150))");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS Village(Id VARCHAR(36) PRIMARY KEY NOT NULL, WardId VARCHAR(36) NOT NULL, NameEN NVARCHAR(150), Name NVARCHAR(150) NOT NULL, Type NVARCHAR(50))");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS ChildProfile(Id VARCHAR(36) PRIMARY KEY NOT NULL, " +
                "InfoDate DATETIME, " +
                "EmployeeName NVARCHAR(150), " +
                "ProgramCode NVARCHAR(100), " +
                "ProvinceId VARCHAR(36), " +
                "DistrictId VARCHAR(36), " +
                "WardId VARCHAR(36), " +
                "Address NVARCHAR(250), " +
                "FullAddress NVARCHAR(500), " +
                "ChildCode  NVARCHAR(100), " +
                "SchoolId NVARCHAR(100), " +
                "SchoolOtherName NVARCHAR(500), " +
                "EthnicId NVARCHAR(36), " +
                "ReligionId VARCHAR(36), " +
                "Name NVARCHAR(150), " +
                "NickName NVARCHAR(150), " +
                "Gender INT, " +
                "DateOfBirth DATETIME, " +
                "LeaningStatus VARCHAR(2), " +
                "ClassInfo NVARCHAR(100), " +
                "FavouriteSubject NVARCHAR, " +
                "LearningCapacity NVARCHAR, " +
                "Housework NVARCHAR, " +
                "Health NVARCHAR, " +
                "Personality NVARCHAR, " +
                "Hobby NVARCHAR, " +
                "Dream NVARCHAR, " +
                "FamilyMember NVARCHAR, " +
                "LivingWithParent NVARCHAR, " +
                "NotLivingWithParent NVARCHAR, " +
                "LivingWithOther NVARCHAR, " +
                "LetterWrite NVARCHAR, " +
                "HouseType NVARCHAR, " +
                "HouseRoof NVARCHAR, " +
                "HouseWall NVARCHAR, " +
                "HouseFloor NVARCHAR, " +
                "UseElectricity NVARCHAR, " +
                "SchoolDistance NVARCHAR, " +
                "ClinicDistance NVARCHAR, " +
                "WaterSourceDistance NVARCHAR, " +
                "WaterSourceUse NVARCHAR, " +
                "RoadCondition NVARCHAR, " +
                "IncomeFamily NVARCHAR, " +
                "HarvestOutput NVARCHAR, " +
                "NumberPet NVARCHAR, " +
                "FamilyType NVARCHAR, " +
                "TotalIncome NVARCHAR, " +
                "IncomeSources NVARCHAR, " +
                "IncomeOther NVARCHAR, " +
                "StoryContent NVARCHAR, " +
                "ImagePath NVARCHAR(256), " +
                "ImageThumbnailPath NVARCHAR(256), " +
                "AreaApproverId VARCHAR(36), " +
                "AreaApproverDate DATETIME, " +
                "OfficeApproveBy VARCHAR(36), " +
                "OfficeApproveDate DATETIME, " +
                "ProcessStatus VARCHAR, " +
                "IsDelete BOOLEAN, " +
                "CreateBy VARCHAR(36), " +
                "CreateDate DATETIME, " +
                "UpdateBy VARCHAR(36), " +
                "UpdateDate DATETIME, " +
                "ConsentName NVARCHAR(150), " +
                "ConsentRelationship NVARCHAR(150), " +
                "ConsentVillage NVARCHAR(150), " +
                "ConsentWard NVARCHAR(150), " +
                "SiblingsJoiningChildFund NVARCHAR, " +
                "Malformation NVARCHAR, " +
                "Orphan NVARCHAR, " +
                "EmployeeTitle INT, " +
                "ImageSignaturePath  NVARCHAR(256), " +
                "ImageSignatureThumbnailPath  NVARCHAR(256), " +
                "SaleforceId  NVARCHAR(50), " +
                "Handicap  BOOLEAN, " +
                "ImageSize  INT, " +
                "SchoolName VARCHAR, " +
                "TypeChildProfile VARCHAR)"
        );
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS ReportProfile(Content NVARCHAR ( 500 ), ChildProfileId VARCHAR ( 36 ) NOT NULL, ProcessStatus VARCHAR ( 1 ) NOT NULL, IsDelete INT NOT NULL, CreateBy VARCHAR ( 36 ), CreateDate DATETIME, UpdateBy VARCHAR ( 36 ), UpdateDate DATETIME, Description NVARCHAR,Url NVARCHAR );");
        dataBaseHelper.queryData("CREATE TABLE IF NOT EXISTS ImageChildByYear(ChildProfileId VARCHAR ( 50 ) , ImageUrl NVARCHAR(550));");

        SyncCombobox syncCombobox = new SyncCombobox();
    }

    public static void getBackgroundData(final Context context, final LoginProfileModel loginProfileModel, final boolean isAgain) {
        genDatabaseSQLite(context);
        final SharedPreferences sharedPreferencesDataFix = context.getSharedPreferences(Constants.Childprofile_Data_Fix, Context.MODE_PRIVATE);

        new Thread(new Runnable() {
            @Override
            public void run() {
                String dataFixRelationship = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Relationship, null);
                if (!isAgain || (dataFixRelationship == null || dataFixRelationship == "" || dataFixRelationship == "[]")) {

                    ANRequest requestGetRelationship = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetRelationship"))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });
                    ANResponse<JSONArray> responseRelationship = requestGetRelationship.executeForJSONArray();
                    if (responseRelationship.isSuccess()) {
                        JSONArray jsonRelationship = responseRelationship.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_Relationship, jsonRelationship.toString());
                        editor.apply();
                        List<ComboboxResult> listRelationShip = new ArrayList<>();
                        listRelationShip = new Gson().fromJson(jsonRelationship.toString(), new TypeToken<List<ComboboxResult>>() {
                        }.getType());
                        if (listRelationShip.size() > 0) {
                            for (int i = 0; i < listRelationShip.size(); i++) {
                                ContentValues values = new ContentValues();
                                values.put("Id", listRelationShip.get(i).Id);
                                values.put("Name", listRelationShip.get(i).Name);
                                values.put("Gender", listRelationShip.get(i).PId);
//                                dataBaseHelper.insert(Constants.DATABASE_TABLE_Relationship, values);
                            }
                        }
                    }
                }

                String dataFixGeligion = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Geligion, null);
                if (!isAgain || (dataFixGeligion == null || dataFixGeligion == "" || dataFixGeligion == "[]")) {
                    ANRequest requestGetGeligion = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetGeligion"))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });

                    ANResponse<JSONArray> responseGeligion = requestGetGeligion.executeForJSONArray();
                    if (responseGeligion.isSuccess()) {
                        JSONArray jsonGeligion = responseGeligion.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_Geligion, jsonGeligion.toString());
                        editor.apply();
                        List<ComboboxResult> listReligion = new ArrayList<>();
                        listReligion = new Gson().fromJson(jsonGeligion.toString(), new TypeToken<List<ComboboxResult>>() {
                        }.getType());
                        if (listReligion.size() > 0) {
                            for (int i = 0; i < listReligion.size(); i++) {
                                ContentValues values = new ContentValues();
                                values.put("Id", listReligion.get(i).Id);
                                values.put("Name", listReligion.get(i).Name);
//                                dataBaseHelper.insert(Constants.DATABASE_TABLE_Religion, values);
                            }
                        }
                    }
                }

                String dataFixNation = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Nation, null);
                if (!isAgain || (dataFixNation == null || dataFixNation == "" || dataFixNation == "[]")) {
                    ANRequest requestGetNation = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetNation"))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });

                    ANResponse<JSONArray> responseNation = requestGetNation.executeForJSONArray();
                    if (responseNation.isSuccess()) {
                        JSONArray jsonNation = responseNation.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_Nation, jsonNation.toString());
                        editor.apply();
                        List<ComboboxResult> listNation = new ArrayList<>();
                        listNation = new Gson().fromJson(jsonNation.toString(), new TypeToken<List<ComboboxResult>>() {
                        }.getType());
                        if (listNation.size() > 0) {
                            for (int i = 0; i < listNation.size(); i++) {
                                ContentValues values = new ContentValues();
                                values.put("Id", listNation.get(i).Id);
                                values.put("Name", listNation.get(i).Name);
//                                dataBaseHelper.insert(Constants.DATABASE_TABLE_Ethnic, values);
                            }
                        }
                    }
                }

                String dataFixProvince_Area = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Province_Area, null);
                if (!isAgain || (dataFixProvince_Area == null || dataFixProvince_Area == "" || dataFixProvince_Area == "[]")) {
                    ANRequest requestGetProvince = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetProvinceByArea?areaUserId=" + loginProfileModel.AreaUserId))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });

                    ANResponse<JSONArray> responseProvince = requestGetProvince.executeForJSONArray();
                    if (responseProvince.isSuccess()) {
                        JSONArray jsonProvince = responseProvince.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_Province_Area, jsonProvince.toString());
                        editor.apply();
                    }
                }

                String dataFixDistrict_Area = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_District_Area, null);
                if (!isAgain || (dataFixDistrict_Area == null || dataFixDistrict_Area == "" || dataFixDistrict_Area == "[]")) {
                    ANRequest requestGetDistrict = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetDistrictByArea?areaDistrictId=" + loginProfileModel.DistrictId))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });

                    ANResponse<JSONArray> responseDistrict = requestGetDistrict.executeForJSONArray();
                    if (responseDistrict.isSuccess()) {
                        JSONArray jsonDistrict = responseDistrict.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_District_Area, jsonDistrict.toString());
                        editor.apply();
                    }
                }

                String dataFixWard_Area = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Ward_Area, null);
                if (!isAgain || (dataFixWard_Area == null || dataFixWard_Area == "" || dataFixWard_Area == "[]")) {
                    ANRequest requestGetWard = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetWardByArea?areaWardId=" + loginProfileModel.WardId))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });

                    ANResponse<JSONArray> responseWard = requestGetWard.executeForJSONArray();
                    if (responseWard.isSuccess()) {
                        JSONArray jsonWard = responseWard.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_Ward_Area, jsonWard.toString());
                        editor.apply();
                    }
                }

                String dataFixJob = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Job, null);
                if (!isAgain || (dataFixJob == null || dataFixJob == "" || dataFixJob == "[]")) {
                    ANRequest requestGetJob = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetJob"))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });

                    ANResponse<JSONArray> responseJob = requestGetJob.executeForJSONArray();
                    if (responseJob.isSuccess()) {
                        JSONArray jsonJob = responseJob.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_Job, jsonJob.toString());
                        editor.apply();
                        List<ComboboxResult> listJob = new ArrayList<>();
                        listJob = new Gson().fromJson(jsonJob.toString(), new TypeToken<List<ComboboxResult>>() {
                        }.getType());
                        if (listJob.size() > 0) {
                            for (int i = 0; i < listJob.size(); i++) {
                                ContentValues values = new ContentValues();
                                values.put("Id", listJob.get(i).Id);
                                values.put("Name", listJob.get(i).Name);
                                values.put("NameEn", listJob.get(i).PId);
//                                dataBaseHelper.insert(Constants.DATABASE_TABLE_Job, values);
                            }
                        }
                    }
                }

                String dataReportContent = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Repoet_Content, null);
                if (!isAgain || (dataReportContent == null || dataReportContent == "" || dataReportContent == "[]")) {
                    ANRequest requestGetReportContent = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetReportContent"))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });

                    ANResponse<JSONArray> responseReportContent = requestGetReportContent.executeForJSONArray();
                    if (responseReportContent.isSuccess()) {
                        JSONArray jsonJob = responseReportContent.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_Repoet_Content, jsonJob.toString());
                        editor.apply();
                    }
                }

                String dataSchool = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_School, null);
                if (!isAgain || (dataSchool == null || dataSchool == "" || dataSchool == "[]")) {
                    ANRequest requestGetSchool = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetSchool"))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });

                    ANResponse<JSONArray> responseSchool = requestGetSchool.executeForJSONArray();
                    if (responseSchool.isSuccess()) {
                        JSONArray jsonSchool = responseSchool.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_School, jsonSchool.toString());
                        editor.apply();
                        List<ComboboxResult> listSchool = new ArrayList<>();
                        listSchool = new Gson().fromJson(jsonSchool.toString(), new TypeToken<List<ComboboxResult>>() {
                        }.getType());
                        if (listSchool.size() > 0) {
                            for (int i = 0; i < listSchool.size(); i++) {
                                ContentValues values = new ContentValues();
                                values.put("Id", listSchool.get(i).Id);
                                values.put("WardId", listSchool.get(i).PId);
                                values.put("SchoolName", listSchool.get(i).Name);
//                                dataBaseHelper.insert(Constants.DATABASE_TABLE_School, values);
                            }
                        }
                    }
                }

                String dataVillage = sharedPreferencesDataFix.getString(Constants.Key_Data_Fix_Village + loginProfileModel.WardId, null);
                if (!isAgain || (dataVillage == null || dataVillage == "" || dataVillage == "[]")) {
                    ANRequest requestGetVillage = AndroidNetworking.get(Utils.GetUrlApi("api/Combobox/GetVillageByWrad?id=" + loginProfileModel.WardId))
                            .setPriority(Priority.MEDIUM)
                            .build().setAnalyticsListener(new AnalyticsListener() {
                                @Override
                                public void onReceived(long timeTakenInMillis, long bytesSent, long bytesReceived, boolean isFromCache) {

                                }
                            });
                    ANResponse<JSONArray> responseVillage = requestGetVillage.executeForJSONArray();
                    if (responseVillage.isSuccess()) {
                        JSONArray jsonVillage = responseVillage.getResult();
                        SharedPreferences.Editor editor = sharedPreferencesDataFix.edit();
                        editor.putString(Constants.Key_Data_Fix_Village + loginProfileModel.WardId, jsonVillage.toString());
                        editor.apply();
                        List<ComboboxResult> listVillage = new ArrayList<>();
                        listVillage = new Gson().fromJson(jsonVillage.toString(), new TypeToken<List<ComboboxResult>>() {
                        }.getType());
                        if (listVillage.size() > 0) {
                            for (int i = 0; i < listVillage.size(); i++) {
                                ContentValues values = new ContentValues();
                                values.put("Id", listVillage.get(i).Id);
                                values.put("WardId", listVillage.get(i).PId);
                                values.put("Name", listVillage.get(i).Name);
                                values.put("NameEn", listVillage.get(i).Name);
//                                dataBaseHelper.insert(Constants.DATABASE_TABLE_Village, values);
                            }
                        }
                    }

                }
            }
        }).start();
    }
}
