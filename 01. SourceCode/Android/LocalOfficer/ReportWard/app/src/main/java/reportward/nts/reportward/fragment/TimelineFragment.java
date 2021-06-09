package reportward.nts.reportward.fragment;

import android.app.Dialog;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.androidnetworking.interfaces.StringRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.AdapterTimeline;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.ChildProcessingContentModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ProcessingContentModel;

public class TimelineFragment extends Fragment {
    private View view;
    private AdapterTimeline adapterTimeline;
    public String Id;
    List<ProcessingContentModel> listReportContent;
    private TextView txtName;
    private TextView txtAddress;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        view = inflater.inflate(R.layout.fragment_cadangxuly, container, false);
        txtName = view.findViewById(R.id.tv_Name);
        txtAddress = view.findViewById(R.id.tv_Address);
        RecyclerView recyclerView = view.findViewById(R.id.list_Timeline);
        LinearLayout lyReport = view.findViewById(R.id.lyNext);
        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }

        lyReport.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DialogReport(Id);
            }
        });

        Bundle bundle = this.getArguments();
        if (bundle != null) {
            txtName.setText(bundle.getString("Name", ""));
            txtAddress.setText(bundle.getString("Address", ""));
        }

        SearchProcessingContent();
        listReportContent = new ArrayList<ProcessingContentModel>();
       // adapterTimeline = new AdapterTimeline(getContext(), listReportContent);


        adapterTimeline.SetOnItemClickListener(new AdapterTimeline.OnItemClickListener() {
            @Override
            public void onButtonDetailClick(View view, int position, ProcessingContentModel obj) {
                DialogDetailReport(obj.Id);
            }

            @Override
            public void onButtonMoreClick(View view, int position, ProcessingContentModel obj) {
                DialogDetailReport(obj.Id);
            }
        });

        recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
        recyclerView.setAdapter(adapterTimeline);
        // Inflate the layout for this fragment
        return view;
    }

    public void DialogDetailReport(String Id) {
        final Dialog dialog = new Dialog(getContext());
        dialog.setContentView(R.layout.popup_detail_report);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        TextView btnClose = dialog.findViewById(R.id.btnClose);
        final TextView tvContent = dialog.findViewById(R.id.tv_Content);
        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialog.dismiss();
            }
        });


        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,"api/ProfileReport/DetailProcessingContent?id=" + Id))
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {

                    @Override
                    public void onResponse(JSONObject response) {
                        ProcessingContentModel resultObject = new Gson().fromJson(response.toString(), new TypeToken<ProcessingContentModel>() {
                        }.getType());
                        ProcessingContentModel processingContentModel = resultObject;
                        tvContent.setText(processingContentModel.Content);

                    }

                    @Override
                    public void onError(ANError anError) {

                    }
                });
        dialog.show();
    }

    public void DialogReport(final String id) {
        final Dialog dialog = new Dialog(getContext());
        dialog.setContentView(R.layout.popup_report);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        Button btnAdd = dialog.findViewById(R.id.btnAdd);
        final TextView txtContent = dialog.findViewById(R.id.txtContent);
        final TextInputLayout tilContent = dialog.findViewById(R.id.tilContent);
        btnAdd.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                ProcessingContentModel processingContentModel = new ProcessingContentModel();
                processingContentModel.ProfileChildId = id;
                processingContentModel.ProcessingBy = loginProfileModel.id;
                processingContentModel.Content = txtContent.getText().toString();

                //Check validate
                if (processingContentModel.Content.isEmpty()) {
                    tilContent.setError("Không được để trống.");
                    txtContent.findFocus();
                    return;
                } else {
                    tilContent.setErrorEnabled(false);
                }

                if (id == null || id.isEmpty()) {
                    Toast.makeText(getActivity(), "Hãy chọn trẻ trước khi báo cáo!", Toast.LENGTH_SHORT);
                    return;
                }

                JSONObject jsonModel = new JSONObject();
                try {
                    jsonModel = new JSONObject(new Gson().toJson(processingContentModel));
                } catch (JSONException e) {
                }

                AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,"api/ProfileReport/AddProcessingContent"))
                        .addJSONObjectBody(jsonModel)
                        .setPriority(Priority.MEDIUM)
                        .build()
                        .getAsString(new StringRequestListener() {
                            @Override
                            public void onResponse(String response) {
                                //progressDialog.dismiss();
                                dialog.dismiss();
                                listReportContent.clear();
                                SearchProcessingContent();
                                Toast.makeText(getActivity(), "Báo cáo thành công.", Toast.LENGTH_SHORT).show();
                            }

                            @Override
                            public void onError(ANError anError) {
                                //progressDialog.dismiss();
                                Utils.showErrorMessage(getActivity().getApplication(), anError);
                            }
                        });
            }
        });

        Button btnClose = dialog.findViewById(R.id.btnClose);
        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialog.dismiss();
            }
        });
        dialog.show();
    }

    public void SearchProcessingContent() {
//        JSONObject jsonModel = new JSONObject();
//        try {
//            jsonModel = new JSONObject(new Gson().toJson(Id));
//        } catch (JSONException e) {
//            // MessageUtils.Show(getActivity(), e.getMessage());
//        }
        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,"api/ProfileReport/GetProcessingContentByProfile?id=" + Id))
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ChildProcessingContentModel resultObject = new Gson().fromJson(response.toString(), new TypeToken<ChildProcessingContentModel>() {
                        }.getType());

                        listReportContent.addAll(resultObject.ListProcessingContent);
                        txtName.setText(resultObject.ChildName);
                        txtAddress.setText(resultObject.FullAddress);
                        adapterTimeline.notifyDataSetChanged();
                    }

                    @Override
                    public void onError(ANError anError) {

                    }
                });
    }
}
