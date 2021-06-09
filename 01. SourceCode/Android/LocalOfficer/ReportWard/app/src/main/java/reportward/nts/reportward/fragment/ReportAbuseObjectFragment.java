package reportward.nts.reportward.fragment;

import android.app.Dialog;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.PrisonerListAdapter;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.ComboboxResult;
import reportward.nts.reportward.model.DoiTuongXamHaiModel;
import reportward.nts.reportward.model.DoiTuongXamHaiViewModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseObjectFragment extends Fragment {
    private boolean isLoad = true;
    private View view;
    private EditText txtHoTen;
    private RadioGroup rgGioiTinh;
    private RecyclerView rvPrisoner;
    private PrisonerListAdapter prisonerListAdapter;
    private EditText spnNamSinh, spnQuanHeVoiTre, spnLoaiDoiTuongXamHai;
    private String[] arrayQuanHeVoiTre, arrayLoaiDoiTuongXamHai;
    private List<ComboboxResult> listQuanHeVoiTre, listLoaiDoiTuongXamHai;
    private static String maSoHoSo;
    private Dialog dialogAddAbuseObject, dialogDetailChildAbuseObject;
    private LinearLayout lyAddPrisoner, lySave, lyClose;
    private DoiTuongXamHaiModel doiTuongXamHaiModel;
    private TextView tvNotRecord, btnCloseDetail, tvHoTen, tvGioiTinh, tvNamSinh, tvQuanHeVoiTre, tvLoaiDoiTuongXamHai;
    private List<DoiTuongXamHaiViewModel> listDoiTuong;
    private String idDoiTuongEdit = "";
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    public ReportAbuseObjectFragment() {

    }

    public static ReportAbuseObjectFragment newInstance(String maHoSo) {
        maSoHoSo = maHoSo;
        ReportAbuseObjectFragment fragment = new ReportAbuseObjectFragment();
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        if (isLoad) {
            view = inflater.inflate(R.layout.fragment_report_abuse_object, container, false);

            //Get thông tin login
            prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
            String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
            if (loginProfile != null) {
                loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
                }.getType());
            }

            initComponent();

            initDialogAddAbuseObject();

            initDialogDetailAbuseObject();

            getDataCombobox(loginProfileModel.accessToken);
        }
        isLoad = false;

        // Inflate the layout for this fragment
        return view;
    }

    /***
     * Khỏi tạo thành phần trên giao diện
     */
    private void initComponent() {
        rvPrisoner = view.findViewById(R.id.rvPrisoner);
        rvPrisoner.setLayoutManager(new GridLayoutManager(getActivity(), 1));

        tvNotRecord = view.findViewById(R.id.tvNotRecord);

        if (listDoiTuong != null && listDoiTuong.size() > 0) {
            tvNotRecord.setVisibility(View.GONE);
            prisonerListAdapter = new PrisonerListAdapter(getContext(), listDoiTuong);
            rvPrisoner.setAdapter(prisonerListAdapter);
            prisonerListAdapter.SetOnItemClickListener(new PrisonerListAdapter.OnItemClickListener() {
                @Override
                public void onEditClick(View view, int position, DoiTuongXamHaiViewModel obj) {
                    idDoiTuongEdit = obj.id;
                    viewDataEdit(obj);
                    dialogAddAbuseObject.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.WRAP_CONTENT);
                    dialogAddAbuseObject.show();
                }

                @Override
                public void onDetailClick(View view, int position, DoiTuongXamHaiViewModel obj) {
                    thongDoiTuong(obj);
                }
            });
        } else {
            tvNotRecord.setVisibility(View.VISIBLE);
        }

        lyAddPrisoner = view.findViewById(R.id.lyAddPrisoner);
        lyAddPrisoner.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogAddAbuseObject.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.WRAP_CONTENT);
                dialogAddAbuseObject.show();
            }
        });
    }

    /***
     * Show Dialog chọn Tỉnh/Thành
     * @param editText
     * @param arrayName
     * @param listSource
     * @param title
     * @param funtion
     */
    private void showChooseDialog(final EditText editText, final String[] arrayName, final List<ComboboxResult> listSource, final String title, final Runnable funtion) {
        final android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setSingleChoiceItems(arrayName, Utils.getIndexByName(arrayName, Utils.getText(editText)), new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
                editText.setText(arrayName[which]);

                String id = listSource != null ? listSource.get(which).id : "";
                editText.setTag(id);

                if (funtion != null) {
                    try {
                        funtion.run();
                    } catch (Exception ex) {
                    }
                }
            }
        });
        builder.show();
    }

    /***
     * Lấy dữ liệu cho combobox
     * @param authorizationKey
     */
    private void getDataCombobox(String authorizationKey) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsquanhevoinannhan))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listQuanHeVoiTre = resultObject.data;

                        arrayQuanHeVoiTre = new String[listQuanHeVoiTre.size()];
                        int index = 0;
                        for (ComboboxResult item : listQuanHeVoiTre) {
                            arrayQuanHeVoiTre[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsloaidoituongxamhai))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listLoaiDoiTuongXamHai = resultObject.data;

                        arrayLoaiDoiTuongXamHai = new String[listLoaiDoiTuongXamHai.size()];
                        int index = 0;
                        for (ComboboxResult item : listLoaiDoiTuongXamHai) {
                            arrayLoaiDoiTuongXamHai[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

    private void initDialogAddAbuseObject() {
        dialogAddAbuseObject = new Dialog(getContext());
        dialogAddAbuseObject.setContentView(R.layout.popup_add_abuse_object);
        dialogAddAbuseObject.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogAddAbuseObject.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogAddAbuseObject.setCanceledOnTouchOutside(false);

        txtHoTen = dialogAddAbuseObject.findViewById(R.id.txtHoTen);
        rgGioiTinh = dialogAddAbuseObject.findViewById(R.id.rgGioiTinh);

        spnNamSinh = dialogAddAbuseObject.findViewById(R.id.spnNamSinh);
        spnQuanHeVoiTre = dialogAddAbuseObject.findViewById(R.id.spnQuanHeVoiTre);
        spnLoaiDoiTuongXamHai = dialogAddAbuseObject.findViewById(R.id.spnLoaiDoiTuongXamHai);

        lySave = dialogAddAbuseObject.findViewById(R.id.lySave);
        lyClose = dialogAddAbuseObject.findViewById(R.id.lyClose);

        spnNamSinh.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        spnQuanHeVoiTre.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnQuanHeVoiTre, arrayQuanHeVoiTre, listQuanHeVoiTre, "Chọn mối quan hệ", null);
            }
        });

        spnLoaiDoiTuongXamHai.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnLoaiDoiTuongXamHai, arrayLoaiDoiTuongXamHai, listLoaiDoiTuongXamHai, "Chọn loại đối tượng xâm hại", null);
            }
        });

        lyClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                clearData();
                dialogAddAbuseObject.hide();
            }
        });

        lySave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (!Utils.isEmpty(idDoiTuongEdit)) {
                    editAbuseObject();
                } else {
                    saveAbuseObject();
                }
            }
        });

        TextView btnClose = dialogAddAbuseObject.findViewById(R.id.btnClose);
        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogAddAbuseObject.hide();
            }
        });
    }

    private void saveAbuseObject() {
        getInfoModel();

        if (!validateInput()) {
            return;
        }

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(doiTuongXamHaiModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.doituong))
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<String> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<String>>() {
                        }.getType());
                        if (resultObject.status.equals(Constants.StatusSuccess)) {
                            clearData();
                            getListChild(maSoHoSo);
                            Toast.makeText(getActivity(), "Thêm mới đối tượng thành công!", Toast.LENGTH_SHORT).show();
                        } else {
                            Toast.makeText(getActivity(), "Có lỗi phát sinh thêm mới đối tượng!", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    private void editAbuseObject() {
        getInfoModel();

        if (!validateInput()) {
            return;
        }

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(doiTuongXamHaiModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.suadoituong) + idDoiTuongEdit)
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<String> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<String>>() {
                        }.getType());
                        if (resultObject.status.equals(Constants.StatusSuccess)) {
                            clearData();
                            getListChild(maSoHoSo);
                            Toast.makeText(getActivity(), "Cập nhật đối tượng thành công!", Toast.LENGTH_SHORT).show();
                        } else {
                            Toast.makeText(getActivity(), "Có lỗi phát sinh cập nhật đối tượng!", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    private void viewDataEdit(DoiTuongXamHaiViewModel obj) {
        txtHoTen.setText(obj.hoTen);
        for (int index = 0; index < rgGioiTinh.getChildCount(); index++) {
            RadioButton radioButton = (RadioButton) rgGioiTinh.getChildAt(index);
            if (radioButton.getTag().toString().equals(obj.gioiTinhId)) {
                radioButton.setChecked(true);
            }
        }
        spnNamSinh.setText(DateUtils.ConvertYMDServerToDMY(obj.namSinh));
        spnQuanHeVoiTre.setTag(obj.quanHeVoiTreId);
        spnQuanHeVoiTre.setText(obj.quanHeVoiTre);
        spnLoaiDoiTuongXamHai.setTag(obj.loaiDoiTuongXamHaiId);
        spnLoaiDoiTuongXamHai.setText(obj.loaiDoiTuongXamHai);
    }

    /***
     * Gét thông tin trên fragment
     */
    private void getInfoModel() {
        doiTuongXamHaiModel = new DoiTuongXamHaiModel();
        doiTuongXamHaiModel.maSoHoSo = maSoHoSo;
        doiTuongXamHaiModel.hoTen = txtHoTen.getText().toString();
        int radioButtonID = rgGioiTinh.getCheckedRadioButtonId();
        RadioButton radioButton = (RadioButton) rgGioiTinh.findViewById(radioButtonID);
        if (radioButton != null) {
            doiTuongXamHaiModel.gioiTinh = radioButton.getTag().toString();
        }
        doiTuongXamHaiModel.namSinh = DateUtils.ConvertDMYToYMD(spnNamSinh.getText().toString());
        doiTuongXamHaiModel.quanHeVoiTre = spnQuanHeVoiTre.getTag() != null ? spnQuanHeVoiTre.getTag().toString() : "";
        doiTuongXamHaiModel.loaiDoiTuongXamHai = spnLoaiDoiTuongXamHai.getTag() != null ? spnLoaiDoiTuongXamHai.getTag().toString() : "";
    }

    private boolean validateInput() {
        if (Utils.isDataEmpty(doiTuongXamHaiModel.hoTen)) {
            Toast.makeText(getActivity(), "Chưa nhập họ tên!", Toast.LENGTH_SHORT).show();
            return false;
        }
        if (Utils.isDataEmpty(doiTuongXamHaiModel.gioiTinh)) {
            Toast.makeText(getActivity(), "Chưa chọn giới tính!", Toast.LENGTH_SHORT).show();
            return false;
        }
        if (Utils.isDataEmpty(doiTuongXamHaiModel.namSinh)) {
            Toast.makeText(getActivity(), "Chưa nhập năm sinh!", Toast.LENGTH_SHORT).show();
            return false;
        }
        return true;
    }

    private void clearData() {
        idDoiTuongEdit = "";
        doiTuongXamHaiModel = new DoiTuongXamHaiModel();
        txtHoTen.setText("");
        ((RadioButton) rgGioiTinh.getChildAt(0)).setChecked(true);
        spnNamSinh.setText("");
        spnQuanHeVoiTre.setText("");
        spnQuanHeVoiTre.setTag("");
        spnLoaiDoiTuongXamHai.setText("");
        spnLoaiDoiTuongXamHai.setTag("");
    }

    private void getListChild(String soHoSo) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsDoiTuong) + "?maSoHoSo=" + soHoSo)
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultModel<List<DoiTuongXamHaiViewModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<DoiTuongXamHaiViewModel>>>() {
                            }.getType());

                            if (resultObject.status.equals(Constants.StatusSuccess)) {
                                listDoiTuong = resultObject.data;
                                prisonerListAdapter = new PrisonerListAdapter(getContext(), listDoiTuong);
                                rvPrisoner.setAdapter(prisonerListAdapter);
                                prisonerListAdapter.SetOnItemClickListener(new PrisonerListAdapter.OnItemClickListener() {
                                    @Override
                                    public void onEditClick(View view, int position, DoiTuongXamHaiViewModel obj) {

                                    }

                                    @Override
                                    public void onDetailClick(View view, int position, DoiTuongXamHaiViewModel obj) {
                                        thongDoiTuong(obj);
                                    }
                                });

                                if (resultObject.totalrecord > 0) {
                                    tvNotRecord.setVisibility(View.GONE);
                                } else {
                                    tvNotRecord.setVisibility(View.VISIBLE);
                                }
                            }

                        } catch (Exception ex) {

                        }
                    }

                    @Override
                    public void onError(ANError anError) {

                    }
                });
    }

    private void initDialogDetailAbuseObject() {
        dialogDetailChildAbuseObject = new Dialog(getContext());
        dialogDetailChildAbuseObject.setContentView(R.layout.popup_detail_abuse_object);
        dialogDetailChildAbuseObject.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogDetailChildAbuseObject.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogDetailChildAbuseObject.setCanceledOnTouchOutside(false);

        btnCloseDetail = dialogDetailChildAbuseObject.findViewById(R.id.btnCloseDetail);
        tvHoTen = dialogDetailChildAbuseObject.findViewById(R.id.tvHoTen);
        tvGioiTinh = dialogDetailChildAbuseObject.findViewById(R.id.tvGioiTinh);
        tvNamSinh = dialogDetailChildAbuseObject.findViewById(R.id.tvNamSinh);
        tvQuanHeVoiTre = dialogDetailChildAbuseObject.findViewById(R.id.tvQuanHeVoiTre);
        tvLoaiDoiTuongXamHai = dialogDetailChildAbuseObject.findViewById(R.id.tvLoaiDoiTuongXamHai);

        btnCloseDetail.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogDetailChildAbuseObject.hide();
            }
        });
    }

    private void thongDoiTuong(DoiTuongXamHaiViewModel obj) {
        tvHoTen.setText(obj.hoTen);
        tvGioiTinh.setText(obj.gioiTinh);
        tvNamSinh.setText(DateUtils.ConvertYMDServerToDMY(obj.namSinh));
        tvQuanHeVoiTre.setText(obj.quanHeVoiTre);
        tvLoaiDoiTuongXamHai.setText(obj.loaiDoiTuongXamHai);
        dialogDetailChildAbuseObject.show();
    }
}
