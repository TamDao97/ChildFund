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
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.model.ThongTinTreViewModel;

public class AdapterDetailCatuvan extends RecyclerView.Adapter<AdapterDetailCatuvan.ViewHolder> {
    private final int mBackground;
    private Context ctx;
    private List<ThongTinTreViewModel> listObject = new ArrayList<>();
    private final TypedValue mTypedValue = new TypedValue();

    public AdapterDetailCatuvan(Context ctx, List<ThongTinTreViewModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<ThongTinTreViewModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_thongtintre, parent, false);
        v.setBackgroundResource(mBackground);
        AdapterDetailCatuvan.ViewHolder vh = new AdapterDetailCatuvan.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, int position) {
        final ThongTinTreViewModel thongTinTreViewModel = listObject.get(position);
        holder.txtTenTre.setText(thongTinTreViewModel.hoTen);
        holder.txtGioiTinhTre.setText(thongTinTreViewModel.gioiTinh);
        holder.txtDanToc.setText(thongTinTreViewModel.danToc);
        holder.txtNgaySinh.setText(DateUtils.ConvertYMDServerToDMY(thongTinTreViewModel.namSinh));
        holder.txtDiHoc.setText(thongTinTreViewModel.treCoDiHoc);
        holder.txtDoiTuongTre.setText(thongTinTreViewModel.doiTuongTre);
        holder.txtDiaChiTre.setText(thongTinTreViewModel.diaChi);
    }

    @Override
    public int getItemCount() {
        return listObject.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        public TextView txtTenTre;
        public TextView txtGioiTinhTre;
        public TextView txtDanToc;
        public TextView txtNgaySinh;
        public TextView txtDiHoc;
        public TextView txtDoiTuongTre;
        public TextView txtDiaChiTre;

        public ViewHolder(View v) {
            super(v);
            txtTenTre = v.findViewById(R.id.txtTenTre);
            txtGioiTinhTre = v.findViewById(R.id.txtGioiTinhTre);
            txtDanToc = v.findViewById(R.id.txtDanToc);
            txtNgaySinh = v.findViewById(R.id.txtNgaySinh);
            txtDiHoc = v.findViewById(R.id.txtDiHoc);
            txtDoiTuongTre = v.findViewById(R.id.txtDoiTuongTre);
            txtDiaChiTre = v.findViewById(R.id.txtDiaChiTre);
        }
    }
}
