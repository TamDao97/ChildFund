package reportward.nts.reportward.adapter;

import android.content.Context;
import android.graphics.Color;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.model.BaoCaoThuPhamModel;

public class BaoCaoThuPhamGioiTinhAdapter extends RecyclerView.Adapter<BaoCaoThuPhamGioiTinhAdapter.ViewHolder> {
    private final int mBackground;
    private Context ctx;
    private final TypedValue mTypedValue = new TypedValue();
    private List<BaoCaoThuPhamModel> listObject = new ArrayList<>();
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void showDialogDetail(View view, int position, BaoCaoThuPhamModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public BaoCaoThuPhamGioiTinhAdapter(Context ctx, List<BaoCaoThuPhamModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<BaoCaoThuPhamModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }


    @Override
    public BaoCaoThuPhamGioiTinhAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_bc_thu_pham_gioi_tinh, parent, false);
        v.setBackgroundResource(mBackground);
        BaoCaoThuPhamGioiTinhAdapter.ViewHolder vh = new BaoCaoThuPhamGioiTinhAdapter.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(BaoCaoThuPhamGioiTinhAdapter.ViewHolder holder, final int position) {
        final BaoCaoThuPhamModel baoCaoThuPhamModel = listObject.get(position);
        if (position % 2 == 0) {
            holder.itemRow.setBackgroundColor(Color.parseColor("#D1DCFF"));
        } else {
            holder.itemRow.setBackgroundColor(Color.parseColor("#FFFFFF"));
        }
        holder.txtQuanHeVoiTre.setText(baoCaoThuPhamModel.quanHeVoiTre);
        holder.txtNam.setText(baoCaoThuPhamModel.gioiTinhNam);
        holder.txtNu.setText(baoCaoThuPhamModel.gioiTinhNu);
        holder.txtKhac.setText(baoCaoThuPhamModel.gioiTinhKhongXacDinh);
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
        public TextView txtQuanHeVoiTre;
        public TextView txtNam, txtNu, txtKhac;
        LinearLayout itemRow;


        public ViewHolder(View v) {
            super(v);
            txtQuanHeVoiTre = v.findViewById(R.id.txtQuanHeVoiTre);
            txtNam = v.findViewById(R.id.txtNam);
            txtNu = v.findViewById(R.id.txtNu);
            txtKhac = v.findViewById(R.id.txtKhac);
            itemRow = v.findViewById(R.id.itemRow);
        }
    }
}
