package reportward.nts.reportward.fragment;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
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
import java.util.concurrent.Callable;

import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.AdapterListReport;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.LinkApi;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.ComboboxResult;
import reportward.nts.reportward.model.CaTuVanResultModel;
import reportward.nts.reportward.model.CaTuVanSearch;
import reportward.nts.reportward.model.LoginProfileModel;
import reportward.nts.reportward.model.ProcessingContentModel;
import reportward.nts.reportward.model.ResultModel;

public class ListReportFragment extends Fragment {
    private View view;
    private CaTuVanSearch caTuVanSearch;
    List<CaTuVanResultModel> listReport;
    private RecyclerView recyclerView;
    private AdapterListReport adapterListReport;
    boolean userScrolled = false;
    private Dialog dialogSearch;
    private EditText txtTuKhoa, spnGioiTinh, spnDanToc, txtTuNgay, txtDenNgay;
    private String[] arrayTinhTrangXL, arrayLoaiDoiTuong;
    private List<ComboboxResult> listTimhTrangXL, listLoaiDoiTuong;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_list_report, container, false);
        recyclerView = view.findViewById(R.id.list_report);
        recyclerView.setLayoutManager(new GridLayoutManager(getActivity(), 1));
        recyclerView.setHasFixedSize(true);

        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }

        caTuVanSearch = new CaTuVanSearch();
        caTuVanSearch.sortType = 1;
        caTuVanSearch.sortColumn = "Tinh";
        caTuVanSearch.pageNumber = 1;
        caTuVanSearch.pageSize = 10;
        caTuVanSearch.maTinh = loginProfileModel.tinh;

        listReport = new ArrayList<CaTuVanResultModel>();
        SearchReportProfile(caTuVanSearch);

        //gọi popup search
        setHasOptionsMenu(true);

        //Lấy dữ liệu combobox
        getDataCombobox(loginProfileModel.accessToken);

        //Khởi tạo dialog tìm kiếm
        initDialogSearch();

        return view;
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
                showDialogSearch();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    /***
     * Lấy dữ liệu cho combobox
     * @param authorizationKey
     */
    private void getDataCombobox(String authorizationKey) {
        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.dstrangthaihoso))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listTimhTrangXL = resultObject.data;

                        arrayTinhTrangXL = new String[listTimhTrangXL.size()];
                        int index = 0;
                        for (ComboboxResult item : listTimhTrangXL) {
                            arrayTinhTrangXL[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });

        AndroidNetworking.get(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.dsdoituongtre))
                .addHeaders("Authorization", authorizationKey)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        ResultModel<List<ComboboxResult>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultModel<List<ComboboxResult>>>() {
                        }.getType());
                        listLoaiDoiTuong = resultObject.data;

                        arrayLoaiDoiTuong = new String[listLoaiDoiTuong.size()];
                        int index = 0;
                        for (ComboboxResult item : listLoaiDoiTuong) {
                            arrayLoaiDoiTuong[index] = item.text;
                            index++;
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

    /***
     * Khởi tạo dialog tìm kiếm
     */
    private void initDialogSearch() {
        dialogSearch = new Dialog(getContext());
        dialogSearch.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialogSearch.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogSearch.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialogSearch.setCanceledOnTouchOutside(false);
        dialogSearch.setContentView(R.layout.popup_search);

        txtTuKhoa = dialogSearch.findViewById(R.id.txtTuKhoa);
        spnGioiTinh = dialogSearch.findViewById(R.id.spnGioiTinh);
        spnDanToc = dialogSearch.findViewById(R.id.spnDanToc);
        txtTuNgay = dialogSearch.findViewById(R.id.txtTuNgay);
        txtDenNgay = dialogSearch.findViewById(R.id.txtDenNgay);

        spnGioiTinh.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnGioiTinh, arrayLoaiDoiTuong, listLoaiDoiTuong, "Chọn giới tính", null);
            }
        });

        spnDanToc.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showChooseDialog(spnDanToc, arrayTinhTrangXL, listTimhTrangXL, "Chọn dân tộc", null);
            }
        });

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

        Button btnSearch = dialogSearch.findViewById(R.id.btnSearch);
        Button btnClose = dialogSearch.findViewById(R.id.btnClose);
        btnClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogSearch.dismiss();
            }
        });
        btnSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                caTuVanSearch.tuKhoa = txtTuKhoa.getText().toString();
                caTuVanSearch.gioiTinh = spnGioiTinh.getTag() != null ? spnGioiTinh.getTag().toString() : "";
                caTuVanSearch.danToc = spnDanToc.getTag() != null ? spnDanToc.getTag().toString() : "";
                caTuVanSearch.tuNgay = DateUtils.ConvertDMYToYMD(txtTuNgay.getText().toString());
                caTuVanSearch.denNgay = DateUtils.ConvertDMYToYMD(txtDenNgay.getText().toString());
                SearchReportProfile(caTuVanSearch);
                dialogSearch.dismiss();
            }
        });
    }

    /***
     * Show dialog tìm kiếm
     */
    private void showDialogSearch() {
        txtTuKhoa.setText(caTuVanSearch.tuKhoa);
        spnGioiTinh.setTag(caTuVanSearch.gioiTinh);
        spnDanToc.setTag(caTuVanSearch.danToc);
        txtTuNgay.setText(DateUtils.ConvertYMDServerToDMY(caTuVanSearch.tuNgay));
        txtDenNgay.setText(DateUtils.ConvertYMDServerToDMY(caTuVanSearch.denNgay));

        dialogSearch.show();
    }

    /***
     *
     * @param editText
     * @param arrayName
     * @param listSource
     * @param title
     */
    private void showChooseDialog(final EditText editText, final String[] arrayName, final List<ComboboxResult> listSource, final String title, final Callable<Void> voidCallable) {
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

                if (voidCallable != null) {
                    try {
                        voidCallable.call();
                    } catch (Exception ex) {
                    }
                }
            }
        });
        builder.show();
    }

    public void ShowDetailFragment(final String id, String Name, String Address) {
        TimelineFragment timelineFragment = new TimelineFragment();
        timelineFragment.Id = id;
        Bundle bundle = new Bundle();
        bundle.putString("Id", id);
        bundle.putString("Name", Name);
        bundle.putString("Address", Address);
        Utils.ChangeFragment(getActivity(), timelineFragment, bundle);
    }

    public void DialogReport(final String id) {
        final Dialog dialog = new Dialog(getContext());
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialog.setContentView(R.layout.popup_report);
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

    private void SearchReportProfile(CaTuVanSearch hoSoTreSearch) {
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(hoSoTreSearch));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }

        AndroidNetworking.post(Utils.getUrlAPIByArea(loginProfileModel.area,LinkApi.dsCaTuVan))
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
                            listReport = resultObject.data;

                            adapterListReport = new AdapterListReport(getContext(), listReport);
                            recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
                            recyclerView.setAdapter(adapterListReport);

                            adapterListReport.SetOnItemClickListener(new AdapterListReport.OnItemClickListener() {
                                @Override
                                public void onButtonDetailClick(View view, int position, CaTuVanResultModel obj) {
                                    //ShowDetailFragment(obj.Id, obj.Name, obj.FullAddress);
                                }

                                @Override
                                public void onButtonReportClick(View view, final int position, CaTuVanResultModel obj) {
                                    //DialogReport(obj.Id);
                                }
                            });

                        } catch (Exception ex) {
                            String tam = "";
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                    }
                });
    }

