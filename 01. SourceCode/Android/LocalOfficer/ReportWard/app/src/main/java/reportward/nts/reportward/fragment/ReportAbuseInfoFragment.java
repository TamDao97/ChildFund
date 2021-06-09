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
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RelativeLayout;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.ChooseListAdapter;
import reportward.nts.reportward.adapter.VanDeListAdapter;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.ComboboxResult;
import reportward.nts.reportward.model.HoSoCaTuVanModel;
import reportward.nts.reportward.model.ItemObjectInSubModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * to handle interaction events.
 * create an instance of this fragment.
 */
public class ReportAbuseInfoFragment extends Fragment {
    private static boolean isLoad = true;
    private View view;
    private LinearLayout lySuport;
    public EditText txtNgaytao, txtHoTen, txtDiaChi, txtSoDienThoai, txtMangXaHoi, txtEmail;
    public TextView txtMaSoHoSo;
    private Switch swLienQuanInternet;
    public RadioGroup rgGioiTinh;
    public EditText spnLoaiCa, spnCacVanDe, spnNguonThongTin, spnNamSinh, spnDoiTuong, spnDanToc, spnMoiTruongXamHai,
            spnTinhNguoiGoi, spnHuyenNguoiGoi, spnXaNguoiGoi;
    private String[] arrayDoiTuong, arrayLoaiCa, arrayNguonThongTin, arrayDanToc, arrayPhanLoaiHoSo, arrayMoiTruongXamHai,
            arrayTrangThaiHoSo, arrayTinh, arrayHuyen, arrayXa;
    private List<ComboboxResult> listDoiTuong, listLoaiCa, listNguonThongTin, listDanToc, listPhanLoaiHoSo, listMoiTruongXamHai,
            listTrangThaiHoSo, listTinh, listHuyen, listXa;
    private static String maSoHoSo;
    private RecyclerView rvVanDe, rvVanDeChoose;
    private VanDeListAdapter vanDeListAdapter;
    private ChooseListAdapter chooseListVanDeAdapter;
    private List<ItemObjectInSubModel> listVanDe;
    private Dialog dialogChoosevanDe;
    private Button btnChoose, btnClose;
    private List<ComboboxResult> listVanDeChoose = new ArrayList<>(), listVanDeChooseTemp = new ArrayList<>(), listVanDeUnChooseTemp = new ArrayList<>();
    private HoSoCaTuVanModel hoSoCaTuVanModel;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;
    private RelativeLayout progressDialog;
    private int countResponse = 0;

    public ReportAbuseInfoFragment() {
        String temp = "";
    }

    public static ReportAbuseInfoFragment newInstance(String maHoSo) {
        maSoHoSo = maHoSo;
        isLoad = true;
        ReportAbuseInfoFragment fragment = new ReportAbuseInfoFragment();
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        if (isLoad) {
            view = inflater.inflate(R.layout.fragment_report_abuse_info, container, false);

            //Get thông tin login
            prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
            String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
            if (loginProfile != null) {
                loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
                }.getType());
            }

            initComponent();

            clearData();

            initDialogChooseVanDe();

            getDataCombobox(loginProfileModel.accessToken);

