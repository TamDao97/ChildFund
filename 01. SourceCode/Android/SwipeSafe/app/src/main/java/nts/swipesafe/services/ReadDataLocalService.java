package nts.swipesafe.services;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.support.annotation.Nullable;
import android.util.Log;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.ANRequest;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.JsonSyntaxException;
import com.google.gson.reflect.TypeToken;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.GlobalVariable;
import nts.swipesafe.common.LinkApi;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ComboboxItem;
import nts.swipesafe.model.GalleryModel;
import nts.swipesafe.model.InternetModel;
import nts.swipesafe.model.ReportMobileModel;
import nts.swipesafe.model.ReportOfflineModel;
import nts.swipesafe.model.ResultModel;
import nts.swipesafe.model.UploadFileResultModel;
import nts.swipesafe.model.VideoModel;

public class ReadDataLocalService extends IntentService {
    private Context context;
    private GlobalVariable global;
    private List<ReportOfflineModel> listReportOffline = null;

    public ReadDataLocalService() {
        super("Send_Offline_Service_intent_thread");
    }

    @Override
    protected void onHandleIntent(@Nullable Intent intent) {
        try {
            String tinhJson = Utils.ReadJSONFromAsset(context, "tinh_tp.json");
            global.ListProvince = new Gson().fromJson(tinhJson, new TypeToken<ArrayList<ComboboxItem>>() {
            }.getType());
        } catch (Exception ex) {
        }

        try {
            String qhJson = Utils.ReadJSONFromAsset(context, "moi_quan_he.json");
            global.ListRelationship = new Gson().fromJson(qhJson, new TypeToken<ArrayList<ComboboxItem>>() {
            }.getType());
        } catch (Exception ex) {
        }

        try {
            String webJson = Utils.ReadJSONFromAsset(context, "internet.json");
            global.ListInternetImage = new Gson().fromJson(webJson, new TypeToken<ArrayList<InternetModel>>() {
            }.getType());
        } catch (Exception ex) {
        }

        try {
            AndroidNetworking.get("https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&playlistId=" + Constants.KeyPlaylistIdATM + "&key=" + Constants.KeyYoutube)
                    .setPriority(Priority.LOW)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                JSONArray listVideo = response.getJSONArray("items");
                                InternetModel internetModel;
                                for (int i = 0; i < listVideo.length(); i++) {
                                    JSONObject itemVideo = listVideo.getJSONObject(i).getJSONObject("snippet");
                                    internetModel = new InternetModel();
                                    internetModel.Title = itemVideo.getString("title");
                                    internetModel.ImageThumbnail = itemVideo.getJSONObject("thumbnails").getJSONObject("default").getString("url");
                                    internetModel.DateTime = itemVideo.getString("publishedAt");
                                    internetModel.VideoId = itemVideo.getJSONObject("resourceId").getString("videoId");
                                    internetModel.IsVideo = true;
                                    global.ListInternetVideo.add(internetModel);
                                }
                            } catch (Exception ex) {

                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            String eror = "";
                        }
                    });
        } catch (Exception ex) {

        }

        try{
            AndroidNetworking.get("https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&playlistId="+Constants.KeyPlaylistId111+"&key=" + Constants.KeyYoutube)
                    .setPriority(Priority.LOW)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                JSONArray listVideo = response.getJSONArray("items");
                                VideoModel videoModel;
                                for (int i = 0; i < listVideo.length(); i++) {
                                    JSONObject itemVideo = listVideo.getJSONObject(i).getJSONObject("snippet");
                                    videoModel = new VideoModel();
                                    videoModel.Title = itemVideo.getString("title");
                                    videoModel.ImageThumbnail = itemVideo.getJSONObject("thumbnails").getJSONObject("default").getString("url");
                                    videoModel.DateTime = itemVideo.getString("publishedAt");
                                    videoModel.VideoId = itemVideo.getJSONObject("resourceId").getString("videoId");
                                    global.ListGenderVideo.add(videoModel);
                                }
                            } catch (Exception ex) {

                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            String eror = "";
                        }
                    });
        }catch (Exception ex){

        }
    }

    @Override
    public int onStartCommand(Intent intent, int flag, int startId) {
        context = getApplicationContext();
        global = (GlobalVariable) getApplication();
        return super.onStartCommand(intent, flag, startId);
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
    }
}