//    private void implementScrollListener() {
//        recyclerView.setOnScrollListener(new AbsListView.OnScrollListener() {
//
//            @Override
//            public void onScrollStateChanged(AbsListView arg0, int scrollState) {
//                // If scroll state is touch scroll then set userScrolled
//                // true
//                if (scrollState == AbsListView.OnScrollListener.SCROLL_STATE_TOUCH_SCROLL) {
//                    userScrolled = true;
//                }
//            }
//
//            @Override
//            public void onScroll(AbsListView view, int firstVisibleItem,
//                                 int visibleItemCount, int totalItemCount) {
//                // Now check if userScrolled is true and also check ifté
//                // the item is end then update list view and set
//                // userScrolled to false
//                if (userScrolled) {
//                    // thực hiện load thêm dữ liệu mới khi người dùng đang scroll ở dòng cuối cùng của listview
//                    if (firstVisibleItem + visibleItemCount == totalItemCount) {
//                        updateListView();
//                        userScrolled = false;
//
//                        // Trường hợp khi người dùng load tới dòng trên cùng, thì lấy lại dữ liệu mới
//                    } else if (firstVisibleItem == 0 && totalItemCount > 15) {
//                        populateListView();
//
//                        userScrolled = false;
//                    }
//                }
//            }
//        });
//    }

    private void updateListView() {
        new Handler().postDelayed(new Runnable() {

            @Override
            public void run() {
                caTuVanSearch.pageNumber++;
                // SearchChildProfile(hoSoTreSearch);
                // After adding new data hide the view.
            }
        }, 100);
    }

    private void populateListView() {
        this.caTuVanSearch.pageNumber = 1;
        // SearchChildProfile(this.hoSoTreSearch);
    }
}
