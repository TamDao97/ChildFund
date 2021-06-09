package reportward.nts.reportward.fragment;

import android.app.AlertDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.RelativeLayout;
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
import reportward.nts.reportward.adapter.AdapterListSearchReport;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.CaTuVanUpdateModel;
import reportward.nts.reportward.model.ComboboxResult;
import reportward.nts.reportward.model.CaTuVanResultModel;
import reportward.nts.reportward.model.CaTuVanSearch;
import reportward.nts.reportward.model.HoSoChiTietUpdateModel;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ResultModel;
import reportward.nts.reportward.model.UpdateReportMobileModel;

public class ListSearchReportFragment extends Fragment {
    private View view;
    private ProgressDialog progressDialog;
    private ListView listView;
    private AdapterListSearchReport profileListAdapter;
    private TextView item_total, price_total;
    private LinearLayout lyt_notfound;
    private List<CaTuVanResultModel> listProfile = new ArrayList<CaTuVanResultModel>();
    private CaTuVanSearch caTuVanSearch;
    private String[] arrayDanToc, arrayGioiTinh, arrayTinhThanh, arrayQuanHuyen, arrayXa, arrayVanDe;
    private List<ComboboxResult> listDanToc, listGioiTinh, listTinhThanh, listQuanHuyen, listXa, listVanDe;
    Dialog dialog;
    //private LoginProfileModel loginProfileModel;
    //private ProfileListAdapter adapter = null;
    public int currentPage;
    boolean userScrolled = false;

    private static RelativeLayout bottomLayout;

    private EditText spnDantoc, spnGioiTinh, txtTuNgay, txtDenNgay, spnTinhThanh, spnQuanHuyen, spnXa, spnVanDe, txtTuKhoa;