            if (listVanDeChoose != null && listVanDeChoose.size() > 0) {
                rvVanDeChoose.setVisibility(View.VISIBLE);
            } else {
                rvVanDeChoose.setVisibility(View.GONE);
            }
        }
        isLoad = false;

        return view;
    }

    private void initComponent() {
        progressDialog = view.findViewById(R.id.progressDialog);

        txtMaSoHoSo = view.findViewById(R.id.txtMaSoHoSo);
        txtMaSoHoSo.setText(maSoHoSo);
        txtNgaytao = view.findViewById(R.id.txtNgaytao);
        txtNgaytao.setText(DateUtils.CurrentDate(DateUtils.DATE_FORMAT_DD_MM_YYYY));
        txtHoTen = view.findViewById(R.id.txtHoTen);
        txtDiaChi = view.findViewById(R.id.txtDiaChi);
        txtSoDienThoai = view.findViewById(R.id.txtSoDienThoai);
        txtMangXaHoi = view.findViewById(R.id.txtMangXaHoi);
        txtEmail = view.findViewById(R.id.txtEmail);

        swLienQuanInternet = view.findViewById(R.id.swLienQuanInternet);
        rgGioiTinh = view.findViewById(R.id.rgGioiTinh);

        spnLoaiCa = view.findViewById(R.id.spnLoaiCa);
        spnCacVanDe = view.findViewById(R.id.spnCacVanDe);
        spnNguonThongTin = view.findViewById(R.id.spnNguonThongTin);
        spnNamSinh = view.findViewById(R.id.spnNamSinh);
        spnDoiTuong = view.findViewById(R.id.spnDoiTuong);
        spnDanToc = view.findViewById(R.id.spnDanToc);
        spnMoiTruongXamHai = view.findViewById(R.id.spnMoiTruongXamHai);
        spnTinhNguoiGoi = view.findViewById(R.id.spnTinhNguoiGoi);
        spnHuyenNguoiGoi = view.findViewById(R.id.spnHuyenNguoiGoi);
        spnXaNguoiGoi = view.findViewById(R.id.spnXaNguoiGoi);

        lySuport = view.findViewById(R.id.lySuport);

        rvVanDeChoose = view.findViewById(R.id.rvVanDeChoose);
        rvVanDeChoose.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        chooseListVanDeAdapter = new ChooseListAdapter(getActivity(), listVanDeChoose);
        rvVanDeChoose.setAdapter(chooseListVanDeAdapter);

        txtNgaytao.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        spnNamSinh.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        spnNamSinh.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String birthday = spnNamSinh.getText().toString();
                SimpleDateFormat dateFormat = new SimpleDateFormat(DateUtils.DATE_FORMAT_DD_MM_YYYY);
                try {
                    if (!birthday.isEmpty() && dateFormat.parse(birthday).compareTo(dateFormat.parse(DateUtils.CurrentDate(DateUtils.DATE_FORMAT_DD_MM_YYYY))) >= 0) {
                        spnNamSinh.setText("");
                        spnNamSinh.requestFocus();
                        Toast.makeText(getActivity(), "Năm sinh không đúng.", Toast.LENGTH_SHORT).show();
                    }

                } catch (Exception ex) {
                    spnNamSinh.setText("");
                    spnNamSinh.requestFocus();
                    Toast.makeText(getActivity(), "Năm sinh không đúng.", Toast.LENGTH_SHORT).show();
                }
            }
        });

        spnLoaiCa.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnLoaiCa, arrayLoaiCa, listLoaiCa, "Chọn loại ca", runnableVisibilitySuport);
            }
        });

        spnNguonThongTin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnNguonThongTin, arrayNguonThongTin, listNguonThongTin, "Chọn nguồn thông tin", null);
            }
        });

        spnDoiTuong.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnDoiTuong, arrayDoiTuong, listDoiTuong, "Chọn đối tượng cung cấp", null);
            }
        });

        spnDanToc.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnDanToc, arrayDanToc, listDanToc, "Chọn dân tộc", null);
            }
        });

        spnMoiTruongXamHai.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnMoiTruongXamHai, arrayMoiTruongXamHai, listMoiTruongXamHai, "Chọn môi trường xâm hại", null);
            }
        });

        spnTinhNguoiGoi.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnTinhNguoiGoi, arrayTinh, listTinh, "Chọn Tỉnh/ Thành", runnableDataDistrict);
            }
        });

        spnHuyenNguoiGoi.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnHuyenNguoiGoi, arrayHuyen, listHuyen, "Chọn Quận/ Huyện", runnableDataWrap);
            }
        });

        spnXaNguoiGoi.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnXaNguoiGoi, arrayXa, listXa, "Chọn Xã/ Phường", null);
            }
        });

        spnCacVanDe.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                listVanDeChooseTemp = new ArrayList<>();
                listVanDeUnChooseTemp = new ArrayList<>();
                vanDeListAdapter = new VanDeListAdapter(getActivity(), listVanDe, listVanDeChoose);
                rvVanDe.setAdapter(vanDeListAdapter);
                vanDeListAdapter.SetOnItemClickListener(new VanDeListAdapter.OnItemClickListener() {
                    @Override
                    public void onCheckClick(boolean isCheck, ItemObjectInSubModel childObjectTypeModel) {
                        ComboboxResult comboboxResultAdd = new ComboboxResult();
                        comboboxResultAdd.id = childObjectTypeModel.id;
                        comboboxResultAdd.text = childObjectTypeModel.text;
                        if (isCheck) {
                            boolean isExit = false;
                            if (listVanDeChooseTemp != null && listVanDeChooseTemp.size() > 0) {
                                for (ComboboxResult itemCheck : listVanDeChooseTemp) {
                                    if (itemCheck.id.equals(comboboxResultAdd.id)) {
                                        isExit = true;
                                        break;
                                    }
                                }
                            }
                            if (!isExit) {
                                listVanDeChooseTemp.add(comboboxResultAdd);
                            }

                            if (listVanDeUnChooseTemp != null && listVanDeUnChooseTemp.size() > 0) {
                                for (ComboboxResult itemCheck : listVanDeUnChooseTemp) {
                                    if (itemCheck.id.equals(comboboxResultAdd.id)) {
                                        listVanDeUnChooseTemp.remove(itemCheck);
                                        break;
                                    }
                                }
                            }
                        } else {
                            boolean isExit = false;
                            if (listVanDeUnChooseTemp != null && listVanDeUnChooseTemp.size() > 0) {
                                for (ComboboxResult itemCheck : listVanDeUnChooseTemp) {
                                    if (itemCheck.id.equals(comboboxResultAdd.id)) {
                                        isExit = true;
                                        break;
                                    }
                                }
                            }
                            if (!isExit) {
                                listVanDeUnChooseTemp.add(comboboxResultAdd);
                            }

                            if (listVanDeChooseTemp != null && listVanDeChooseTemp.size() > 0) {
                                for (ComboboxResult itemCheck : listVanDeChooseTemp) {
                                    if (itemCheck.id.equals(comboboxResultAdd.id)) {
                                        listVanDeChooseTemp.remove(itemCheck);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    @Override
                    public void onChooseClick(List<ComboboxResult> listChoose, List<ComboboxResult> listUnChoose) {
                        if (listChoose != null && listChoose.size() > 0) {
                            for (ComboboxResult itemChoose : listChoose) {
                                boolean isExit = false;
                                if (listVanDeChooseTemp != null && listVanDeChooseTemp.size() > 0) {
                                    for (ComboboxResult itemCheck : listVanDeChooseTemp) {
                                        if (itemCheck.id.equals(itemChoose.id)) {
                                            isExit = true;
                                            break;
                                        }
                                    }
                                }
                                if (!isExit) {
                                    listVanDeChooseTemp.add(itemChoose);
                                }

                                if (listVanDeUnChooseTemp != null && listVanDeUnChooseTemp.size() > 0) {
                                    for (ComboboxResult itemCheck : listVanDeUnChooseTemp) {
                                        if (itemCheck.id.equals(itemChoose.id)) {
                                            listVanDeUnChooseTemp.remove(itemCheck);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (listUnChoose != null && listChoose.size() > 0) {
                            for (ComboboxResult itemUnChoose : listUnChoose) {
                                boolean isExit = false;
                                if (listVanDeUnChooseTemp != null && listVanDeUnChooseTemp.size() > 0) {
                                    for (ComboboxResult itemCheck : listVanDeUnChooseTemp) {
                                        if (itemCheck.id.equals(itemUnChoose.id)) {
                                            isExit = true;
                                            break;
                                        }
                                    }
                                }
                                if (!isExit) {
                                    listVanDeUnChooseTemp.add(itemUnChoose);
                                }

                                if (listVanDeChooseTemp != null && listVanDeChooseTemp.size() > 0) {
                                    for (ComboboxResult itemCheck : listVanDeChooseTemp) {
                                        if (itemCheck.id.equals(itemUnChoose.id)) {
                                            listVanDeChooseTemp.remove(itemCheck);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
                dialogChoosevanDe.show();
            }
        });
    }

    private void clearData() {
        txtSoDienThoai.setText("");
        txtMangXaHoi.setText("");
        txtEmail.setText("");
        spnDoiTuong.setTag("");
        spnDoiTuong.setText("");
        txtHoTen.setText("");
        spnNamSinh.setText("");
        txtDiaChi.setText("");
        spnTinhNguoiGoi.setTag("");
        spnTinhNguoiGoi.setText("");
        spnHuyenNguoiGoi.setTag("");
        spnHuyenNguoiGoi.setText("");
        spnXaNguoiGoi.setTag("");
        spnXaNguoiGoi.setText("");
        spnDanToc.setTag("");
        spnDanToc.setText("");
        spnLoaiCa.setTag("");
        spnLoaiCa.setText("");
        spnNguonThongTin.setTag("");
        spnNguonThongTin.setText("");
        spnMoiTruongXamHai.setTag("");
        spnMoiTruongXamHai.setText("");
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
        progressDialog.setVisibility(View.VISIBLE);
        countResponse = 0;

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsVanDe))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        countResponse++;
                        visibilityProgressDialog();
                        String tamp = "";
                        ResultModel<List<ItemObjectInSubModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ItemObjectInSubModel>>>() {
                        }.getType());
                        listVanDe = resultObject.data;
                    }

                    @Override
                    public void onError(ANError anError) {
                        countResponse++;
                        visibilityProgressDialog();
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsDoiTuongGoi))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        countResponse++;
                        visibilityProgressDialog();
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listDoiTuong = resultObject.data;

                        arrayDoiTuong = new String[listDoiTuong.size()];
                        int index = 0;
                        for (ComboboxResult item : listDoiTuong) {
                            arrayDoiTuong[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        countResponse++;
                        visibilityProgressDialog();
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsloaihoso))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        countResponse++;
                        visibilityProgressDialog();
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listLoaiCa = resultObject.data;

                        arrayLoaiCa = new String[listLoaiCa.size()];
                        int index = 0;
                        for (ComboboxResult item : listLoaiCa) {
                            if (index == 0 && hoSoCaTuVanModel == null) {
                                spnLoaiCa.setTag(item.id);
                                spnLoaiCa.setText(item.text);
                            }
                            arrayLoaiCa[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        countResponse++;
                        visibilityProgressDialog();
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsnguonthongtin))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        countResponse++;
                        visibilityProgressDialog();
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listNguonThongTin = resultObject.data;

                        arrayNguonThongTin = new String[listNguonThongTin.size()];
                        int index = 0;
                        for (ComboboxResult item : listNguonThongTin) {
                            arrayNguonThongTin[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        countResponse++;
                        visibilityProgressDialog();
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsDanToc))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        countResponse++;
                        visibilityProgressDialog();
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
                        countResponse++;
                        visibilityProgressDialog();
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsmoitruongxamhai))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        countResponse++;
                        visibilityProgressDialog();
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listMoiTruongXamHai = resultObject.data;

                        arrayMoiTruongXamHai = new String[listMoiTruongXamHai.size()];
                        int index = 0;
                        for (ComboboxResult item : listMoiTruongXamHai) {
                            arrayMoiTruongXamHai[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        countResponse++;
                        visibilityProgressDialog();
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dstinhtp))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        countResponse++;
                        visibilityProgressDialog();
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listTinh = resultObject.data;

                        arrayTinh = new String[listTinh.size()];
                        int index = 0;
                        for (ComboboxResult item : listTinh) {
                            if (item.id.equals(loginProfileModel.tinh) && hoSoCaTuVanModel == null) {
                                spnTinhNguoiGoi.setText(item.text);
                                spnTinhNguoiGoi.setTag(item.id);
                            }
                            arrayTinh[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        countResponse++;
                        visibilityProgressDialog();
                    }
                });

        getDataDistrict(authorizationKey, loginProfileModel.tinh);

        getDataWrap(authorizationKey, loginProfileModel.quanHuyen);
    }

    private void visibilityProgressDialog() {
        if (countResponse == 7) {
            progressDialog.setVisibility(View.GONE);
        }
    }

    Runnable runnableVisibilitySuport = new Runnable() {
        @Override
        public void run() {
            if (spnLoaiCa.getTag() != null && spnLoaiCa.getTag().toString().equals("2")) {
                lySuport.setVisibility(View.VISIBLE);
            } else {
                lySuport.setVisibility(View.GONE);
            }
        }
    };

    Runnable runnableDataDistrict = new Runnable() {
        @Override
        public void run() {
            getDataDistrict(loginProfileModel.accessToken, spnTinhNguoiGoi.getTag().toString());
            spnHuyenNguoiGoi.setTag("");
            spnHuyenNguoiGoi.setText("");
            spnXaNguoiGoi.setTag("");
            spnXaNguoiGoi.setText("");
        }
    };

    private void getDataDistrict(String authorizationKey, String tinhId) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.DSQuanHuyen) + "?maTinh=" + tinhId)
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listHuyen = resultObject.data;

                        arrayHuyen = new String[listHuyen.size()];
                        int index = 0;
                        for (ComboboxResult item : listHuyen) {
                            if (item.id.equals(loginProfileModel.quanHuyen) && hoSoCaTuVanModel == null) {
                                spnHuyenNguoiGoi.setText(item.text);
                                spnHuyenNguoiGoi.setTag(item.id);
                            }
                            arrayHuyen[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

    Runnable runnableDataWrap = new Runnable() {
        @Override
        public void run() {
            getDataWrap(loginProfileModel.accessToken, spnHuyenNguoiGoi.getTag().toString());
            spnXaNguoiGoi.setTag("");
            spnXaNguoiGoi.setText("");
        }
    };

    private void getDataWrap(String authorizationKey, String huyenId) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.DSPhuongXa) + "?maHuyen=" + huyenId)
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listXa = resultObject.data;

                        arrayXa = new String[listXa.size()];
                        int index = 0;
                        for (ComboboxResult item : listXa) {
                            if (item.id.equals(loginProfileModel.xaPhuong) && hoSoCaTuVanModel == null) {
                                spnXaNguoiGoi.setText(item.text);
                                spnXaNguoiGoi.setTag(item.id);
                            }
                            arrayXa[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

    /***
     * Gét thông tin trên fragment
     */
    public HoSoCaTuVanModel GetInfoModel() {
        hoSoCaTuVanModel = new HoSoCaTuVanModel();
        try {
            hoSoCaTuVanModel.MaSoHoSo = txtMaSoHoSo.getText().toString();
            hoSoCaTuVanModel.SoDienThoai = txtSoDienThoai.getText().toString();
            hoSoCaTuVanModel.MangXaHoi = txtMangXaHoi.getText().toString();
            hoSoCaTuVanModel.Email = txtEmail.getText().toString();
            hoSoCaTuVanModel.MaDoiTuong = spnDoiTuong.getTag() != null ? spnDoiTuong.getTag().toString() : "";
            hoSoCaTuVanModel.HoTen = txtHoTen.getText().toString();
            hoSoCaTuVanModel.NamSinh = DateUtils.ConvertDMYToYMD(spnNamSinh.getText().toString());
            int radioButtonID = rgGioiTinh.getCheckedRadioButtonId();
            RadioButton radioButton = (RadioButton) rgGioiTinh.findViewById(radioButtonID);
            if (radioButton != null) {
                hoSoCaTuVanModel.GioiTinh = radioButton.getTag().toString();
            }
            hoSoCaTuVanModel.DiaChi = txtDiaChi.getText().toString();
            hoSoCaTuVanModel.NguoiDung = loginProfileModel.tenDangNhap;
            hoSoCaTuVanModel.MaTinh = loginProfileModel.tinh;
            hoSoCaTuVanModel.MaTinhNguoiGoi = spnTinhNguoiGoi.getTag() != null ? spnTinhNguoiGoi.getTag().toString() : "";
            hoSoCaTuVanModel.MaHuyenNguoiGoi = spnHuyenNguoiGoi.getTag() != null ? spnHuyenNguoiGoi.getTag().toString() : "";
            hoSoCaTuVanModel.MaXaNguoiGoi = spnXaNguoiGoi.getTag() != null ? spnXaNguoiGoi.getTag().toString() : "";
            hoSoCaTuVanModel.MaDanToc = spnDanToc.getTag() != null ? spnDanToc.getTag().toString() : "";
            hoSoCaTuVanModel.LoaiCa = spnLoaiCa.getTag() != null ? spnLoaiCa.getTag().toString() : "";
            hoSoCaTuVanModel.LoaiHoSo = spnLoaiCa.getTag() != null ? spnLoaiCa.getTag().toString() : "";
            hoSoCaTuVanModel.CacVanDe = "";
            if (listVanDeChoose != null && listVanDeChoose.size() > 0) {
                for (ComboboxResult itemChoose : listVanDeChoose) {
                    hoSoCaTuVanModel.CacVanDe += "," + itemChoose.id;
                }
                hoSoCaTuVanModel.CacVanDe = hoSoCaTuVanModel.CacVanDe.replaceFirst(",", "");
            }
            hoSoCaTuVanModel.NguonThongTin = spnNguonThongTin.getTag() != null ? spnNguonThongTin.getTag().toString() : "";
            hoSoCaTuVanModel.LienQuanInternet = swLienQuanInternet.isChecked();
            //Loại ca hỗ trợ mới lấy thông tin này
            if (hoSoCaTuVanModel.LoaiCa.equals("2")) {
                hoSoCaTuVanModel.MoiTruongXamHai = spnMoiTruongXamHai.getTag() != null ? spnMoiTruongXamHai.getTag().toString() : "";
            }
            hoSoCaTuVanModel.KetQuaHoTroCanThiep = "";
        } catch (Exception ex) {
        }
        return hoSoCaTuVanModel;
    }

    private void initDialogChooseVanDe() {
        dialogChoosevanDe = new Dialog(getContext());
        dialogChoosevanDe.setContentView(R.layout.popup_choose_vande);
        dialogChoosevanDe.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogChoosevanDe.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogChoosevanDe.setCanceledOnTouchOutside(false);

        rvVanDe = dialogChoosevanDe.findViewById(R.id.rvVanDe);
        rvVanDe.setLayoutManager(new GridLayoutManager(getActivity(), 1));

        btnChoose = dialogChoosevanDe.findViewById(R.id.btnChoose);
        btnClose = dialogChoosevanDe.findViewById(R.id.btnClose);

        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogChoosevanDe.hide();
            }
        });

        btnChoose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                for (ComboboxResult comboboxResult : listVanDeChooseTemp) {
                    boolean isExit = false;
                    for (ComboboxResult itemChoose : listVanDeChoose) {
                        if (itemChoose.id.equals(comboboxResult.id)) {
                            isExit = true;
                        }
                    }
                    if (!isExit)
                        listVanDeChoose.add(comboboxResult);
                }

                List<ComboboxResult> listTemp = new ArrayList<>();
                listTemp.addAll(listVanDeChoose);
                listVanDeChoose = new ArrayList<>();
                for (ComboboxResult itemChoose : listTemp) {
                    boolean isExit = false;
                    for (ComboboxResult itemUnChoose : listVanDeUnChooseTemp) {
                        if (itemChoose.id.equals(itemUnChoose.id)) {
                            isExit = true;
                            break;
                        }
                    }
                    if (!isExit) {
                        listVanDeChoose.add(itemChoose);
                    }
                }

                chooseListVanDeAdapter = new ChooseListAdapter(getActivity(), listVanDeChoose);
                rvVanDeChoose.setAdapter(chooseListVanDeAdapter);
                chooseListVanDeAdapter.SetOnItemClickListener(new ChooseListAdapter.OnItemClickListener() {
                    @Override
                    public void onItemClick(View view, int position, ComboboxResult comboboxResult) {
                        listVanDeChoose.remove(position);
                        chooseListVanDeAdapter.notifyDataSetChanged();
                    }
                });
                if (listVanDeChoose != null && listVanDeChoose.size() > 0) {
                    rvVanDeChoose.setVisibility(View.VISIBLE);
                } else {
                    rvVanDeChoose.setVisibility(View.GONE);
                }
                dialogChoosevanDe.hide();
            }
        });
    }
}
