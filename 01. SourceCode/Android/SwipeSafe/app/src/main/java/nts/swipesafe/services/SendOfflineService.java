package nts.swipesafe.services;

import android.app.IntentService;
import android.content.Intent;
import android.content.SharedPreferences;
import android.support.annotation.Nullable;
import android.util.Log;
import android.view.View;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.ANRequest;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.JsonSyntaxException;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.LinkApi;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.GalleryModel;
import nts.swipesafe.model.ReportMobileModel;
import nts.swipesafe.model.ReportOfflineModel;
import nts.swipesafe.model.ResultModel;
import nts.swipesafe.model.UploadFileResultModel;

public class SendOfflineService extends IntentService {
    private SharedPreferences sharedPreferences;
    private boolean isProcess = false;
    private List<ReportOfflineModel> listReportOffline = null;
    private boolean isConnectedNetwork = false;
    private boolean isLoop = false;

    public SendOfflineService() {
        super("Send_Offline_Service_intent_thread");
    }

    @Override
    protected void onHandleIntent(@Nullable Intent intent) {
        isLoop = true;
        while (isLoop) {
            try {
                String reportOffline = sharedPreferences.getString(Constants.KeyReportOffline, "");

                isConnectedNetwork = Utils.checkConnectedNetwork(getApplicationContext());

                if (!Utils.isEmpty(reportOffline)) {
                    listReportOffline = new Gson().fromJson(reportOffline, new TypeToken<List<ReportOfflineModel>>() {
                    }.getType());
                }

                if (!isProcess && listReportOffline != null && listReportOffline.size() > 0 && isConnectedNetwork) {
                    isProcess = true;
                    try {
                        Log.i("ReLogin_" + DateUtils.CurrentDate(DateUtils.DATE_FORMAT_DD_MM_YYYY_HH_MM), "_Gửi báo cáo offline");
                        final ReportOfflineModel reportOfflineModel = listReportOffline.get(0);
                        if (reportOfflineModel.ListFile != null && reportOfflineModel.ListFile.size() > 0) {
                            reportOfflineModel.ReportData.ListFiles = new ArrayList<>();
                            ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Utils.getUrlAPIByArea(reportOfflineModel.Area, LinkApi.upload));
                            int index = 0;
                            for (GalleryModel galleryModel : reportOfflineModel.ListFile) {
                                File file = new File(galleryModel.FileUrl);
                                anRequest.addMultipartFile("file" + index, file);
                                index++;
                            }
                            anRequest.setPriority(Priority.MEDIUM)
                                    .build()
                                    .getAsJSONObject(new JSONObjectRequestListener() {
                                        @Override
                                        public void onResponse(JSONObject response) {
                                            try {
                                                ResultModel<List<UploadFileResultModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<UploadFileResultModel>>>() {
                                                }.getType());

                                                for (UploadFileResultModel modelFile : resultObject.data) {
                                                    reportOfflineModel.ReportData.ListFiles.add(modelFile.mappingName);
                                                }
                                            } catch (Exception ex) {
                                            }
                                            sendReportContent(reportOfflineModel.ReportData, reportOfflineModel.Area);
                                        }

                                        @Override
                                        public void onError(ANError anError) {
                                            sendReportContent(reportOfflineModel.ReportData, reportOfflineModel.Area);
                                        }
                                    });
                        } else {
                            sendReportContent(reportOfflineModel.ReportData, reportOfflineModel.Area);
                        }
                    } catch (Exception ex) {
                        isProcess = false;
                    }
                } else {
                    //Trường hợp không có báo cáo offine thì dừng vòng lặp
                    if (listReportOffline == null || listReportOffline.size() == 0) {
                        isLoop = false;
                    }
                }
                Thread.sleep(120000);
            } catch (JsonSyntaxException ex) {
                String temp = ex.getMessage();
            } catch (InterruptedException ex) {
                String temp = ex.getMessage();
            }
        }
    }

    @Override
    public int onStartCommand(Intent intent, int flag, int startId) {
        sharedPreferences = getApplicationContext().getSharedPreferences(Constants.DataReportOffline, MODE_PRIVATE);
        return super.onStartCommand(intent, flag, startId);
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
    }

    /***
     *Đây nộji dung báo caó
     * @param reportMobileModel
     */
    private void sendReportContent(ReportMobileModel reportMobileModel, String area) {
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(reportMobileModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.getUrlAPIByArea(area, LinkApi.taomoicatuvan))
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        if (listReportOffline != null && listReportOffline.size() > 0) {
                            listReportOffline.remove(0);
                            String json = new Gson().toJson(listReportOffline);
                            SharedPreferences.Editor editor = sharedPreferences.edit();
                            editor.putString(Constants.KeyReportOffline, json);
                            editor.apply();
                        }
                        isProcess = false;
                    }

                    @Override
                    public void onError(ANError anError) {
                        if (listReportOffline != null && listReportOffline.size() > 0) {
                            listReportOffline.get(0).CountError += 1;
                            //Lỗi quá 10 lần thì xóa bỏ bản ghi
                            if (listReportOffline.get(0).CountError > 10) {
                                listReportOffline.remove(0);
                            }
                            String json = new Gson().toJson(listReportOffline);
                            SharedPreferences.Editor editor = sharedPreferences.edit();
                            editor.putString(Constants.KeyReportOffline, json);
                            editor.apply();
                        }
                        isProcess = false;
                    }
                });
    }
}