    private HoSoChiTietUpdateModel hoSoChiTietUpdateModel;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_search_report, null);
        listView = (ListView) view.findViewById(R.id.listView);
        progressDialog = new ProgressDialog(getActivity());
        bottomLayout = (RelativeLayout) view
                .findViewById(R.id.loadItemsLayout_listView);

        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }

        caTuVanSearch = new CaTuVanSearch();
        caTuVanSearch.sortType = 0;
        caTuVanSearch.sortColumn = "Tinh";
        caTuVanSearch.pageNumber = 1;
        caTuVanSearch.pageSize = 200;
        caTuVanSearch.maTinh = loginProfileModel.tinh;
        caTuVanSearch.maHuyen = loginProfileModel.quanHuyen;
        caTuVanSearch.maPhuong = loginProfileModel.xaPhuong;

        lyt_notfound = view.findViewById(R.id.lyt_notfound);

        getDataCombobox(loginProfileModel.accessToken);
        listProfile = new ArrayList<CaTuVanResultModel>();
        profileListAdapter = new AdapterListSearchReport(getActivity(), listProfile);
        profileListAdapter.SetOnItemClickListener(new AdapterListSearchReport.OnItemClickListener() {
            @Override
            public void onItemClick(View view, int position, CaTuVanResultModel obj) {
                DialogReport(obj.soHoSo);
            }
        });
        profileListAdapter.notifyDataSetChanged();
        listView.setAdapter(profileListAdapter);

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                try {
                    FagmentDetailCatuvan fragment = new FagmentDetailCatuvan();
                    fragment.soHoSo = listProfile.get(position).soHoSo;
                    Utils.ChangeFragment(getActivity(), fragment, null);
                } catch (Exception ex) {
                }
            }
        });

        populateListView();
        implementScrollListener();
        //gọi popup search
        setHasOptionsMenu(true);
        return view;
    }

    public void DialogReport(final String id) {
        hoSoChiTietUpdateModel = new HoSoChiTietUpdateModel();
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.hosochitiet + "?maSoHoSo=" + id))
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultModel<HoSoChiTietUpdateModel> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<HoSoChiTietUpdateModel>>() {
                            }.getType());
                            hoSoChiTietUpdateModel = resultObject.data;

                        } catch (Exception ex) {
                            String temp = ex.getMessage();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        bottomLayout.setVisibility(View.GONE);
                    }
                });

        final Dialog dialog = new Dialog(getContext());
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialog.setContentView(R.layout.popup_report);
        final Button btnAdd = dialog.findViewById(R.id.btnAdd);
        final TextView txtContent = dialog.findViewById(R.id.txtContent);
        final TextInputLayout tilContent = dialog.findViewById(R.id.tilContent);
        btnAdd.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Check validate
                if (Utils.isEmpty(txtContent.getText().toString())) {
                    tilContent.setError("Không được để trống.");
                    txtContent.findFocus();
                    return;
                } else {
                    tilContent.setErrorEnabled(false);
                }

                if (Utils.isEmpty(hoSoChiTietUpdateModel.maSoHoSo)) {
                    Toast.makeText(getActivity(), "Hãy chọn trẻ trước khi báo cáo!", Toast.LENGTH_SHORT);
                    return;
                }

                UpdateReportMobileModel updateReportMobileModel = new UpdateReportMobileModel();
                updateReportMobileModel.loaiCa = hoSoChiTietUpdateModel.loaiCa;
                updateReportMobileModel.maTinh = hoSoChiTietUpdateModel.maTinh;
                updateReportMobileModel.maSoHoSo = hoSoChiTietUpdateModel.maSoHoSo;
                updateReportMobileModel.ngayTaoHoSo = hoSoChiTietUpdateModel.ngayTaoHoSo;
                updateReportMobileModel.nguoiTaoHoSo = loginProfileModel.tenDangNhap;
                updateReportMobileModel.ngayTuVanCuoiCung = DateUtils.CurrentDate("yyyy-MM-dd HH:mm");
                updateReportMobileModel.maDoiTuong = hoSoChiTietUpdateModel.maDoiTuong;
                updateReportMobileModel.hoTen = hoSoChiTietUpdateModel.hoTen;
                updateReportMobileModel.namSinh = hoSoChiTietUpdateModel.namSinh;
                updateReportMobileModel.gioiTinh = hoSoChiTietUpdateModel.gioiTinh;
                updateReportMobileModel.maHoanCanh = hoSoChiTietUpdateModel.maHoanCanh;
                updateReportMobileModel.maTrinhDoVanHoa = hoSoChiTietUpdateModel.maTrinhDoVanHoa;
                updateReportMobileModel.maDanToc = hoSoChiTietUpdateModel.maDanToc;
                updateReportMobileModel.maTinhNguoiGoi = hoSoChiTietUpdateModel.maTinhNguoiGoi;
                updateReportMobileModel.maHuyenNguoiGoi = hoSoChiTietUpdateModel.maHuyenNguoiGoi;
                updateReportMobileModel.maXaNguoiGoi = hoSoChiTietUpdateModel.maXaNguoiGoi;
                updateReportMobileModel.ngaySuaCuoi = DateUtils.CurrentDate("yyyy-MM-dd HH:mm");
                updateReportMobileModel.nguoiSuaCuoi = loginProfileModel.tenDangNhap;
                updateReportMobileModel.gioiTinhTre = hoSoChiTietUpdateModel.gioiTinhTre;
                updateReportMobileModel.tuoiCuaTre = hoSoChiTietUpdateModel.tuoiCuaTre;
                updateReportMobileModel.maDanTocCuaTre = hoSoChiTietUpdateModel.maDanTocCuaTre;
                updateReportMobileModel.maTrinhDoVanHoaCuaTre = hoSoChiTietUpdateModel.maTrinhDoVanHoaCuaTre;
                updateReportMobileModel.soDienThoai = hoSoChiTietUpdateModel.soDienThoai;
                updateReportMobileModel.cacVanDe = hoSoChiTietUpdateModel.cacVanDe;
                updateReportMobileModel.cacVanDeKhac = hoSoChiTietUpdateModel.cacVanDeKhac;
                updateReportMobileModel.phanLoaiHoSo = hoSoChiTietUpdateModel.phanLoaiHoSo;
                updateReportMobileModel.moiTruongXamHai = hoSoChiTietUpdateModel.moiTruongXamHai;
                updateReportMobileModel.trangThaiHoSo = hoSoChiTietUpdateModel.trangThaiHoSo;
                updateReportMobileModel.ketQuaHoTroCanThiep = hoSoChiTietUpdateModel.ketQuaHoTroCanThiep;
                updateReportMobileModel.loaiHoSo = hoSoChiTietUpdateModel.loaiHoSo;
                updateReportMobileModel.nguonThongTin = hoSoChiTietUpdateModel.nguonThongTin;
                updateReportMobileModel.lienQuanInternet = hoSoChiTietUpdateModel.lienQuanInternet;
                updateReportMobileModel.suDung = hoSoChiTietUpdateModel.suDung;
                ComboboxResult objectInit = new ComboboxResult();
                objectInit.id = hoSoChiTietUpdateModel.maDoiTuong;
                updateReportMobileModel.doiTuongInit = objectInit;
                objectInit = new ComboboxResult();
                objectInit.id = hoSoChiTietUpdateModel.loaiHoSo;
                updateReportMobileModel.loaiHoSoInit = objectInit;
                objectInit = new ComboboxResult();
                objectInit.id = hoSoChiTietUpdateModel.trangThaiHoSo;
                updateReportMobileModel.trangThaiHoSoInit = objectInit;
                objectInit = new ComboboxResult();
                objectInit.id = hoSoChiTietUpdateModel.maDanToc;
                updateReportMobileModel.danTocInit = objectInit;
                objectInit = new ComboboxResult();
                objectInit.id = hoSoChiTietUpdateModel.phanLoaiHoSo;
                updateReportMobileModel.phanLoaiHoSoInit = objectInit;
                objectInit = new ComboboxResult();
                objectInit.id = hoSoChiTietUpdateModel.moiTruongXamHai;
                updateReportMobileModel.moiTruongXamHaiInit = objectInit;
                objectInit = new ComboboxResult();
                objectInit.id = hoSoChiTietUpdateModel.nguonThongTin;
                updateReportMobileModel.nguonThongTinInit = objectInit;
                updateReportMobileModel.treCoDiHoc = "";
                updateReportMobileModel.tuVanKhac = new ArrayList<String>();
                updateReportMobileModel.dsKetQuaString = new ArrayList<String>();
                updateReportMobileModel.dsVanDeString = new ArrayList<String>();
                updateReportMobileModel.nguoiDung = loginProfileModel.tenDangNhap;

                updateReportMobileModel.caTuVan = new CaTuVanUpdateModel();
                objectInit = new ComboboxResult();
                objectInit.id = "1";
                updateReportMobileModel.caTuVan.hinhThucLienHeInit = objectInit;
                updateReportMobileModel.caTuVan.email = "";
                updateReportMobileModel.caTuVan.soDienThoai = "";
                updateReportMobileModel.caTuVan.mangXaHoi = "";
                updateReportMobileModel.caTuVan.noiDungTuVan = txtContent.getText().toString();
                updateReportMobileModel.caTuVan.ngayTuVan = DateUtils.CurrentDate("yyyy-MM-dd HH:mm");
                updateReportMobileModel.caTuVan.nguoiTuVanName = "";
                updateReportMobileModel.caTuVan.hinhThucLienHe = "1";
                updateReportMobileModel.caTuVan.listFiles = new ArrayList<String>();

                JSONObject jsonModel = new JSONObject();
                try {
                    jsonModel = new JSONObject(new Gson().toJson(updateReportMobileModel));
                } catch (JSONException e) {
                }

                AndroidNetworking.put(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.hosoUpdate + "/" + id))
                        .addHeaders("Authorization", loginProfileModel.accessToken)
                        .addJSONObjectBody(jsonModel)
                        .setPriority(Priority.MEDIUM)
                        .build()
                        .getAsString(new StringRequestListener() {
                            @Override
                            public void onResponse(String response) {
                                //progressDialog.dismiss();
                                dialog.dismiss();
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

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        inflater.inflate(R.menu.menu_search, menu);
        super.onCreateOptionsMenu(menu, inflater);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.search:
                DialogSearch();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    private void showChooseDialog(final EditText editText, final String[] arrayName, final List<ComboboxResult> listSource, final String title, final Runnable funtion) {
        final AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
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

    private void getDataCombobox(String authorizationKey) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsDanToc))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listDanToc = resultObject.data;
                        ComboboxResult comboboxResult = new ComboboxResult();
                        comboboxResult.id = "";
                        comboboxResult.text = "Tất cả";
                        listDanToc.add(0, comboboxResult);

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

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsGioiTinh))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listGioiTinh = resultObject.data;
                        ComboboxResult comboboxResult = new ComboboxResult();
                        comboboxResult.id = "";
                        comboboxResult.text = "Tất cả";
                        listGioiTinh.add(0, comboboxResult);
                        arrayGioiTinh = new String[listGioiTinh.size()];
                        int index = 0;
                        for (ComboboxResult item : listGioiTinh) {
                            arrayGioiTinh[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dstinhtp))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listTinhThanh = resultObject.data;

                        arrayTinhThanh = new String[listTinhThanh.size()];
                        int index = 0;
                        for (ComboboxResult item : listTinhThanh) {
                            arrayTinhThanh[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsVanDe))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listVanDe = resultObject.data;
                        ComboboxResult comboboxResult = new ComboboxResult();
                        comboboxResult.id = "";
                        comboboxResult.text = "Tất cả";
                        listVanDe.add(0, comboboxResult);

                        arrayVanDe = new String[listVanDe.size()];
                        int index = 0;
                        for (ComboboxResult item : listVanDe) {
                            arrayVanDe[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });

        getDataDistrict(authorizationKey, loginProfileModel.tinh);
        getDataWrap(authorizationKey, loginProfileModel.quanHuyen);
    }

    Runnable runnableDataDistrict = new Runnable() {
        @Override
        public void run() {
            getDataDistrict(loginProfileModel.accessToken, spnTinhThanh.getTag().toString());
            spnQuanHuyen.setTag("");
            spnQuanHuyen.setText("");
            spnXa.setTag("");
            spnXa.setText("");
            //txtAddress.setText("");
        }
    };

    Runnable runnableDataWrap = new Runnable() {
        @Override
        public void run() {
            getDataWrap(loginProfileModel.accessToken, spnQuanHuyen.getTag().toString());
            spnXa.setTag("");
            spnXa.setText("");
            //txtAddress.setText("");
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
                        listQuanHuyen = resultObject.data;

                        arrayQuanHuyen = new String[listQuanHuyen.size()];
                        int index = 0;
                        for (ComboboxResult item : listQuanHuyen) {
                            arrayQuanHuyen[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

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
                            arrayXa[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

    private void DialogSearch() {
        final Dialog dialog = new Dialog(getContext());
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialog.setCanceledOnTouchOutside(false);
        dialog.setContentView(R.layout.popup_search);
        txtTuKhoa = dialog.findViewById(R.id.txtTuKhoa);
        txtTuKhoa.setText(caTuVanSearch.tuKhoa);

        //dân tộc
        spnDantoc = dialog.findViewById(R.id.spnDanToc);
        spnDantoc.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnDantoc, arrayDanToc, listDanToc, "Chọn dân tộc", null);
            }
        });
        spnDantoc.setTag(caTuVanSearch.danToc);
        spnDantoc.setText(Utils.getNameById(listDanToc, caTuVanSearch.danToc));


        //giới tính
        spnGioiTinh = dialog.findViewById(R.id.spnGioiTinh);
        spnGioiTinh.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showChooseDialog(spnGioiTinh, arrayGioiTinh, listGioiTinh, "Chọn giới tính", null);
            }
        });
        spnGioiTinh.setTag(caTuVanSearch.gioiTinh);
        spnGioiTinh.setText(Utils.getNameById(listGioiTinh, caTuVanSearch.gioiTinh));


        //Tỉnh thành phố
        spnTinhThanh = dialog.findViewById(R.id.spnTinh);
        String tinh = null;
        if (loginProfileModel.tinh != null) {
            spnTinhThanh.setEnabled(false);
            if (listTinhThanh != null && listTinhThanh.size() > 0) {
                for (int i = 0; i < listTinhThanh.size(); i++) {
                    if (loginProfileModel.tinh.equals(listTinhThanh.get(i).id)) {
                        tinh = listTinhThanh.get(i).text;
                    }
                }
                spnTinhThanh.setText(tinh);
                spnTinhThanh.setTag(loginProfileModel.tinh);
            }
        } else {
            spnTinhThanh.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    showChooseDialog(spnTinhThanh, arrayTinhThanh, listTinhThanh, "Chọn Tỉnh/Thành phố", runnableDataDistrict);
                }
            });
        }


        //Quận huyện
        String huyen = null;
        spnQuanHuyen = dialog.findViewById(R.id.spnQuanHuyen);
        if (loginProfileModel.quanHuyen != null) {
            spnQuanHuyen.setEnabled(false);
            if (listQuanHuyen != null && listQuanHuyen.size() > 0) {
                for (int i = 0; i < listQuanHuyen.size(); i++) {
                    if (loginProfileModel.quanHuyen.equals(listQuanHuyen.get(i).id)) {
                        huyen = listQuanHuyen.get(i).text;
                    }
                }
                spnQuanHuyen.setText(huyen);
                spnQuanHuyen.setTag(loginProfileModel.quanHuyen);
            }
        } else {
            spnQuanHuyen.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    showChooseDialog(spnQuanHuyen, arrayQuanHuyen, listQuanHuyen, "Chọn Quận/Huyện", runnableDataWrap);
                }
            });
        }


        //Xã phường
        spnXa = dialog.findViewById(R.id.spnXaPhuong);
        String xa = null;
        if (loginProfileModel.xaPhuong != null) {
            spnXa.setEnabled(false);
            if(listXa!=null && listXa.size()>0) {
                for (int i = 0; i < listXa.size(); i++) {
                    if (loginProfileModel.xaPhuong.equals(listXa.get(i).id)) {
                        xa = listXa.get(i).text;
                    }
                }
                spnXa.setText(xa);
                spnXa.setTag(loginProfileModel.xaPhuong);
            }
        } else {
            spnXa.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    showChooseDialog(spnXa, arrayXa, listXa, "Chọn Xã/Phường", null);
                }
            });
        }


        //Danh sách vấn đề
        spnVanDe = dialog.findViewById(R.id.spnVanDe);
        spnVanDe.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showChooseDialog(spnVanDe, arrayVanDe, listVanDe, "Các vấn đề", null);
            }
        });
        spnVanDe.setTag(caTuVanSearch.cacVanDe);
        spnVanDe.setText(Utils.getNameById(listVanDe, caTuVanSearch.cacVanDe));

        //khoảng thời gian
        txtTuNgay = dialog.findViewById(R.id.txtTuNgay);
        txtTuNgay.setText(DateUtils.ConvertYMDServerToDMY(caTuVanSearch.tuNgay));
        txtDenNgay = dialog.findViewById(R.id.txtDenNgay);
        txtDenNgay.setText(DateUtils.ConvertYMDServerToDMY(caTuVanSearch.denNgay));
        txtTuNgay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });

        txtDenNgay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getActivity(), (EditText) v);
            }
        });


        Button btnSearch = dialog.findViewById(R.id.btnSearch);
        Button btnClose = dialog.findViewById(R.id.btnClose);
        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialog.dismiss();
            }
        });

        btnSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                caTuVanSearch.maTinh = spnTinhThanh.getTag() != null ? spnTinhThanh.getTag().toString() : "";
                caTuVanSearch.maHuyen = spnQuanHuyen.getTag() != null ? spnQuanHuyen.getTag().toString() : "";
                caTuVanSearch.maPhuong = spnXa.getTag() != null ? spnXa.getTag().toString() : "";
                caTuVanSearch.cacVanDe = spnVanDe.getTag() != null ? spnVanDe.getTag().toString() : "";
                caTuVanSearch.cacVanDe = spnVanDe.getTag() != null ? spnVanDe.getTag().toString() : "";
                caTuVanSearch.gioiTinh = spnGioiTinh.getTag() != null ? spnGioiTinh.getTag().toString() : "";
                caTuVanSearch.danToc = spnDantoc.getTag() != null ? spnDantoc.getTag().toString() : "";
                caTuVanSearch.tuNgay = DateUtils.ConvertDMYToYMD(txtTuNgay.getText().toString());
                caTuVanSearch.denNgay = DateUtils.ConvertDMYToYMD(txtDenNgay.getText().toString());
                caTuVanSearch.tuKhoa = txtTuKhoa.getText().toString();
                populateListView();
                dialog.dismiss();
            }
        });
        dialog.show();
    }


    private void populateListView() {
        this.caTuVanSearch.pageNumber = 1;
        SearchChildProfile(caTuVanSearch);
    }

    private void implementScrollListener() {
        listView.setOnScrollListener(new AbsListView.OnScrollListener() {

            @Override
            public void onScrollStateChanged(AbsListView arg0, int scrollState) {
                // If scroll state is touch scroll then set userScrolled
                // true
                if (scrollState == AbsListView.OnScrollListener.SCROLL_STATE_TOUCH_SCROLL) {
                    userScrolled = true;
                }
            }

            @Override
            public void onScroll(AbsListView view, int firstVisibleItem,
                                 int visibleItemCount, int totalItemCount) {
                // Now check if userScrolled is true and also check ifté
                // the item is end then update list view and set
                // userScrolled to false
                if (userScrolled) {
                    // thực hiện load thêm dữ liệu mới khi người dùng đang scroll ở dòng cuối cùng của listview
                    if (firstVisibleItem + visibleItemCount == totalItemCount) {
                        updateListView();
                        userScrolled = false;

                        // Trường hợp khi người dùng load tới dòng trên cùng, thì lấy lại dữ liệu mới
                    } else if (firstVisibleItem == 0 && totalItemCount > 15) {
                        populateListView();

                        userScrolled = false;
                    }
                }
            }
        });
    }

    private void updateListView() {
        new Handler().postDelayed(new Runnable() {

            @Override
            public void run() {
                caTuVanSearch.pageNumber++;
                SearchChildProfile(caTuVanSearch);
                // After adding new data hide the view.
            }
        }, 100);
    }

    private void SearchChildProfile(final CaTuVanSearch hoSoTreModel) {

        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(hoSoTreModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        // Show Progress Layout
        bottomLayout.setVisibility(View.VISIBLE);
        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area, LinkApi.dsCaTuVan))
                .addHeaders("Authorization", loginProfileModel.accessToken)
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultModel<List<CaTuVanResultModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<CaTuVanResultModel>>>() {
                            }.getType());
                            //item_total.setText(String.valueOf(resultObject.TotalItem));
                            //set data and list adapter

                            if (resultObject.totalrecord >= 0) {
                                if (hoSoTreModel.pageNumber == 1) {
                                    listProfile.clear();
                                }

                                listProfile.addAll(resultObject.data);
//                                profileListAdapter = new AdapterListSearchReport(getActivity(), listProfile);
                                profileListAdapter.notifyDataSetChanged();
//                                listView.setAdapter(profileListAdapter);
                            }

                            if (listProfile.size() > 0) {
                                lyt_notfound.setVisibility(View.GONE);
                            } else {
                                lyt_notfound.setVisibility(View.VISIBLE);
                            }

                        } catch (Exception ex) {
                            lyt_notfound.setVisibility(View.VISIBLE);
                        }
                        bottomLayout.setVisibility(View.GONE);
                    }

                    @Override
                    public void onError(ANError anError) {
                        lyt_notfound.setVisibility(View.VISIBLE);
                        bottomLayout.setVisibility(View.GONE);
                    }
                });
    }
}
