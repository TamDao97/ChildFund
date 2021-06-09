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
import android.widget.Switch;
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

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.ChildListAdapter;
import reportward.nts.reportward.adapter.ChildObjectTypeListAdapter;
import reportward.nts.reportward.adapter.ChooseListAdapter;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.ItemObjectInSubModel;
import reportward.nts.reportward.model.ComboboxResult;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;
import reportward.nts.reportward.model.ThongTinTreModel;
import reportward.nts.reportward.model.ThongTinTreViewModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseChildFragment extends Fragment {
    private boolean isLoad = true;
    private View view;
    private EditText txtHoTen, txtDiaChi;
    private RadioGroup rgGioiTinh;
    private Switch swTreCoDiHoc;
    private EditText spnNamSinh, spnDanToc;
    private TextView tvNotRecord, btnClose;
    private RecyclerView rvChild, rvObjectType, rvObjectTypeChoose;
    private ChildListAdapter childListAdapter;
    private ChildObjectTypeListAdapter childObjectTypeListAdapter;
    private List<ItemObjectInSubModel> listLoaiDoiTuong;
    private List<ComboboxResult> listLoaiDoiTuongChoose = new ArrayList<>();
    private ChooseListAdapter chooseListAdapter;
    private String[] arrayDanToc;
    private List<ComboboxResult> listDanToc;
    private static String maSoHoSo;
    private Dialog dialogAddChild, dialogDetailChild;
    private LinearLayout lyAddChild, lySave, lyClose;
    private ThongTinTreModel thongTinTreModel;
    private TextView btnCloseDetail, tvHoTen, tvGioiTinh, tvDanToc, tvNamSinh, tvTreCoDiHoc, tvDiaChi, tvDoiTuongTre;
    private List<ThongTinTreViewModel> listTre;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    public ReportAbuseChildFragment() {

    }

    public static ReportAbuseChildFragment newInstance(String maHoSo) {
        maSoHoSo = maHoSo;
        ReportAbuseChildFragment fragment = new ReportAbuseChildFragment();
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        if (isLoad) {
            view = inflater.inflate(R.layout.fragment_report_abuse_child, container, false);

            //Get thông tin login
            prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
            String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
            if (loginProfile != null) {
                loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
                }.getType());
            };

            initComponent();

            initDialogAddChild();

            initDialogDetailChild();

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
        rvChild = view.findViewById(R.id.rvChild);
        rvChild.setLayoutManager(new GridLayoutManager(getActivity(), 1));

        tvNotRecord = view.findViewById(R.id.tvNotRecord);

        lyAddChild = view.findViewById(R.id.lyAddChild);
        lyAddChild.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogAddChild.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.WRAP_CONTENT);
                dialogAddChild.show();
            }
        });
        if (listTre != null && listTre.size() > 0) {
            tvNotRecord.setVisibility(View.GONE);
            childListAdapter = new ChildListAdapter(getContext(), listTre);
            rvChild.setAdapter(childListAdapter);
            childListAdapter.SetOnItemClickListener(new ChildListAdapter.OnItemClickListener() {
                @Override
                public void onDetailClick(View view, int position, ThongTinTreViewModel obj) {
                    thongTinTre(obj);
                }
            });
        } else {
            tvNotRecord.setVisibility(View.VISIBLE);
        }
    }

    /***
     * Show Dialog chọn
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
     * Show Dialog mutile choose
     * @param arrayName
     * @param title
     * @param funtion
     */
    private void showMultiChooseDialog(final String[] arrayName, final boolean[] arrayCheck, final String[] arrayId, final String title, final Runnable funtion) {
        final android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(getActivity());
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setMultiChoiceItems(arrayName, arrayCheck, new DialogInterface.OnMultiChoiceClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which, boolean isChecked) {
                arrayCheck[which] = isChecked;
            }
        });

        // Set the positive/yes button click listener
        builder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                ComboboxResult comboboxResult;
                for (int i = 0; i < arrayCheck.length; i++) {
                    boolean checked = arrayCheck[i];
                    if (checked) {
                        comboboxResult = new ComboboxResult();
                        comboboxResult.text = arrayName[i];
                        comboboxResult.id = arrayId[i];
                        listLoaiDoiTuongChoose.add(comboboxResult);
                    } else {
                        if (listLoaiDoiTuongChoose != null && listLoaiDoiTuongChoose.size() > 0) {
                            for (ComboboxResult item : listLoaiDoiTuongChoose) {
                                if (item.id.equals(arrayId[i])) {
                                    listLoaiDoiTuongChoose.remove(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                chooseListAdapter.notifyDataSetChanged();
                if (listLoaiDoiTuongChoose != null && listLoaiDoiTuongChoose.size() > 0) {
                    rvObjectTypeChoose.setVisibility(View.VISIBLE);
                } else {
                    rvObjectTypeChoose.setVisibility(View.GONE);
                }
            }
        });

        // Set the neutral/cancel button click listener
        builder.setNeutralButton("Cancel", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                // Do something when click the neutral button
            }
        });

        builder.show();
    }

    /***
     * Lấy dữ liệu cho combobox
     * @param authorizationKey
     */
    private void getDataCombobox(String authorizationKey) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.dsdoituongtre))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ItemObjectInSubModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ItemObjectInSubModel>>>() {
                        }.getType());
                        listLoaiDoiTuong = resultObject.data;
                        childObjectTypeListAdapter = new ChildObjectTypeListAdapter(getActivity(), listLoaiDoiTuong);
                        rvObjectType.setAdapter(childObjectTypeListAdapter);
                        childObjectTypeListAdapter.SetOnItemClickListener(new ChildObjectTypeListAdapter.OnItemClickListener() {
                            @Override
                            public void onItemClick(View view, int position, ItemObjectInSubModel childObjectTypeModel) {
                                if (childObjectTypeModel.listItem != null && childObjectTypeModel.listItem.size() > 0) {
                                    String[] arrayName = new String[childObjectTypeModel.listItem.size()];
                                    boolean[] arrayCheck = new boolean[childObjectTypeModel.listItem.size()];
                                    String[] arrayId = new String[childObjectTypeModel.listItem.size()];
                                    int index = 0;
                                    for (ItemObjectInSubModel item : childObjectTypeModel.listItem) {
                                        arrayName[index] = item.text;
                                        arrayId[index] = item.id;
                                        arrayCheck[index] = false;
                                        if (listLoaiDoiTuongChoose != null && listLoaiDoiTuongChoose.size() > 0) {
                                            for (ComboboxResult itemCheck : listLoaiDoiTuongChoose) {
                                                if (itemCheck.id.equals(item.id)) {
                                                    arrayCheck[index] = true;
                                                    break;
                                                }
                                            }
                                        }
                                        index++;
                                    }
                                    showMultiChooseDialog(arrayName, arrayCheck, arrayId, "Chọn đối tượng trẻ", null);
                                }
                            }
                        });
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.dsDanToc))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listDanToc = resultObject.data;

                        arrayDanToc = new String[listDanToc.size()];
                        int index = 0;
                        for (ComboboxResult item : listDanToc) {
                            arrayDanToc[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

    private void initDialogAddChild() {
        dialogAddChild = new Dialog(getContext());
        dialogAddChild.setContentView(R.layout.popup_add_abuse_child);
        dialogAddChild.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogAddChild.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogAddChild.setCanceledOnTouchOutside(false);

        btnClose = dialogAddChild.findViewById(R.id.btnClose);

        txtHoTen = dialogAddChild.findViewById(R.id.txtHoTen);
        txtDiaChi = dialogAddChild.findViewById(R.id.txtDiaChi);
        rgGioiTinh = dialogAddChild.findViewById(R.id.rgGioiTinh);
        swTreCoDiHoc = dialogAddChild.findViewById(R.id.swTreCoDiHoc);

        spnNamSinh = dialogAddChild.findViewById(R.id.spnNamSinh);
        spnDanToc = dialogAddChild.findViewById(R.id.spnDanToc);

        rvObjectType = dialogAddChild.findViewById(R.id.rvObjectType);
        rvObjectType.setLayoutManager(new GridLayoutManager(getActivity(), 1));

        rvObjectTypeChoose = dialogAddChild.findViewById(R.id.rvObjectTypeChoose);
        rvObjectTypeChoose.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        rvObjectTypeChoose.setHasFixedSize(true);
        chooseListAdapter = new ChooseListAdapter(getActivity(), listLoaiDoiTuongChoose);
        rvObjectTypeChoose.setAdapter(chooseListAdapter);
        chooseListAdapter.SetOnItemClickListener(new ChooseListAdapter.OnItemClickListener() {
            @Override
            public void onItemClick(View view, int position, ComboboxResult comboboxResult) {
                listLoaiDoiTuongChoose.remove(position);
                chooseListAdapter.notifyDataSetChanged();
                if (listLoaiDoiTuongChoose != null && listLoaiDoiTuongChoose.size() > 0) {
                    rvObjectTypeChoose.setVisibility(View.VISIBLE);
                } else {
                    rvObjectTypeChoose.setVisibility(View.GONE);
                }
            }
        });

        lySave = dialogAddChild.findViewById(R.id.lySave);
        lyClose = dialogAddChild.findViewById(R.id.lyClose);

        spnNamSinh.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        spnDanToc.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnDanToc, arrayDanToc, listDanToc, "Chọn dân tộc", null);
            }
        });

        lyClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogAddChild.hide();
            }
        });

        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogAddChild.hide();
            }
        });

        lySave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                saveChild();
            }
        });
    }

    private void saveChild() {
        getInfoModel();

        if (!validateInput()) {
            return;
        }

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(thongTinTreModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.tre))
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
                            Toast.makeText(getActivity(), "Thêm mới trẻ thành công!", Toast.LENGTH_SHORT).show();
                        } else {
                            Toast.makeText(getActivity(), "Có lỗi phát sinh thêm mới trẻ!", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Utils.showErrorMessage(getActivity().getApplication(), anError);
                    }
                });
    }

    /***
     * Gét thông tin trên fragment
     */
    private void getInfoModel() {
        thongTinTreModel = new ThongTinTreModel();
        thongTinTreModel.maSoHoSo = maSoHoSo;
        thongTinTreModel.hoTen = txtHoTen.getText().toString();
        thongTinTreModel.danToc = spnDanToc.getTag() != null ? spnDanToc.getTag().toString() : "";
        int radioButtonID = rgGioiTinh.getCheckedRadioButtonId();
        RadioButton radioButton = (RadioButton) rgGioiTinh.findViewById(radioButtonID);
        if (radioButton != null) {
            thongTinTreModel.gioiTinh = radioButton.getTag().toString();
        }
        thongTinTreModel.namSinh = DateUtils.ConvertDMYToYMD(spnNamSinh.getText().toString());
        thongTinTreModel.treCoDiHoc = swTreCoDiHoc.isChecked();
        thongTinTreModel.diaChi = txtDiaChi.getText().toString();
        thongTinTreModel.doiTuongTre = "";
        if (listLoaiDoiTuongChoose != null && listLoaiDoiTuongChoose.size() > 0) {
            for (ComboboxResult itemChoose : listLoaiDoiTuongChoose) {
                thongTinTreModel.doiTuongTre += "," + itemChoose.id;
            }
            thongTinTreModel.doiTuongTre = thongTinTreModel.doiTuongTre.replaceFirst(",", "");
        }
    }

    private boolean validateInput() {
        if (Utils.isDataEmpty(thongTinTreModel.hoTen)) {
            Toast.makeText(getActivity(), "Chưa nhập họ tên!", Toast.LENGTH_SHORT).show();
            return false;
        }
        if (Utils.isDataEmpty(thongTinTreModel.gioiTinh)) {
            Toast.makeText(getActivity(), "Chưa chọn giới tính!", Toast.LENGTH_SHORT).show();
            return false;
        }
        if (Utils.isDataEmpty(thongTinTreModel.namSinh)) {
            Toast.makeText(getActivity(), "Chưa nhập năm sinh!", Toast.LENGTH_SHORT).show();
            return false;
        }
        if (Utils.isDataEmpty(thongTinTreModel.danToc)) {
            Toast.makeText(getActivity(), "Chưa chọn dân tộc!", Toast.LENGTH_SHORT).show();
            return false;
        }
        return true;
    }

    private void clearData() {
        thongTinTreModel = new ThongTinTreModel();
        txtHoTen.setText("");
        spnDanToc.setText("");
        spnDanToc.setTag("");
        ((RadioButton) rgGioiTinh.getChildAt(0)).setChecked(true);
        spnNamSinh.setText("");
        swTreCoDiHoc.setChecked(false);
        txtDiaChi.setText("");
        listLoaiDoiTuongChoose = new ArrayList<>();
        chooseListAdapter = new ChooseListAdapter(getActivity(), listLoaiDoiTuongChoose);
        rvObjectTypeChoose.setAdapter(chooseListAdapter);
    }

    private void getListChild(String soHoSo) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.dsTre) + "?maSoHoSo=" + soHoSo)
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultModel<List<ThongTinTreViewModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ThongTinTreViewModel>>>() {
                            }.getType());

                            if (resultObject.status.equals(Constants.StatusSuccess)) {
                                listTre = resultObject.data;
                                childListAdapter = new ChildListAdapter(getContext(), listTre);
                                rvChild.setAdapter(childListAdapter);
                                childListAdapter.SetOnItemClickListener(new ChildListAdapter.OnItemClickListener() {
                                    @Override
                                    public void onDetailClick(View view, int position, ThongTinTreViewModel obj) {
                                        thongTinTre(obj);
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

    private void initDialogDetailChild() {
        dialogDetailChild = new Dialog(getContext());
        dialogDetailChild.setContentView(R.layout.popup_detail_abuse_child);
        dialogDetailChild.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogDetailChild.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogDetailChild.setCanceledOnTouchOutside(false);

        btnCloseDetail = dialogDetailChild.findViewById(R.id.btnCloseDetail);
        tvHoTen = dialogDetailChild.findViewById(R.id.tvHoTen);
        tvGioiTinh = dialogDetailChild.findViewById(R.id.tvGioiTinh);
        tvDanToc = dialogDetailChild.findViewById(R.id.tvDanToc);
        tvNamSinh = dialogDetailChild.findViewById(R.id.tvNamSinh);
        tvTreCoDiHoc = dialogDetailChild.findViewById(R.id.tvTreCoDiHoc);
        tvDiaChi = dialogDetailChild.findViewById(R.id.tvDiaChi);
        tvDoiTuongTre = dialogDetailChild.findViewById(R.id.tvDoiTuongTre);

        btnCloseDetail.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogDetailChild.hide();
            }
        });
    }

    private void thongTinTre(ThongTinTreViewModel obj) {
        tvHoTen.setText(obj.hoTen);
        tvGioiTinh.setText(obj.gioiTinh);
        tvDanToc.setText(obj.danToc);
        tvNamSinh.setText(DateUtils.ConvertYMDServerToDMY(obj.namSinh));
        tvTreCoDiHoc.setText(obj.treCoDiHoc);
        tvDiaChi.setText(obj.diaChi);
        tvDoiTuongTre.setText(obj.doiTuongTre);

        dialogDetailChild.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.WRAP_CONTENT);
        dialogDetailChild.show();
    }
}
