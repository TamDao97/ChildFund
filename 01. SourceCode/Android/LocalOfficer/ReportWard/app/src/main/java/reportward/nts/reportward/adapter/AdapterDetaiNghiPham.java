package reportward.nts.reportward.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.model.DoiTuongXamHaiViewModel;
import reportward.nts.reportward.model.ThongTinTreViewModel;

public class AdapterDetaiNghiPham extends RecyclerView.Adapter<AdapterDetaiNghiPham.ViewHolder> {
    private final int mBackground;
    private Context ctx;
    private List<DoiTuongXamHaiViewModel> listObject = new ArrayList<>();
    private final TypedValue mTypedValue = new TypedValue();

    public AdapterDetaiNghiPham(Context ctx, List<DoiTuongXamHaiViewModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<DoiTuongXamHaiViewModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_nghipham, parent, false);
        v.setBackgroundResource(mBackground);
        AdapterDetaiNghiPham.ViewHolder vh = new AdapterDetaiNghiPham.ViewHolder(v);
        return vh;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        public TextView txtTenNghiPham;
        public TextView txtGioiTinhNghiPham;
        public TextView txtQuanHeVoiTre;
        public TextView txtLoaiXamHai;

        public ViewHolder(View v) {
            super(v);
            txtTenNghiPham = v.findViewById(R.id.txtTenNghiPham);
            txtGioiTinhNghiPham = v.findViewById(R.id.txtGioiTinhNghiPham);
            txtQuanHeVoiTre = v.findViewById(R.id.txtQuanHeVoiTre);
            txtLoaiXamHai = v.findViewById(R.id.txtLoaiXamHai);
        }
    }

    @Override
    public void onBindViewHolder(AdapterDetaiNghiPham.ViewHolder holder, int position) {
        final DoiTuongXamHaiViewModel doiTuongXamHaiViewModel = listObject.get(position);
        holder.txtTenNghiPham.setText(doiTuongXamHaiViewModel.hoTen);
        holder.txtGioiTinhNghiPham.setText(doiTuongXamHaiViewModel.gioiTinh);
        holder.txtQuanHeVoiTre.setText(doiTuongXamHaiViewModel.quanHeVoiTre);
        holder.txtLoaiXamHai.setText(doiTuongXamHaiViewModel.loaiDoiTuongXamHai);
    }

    @Override
    public int getItemCount() {
        return listObject.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
